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
        [JsonPropertyName("timestamp")]
        public string TimeStamp { get; init; }
        [JsonPropertyName("event_id")]
        public int EventId { get; init; }
        [JsonPropertyName("severity")]
        public string Severity { get; init; }
        [JsonPropertyName("source")]
        public string Source { get; init; }
        [JsonPropertyName("user")]
        public string User { get; init; }
        [JsonPropertyName("ip_address")]
        public string IpAddress { get; init; }
        [JsonPropertyName("message")]
        public string Message { get; init; }
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
