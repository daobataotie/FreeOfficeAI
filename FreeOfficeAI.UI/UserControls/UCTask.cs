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
        Label lastControl = null;
        private bool done = true;

        public UCTask(Action<string> insertOffice, Func<string> getContent)
        {
            InitializeComponent();

            panelMessage.BackColor = Color.LightCyan;
            panelMessage.AutoScroll = true;

            insertToOffice = insertOffice;
            getWordContent = getContent;
        }

        public void Send(string systemContent, string userContent, string functionType)
        {
            if (!done)
                return;

            AddMessage(functionType, true); // 自己发出的消息

            //获取Word文档内容
            string wordContent = getWordContent();
            if (string.IsNullOrWhiteSpace(wordContent))
            {
                AddMessage("空数据！", false);
                return;
            }

            userContent += wordContent;

            done = false;

            try
            {
                AddMessage("思考中...", false); // 返回的消息
                Task.Run(async () =>
                {
                    try
                    {
                        // 读取流式响应
                        await foreach (var responsePart in OllamaApi.GetResponseStreamAsync(systemContent, userContent))
                        {
                            LableUpdate(responsePart);
                        }

                        AddInserBtn();
                    }
                    catch (Exception ex)
                    {
                        LableUpdate(ex.Message, false);
                    }
                    finally
                    {
                        done = true;
                    }
                });

            }
            catch (Exception ex)
            {
                LableUpdate(ex.Message, false);
            }
        }

        private void AddMessage(string message, bool isSelf)
        {
            RoundedLabelControl messageLabel = new RoundedLabelControl();
            messageLabel.Text = message;
            //messageLabel = LabelAutoSizeMode.Vertical;
            messageLabel.BackColor = isSelf ? Color.FromArgb(137, 217, 97) : Color.Snow;
            messageLabel.Padding = new Padding(2);
            messageLabel.Font = new Font("微软雅黑", 10);
            messageLabel.ForeColor = Color.Black;

            PictureBox avatarBox = new PictureBox();
            avatarBox.Size = new Size(30, 30);
            avatarBox.SizeMode = PictureBoxSizeMode.StretchImage;

            int addY = 0;
            if (lastControl?.Tag?.ToString() == "1")
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

                // 滚动到控件视图
                panelMessage.ScrollControlIntoView(lastControl);


                Application.DoEvents(); // 更新 UI
            }
        }

        /// <summary>
        /// 添加插入按钮
        /// </summary>
        private void AddInserBtn()
        {
            System.Windows.Forms.Button btnInsert = new System.Windows.Forms.Button();
            btnInsert.Text = "插入文档";
            btnInsert.Size = new Size(70, 20);
            btnInsert.Location = new Point(50, lastControl.Bottom + 1);
            btnInsert.Tag = lastControl.Text;
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
            string content = (string)(sender as System.Windows.Forms.Button).Tag;
            if (!string.IsNullOrWhiteSpace(content))
            {
                //如果包含“</think>”,则只截取后面内容
                if (content.Contains("</think>"))
                {
                    content = content.Substring(content.IndexOf("</think>") + 8);
                }

                insertToOffice(content.Trim());
            }
        }
    }
}
