using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetFilter;

namespace CWoManagerPcs
{
    public partial class fProcessLink : Form
    {
        public fProcessLink()
        {
            InitializeComponent();
        }

        public string g_sModelID;
        public string g_sWorkOrder;
        public string g_sRouteID;
        public string g_sModelName;

        private void editFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != 13)
                return;

            ShowProcess();
        }

        private void bbtnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= LVAll.Items.Count - 1; i++)
            {
                LVAll.Items[i].Checked = true;
            }

        }

        private void bbtnSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= LVAll.Items.Count - 1; i++)
            {
                LVAll.Items[i].Checked = false;
            }
        }

        private void bbtnChoose_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= LVAll.Items.Count - 1; i++)
            {
                if (LVAll.Items[i].Checked)
                {
                    //Find必須用Name來找,因此將Name設成跟Text相同
                    if (LVChoose.Items.Find(LVAll.Items[i].Text, false).Length == 0)
                    {
                        LVChoose.Items.Add(LVAll.Items[i].Text);
                        LVChoose.Items[LVChoose.Items.Count - 1].SubItems.Add(LVAll.Items[i].SubItems[1].Text);
                        LVChoose.Items[LVChoose.Items.Count - 1].SubItems.Add("");
                        LVChoose.Items[LVChoose.Items.Count - 1].SubItems.Add(LVAll.Items[i].SubItems[2].Text);
                        LVChoose.Items[LVChoose.Items.Count - 1].Name = LVAll.Items[i].Text;
                    }
                }
            }
        }

        private void fProcessLink_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            LabModel.Text = g_sModelName;

            LVChoose.Items.Clear();
            string sSQL = string.Empty;
            if (!string.IsNullOrEmpty(g_sModelID))
            {
                sSQL = $@"SELECT B.PROCESS_NAME, B.PROCESS_CODE, B.PROCESS_ID, A.OPERATION_SEQ
  FROM SAJET.SYS_MODEL_PROCESS A
  LEFT JOIN SAJET.SYS_PROCESS B
    ON A.PROCESS_ID = B.PROCESS_ID
 WHERE A.MODEL_ID = '{g_sModelID}'
   AND B.ENABLED = 'Y'
 ORDER BY B.PROCESS_CODE";
            }
            else
            {
                sSQL = $@"SELECT B.PROCESS_NAME, B.PROCESS_CODE, B.PROCESS_ID, A.OPERATION_SEQ
  FROM SAJET.SYS_MODEL_PROCESS A
  LEFT JOIN SAJET.SYS_PROCESS B
    ON A.PROCESS_ID = B.PROCESS_ID
 WHERE A.WORK_ORDER = '{g_sWorkOrder}'
   AND B.ENABLED = 'Y'
 ORDER BY B.PROCESS_CODE";
            }

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                LVChoose.Items.Add(dsTemp.Tables[0].Rows[i]["PROCESS_CODE"].ToString());
                LVChoose.Items[LVChoose.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["PROCESS_NAME"].ToString());
                LVChoose.Items[LVChoose.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["OPERATION_SEQ"].ToString());
                LVChoose.Items[LVChoose.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["PROCESS_ID"].ToString());
                LVChoose.Items[LVChoose.Items.Count - 1].Name = dsTemp.Tables[0].Rows[i]["PROCESS_CODE"].ToString();
            }
            ShowProcess();
        }

        public void ShowProcess()
        {
            LVAll.Items.Clear();
            string sPartFilter = editFilter.Text.Trim() + "%";
            string sSQL = $@"SELECT PROCESS_CODE, PROCESS_NAME, PROCESS_ID
  FROM SAJET.SYS_PROCESS
 WHERE PROCESS_ID IN (SELECT NEXT_PROCESS_ID
                        FROM SAJET.SYS_ROUTE_DETAIL
                       WHERE ROUTE_ID = '{g_sRouteID}')
";
            if (!string.IsNullOrEmpty(editFilter.Text.Trim()))
                sSQL = sSQL + $@"   AND PROCESS_NAME LIKE '{sPartFilter}' 
";
            sSQL = sSQL + " ORDER BY PROCESS_CODE";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                LVAll.Items.Add(dsTemp.Tables[0].Rows[i]["PROCESS_CODE"].ToString());
                LVAll.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i]["PROCESS_NAME"].ToString());
                LVAll.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i]["PROCESS_ID"].ToString());
            }
        }

        private void bbtnSave_Click(object sender, EventArgs e)
        {
            string sSQL = "";

            for (int i = 0; i <= LVChoose.Items.Count - 1; i++)
            {
                string sOperationSEQ = LVChoose.Items[i].SubItems[2].Text;
                if (string.IsNullOrEmpty(sOperationSEQ))
                {
                    continue;
                }
                string sProcessID = LVChoose.Items[i].SubItems[3].Text;
                if (string.IsNullOrEmpty(sSQL))
                    sSQL = $@"SELECT '{g_sModelID}',
           '{g_sWorkOrder}',
           '{sProcessID}',
           '{sOperationSEQ}',
           '{ClientUtils.UserPara1}',
           SYSDATE
      FROM DUAL";
                else
                    sSQL += $@"
    UNION ALL
    SELECT '{g_sModelID}',
           '{g_sWorkOrder}',
           '{sProcessID}',
           '{sOperationSEQ}',
           '{ClientUtils.UserPara1}',
           SYSDATE
      FROM DUAL";

            }
            if (string.IsNullOrEmpty(sSQL))
            {
                if (!string.IsNullOrWhiteSpace(g_sModelID))
                {
                    sSQL = $"DELETE FROM SAJET.SYS_MODEL_PROCESS A WHERE A.MODEL_ID = '{g_sModelID}'";
                }
                else
                {
                    sSQL = $"DELETE FROM SAJET.SYS_MODEL_PROCESS A WHERE A.WORK_ORDER = '{g_sWorkOrder}'";
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(g_sModelID))
                {
                    sSQL = $@"BEGIN
  DELETE FROM SAJET.SYS_MODEL_PROCESS A WHERE A.MODEL_ID = '{g_sModelID}';
  
  INSERT INTO SAJET.SYS_MODEL_PROCESS A
    (A.MODEL_ID,
     A.WORK_ORDER,
     A.PROCESS_ID,
     A.OPERATION_SEQ,
     A.UPDATE_USERID,
     A.UPDATE_TIME)
    {sSQL};
END;";
                }
                else
                {
                    sSQL = $@"BEGIN
  DELETE FROM SAJET.SYS_MODEL_PROCESS A WHERE A.WORK_ORDER = '{g_sWorkOrder}';
  
  INSERT INTO SAJET.SYS_MODEL_PROCESS A
    (A.MODEL_ID,
     A.WORK_ORDER,
     A.PROCESS_ID,
     A.OPERATION_SEQ,
     A.UPDATE_USERID,
     A.UPDATE_TIME)
    {sSQL};
END;";
                }

            }

            ClientUtils.ExecuteSQL(sSQL);
            DialogResult = DialogResult.OK;
        }

        private void bbtnRemove_Click(object sender, EventArgs e)
        {
            for (int i = LVChoose.Items.Count - 1; i >= 0; i--)
            {
                if (LVChoose.Items[i].Checked)
                    LVChoose.Items[i].Remove();
            }
        }

        private void bbtnSelectAll1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= LVChoose.Items.Count - 1; i++)
            {
                LVChoose.Items[i].Checked = true;
            }
        }

        private void bbtnSelectNone1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= LVChoose.Items.Count - 1; i++)
            {
                LVChoose.Items[i].Checked = false;
            }
        }

        private void btnSetProcess_Click(object sender, EventArgs e)
        {
            List<int> indexList = new List<int>();
            foreach (ListViewItem item in LVChoose.Items)
            {
                if (item.Checked)
                {
                    indexList.Add(item.Index);
                }
            }

            if (indexList.Count == 0)
            {
                SajetCommon.Show_Message("Pls select process", 1);
                return;
            }

            string sSQL = $@"SELECT T.PARAM_VALUE
  FROM SAJET.SYS_BASE T
 WHERE T.PROGRAM = 'Data Center'
   AND T.PARAM_NAME = 'OPERATION SEQ'";
            var dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];
            if (dtTemp.Rows.Count <= 0)
            {
                SajetCommon.Show_Message("Operation SEQ is null", 1);
                return;
            }
            string sParamVal = dtTemp.Rows[0][0].ToString();
            if (string.IsNullOrWhiteSpace(sParamVal))
            {
                SajetCommon.Show_Message("Operation SEQ is null", 1);
                return;
            }
            fFilter f = new fFilter();
            f.sSQL = $@"SELECT COLUMN_VALUE ""Operation SEQ"" FROM TABLE(SAJET.SPLITSTR('{sParamVal}', ',')) ";
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (f.dgvData.CurrentRow == null)
                    return;
                string operationSEQ = f.dgvData.CurrentRow.Cells[0].Value.ToString();
                foreach (ListViewItem item in LVChoose.Items)
                {
                    if (item.SubItems[2].Text == operationSEQ)
                    {
                        SajetCommon.Show_Message("Operation SEQ is set", 1);
                        return;
                    }
                }

                foreach (int index in indexList)
                {
                    LVChoose.Items[index].SubItems[2].Text = f.dgvData.CurrentRow.Cells[0].Value.ToString();
                }
            }
        }

        private void LVChoose_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked == true)
            {
                foreach (ListViewItem item in LVChoose.Items)
                {
                    if (item.Checked == true && item.Text != e.Item.Text)
                    {
                        item.Checked = false;
                    }
                }
            }
        }
    }
}