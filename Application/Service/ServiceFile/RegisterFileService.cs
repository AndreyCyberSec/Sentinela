using Application.Service;
using ClosedXML.Excel;
using Core.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.ServiceFile
{
    public class RegisterFileService
    {
       
        public async Task RegisterLogAsync(string fileName, string filePath, string originalFile)
        {
            logAddressService _logAddressService = new logAddressService();


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
        public async Task RegisterJsonAsync(string originalFile, string fileName, string outPutDirectory)
        {
            jsonAddressService _jsonAddressService = new jsonAddressService();
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(outPutDirectory))
            {
                AnsiConsole.MarkupLine("[red] You need insert a name file for continue[/]");
                return;
            }
            try
            {
                string fullPath = Path.GetFullPath(outPutDirectory);

                if (Directory.Exists(fullPath))
                {
                    string nameFile = fileName;
                    fullPath = Path.Combine(fullPath, nameFile);
                }
                else if (!fullPath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    fullPath += ".xlsx";
                }

                string? directoryPath = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                //criação da planilha
                using var workBook = new XLWorkbook();
                var workSheet = workBook.Worksheets.Add("Logs de auditoria");

                //criação dos cabeçalhos
                workSheet.Cell(1, 1).Value = "TimeStamp";
                workSheet.Cell(1, 2).Value = "Severity";
                workSheet.Cell(1, 3).Value = "IpAddress";
                workSheet.Cell(1, 4).Value = "Message";

                //estilização dos cabeçalhos
                var headerRow = workSheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#1F4E78");
                headerRow.Style.Font.FontColor = XLColor.White;

                var logs = await _jsonAddressService.ReadJsonAsync(originalFile);
                if (logs == null || logs.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No log records found to export.[/]");
                    return;
                }

                //preenchimento das linhas
                int currentRow = 2;
                foreach (var row in logs)
                {
                    if (row == null) continue;
                    workSheet.Cell(currentRow, 1).Value = row.TimeStamp.ToString() ?? "-";
                    workSheet.Cell(currentRow, 2).Value = row.Severity ?? "-";
                    workSheet.Cell(currentRow, 3).Value = row.Source ?? "-";
                    workSheet.Cell(currentRow, 4).Value = row.IpAddress ?? "-"  ;
                    workSheet.Cell(currentRow, 5).Value = row.Message ?? "-";

                    if (row.Severity == "CRITICAL")
                    {
                        workSheet.Row(currentRow).Style.Fill.BackgroundColor = XLColor.FromHtml("#FCE4D6");
                    }

                    currentRow++;
                }

                workSheet.Columns().AdjustToContents();

                await using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
               
                workBook.SaveAs(fileStream);
                AnsiConsole.MarkupLine($"[green]Log file path registered successfully: {fullPath}[/]");


            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error registering the log file path: {ex.Message}[/]");
                throw;
            }
        }
     }
}
