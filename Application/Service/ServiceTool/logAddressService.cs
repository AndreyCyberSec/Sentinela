using Application.InterfacesService.InterfaceFind;
using Application.InterfacesService.InterfaceReadOnlySpan;
using Application.InterfacesService.InterfaceTool;
using Application.Service.ServiceReadOnly;
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

namespace Application.Service.ServiceTool
{
    public class logAddressService : ILogAddress
    {
        private readonly IEnumerable<IReadOnlySpan> readOnlySpan;
        private readonly ILogFind logFind;

        public logAddressService(IEnumerable<IReadOnlySpan> readOnlySpan, ILogFind logFind)
        {
            this.readOnlySpan = readOnlySpan;
            this.logFind = logFind;
        }
        
        public async Task<Dictionary<string, string?>> GetIpAddressAsync(string filePath)
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
                        foreach(var read in readOnlySpan)
                    {
                        LogEntity logEntity = read.OnlySpan(line);
                        logsLidos.Add(logEntity);
                    }
                        
                    }
                
                Dictionary<string,string?> logEntity1 = logFind.TopIpaddress(logsLidos);

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
                    foreach (var read in readOnlySpan)
                    {
                        LogEntity logEntity = read.OnlySpan(line);
                        logsLidos.Add(logEntity);
                    }
                }
                
              List<LogEntity> logEntities = logFind.TopEndpoint(logsLidos);
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

    



