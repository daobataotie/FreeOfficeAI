using FreeOfficeAI.Core;
using FreeOfficeAI.UI.CustomControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeOfficeAI.UI.UserControls
{
    //用于Excel表格公式处理还有问题，暂时不建议使用

    public partial class UCTool : UCBase
    {
        private static List<string> keyWordsSelection = new List<string>() { "选", "点击" };
        private static List<string> keyWordsAll = new List<string>() { "整个", "全部", "整张", "所有" };

        public UCTool(Action<string> insertOffice, Func<string, bool> exeVBA, Func<bool, string> getContent)
        {
            InitializeComponent();

            rdoBtnChat.Checked = true;  //默认对话模式

            insertToOffice = insertOffice;
            executeVBA = exeVBA;
            getExcelContent = getContent;
        }


        protected override void SendRequest()
        {
            //todo VBA 报错处理

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
                        bool isChat = rdoBtnChat.Checked;
                        if (isChat)
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

                            AddInsertBtn("插入文档", "0");
                        }
                        else
                        {
                            var ollamaMessage = request.Messages;
                            if (ollamaMessage == null || ollamaMessage.Count == 0)
                            {
                                bool getSelectionExcel = false;  // 默认读取全部Excel数据
                                if (/*!keyWordsAll.Any(k => message.Contains(k)) && */keyWordsSelection.Any(k => message.Contains(k)))
                                    getSelectionExcel = true;

                                var excelContent = getExcelContent(getSelectionExcel);
                                message += "，Excel数据：" + excelContent;

                                string systemContent = "你是一位Excel专家，擅长表格公式、图表、统计分析、数据处理等方面，以及VBA代码编写。你可以根据用户的描述生成可执行的完整VBA代码，用于操作Excel工作表，进行插入公式、修改/查询/统计内容、生成图表、分析数据等。";
                                string userContent = $"请针对以下功能写一个完整的可执行VBA代码:{message}。\n注意：1，VBA方法名为“FreeOfficeAIExample”，且名称下一行为固定内容“On Error Resume Next”表示忽略错误；2，并在方法之前添加“FreeOfficeAIStart”关键字用以表示代码段开始，方法结尾添加“FreeOfficeAIEnd”关键字用以表示代码段结束；3，考虑性能问题，优化代码提高运行效率；4，VBA方法应以“End Sub”结尾；5，检查生成的VBA代码是否存在语法错误并修正。";

                                //第一次请求，生成系统消息和用户消息
                                ollamaMessage = new List<OllamaMessage>()
                                {
                                    new OllamaMessage { Role = "system", Content = systemContent },
                                    new OllamaMessage { Role = "user", Content = userContent },
                                };
                            }
                            else
                                ollamaMessage.Add(new OllamaMessage { Role = "user", Content = message });    // 后续请求，追加用户消息

                            request = new OllamaRequest()
                            {
                                Messages = ollamaMessage,
                            };

                            // 读取流式响应
                            await foreach (var responsePart in OllamaApi.GetChatStreamAsync(request))
                            {
                                UpdateMessage(responsePart);
                            }

                            //在连续对话时，后续对话可能没有包含VBA代码
                            if (lastControl.Text.Contains("FreeOfficeAIExample"))
                                AddInsertBtn("应用公式", "1");
                            else
                                AddInsertBtn("插入文档", "0");
                        }
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

        protected override void ExecuteInsert(object tag)
        {
            string[] strs = tag as string[];
            string content = strs[0];
            string type = strs[1];

            if (!string.IsNullOrWhiteSpace(content))
            {
                //如果包含“</think>”,则只截取后面内容
                if (content.Contains("</think>"))
                {
                    content = content.Substring(content.IndexOf("</think>") + 8);
                }

                content = content.Trim();

                if (type == "0")
                {
                    insertToOffice(content);
                }
                else
                {
                    if (content.Contains("FreeOfficeAIStart") && content.Contains("FreeOfficeAIEnd"))
                    {
                        if (MessageBox.Show("1，通过VBA操作Excel会导致更改不可撤销，请保存原始文档以防发生意外。\r\n2，该功能目前还处于测试阶段，可能存在一些问题，请谨慎使用。\r\n3，应用公式发生错误时，需手动关闭VBA编辑器。\r\n\r\n是否应用该公式？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            string vbaCode = content.Substring(content.IndexOf("FreeOfficeAIStart") + 17, content.IndexOf("FreeOfficeAIEnd") - content.IndexOf("FreeOfficeAIStart") - 17).Trim();

                            //再次过滤代码段，只保留 Sub 和 End Sub 之间的代码
                            int subIndex = vbaCode.ToLower().IndexOf("sub");
                            int endSubIndex = vbaCode.ToLower().LastIndexOf("sub");
                            if (subIndex >= 0 && endSubIndex > subIndex)
                            {
                                vbaCode = vbaCode.Substring(subIndex, endSubIndex - subIndex + 3);
                            }

                            var success = executeVBA(vbaCode);
                        }
                    }
                    else
                    {
                        AddMessage("公式有误，请检查！", false);
                    }
                }
            }
        }
    }
}
