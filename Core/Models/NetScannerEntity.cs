using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public record NetScannerEntity
    {
        public string HostName { get; init; }
        public int Port { get; init; }
        public bool IsOpen { get; init; }
        public long LatencyMs { get; init; }
        public string ErrorMessage { get; init; }

        public NetScannerEntity() { }

        public NetScannerEntity(string hostName, int port, bool isOpen, long latencyMs, string errorMessage)
        {
            HostName = hostName;
            Port = port;
            IsOpen = isOpen;
            LatencyMs = latencyMs;
            ErrorMessage = errorMessage;
        }
    }
}
