using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesService.InterfaceTool
{
    public interface IEgineEnv
    {
        public Task<string> GetEnv(string file);
       public byte[] Encrypt(string plaintext, string passoword);
       public string Decrypt(byte[] encryptedPayload, string password);
        
       public byte[] NewPassword(string password, byte[] salt);

       


    }
}
