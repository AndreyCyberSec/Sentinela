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

        public void ToFormatedInSpan(string line)
        {
            if (string.IsNullOrEmpty(line)) AnsiConsole.MarkupLine($"[red] insert line is empty or null. Try again![/]");
            else AnsiConsole.MarkupLine($"[green]system is making your date[/]");
        }
        public static LogEntity ReadOnlyGetIpEntity(string line)
        {
            
            ReadOnlySpan<char> span = line.AsSpan().Trim();

            //ipaddress
            int firstSpace = span.IndexOf(' ');
            if (firstSpace == -1) return new LogEntity();
            ReadOnlySpan<char> ipaddress = span.Slice(0, firstSpace);
            span = span.Slice(firstSpace + 1).TrimStart();

            //timestamp
            int openBracket = span.IndexOf('[');
            int closeBracket = span.IndexOf("]");
            ReadOnlySpan<char> dateSpan = ReadOnlySpan<char>.Empty;
            if(openBracket != -1 && closeBracket != -1 && closeBracket > openBracket)
            {
                dateSpan = span.Slice(openBracket + 1, closeBracket - openBracket -1);
                span = span.Slice(closeBracket + 1).TrimStart();
            }

            //endpoint
            int firstQuote = span.IndexOf('"');
            int secondQuote = -1;
            ReadOnlySpan<char> method = ReadOnlySpan<char>.Empty;
            ReadOnlySpan<char> endpoint = ReadOnlySpan<char>.Empty;

            if (firstQuote != -1)
            {
                ReadOnlySpan<char> afterFirstQuote = span.Slice(firstQuote + 1);
                secondQuote = afterFirstQuote.IndexOf('"');

                if (secondQuote != -1)
                {
                    ReadOnlySpan<char> requestSpan = afterFirstQuote.Slice(0, secondQuote); // ex: "GET /api/users HTTP/1.1"
                    span = afterFirstQuote.Slice(secondQuote + 1).TrimStart(); // Restante da linha

                    // Quebra o bloco da requisição interna ("GET", "/api/users")
                    int reqSpace1 = requestSpan.IndexOf(' ');
                    if (reqSpace1 != -1)
                    {
                        method = requestSpan.Slice(0, reqSpace1);
                        ReadOnlySpan<char> restOfReq = requestSpan.Slice(reqSpace1 + 1);

                        int reqSpace2 = restOfReq.IndexOf(' ');
                        endpoint = reqSpace2 != -1 ? restOfReq.Slice(0, reqSpace2) : restOfReq;
                    }
                }
            }

            //status code

            int firstStatus = span.IndexOf(' ');
            ReadOnlySpan<char> status = firstStatus != -1 ? span.Slice(0, firstStatus).Trim() : span.Trim();

            if (firstStatus != -1)
            {
                span = span.Slice(firstStatus + 1).TrimStart();
            }

          


            //user agente
            int Start = span.IndexOf('"');
            ReadOnlySpan<char> userAgent = ReadOnlySpan<char>.Empty;

            if (Start != -1)
            {
                ReadOnlySpan<char> after = span.Slice(Start + 1);
                int End = after.IndexOf('"');
                userAgent = End != -1 ? after.Slice(0, End) : after;
            }
            else
            {
                userAgent = span;
            }



            return new LogEntity (
                ipaddress.ToString(),
                dateSpan.ToString()
                );

           
        }

        public static LogEntity ReadOnlyGetEndpointEntity(string line)
        {

            ReadOnlySpan<char> span = line.AsSpan().Trim();

            //ipaddress
            int firstSpace = span.IndexOf(' ');
            if (firstSpace == -1) return new LogEntity();
            ReadOnlySpan<char> ipaddress = span.Slice(0, firstSpace);
            span = span.Slice(firstSpace + 1).TrimStart();

            //timestamp
            int openBracket = span.IndexOf('[');
            int closeBracket = span.IndexOf("]");
            ReadOnlySpan<char> dateSpan = ReadOnlySpan<char>.Empty;
            if (openBracket != -1 && closeBracket != -1 && closeBracket > openBracket)
            {
                dateSpan = span.Slice(openBracket + 1, closeBracket - openBracket - 1);
                span = span.Slice(closeBracket + 1).TrimStart();
            }

            //endpoint
            int firstQuote = span.IndexOf('"');
            int secondQuote = -1;
            ReadOnlySpan<char> method = ReadOnlySpan<char>.Empty;
            ReadOnlySpan<char> endpoint = ReadOnlySpan<char>.Empty;

            if (firstQuote != -1)
            {
                ReadOnlySpan<char> afterFirstQuote = span.Slice(firstQuote + 1);
                secondQuote = afterFirstQuote.IndexOf('"');

                if (secondQuote != -1)
                {
                    ReadOnlySpan<char> requestSpan = afterFirstQuote.Slice(0, secondQuote); // ex: "GET /api/users HTTP/1.1"
                    span = afterFirstQuote.Slice(secondQuote + 1).TrimStart(); // Restante da linha

                    // Quebra o bloco da requisição interna ("GET", "/api/users")
                    int reqSpace1 = requestSpan.IndexOf(' ');
                    if (reqSpace1 != -1)
                    {
                        method = requestSpan.Slice(0, reqSpace1);
                        ReadOnlySpan<char> restOfReq = requestSpan.Slice(reqSpace1 + 1);

                        int reqSpace2 = restOfReq.IndexOf(' ');
                        endpoint = reqSpace2 != -1 ? restOfReq.Slice(0, reqSpace2) : restOfReq;
                    }
                }
            }

            //status code

            int firstStatus = span.IndexOf(' ');
            ReadOnlySpan<char> status = firstStatus != -1 ? span.Slice(0, firstStatus).Trim() : span.Trim();

           if(firstStatus != -1)
            {
                span = span.Slice(firstStatus + 1).TrimStart();
            }

          


            //user agente

            int Start = span.IndexOf('"');
            span = span.Slice(Start + 1).TrimStart();
            int StartSecond = span.IndexOf('"');
            span = span.Slice(StartSecond + 1).TrimStart();
            int StartThree = span.IndexOf('"');
            
            ReadOnlySpan<char> userAgent = ReadOnlySpan<char>.Empty;

            if (Start != -1)
            {
                ReadOnlySpan<char> after = span.Slice(StartThree + 1);
                int End = after.IndexOf('"');
                userAgent = End != -1 ? after.Slice(0, End) : after;
               
            }
            else
            {
                userAgent = span;
            }

            return new LogEntity(
               endpoint.ToString(),
               method.ToString(),
               status.ToString(),
               userAgent.ToString()
              
               );
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
