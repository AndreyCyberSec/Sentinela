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
using Core.InterfacesService.InterfaceReadOnlySpan;
using Application.Service.ServiceReadOnly;
using Core.Interfaces.InterfaceFile;
using Application.InterfacesService.InterfaceReadOnlySpan;

namespace Sentinela
{
    public class Scanner
    {
       public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddSingleton<IFileReader, ReadFileServiceImpl>();
            services.AddSingleton<IFileRegister, RegisterFileService>();
            services.AddSingleton<IReadEndPointOnlySpanLog, GetEndpointSpanService>();
            services.AddSingleton<IReadIpOnlySpan, GetIpAddresSpanService>();
            services.AddSingleton<IScannerWebService, ScannerWebService>();
            services.AddSingleton<IScannerService, ScannerService>();
            services.AddScoped<ISystem, SystemService>();

            await using var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<ISystem>();
            await app.RunAsync();
        } 
       
    }
}
