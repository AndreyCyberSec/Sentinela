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
        public string EndpointStatusCode { get; }
        public string UserAgent { get; }

        public List<string> GetEndpoints()
        {
            return new List<string> { Endpoint, EndpointMethod, EndpointStatusCode, UserAgent };
        }

        public LogEntity() { }
        public LogEntity(string ipAddress, string date, string endpoint, string endpointMethod, string endpointStatusCode, string userAgent)
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

        public LogEntity(string endpoint, string endpointMethod, string endpointStatusCode, string userAgent, string date)
        {
           
         
            Endpoint = endpoint;
            EndpointMethod = endpointMethod;
            EndpointStatusCode = endpointStatusCode;
            UserAgent = userAgent;
            Date = date;
        }
    }
}
