using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace CBOM
{
    public partial class fErrorData : Form
    {
        public fErrorData()
        {
            InitializeComponent();
        }

        private void fErrorData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
