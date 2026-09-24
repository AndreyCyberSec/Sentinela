using Application.InterfacesService.InterfaceTool;
using Core.Models;
using DocumentFormat.OpenXml.Vml.Office;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.ServiceTool
{
    public class CheckCAService : ICheckCA
    {
        public Task<CheckCAEntity> GetHostAsync(string hostName, int port)
        {
            var checkHost = new CheckCAEntity { HostName = hostName, Port = port };
            return Task.FromResult(checkHost);
        }

        public async Task<CheckCAEntity> CheckSslAsync(string hostName, int port = 443, CancellationToken cancellationToken = default)
        {
            var host = new CheckCAEntity { HostName = hostName, Port = port };
            try
            {
                using var client = new TcpClient();
                await client.ConnectAsync(hostName, port, cancellationToken);

                using var sslStream = new SslStream(
                    client.GetStream(),
                    false,
                    (sender, certificate, chain, sslPolicyErrors) =>
                    {
                        host.IsValid = sslPolicyErrors == SslPolicyErrors.None;
                        return true;
                    },
                    null
                    );
                await sslStream.AuthenticateAsClientAsync(hostName);

                if (sslStream.RemoteCertificate is X509Certificate2 cert)
                {
                    host.Issuer = cert.IssuerName.Format(false);
                    host.Subject = cert.SubjectName.Format(false);
                    host.EffectiveDate = cert.NotBefore;
                    host.ExpirationDate = cert.NotAfter;
                    host.DaysRemaining = (int)(cert.NotAfter - DateTime.Now).TotalDays;
                    host.SignatureAlgorithm = cert.SignatureAlgorithm.FriendlyName ?? "Desconhecido";
                    host.Protocol = sslStream.SslProtocol.ToString();

                    // Regras de Conformidade
                    if (!host.IsValid)
                    {
                        host.Status = "CRÍTICO";
                        host.Message = "Certificado com erro de cadeia ou autoassinado (Untrusted).";
                    }
                    else if (host.DaysRemaining <= 0)
                    {
                        host.Status = "CRÍTICO";
                        host.Message = "Certificado SSL/TLS expirado!";
                    }
                    else if (host.DaysRemaining <= 30)
                    {
                        host.Status = "ALERTA";
                        host.Message = $"Certificado próximo do vencimento ({host.DaysRemaining} dias restantes).";
                    }
                    else
                    {
                        host.Status = "CONFORME";
                        host.Message = "Certificado válido e em conformidade de segurança.";
                    }

                }
                else
                {
                    host.Status = "FALHA";
                    host.Message = "Não foi possível obter o certificado do servidor remotos.";
                }
            }
            catch (Exception ex)
            {
                host.Status = "ERRO";
                host.Message = $"Erro ao verificar o certificado SSL/TLS: {ex.Message}";
            }
            return host;
        }
    }
}
