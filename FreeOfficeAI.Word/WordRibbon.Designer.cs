namespace FreeOfficeAI.Word
{
    partial class WordRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public WordRibbon()
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
            this.btnSummary = this.Factory.CreateRibbonButton();
            this.btnContractCheck = this.Factory.CreateRibbonButton();
            this.btnRectify = this.Factory.CreateRibbonButton();
            this.btnTranslate = this.Factory.CreateRibbonButton();
            this.btnChat = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
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
            this.group1.Items.Add(this.btnSummary);
            this.group1.Items.Add(this.btnContractCheck);
            this.group1.Items.Add(this.btnRectify);
            this.group1.Items.Add(this.btnTranslate);
            this.group1.Items.Add(this.btnChat);
            this.group1.Label = "工具箱";
            this.group1.Name = "group1";
            // 
            // btnSummary
            // 
            this.btnSummary.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnSummary.Image = global::FreeOfficeAI.Word.Properties.Resources.提炼摘要;
            this.btnSummary.Label = "总结提炼";
            this.btnSummary.Name = "btnSummary";
            this.btnSummary.ShowImage = true;
            this.btnSummary.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnSummary_Click);
            // 
            // btnContractCheck
            // 
            this.btnContractCheck.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnContractCheck.Image = global::FreeOfficeAI.Word.Properties.Resources.查找;
            this.btnContractCheck.Label = "合同检查";
            this.btnContractCheck.Name = "btnContractCheck";
            this.btnContractCheck.ShowImage = true;
            this.btnContractCheck.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnContractCheck_Click);
            // 
            // btnRectify
            // 
            this.btnRectify.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnRectify.Image = global::FreeOfficeAI.Word.Properties.Resources.矫正;
            this.btnRectify.Label = "纠错矫正";
            this.btnRectify.Name = "btnRectify";
            this.btnRectify.ShowImage = true;
            this.btnRectify.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnRectify_Click);
            // 
            // btnTranslate
            // 
            this.btnTranslate.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnTranslate.Image = global::FreeOfficeAI.Word.Properties.Resources.翻译;
            this.btnTranslate.Label = "翻译";
            this.btnTranslate.Name = "btnTranslate";
            this.btnTranslate.ShowImage = true;
            this.btnTranslate.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnTranslate_Click);
            // 
            // btnChat
            // 
            this.btnChat.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnChat.Image = global::FreeOfficeAI.Word.Properties.Resources.对话;
            this.btnChat.Label = "对话";
            this.btnChat.Name = "btnChat";
            this.btnChat.ShowImage = true;
            this.btnChat.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnChat_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.btnAbout);
            this.group2.Label = "帮助";
            this.group2.Name = "group2";
            // 
            // btnAbout
            // 
            this.btnAbout.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnAbout.Image = global::FreeOfficeAI.Word.Properties.Resources.关于;
            this.btnAbout.Label = "关于";
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.ShowImage = true;
            this.btnAbout.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnAbout_Click);
            // 
            // WordRibbon
            // 
            this.Name = "WordRibbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tabFreeOfficeAI);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.WordRibbon_Load);
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
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSummary;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnContractCheck;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnRectify;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnChat;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnTranslate;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAbout;
    }

    partial class ThisRibbonCollection
    {
        internal WordRibbon WordRibbon
        {
            get { return this.GetRibbon<WordRibbon>(); }
        }
    }
}
