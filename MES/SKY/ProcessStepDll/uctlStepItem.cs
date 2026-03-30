using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetTable;

namespace ProcessStepDll
{
    public partial class uctlStepItem : UserControl
    {
        public delegate void m_OnRefresh();
        public event m_OnRefresh OnRefresh;
        private string g_sDataSQL;
        public string g_sOrderField;
        public string sUserID { set; get; }
        public string sFunction { set; get; }
        public string sProgram { set; get; }

        
        private MESGridView.Cache memoryCache;
        public uctlStepItem()
        {
            InitializeComponent();
        }
        public void Initial()
        {
            SajetCommon.SetLanguageControl(this);
            TableDefine.Initial_Table();

            //Filter - Detail
            combFilter.Items.Clear();
            for (int i = 0; i <= TableDefine.tGridDetailField.Length - 1; i++)
            {
                combFilter.Items.Add(TableDefine.tGridDetailField[i].sCaption);
                combFilterField.Items.Add(TableDefine.tGridDetailField[i].sFieldName);
            }
            if (combFilter.Items.Count > 0)
                combFilter.SelectedIndex = 0;

            g_sOrderField = TableDefine.gsDef_DtlOrderField;
            combShow.SelectedIndex = 0;
            /*
            combFilter.Items.Clear();
            combFilterField.Items.Clear();
            combFilter.Items.Add(SajetCommon.SetLanguage("Step Item Code"));
            combFilterField.Items.Add("STEP_ITEM_CODE");
            g_sOrderField = "STEP_ITEM_CODE";
            combFilter.Items.Add(SajetCommon.SetLanguage("Step Item Name"));
            combFilterField.Items.Add("STEP_ITEM_NAME");
            combShow.SelectedIndex = 0;
            if (combFilter.Items.Count > 0)
                combFilter.SelectedIndex = 0;
             */ 
            Check_Privilege(); //確認權限
            
        }
        private void Check_Privilege()  //取得權限部分
        {
            string sPrivilege = ClientUtils.GetPrivilege(sUserID, sFunction, sProgram).ToString();

            btnAppend.Enabled = SajetCommon.CheckEnabled("INSERT", sPrivilege);
            btnModify.Enabled = SajetCommon.CheckEnabled("UPDATE", sPrivilege);
            btnEnabled.Enabled = SajetCommon.CheckEnabled("ENABLED", sPrivilege);
            btnDisabled.Enabled = SajetCommon.CheckEnabled("DISABLED", sPrivilege);
            btnDelete.Enabled = SajetCommon.CheckEnabled("DELETE", sPrivilege);
        }
         
        public void ShowData()
        {
            string sSQL = "Select * from " + TableDefine.gsDef_DtlTable + " ";
            if (combShow.SelectedIndex == 0)
                sSQL = sSQL + " where Enabled = 'Y' ";
            else if (combShow.SelectedIndex == 1)
                sSQL = sSQL + " where Enabled = 'N' ";

            if (combFilter.SelectedIndex > -1 && editFilter.Text.Trim() != "")
            {
                string sFieldName = combFilterField.Items[combFilter.SelectedIndex].ToString();
                if (combShow.SelectedIndex <= 1)
                    sSQL = sSQL + " and ";
                else
                    sSQL = sSQL + " where ";
                sSQL = sSQL + sFieldName + " like '" + editFilter.Text.Trim() + "%'";
            }
            sSQL = sSQL + " order by " + g_sOrderField;
            g_sDataSQL = sSQL;
            (new MESGridView.DisplayGridView()).GetGridView(gvData, sSQL, out memoryCache);


            //欄位Title  
            for (int i = 0; i <= gvData.Columns.Count - 1; i++)
            {
                gvData.Columns[i].Visible = false;
            }
            for (int i = 0; i <= TableDefine.tGridDetailField.Length - 1; i++)
            {
                string sGridField = TableDefine.tGridDetailField[i].sFieldName;

                if (gvData.Columns.Contains(sGridField))
                {
                    gvData.Columns[sGridField].HeaderText = TableDefine.tGridDetailField[i].sCaption;
                    gvData.Columns[sGridField].DisplayIndex = i; //欄位顯示順序
                    gvData.Columns[sGridField].Visible = true;
                }
            }
            gvData.Focus();            
        }

        private void editFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            ShowData();
            SetSelectRow(gvData, "", "STEP_ITEM_ID");  //(顯示區域,"",ITEM_TYPE_ID)

