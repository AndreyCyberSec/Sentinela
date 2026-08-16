using Core.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public class ScannerService
    {
        private readonly logAddressService addressService;
        private readonly jsonAddressService jsonService;
        private readonly ReadFileServiceImpl readFileService;
        private readonly RegisterFileService registerFileService;

        public ScannerService(logAddressService logAddressService, jsonAddressService jsonAddressService, ReadFileServiceImpl _readFileService, RegisterFileService registerFileService)
        {
            addressService = logAddressService;
            jsonService = jsonAddressService;
            readFileService = _readFileService;
            this.registerFileService = registerFileService;
        }
        public async Task ScannerToolAsync()
        {
            var toppings = AnsiConsole.Prompt(
                        new MultiSelectionPrompt<string>()
                           .Title("Choose the [green]tool[/]...")
                           .NotRequired()
                           .InstructionsText("[grey](Press [blue]<space>[/] to toggle, [green]<enter>[/] to confirm)[/]")
                           .AddChoices("Sentinela-Analyzer-Log", "Management-.Env", "Net-WatchService",
                                       "Register-Dns", "Sys-Auditor", "Check-SSl/TLS", "[red]To finish[/]"));
            while (toppings.Last() != "[red]To finish[/]")
            {
                switch (toppings)
                {
                    case var t when t.Contains("Sentinela-Analyzer-Log"):
                        AnsiConsole.MarkupLine("[green]You selected Sentinela-Analyzer-Log\nThis tool select the top 10 IP with date, endpoint,method" +
                            "status of code and user agent[/]");
                        await AnsiConsole.Status()
                              .Spinner(Spinner.Known.Binary)
                              .StartAsync("Loading...", async ctx =>
                              {
                                  await Task.Delay(500);
                              });
                        var confirmedForRegister = AnsiConsole.Confirm("Do you want to register the log file path?");
                        if (!confirmedForRegister)
                        {
                            AnsiConsole.MarkupLine("[red]Operation canceled by the user.[/]");
                            await AnsiConsole.Status()
                              .Spinner(Spinner.Known.Binary)
                              .StartAsync("Loading...", async ctx =>
                              {
                                  await Task.Delay(300);
                              });
                            var enteredFilePath = await AnsiConsole.AskAsync<string>("Enter the [green]file path[/]:");
                            var formatedFilePath = enteredFilePath.Trim('\'', '"', ' ');
                            await readFileService.ReadFileAsync(formatedFilePath);
                          
                            break;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[green]You selected to register the log file path[/]");
                            var enteredFilePath = await AnsiConsole.AskAsync<string>("Enter the [green]file path[/]:");
                            var enteredFilePathForRegister = await AnsiConsole.AskAsync<string>("Enter the [green]file path for register[/]:");
                            var filePathName = await AnsiConsole.AskAsync<string>("Enter the [green]file name[/]:");
                            var formatedFilePath = enteredFilePath.Trim('\'', '"', ' ');
                            var formatedFilePathForRegister = enteredFilePathForRegister.Trim('\'', '"', ' ');
                            var formatedFileName = filePathName.Trim('\'', '"', ' ');
                            await readFileService.ReadFileAsync(formatedFilePath);
                            if (formatedFilePath.EndsWith(".json"))
                            {
                                await registerFileService.RegisterJsonAsync(formatedFilePath, formatedFileName, formatedFilePathForRegister);
                            }
                            else
                            {
                                await addressService.RegisterLogAsync(formatedFileName, formatedFilePathForRegister, formatedFilePath);
                            }
                            break;
                        }

                    case var t when t.Contains("Management-.Env"):
                        AnsiConsole.MarkupLine("[green]You selected Management-.Env[/]");
                        break;
                    case var t when t.Contains("Net-WatchService"):
                        AnsiConsole.MarkupLine("[green]You selected Net-WatchService[/]");
                        break;
                    case var t when t.Contains("Register-Dns"):
                        AnsiConsole.MarkupLine("[green]You selected Register-Dns[/]");
                        break;
                    case var t when t.Contains("Sys-Auditor"):
                        AnsiConsole.MarkupLine("[green]You selected Sys-Auditor[/]");
                        break;
                    case var t when t.Contains("Check-SSl/TLS"):
                        AnsiConsole.MarkupLine("[green]You selected Check-SSl/TLS[/]");
                        break;
                    default:
                        AnsiConsole.MarkupLine("[red]No valid tool selected.[/]");
                        break;
                }

                toppings = AnsiConsole.Prompt(
                     new MultiSelectionPrompt<string>()
                        .Title("Choose the [green]tool[/]...")
                        .NotRequired()
                        .InstructionsText("[grey](Press [blue]<space>[/] to toggle, [green]<enter>[/] to confirm)[/]")
                        .AddChoices("Sentinela-Analyzer-Log", "Management-.Env", "Net-WatchService",
                                    "Register-Dns", "Sys-Auditor", "Check-SSl/TLS", "[red]To finish[/]"));

            }
            AnsiConsole.MarkupLine("[red]Exiting the application...[/]");
        }
    }
}
