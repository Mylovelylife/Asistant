using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;
using SajetTable;

namespace ProcessStepDll
{
    public partial class fDetailData : Form
    {
         //private fMain fMainControl;
        public delegate void m_OnAppend(string f_StepItemCode);
        public event m_OnAppend OnAppend;
        public fDetailData()
        {
            InitializeComponent();
        }
        public fDetailData(fMain f)
        {
            InitializeComponent();
           // fMainControl = f;
        }
        public string g_sUpdateType, g_sformText;
    //    public string g_sStepItemCode, g_sStepItemName;
        public string g_sKeyID, NeedMaterial, WOMaterial;
        public DataGridViewRow dataCurrentRow;
        //public DataGridViewRow dataCurrentRow;
       
        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;
        public string g_sProcessName, g_sProcessID;

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            this.Text = g_sformText;
            if (g_sProcessID == "0")
            {
                LabProcess.Visible = label1.Visible = false;
            }
            LabProcess.Text = g_sProcessName;

            if (g_sUpdateType == "MODIFY")
            {

                combCSN.SelectedIndex = combCSN.Items.IndexOf(dataCurrentRow.Cells["NEED_CSN"].Value.ToString());
                combATETest.SelectedIndex = combATETest.Items.IndexOf(dataCurrentRow.Cells["ATE_TEST"].Value.ToString());
                combATEVerify.SelectedIndex = combATEVerify.Items.IndexOf(dataCurrentRow.Cells["ATE_VERIFY"].Value.ToString());
                editStepCode.Text = dataCurrentRow.Cells["STEP_ITEM_CODE"].Value.ToString();
                editStepName.Text = dataCurrentRow.Cells["STEP_ITEM_NAME"].Value.ToString();
                g_sKeyID = SajetCommon.GetID("SAJET.SYS_STEP_ITEM", "STEP_ITEM_ID", "STEP_ITEM_CODE", editStepCode.Text);
            }
            else
            {
                combATETest.SelectedIndex = combATEVerify.SelectedIndex = combCSN.SelectedIndex = 1;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {            
            for (int i = 0; i <= panelControl.Controls.Count - 1; i++)
            {
                if (panelControl.Controls[i] is TextBox)
                {
                    panelControl.Controls[i].Text = panelControl.Controls[i].Text.Trim();
                }
            }
           
            if (editStepCode.Text == "")
            {
                string sData = LabCode.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editStepCode.Focus();
                editStepCode.SelectAll();
                return;
            }            
            
            //檢查Code是否重複
            sSQL = " Select * from SAJET.SYS_STEP_ITEM "
                 + " Where STEP_ITEM_CODE = '" + editStepCode.Text + "' ";
            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " and STEP_ITEM_ID <> '" + g_sKeyID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = LabCode.Text + " : " + editStepCode.Text;
                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editStepCode.Focus();
                editStepCode.SelectAll();
                return;
            }

            if (editStepName.Text == "")
            {
                string sData = LabName.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editStepCode.Focus();
                editStepCode.SelectAll();
                return;
            }

            /*
            //檢查Name是否重複
            sSQL = " Select * from SAJET.SYS_STEP_ITEM "
                 + " Where STEP_ITEM_NAME = '" + editStepName.Text + "' ";
            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " and STEP_ITEM_ID <> '" + g_sKeyID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = LabName.Text + " : " + editStepName.Text;
                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editStepCode.Focus();
                editStepCode.SelectAll();
                return;
            }
            */

            //Update DB
            try
            {
                if (g_sUpdateType == "APPEND")
                {
                    AppendData();
                    bAppendSucess = true;
                    string sMsg = SajetCommon.SetLanguage("Data Append OK", 2) + " !" + Environment.NewLine + SajetCommon.SetLanguage("Append Other Data", 2) + " ?";
                    OnAppend(editStepCode.Text);
                    if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                    {
                        ClearData();

                        editStepCode.Focus();
                        return;
                    }
                    DialogResult = DialogResult.OK;
                }
                else if (g_sUpdateType == "MODIFY")
                {
                    ModifyData();
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                return;
            }
        }

        private void AppendData()
        {
            string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_STEP_ITEM", "STEP_ITEM_ID", 8);
            string sMaxID1 = SajetCommon.GetMaxID("SAJET.SYS_HT_STEP_ITEM", "STEP_ITEM_ID", 8);
            if (String.Compare(sMaxID1 , sMaxID) >0)
                sMaxID = sMaxID1;

                object[][] Params = new object[7][];
            sSQL = " Insert into SAJET.SYS_STEP_ITEM "
                 + " (STEP_ITEM_ID,STEP_ITEM_CODE,STEP_ITEM_NAME,UPDATE_USERID,UPDATE_TIME "
                 + " ,ENABLED,NEED_CSN,ATE_TEST,ATE_VERIFY) "
                 + " Values "
                 + " (:STEP_ITEM_ID,:STEP_ITEM_CODE,:STEP_ITEM_NAME,:UPDATE_USERID,SYSDATE,'Y',:NEED_CSN,:ATE_TEST,:ATE_VERIFY) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STEP_ITEM_ID", sMaxID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STEP_ITEM_CODE", editStepCode.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STEP_ITEM_NAME", editStepName.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEED_CSN", combCSN.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ATE_TEST", combATETest.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ATE_VERIFY",combATEVerify.Text  };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            CopyToDetailHistory(sMaxID);

            //新增資料至製程工序關聯表
            if (g_sProcessID != "0")
            {
                sSQL = string.Format(@" Insert into SAJET.SYS_PROCESS_STEP (PROCESS_ID,STEP_ITEM_ID,UPDATE_USERID,UPDATE_TIME,ENABLED) 
                                    Values ('{0}','{1}','{2}',SYSDATE,'Y') ", g_sProcessID, sMaxID, fMain.g_sUserID);
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                //製程工序關聯歷程
                sSQL = @" Insert into SAJET.SYS_HT_PROCESS_STEP SELECT * FROM SAJET.SYS_PROCESS_STEP WHERE STEP_ITEM_ID =" + sMaxID;
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
        }
        private void ModifyData()
        {
        

            object[][] Params = new object[7][];
            sSQL = " Update SAJET.SYS_STEP_ITEM "
                 + " set STEP_ITEM_CODE = :STEP_ITEM_CODE "
                 + "    ,STEP_ITEM_NAME = :STEP_ITEM_NAME "
                 + "    ,UPDATE_USERID =:UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + "    ,NEED_CSN =:NEED_CSN "
                 + "    ,ATE_TEST =:ATE_TEST "
                 + "    ,ATE_VERIFY =:ATE_VERIFY "
                 + " where STEP_ITEM_ID = :STEP_ITEM_ID ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STEP_ITEM_CODE", editStepCode.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STEP_ITEM_NAME", editStepName.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEED_CSN", combCSN.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ATE_TEST", combATETest.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ATE_VERIFY", combATEVerify.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STEP_ITEM_ID", g_sKeyID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            CopyToDetailHistory(g_sKeyID);
        }

        private void combNeedMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void combWOMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
        }

        private void ClearData()
        {
            for (int i = 0; i <= panelControl.Controls.Count - 1; i++)
            {
                if (panelControl.Controls[i] is TextBox)
                {
                    panelControl.Controls[i].Text = "";
                }
                    
                else if (panelControl.Controls[i] is ComboBox)
                {
                    ((ComboBox)panelControl.Controls[i]).SelectedIndex = ((ComboBox)panelControl.Controls[i]).Items.IndexOf("N");
                }
                     
            }
        }
        public void CopyToDetailHistory(string sID)
        {
            string sSQL = " Insert into " + TableDefine.gsDef_DtlHTTable + " "
                        + " Select * from " + TableDefine.gsDef_DtlTable + " "
                        + " where " + TableDefine.gsDef_DtlKeyField + " = '" + sID + "' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
        }

        private void editStepCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editStepCode.Text = editStepCode.Text.Trim();
            if (!string.IsNullOrEmpty(editStepCode.Text))
            {
                editStepName.Focus();
                editStepName.SelectAll();
            }
        }

        private void editStepName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editStepName.Text = editStepName.Text.Trim();
            if (!string.IsNullOrEmpty(editStepName.Text))
                btnOK.Focus();
        }
    }
}