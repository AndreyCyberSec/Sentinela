using Application.InterfacesService.InterfaceScanner;
using Application.InterfacesService.InterfaceTool;
using Application.Service.ServiceFile;
using Core.Interfaces.InterfaceFile;
using Core.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.ServiceApplication
{
    public class ScannerService : IScannerService
    {
      
        private readonly IFileReader readFileService;
        private readonly IFileRegister registerFileService;
        private readonly INetScannerService netScannerService;
        private readonly IDnsEntityService dnsEntityService;
        private readonly ISysAuditor sysAuditorService;

        public ScannerService( IFileReader _readFileService, IFileRegister registerFileService,
            INetScannerService _netScannerService, IDnsEntityService dnsEntityService, ISysAuditor _sysAuditorService)
        {
            readFileService = _readFileService;
            this.registerFileService = registerFileService;
            netScannerService = _netScannerService;
            this.dnsEntityService = dnsEntityService;
            this.sysAuditorService = _sysAuditorService;
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
                        AnsiConsole.MarkupLine(@"[green]You selected Sentinela-Analyzer-Log 
                                                     This tool select the top 10 IP with date, endpoint,method" +
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
                                await registerFileService.RegisterLogAsync(formatedFileName, formatedFilePathForRegister, formatedFilePath);
                            }
                            break;
                        }

                    case var t when t.Contains("Management-.Env"):
                        AnsiConsole.MarkupLine("[green]You selected Management-.Env[/]");
                        var functionEnv = AnsiConsole.Prompt(new MultiSelectionPrompt<string>()
                            .Title("Do you want to encrypt or decrypt the log file path?")
                            .AddChoices(new[] { "Encrypt", "Decrypt" }));
                        if (functionEnv.Contains("Decrypt"))
                        {
                            var envPath = await AnsiConsole.AskAsync<string>("Enter the [green]file path for decrypt[/]:");
                            var envPassword = await AnsiConsole.PromptAsync(
                                new TextPrompt<string>("Enter the [green]password[/]:")
                                    .PromptStyle("red")
                                    .Secret());
                            var formatedEnvPath = envPath.Trim('\'', '"', ' ');
                            await AnsiConsole.Status()
                                  .Spinner(Spinner.Known.Binary)
                                  .StartAsync("Decrypting vault...", async ctx =>
                                  {
                                      await Task.Delay(100);
                                  });
                            var envDecrypted = await registerFileService.DecryptEnvAsync(formatedEnvPath, envPassword);
                            await AnsiConsole.Status()
                            .Spinner(Spinner.Known.Binary)
                            .StartAsync("Loading...", async ctx =>
                            {
                                await Task.Delay(100);
                            });
                            var panel = new Panel(envDecrypted.EscapeMarkup())
                            {
                                Header = new PanelHeader("[green]Vault Decrypted[/]"),
                                Border = BoxBorder.Rounded,
                                Padding = new Padding(1, 1, 1, 1),
                                Expand = true
                            };
                            AnsiConsole.Write(panel);
                        }
                        else
                        {
                            await AnsiConsole.Status()
                            .Spinner(Spinner.Known.Binary)
                            .StartAsync("Loading...", async ctx =>
                            {
                                await Task.Delay(500);
                            });

                            var path = await AnsiConsole.PromptAsync(
                                    new TextPrompt<string>("Enter the [green]file path[/]:")
                                             .Validate(filePath =>
                                             {
                                                 var clean = filePath.Trim('\'', '"', ' ');
                                                 return File.Exists(clean)
                                               ? ValidationResult.Success()
                                           : ValidationResult.Error("[red]File not found. Please verify the path.[/]");
                                             }));
                            var RegisterPath = await AnsiConsole.AskAsync<string>("Enter the [green]file path for register[/]:");
                            var NameFile = await AnsiConsole.AskAsync<string>("Enter the [green]file name[/]:");
                            var password = await AnsiConsole.PromptAsync(
                                new TextPrompt<string>("Enter the [green]password[/]:")
                                    .PromptStyle("red")
                                    .Secret());
                            var formatedPath = path.Trim('\'', '"', ' ');
                            var formatedRegisterPath = RegisterPath.Trim('\'', '"', ' ');
                            var formatedNameFile = NameFile.Trim('\'', '"', ' ');
                            await AnsiConsole.Status()
                                  .Spinner(Spinner.Known.Binary)
                                  .StartAsync("Encrypting and generating vault...", async ctx =>
                                  {
                                      await registerFileService.RegisterEnvAsync(formatedPath, formatedNameFile, password, formatedRegisterPath);
                                  });
                            AnsiConsole.MarkupLine("[bold green]Vault created successfully![/]");

                        }

                            break;
                    case var t when t.Contains("Net-WatchService"):
                        AnsiConsole.MarkupLine("[green]You selected Net-WatchService[/]");
                       
                        var quantidade = await AnsiConsole.PromptAsync(
                            new TextPrompt<int>("Enter the [green]number of scans[/]:")
                                .Validate(num =>
                                {
                                    return num > 0
                                        ? ValidationResult.Success()
                                        : ValidationResult.Error("[red]Number of scans must be greater than 0.[/]");
                                }));
                        var targets = new List<(string Host, int Port)>();
                        for (int i = 0; i < quantidade; i++)
                        {
                            var targetHost = await AnsiConsole.PromptAsync(
                           new TextPrompt<string>("Enter the [green]hostname or IP address[/]:")
                               .Validate(host =>
                               {
                                   return !string.IsNullOrWhiteSpace(host)
                                       ? ValidationResult.Success()
                                       : ValidationResult.Error("[red]Hostname cannot be empty.[/]");
                               }));

                            var targetPort = await AnsiConsole.PromptAsync(
                                new TextPrompt<int>("Enter the [green]port number[/]:")
                                    .Validate(port =>
                                    {
                                        return port > 0 && port <= 65535
                                            ? ValidationResult.Success()
                                            : ValidationResult.Error("[red]Port must be between 1 and 65535.[/]");
                                    }));
                            var hostSanitizado = targetHost.Trim('\'', '"', ' ');
                            targets.Add((hostSanitizado, targetPort));
                        }
                       

                        var outputDirectory = await AnsiConsole.PromptAsync(
                            new TextPrompt<string>("Enter the [green]output directory[/] or Enter  for directory default").AllowEmpty());

                        var dirSanitizado = string.IsNullOrWhiteSpace(outputDirectory) ? null : outputDirectory.Trim('\'', '"', ' ');

                        IReadOnlyList<NetScannerEntity> scanResults = Array.Empty<NetScannerEntity>();

                        await AnsiConsole.Status()
                                .Spinner(Spinner.Known.Dots)
                                .StartAsync($"Escaneando {targets.Count} alvos...", async ctx =>
                                {
                                    scanResults = await netScannerService.CheckAllPortsAsync(targets, timeoutMs: 2000);
                                    await registerFileService.CheckPortResult(scanResults, dirSanitizado);
                                    
                                });
                                       var tableScan = new Table()
                                                .Border(TableBorder.Rounded)
                                                 .AddColumns(
                                                     "[bold yellow]TimeStamp[/]",
                                                     "[bold green]HostName[/]",
                                                     "[bold blue]IsOpen[/]",
                                                     "[bold white]Latency (ms)[/]",
                                                     "[bold red]Diagnóstico[/]"
                                                 );

                        foreach (var res in scanResults)
                        {
                            string status = res.IsOpen ? "[green]OPEN[/]" : "[red]CLOSED[/]";
                            string latency = res.IsOpen ? $"{res.LatencyMs} ms" : "-";
                            string diagnostico = res.ErrorMessage ?? "[green]SYN-ACK OK[/]";

                            tableScan.AddRow(
                                $"[white]{res.HostName}[/]",
                                $"[yellow]{res.Port}[/]",
                                status,
                                latency,
                                diagnostico.EscapeMarkup()
                            );
                        }
                      AnsiConsole.Write(tableScan);

                        AnsiConsole.MarkupLine($"[bold green]Varredura finalizada e registrada com sucesso![/]");

                        break;
                    case var t when t.Contains("Register-Dns"):
                        AnsiConsole.MarkupLine("[green]You selected Register-Dns[/]");

                        var domainInput = await AnsiConsole.PromptAsync(
                                    new TextPrompt<string>("Digite o [green]domínio[/] para auditoria (ex: google.com):")
                                        .Validate(domain => !string.IsNullOrWhiteSpace(domain) && !domain.Contains("://")
                                        ? ValidationResult.Success()
                                        : ValidationResult.Error("[red]Digite apenas o nome do domínio (ex: empresa.com), sem 'http://'.[/]")));

                        var outputDir = await AnsiConsole.PromptAsync(
                                    new TextPrompt<string>("Diretório para salvar o relatório de log [grey](Enter para padrão)[/]:")
                                        .AllowEmpty());

                        string domainSanitizado = domainInput.Trim('\'', '"', ' ').ToLower();
                        string? dirFormatado = string.IsNullOrWhiteSpace(outputDir) ? null : outputDir.Trim('\'', '"', ' ');

                        DnsEntity resultado = null!;

                        await AnsiConsole.Status()
                                .Spinner(Spinner.Known.Dots)
                                .StartAsync($"Consultando registros DNS de [yellow]{domainSanitizado}[/]...", async ctx =>
                                {
                                    // O serviço executa as chamadas de rede e retorna a entidade preenchida
                                    resultado = await dnsEntityService.AuditDomainAsync(domainSanitizado);

                                    // Gravação isolada do arquivo de log utilizando os dados diretamente da memória (RAM)
                                    if (resultado != null)
                                    {
                                        await registerFileService.CheckDnsResult(resultado, dirFormatado);
                                    }
                                });

                        var table = new Table()
                                .Border(TableBorder.Rounded)
                                .Title($"[bold yellow]Resultado da Auditoria DNS: {resultado.Hostname}[/]")
                                .AddColumns(
                                    "[bold white]Verificação[/]",
                                    "[bold white]Status[/]",
                                    "[bold white]Detalhes / Registros[/]"
                                );
                        // Linha: Registro A
                        table.AddRow("Registro IPv4 (A)", "[green]OK[/]", resultado.A.EscapeMarkup());

                        // Linha: Registro AAAA
                        table.AddRow("Registro IPv6 (AAAA)", "[green]OK[/]", resultado.AAAA.EscapeMarkup());

                        // Linha: Regra SPF
                        string spfStatus = resultado.Spf.StartsWith("v=spf1", StringComparison.OrdinalIgnoreCase)
                            ? "[bold green]PROTEGIDO[/]"
                            : "[bold red]VULNERÁVEL[/]";
                        table.AddRow("Política SPF", spfStatus, resultado.Spf.EscapeMarkup());

                        // Linha: Regra DMARC
                        string dmarcStatus = resultado.Dmarc.StartsWith("v=DMARC1", StringComparison.OrdinalIgnoreCase)
                            ? "[bold green]PROTEGIDO[/]"
                            : "[bold yellow]ALERTA[/]";
                        table.AddRow("Política DMARC", dmarcStatus, resultado.Dmarc.EscapeMarkup());

                        // Imprime a tabela completa no console
                        AnsiConsole.Write(table);

                        if (!string.IsNullOrEmpty(resultado.Message))
                        {
                            AnsiConsole.MarkupLine($"[grey]Mensagem do Diagnóstico: {resultado.Message.EscapeMarkup()}[/]");
                        }

                        AnsiConsole.MarkupLine("[bold green]Auditoria DNS finalizada com sucesso![/]\n");

                        break;
                    case var t when t.Contains("Sys-Auditor"):
                        AnsiConsole.MarkupLine("[green]You selected Sys-Auditor[/]");

                        var diretorio = await AnsiConsole.PromptAsync(
                                   new TextPrompt<string>("Diretório para salvar o relatório de log [grey](Enter para padrão)[/]:")
                                       .AllowEmpty());

                        string? dirFormatadoSys = string.IsNullOrWhiteSpace(diretorio) ? null : diretorio.Trim('\'', '"', ' ');
                        var hostname = sysAuditorService.GetHostname().EscapeMarkup();
                        SysAuditorEntity resultadoSys = null!;
                        await AnsiConsole.Status()
                                .Spinner(Spinner.Known.Dots)
                                .StartAsync($"Consultando maquina de [yellow]{hostname}...[/]", async ctx =>
                                {
                                    // O serviço executa as chamadas de rede e retorna a entidade preenchida
                                    resultadoSys = await sysAuditorService.AuditSystemAsync();

                                    // Gravação isolada do arquivo de log utilizando os dados diretamente da memória (RAM)
                                    if (resultadoSys != null)
                                    {
                                        await registerFileService.RegisterSysAuditor(resultadoSys, dirFormatadoSys);
                                    }
                                });

                        var tableSys = new Table()
                                .Border(TableBorder.Rounded)
                                .Title($"[bold yellow]Resultado da Auditoria do Sistema: {resultadoSys.HostNameMachine}[/]")
                                .AddColumns(
                                    "[bold white]Verificação[/]",
                                    "[bold white]Resultado[/]"
                                );
                        tableSys.AddRow("Nome da Máquina", resultadoSys.HostNameMachine.EscapeMarkup());    
                        tableSys.AddRow("Versão do Sistema Operacional", resultadoSys.OSVersion.EscapeMarkup());
                        tableSys.AddRow("Uso da CPU", resultadoSys.CPUusage.EscapeMarkup());
                        tableSys.AddRow("Uso da Memória RAM", resultadoSys.RAMusage.EscapeMarkup());
                        tableSys.AddRow("Uso do Disco", resultadoSys.DiskUsage.EscapeMarkup());
                        tableSys.AddRow("Processos Ativos", resultadoSys.ActiveProcesses.EscapeMarkup());
                        tableSys.AddRow("Interface de Rede", resultadoSys.NetworkInterface.EscapeMarkup());
                        tableSys.AddRow("Tempo de Atividade do Sistema", resultadoSys.SystemUptime.EscapeMarkup());
                        tableSys.AddRow("Status de Conformidade", resultadoSys.ComplianceStatus.EscapeMarkup());
                        tableSys.AddRow("Mensagem do Diagnóstico", resultadoSys.Message.EscapeMarkup());
                        tableSys.AddRow("Timestamp", resultadoSys.Timestamp.EscapeMarkup());

                        string statusCor = resultadoSys.ComplianceStatus.Contains("ALERTA") ? "yellow" : "green";
                        tableSys.AddRow("Status de Conformidade", $"[{statusCor}]{resultadoSys.ComplianceStatus.EscapeMarkup()}[/]");

                        tableSys.AddRow("Mensagem do Diagnóstico", resultadoSys.Message.EscapeMarkup());
                        tableSys.AddRow("Timestamp", resultadoSys.Timestamp.EscapeMarkup());

                        AnsiConsole.Write(tableSys);

                        AnsiConsole.MarkupLine("[bold green]Auditoria do Sistema finalizada com sucesso![/]\n");

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
