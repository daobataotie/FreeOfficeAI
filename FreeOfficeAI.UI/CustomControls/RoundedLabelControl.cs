﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeOfficeAI.UI.CustomControls
{
    /// <summary>
    /// 自定义圆角标签控件
    /// </summary>
    public class RoundedLabelControl : Label
    {
        private const int CornerRadius = 10;

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            {
                Rectangle rect = new Rectangle(-1, -1, this.Width, this.Height);
                path.AddArc(rect.X, rect.Y, CornerRadius, CornerRadius, 180, 90);
                path.AddArc(rect.X + rect.Width - CornerRadius, rect.Y, CornerRadius, CornerRadius, 270, 90);
                path.AddArc(rect.X + rect.Width - CornerRadius, rect.Y + rect.Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
                path.AddArc(rect.X, rect.Y + rect.Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);
                path.CloseFigure();

                this.Region = new Region(path);

                using (SolidBrush brush = new SolidBrush(this.BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                using (Pen pen = new Pen(Color.Gray, 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }

                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Near;
                format.LineAlignment = StringAlignment.Center;

                Rectangle textRect = new Rectangle(3, 0, this.Width, this.Height);  //左边空出3个像素
                e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), textRect, format);
            }
        }
    }
}
