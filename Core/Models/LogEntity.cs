using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Vml;
using Spectre.Console;
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
        //construtor vazio para retorno de erros no parser dos  logs
        public LogEntity() { }
        //construtor para trazer todos os atributos desse log
        public LogEntity(string ipAddress, string date, string endpoint, string endpointMethod, string endpointStatusCode, string userAgent)
        {
            IpAddress = ipAddress;
            Date = date;
            Endpoint = endpoint;
            EndpointMethod = endpointMethod;
            EndpointStatusCode = endpointStatusCode;
            UserAgent = userAgent;
        }
        //construtor para log de ip
        public LogEntity(string ipAddress, string date)
        {
            IpAddress = ipAddress;
            Date = date;
        }
        //construtor para log de endpoint com data
        public LogEntity(string endpoint, string endpointMethod, string endpointStatusCode, string userAgent, string date)
        {
           
         
            Endpoint = endpoint;
            EndpointMethod = endpointMethod;
            EndpointStatusCode = endpointStatusCode;
            UserAgent = userAgent;
            Date = date;
        }
        //construtor para log de endpoint sem data
        public LogEntity(string endpoint, string endpointMethod, string endpointStatusCode, string userAgent)
        {


            Endpoint = endpoint;
            EndpointMethod = endpointMethod;
            EndpointStatusCode = endpointStatusCode;
            UserAgent = userAgent;
        }
        public static Dictionary<string,string> TopIpaddress(List<LogEntity> logsLidos)
        {
            var topAddresses = logsLidos
                   .GroupBy(kvp => kvp.IpAddress)
                   .OrderByDescending(x => x.Count())
                   .Take(10)
                   .ToDictionary(
                       group => group.Key,
                       group => group.Max(x => x.Date)
                   );
            return topAddresses;

        }

        public static List<LogEntity> TopEndpoint(List<LogEntity> logsLidos)
        {
            var topEndpoints = logsLidos
                   .GroupBy(kvp => kvp.GetEndpoints())
                   .OrderByDescending(x => x.Count())
                   .Take(10)
                   .SelectMany(group => group)
                  .ToList();

            return topEndpoints;
        }

        
    }
}
