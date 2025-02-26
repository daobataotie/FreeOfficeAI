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
        private static readonly string ollamaApiUrl = "http://10.6.100.112:11434/api";  // "http://localhost:11434/api";
        private static readonly string ollamaModelName = "deepseek-r1:14b";  //"codellama:34b";  // "deepseek-r1:1.5b";
        private static readonly string ollamaModelName_Excel = "qwen2.5-coder:32b";
        private static readonly HttpClient client = new HttpClient();

        public static async Task<string> GetResponseAsync(string prompt)
        {
            var requestData = new
            {
                model = ollamaModelName,
                prompt = prompt,
                stream = false  // 设置为true可启用流式响应
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ollamaApiUrl + "/generate", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(responseContent);
            if (ollamaResponse?.response == null)
                return responseContent;

            return ollamaResponse.response.Trim();
        }

        public static async Task<(bool, string)> GetVBAResponseAsync(string prompt)
        {
            var requestData = new
            {
                //model = ollamaModelName,
                model = ollamaModelName_Excel,
                prompt = prompt,
                stream = false  // 设置为true可启用流式响应
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ollamaApiUrl + "/generate", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(responseContent);
            if (ollamaResponse?.response == null)
                return (false, "抱歉，出错了！");

            if (ollamaResponse.response.Contains("</think>"))
            {
                ollamaResponse.response = ollamaResponse.response.Substring(ollamaResponse.response.IndexOf("</think>") + 8);
            }

            return (true, ollamaResponse.response.Trim());
        }

        public static async IAsyncEnumerable<string> GetResponseStreamAsync(string prompt)
        {
            var requestData = new
            {
                model = ollamaModelName,
                prompt = prompt,
                stream = true  // 设置为true可启用流式响应
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await client.PostAsync(ollamaApiUrl + "/generate", content);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new System.IO.StreamReader(stream);

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    // 解析每块 JSON 数据
                    string chunk = null;
                    try
                    {
                        var jsonDoc = JsonDocument.Parse(line);
                        if (jsonDoc.RootElement.TryGetProperty("response", out var responseProp))
                        {
                            chunk = responseProp.GetString().Replace("\n", "\r\n");
                        }

                        else if (jsonDoc.RootElement.TryGetProperty("error", out var errorProp))  // 检查是否报错
                        {
                            chunk = errorProp.GetString();
                            yield break;
                        }

                        if (jsonDoc.RootElement.TryGetProperty("done", out var doneProp) && doneProp.GetBoolean())  // 检查是否结束
                        {
                            yield break;
                        }
                    }
                    catch (JsonException ex)
                    {
                        chunk = "[解析错误] " + ex.Message;
                        yield break;
                    }
                    if (chunk != null)
                    {
                        yield return chunk;
                    }
                }
            }
        }

        public static async IAsyncEnumerable<string> GetResponseStreamAsync(string systemContent, string userContent)
        {
            var requestData = new
            {
                model = ollamaModelName,
                messages = new[]
                {
                    new { role = "system", content = systemContent },
                    new { role = "user", content = userContent }
                },
                stream = true  // 设置为true可启用流式响应
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await client.PostAsync(ollamaApiUrl + "/chat", content);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new System.IO.StreamReader(stream);

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    // 解析每块 JSON 数据
                    string chunk = null;
                    try
                    {
                        var jsonDoc = JsonDocument.Parse(line);
                        if (jsonDoc.RootElement.TryGetProperty("message", out var responseProp))
                        {
                            if (responseProp.TryGetProperty("content", out var responseContent))
                            {
                                chunk = responseContent.GetString().Replace("\n", "\r\n");
                            }
                        }

                        else if (jsonDoc.RootElement.TryGetProperty("error", out var errorProp))  // 检查是否报错
                        {
                            chunk = errorProp.GetString();
                            yield break;
                        }

                        if (jsonDoc.RootElement.TryGetProperty("done", out var doneProp) && doneProp.GetBoolean())  // 检查是否结束
                        {
                            yield break;
                        }
                    }
                    catch (JsonException ex)
                    {
                        chunk = "[解析错误] " + ex.Message;
                        yield break;
                    }
                    if (chunk != null)
                    {
                        yield return chunk;
                    }
                }
            }
        }


        public static async IAsyncEnumerable<string> GetResponseStreamAsync_VBA(string systemContent, string userContent)
        {
            var requestData = new
            {
                model = ollamaModelName_Excel,
                messages = new[]
                {
                    new { role = "system", content = systemContent },
                    new { role = "user", content = userContent }
                },
                stream = true  // 设置为true可启用流式响应
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await client.PostAsync(ollamaApiUrl + "/chat", content);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new System.IO.StreamReader(stream);

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    // 解析每块 JSON 数据
                    string chunk = null;
                    try
                    {
                        var jsonDoc = JsonDocument.Parse(line);
                        if (jsonDoc.RootElement.TryGetProperty("message", out var responseProp))
                        {
                            if (responseProp.TryGetProperty("content", out var responseContent))
                            {
                                chunk = responseContent.GetString().Replace("\n", "\r\n");
                            }
                        }

                        else if (jsonDoc.RootElement.TryGetProperty("error", out var errorProp))  // 检查是否报错
                        {
                            chunk = errorProp.GetString();
                            yield break;
                        }

                        if (jsonDoc.RootElement.TryGetProperty("done", out var doneProp) && doneProp.GetBoolean())  // 检查是否结束
                        {
                            yield break;
                        }
                    }
                    catch (JsonException ex)
                    {
                        chunk = "[解析错误] " + ex.Message;
                        yield break;
                    }
                    if (chunk != null)
                    {
                        yield return chunk;
                    }
                }
            }
        }
    }

    public class OllamaResponse
    {
        public string model { get; set; }
        public string created_at { get; set; }

        /// <summary>
        /// 完整的响应
        /// </summary>
        public string response { get; set; }
        public bool done { get; set; }

        /// <summary>
        /// 用于此响应的对话编码，可以在下一个请求中发送以保持对话记忆
        /// </summary>
        public int[] context { get; set; }
    }
}
