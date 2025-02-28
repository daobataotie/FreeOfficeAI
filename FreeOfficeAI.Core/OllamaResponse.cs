using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FreeOfficeAI.Core
{
    public class OllamaResponse
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("created_at")]
        public string Created_at { get; set; }

        [JsonPropertyName("message")]
        public OllamaMessage Message { get; set; }

        [JsonPropertyName("response")]
        public string Response { get; set; }

        [JsonPropertyName("done")]
        public bool Done { get; set; }

        /// <summary>
        /// 用于此响应的对话编码，可以在下一个请求中发送以保持对话记忆
        /// </summary>
        [JsonPropertyName("context")]
        public int[] Context { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}
