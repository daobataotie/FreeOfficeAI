using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FreeOfficeAI.Core
{
    public class OllamaMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
