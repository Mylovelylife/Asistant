using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetFilter;
using System.Data.OracleClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Linq;

namespace CBOM
{
    public partial class fData : Form
    {
        public fData()
        {
            InitializeComponent();
        }
        
        public string g_sProcessID, g_sStepID;
        public string g_sItemPartID,g_sItemPartType,g_sItemSpec1,g_sModelName;              
        public bool g_sChangeGroup;
        public string g_sBOM_ID, g_sProcess, g_sStep, g_sKey;
        public string g_sPartNo, g_sVer;
        public string g_sUpdateType, g_sRowid;
        string sSQL;
        DataSet dsTemp;

        private void bbtnOK_Click(object sender, EventArgs e)
        {
            if (editSubPartNo.Text.Trim() == "")
            {
                string sData = LabSubPart.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editSubPartNo.Focus();
                return;
            }
            
            if (editQty.Text.Trim() == "0" || editQty.Text.Trim() == "")
            {
                SajetCommon.Show_Message("Qty Error", 0);
                editQty.Focus();
                editQty.SelectAll();
                return;
            }

            if (editSubPartVer.Text.Trim() == "")
                editSubPartVer.Text = "N/A";
            if (combProcess.Text.Trim() == "")
            {
                SajetCommon.Show_Message("Process Name Error", 0);
                return;
            }
            else
            {
                g_sProcessID = fMain.GET_FIELD_ID("SAJET.SYS_PROCESS", "PROCESS_NAME", "PROCESS_ID", combProcess.Text);                
            }

            if (combStep.Text.Trim() == "")
            {
                SajetCommon.Show_Message("Step Name Error", 0);
                return;
            }
            else
            {
                g_sStepID = fMain.GET_FIELD_ID("SAJET.SYS_STEP_ITEM", "STEP_ITEM_NAME", "STEP_ITEM_ID", combStep.Text);
            }

            if (GET_PART_ID(editSubPartNo.Text) == "0")
            {
                SajetCommon.Show_Message("Sub Part No Error", 0);
                editSubPartNo.Focus();
                editSubPartNo.SelectAll();
                return;
            }

            //若加入替代料,Group不可為0
            if ((g_sChangeGroup) & (editGroup.Text == "0" | editGroup.Text == ""))
            {
                SajetCommon.Show_Message("Please Change Relation", 0);
                editGroup.Focus();
                editGroup.SelectAll();
                return;
            }
            
            if (string.IsNullOrEmpty(g_sBOM_ID))
            {                
                sSQL = "Select NVL(Max(BOM_ID),0) + 1 BOM_ID "
                     + "From SAJET.SYS_BOM_INFO ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows[0]["BOM_ID"].ToString() == "1")
                {
                    sSQL = " Select RPAD(NVL(PARAM_VALUE,'1'),2,'0') || '000001' BOM_ID "
                         + " From SAJET.SYS_BASE "
                         + " Where PARAM_NAME = 'DBID' ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                }
                g_sBOM_ID = dsTemp.Tables[0].Rows[0]["BOM_ID"].ToString();

                string sPartID = fMain.GET_FIELD_ID("SAJET.SYS_PART", "PART_NO", "PART_ID", g_sPartNo);
                sSQL = "INSERT INTO sajet.sys_bom_info "
                     + "(BOM_ID,PART_ID,VERSION,UPDATE_USERID)"
                     + "VALUES "
                     + "('" + g_sBOM_ID + "','" + sPartID + "','" + g_sVer + "','" + ClientUtils.UserPara1 + "')";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }

            sSQL = "Select ITEM_PART_ID from sajet.sys_bom "
                 + "Where BOM_ID='" + g_sBOM_ID + " '"
                 + "and NVL(Process_ID,'0') = '" + g_sProcessID + " '"
                 + "and STEP_ITEM_ID = '" + g_sStepID + " '"
                 + "and ITEM_PART_ID = '" + g_sItemPartID + " '";
            if (g_sUpdateType == "Modify")
                sSQL = sSQL + "and rowid <> '" + g_sRowid + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = LabSubPart.Text + " : " + editSubPartNo.Text;
                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editSubPartNo.Focus();
                editSubPartNo.SelectAll();
                return;
            }
            DialogResult = DialogResult.OK;
        }        

