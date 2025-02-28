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

            try
            {
                AddMessage("思考中...", false); // 返回的消息
                Task.Run(async () =>
                {
                    try
                    {
                        request = new OllamaRequest()
                        {
                            Prompt = message,
                            Context = request.Context
                        };

                        // 读取流式响应
                        await foreach (var responsePart in OllamaApi.GetGenerateStreamAsync(request))
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
