using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesService.InterfaceReadOnlySpan
{
    public interface IReadIpOnlySpan
    {
        public LogEntity OnlySpan(string line);
    }
}
