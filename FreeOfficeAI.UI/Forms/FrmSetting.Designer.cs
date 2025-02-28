namespace FreeOfficeAI.UI.Forms
{
    partial class FrmSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSaveModel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cobFrame = new System.Windows.Forms.ComboBox();
            this.cobModelName = new System.Windows.Forms.ComboBox();
            this.rdbApiKey = new System.Windows.Forms.RadioButton();
            this.rdbLocal = new System.Windows.Forms.RadioButton();
            this.btnConnTestLocal = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(384, 211);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSaveModel);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.rdbApiKey);
            this.tabPage1.Controls.Add(this.rdbLocal);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(376, 185);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "大模型设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSaveModel
            // 
            this.btnSaveModel.Location = new System.Drawing.Point(151, 154);
            this.btnSaveModel.Name = "btnSaveModel";
            this.btnSaveModel.Size = new System.Drawing.Size(75, 23);
            this.btnSaveModel.TabIndex = 7;
            this.btnSaveModel.Text = "保存";
            this.btnSaveModel.UseVisualStyleBackColor = true;
            this.btnSaveModel.Click += new System.EventHandler(this.btnSaveModel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnConnTestLocal);
            this.panel1.Controls.Add(this.txtAddress);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cobFrame);
            this.panel1.Controls.Add(this.cobModelName);
            this.panel1.Location = new System.Drawing.Point(15, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(353, 100);
            this.panel1.TabIndex = 6;
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(57, 4);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(293, 21);
            this.txtAddress.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "地址：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "框架：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "模型名：";
            // 
            // cobFrame
            // 
            this.cobFrame.FormattingEnabled = true;
            this.cobFrame.Location = new System.Drawing.Point(57, 39);
            this.cobFrame.Name = "cobFrame";
            this.cobFrame.Size = new System.Drawing.Size(159, 20);
            this.cobFrame.TabIndex = 2;
            // 
            // cobModelName
            // 
            this.cobModelName.FormattingEnabled = true;
            this.cobModelName.Location = new System.Drawing.Point(57, 74);
            this.cobModelName.Name = "cobModelName";
            this.cobModelName.Size = new System.Drawing.Size(159, 20);
            this.cobModelName.TabIndex = 4;
            // 
            // rdbApiKey
            // 
            this.rdbApiKey.AutoSize = true;
            this.rdbApiKey.Location = new System.Drawing.Point(72, 15);
            this.rdbApiKey.Name = "rdbApiKey";
            this.rdbApiKey.Size = new System.Drawing.Size(59, 16);
            this.rdbApiKey.TabIndex = 1;
            this.rdbApiKey.Text = "ApiKey";
            this.rdbApiKey.UseVisualStyleBackColor = true;
            // 
            // rdbLocal
            // 
            this.rdbLocal.AutoSize = true;
            this.rdbLocal.Checked = true;
            this.rdbLocal.Location = new System.Drawing.Point(19, 15);
            this.rdbLocal.Name = "rdbLocal";
            this.rdbLocal.Size = new System.Drawing.Size(47, 16);
            this.rdbLocal.TabIndex = 0;
            this.rdbLocal.TabStop = true;
            this.rdbLocal.Text = "本地";
            this.rdbLocal.UseVisualStyleBackColor = true;
            // 
            // btnConnTestLocal
            // 
            this.btnConnTestLocal.Location = new System.Drawing.Point(258, 72);
            this.btnConnTestLocal.Name = "btnConnTestLocal";
            this.btnConnTestLocal.Size = new System.Drawing.Size(75, 23);
            this.btnConnTestLocal.TabIndex = 8;
            this.btnConnTestLocal.Text = "连接测试";
            this.btnConnTestLocal.UseVisualStyleBackColor = true;
            this.btnConnTestLocal.Click += new System.EventHandler(this.btnConnTestLocal_Click);
            // 
            // FrmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 211);
            this.Controls.Add(this.tabControl1);
            this.Name = "FrmSetting";
            this.Text = "设置";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RadioButton rdbApiKey;
        private System.Windows.Forms.RadioButton rdbLocal;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cobFrame;
        private System.Windows.Forms.ComboBox cobModelName;
        private System.Windows.Forms.Button btnSaveModel;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnConnTestLocal;
    }
}