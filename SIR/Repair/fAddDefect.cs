using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;
using SajetFilter;
namespace RepairDll
{
    public partial class fAddDefect : Form
    {
        public fAddDefect()
        {
            InitializeComponent();
            ClientUtils.SetLanguage(this, fMain.g_sExeName);
        }

        private void fAddDefect_Load(object sender, EventArgs e)
        {
        }

        private void fAddDefect_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
        }

        private void btnFilterDefect_Click(object sender, EventArgs e)
        {
            
            string sSQL = " Select DEFECT_CODE,DEFECT_DESC "
               + " From SAJET.SYS_DEFECT "
               + " Where Enabled='Y' "
               + " Order By DEFECT_CODE ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editDefect.Text = f.dgvData.CurrentRow.Cells["DEFECT_CODE"].Value.ToString();
                KeyPressEventArgs sKey = new KeyPressEventArgs((char)Keys.Return);
                editDefect_KeyPress(editDefect, sKey);
            }
            f.Dispose();
        }

        private void editDefect_KeyPress(object sender, KeyPressEventArgs e)
        {

           // lablDefectDesc2.Text = string.Empty;
            labldefectDesc.Text = string.Empty;
            editDefect.Text = editDefect.Text.Trim();
            string sDefectCode = editDefect.Text;
            if (e.KeyChar != (char)Keys.Return)
                return;
            if (!CheckDefect(sDefectCode))
            {
                editDefect.Focus();
                editDefect.SelectAll();
            }
            else
            {
                editLocation.Focus();
                editLocation.SelectAll();
            }
        }
        private bool CheckDefect(string sDefectCode)
        {
            string sSQL = " Select DEFECT_CODE,DEFECT_DESC"
                    + " From SAJET.SYS_DEFECT "
                    + " Where Enabled = 'Y' "
                    + " and DEFECT_CODE = :DEFECT_CODE ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_CODE", sDefectCode };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                ClientUtils.ShowMessage(SajetCommon.SetLanguage("Defect Code Error", 1), 0);
                //SajetCommon.Show_Message(SajetCommon.SetLanguage("Defect Code Error", 1), 0);
                return false;
            }
            labldefectDesc.Text = dsTemp.Tables[0].Rows[0]["DEFECT_DESC"].ToString();
           // lablDefectDesc2.Text = dsTemp.Tables[0].Rows[0]["DEFECT_DESC2"].ToString();
            return true;
        }
    }
}