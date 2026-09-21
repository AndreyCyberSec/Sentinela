using Application.InterfacesService.InterfaceTool;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.ServiceTool
{
    public class SysAuditorService : ISysAuditor
    {
        public async Task<SysAuditorEntity> AuditSystemAsync()
        {
           return await Task.Run(() =>
           {
               var report = new SysAuditorEntity();

               try
               {
                   TimeSpan uptime = TimeSpan.FromMilliseconds(Environment.TickCount64);
                   report.SystemUptime = $"{uptime.Days} days, {uptime.Hours} hours, {uptime.Minutes} minutes, {uptime.Seconds} seconds";

                   var drivesInfo = DriveInfo.GetDrives()
                    .Where(d => d.IsReady)
                    .Select(d => $"{d.Name} ({d.AvailableFreeSpace / 1024 / 1024 / 1024}GB livres de {d.TotalSize / 1024 / 1024 / 1024}GB)")
                    .ToList();
                   report.DiskUsage = string.Join(" | ", drivesInfo);

                   Process[] processes = Process.GetProcesses();
                   report.ActiveProcesses = $"{processes.Length} processos em execução";

                   var netInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .Select(n => $"{n.Name} ({n.Description})")
                    .ToList();
                   report.NetworkInterface = netInterfaces.Any() ? string.Join(" | ", netInterfaces) : "Nenhuma interface ativa";

                   using (var currentProc = Process.GetCurrentProcess())
                   {
                       long ramUsedMb = currentProc.WorkingSet64 / 1024 / 1024;
                       report.RAMusage = $"Uso pelo Sentinela: {ramUsedMb} MB";
                   }

                   if(uptime >= TimeSpan.FromDays(1))
                   {
                       report.ComplianceStatus = "Compliant";
                       report.Message = "O sistema está em conformidade com os requisitos de auditoria.";
                   }
                   else
                   {
                       report.ComplianceStatus = "Non-Compliant";
                       report.Message = "O sistema não está em conformidade com os requisitos de auditoria.";
                   }
               }
               catch (Exception ex)
               {
                   report.Message = $"Error during system audit: {ex.Message}";
                   report.ComplianceStatus = "Error";
               }

               return report;
           });
        }

        public Task<string> GetHostname()
        {
            return  Task.FromResult(Environment.MachineName);
        }
    }
}
