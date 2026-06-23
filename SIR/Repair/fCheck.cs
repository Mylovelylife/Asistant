using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Collections.Specialized;

namespace RepairDll
{
    public partial class fCheck : Form
    {
        public fCheck(string sProgram)
        {
            InitializeComponent();
            g_sProgram = sProgram;
        }
        string g_sProgram;
        public string strHideColumn = string.Empty;
        public string sSQL;
        StringCollection g_tsField = new StringCollection();
        public string sSN;
        public string sComfirmID;
        public string sUserID;
        string g_sComfirmEmp;

        private void fCheck_Load(object sender, EventArgs e)
        {
            DataSet dsSearch = ClientUtils.ExecuteSQL(sSQL);
            dgvData.DataSource = dsSearch;
            dgvData.DataMember = dsSearch.Tables[0].ToString();

            for (int i = 0; i <= dsSearch.Tables[0].Columns.Count - 1; i++)
            {
                g_tsField.Add(dsSearch.Tables[0].Columns[i].ToString());
            }

            if (dgvData.Rows.Count > 0)
                dgvData.CurrentCell = dgvData.Rows[0].Cells[0];

            if (!strHideColumn.Equals(string.Empty))
            {
                dgvData.Columns[strHideColumn].Visible = false;
            }
            string sMsg = "";
            g_sComfirmEmp = (SajetCommon.GetSysBaseData(g_sProgram, "Confirm Employee", ref sMsg));
            if (g_sComfirmEmp == "0" || g_sComfirmEmp == "")
            {
                sSQL = "Select EMP_NO,EMP_NAME,ENABLED from sajet.sys_emp "
                            + "Where EMP_ID = '" + sUserID + "' ";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    editEmp.Text = dsTemp.Tables[0].Rows[0]["EMP_NO"].ToString();
                    labEmp.Text = dsTemp.Tables[0].Rows[0]["EMP_NAME"].ToString();
                }
            }       
            lbSN.Text = sSN;

            SajetCommon.SetLanguageControl(this);
            //editEmp.Focus();           
        }

        private void editEmp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            string sEmpNo = "";
            string sEmpName = "";    

            Get_EmpNo(sUserID, out sEmpNo, out sEmpName);
            if (sEmpNo == editEmp.Text && g_sComfirmEmp =="1")
            {
                ClientUtils.ShowMessage("Repair and Comfirm are the same Employee! Please Scan again.", 0);
                //SajetCommon.Show_Message("Repair and Comfirm are the same Employee! Please Scan again.",0);
                editEmp.Text = string.Empty;
                editEmp.Focus();
                editEmp.SelectAll();
                return;
            }
            else
            {
                labEmp.Text = "";
                sComfirmID = "0";
              

                if (!Check_ComfirmEmp())
                {
                    editEmp.Focus();
                    editEmp.SelectAll();
                    return;
                }
            }
        }

        private bool Check_ComfirmEmp()
        {
            //若Repairer空白,則自動帶出login user
            editEmp.Text = editEmp.Text.Trim();
            if (!string.IsNullOrEmpty(editEmp.Text))
            {
                sSQL = "Select EMP_NAME,EMP_ID,ENABLED from sajet.sys_emp "
                     + "Where EMP_NO = '" + editEmp.Text + "'";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                   ClientUtils.ShowMessage(SajetCommon.SetLanguage("Employee")+"(" + editEmp.Text + ")"+ SajetCommon.SetLanguage("Error"), 0);
                    return false;
                }
                else if (dsTemp.Tables[0].Rows[0]["ENABLED"].ToString() != "Y")
                {
                    ClientUtils.ShowMessage(SajetCommon.SetLanguage("Employee")+"(" + editEmp.Text + ")"+SajetCommon.SetLanguage("Disabled"), 0);
                    return false;
                }
                sComfirmID = dsTemp.Tables[0].Rows[0]["EMP_ID"].ToString();
                labEmp.Text = dsTemp.Tables[0].Rows[0]["EMP_NAME"].ToString();

                return true;
            }
            else
            {
                ClientUtils.ShowMessage("Please Input Employee ID", 0);
                return false;
            }
        }

        public void Get_EmpNo(string sEmpID, out string sEmpNo, out string sEmpName)
        {
            sSQL = "Select EMP_NO,EMP_NAME from sajet.sys_emp "
                 + "where EMP_ID = '" + sEmpID + "'";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sEmpNo = dsTemp.Tables[0].Rows[0]["EMP_NO"].ToString();
                sEmpName = dsTemp.Tables[0].Rows[0]["EMP_NAME"].ToString();
            }
            else
            {
                sEmpNo = "";
                sEmpName = "";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string sEmpNo = "";
            string sEmpName = "";  

            if (dgvData.Rows.Count > 0 && dgvData.CurrentRow != null)
            {
                if (!string.IsNullOrEmpty(sComfirmID))
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    if (Check_ComfirmEmp())
                    {
                        Get_EmpNo(sUserID, out sEmpNo, out sEmpName);
                        if (sEmpNo == editEmp.Text && g_sComfirmEmp=="1")
                        {
                            ClientUtils.ShowMessage("Repair and Comfirm are the same Employee! Please Scan again.", 0);
                            editEmp.Text = string.Empty;
                          
                        }
                        else
                        {
                            DialogResult = DialogResult.OK;
                        }                     
                    }
                  
                }
            }
            editEmp.Focus();
            editEmp.SelectAll();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        private void editEmp_TextChanged(object sender, EventArgs e)
        {
            sComfirmID = "";
            labEmp.Text = "";
        }

        private void fCheck_Shown(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(editEmp.Text))
                editEmp.Focus();
        }
    }
}