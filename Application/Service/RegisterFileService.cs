using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public class RegisterFileService
    {
        private readonly jsonAddressService _jsonAddressService;
        private readonly logAddressService _logAddressService;

        public RegisterFileService(jsonAddressService jsonAddressService, logAddressService logAddressService)
        {
            _jsonAddressService = jsonAddressService;
            _logAddressService = logAddressService;
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
                    foreach (var log in await _logAddressService.GetIpAddressAsync(originalFile))
                    {
                        var escapedIpAddress = log.Key.EscapeMarkup();
                        var escapedDate = log.Value.EscapeMarkup();
                        await writer.WriteLineAsync($"{escapedIpAddress};{escapedDate}");
                    }

                    await writer.WriteLineAsync();

                    await writer.WriteLineAsync("-----ENDPOINTS----");
                    await writer.WriteLineAsync("Endpoint;Method;Status Code;User Agent");
                    foreach (var log in await _logAddressService.GetEndPointAsync(originalFile))
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
        public async Task RegisterJsonAsync(string fileName, string filePath, string originalFile)
        {
            if(string.IsNullOrEmpty(fileName)){
                AnsiConsole.MarkupLine("[red] You need insert a name file for continue[/]");
                return;
            }
            try
            {
                if(!fileName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    fileName += ".xls";
                }
            }
            catch (Exception ex)
            {

            }
        }
     }
}
