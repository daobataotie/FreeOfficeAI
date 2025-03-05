using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FreeOfficeAI.Core
{
    internal class OllamaTags
    {
        [JsonPropertyName("models")]
        public OllamaModel[] Models { get; set; }
    }

    internal class OllamaModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("modified_at")]
        public string ModifiedAt { get; set; }
        [JsonPropertyName("size")]
        public long Size { get; set; }
    }
}
