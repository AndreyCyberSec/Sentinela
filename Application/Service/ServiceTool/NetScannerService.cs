using Application.InterfacesService.InterfaceTool;
using Core.Models;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.ServiceTool
{
    public class NetScannerService : INetScannerService
    {
        public async Task<NetScannerEntity> PortCheck(string hostName, int port, int timeOutMs, CancellationToken cancellationToken= default)
        {
            var ipEndpoint = new IPEndPoint(IPAddress.Parse(hostName), port);
            using TcpClient client = new();
            await client.ConnectAsync(ipEndpoint.Address, ipEndpoint.Port, cancellationToken);
            await using NetworkStream stream = client.GetStream();
            var buffer = new byte[1];
            int receivedBytes = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
            var message = receivedBytes > 0 ? Encoding.UTF8.GetString(buffer, 0, receivedBytes) : string.Empty;
            Console.WriteLine($"Received message: {message}");
        }
    }
}
