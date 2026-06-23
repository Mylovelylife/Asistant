using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;
using SajetFilter;

namespace RepairDll
{
    public partial class fData : Form
    {
        public fData()
        {
            InitializeComponent();
        }
        string sSQL = string.Empty;
        DataSet dsTemp;
        public string g_sSN,g_sType;
        public string g_sReasonID,g_sReason,g_sReasonDesc,g_sReasonDesc1;
        public string g_sDutyID, g_sDuty, g_sDutyDesc, g_sDutyDesc1;
        private void btnSearchReason_Click(object sender, EventArgs e)
        {
            sSQL = " Select Reason_Code,Reason_Desc,Reason_desc2 "
                 + " From SAJET.SYS_REASON "
                 + " Where Enabled='Y' "
                 + " Order By Reason_Code ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editReason.Text = f.dgvData.CurrentRow.Cells["Reason_Code"].Value.ToString();
                KeyPressEventArgs sKey = new KeyPressEventArgs((char)Keys.Return);
                editReason_KeyPress(editReason, sKey);
            }
            f.Dispose();
        }

        private void editReason_KeyPress(object sender, KeyPressEventArgs e)
        {
            LabReasonDesc.Text = "";
            LabReasonDesc1.Text = "";
            g_sReasonID = "0";
            if (e.KeyChar != (char)Keys.Return)
                return;

            if (Check_Reason())
            {
                editDuty.Focus();
                editDuty.SelectAll();
            }
        }
        public bool Check_Reason()
        {
            sSQL = " Select Reason_Id, Reason_Code, Reason_Desc,REASON_DESC2 "
                     + " From SAJET.SYS_REASON "
                     + " Where Enabled = 'Y' "
                     + " and REASON_CODE = '" + editReason.Text + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                ClientUtils.ShowMessage(SajetCommon.SetLanguage("Reason Code Error", 1) + Environment.NewLine
                                       + SajetCommon.SetLanguage("Reason Code", 1) + " : " + editReason.Text, 0);
                editReason.Focus();
                editReason.SelectAll();
                return false;
            }
            g_sReasonID = dsTemp.Tables[0].Rows[0]["Reason_ID"].ToString();
            LabReasonDesc.Text = dsTemp.Tables[0].Rows[0]["Reason_Desc"].ToString();
            LabReasonDesc1.Text = dsTemp.Tables[0].Rows[0]["REASON_DESC2"].ToString();
            g_sReason = dsTemp.Tables[0].Rows[0]["Reason_Code"].ToString();
            g_sReasonDesc = LabReasonDesc.Text;
            g_sReasonDesc1 = LabReasonDesc1.Text;

            return true;
        }

        private void btnSearchDuty_Click(object sender, EventArgs e)
        {
            sSQL = " Select Duty_Code,Duty_Desc,DUTY_DESC2 "
                + " From SAJET.SYS_DUTY "
                + " Where Enabled='Y' "
                + " Order By Duty_Code ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editDuty.Text = f.dgvData.CurrentRow.Cells["Duty_Code"].Value.ToString();
                KeyPressEventArgs sKey = new KeyPressEventArgs((char)Keys.Return);
                editDuty_KeyPress(editDuty, sKey);
            }
            f.Dispose();
        }

        private void editDuty_KeyPress(object sender, KeyPressEventArgs e)
        {
            LabDutyDesc.Text = "";
            LabDutyDesc1.Text = "";
            g_sDutyID = "0";
            if (e.KeyChar != (char)Keys.Return)
                return;

            if (Check_Duty())
            {
                btnOK.Focus();                
            }
        }
        public bool Check_Duty()
        {
            sSQL = " Select Duty_ID, Duty_Code, Duty_Desc,DUTY_DESC2 "
                 + " From SAJET.SYS_Duty "
                 + " Where Enabled = 'Y' "
                 + " and Duty_Code = '" + editDuty.Text + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                ClientUtils.ShowMessage(SajetCommon.SetLanguage("Duty Code Error", 1) + Environment.NewLine
                               + SajetCommon.SetLanguage("Duty Code", 1) + " : " + editDuty.Text, 0);
                editDuty.Focus();
                editDuty.SelectAll();
                return false;
            }
            g_sDutyID = dsTemp.Tables[0].Rows[0]["Duty_ID"].ToString();
            LabDutyDesc.Text = dsTemp.Tables[0].Rows[0]["Duty_Desc"].ToString();
            LabDutyDesc1.Text = dsTemp.Tables[0].Rows[0]["DUTY_DESC2"].ToString();
            g_sDuty = dsTemp.Tables[0].Rows[0]["Duty_Code"].ToString();
            g_sDutyDesc = LabDutyDesc.Text;
            g_sDutyDesc1 = LabDutyDesc1.Text;
            return true;
        }

        private void fData_Load(object sender, EventArgs e)
        {
            ClientUtils.SetLanguage(this, fMain.g_sExeName);
            this.Text = g_sType;
            lblSerialNumber.Text = g_sSN;
            LabReasonDesc.Text = "Reason Desc";
            LabDutyDesc.Text = "Duty Desc";

            if (g_sType == "Modify")
            {
                editReason.Text = g_sReason;
                LabReasonDesc.Text = g_sReasonDesc;
                LabReasonDesc1.Text = g_sReasonDesc1;
                editDuty.Text = g_sDuty;
                LabDutyDesc.Text = g_sDutyDesc;
                LabDutyDesc1.Text = g_sDutyDesc1;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!Check_Reason()) return;
            if (!Check_Duty()) return;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
