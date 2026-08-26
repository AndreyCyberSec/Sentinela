using Application.InterfacesService.InterfaceReadOnlySpan;
using Core.InterfacesService.InterfaceReadOnlySpan;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.ServiceReadOnly
{
    public class GetIpAddresSpanService : IReadIpOnlySpan
    {
      
        public LogEntity OnlySpan(string line)
        {
            ReadOnlySpan<char> span = line.AsSpan().Trim();

            //ipaddress
            int firstSpace = span.IndexOf(' ');
            if (firstSpace == -1) return new LogEntity();
            ReadOnlySpan<char> ipaddress = span.Slice(0, firstSpace);
            span = span.Slice(firstSpace + 1).TrimStart();

            //timestamp
            int openBracket = span.IndexOf('[');
            int closeBracket = span.IndexOf(']');
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



            return new LogEntity(
                ipaddress.ToString(),
                dateSpan.ToString()
                );

        }

        
    }
}
