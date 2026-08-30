using Application.InterfacesService.InterfaceTool;
using Core.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.ServiceTool
{
    public class EngineEnvService : IEgineEnv
    {
        private const int SaltSize = 16; //128 bits para a senha
        private const int NonceSize = 12; // 96 bits padrão AES-GCM
        private const int TagSize = 16;   // 128 bits de autenticação

        public async Task<string> GetEnv(string file)
        {
            if (string.IsNullOrEmpty(file)) {
                AnsiConsole.MarkupLine("[red]Is necessary to put the archive.[/]");
                    return string.Empty;
            }
            try
            {
                await using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read,
                    FileShare.Read, bufferSize: 4096, useAsync: true);
                using var fileReader = new StreamReader(fileStream);
                return await fileReader.ReadToEndAsync();
            }
            catch(Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error for read .env.[/]\n{ex.Message}");
                throw;
            }
        }

        public byte[] NewPassword(string password, byte[] salt)
        {
            // Deriva uma chave de 32 bytes a partir da senha fornecida
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations: 100_000,
                hashAlgorithm: HashAlgorithmName.SHA256);

            return pbkdf2.GetBytes(32);
        }
        public string Decrypt(byte[] encryptedPayload, string password)
        {
            int headerSize = SaltSize + NonceSize + TagSize;
            if (encryptedPayload == null || encryptedPayload.Length < headerSize)
                throw new CryptographicException("Payload do vault corrompido ou incompleto.");

            ReadOnlySpan<byte> payloadSpan = encryptedPayload.AsSpan();
            ReadOnlySpan<byte> salt = payloadSpan.Slice(0, SaltSize);
            ReadOnlySpan<byte> nonce = payloadSpan.Slice(SaltSize, NonceSize);
            ReadOnlySpan<byte> tag = payloadSpan.Slice(SaltSize + NonceSize, TagSize);
            ReadOnlySpan<byte> cipherText = payloadSpan.Slice(headerSize);

            byte[] key = NewPassword(password, salt.ToArray());

            byte[] decryptedBytes = new byte[cipherText.Length];

            using (var aesGcm = new AesGcm(key, TagSize))
            {
                // Se a senha estiver errada ou os dados forem alterados, lança CryptographicException
                aesGcm.Decrypt(nonce, cipherText, tag, decryptedBytes);
            }

            return Encoding.UTF8.GetString(decryptedBytes);


        }

        public byte[] Encrypt(string plaintext, string password)
        {
            byte[] salt = new byte[SaltSize];
            byte[] nonce = new byte[NonceSize];
            RandomNumberGenerator.Fill(salt);
            RandomNumberGenerator.Fill(nonce);
            byte[] key = NewPassword(password, salt);
            if (key == null || key.Length != 32)
                throw new ArgumentException("A chave mestra deve conter exatamente 32 bytes (256 bits).", nameof(key));
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] cipherText = new byte[plaintextBytes.Length];
            byte[] tag = new byte[TagSize];


            using (var aesGcm = new AesGcm(key, TagSize))
            {
                aesGcm.Encrypt(nonce, plaintextBytes, cipherText, tag);
            }

            byte[] finalPayload = new byte[SaltSize + NonceSize + TagSize + cipherText.Length];

            var spanPayload = finalPayload.AsSpan();
            salt.CopyTo(spanPayload.Slice(0, SaltSize));
            nonce.CopyTo(spanPayload.Slice(SaltSize, NonceSize));
            tag.CopyTo(spanPayload.Slice(SaltSize + NonceSize, TagSize));
            cipherText.CopyTo(spanPayload.Slice(SaltSize+ NonceSize + TagSize));

            return finalPayload;
        }
    }
}
