using Application.Service;
using Core.InterfaceFile;
using Core.Models;
using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.ServiceFile
{


    public class ReadFileServiceImpl : IFileReader
    {
        private readonly jsonAddressService _jsonAddressService;
        private readonly logAddressService _logAddressService;

        public ReadFileServiceImpl()
        {
            _jsonAddressService =new jsonAddressService();
            _logAddressService =new logAddressService();
        }

        public bool CanReadFile(string filePath)
        {
            if(!File.Exists(filePath))
            {
                AnsiConsole.MarkupLine($"[red]The file does not exist: {filePath}[/]");
                return false;
            }
            if (filePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase) ||
               filePath.EndsWith(".log", StringComparison.OrdinalIgnoreCase) ||
               filePath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) ||
               filePath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Unsupported file format: {filePath}[/]");
                return false;
            }
        }

        public async Task ReadFileAsync(string filePath)
        {
            if(!CanReadFile(filePath))
            {
                return;
            }

            var extension = Path.GetExtension(filePath);

            switch(extension.ToLowerInvariant())
            {
                case ".json":
                    await ReadFileJsonAsync(filePath);
                    break;
                case ".log":
                    await ReadLogAsync(filePath);
                    break;
                case ".txt":
                    await ReadTxtAsync(filePath);
                    break;
                case ".csv":
                    await ReadCsvAync(filePath);
                    break;
                default:
                    AnsiConsole.MarkupLine($"[red]Unsupported file format: {filePath}[/]");
                    break;
            }
        }

        public async Task ReadCsvAync(string filePath)
        {
            //falta implementar a leitura de arquivos CSV
            AnsiConsole.MarkupLine($"[yellow]CSV file processing is not implemented yet: {filePath}[/]");
            
        }

        public async Task ReadFileJsonAsync(string filePath)
        {
            List<JsonEntity?> result = await _jsonAddressService.ReadJsonAsync(filePath);

            var tableJson = new Table();
            tableJson.AddColumns("[bold white]TimeStamp[/]", "[bold green]Severity[/]", "[bold yellow]Ip Address[/]", "[bold red]Message[/]");
            foreach (JsonEntity? json in result)
            {
                if (json == null) continue;
                
                var severity = json.Severity.EscapeMarkup() ?? "-";
                var ipAddress = json.IpAddress.EscapeMarkup() ?? "-";
                var message = json.Message.EscapeMarkup() ?? "-";
                tableJson.AddRow($"[white]{json.TimeStamp}[/]", $"[green]{severity}[/]", $"[yellow]{ipAddress}[/]", $"[red]{message}[/]");
            }
            AnsiConsole.Write(tableJson);
        }

        public async Task ReadLogAsync(string filePath)
        {
            var result = await _logAddressService.GetIpAddressAsync(filePath);
            List<LogEntity> resultEndPoint = await _logAddressService.GetEndPointAsync(filePath);
            var tableIpAddress = new Table();
            tableIpAddress.AddColumn("[bold white]IP Address[/]").AddColumns("[bold blue]Date[/]");
            foreach (KeyValuePair<string, string> kvp in result)
            {
                tableIpAddress.AddRow($"[white]{kvp.Key}[/]", $"[blue]{kvp.Value.EscapeMarkup()}[/]");
            }
            AnsiConsole.Write(tableIpAddress);
            var tableEndPoint = new Table();
            tableEndPoint.AddColumn("[bold white]Endpoint[/]").AddColumns("[bold green]EndpointMethod[/]").AddColumns("[bold blue]EndpointStatus[/]").AddColumns("[bold yellow]UserAgent[/]");
            foreach (LogEntity endpoint in resultEndPoint)
            {
                tableEndPoint.AddRow($"[white]{endpoint.Endpoint?.EscapeMarkup()}[/]", $"[green]{endpoint.EndpointMethod?.EscapeMarkup()}[/]", $"[blue]{endpoint.EndpointStatusCode?.EscapeMarkup()}[/]", $"[yellow]{endpoint.UserAgent?.EscapeMarkup()}[/]");
            }
            AnsiConsole.Write(tableEndPoint);
        }

        public async Task ReadTxtAsync(string filePath)
        {
            // falta implementar a leitura de arquivos TXT, por isso será a mesma implementação do ReadLogAsync
            var result = await _logAddressService.GetIpAddressAsync(filePath);
            List<LogEntity> resultEndPoint = await _logAddressService.GetEndPointAsync(filePath);
            var tableIpAddress = new Table();
            tableIpAddress.AddColumn("[bold white]IP Address[/]").AddColumns("[bold blue]Date[/]");
            foreach (KeyValuePair<string, string> kvp in result)
            {
                tableIpAddress.AddRow($"[white]{kvp.Key}[/]", $"[blue]{kvp.Value.EscapeMarkup()}[/]");
            }
            AnsiConsole.Write(tableIpAddress);
            var tableEndPoint = new Table();
            tableEndPoint.AddColumn("[bold white]Endpoint[/]").AddColumns("[bold green]EndpointMethod[/]").AddColumns("[bold blue]EndpointStatus[/]").AddColumns("[bold yellow]UserAgent[/]");
            foreach (LogEntity endpoint in resultEndPoint)
            {
                tableEndPoint.AddRow($"[white]{endpoint.Endpoint?.EscapeMarkup()}[/]", $"[green]{endpoint.EndpointMethod?.EscapeMarkup()}[/]", $"[blue]{endpoint.EndpointStatusCode?.EscapeMarkup()}[/]", $"[yellow]{endpoint.UserAgent?.EscapeMarkup()}[/]");
            }
            AnsiConsole.Write(tableEndPoint);
        }
    }
}