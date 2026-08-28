using Application.InterfacesService.InterfaceTool;
using Core.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Service.ServiceTool
{
    public class jsonAddressService : IJsonAddress
    {
        private readonly JsonSerializerOptions optionsJson = new JsonSerializerOptions
        {
           
            PropertyNameCaseInsensitive = true
        };
    public async Task<List<JsonEntity?>> ReadJsonAsync(string filePath)
        {
            
            try
            {
                if (!File.Exists(filePath))
                {
                    AnsiConsole.MarkupLine($"[red]The JSON log file does not exist: {filePath}[/]");
                    return null;
                }
               await  using var reader = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                var logsLidos = await JsonSerializer.DeserializeAsync<List<JsonEntity>>(reader, optionsJson);

                 var top10Logs = logsLidos
                             .GroupBy(log => log.GetLogDetails())
                             .OrderByDescending(group => group.Count())
                             .SelectMany(group => group)
                             .Take(10)
                             .ToList();
                 return top10Logs;
                

            }
            catch (JsonException jsonEx)
            {
                AnsiConsole.MarkupLine($"[red]Error deserializing the JSON log file: {jsonEx.Message}[/]");
                throw;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error reading the JSON log file: {ex.Message}[/]");
                throw;
            }


        }
    }
}
