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
    public partial class UCBase : UserControl
    {
        public Action<string> insertToOffice;

        public Func<string> getWordContent;

        public Func<bool, string> getExcelContent;

        public Func<string, bool> executeVBA;

        public RoundedLabel lastControl = null;

        public UCBase()
        {
            InitializeComponent();

            DoubleBuffered = true;  // 启用双缓冲

            splitContainerControl1.BackColor = Color.SkyBlue;     //控制移动条的颜色
            splitContainerControl1.Panel1.BackColor = Color.LightCyan;
            splitContainerControl1.Panel2.BackColor = Color.White;
            panelMessage.BackColor = Color.LightCyan;
            panelMessage.AutoScroll = true;
            panelMessage.VerticalScroll.Visible = true;

            txtInput.KeyDown += TxtInput_KeyDown; // 绑定 KeyDown 事件
            btnSend.Click += SendButton_Click;
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

        /// <summary>
        /// 发送按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendButton_Click(object sender, EventArgs e)
        {
            SendRequest();
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        protected virtual void SendRequest()
        {

        }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="isSelf">是否为自己发送的消息</param>
        protected void AddMessage(string content, bool isSelf)
        {
            RoundedLabel messageLabel = new RoundedLabel();
            messageLabel.Text = content;
            messageLabel.BackColor = isSelf ? Color.FromArgb(137, 217, 97) : Color.Snow;

            PictureBox avatarBox = new PictureBox();
            avatarBox.Size = new Size(25, 25);
            avatarBox.SizeMode = PictureBoxSizeMode.StretchImage;

            int y = (lastControl?.Bottom ?? 0) + 20;
            if (!string.IsNullOrWhiteSpace(lastControl?.Tag?.ToString()))
                y += 20;

            if (isSelf)
            {
                messageLabel.Location = new Point(panelMessage.Width / 2, y);
                messageLabel.Width = panelMessage.Width / 2 - 50;
                //messageLabel.Location = new Point(180, y);
                //messageLabel.Width = 140;

                messageLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

                avatarBox.Image = Properties.Resources.User;
                avatarBox.Location = new Point(panelMessage.Width - 45, messageLabel.Top);
                avatarBox.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            }
            else
            {
                messageLabel.Location = new Point(40, y);
                messageLabel.Width = panelMessage.Width - 90;
                //messageLabel.Location = new Point(50, y);
                //messageLabel.Width = 270;

                messageLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

                avatarBox.Image = Properties.Resources.AI;
                avatarBox.Location = new Point(5, messageLabel.Top);
                avatarBox.Anchor = AnchorStyles.Left | AnchorStyles.Top;

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
        protected void UpdateMessage(string content, bool isAppend = true)
        {
            if (lastControl.InvokeRequired)
            {
                lastControl.Invoke(new Action(() =>
                {
                    UpdateMessage(content, isAppend);
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
                    lastControl.Height = (int)textSize.Height + lastControl.Padding.Vertical + 1;
                }

                panelMessage.ScrollControlIntoView(lastControl);
                //panelMessage.VerticalScroll.Value = panelMessage.VerticalScroll.Maximum;

                Application.DoEvents(); // 更新 UI
            }
        }

        /// <summary>
        /// 添加插入按钮
        /// </summary>
        /// <param name="text">按钮文本</param>
        /// <param name="tag">按钮Tag</param>
        protected void AddInsertBtn(string text, string tag = "")
        {
            Button btnInsert = new Button();

            btnInsert.Text = text;

            int x = 0;
            int y = 0;
            string txt = "";
            if (lastControl.InvokeRequired)
            {
                lastControl.Invoke(new Action(() =>
                {
                    x = lastControl.Left;
                    y = lastControl.Bottom + 1;
                    txt = lastControl.Text;

                    lastControl.Tag = "1";
                }));
            }
            else
            {
                x = lastControl.Left;
                y = lastControl.Bottom + 1;
                txt = lastControl.Text;

                lastControl.Tag = "1";
            }

            btnInsert.Size = new Size(70, 20);
            btnInsert.Location = new Point(x + 2, y);

            if (string.IsNullOrWhiteSpace(tag))
                btnInsert.Tag = txt;
            else
                btnInsert.Tag = new string[] { txt, tag };

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
        }

        /// <summary>
        /// 插入按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInsert_Click(object sender, EventArgs e)
        {
            ExecuteInsert((sender as Button).Tag);
        }

        protected virtual void ExecuteInsert(object tag)
        {
            string content = tag?.ToString();
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
