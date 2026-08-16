using Application.Service;
using Core.Models;
using Spectre.Console;
using System.Runtime.CompilerServices;

namespace Sentinela
{
    public class Scanner
    {

        private readonly ScannerWebService scannerWebService;
        private readonly ScannerService scannerService;

        public Scanner(ScannerWebService scannerWebService, ScannerService scannerService)
        {
            this.scannerWebService = scannerWebService;
            this.scannerService = scannerService;
        }

        public async Task RunAsync()
        {
            await scannerWebService.ScannerWebApplication();
            await scannerService.ScannerToolAsync();
        }

        public static async Task Main(string[] args)
        {
            var scannerWebService = new ScannerWebService();
            var scannerService = new ScannerService(
                new logAddressService(),
                new jsonAddressService(),
                new ReadFileServiceImpl(),
                new RegisterFileService()
            );
            var scanner = new Scanner(scannerWebService, scannerService);
            await scanner.RunAsync();






        }
    }
}
