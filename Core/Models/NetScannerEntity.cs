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
        public string TimeStamp { get; init; }
        public int Port { get; init; }
        public bool IsOpen { get; init; }
        public long LatencyMs { get; init; }
        public string ErrorMessage { get; init; }

        public List<string> GetScan()
        {
            return new List<string>() { HostName, Port.ToString(), IsOpen.ToString(), LatencyMs.ToString()};
        }

        public NetScannerEntity() { }

        public NetScannerEntity(string hostName, int port, bool isOpen, long latencyMs, string errorMessage)
        {
            HostName = hostName;
            Port = port;
            IsOpen = isOpen;
            LatencyMs = latencyMs;
            ErrorMessage = errorMessage;
        }
        public NetScannerEntity(string hostName,string timeStamp, int port, bool isOpen, long latencyMs)
        {
            HostName = hostName;
            TimeStamp = timeStamp;
            Port = port;
            IsOpen = isOpen;
            LatencyMs = latencyMs;
            
        }
    }
}
