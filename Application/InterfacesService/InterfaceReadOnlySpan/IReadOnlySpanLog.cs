using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.InterfacesService.InterfaceReadOnlySpan
{
    public interface IReadOnlySpanLog
    {
        public LogEntity OnlySpan(string line); 
    }
}
