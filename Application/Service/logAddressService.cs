using Application.Service.ServiceReadOnly;
using Core.InterfacesService.InterfaceReadOnlySpan;
using Core.Models;
using DocumentFormat.OpenXml.Bibliography;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Service
{
    public class logAddressService
    {
        private readonly IReadOnlySpanLog readOnlyIpLog;
        private readonly IReadOnlySpanLog readOnlyEndpointLog;

        public logAddressService(IReadOnlySpanLog readOnlyIpLog, IReadOnlySpanLog readOnlyEndpointLog)
        {
            this.readOnlyIpLog = readOnlyIpLog;
            this.readOnlyEndpointLog = readOnlyEndpointLog;
        }
        
        public async Task<Dictionary<string, string>> GetIpAddressAsync(string filePath)
        {
            var logsLidos = new List<LogEntity>();
            
            try
            {
                await using var fileStream = new FileStream(filePath,FileMode.Open, FileAccess.Read,
                    FileShare.ReadWrite, bufferSize: 4096, useAsync: true);
                 using var reader = new StreamReader(fileStream);
               
                    string? line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if(string.IsNullOrEmpty(line)) continue;
                        LogEntity logEntity = readOnlyIpLog.OnlySpan(line);
                        logsLidos.Add(logEntity);
                      
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
            var logsLidos = new List<LogEntity>();
            
            try
            {
                await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read,
                    FileShare.ReadWrite, bufferSize:4096, useAsync: true);
                using var reader = new StreamReader(fileStream);
                    string? line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if(string.IsNullOrEmpty(line)) continue;
                        LogEntity logEntity = readOnlyEndpointLog.OnlySpan(line);
                        logsLidos.Add(logEntity);
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

    



