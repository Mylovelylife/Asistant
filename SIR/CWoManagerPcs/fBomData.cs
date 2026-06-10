using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetFilter;
using SajetClass;

namespace CWoManagerPcs
{
    public partial class fBomData : Form
    {
        public fBomData()
        {
            InitializeComponent();
        }

        public string g_sProcessID, g_sSelectProcess;
        public string g_sItemPartID;
        public string g_sItemPartType;
        public string g_sItemSpec1;
        public bool g_sChangeGroup;

        public string g_sFIFO;
        public string g_sKP;

        private void bbtnOK_Click(object sender, EventArgs e)
        {
            string sSQL;
            DataSet DS;
            if (editSubPart.Text.Trim() == "")
            {
                string sData = LabSubPart.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editSubPart.Focus();
                return;
            }
            
            if (editQty.Text.Trim() == "0" | editQty.Text.Trim() == "")
            {
                SajetCommon.Show_Message("Qty Error", 0);
                editQty.Focus();
                editQty.SelectAll();
                return;
            }

            if (editSubPartVer.Text.Trim() == "")
                editSubPartVer.Text = "N/A";
            if (combProcess.Text.Trim() == "")
                g_sProcessID = "0";
            else
            {
                g_sProcessID = GET_FIELD_ID("SAJET.SYS_PROCESS", "PROCESS_NAME", "PROCESS_ID", combProcess.Text);                
            }

            if (GET_PART_ID(editSubPart.Text) == "0")
            {
                SajetCommon.Show_Message("Sub Part No Error", 0);
                editSubPart.Focus();
                return;
            }

            //若加入替代料,Group不可為0
            if ((g_sChangeGroup) & (editGroup.Text == "0" | editGroup.Text == ""))
            {
                SajetCommon.Show_Message("Please Change Relation (Relation<>0)", 0);
                editGroup.Focus();
                editGroup.SelectAll();
                return;
            }
            
            //是否重複
            sSQL = " Select ITEM_PART_ID from sajet.g_wo_bom "
                 + " Where WORK_ORDER ='" + LabWorkOrder.Text+ "' "
                 + " and NVL(Process_ID,'0') = '" + g_sProcessID + "' "
                 + " and ITEM_PART_ID = '" + g_sItemPartID + "' ";
            DS = ClientUtils.ExecuteSQL(sSQL);         
            if (DS.Tables[0].Rows.Count > 0)
            {
                string sData = LabSubPart.Text + " : " + editSubPart.Text;
                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private string GET_FIELD_ID(string sTable, string sFieldName, string sFieldID, string sFieldValue)
        {
            string sSQL = " Select " + sFieldID + " FIELD_ID from " + sTable
                        + " Where " + sFieldName + " = '" + sFieldValue+"' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);         
            if (DS.Tables[0].Rows.Count > 0)
                return DS.Tables[0].Rows[0]["FIELD_ID"].ToString();
            else
                return "0";
        }

        private string GET_PART_ID(string sPartNo)
        {
            g_sItemPartID = "";
            g_sItemPartType = "";
            g_sItemSpec1 = "";

            string sSQL = " Select PART_ID,PART_TYPE,SPEC1 from SAJET.SYS_PART "
                        + " Where PART_NO = '" + sPartNo + "' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);         
            if (DS.Tables[0].Rows.Count > 0)
            {
                g_sItemPartID = DS.Tables[0].Rows[0]["PART_ID"].ToString();
                g_sItemPartType = DS.Tables[0].Rows[0]["PART_TYPE"].ToString();
                g_sItemSpec1 = DS.Tables[0].Rows[0]["SPEC1"].ToString();
                return g_sItemPartID;
            } 
            else
                return "0";
        }

        private void btnSearchPart_Click(object sender, EventArgs e)
        {
            string sSQL = " select part_no,spec1,spec2 "
                + " from sajet.sys_part "
                + " where enabled = 'Y' "
                + " and part_no Like '" + editSubPart.Text + "%' "
                + " Order By part_no ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editSubPart.Text = f.dgvData.CurrentRow.Cells["part_no"].Value.ToString();              
            }
        }

        private void fBomData_Load(object sender, EventArgs e)
        {
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            
            SajetCommon.SetLanguageControl(this);
            combProcess.Items.Clear();
            combProcess.Items.Add("");
            string sSQL = "Select Process_Name from sajet.sys_process "
                        + "where enabled='Y' "
                        + "order by process_name ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combProcess.Items.Add(dsTemp.Tables[0].Rows[i]["Process_Name"].ToString());
            }
            combProcess.SelectedIndex = combProcess.Items.IndexOf(g_sSelectProcess);
        }

        private void editSubPart_EnabledChanged(object sender, EventArgs e)
        {
            btnSearchPart.Enabled = editSubPart.Enabled;
        }
    }
}