using Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesService.InterfaceTool
{
    public interface IDnsEntityService
    {
        public Task<DnsEntity> AuditDomainAsync(string hostname, CancellationToken cancellationToken=default);

    }
}
