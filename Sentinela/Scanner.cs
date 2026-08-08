using Application.Service;
using Core.Models;
using Spectre.Console;
using System.Runtime.CompilerServices;

namespace Sentinela
{
    public class Scanner
    {
        private static readonly logAddressService addressService = new logAddressService();
        public static async Task Main(string[] args)
        {
            var title = new FigletText("Sentinela")
      .Color(Color.Green);

            var subtitle = new Markup("[italic grey]Welcome the tools of Sentinela[/]");
            var subtitle1 = new Markup("[italic grey]Version: 1.0.0[/]");
            var subtitle2 = new Markup("[italic grey]Development by Andrey[/]");

            var titleAligned = Align.Center(title);
            var subtitleAligned = Align.Left(subtitle);
            var subtitle1Aligned = Align.Left(subtitle1);
            var subtitle2Aligned = Align.Left(subtitle2);

            AnsiConsole.Write(titleAligned);
            AnsiConsole.Write(subtitleAligned);
            AnsiConsole.Write(subtitle1Aligned);
            AnsiConsole.Write(subtitle2Aligned);
            AnsiConsole.WriteLine();
            await AnsiConsole.Status()
                      .Spinner(Spinner.Known.Binary)
                     .StartAsync("Loading...", async ctx =>
                          {
                             await Task.Delay(1000);
                          });

            var toppings = AnsiConsole.Prompt(
                     new MultiSelectionPrompt<string>()
                        .Title("Choose the [green]tool[/]...")
                        .NotRequired()
                        .InstructionsText("[grey](Press [blue]<space>[/] to toggle, [green]<enter>[/] to confirm)[/]")
                        .AddChoices("Sentinela-Analyzer-Log", "Management-.Env", "Net-WatchService",
                                    "Register-Dns", "Sys-Auditor", "Check-SSl/TLS","[red]To finish[/]"));
            while ( toppings.Last() != "[red]To finish[/]")
            {
                switch (toppings)
                {
                    case var t when t.Contains("Sentinela-Analyzer-Log"):
                        AnsiConsole.MarkupLine("[green]You selected Sentinela-Analyzer-Log\nThis tool select the top 10 IP with date, endpoint,method" +
                            "status of code and user agent[/]");
                        var confirmed = AnsiConsole.Confirm("Do you want to continue?");
                        if (!confirmed)
                        {
                            AnsiConsole.MarkupLine("[red]Operation canceled by the user.[/]");
                            break;
                        }
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
                            break;
                        }
                        var enteredFilePath = await AnsiConsole.AskAsync<string>("Enter the [green]file path[/]:");
                        var enteredFilePathForRegister = await AnsiConsole.AskAsync<string>("Enter the [green]file path for register[/]:");
                        var filePathName = await AnsiConsole.AskAsync<string>("Enter the [green]file name[/]:");
                        var formatedFilePath = enteredFilePath.Trim('\'', '"',' ');
                        var formatedFilePathForRegister = enteredFilePathForRegister.Trim('\'', '"', ' ');
                        var formatedFileName = filePathName.Trim('\'', '"', ' ');
                        var result = await addressService.GetIpAddressAsync(formatedFilePath);
                        List<LogEntity> resultEndPoint = await addressService.GetEndPointAsync(formatedFilePath);
                        await addressService.RegisterLogAsync(formatedFileName, formatedFilePathForRegister, formatedFilePath);
                        var tableIpAddress = new Table();
                        tableIpAddress.AddColumn("[bold white]IP Address[/]").AddColumns("[bold blue]Date[/]");
                        foreach (KeyValuePair<string, string> kvp in result)
                        {
                            tableIpAddress.AddRow($"[white]{kvp.Key}[/]", $"[blue]{kvp.Value.EscapeMarkup()}[/]");
                        }
                        AnsiConsole.Write(tableIpAddress);
                        var tableEndPoint = new Table();
                        tableEndPoint.AddColumn("[bold white]Endpoint[/]").AddColumns("[bold blue]EndpointStatus[/]").AddColumns("[bold green]EndpointMethod[/]").AddColumns("[bold yellow]UserAgent[/]");
                        foreach (LogEntity endpoint in resultEndPoint)
                        {
                         tableEndPoint.AddRow($"[white]{endpoint.Endpoint}[/]", $"[blue]{endpoint.EndpointStatusCode}[/]", $"[green]{endpoint.EndpointMethod}[/]", $"[yellow]{endpoint.UserAgent}[/]");
                        }
                        AnsiConsole.Write(tableEndPoint);

                        break;
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
