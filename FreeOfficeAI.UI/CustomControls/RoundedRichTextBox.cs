using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FreeOfficeAI.UI.CustomControls
{
    //todo 显示效果，高度计算还待优化
    public class RoundedRichTextBox : RichTextBox
    {
        private int radius = 12;
        private bool _adjustingHeight = false;
        private Padding _customPadding = default;

        // 重写Padding属性
        public new Padding Padding
        {
            get { return _customPadding; }
            set
            {
                _customPadding = value;
                // 更新内容区域
                RecalculateMargins();
                this.Invalidate();
            }
        }

        // P/Invoke 声明
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        // 消息常量
        private const int EM_SETRECT = 0xB3;

        public int Radius
        {
            get { return radius; }
            set { radius = value; this.Invalidate(); }
        }

        public RoundedRichTextBox()
        {
            // 基本设置
            this.BorderStyle = BorderStyle.None;
            this.ReadOnly = true;
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;
            this.ScrollBars = RichTextBoxScrollBars.None;
            this.Multiline = true;
            this.WordWrap = true;
            this.DetectUrls = true;
            this.AutoSize = false;
            //this.Margin = new Padding(5);
            _customPadding = new Padding(6);
            this.Font = new Font(this.Font.Name, 10);
            this.Height = 25;
            //this.Font = new Font("微软雅黑", 10);

            // 禁用自定义绘制，让RichTextBox自己处理文本渲染
            this.SetStyle(ControlStyles.UserPaint, false);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            // 初始化内容区域
            this.HandleCreated += (s, e) => RecalculateMargins();
            this.SizeChanged += (s, e) => RecalculateMargins();
        }

        // 重新计算内容区域，应用自定义Padding
        private void RecalculateMargins()
        {
            if (!this.IsHandleCreated || this.IsDisposed)
                return;

            // 创建一个RECT结构体，表示文本区域
            RECT rect = new RECT
            {
                Left = _customPadding.Left,
                Top = _customPadding.Top,
                Right = this.ClientSize.Width - _customPadding.Right,
                Bottom = this.ClientSize.Height - _customPadding.Bottom
            };

            // 将RECT结构体转换为IntPtr
            IntPtr lpRect = Marshal.AllocCoTaskMem(Marshal.SizeOf(rect));
            Marshal.StructureToPtr(rect, lpRect, false);

            // 发送EM_SETRECT消息设置文本区域
            SendMessage(this.Handle, EM_SETRECT, IntPtr.Zero, lpRect);

            // 释放内存
            Marshal.FreeCoTaskMem(lpRect);
        }

        // RECT结构体定义
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            // 创建圆角区域
            UpdateRegion();

            // 应用自定义Padding
            RecalculateMargins();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // 调整大小时更新区域
            UpdateRegion();

            // 重新应用自定义Padding
            RecalculateMargins();
        }

        private void UpdateRegion()
        {
            if (this.Width <= 0 || this.Height <= 0)
                return;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, this.Height - radius, radius, radius, 90, 90);
                path.CloseAllFigures();

                this.Region = new Region(path);
            }
        }

        // 改进的高度调整方法
        public void AdjustHeight()
        {
            if (_adjustingHeight) return;

            _adjustingHeight = true;

            try
            {
                // 计算文本高度
                int textHeight = 0;

                // 使用TextRenderer测量文本高度，考虑自定义Padding
                //Size textSize = TextRenderer.MeasureText(this.Text, this.Font,
                //    new Size(this.Width - _customPadding.Horizontal - 10, 0),
                //    TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);

                //textHeight = textSize.Height + _customPadding.Vertical;
                //SizeF textSize = new SizeF();

                //using (Graphics g = this.CreateGraphics())
                //{
                //    textSize = g.MeasureString(this.Text, this.Font, this.Width - _customPadding.Horizontal);

                //    // 设置控件的高度，考虑 Padding 的影响
                //    textHeight = (int)textSize.Height + _customPadding.Vertical + 1;
                //}
                if (this.TextLength > 0)
                {
                    Point lastCharPos = this.GetPositionFromCharIndex(this.TextLength - 1);
                    int lineHeight = this.Font.Height;
                    textHeight = lastCharPos.Y + lineHeight + this.Padding.Vertical - 2;
                }

                // 设置最小高度
                int minHeight = 25;

                // 设置新高度，确保至少有最小高度
                int newHeight = Math.Max(minHeight, textHeight);

                // 只有当高度真的需要改变时才设置
                if (this.Height != newHeight)
                {
                    this.Height = newHeight;

                    // 重新应用自定义Padding
                    RecalculateMargins();
                }
            }
            finally
            {
                _adjustingHeight = false;
            }
        }

        // 重写OnTextChanged方法，在文本变化时重新计算内容区域
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            RecalculateMargins();
        }
    }
}
