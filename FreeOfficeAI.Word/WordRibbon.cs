using FreeOfficeAI.UI.Forms;
using FreeOfficeAI.UI.UserControls;
using Microsoft.Office.Tools;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace FreeOfficeAI.Word
{
    public partial class WordRibbon
    {
        private void WordRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            group1.Label = "工具箱";

            btnSummary.Label = "总结提炼\n";
            btnSummary.Click += BtnSummary_Click;
            btnContractCheck.Label = "合同检查\n";
            btnContractCheck.Click += BtnContractCheck_Click;
            btnRectify.Label = "纠错矫正\n";
            btnRectify.Click += BtnRectify_Click;
            btnTranslate.Label = "翻译\n";
            btnTranslate.Click += BtnTranslate_Click;
            btnChat.Label = "对话\n";
            btnChat.Click += BtnChat_Click;


            group2.Label = "帮助";
            btnSetting.Label = "设置\n";
            btnSetting.Click += BtnSetting_Click;
            btnAbout.Label = "关于\n";
            btnAbout.Click += BtnAbout_Click;
        }

        private void BtnSummary_Click(object sender, RibbonControlEventArgs e)
        {
            var customTaskPane = GetTaskPane(typeof(UCTask));

            if (customTaskPane != null && customTaskPane.Visible)
            {
                string systemContent = "你是一位文档总结专家，擅长根据用户提供的文档内容进行总结和提炼，你拥有强大的内容分析能力，能准确提取关键信息和核心要点。";
                string userContent = "请对以下内容进行总结提炼出核心要点，并直接给出结果，尽量简洁：";

                (customTaskPane.Control as UCTask).Send(systemContent, userContent, "总结提炼");
            }
        }

        private void BtnContractCheck_Click(object sender, RibbonControlEventArgs e)
        {
            var customTaskPane = GetTaskPane(typeof(UCTask));

            if (customTaskPane != null && customTaskPane.Visible)
            {
                string systemContent = "你是一位法律专家同时也是一位商务合同攥写专家，擅长商务合同审查，你有丰富的法律知识和经验，能快速准确地分析商务合同的法律风险以及合同内容的合规性。";
                string userContent = "请对以下合同进行风险分析及合规性检查，并直接给出结果，尽量简洁：";
                (customTaskPane.Control as UCTask).Send(systemContent, userContent, "合同检查");
            }
        }

        private void BtnRectify_Click(object sender, RibbonControlEventArgs e)
        {
            var customTaskPane = GetTaskPane(typeof(UCTask));

            if (customTaskPane != null && customTaskPane.Visible)
            {
                string systemContent = "你是一位语言专家，精通多种语言，你有丰富的语法知识和文档编写能力，能快速准确地查找文档中的语法错误，或者文字错误";
                string userContent = "请对以下内容进行语法检查和错字纠正，若没有错误请直接返回“没有错误”，若有，请输出你认为有语法或文字错误的地方，尽量简洁：";
                (customTaskPane.Control as UCTask).Send(systemContent, userContent, "纠错矫正");
            }
        }

        private void BtnTranslate_Click(object sender, RibbonControlEventArgs e)
        {
            var customTaskPane = GetTaskPane(typeof(UCTask));

            if (customTaskPane != null && customTaskPane.Visible)
            {
                string systemContent = "你是一位语言专家，精通中文和其他多种语言，能自动识别内容语言，若为中文则翻译成英文，非中文则翻译成中文。";
                string userContent = "请翻译以下内容，不需要思考过程，不需要解释，直接翻译并给出结果：";
                (customTaskPane.Control as UCTask).Send(systemContent, userContent, "翻译");
            }
        }

        private void BtnChat_Click(object sender, RibbonControlEventArgs e)
        {
            var customTaskPane = GetTaskPane(typeof(UCChat));
        }

        private void BtnAbout_Click(object sender, RibbonControlEventArgs e)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            AppInfo appInfo = new AppInfo()
            {
                ProductName = "FreeOfficeAI-Word插件",
                Version = $"{version.Major}.{version.Minor}.{version.Build}",
                Author = "曹瑞",
                Description = "    本插件调用本地大模型，实现文档分析、合同审核、纠错矫正、翻译、对话等功能。\r\n    目前为第一版，功能还在完善中，将持续更新。\r\n    使用说明：点击按钮时，默认处理选中文本，若无选中文本，则处理整篇文档\r\n\r\n",
            };
            FrmAbout frmAbout = new FrmAbout(appInfo);
            frmAbout.ShowDialog();
        }

        private void BtnSetting_Click(object sender, RibbonControlEventArgs e)
        {
            FrmSetting frmSetting = new FrmSetting();
            frmSetting.ShowDialog();
        }

        private CustomTaskPane GetTaskPane(Type controlType)
        {
            try
            {
                CustomTaskPane customTaskPane = null;
                foreach (var taskPane in Globals.ThisAddIn.CustomTaskPanes)
                {
                    if (taskPane.Control.GetType() == controlType)
                    {
                        if (taskPane.Control.Tag != null && int.TryParse(taskPane.Control.Tag.ToString(), out int hwnd) && hwnd == Globals.ThisAddIn.Application.ActiveWindow.Hwnd)
                        {
                            taskPane.Visible = true;
                            customTaskPane = taskPane;
                            break;
                        }
                    }
                }

                if (customTaskPane == null)
                {
                    if (controlType == typeof(UCTask))
                        customTaskPane = CreateTaskPanel();
                    else if (controlType == typeof(UCChat))
                        customTaskPane = CreateChatPanel();
                }

                return customTaskPane;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private CustomTaskPane CreateTaskPanel()
        {
            UCTask uc = new UCTask((arg) => Globals.ThisAddIn.InsertToWordHandler(arg), () => Globals.ThisAddIn.GetWordContentHandler());
            uc.Tag = Globals.ThisAddIn.Application.ActiveWindow.Hwnd;
            int width = uc.Width;

            CustomTaskPane customTaskPane;
            customTaskPane = Globals.ThisAddIn.CustomTaskPanes.Add(uc, "FreeOfficeAI助手");
            customTaskPane.DockPosition = Microsoft.Office.Core.MsoCTPDockPosition.msoCTPDockPositionRight;
            customTaskPane.Width = width;
            customTaskPane.Visible = true;
            return customTaskPane;
        }

        private CustomTaskPane CreateChatPanel()
        {
            UCChat uc = new UCChat((arg) => Globals.ThisAddIn.InsertToWordHandler(arg));
            uc.Tag = Globals.ThisAddIn.Application.ActiveWindow.Hwnd;
            int width = uc.Width;

            CustomTaskPane customTaskPane;
            customTaskPane = Globals.ThisAddIn.CustomTaskPanes.Add(uc, "FreeOfficeAI对话");
            customTaskPane.DockPosition = Microsoft.Office.Core.MsoCTPDockPosition.msoCTPDockPositionRight;
            customTaskPane.Width = width;  //UC添加到任务窗格后，宽度变为0
            customTaskPane.Visible = true;
            return customTaskPane;
        }

    }
}
