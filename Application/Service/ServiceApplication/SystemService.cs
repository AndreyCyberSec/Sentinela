using Application.InterfacesService.InterfaceScanner;
using Application.InterfacesService.InterfaceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.ServiceApplication
{
    public class SystemService : ISystem
    {
        private readonly IScannerService scannerService;
        private readonly IScannerWebService service;

        public SystemService(IScannerService scannerService, IScannerWebService service)
        {
            this.scannerService = scannerService;
            this.service = service;
        }
        public async Task RunAsync()
        {
            await service.ScannerWebApplication();
            await scannerService.ScannerToolAsync();
        }
    }
}
