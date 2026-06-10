using SajetClass;
using SajetFilter;
using System;
using System.Data;
using System.Windows.Forms;

namespace CWoManagerPcs
{
    public partial class fBomData_New : Form
    {
        public string g_sProcessID, g_sSelectProcess, g_sOldProcess;
        public string g_sItemPartID;
        public string g_sItemPartType;
        public string g_sItemSpec1;
        public string g_sFunc;
        public bool g_sChangeGroup;
        public string g_sBomType;
        public string g_sBomTypeText;
        public string g_sRouteID;


        public fBomData_New()
        {
            InitializeComponent();
        }
        private void btnOK_Click(object sender, EventArgs e)
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
                 + " Where WORK_ORDER ='" + LabWorkOrder.Text + "' "
                 + " and NVL(Process_ID,'0') = '" + g_sProcessID + "' "
                 + " and ITEM_PART_ID = '" + g_sItemPartID + "' ";
            DS = ClientUtils.ExecuteSQL(sSQL);

            if (DS.Tables[0].Rows.Count > 1
                || (DS.Tables[0].Rows.Count > 0 && g_sFunc != "MODIFY")
                || (g_sFunc == "MODIFY" && g_sOldProcess != g_sSelectProcess && DS.Tables[0].Rows.Count > 0))
            {
                string sData = LabSubPart.Text + " : " + editSubPart.Text;
                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                return;
            }

            if (rbtnPart.Checked)
            {
                g_sBomType = "0";
                g_sBomTypeText = rbtnPart.Text;
            }
            else if (rbtnKP.Checked)
            {
                g_sBomType = "1";
                g_sBomTypeText = rbtnKP.Text;
            }
            else if (rbtnLot.Checked)
            {
                g_sBomType = "2";
                g_sBomTypeText = rbtnLot.Text;
            }
            else
            {
                g_sBomType = "1";
                g_sBomTypeText = rbtnPart.Text;
            }
            DialogResult = DialogResult.OK;
        }

        private string GET_FIELD_ID(string sTable, string sFieldName, string sFieldID, string sFieldValue)
        {
            string sSQL = " Select " + sFieldID + " FIELD_ID from " + sTable
                        + " Where " + sFieldName + " = '" + sFieldValue + "' ";
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

        private void tbChkString_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbKPSNLen.Text, out int length))
            {
                if (int.TryParse(tbChkIndex.Text, out int index))
                {
                    if (tbChkString.Text.Length > length - index + 1)
                    {
                        tbChkString.Text = tbChkString.Text.Substring(0, length - index + 1);
                    }
                }
                else
                {
                    if (tbChkString.Text.Length > length)
                    {
                        tbChkString.Text = tbChkString.Text.Substring(0, length);
                    }
                }

            }
            else
            {
                tbChkString.Text = "";
            }
        }

        private void tbKPSNLen_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                // 如果输入非数字，则取消输入
                e.Handled = true;
            }
        }

        private void tbChkIndex_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                // 如果输入非数字，则取消输入
                e.Handled = true;
            }
        }

        private void tbChkIndex_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbKPSNLen.Text, out int length))
            {
                tbKPSNLen.Text = length.ToString();

                if (int.TryParse(tbChkIndex.Text, out int index))
                {
                    if (length < index)
                    {
                        tbChkIndex.Text = tbKPSNLen.Text;
                        index = length;
                    }
                    if (tbChkString.Text.Length > length - index + 1)
                    {
                        tbChkString.Text = tbChkString.Text.Substring(0, length - index + 1);
                    }
                }
                else
                {
                    if (tbChkString.Text.Length > length)
                    {
                        tbChkString.Text = tbChkString.Text.Substring(0, length);
                    }
                }

            }
            else
            {
                tbChkString.Text = "";
                tbChkIndex.Text = "";
            }
        }

        private void tbKPSNLen_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbKPSNLen.Text, out int length))
            {
                if (int.TryParse(tbChkIndex.Text, out int index))
                {
                    if (length < index)
                    {
                        index = length;
                    }
                    tbChkIndex.Text = index.ToString();
                    if (tbChkString.Text.Length > length - index + 1)
                    {
                        tbChkString.Text = tbChkString.Text.Substring(0, length - index + 1);
                    }
                }
                else
                {
                    if (tbChkString.Text.Length > length)
                    {
                        tbChkString.Text = tbChkString.Text.Substring(0, length);
                    }
                }
            }
            else
            {
                tbChkString.Text = "";
            }
        }

        private void combProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            g_sSelectProcess = string.IsNullOrWhiteSpace(combProcess.SelectedText) ? "N/A" : combProcess.SelectedText;
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
            string sSQL = $@"SELECT PROCESS_NAME
  FROM SAJET.SYS_PROCESS
 WHERE ENABLED = 'Y'
   AND PROCESS_ID IN (SELECT NEXT_PROCESS_ID
                        FROM SAJET.SYS_ROUTE_DETAIL
                       WHERE ROUTE_ID = '{g_sRouteID}')
 ORDER BY PROCESS_NAME";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combProcess.Items.Add(dsTemp.Tables[0].Rows[i]["Process_Name"].ToString());
            }
            combProcess.SelectedIndex = combProcess.Items.IndexOf(g_sSelectProcess);
            combProcess.SelectedIndexChanged += new System.EventHandler(this.combProcess_SelectedIndexChanged);

            switch (g_sBomType)
            {
                case "0":
                    rbtnPart.Checked = true; break;
                case "1":
                    rbtnKP.Checked = true; break;
                case "2":
                    rbtnLot.Checked = true; break;
                default:
                    rbtnKP.Checked = true; break;
            }
        }

        private void editSubPart_EnabledChanged(object sender, EventArgs e)
        {
            btnSearchPart.Enabled = editSubPart.Enabled;
        }

    }
}