            editFilter.Focus();
        }
        private void SetSelectRow(DataGridView GridData, String sPrimaryKey, String sField)
        {
            if (GridData.Rows.Count > 0)
            {
                int iIndex = 0;
                string sShowField = GridData.Columns[0].Name;
                for (int i = 0; i <= GridData.Columns.Count - 1; i++)
                {
                    if (GridData.Columns[i].Visible)
                    {
                        //第一個有顯示的欄位(focus到隱藏欄位會錯誤)
                        sShowField = GridData.Columns[i].Name;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(sPrimaryKey))
                {
                    string sCondition = "";
                    string[] tsField = sField.Split(',');
                    string[] tsValue = sPrimaryKey.Split(',');
                    for (int j = 0; j <= tsField.Length - 1; j++)
                    {
                        if (j == 0)
                            sCondition = " Where " + tsField[j].ToString() + "='" + tsValue[j].ToString() + "' ";
                        else
                            sCondition = sCondition + " and " + tsField[j].ToString() + "='" + tsValue[j].ToString() + "' ";

                    }
                    //改用SQL找,不由Grid讀值,否則速度會慢
                    string sDataSQL = g_sDataSQL;
                   
                    string sText = "select idx from ("
                                 + " Select aa.*,rownum-1 idx from ("
                                 + sDataSQL
                                 + " ) aa ) "
                                 + sCondition;
                    DataSet ds = ClientUtils.ExecuteSQL(sText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        iIndex = Convert.ToInt32(ds.Tables[0].Rows[0]["idx"].ToString());
                    }
                }
                GridData.Focus();
                GridData.CurrentCell = GridData.Rows[iIndex].Cells[sShowField];
                GridData.Rows[iIndex].Selected = true;
            }
        }

        private void combShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Visible = (combShow.SelectedIndex == 1);
            btnDisabled.Visible = (combShow.SelectedIndex == 0);
            btnEnabled.Visible = (combShow.SelectedIndex == 1);
            //顯示查詢資訊
            ShowData();
            SetSelectRow(gvData, "", "STEP_ITEM_ID");  
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {

            fDetailData f = new fDetailData();
            try
            {
                f.OnAppend += new fDetailData.m_OnAppend(f_OnAppend);
                f.g_sUpdateType = "APPEND";
                f.g_sformText = btnAppend.Text;  //當按下新增時，將btnAppend的值顯示於主畫面標頭上
                f.g_sProcessID = "0";
                f.g_sProcessName = "N/A";               
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    SetSelectRow(gvData, "", "STEP_ITEM_ID");
                }
            }
            finally
            {
                f.Dispose();
            }
        }

