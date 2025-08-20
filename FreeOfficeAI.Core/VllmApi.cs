using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace FreeOfficeAI.Core
{
    public class VllmApi
    { /// <summary>
      /// 测试与OpenAI API的连接
      /// </summary>
      /// <param name="apiUrl">API基础URL</param>
      /// <param name="apiKey">OpenAI API密钥</param>
      /// <param name="modelName">模型名称</param>
      /// <param name="cancellationToken">取消令牌</param>
      /// <returns>连接是否成功</returns>
        public static async Task<bool> TestConnection(string apiUrl, string modelName, CancellationToken cancellationToken)
        {
            var requestData = new
            {
                model = modelName,
                messages = new[]
                {
                    new { role = "user", content = "Test" }
                },
                max_tokens = 5
            };

            var json = JsonSerializer.Serialize(requestData);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 创建请求消息
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{apiUrl}/v1/chat/completions")
            {
                Content = content
            };

            var client = CreateHttpClient(60);
            using var response = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return true;
        }

        /// <summary>
        /// 获取生成结果
        /// </summary>
        /// <param name="apiUrl">API基础URL</param>
        /// <param name="modelName">模型名称</param>
        /// <param name="prompt">提示文本</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="isStream">是否使用流式响应</param>
        /// <returns>生成的文本响应</returns>
        public static async Task<string> GetCompletionAsync(string apiUrl, string modelName, string prompt, CancellationToken cancellationToken, bool isStream = false)
        {
            using HttpResponseMessage response = await GetResponseCompletion(apiUrl, modelName, prompt, isStream, cancellationToken);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var openAIResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseContent);

            if (openAIResponse?.choices == null || openAIResponse.choices.Count == 0)
                return responseContent;

            return openAIResponse.choices[0].message.content.Trim();
        }

        /// <summary>
        /// 获取聊天完成响应
        /// </summary>
        //private static async Task<HttpResponseMessage> GetResponseCompletion(string apiUrl, string modelName, string prompt, bool isStream, CancellationToken cancellationToken)
        //{
        //    var requestData = new
        //    {
        //        model = modelName,
        //        prompt = prompt,
        //        stream = isStream,
        //        temperature = 0,
        //        max_tokens = 8000,
        //    };

        //    var json = JsonConvert.SerializeObject(requestData);
        //    using var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    // 创建请求消息
        //    using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{apiUrl}/v1/completions")
        //    {
        //        Content = content
        //    };

        //    var client = CreateHttpClient(200);
        //    var response = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        //    response.EnsureSuccessStatusCode();

        //    return response;
        //}

        private static async Task<HttpResponseMessage> GetResponseCompletion(string apiUrl, string modelName, string prompt, bool isStream, CancellationToken cancellationToken)
        {
            var requestData = new
            {
                model = modelName,
                messages = new List<OpenAIMessage>
                {
                   new() { role = "user", content = prompt }
                },
                stream = isStream,
                temperature = 0,
                max_tokens = 8000,
            };

            var json = JsonSerializer.Serialize(requestData);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 创建请求消息
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{apiUrl}/v1/chat/completions")
            {
                Content = content
            };

            var client = CreateHttpClient(200);
            var response = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            return response;
        }

        /// <summary>
        /// 创建配置好的HttpClient
        /// </summary>
        /// <param name="timeout">超时时间（秒）</param>
        /// <returns>配置好的HttpClient</returns>
        private static HttpClient CreateHttpClient(int timeout)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer ");
            client.Timeout = TimeSpan.FromSeconds(timeout);
            return client;
        }
    }
    /// <summary>
    /// OpenAI API响应对象
    /// </summary>
    public class OpenAIResponse
    {
        /// <summary>
        /// 响应ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 对象类型
        /// </summary>
        public string @object { get; set; }

        /// <summary>
        /// 创建时间戳
        /// </summary>
        public long created { get; set; }

        /// <summary>
        /// 使用的模型
        /// </summary>
        public string model { get; set; }

        /// <summary>
        /// 系统指纹
        /// </summary>
        public string system_fingerprint { get; set; }

        /// <summary>
        /// 选择结果列表
        /// </summary>
        public List<OpenAIChoice> choices { get; set; }

        /// <summary>
        /// 使用情况统计
        /// </summary>
        public OpenAIUsage usage { get; set; }
    }

    /// <summary>
    /// OpenAI选择结果
    /// </summary>
    public class OpenAIChoice
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int index { get; set; }

        public string text { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public OpenAIMessage message { get; set; }

        /// <summary>
        /// 结束原因
        /// </summary>
        public string finish_reason { get; set; }
    }

    /// <summary>
    /// OpenAI消息
    /// </summary>
    public class OpenAIMessage
    {
        /// <summary>
        /// 角色（system, user, assistant）
        /// </summary>
        public string role { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { get; set; }
    }

    /// <summary>
    /// OpenAI使用情况统计
    /// </summary>
    public class OpenAIUsage
    {
        /// <summary>
        /// 提示使用的令牌数
        /// </summary>
        public int prompt_tokens { get; set; }

        /// <summary>
        /// 完成使用的令牌数
        /// </summary>
        public int completion_tokens { get; set; }

        /// <summary>
        /// 总共使用的令牌数
        /// </summary>
        public int total_tokens { get; set; }
    }
}
