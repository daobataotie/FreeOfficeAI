namespace FreeOfficeAI.Excel
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
            this.btnChat = this.Factory.CreateRibbonButton();
            this.tabFreeOfficeAI.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabFreeOfficeAI
            // 
            this.tabFreeOfficeAI.Groups.Add(this.group1);
            this.tabFreeOfficeAI.Label = "FreeOfficeAI";
            this.tabFreeOfficeAI.Name = "tabFreeOfficeAI";
            // 
            // group1
            // 
            this.group1.Items.Add(this.btnTool);
            this.group1.Items.Add(this.btnChat);
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
            this.btnTool.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnTool_Click);
            // 
            // btnChat
            // 
            this.btnChat.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnChat.Image = global::FreeOfficeAI.Excel.Properties.Resources.对话;
            this.btnChat.Label = "对话";
            this.btnChat.Name = "btnChat";
            this.btnChat.ShowImage = true;
            this.btnChat.Visible = false;
            this.btnChat.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnChat_Click);
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
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tabFreeOfficeAI;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnTool;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnChat;
    }

    partial class ThisRibbonCollection
    {
        internal ExcelRibbon ExcelRibbon
        {
            get { return this.GetRibbon<ExcelRibbon>(); }
        }
    }
}
