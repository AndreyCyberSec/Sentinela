using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models
{
    public record class JsonEntity
    {
        [JsonPropertyName("timestamamp")]
        public string TimeStamp { get; }
        [JsonPropertyName("event_id")]
        public int EventId { get; }
        [JsonPropertyName("severity")]
        public string Severity { get; }
        [JsonPropertyName("source")]
        public string Source { get; }
        [JsonPropertyName("user")]
        public string User { get; }
        [JsonPropertyName("ip_address")]
        public string IpAddress { get; }
        [JsonPropertyName("message")]
        public string Message { get; }

        public List<string> GetLogDetails()
        {
            return new List<string> { TimeStamp, Severity, IpAddress, Message };
        }

        public JsonEntity() { }
        public JsonEntity(string timeStamp, int eventId, string severity, string source, string user, string ipAddress, string message)
        {
            TimeStamp = timeStamp;
            EventId = eventId;
            Severity = severity;
            Source = source;
            User = user;
            IpAddress = ipAddress;
            Message = message;
        }

        public JsonEntity(string timeStamp, string severity, string ipAddress, string message)
        {
            TimeStamp = timeStamp;
            Severity = severity;
            IpAddress = ipAddress;
            Message = message;
        }
    }
}
