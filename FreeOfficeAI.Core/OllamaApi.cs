using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeOfficeAI.Core
{
    public class OllamaApi
    {
        private static readonly HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(30) };

        /// <summary>
        /// 获取生成结果(非流式)
        /// </summary>
        /// <param name="request">Ollama请求对象(Prompt)</param>
        /// <returns></returns>
        public static async Task<string> GetGenerateAsync(OllamaRequest request)
        {
            try
            {
                HttpResponseMessage response = await GetResponseGenerate(request, false);

                var responseContent = await response.Content.ReadAsStringAsync();
                var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(responseContent);

                if (ollamaResponse?.Response == null)
                    return responseContent;

                return ollamaResponse.Response.Trim();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取生成结果(流式)
        /// </summary>
        /// <param name="request">Ollama请求对象(Prompt)</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<string> GetGenerateStreamAsync(OllamaRequest request)
        {
            HttpResponseMessage response = await GetResponseGenerate(request, true);

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new System.IO.StreamReader(stream);

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    // 解析每块 JSON 数据
                    string chunk = null;
                    var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(line);

                    if (ollamaResponse != null)
                    {
                        if (ollamaResponse.Response != null)
                        {
                            chunk = ollamaResponse.Response.Replace("\n", "\r\n");
                            yield return chunk;
                        }
                        else if (ollamaResponse.Error != null)  // 检查是否报错
                        {
                            chunk = ollamaResponse.Error;
                            yield return chunk;

                            break;
                        }

                        if (ollamaResponse.Done)
                        {
                            request.Context = ollamaResponse.Context;
                            yield break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取聊天结果(流式)
        /// </summary>
        /// <param name="request">Ollama请求对象(Messages)</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<string> GetChatStreamAsync(OllamaRequest request)
        {
            using HttpResponseMessage response = await GetResponseChat(request);

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new System.IO.StreamReader(stream);

            StringBuilder sb = new StringBuilder();

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    // 解析每块 JSON 数据
                    string chunk = null;
                    var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(line);

                    if (ollamaResponse != null)
                    {
                        if (ollamaResponse.Message != null && ollamaResponse.Message.Content != null)
                        {
                            chunk = ollamaResponse.Message.Content.Replace("\n", "\r\n");
                            sb.Append(chunk);

                            yield return chunk;
                        }
                        else if (ollamaResponse.Error != null)  // 检查是否报错
                        {
                            chunk = ollamaResponse.Error;
                            sb.Append(chunk);

                            yield return chunk;

                            break;
                        }

                        if (ollamaResponse.Done)
                        {
                            request.Messages.Add(new OllamaMessage { Role = ollamaResponse.Message.Role, Content = sb.ToString() });
                            yield break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="apiUrl">地址</param>
        /// <param name="modelName">模型名</param>
        /// <returns></returns>
        public static async Task<bool> TestConnection(string apiUrl, string modelName)
        {
            var requestData = new
            {
                model = modelName,
                prompt = "Test",
                stream = false
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl + "/api/generate", content);
            response.EnsureSuccessStatusCode();

            // 释放资源
            content.Dispose();
            response.Dispose();

            return true;
        }


        private static async Task<HttpResponseMessage> GetResponseGenerate(OllamaRequest request, bool isStream = true)
        {
            var requestData = new
            {
                model = ConfigSetting.GetSetting().ModelName,
                prompt = request.Prompt,
                context = request.Context,
                stream = isStream
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ConfigSetting.GetSetting().ApiUrl + "/api/generate", content);
            response.EnsureSuccessStatusCode();
            return response;
        }

        private static async Task<HttpResponseMessage> GetResponseChat(OllamaRequest request)
        {
            var requestData = new
            {
                model = ConfigSetting.GetSetting().ModelName,
                messages = request.Messages,
                stream = true
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(ConfigSetting.GetSetting().ApiUrl + "/api/chat", content);
            response.EnsureSuccessStatusCode();
            return response;
        }

    }
}
