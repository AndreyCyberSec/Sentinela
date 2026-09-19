using Core.Models;
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

        public Task RegisterEnvAsync(string filePath,string fileName,string password, string? outPutDirectory = null);
        public Task<string> DecryptEnvAsync(string fileEnv, string password);
        public Task CheckPortResult(IReadOnlyList<NetScannerEntity> scanResults, string? outPutDirectory = null);

        public Task CheckDnsResult(IReadOnlyList<DnsEntity> dnsResults, string? outPutDirectory = null);

    }
}
