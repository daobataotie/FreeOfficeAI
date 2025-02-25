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

namespace FreeOfficeAI.Word
{
    public partial class ThisAddIn
    {
        private CommandBarPopup menuGroup;
        private bool isProcessingRightClick = false;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            //todo 发现在安装后无法调出右键菜单，但是Debug可以
            //右击前创建右键菜单
            Application.WindowBeforeRightClick += Application_WindowBeforeRightClick;
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            CommandBar contextMenu = Application.CommandBars["Text"];

            // 清理旧菜单项
            CleanExistingMenu(contextMenu);
        }

        private void Application_WindowBeforeRightClick(Selection Sel, ref bool Cancel)
        {
            if (isProcessingRightClick)
                return;

            isProcessingRightClick = true;

            try
            {
                // 仅当选中文本时显示菜单
                if (string.IsNullOrWhiteSpace(Sel.Text))
                    return;

                CommandBar contextMenu = Application.CommandBars["Text"];

                // 清理旧菜单项
                CleanExistingMenu(contextMenu);

                // 创建新菜单项
                CreateMenu(contextMenu);
            }
            finally
            {
                isProcessingRightClick = false;
            }
        }

        private void CleanExistingMenu(CommandBar contextMenu)
        {
            try
            {
                // 使用逆向遍历避免删除时的索引问题
                for (int i = contextMenu.Controls.Count; i >= 1; i--)
                {
                    var ctrl = contextMenu.Controls[i];
                    if (ctrl.Tag?.ToString() == "FreeOfficeAIMenu")
                    {
                        // 解除事件绑定
                        if (ctrl is CommandBarPopup group)
                        {
                            foreach (CommandBarControl subCtrl in group.Controls)
                            {
                                if (subCtrl is CommandBarButton button)
                                {
                                    button.Click -= TranslateButton_Click; // 额外确保解除绑定
                                    Marshal.FinalReleaseComObject(button);
                                }
                            }
                        }

                        ctrl.Delete();
                        Marshal.FinalReleaseComObject(ctrl);
                    }
                }
            }
            finally
            {
                if (menuGroup != null)
                {
                    foreach (CommandBarControl ctrl in menuGroup.Controls)
                    {
                        if (ctrl is CommandBarButton button)
                        {
                            button.Click -= TranslateButton_Click; // 额外确保解除绑定
                            Marshal.FinalReleaseComObject(button);
                        }
                    }

                    Marshal.FinalReleaseComObject(menuGroup);
                    menuGroup = null;
                }
            }
        }

        private void CreateMenu(CommandBar contextMenu)
        {
            // 创建菜单组
            menuGroup = (CommandBarPopup)contextMenu.Controls.Add(MsoControlType.msoControlPopup, Temporary: true); // 使用临时菜单项
            menuGroup.Caption = "FreeOfficeAI";
            menuGroup.Tag = "FreeOfficeAIMenu";

            // 在菜单组中添加子菜单项
            CommandBarButton button = (CommandBarButton)menuGroup.Controls.Add(MsoControlType.msoControlButton, Temporary: true);
            button.Caption = "翻译";
            button.Click += TranslateButton_Click;
        }

        private void TranslateButton_Click(CommandBarButton btn, ref bool CancelDefault)
        {
            try
            {
                // 防止快速双击导致多次触发
                if (btn == null)
                    return;

                string content = Application.Selection.Text;
                if (!string.IsNullOrWhiteSpace(content))
                {
                    string prompt = $"你是一位语言专家，精通中文和其他多种语言，能自动识别内容语言，若为中文则翻译成英文，非中文则翻译成中文。请翻译以下内容，不需要思考过程，不需要解释，直接翻译并给出结果：{content.Trim()}";
                    var response = OllamaApi.GetResponseAsync(prompt);

                    string transContent = response.Result;
                    if (transContent.Contains("</think>"))
                    {
                        transContent = transContent.Substring(transContent.IndexOf("</think>") + 8);
                    }

                    System.Windows.Forms.MessageBox.Show(transContent);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"错误：{ex.Message}");
            }
            finally
            {
                // 清理COM对象
                if (btn != null)
                {
                    btn.Click -= TranslateButton_Click;
                    Marshal.FinalReleaseComObject(menuGroup);
                    menuGroup = null;
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
