using Application.InterfacesService.InterfaceFind;
using Application.InterfacesService.InterfaceReadOnlySpan;
using Application.InterfacesService.InterfaceTool;
using Core.Models;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Service.ServiceTool
{
    public class NetScannerService : INetScannerService
    {
        private readonly IEnumerable<IReadOnlySpanNet> readOnlySpanNet;
        private readonly ILogFind logFind;

        public NetScannerService(IEnumerable<IReadOnlySpanNet> readOnlySpanNet, ILogFind logFind)
        {
            this.readOnlySpanNet = readOnlySpanNet;
            this.logFind = logFind;
        }
        public async Task<IReadOnlyList<NetScannerEntity>> CheckAllPortsAsync(IEnumerable<(string Host, int Port)> targets, int timeoutMs = 2000, CancellationToken cancellationToken = default)
        {
            // 1. Cria a coleção de tarefas em voo (sem usar await aqui!)
            // Cada item do Select inicia a tarefa imediatamente
            var tasks = targets.Select(target =>
                PortCheck(target.Host, target.Port, timeoutMs, cancellationToken));

            // 2. Task.WhenAll aguarda todas completarem em paralelo
            // e empacota todos os retornos em um array NetScannerEntity[]
            NetScannerEntity[] results = await Task.WhenAll(tasks);

            return results;
        }

        public async Task<List<NetScannerEntity>> GetNetScanner(string filePath)
        {
            var results = new List<NetScannerEntity>();
            try
            {
                await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read,
                        FileShare.ReadWrite, bufferSize: 4096, useAsync: true);
                using var reader = new StreamReader(fileStream);
                string? line;
                while((line = await reader.ReadLineAsync()) != null)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    foreach (var read in readOnlySpanNet)
                    {
                        NetScannerEntity netEntity = read.OnlySpan(line);
                        results.Add(netEntity);
                    }
                }
                List<NetScannerEntity> netEntities = logFind.TopScan(results);

                return netEntities;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler o arquivo: {ex.Message}");
                throw;
            }

        }

        public async Task<NetScannerEntity> PortCheck(
            string host,
            int port,
            int timeoutMs = 2000,
            CancellationToken cancellationToken = default)
        {
            var stopwatch = Stopwatch.StartNew();

            //token de cancelamento baseado no timeout
            using var timeoutCts = new CancellationTokenSource(timeoutMs);
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken);
            using var client = new TcpClient();

            try
            {
                //Handshake TCP não bloqueante gerenciado pelo kernel do SO
                await client.ConnectAsync(host, port, linkedCts.Token);

                return new NetScannerEntity(
                    host,
                    port,
                    true,
                    stopwatch.ElapsedMilliseconds,
                    errorMessage: null);
            }
            catch (OperationCanceledException)
            {
                string motivo = timeoutCts.IsCancellationRequested
                    ? "Timeout (Sem resposta / Firewall)"
                    : "Cancelado pelo operador";

                return new NetScannerEntity(
                    host,
                    port,
                    false,
                    stopwatch.ElapsedMilliseconds,
                    motivo);
            }
            catch (SocketException ex)
            {
                //Exceção disparada quando a porta responde com RST (Recusada) ou host inacessível
                return new NetScannerEntity(
                    host,
                    port,
                    false,
                    stopwatch.ElapsedMilliseconds,
                    $"Recusada ({ex.SocketErrorCode})");
            }
            finally
            {
                //Garante a parada do cronômetro da CPU em qualquer situação
                stopwatch.Stop();
            }
        }
    }
}