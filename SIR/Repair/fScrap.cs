using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace RepairDll
{
    public partial class fScrap : Form
    {
        public string g_sSN;
        public string g_sScrapType;
        public string g_sMemo;
        public fScrap()
        {
            InitializeComponent();
        }

        private void fScrap_Load(object sender, EventArgs e)
        {
            ClientUtils.SetLanguage(this, fMain.g_sExeName);
            LabSN.Text = g_sSN;
            rbtnScrap.Checked = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            editMemo.Text = editMemo.Text.Trim();
            if (string.IsNullOrEmpty(editMemo.Text))
            {
                ClientUtils.ShowMessage("Please Input Memo", 0);
                editMemo.Focus();
                editMemo.SelectAll();
                return;
            }
            if (rbtnScrap.Checked)
                g_sScrapType = "1";
            else
                g_sScrapType = "2";
            g_sMemo = editMemo.Text;
            DialogResult = DialogResult.OK;
        }

        private void fScrap_Activated(object sender, EventArgs e)
        {
            editMemo.Focus();
        }
    }
}