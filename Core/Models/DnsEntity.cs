using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public record class DnsEntity
    {
        public string Hostname { get; init; }
        public string A { get; init; }
        public string AAAA { get; init; }
        public string Mx { get; init; }
        public string Txt { get; init; }
        public string Spf { get; init; }
        public string Dmarc { get; init; }
        public string? Message { get; init; }
        public string TimeStamp { get; init; } = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        public DnsEntity() { }

        public DnsEntity(string hostname, string a, string aaaa, string mx, string txt, string spf, string dmarc, string? message = null)
        {
            Hostname = hostname;
            A = a;
            AAAA = aaaa;
            Mx = mx;
            Txt = txt;
            Spf = spf;
            Dmarc = dmarc;
            Message = message;
        }
    }
}
