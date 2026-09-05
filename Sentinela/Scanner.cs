using Application.Service;
using Application.Service.ServiceFile;
using Application.Service.ServiceDesign;
using Core.Models;
using Spectre.Console;
using System.Runtime.CompilerServices;
using Application.Service.ServiceApplication;
using Application.InterfacesService.InterfaceSystem;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.DependencyInjection;
using Application.InterfacesService.InterfaceScanner;
using Application.Service.ServiceReadOnly;
using Core.Interfaces.InterfaceFile;
using Application.InterfacesService.InterfaceReadOnlySpan;
using Application.InterfacesService.InterfaceTool;
using Application.Service.ServiceTool;
using Application.InterfacesService.InterfaceFind;
using Application.Service.ServiceFind;

namespace Sentinela
{
    public class Scanner
    {
       public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddSingleton<IFileReader, ReadFileServiceImpl>();
            services.AddSingleton<IJsonAddress, jsonAddressService>();
            services.AddSingleton<ILogAddress, logAddressService>();
            services.AddSingleton<ILogFind, LogFindService>();
            services.AddSingleton<IFileRegister, RegisterFileService>();
            services.AddSingleton<IReadOnlySpan, GetEndpointSpanService>();
            services.AddSingleton<IReadOnlySpan, GetIpAddresSpanService>();
            services.AddSingleton<IEgineEnv, EngineEnvService>();
            services.AddSingleton<IScannerWebService, ScannerWebService>();
            services.AddSingleton<IScannerService, ScannerService>();
            services.AddScoped<ISystem, SystemService>();

            await using var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<ISystem>();
            await app.RunAsync();
        } 
       
    }
}
