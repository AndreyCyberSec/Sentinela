using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public record SysAuditorEntity
    {
        public string HostNameMachine { get; init; } = Environment.MachineName;
        public string OSVersion { get; init; } = Environment.OSVersion.ToString();
        public string CPUusage { get; init; } = string.Empty;
        public string RAMusage { get; init; } = string.Empty;
        public string DiskUsage { get; init; } = string.Empty;
        public string ActiveProcesses { get; init; } = string.Empty;
        public string NetworkInterface { get; init; } = string.Empty;
        public string SystemUptime { get; init; } = string.Empty;
        public string ComplianceStatus { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string Timestamp { get; init; } = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
    }
}