        void f_OnAppend(string f_StepItemCode)
        {
            ShowData();
            SetSelectRow(gvData, f_StepItemCode, "STEP_ITEM_CODE");
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            fDetailData f = new fDetailData();
            try
            {
                f.dataCurrentRow = gvData.CurrentRow;
                string sStepItemName = gvData.CurrentRow.Cells["STEP_ITEM_NAME"].Value.ToString();
                string sStepItemCode = gvData.CurrentRow.Cells["STEP_ITEM_CODE"].Value.ToString();               
                f.g_sProcessID = "0";
                f.g_sProcessName = "N/A";

                f.g_sUpdateType = "MODIFY";
                f.g_sformText = btnModify.Text;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    SetSelectRow(gvData, sStepItemCode, "STEP_ITEM_CODE");
                }
            }
            finally
            {
                f.Dispose();
            }
        }

        private void btnDisabled_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sID = gvData.CurrentRow.Cells["STEP_ITEM_ID"].Value.ToString();  //資料表中的ITEM_TYPE_ID欄位名稱值
            string sType = "";
            string sEnabled = "";
            if (sender == btnDisabled)
            {
                sType = btnDisabled.Text;   //sType = Disabled
                sEnabled = "N";
            }
            else if (sender == btnEnabled)
            {
                sType = btnEnabled.Text;  //sType = Enabled
                sEnabled = "Y";
            }
            //Columns欄位名稱;CurrentRow欄位值
            string sData = gvData.Columns["STEP_ITEM_CODE"].HeaderText + " : " + gvData.CurrentRow.Cells["STEP_ITEM_CODE"].Value.ToString();
            string sMsg = sType + " ?" + Environment.NewLine + sData; //{Disabled;Enabled}?+sData

            if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                return;

            string sSQL  = " Update  SAJET.SYS_STEP_ITEM "
                 + " set Enabled = '" + sEnabled + "'  "
                 + "    ,UPDATE_USERID = '"+sUserID+"' "
                 + "    ,UPDATE_TIME = SYSDATE  "
                 + " where STEP_ITEM_ID = '" + sID + "'";
            ClientUtils.ExecuteSQL(sSQL);
            CopyToHistory(sID);

            ShowData();
            SetSelectRow(gvData, "","STEP_ITEM_ID");
            OnRefresh();
        }
        private void CopyToHistory(string sID)
        {
            string sSQL = @"INSERT INTO SAJET.SYS_HT_STEP_ITEM SELECT * FROM SAJET.SYS_STEP_ITEM WHERE STEP_ITEM_ID='" + sID + "' ";
            ClientUtils.ExecuteSQL(sSQL);
        }

        private void btnEnabled_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sID = gvData.CurrentRow.Cells["STEP_ITEM_ID"].Value.ToString();  //資料表中的ITEM_TYPE_ID欄位名稱值
            string sType = "";
            string sEnabled = "";
            if (sender == btnDisabled)
            {
                sType = btnDisabled.Text;   //sType = Disabled
                sEnabled = "N";
            }
            else if (sender == btnEnabled)
            {
                sType = btnEnabled.Text;  //sType = Enabled
                sEnabled = "Y";
            }
            //Columns欄位名稱;CurrentRow欄位值
            string sData = gvData.Columns["STEP_ITEM_CODE"].HeaderText + " : " + gvData.CurrentRow.Cells["STEP_ITEM_CODE"].Value.ToString();
            string sMsg = sType + " ?" + Environment.NewLine + sData; //{Disabled;Enabled}?+sData

            if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                return;

            string sSQL = " Update  SAJET.SYS_STEP_ITEM "
               + "   SET Enabled = '" + sEnabled + "'  "
               + "    ,UPDATE_USERID = '" + sUserID + "' "
               + "    ,UPDATE_TIME = SYSDATE  "
               + " where STEP_ITEM_ID = '" + sID + "'";
         
            ClientUtils.ExecuteSQL(sSQL);
            CopyToHistory(sID);

            ShowData();
            SetSelectRow(gvData, "", "STEP_ITEM_ID");
            OnRefresh();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            try
            {
                string sID = gvData.CurrentRow.Cells["STEP_ITEM_ID"].Value.ToString();  //資料表中的ITEM_TYPE_ID欄位名稱值
                string sData = gvData.Columns["STEP_ITEM_CODE"].HeaderText + " : " + gvData.CurrentRow.Cells["STEP_ITEM_CODE"].Value.ToString();

                
                string sMsg = btnDelete.Text + " ?" + Environment.NewLine + sData;
                if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                    return;

                string sSQL = " Update SAJET.SYS_STEP_ITEM "
                     + " set Enabled = 'Drop'  "                            //將狀態更新為Drop
                     + "    ,UPDATE_USERID = '" + sUserID + "'  "
                     + "    ,UPDATE_TIME = SYSDATE  "
                     + " where STEP_ITEM_ID  = '" + sID + "'";
                ClientUtils.ExecuteSQL(sSQL);
                CopyToHistory(sID);  //將刪除動作紀錄至歷史區
                //刪除主資料部分
                sSQL = " Delete  SAJET.SYS_STEP_ITEM  "
                     + " where STEP_ITEM_ID = '" + sID + "'";
                ClientUtils.ExecuteSQL(sSQL);

                ShowData();
                SetSelectRow(gvData, "", "STEP_ITEM_ID");
                OnRefresh();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.Filter = "All Files(*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            string sFileName = saveFileDialog1.FileName;

            ExportExcel.CreateExcel Export = new ExportExcel.CreateExcel(sFileName);
            if (sender == btnExport)
                Export.ExportToExcel(gvData);
            
        }

        private void gvData_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = memoryCache.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }

        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dvControl = (DataGridView)contextMenuStrip1.SourceControl;

            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            string sID = gvData.CurrentRow.Cells["STEP_ITEM_ID"].Value.ToString();
                           string sSQL = " Select a.STEP_ITEM_CODE,a.STEP_ITEM_NAME "
                     + "       ,a.ENABLED,b.emp_name,a.UPDATE_TIME "
                     + " from  SAJET.SYS_HT_STEP_ITEM A "
                     + "     ,sajet.sys_emp b "
                     + " Where a.STEP_ITEM_ID='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " Order By a.Update_Time ";
            
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            fHistory fHistory = new fHistory();
            fHistory.dgvHistory.DataSource = dsTemp;
            fHistory.dgvHistory.DataMember = dsTemp.Tables[0].ToString();
            //替換欄位名稱
            for (int i = 0; i <= fHistory.dgvHistory.Columns.Count - 1; i++)
            {
                string sGridField = fHistory.dgvHistory.Columns[i].HeaderText;
                string sField = "";
                for (int j = 0; j <= dvControl.Columns.Count - 1; j++)
                {
                    sField = dvControl.Columns[j].Name;
                    if (sGridField == sField)
                    {
                        fHistory.dgvHistory.Columns[i].HeaderText = dvControl.Columns[j].HeaderText;
                        break;
                    }
                }
            }
            fHistory.ShowDialog();
            fHistory.Dispose();
        }
    }
}
