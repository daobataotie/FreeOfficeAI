using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;

namespace FreeOfficeAI.Excel
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
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
                contentRange= Globals.ThisAddIn.Application.Selection as Microsoft.Office.Interop.Excel.Range; 

                //// 获取当前选择的区域
                //var selection = Globals.ThisAddIn.Application.Selection as Microsoft.Office.Interop.Excel.Range;
                //if (selection != null)
                //{
                //    List<ExcelContent> contents = new List<ExcelContent>();
                //    foreach (Microsoft.Office.Interop.Excel.Range cell in selection.Cells)
                //    {
                //        contents.Add(new ExcelContent
                //        {
                //            Cell = cell.Address,
                //            Value = cell.Value2?.ToString()
                //        });
                //    }

                //    //return selection.Cells.Value2.ToString();
                //}
            }
            else
            {
                contentRange = (Globals.ThisAddIn.Application.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet).UsedRange;

                //// 获取当前工作表
                //var worksheet = Globals.ThisAddIn.Application.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
                //if (worksheet != null)
                //{
                //    List<ExcelContent> contents = new List<ExcelContent>();
                //    foreach (Microsoft.Office.Interop.Excel.Range cell in selection.Cells)
                //    {
                //        contents.Add(new ExcelContent
                //        {
                //            Cell = cell.Address,
                //            Value = cell.Value2?.ToString()
                //        });
                //    }
                //}
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
            }

            return content;
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
