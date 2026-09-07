using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesService.InterfaceTool
{
    public interface INetScannerService
    {
        public Task<NetScannerEntity> PortCheck(string hostName, int port, int timeOutMs, CancellationToken cancellationToken= default);
    }
}
