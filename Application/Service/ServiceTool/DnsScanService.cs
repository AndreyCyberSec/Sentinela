using Application.InterfacesService.InterfaceTool;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.ServiceTool
{
    public class DnsScanService : IDnsEntityService
    {
        public async Task<DnsEntity> AuditDomainAsync(string hostname, CancellationToken cancellationToken = default)
        {
            if(string.IsNullOrWhiteSpace(hostname) || hostname.Contains("://"))
            {
                throw new Exception("Hostname cannot be null or empty and cannot contain a URL scheme (e.g., http:// or https://)");
            }
            try
            {
                //busca ip A e AAAA
                IPAddress[] addresses = Dns.GetHostAddresses(hostname);

                string recordA = string.Join(", ", addresses.Where(a => a.AddressFamily == AddressFamily.InterNetwork).Select(a => a.ToString()));
                string recordAAAA = string.Join(", ", addresses.Where(a => a.AddressFamily == AddressFamily.InterNetworkV6).Select(a => a.ToString()));

                //buscar o registro de texto
                IPHostEntry hostEntry = await Dns.GetHostEntryAsync(hostname, cancellationToken);
                var aliasesList = hostEntry.Aliases.ToList();

                string recordTxt = string.Join(" | ", aliasesList);

                string spf = aliasesList.FirstOrDefault(a => a.StartsWith("v=spf1", StringComparison.OrdinalIgnoreCase)) ?? "Nenhum registro SPF encontrado (v=spf1)";

                string dmarc = aliasesList.FirstOrDefault(a => a.StartsWith("v=DMARC1", StringComparison.OrdinalIgnoreCase)) ?? "Nenhum registro DMARC encontrado (v=DMARC1)";

                return new DnsEntity
                {
                    Hostname = hostname,
                    A = recordA,
                    AAAA = recordAAAA,
                    Mx = string.Join(", ", hostEntry.Aliases), // Assuming MX records are in aliases for this example
                    Txt = recordTxt,
                    Spf = spf,
                    Dmarc = dmarc,
                    Message = "Audit completed successfully",
                    TimeStamp = DateTime.UtcNow.ToString("o")
                };
            }
            catch (Exception ex)
            {
                return new DnsEntity
                {
                    Hostname = hostname,
                    Message = $"Error during audit: {ex.Message}",
                };
            }
            
        }
    }
}
