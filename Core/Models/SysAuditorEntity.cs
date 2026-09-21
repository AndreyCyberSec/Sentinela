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
        public string CPUusage { get; set; } = string.Empty;
        public string RAMusage { get; set; } = string.Empty;
        public string DiskUsage { get; set; } = string.Empty;
        public string ActiveProcesses { get; set; } = string.Empty;
        public string NetworkInterface { get; set; } = string.Empty;
        public string SystemUptime { get; set; } = string.Empty;
        public string ComplianceStatus { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Timestamp { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

        public SysAuditorEntity() { }

        public SysAuditorEntity(string cpuUsage, string ramUsage, string diskUsage, string activeProcesses, string networkInterface, string systemUptime, string complianceStatus, string message)
        {
            CPUusage = cpuUsage;
            RAMusage = ramUsage;
            DiskUsage = diskUsage;
            ActiveProcesses = activeProcesses;
            NetworkInterface = networkInterface;
            SystemUptime = systemUptime;
            ComplianceStatus = complianceStatus;
            Message = message;
        }
    }
}
