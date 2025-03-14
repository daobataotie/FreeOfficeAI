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
    public partial class UCChat : UCBase
    {
        public UCChat(Action<string> insertOffice)
        {
            InitializeComponent();

            insertToOffice = insertOffice;
        }

        protected override void SendRequest()
        {
            if (!done)
                return;

            string message = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(message))
                return;

            AddMessage(message, true); // 自己发出的消息
            UpdateMessage(message, false);

            txtInput.Text = "";
            txtInput.SelectionStart = 0;
            txtInput.Focus();

            done = false;
            btnSend.Enabled = false;
            btnClean.Enabled = false;

            try
            {
                AddMessage("思考中...", false); // 返回的消息
                Task.Run(async () =>
                {
                    try
                    {
                        //第一次请求，创建用户消息。后续请求，追加用户消息
                        if (request.Messages == null || request.Messages.Count == 0)
                            request.Messages = new List<OllamaMessage>()
                            {
                                new OllamaMessage { Role = "user", Content = message },
                            };
                        else
                            request.Messages.Add(new OllamaMessage
                            {
                                Role = "user",
                                Content = message
                            });

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
                        UpdateStatus();
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
