using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeOfficeAI.UI.Forms
{
    public partial class FrmAbout : Form
    {
        public FrmAbout(AppInfo appInfo)
        {
            InitializeComponent();

            InitialControls(appInfo);
        }

        private void InitialControls(AppInfo appInfo)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            MaximizeBox = false;

            lblName.Text = appInfo.ProductName;
            lblVersion.Text = appInfo.Version;
            lblDeveloper.Text = appInfo.Author;
            txtDescription.Text = appInfo.Description;

            txtDescription.ReadOnly = true;  //设置描述文本框只读
            txtDescription.TabStop = false;  //设置打开页面时描述文本框不被选中
        }
    }

    public class AppInfo
    {
        public string ProductName { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }

    }
}
