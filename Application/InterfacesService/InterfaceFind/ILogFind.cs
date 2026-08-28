using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesService.InterfaceFind
{
    public interface ILogFind
    {
        public  Dictionary<string, string?> TopIpaddress(List<LogEntity> logsLidos);

        public List<LogEntity> TopEndpoint(List<LogEntity> logsLidos);
    }
}
