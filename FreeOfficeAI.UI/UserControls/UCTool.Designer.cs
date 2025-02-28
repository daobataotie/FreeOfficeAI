namespace FreeOfficeAI.UI.UserControls
{
    partial class UCTool
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdbFormula = new System.Windows.Forms.RadioButton();
            this.rdoBtnChat = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.Panel1.SuspendLayout();
            this.splitContainerControl1.Panel2.SuspendLayout();
            this.splitContainerControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            // 
            // splitContainerControl1.Panel2
            // 
            this.splitContainerControl1.Panel2.Controls.Add(this.panel1);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.Controls.Add(this.rdbFormula);
            this.panel1.Controls.Add(this.rdoBtnChat);
            this.panel1.Location = new System.Drawing.Point(3, 121);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(119, 24);
            this.panel1.TabIndex = 2;
            // 
            // rdbFormula
            // 
            this.rdbFormula.AutoSize = true;
            this.rdbFormula.Checked = true;
            this.rdbFormula.Location = new System.Drawing.Point(65, 4);
            this.rdbFormula.Name = "rdbFormula";
            this.rdbFormula.Size = new System.Drawing.Size(47, 16);
            this.rdbFormula.TabIndex = 1;
            this.rdbFormula.TabStop = true;
            this.rdbFormula.Text = "公式";
            this.rdbFormula.UseVisualStyleBackColor = true;
            // 
            // rdoBtnChat
            // 
            this.rdoBtnChat.AutoSize = true;
            this.rdoBtnChat.Checked = true;
            this.rdoBtnChat.Location = new System.Drawing.Point(8, 4);
            this.rdoBtnChat.Name = "rdoBtnChat";
            this.rdoBtnChat.Size = new System.Drawing.Size(47, 16);
            this.rdoBtnChat.TabIndex = 0;
            this.rdoBtnChat.TabStop = true;
            this.rdoBtnChat.Text = "对话";
            this.rdoBtnChat.UseVisualStyleBackColor = true;
            // 
            // UCTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UCTool";
            this.splitContainerControl1.Panel1.ResumeLayout(false);
            this.splitContainerControl1.Panel2.ResumeLayout(false);
            this.splitContainerControl1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdbFormula;
        private System.Windows.Forms.RadioButton rdoBtnChat;
    }
}
