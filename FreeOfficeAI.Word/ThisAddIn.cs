using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Core;
using System.Runtime.InteropServices;
using FreeOfficeAI.Core;
using FreeOfficeAI.UI.UserControls;

namespace FreeOfficeAI.Word
{
    public partial class ThisAddIn
    {
        private CommandBarPopup menuGroup = null;
        private CommandBarButton buttonTranslate = null;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Application.DocumentBeforeClose += Application_DocumentBeforeClose;

            CreateMenu();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            //循环父级菜单项，删除自定义菜单。直接删除自定义会报错，猜测可能是卸载插件或者关闭Word时，menuGroup已经被释放了。
            foreach (CommandBarControl control in Application.CommandBars["Text"].Controls)
            {
                if (control.Tag == "FreeOfficeAIMenu")
                {
                    control.Delete(false);
                    Marshal.FinalReleaseComObject(control);
                    break;
                }
            }
        }

        private void Application_DocumentBeforeClose(Microsoft.Office.Interop.Word.Document Doc, ref bool Cancel)
        {
            foreach (var task in this.CustomTaskPanes)
            {
                if (task.Control is UCBase uc)
                {
                    uc.Visible = false;
                    uc.Dispose();
                }
            }
        }

        private void CreateMenu()
        {
            // 创建菜单组
            menuGroup = (CommandBarPopup)Application.CommandBars["Text"].Controls.Add(MsoControlType.msoControlPopup, Temporary: true); // 使用临时菜单项
            menuGroup.Caption = "FreeOfficeAI";
            menuGroup.Tag = "FreeOfficeAIMenu";

            // 在菜单组中添加子菜单项
            buttonTranslate = (CommandBarButton)menuGroup.Controls.Add(MsoControlType.msoControlButton, Temporary: true);
            buttonTranslate.Caption = "翻译";
            buttonTranslate.Click += TranslateButton_Click;
        }

        private void TranslateButton_Click(CommandBarButton btn, ref bool CancelDefault)
        {
            foreach (var ribbon in Globals.Ribbons)
            {
                if (ribbon is WordRibbon wordRibbon)
                {
                    wordRibbon.BtnTranslate_Click(null, null);
                }
            }
        }


        public void InsertToWordHandler(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
                Globals.ThisAddIn.Application.Selection.InsertAfter(content.Trim());
        }

        public string GetWordContentHandler()
        {
            //若当前有鼠标选中内容，则返回选中内容，否则返回当前文档内容
            string selectedText = Globals.ThisAddIn.Application.Selection.Text.Trim();
            if (selectedText?.Length > 1)  //光标定位在文档中间时，没有选中也会有一个字符
                return selectedText;

            return Globals.ThisAddIn.Application.ActiveDocument.Content.Text.Trim();
        }

        #region VSTO 生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
