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
    public partial class UCBase : UserControl
    {
        public Action<string> insertToOffice;

        public Func<string> getWordContent;

        public Func<bool, string> getExcelContent;

        public Func<string, bool> executeVBA;

        public Control lastControl = null;
        public bool done = true;
        public OllamaRequest request = new OllamaRequest();

        private Timer scrollTimer;

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
            btnClean.Click += BtnClean_Click;

            scrollTimer = new Timer();
            scrollTimer.Interval = 100; // 100毫秒的间隔
            scrollTimer.Tick += ScrollTimer_Tick;
        }

        /// <summary>
        /// 更新消息时，滚动panel。解决消息过长时无法实时滚动到底部的问题。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollTimer_Tick(object sender, EventArgs e)
        {
            if (panelMessage.VerticalScroll.Visible)
            {
                // 计算需要滚动的位置
                Point point = panelMessage.AutoScrollPosition;
                int targetY = Math.Abs(point.Y);
                int maxScroll = panelMessage.DisplayRectangle.Height - panelMessage.ClientSize.Height;

                if (targetY < maxScroll)
                {
                    targetY = Math.Min(targetY + 30, maxScroll); // 每次滚动30个像素
                    panelMessage.AutoScrollPosition = new Point(0, targetY);
                }
                else
                {
                    scrollTimer.Stop();
                }
            }
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
        /// 清除按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClean_Click(object sender, EventArgs e)
        {
            request = new OllamaRequest();

            panelMessage.Controls.Clear();
            lastControl = null;
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
            //RoundedLabel messageLabel = new RoundedLabel();
            RoundedRichTextBox messageControl = new RoundedRichTextBox();
            messageControl.Text = content;
            messageControl.BackColor = isSelf ? Color.FromArgb(137, 217, 97) : Color.Snow;

            PictureBox avatarBox = new PictureBox();
            avatarBox.Size = new Size(25, 25);
            avatarBox.SizeMode = PictureBoxSizeMode.StretchImage;

            int y = (lastControl?.Bottom ?? 0) + 20;
            if (!string.IsNullOrWhiteSpace(lastControl?.Tag?.ToString()))
                y += 20;

            if (isSelf)
            {
                messageControl.Location = new Point(panelMessage.Width / 2, y);
                messageControl.Width = panelMessage.Width / 2 - 50;
                //messageLabel.Location = new Point(180, y);
                //messageLabel.Width = 140;

                messageControl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

                avatarBox.Image = Properties.Resources.User;
                avatarBox.Location = new Point(panelMessage.Width - 45, messageControl.Top);
                avatarBox.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            }
            else
            {
                messageControl.Location = new Point(40, y);
                messageControl.Width = panelMessage.Width - 90;
                //messageLabel.Location = new Point(50, y);
                //messageLabel.Width = 270;

                messageControl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

                avatarBox.Image = Properties.Resources.AI;
                avatarBox.Location = new Point(5, messageControl.Top);
                avatarBox.Anchor = AnchorStyles.Left | AnchorStyles.Top;

            }

            lastControl = messageControl;

            panelMessage.Controls.Add(messageControl);
            panelMessage.Controls.Add(avatarBox);
            panelMessage.ScrollControlIntoView(messageControl);
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

            }
            else
            {
                if (lastControl.Text == "思考中...")
                    lastControl.Text = "";

                if (isAppend)
                    lastControl.Text += content;
                else
                    lastControl.Text = content;

                if (lastControl is RoundedRichTextBox rtb)
                {
                    // 对于RoundedRichTextBox，使用其专门的高度调整方法
                    rtb.AdjustHeight();
                }

                // 启动滚动计时器
                if (!scrollTimer.Enabled)
                {
                    scrollTimer.Start();
                }

                Application.DoEvents(); // 更新 UI
            }
        }


        protected void UpdateStatus()
        {
            done = true;

            if (btnSend.InvokeRequired)
                btnSend.Invoke(new Action(() =>
                {
                    btnSend.Enabled = true;
                }));
            else
                btnSend.Enabled = true;

            if (btnClean.InvokeRequired)
                btnClean.Invoke(new Action(() =>
                {
                    btnClean.Enabled = true;
                }));
            else
                btnClean.Enabled = true;
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
