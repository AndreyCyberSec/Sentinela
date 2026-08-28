using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesService.InterfaceTool
{
    public interface ILogAddress
    {
        Task<Dictionary<string, string?>> GetIpAddressAsync(string filePath);

        Task<List<LogEntity>> GetEndPointAsync(string filePath);
    }
}
