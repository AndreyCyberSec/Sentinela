using Core.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Service
{
    public class logAddressService
    {
        public async Task<Dictionary<string, string>> GetIpAddressAsync(string filePath)
        {
            var file = filePath;
            var logsLidos = new List<LogEntity>();
            try
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        //faça as devidas validações para garantir que a linha está no formato esperado antes de tentar dividir
                        var parts = line.Split(' ');
                        if (parts.Length >= 2)
                        {
                            var ipAddress = parts[0];
                            var date = parts[3] + parts[4];
                            logsLidos.Add(new LogEntity(ipAddress, date));
                        }
                    }
                }
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
            catch (Exception ex)
            {

                AnsiConsole.MarkupLine($"[red]Error reading the file: {ex.Message}[/]");
                throw;
            }

        }

        public async Task<List<LogEntity>> GetEndPointAsync(string filePath)
        {
            var file = filePath;
            var logsLidos = new List<LogEntity>();

            try
            {
                using (var reader = new StreamReader(file))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        //faça as devidas validações para garantir que a linha está no formato esperado antes de tentar dividir
                        var parts = line.Split(' ');
                        if (parts.Length >= 12)
                        {
                            var endpoint = parts[6];
                            var endpointMethod = parts[5];
                            var endpointStatusCode = parts[7];
                            var userAgent = parts[11];
                            var date = parts[3] + parts[4];
                            logsLidos.Add(new LogEntity(endpoint, endpointMethod, endpointStatusCode, userAgent, date));
                        }
                    }
                }
                var topEndpoints = logsLidos
                    .GroupBy(kvp => kvp.GetEndpoints())
                    .OrderByDescending(x => x.Count())
                    .Take(10)
                    .SelectMany(group => group)
                   .ToList();

                return topEndpoints;

            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error reading the file: {ex.Message}[/]");
                throw;
            }
        }

       
    }
}

    



