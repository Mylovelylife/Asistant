using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
namespace PackingDll
{
    public partial class fUnfinish : Form
    {
        public fUnfinish()
        {
            InitializeComponent();
            ClientUtils.SetLanguage(this, fMain.g_sExeName);
        }

        private void LVData_DoubleClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}