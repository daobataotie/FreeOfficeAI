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
    public partial class UCTool : UCBase
    {
        Label lastControl = null;
        private bool done = true;

        private static List<string> keyWordsSelection = new List<string>() { "选", "点击", "部分" };
        private static List<string> keyWordsAll = new List<string>() { "整个", "全部", "整张", "所有" };

        public UCTool(Action<string> insertOffice, Func<string, bool> exeVBA, Func<bool, string> getContent)
        {
            InitializeComponent();

            splitContainerControl1.Panel2.BackColor = Color.White;
            panelMessage.BackColor = Color.LightCyan;
            panelMessage.AutoScroll = true;

            txtInput.KeyDown += TxtInput_KeyDown; // 绑定 KeyDown 事件
            btnSend.Click += SendButton_Click;

            rdoBtnChat.Checked = true;

            insertToOffice = insertOffice;
            executeVBA = exeVBA;
            getExcelContent = getContent;
        }

        /// <summary>
        /// 输入框 KeyDown 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            //当光标位于输入框末尾时，按下回车键发送消息
            if (e.KeyCode == Keys.Enter && txtInput.SelectionStart == txtInput.Text.Length)
            {
                SendButton_Click(null, null);

                e.SuppressKeyPress = true; // 防止默认的换行
                e.Handled = true; // 标记事件已处理，防止产生额外的换行
            }
        }

        private async void SendButton_Click(object sender, EventArgs e)
        {
            if (!done)
                return;

            string message = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(message))
                return;

            AddMessage(message, true); // 自己发出的消息
            LableUpdate(message, false);

            txtInput.Text = "";
            txtInput.SelectionStart = 0;
            txtInput.Focus();

            done = false;
            btnSend.Enabled = false;

            try
            {
                AddMessage("思考中...", false); // 返回的消息
                try
                {
                    bool isChat = rdoBtnChat.Checked;
                    if (isChat)
                    {
                        // 读取流式响应
                        await foreach (var responsePart in OllamaApi.GetResponseStreamAsync(message))
                        {
                            LableUpdate(responsePart);
                        }

                        AddInserBtn(isChat);
                    }
                    else
                    {
                        bool getSelectionExcel = true;
                        if (keyWordsAll.Any(k => message.Contains(k)) && !keyWordsSelection.Any(k => message.Contains(k)))
                            getSelectionExcel = false;

                        message += "，Excel数据：" + getExcelContent(getSelectionExcel);

                        string systemContent = "你是一位Excel专家，擅长表格公式、图表、统计分析、数据处理等方面，以及VBA代码编写。你可以根据用户的描述生成可执行的完整VBA代码，用于操作Excel工作表，进行插入公式、修改/查询/统计内容、生成图表、分析数据等。";
                        string userContent = $"请针对以下功能写一个完整的可执行VBA代码:{message}。\n注意：1，方法名为“FreeOfficeAIExample”，且名称下一行为固定内容“On Error Resume Next”表示忽略错误；2，并在方法之前添加“FreeOfficeAIStart”关键字用以表示代码段开始，方法结尾添加“FreeOfficeAIEnd”关键字用以表示代码段结束；3，考虑性能问题，优化代码提高运行效率；4，VBA方法应以“End Sub”结尾；5，检查生成的VBA代码是否存在语法错误并修正。";

                        // 读取流式响应
                        await foreach (var responsePart in OllamaApi.GetResponseStreamAsync(systemContent, userContent))
                        {
                            LableUpdate(responsePart);
                        }

                        AddInserBtn(isChat);
                    }

                    //var result = await OllamaApi.GetVBAResponseAsync(isChat ? message : prompt);

                    //LableUpdate(result.Item2, false);

                    //if (result.Item1)
                    //{
                    //    done = true;

                    //    AddInserBtn(isChat);
                    //}
                }
                catch (Exception ex)
                {
                    LableUpdate(ex.Message, false);
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

            }
            catch (Exception ex)
            {
                LableUpdate(ex.Message, false);
            }
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="isSelf">是否为自己发送的消息</param>
        private void AddMessage(string content, bool isSelf)
        {
            RoundedLabelControl messageLabel = new RoundedLabelControl();
            messageLabel.Text = content;
            messageLabel.BackColor = isSelf ? Color.FromArgb(137, 217, 97) : Color.Snow;
            messageLabel.Padding = new Padding(2);
            messageLabel.Font = new Font("微软雅黑", 10);
            messageLabel.ForeColor = Color.Black;

            PictureBox avatarBox = new PictureBox();
            avatarBox.Size = new Size(30, 30);
            avatarBox.SizeMode = PictureBoxSizeMode.StretchImage;

            int addY = 0;
            if (!string.IsNullOrWhiteSpace(lastControl?.Tag?.ToString()))
                addY = 20;

            if (isSelf)
            {
                //messageLabel.Location = new Point(chatPanel.Width / 2, lastY + 20);
                messageLabel.Location = new Point(180, (lastControl?.Bottom ?? 0) + 20 + addY);
                messageLabel.Width = 140;

                avatarBox.Image = Properties.Resources.User;
                avatarBox.Location = new Point(325, messageLabel.Top - 5);
            }
            else
            {
                //messageLabel.Location = new Point(10, lastY + 20);
                messageLabel.Location = new Point(50, (lastControl?.Bottom ?? 0) + 20 + addY);
                messageLabel.Width = 270;

                avatarBox.Image = Properties.Resources.AI;
                avatarBox.Location = new Point(10, messageLabel.Top - 5);

            }

            lastControl = messageLabel;

            panelMessage.Controls.Add(messageLabel);
            panelMessage.Controls.Add(avatarBox);
            panelMessage.ScrollControlIntoView(messageLabel);
        }

        /// <summary>
        /// 更新消息
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="isAppend">是否为追加</param>
        private void LableUpdate(string content, bool isAppend = true)
        {
            if (lastControl.InvokeRequired)
            {
                lastControl.Invoke(new Action(() =>
                {
                    LableUpdate(content, isAppend);
                }));

                panelMessage.Invoke(new Action(() =>
                {
                    panelMessage.ScrollControlIntoView(lastControl);
                }));
            }
            else
            {
                if (lastControl.Text == "思考中...")
                    lastControl.Text = "";

                if (isAppend)
                    lastControl.Text += content;
                else
                    lastControl.Text = content;

                using (Graphics g = lastControl.CreateGraphics())
                {
                    SizeF textSize = g.MeasureString(lastControl.Text, lastControl.Font, lastControl.Width - lastControl.Padding.Horizontal);

                    // 设置控件的高度，考虑 Padding 的影响
                    lastControl.Height = (int)textSize.Height + lastControl.Padding.Vertical;
                }

                panelMessage.ScrollControlIntoView(lastControl);

                Application.DoEvents(); // 更新 UI
            }
        }

        /// <summary>
        /// 添加插入按钮
        /// </summary>
        private void AddInserBtn(bool isChat)
        {
            System.Windows.Forms.Button btnInsert = new System.Windows.Forms.Button();

            if (isChat)
                btnInsert.Text = "插入文档";
            else
                btnInsert.Text = "应用公式";

            btnInsert.Size = new Size(70, 20);
            btnInsert.Location = new Point(50, lastControl.Bottom + 1);
            btnInsert.Tag = new string[] { lastControl.Text, isChat ? "0" : "1" };
            btnInsert.Click += BtnInsert_Click;

            if (panelMessage.InvokeRequired)
            {
                panelMessage.Invoke(new Action(() =>
                {
                    panelMessage.Controls.Add(btnInsert);

                    panelMessage.ScrollControlIntoView(btnInsert);
                }));
            }
            else
            {
                panelMessage.Controls.Add(btnInsert);

                panelMessage.ScrollControlIntoView(btnInsert);
            }

            if (lastControl.InvokeRequired)
                lastControl.Invoke(new Action(() =>
                {
                    lastControl.Tag = "1";
                }));
            else
                lastControl.Tag = "1";
        }

        /// <summary>
        /// 插入按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInsert_Click(object sender, EventArgs e)
        {
            string[] tag = (sender as System.Windows.Forms.Button).Tag as string[];
            string content = tag[0];
            string type = tag[1];

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
                    else
                    {
                        AddMessage("公式有误，请检查！", false);
                    }
                }
            }
        }
    }
}
