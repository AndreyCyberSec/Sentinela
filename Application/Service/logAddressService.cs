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

        public async Task RegisterLogAsync(string fileName, string filePath, string originalFile)
        {
            var file = fileName;
            try
            {
                if (!fileName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    file += ".xls";
                }
                var fullPath = Path.Combine(filePath, file);
                var diretorio = Path.GetDirectoryName(fullPath);

                if (!string.IsNullOrEmpty(diretorio) && !Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio);
                }
                using (StreamWriter writer = new StreamWriter(fullPath, false, Encoding.UTF8))
                {
                    await writer.WriteLineAsync("-----IP ADDRESS----");
                    await writer.WriteLineAsync("IP Address;Date");
                    foreach (var log in await GetIpAddressAsync(originalFile))
                    {
                        var escapedIpAddress = log.Key.EscapeMarkup();
                        var escapedDate = log.Value.EscapeMarkup();
                        await writer.WriteLineAsync($"{escapedIpAddress};{escapedDate}");
                    }

                    await writer.WriteLineAsync();

                    await writer.WriteLineAsync("-----ENDPOINTS----");
                    await writer.WriteLineAsync("Endpoint;Method;Status Code;User Agent");
                    foreach (var log in await GetEndPointAsync(originalFile))
                    {
                        var escapedEndpoint = log.Endpoint.EscapeMarkup();
                        var escapedMethod = log.EndpointMethod.EscapeMarkup();
                        var escapedStatusCode = log.EndpointStatusCode.EscapeMarkup();
                        var escapedUserAgent = log.UserAgent.EscapeMarkup();
                        var escapedDate = log.Date.EscapeMarkup();
                        await writer.WriteLineAsync($"{escapedEndpoint};{escapedMethod};{escapedStatusCode};{escapedUserAgent}");
                    }
                    AnsiConsole.MarkupLine($"[green]Log file path registered successfully: {fullPath}[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error registering the log file path: {ex.Message}[/]");
                throw;

            }
        }
    }
}

    



