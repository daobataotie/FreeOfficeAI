using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeOfficeAI.UI.UserControls
{
    public partial class UCBase : UserControl
    {
        public Action<string> insertToOffice;

        public Func<string> getWordContent;

        public Func<bool, string> getExcelContent;

        public Func<string, bool> executeVBA;

        public UCBase()
        {
            InitializeComponent();
        }
    }
}
