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

        public LogEntity(string endpoint, string endpointMethod, string endpointStatusCode, string userAgent)
        {


            Endpoint = endpoint;
            EndpointMethod = endpointMethod;
            EndpointStatusCode = endpointStatusCode;
            UserAgent = userAgent;
        }
        public static LogEntity ReadOnlyGetIpEntity(string line)
        {
            
            ReadOnlySpan<char> span = line.AsSpan().Trim();

            int nextSpace = span.IndexOf(' ');
            ReadOnlySpan<char> ipaddress = span.Slice(0, nextSpace);
            span = span.Slice(nextSpace + 1).TrimStart();

            nextSpace = span.IndexOf(' ');
            ReadOnlySpan<char> timestamp = span.Slice(0, nextSpace);
            span = span.Slice(nextSpace + 1).TrimStart();

         
           

            return new LogEntity (
                ipaddress.ToString(),
                timestamp.ToString()
                );

           
        }

        public static LogEntity ReadOnlyGetEndpointEntity(string file)
        {
            
            ReadOnlySpan<char> span = file.AsSpan().Trim();

            int nextSpace = span.IndexOf(' ');
            ReadOnlySpan<char> ipaddress = span.Slice(0, nextSpace);
            span = span.Slice(nextSpace + 1).TrimStart();

            nextSpace = span.IndexOf(' ');
            ReadOnlySpan<char> timestamp = span.Slice(0, nextSpace);
            span = span.Slice(nextSpace + 1).TrimStart();


            nextSpace = span.IndexOf(' ');
            ReadOnlySpan<char> endpoint = span.Slice(0, nextSpace);
            span = span.Slice(nextSpace + 1).TrimStart();

            nextSpace = span.IndexOf(' ');
            ReadOnlySpan<char> endpointMethod = span.Slice(0, nextSpace);
            span = span.Slice(nextSpace + 1).TrimStart();

            nextSpace = span.IndexOf(' ');
            ReadOnlySpan<char> endpointStatusCode = span.Slice(0, nextSpace);
            span = span.Slice(nextSpace + 1).TrimStart();

            nextSpace = span.IndexOf(' ');
            ReadOnlySpan<char> userAgent = span.Slice(0, nextSpace);
            span = span.Slice(nextSpace + 1).TrimStart();

            
            
           

            return new LogEntity(
               endpoint.ToString(),
               endpointMethod.ToString(),
               endpointStatusCode.ToString(),
               userAgent.ToString()
              
               );
        }

        
    }
}
