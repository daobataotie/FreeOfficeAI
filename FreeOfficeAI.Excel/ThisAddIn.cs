using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using System.Text.Json;
using FreeOfficeAI.UI.UserControls;

namespace FreeOfficeAI.Excel
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            this.Application.WorkbookBeforeClose += Application_WorkbookBeforeClose;
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        private void Application_WorkbookBeforeClose(Microsoft.Office.Interop.Excel.Workbook Wb, ref bool Cancel)
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

        public void InsertToExcelHandler(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                var selection = Globals.ThisAddIn.Application.Selection as Microsoft.Office.Interop.Excel.Range;
                if (selection != null)
                {
                    selection.Cells.Value2 = content;
                }
            }
        }

        public string GetExcelContentHandler(bool getSelection)
        {
            string content = string.Empty;
            Microsoft.Office.Interop.Excel.Range contentRange = null;

            if (getSelection)
            {
                contentRange = Globals.ThisAddIn.Application.Selection as Microsoft.Office.Interop.Excel.Range;
            }
            else
            {
                contentRange = (Globals.ThisAddIn.Application.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet).UsedRange;
            }

            // 获取当前工作表
            var worksheet = Globals.ThisAddIn.Application.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
            if (worksheet != null)
            {
                List<ExcelContent> contents = new List<ExcelContent>();
                foreach (Microsoft.Office.Interop.Excel.Range cell in contentRange.Cells)
                {
                    contents.Add(new ExcelContent
                    {
                        Cell = cell.Address,
                        Value = cell.Value2?.ToString()
                    });
                }

                content = JsonSerializer.Serialize(contents);
            }

            return content;
        }

        public bool ExcuteVBA(string vbaCode)
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
#if DEBUG
                string fileName = "C:\\Users\\Administrator\\Desktop\\log.txt";
                string logMsg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}" +
                    $"错误信息：{ex.Message}{Environment.NewLine}";
                System.IO.File.AppendAllText(fileName, logMsg);

                System.Windows.Forms.MessageBox.Show("公式应用失败");
#endif

                return false;
            }
            finally
            {
                vbProject.VBComponents.Remove(vbComponent);

#if DEBUG
                string fileName = "C:\\Users\\Administrator\\Desktop\\log.txt";
                string logMsg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}" +
                    $"{vbaCode}{Environment.NewLine}" +
                    $"{"".PadRight(50, '-')}{Environment.NewLine}{Environment.NewLine}";
                System.IO.File.AppendAllText(fileName, logMsg);
#endif
            }
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

    public class ExcelContent
    {
        public string Cell { get; set; }

        public string Value { get; set; }
    }
}
