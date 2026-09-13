using Application.InterfacesService.InterfaceReadOnlySpan;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.ServiceReadOnly
{
    public class GetNetScanSpanService : IReadOnlySpanNet
    {
        public NetScannerEntity OnlySpan(string line)
        {
            ReadOnlySpan<char> span = line.AsSpan().Trim();
            //timestamp
            int firstBracetIndex = span.IndexOf('[');
            int finalBracetIndex = span.IndexOf(']');
            if (firstBracetIndex == -1 || finalBracetIndex == -1 || finalBracetIndex <= firstBracetIndex)
            {
                throw new ArgumentException("Invalid log line format.");
            }
            ReadOnlySpan<char> dateSpan = span.Slice(firstBracetIndex + 1, finalBracetIndex - firstBracetIndex - 1);
            span = span.Slice(finalBracetIndex + 1).Trim();
            //agent ou hostname com a porta
            int firstSpaceIndex = span.IndexOf(' ');
            if (firstSpaceIndex == -1)
            {
                throw new ArgumentException("Invalid log line format.");
            }
            ReadOnlySpan<char> agentSpan = span.Slice(0, firstSpaceIndex);
            span = span.Slice(firstSpaceIndex + 1).Trim();
            //endpointstatuscode
            int secondSpaceIndex = span.IndexOf(' ');
            if (secondSpaceIndex == -1)
            {
                throw new ArgumentException("Invalid log line format.");
            }
            ReadOnlySpan<char> endpointStatusCodeSpan = span.Slice(0, secondSpaceIndex + 2);
            span = span.Slice(secondSpaceIndex + 2).Trim();

            //latency
            int thirdSpaceIndex = span.IndexOf(' ');
            if (thirdSpaceIndex == -1)
            {
                throw new ArgumentException("Invalid log line format.");
            }
            ReadOnlySpan<char> latencySpan = span.Slice(0, thirdSpaceIndex+2);
            span = span.Slice(thirdSpaceIndex + 2).Trim();

            return new NetScannerEntity
            {
                HostName = agentSpan.ToString(),
                TimeStamp = dateSpan.ToString(),
                Port = 0, // Assuming port is not available in the log line
                IsOpen = endpointStatusCodeSpan.ToString().Contains("200"), // Example logic for IsOpen
                LatencyMs = long.TryParse(latencySpan.ToString(), out long latency) ? latency : -1,
               
            };

        }
    }


}
