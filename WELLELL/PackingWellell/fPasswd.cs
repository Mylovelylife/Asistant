using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
namespace PackingDll
{
    public partial class fPasswd : Form
    {
        public fPasswd()
        {
            InitializeComponent();
        }

        public static string g_sEMP_ID;
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (editEmpNo.Text == "" || editPasswd.Text == "")
            {
                MessageBox.Show("Please Input Emp No. or Password");
                return;
            }
            fMain f = new fMain();
            string sSQL = "Select EMP_ID,trim(SAJET.password.decrypt(PASSWD)) PWD from sajet.sys_emp "
                        + "where emp_no = '" + editEmpNo.Text + "' ";
            DataSet ds = ClientUtils.ExecuteSQL(sSQL);
            if (ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("Employee Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                editEmpNo.Focus();
                editEmpNo.SelectAll();
                return;
            }
            if (editPasswd.Text != ds.Tables[0].Rows[0]["PWD"].ToString())
            {
                MessageBox.Show("Passwd Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                editPasswd.Focus();
                editPasswd.SelectAll();
                return;
            }
            g_sEMP_ID = ds.Tables[0].Rows[0]["EMP_ID"].ToString();
            f.Dispose();
            DialogResult = DialogResult.OK;
        }

        private void editPasswd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                btnOK_Click(sender, EventArgs.Empty);
        }

        private void editEmpNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                editPasswd.Focus();
                editPasswd.SelectAll();
            }
        }

        private void fPasswd_Load(object sender, EventArgs e)
        {
            ClientUtils.SetLanguage(this, fMain.g_sExeName);
        }

        private void fPasswd_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
        }
    }
}