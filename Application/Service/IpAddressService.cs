using Core.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public class IpAddressService
    {
        public async Task<Dictionary<string,string>> GetIpAddressAsync(string filePath)
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
    }
}
