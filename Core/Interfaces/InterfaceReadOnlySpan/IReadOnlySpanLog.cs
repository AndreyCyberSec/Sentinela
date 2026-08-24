using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.InterfaceReadOnlySpan
{
    public interface IReadOnlySpanLog
    {
        public LogEntity OnlySpan(string line);
    }
}
