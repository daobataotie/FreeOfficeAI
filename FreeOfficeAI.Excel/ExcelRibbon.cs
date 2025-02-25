using FreeOfficeAI.UI.UserControls;
using Microsoft.Office.Tools;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeOfficeAI.Excel
{
    public partial class ExcelRibbon
    {
        // 导入 Windows API 函数
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        private const uint WM_CLOSE = 0x0010;
        private const uint WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;
        private const int VK_Enter = 0x0D;
        private const int VK_ESC = 0x1B;
        private const int VK_Alt = 0xA4;
        private const int VK_F4 = 0x73;

        private void ExcelRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            group1.Label = "工具箱";

            btnTool.Label = "AI工具\n";
            btnChat.Label = "对话\n";
        }

        private void BtnTool_Click(object sender, RibbonControlEventArgs e)
        {
            var customTaskPane = GetTaskPane(typeof(UCTool));
        }

        private void BtnChat_Click(object sender, RibbonControlEventArgs e)
        {
            //var customTaskPane = GetTaskPane(typeof(UCChat));
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
            UCTool uc = new UCTool((arg) => Globals.ThisAddIn.InsertToExcelHandler(arg), (arg) => ExcuteVBA(arg), (arg) => Globals.ThisAddIn.GetExcelContentHandler(arg));
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

        private bool ExcuteVBA(string vbaCode)
        {
            if (string.IsNullOrWhiteSpace(vbaCode))
                return false;

            var excelApp = Globals.ThisAddIn.Application;
            Microsoft.Vbe.Interop.VBProject vbProject = excelApp.ActiveWorkbook.VBProject;
            Microsoft.Vbe.Interop.VBComponent vbComponent = null;

            try
            {
                vbComponent = vbProject.VBComponents.Add(Microsoft.Vbe.Interop.vbext_ComponentType.vbext_ct_StdModule);
                vbComponent.CodeModule.AddFromString(vbaCode);

                //excelApp.VBE.MainWindow.Width = 1000;
                //excelApp.VBE.MainWindow.Height = 1000;

                //bool isSuccess = true;
                //CancellationTokenSource cts = new CancellationTokenSource();
                //cts.Token.Register(() =>
                //{
                //    isSuccess = false;
                //});
                //Task.Run(() =>
                //{
                //    CloseVBEWindow(cts);
                //}, cts.Token);

                excelApp._Run2("FreeOfficeAIExample");

                //if (isSuccess)
                {
                    //System.Windows.Forms.MessageBox.Show("公式应用成功");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                string fileName = "C:\\Users\\Administrator\\Desktop\\log.txt";
                string logMsg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}" +
                    $"错误信息：{ex.Message}{Environment.NewLine}";
                System.IO.File.AppendAllText(fileName, logMsg);

                System.Windows.Forms.MessageBox.Show("公式应用失败");

                return false;
            }
            finally
            {
                vbProject.VBComponents.Remove(vbComponent);

                string fileName = "C:\\Users\\Administrator\\Desktop\\log.txt";
                string logMsg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}" +
                    $"{vbaCode}{Environment.NewLine}" +
                    $"{"".PadRight(50, '-')}{Environment.NewLine}{Environment.NewLine}";
                System.IO.File.AppendAllText(fileName, logMsg);
            }
        }

        private async void CloseVBEWindow(CancellationTokenSource cts)
        {
            try
            {
                string windowTitle = "Microsoft Visual Basic for Applications";
                IntPtr intPtr = IntPtr.Zero;
                while (intPtr == IntPtr.Zero)
                {
                    await Task.Delay(10);
                    intPtr = FindWindow(null, windowTitle);
                }

                IntPtr hWndParent = GetParent(intPtr);

                //SendKeys.SendWait("{ENTER}");
                SendMessage(intPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

                if (hWndParent == IntPtr.Zero)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        await Task.Delay(10);
                        hWndParent = GetParent(intPtr);
                    }
                }

                if (hWndParent != IntPtr.Zero)
                {
                    StringBuilder stringBuilder = new StringBuilder(256);
                    GetWindowText(hWndParent, stringBuilder, stringBuilder.Capacity);
                    if (stringBuilder.ToString().StartsWith("Microsoft Visual Basic"))
                    {
                        SetForegroundWindow(hWndParent);
                        Thread.Sleep(100);
                        SendMessage(hWndParent, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

                        intPtr = FindWindow(null, windowTitle);
                        if (intPtr != IntPtr.Zero)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                SetForegroundWindow(hWndParent);
                                Thread.Sleep(100);

                                SendKeys.SendWait("{ENTER}");
                                SendMessage(intPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

                                intPtr = FindWindow(null, windowTitle);
                                if (intPtr == IntPtr.Zero)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                cts.Cancel();
            }
            catch (Exception ex)
            {
                cts.Cancel();
            }
        }
    }
}
