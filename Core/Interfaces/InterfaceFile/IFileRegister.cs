using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.InterfaceFile
{
    public  interface IFileRegister
    {
      public Task RegisterLogAsync(string fileName, string filePath, string originalFile);
      public Task RegisterJsonAsync(string originalFile, string fileName, string outPutDirectory);
    }
}
