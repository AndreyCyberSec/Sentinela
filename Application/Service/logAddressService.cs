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
                    string? line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if(string.IsNullOrEmpty(line)) continue;
                        LogEntity logEntity = LogEntity.ReadOnlyGetIpEntity(line);
                        logsLidos.Add(logEntity);
                      
                    }
                }
                Dictionary<string,string> logEntity1 = LogEntity.TopIpaddress(logsLidos);

                return logEntity1;
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
                    string? line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if(string.IsNullOrEmpty(line)) continue;
                        LogEntity logEntity = LogEntity.ReadOnlyGetEndpointEntity(line);
                        logsLidos.Add(logEntity);
                    }
                }
              List<LogEntity> logEntities = LogEntity.TopEndpoint(logsLidos);
                return logEntities;

            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error reading the file: {ex.Message}[/]");
                throw;
            }
        }

      
    }
}

    



