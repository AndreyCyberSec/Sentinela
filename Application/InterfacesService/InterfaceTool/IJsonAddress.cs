using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesService.InterfaceTool
{
    public interface IJsonAddress
    {
        Task<List<JsonEntity?>> ReadJsonAsync(string filePath);
    }
}