        private string GET_PART_ID(string sPartNo)
        {
            g_sItemPartID = "";
            g_sItemPartType = "";
            g_sItemSpec1 = "";
            g_sModelName="";

            sSQL = " Select A.PART_ID,A.PART_TYPE,A.SPEC1,B.MODEL_NAME from SAJET.SYS_PART A,SAJET.SYS_MODEL B  "
                 + " Where A.PART_NO = '" + sPartNo + "' "
                 + "  AND A.MODEL_ID = B.MODEL_ID(+) ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                g_sItemPartID = dsTemp.Tables[0].Rows[0]["PART_ID"].ToString();
                g_sItemPartType = dsTemp.Tables[0].Rows[0]["PART_TYPE"].ToString();
                g_sItemSpec1 = dsTemp.Tables[0].Rows[0]["SPEC1"].ToString();
                g_sModelName = dsTemp.Tables[0].Rows[0]["MODEL_NAME"].ToString();
                return g_sItemPartID;
            } 
            else
                return "0";
        }

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            LabPart.Text = g_sPartNo;
            LabVer.Text = g_sVer;

            //Step
            combStep.Items.Clear();
            combStep.Items.Add("");
            //Process
            combProcess.Items.Clear();
            combProcess.Items.Add("");
            sSQL = " Select DISTINCT B.PROCESS_NAME, B.PROCESS_ID from SAJET.SYS_PART_PROCESS_STEP A,"
                 + " SAJET.SYS_PROCESS B,"
                 + " SAJET.SYS_PART C"
                 + " Where A.PROCESS_ID = B.PROCESS_ID "
                 + " AND A.PART_ID = C.PART_ID "
                 + " AND C.PART_NO = '" + g_sPartNo + "' "
                 + " AND A.ENABLED = 'Y' "
                 + " AND B.ENABLED = 'Y' "
                 + " Order by PROCESS_NAME ";          //End Modify By Martin  2009/07/15
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                ComboBoxItem cb = new ComboBoxItem(dsTemp.Tables[0].Rows[i]["PROCESS_NAME"].ToString(), dsTemp.Tables[0].Rows[i]["PROCESS_ID"].ToString());
                combProcess.Items.Add(cb);
            }

            if (g_sProcess != "")
            {
                var item = combProcess.Items.OfType<ComboBoxItem>().FirstOrDefault(x => x.Text == g_sProcess);

                if (item != null)
                {
                    combProcess.SelectedItem = item;
                }
            }
            if (g_sKey != "")
                combKey.SelectedItem = g_sKey;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sPartNo = editSubPartNo.Text = editSubPartNo.Text.Trim();
            if (string.IsNullOrEmpty(sPartNo))
            {
                SajetCommon.Show_Message("Please Input Part No Prefix", 0);
                editSubPartNo.Focus();
                return;
            }
            sPartNo = sPartNo + "%";

            sSQL = @" select part_no,spec1,spec2 
                  from sajet.sys_part 
                  where enabled = 'Y' 
                  and part_no Like :PART_NO 
                  Order By part_no ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", sPartNo };
            fFilter f = new fFilter(Params);
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editSubPartNo.Text = f.dgvData.CurrentRow.Cells["part_no"].Value.ToString();
                //KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                //editSubPartNo_KeyPress(sender, Key);
            }
        }        

        private void editSubPartNo_KeyPress(object sender, KeyPressEventArgs e)
        {            
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }

            if (GET_PART_ID(editSubPartNo.Text) == "0")
            {
                SajetCommon.Show_Message("Sub Part No Error", 0);
                editSubPartNo.Focus();
                editSubPartNo.SelectAll();
                return;
            }
        }
        
        private void editSubPartNo_EnabledChanged(object sender, EventArgs e)
        {
            btnSearch.Enabled = editSubPartNo.Enabled;
        }

        private void combProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combProcess.SelectedIndex > 0)
            {
                //Step
                combStep.Items.Clear();
                combStep.Items.Add("");
                sSQL = $@" SELECT * FROM SAJET.SYS_PART_PROCESS_STEP A,
                           SAJET.SYS_STEP_ITEM B,
                           SAJET.SYS_PART C
                           Where A.STEP_ITEM_ID = B.STEP_ITEM_ID 
                           AND A.PART_ID = C.PART_ID 
                           AND C.PART_NO = '{g_sPartNo}' 
                           AND A.PROCESS_ID = {((ComboBoxItem)combProcess.SelectedItem).Value.ToString()} 
                           AND A.ENABLED = 'Y' 
                           AND B.ENABLED = 'Y' 
                           ORDER BY STEP_ITEM_NAME ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                {
                    ComboBoxItem cb = new ComboBoxItem(dsTemp.Tables[0].Rows[i]["STEP_ITEM_NAME"].ToString(), dsTemp.Tables[0].Rows[i]["STEP_ITEM_ID"].ToString());
                    combStep.Items.Add(cb);
                }
                if (g_sStep != "")
                {
                    var item = combStep.Items.OfType<ComboBoxItem>().FirstOrDefault(x => x.Text == g_sStep);

                    if (item != null)
                    {
                        combStep.SelectedItem = item;
                    }
                }

            }
        }
    }
}