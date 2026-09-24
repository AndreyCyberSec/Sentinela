using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public record CheckCAEntity
    {
        public string HostName { get; set; } = string.Empty;
        public int Port { get; set; } = 443;
        public string Issuer { get; set; } = string.Empty;           
        public string Subject { get; set; } = string.Empty;          
        public DateTime EffectiveDate { get; set; }                  
        public DateTime ExpirationDate { get; set; }                 
        public int DaysRemaining { get; set; }                       
        public string SignatureAlgorithm { get; set; } = string.Empty;
        public string Protocol { get; set; } = string.Empty;         
        public bool IsValid { get; set; }                            
        public string Status { get; set; } = string.Empty;           
        public string Message { get; set; } = string.Empty;
        public string Timestamp { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");

        public CheckCAEntity() { }

        public CheckCAEntity(string hostName, int port, string issuer, string subject, DateTime effectiveDate, DateTime expirationDate, int daysRemaining, string signatureAlgorithm, string protocol, bool isValid, string status, string message)
        {
            HostName = hostName;
            Port = port;
            Issuer = issuer;
            Subject = subject;
            EffectiveDate = effectiveDate;
            ExpirationDate = expirationDate;
            DaysRemaining = daysRemaining;
            SignatureAlgorithm = signatureAlgorithm;
            Protocol = protocol;
            IsValid = isValid;
            Status = status;
            Message = message;
        }

        public CheckCAEntity(string hostName, int port)
        {
            HostName = hostName;
            Port = port;
        }
    }
}
