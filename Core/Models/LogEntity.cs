using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public record class LogEntity
    {
        public string IpAddress { get;}
        public string Date { get; }
        public string Endpoint { get; }
        public string EndpointMethod { get; }
        public int EndpointStatusCode { get; }
        public string UserAgent { get; }

        public LogEntity() { }
        public LogEntity(string ipAddress, string date, string endpoint, string endpointMethod, int endpointStatusCode, string userAgent)
        {
            IpAddress = ipAddress;
            Date = date;
            Endpoint = endpoint;
            EndpointMethod = endpointMethod;
            EndpointStatusCode = endpointStatusCode;
            UserAgent = userAgent;
        }

        public LogEntity(string ipAddress, string date)
        {
            IpAddress = ipAddress;
            Date = date;
        }
    }
}
