using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.InterfaceFile
{
    public interface IFileReader
    {
        public bool CanReadFile(string filePath);

        public Task ReadFileAsync(string filePath);
        public Task ReadFileJsonAsync(string filePath);
        public Task ReadLogAsync(string filePath);
        public Task ReadTxtAsync(string filePath);
        public Task ReadCsvAync(string filePath);

    }
}
