using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesService.InterfaceTool
{
    public interface ICheckCA
    {
        public Task<CheckCAEntity> GetHostAsync(string hostName, int port);
        public Task<CheckCAEntity> CheckSslAsync(string hostName, int port = 443, CancellationToken cancellationToken = default);
    }
}
