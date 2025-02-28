using FreeOfficeAI.Core;
using FreeOfficeAI.UI.CustomControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeOfficeAI.UI.UserControls
{
    public partial class UCTask : UCBase
    {
        public UCTask(Action<string> insertOffice, Func<string> getContent)
        {
            InitializeComponent();

            insertToOffice = insertOffice;
            getWordContent = getContent;
        }

        public void Send(string systemContent, string userContent, string functionType)
        {
            if (!done)
                return;

            AddMessage(functionType, true); // 自己发出的消息
            UpdateMessage(functionType, false); // 更新消息，主要是为了计算控件高度

            //获取Word文档内容
            string wordContent = getWordContent();
            if (string.IsNullOrWhiteSpace(wordContent))
            {
                AddMessage("空数据！", false);
                return;
            }

            userContent += wordContent;

            //点击Word Ribbon按钮时，清空之前的消息
            request = new OllamaRequest()
            {
                Messages = new List<OllamaMessage>()
                {
                    new OllamaMessage { Role = "system", Content = systemContent },
                    new OllamaMessage { Role = "user", Content = userContent },
                },
            };

            GetResponse();
        }

        protected override void SendRequest()
        {
            if (!done)
                return;

            string message = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(message))
                return;

            AddMessage(message, true); // 自己发出的消息
            UpdateMessage(message, false); // 更新消息，主要是为了计算控件高度

            txtInput.Text = "";
            txtInput.SelectionStart = 0;
            txtInput.Focus();

            //第一次请求，生成系统消息和用户消息
            if (request.Messages == null || request.Messages.Count == 0)
            {
                request.Messages = new List<OllamaMessage>()
                {
                    new OllamaMessage { Role = "user", Content = message },
                };
            }
            else
                request.Messages.Add(new OllamaMessage
                {
                    Role = "user",
                    Content = message
                });    // 后续请求，追加用户消息

            GetResponse();
        }

        private void GetResponse()
        {
            try
            {
                done = false;
                btnSend.Enabled = false;

                AddMessage("思考中...", false); // 返回的消息
                Task.Run(async () =>
                {
                    try
                    {
                        // 读取流式响应
                        await foreach (var responsePart in OllamaApi.GetChatStreamAsync(request))
                        {
                            UpdateMessage(responsePart);
                        }

                        AddInsertBtn("插入文档");
                    }
                    catch (Exception ex)
                    {
                        if (ex is TaskCanceledException tce && tce.CancellationToken.IsCancellationRequested)
                            UpdateMessage("请求超时，请重试！", false);
                        else
                            UpdateMessage(ex.Message, false);
                    }
                    finally
                    {
                        done = true;

                        if (btnSend.InvokeRequired)
                            btnSend.Invoke(new Action(() =>
                            {
                                btnSend.Enabled = true;
                            }));
                        else
                            btnSend.Enabled = true;
                    }
                });

            }
            catch (Exception ex)
            {
                UpdateMessage(ex.Message, false);
            }
        }
    }
}
