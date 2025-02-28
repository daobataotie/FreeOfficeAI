using FreeOfficeAI.UI.Forms;
using FreeOfficeAI.UI.UserControls;
using Microsoft.Office.Tools;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeOfficeAI.Excel
{
    public partial class ExcelRibbon
    {
        private void ExcelRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            group1.Label = "工具箱";
            btnTool.Label = "AI工具\n";
            btnTool.Click += BtnTool_Click;

            group2.Label = "帮助";
            btnAbout.Label = "关于\n";
            btnAbout.Click += BtnAbout_Click;
            btnSetting.Label = "设置\n";
            btnSetting.Click += BtnSetting_Click;
        }

        private void BtnTool_Click(object sender, RibbonControlEventArgs e)
        {
            var customTaskPane = GetTaskPane(typeof(UCTool));
        }


        private void BtnAbout_Click(object sender, RibbonControlEventArgs e)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            AppInfo appInfo = new AppInfo()
            {
                ProductName = "FreeOfficeAI-Excel插件",
                Version = $"{version.Major}.{version.Minor}.{version.Build}",
                Author = "曹瑞",
                Description = "    本插件调用本地大模型，实现Excel公式编写、对话等功能。\r\n    目前为第一版，功能还在完善中，将持续更新。\r\n   ",
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
            CustomTaskPane customTaskPane = null;
            foreach (var taskPane in Globals.ThisAddIn.CustomTaskPanes)
            {
                if (taskPane.Control.GetType() == controlType)
                {
                    if (taskPane.Control.Tag != null && int.TryParse(taskPane.Control.Tag.ToString(), out int hwnd) && hwnd == Globals.ThisAddIn.Application.Hwnd)
                    {
                        taskPane.Visible = true;
                        customTaskPane = taskPane;
                        break;
                    }
                }
            }

            if (customTaskPane == null)
            {
                if (controlType == typeof(UCTool))
                    customTaskPane = CreateToolPanel();
                else if (controlType == typeof(UCChat))
                    customTaskPane = CreateChatPanel();
            }

            return customTaskPane;
        }

        private CustomTaskPane CreateToolPanel()
        {
            UCTool uc = new UCTool((arg) => Globals.ThisAddIn.InsertToExcelHandler(arg), (arg) => Globals.ThisAddIn.ExcuteVBA(arg), (arg) => Globals.ThisAddIn.GetExcelContentHandler(arg));
            uc.Tag = Globals.ThisAddIn.Application.Hwnd;
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
            UCChat uc = new UCChat((arg) => Globals.ThisAddIn.InsertToExcelHandler(arg));
            uc.Tag = Globals.ThisAddIn.Application.Hwnd;
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
