using Core.Interface;
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
    public class jsonAddressService
    {
        
        public async Task<List<JsonEntity?>> ReadJsonAsync(string filePath)
        {
            var file = filePath;
            var logsLidos = new List<JsonEntity?>();
            try
            {
                if (!File.Exists(filePath))
                {
                    AnsiConsole.MarkupLine($"[red]The JSON log file does not exist: {file}[/]");
                    return null;
                }
                using (StreamReader reader = new StreamReader(file))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                       line.AsSpan().Slice(0,100);
                        if (line.Length > 0)
                        {
                            var timestamp = line.Substring(0, 15).Trim();
                            var severity = line.Substring(15, 5).Trim();
                            var ipAddress = line.Substring(20, 11).Trim();
                            var message = line.Substring(31, 13).Trim();
                            logsLidos.Add(new JsonEntity(timestamp, severity, ipAddress, message));
                        }

                        
                    }
                    var top10Logs = logsLidos
                            .GroupBy(log => log.GetLogDetails())
                            .OrderByDescending(group => group.Count())
                            .SelectMany(group => group)
                            .Take(10)
                            .ToList();

                    return logsLidos;
                }
                }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error reading the JSON log file: {ex.Message}[/]");
                throw;
            }


        }
    }
}
