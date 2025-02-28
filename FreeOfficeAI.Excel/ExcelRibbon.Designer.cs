﻿namespace FreeOfficeAI.Excel
{
    partial class ExcelRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ExcelRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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
            this.tabFreeOfficeAI = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.btnTool = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.btnSetting = this.Factory.CreateRibbonButton();
            this.btnAbout = this.Factory.CreateRibbonButton();
            this.tabFreeOfficeAI.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabFreeOfficeAI
            // 
            this.tabFreeOfficeAI.Groups.Add(this.group1);
            this.tabFreeOfficeAI.Groups.Add(this.group2);
            this.tabFreeOfficeAI.Label = "FreeOfficeAI";
            this.tabFreeOfficeAI.Name = "tabFreeOfficeAI";
            // 
            // group1
            // 
            this.group1.Items.Add(this.btnTool);
            this.group1.Label = "工具箱";
            this.group1.Name = "group1";
            // 
            // btnTool
            // 
            this.btnTool.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnTool.Image = global::FreeOfficeAI.Excel.Properties.Resources.工具;
            this.btnTool.Label = "AI工具";
            this.btnTool.Name = "btnTool";
            this.btnTool.ShowImage = true;
            // 
            // group2
            // 
            this.group2.Items.Add(this.btnSetting);
            this.group2.Items.Add(this.btnAbout);
            this.group2.Label = "帮助";
            this.group2.Name = "group2";
            // 
            // btnSetting
            // 
            this.btnSetting.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnSetting.Image = global::FreeOfficeAI.Excel.Properties.Resources.设置;
            this.btnSetting.Label = "设置";
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.ShowImage = true;
            // 
            // btnAbout
            // 
            this.btnAbout.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnAbout.Image = global::FreeOfficeAI.Excel.Properties.Resources.关于;
            this.btnAbout.Label = "关于";
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.ShowImage = true;
            // 
            // ExcelRibbon
            // 
            this.Name = "ExcelRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tabFreeOfficeAI);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.ExcelRibbon_Load);
            this.tabFreeOfficeAI.ResumeLayout(false);
            this.tabFreeOfficeAI.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tabFreeOfficeAI;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnTool;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSetting;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAbout;
    }

    partial class ThisRibbonCollection
    {
        internal ExcelRibbon ExcelRibbon
        {
            get { return this.GetRibbon<ExcelRibbon>(); }
        }
    }
}
