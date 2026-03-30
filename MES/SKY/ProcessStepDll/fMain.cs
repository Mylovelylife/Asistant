using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
//using SajetTable;
using System.IO;
using System.Reflection;

namespace ProcessStepDll
{
    public partial class fMain : Form
    {
        private MESGridView.Cache memoryCache;

        public fMain()
        {
            InitializeComponent();
        }

        public static string g_sUserID;
        public string g_sProgram, g_sFunction;
        public string g_sOrderField;
        string g_sDataSQL;
        ToolingUtils ToolUtils = new ToolingUtils();

        uctlProcessTooling objProcessTooling;
        ImportFile _myImportFile = new ImportFile();

        private void Initial_Form()
        {
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
            //  g_sOrderField = TableDefine.gsDef_OrderField;

            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            PropertyInfo info = this.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            info.SetValue(tabControl1, true, null);

        }
        private void fMain_Load(object sender, EventArgs e)
        {

            Initial_Form();

            this.Text = this.Text + "(" + SajetCommon.g_sFileVersion + ")";
            ToolUtils.sFunType = ClientUtils.fParameter;
            ToolUtils.sEmpID = ClientUtils.UserPara1;
            //Filter
            combFilter.Items.Clear();
            combFilterField.Items.Clear();
                combFilter.Items.Add(SajetCommon.SetLanguage("Process Code"));
                combFilterField.Items.Add("PROCESS_CODE");
                g_sOrderField = "PROCESS_CODE";
                combFilter.Items.Add(SajetCommon.SetLanguage("Process Name"));
                combFilterField.Items.Add("PROCESS_NAME");
                combFilter.Items.Add(SajetCommon.SetLanguage("Process Desc"));
                combFilterField.Items.Add("PROCESS_DESC");
                //g_sOrderField = "NVL(PROCESS_CODE,PROCESS_NAME) ";
            if (combFilter.Items.Count > 0)
                combFilter.SelectedIndex = 0;

            Check_Privilege();
            objProcessTooling = new uctlProcessTooling();
            objProcessTooling.Dock = DockStyle.Fill;
            objProcessTooling.OnDeleteProcess += new uctlProcessTooling.m_OnDeleteProcess(objProcessTooling_OnDeleteProcess);
            objProcessTooling.OnDeleteToolingSN += new uctlProcessTooling.m_OnDeleteToolingSN(objProcessTooling_OnDeleteToolingSN);
            objProcessTooling.OnUpdateToolingQTY += new uctlProcessTooling.m_OnUpdateToolingQTY(objProcessTooling_OnUpdateToolingQTY);

            objProcessTooling.sFunType = ToolUtils.sFunType;
            objProcessTooling.Parent = tabPage2;
            objProcessTooling.bDeletePrivilege = btnMaintain.Enabled;
            ShowData();
            _myImportFile.OnCompleted += new ProcessStepDll.ImportFile.m_OnCompleted(_myImportFile_OnCompleted);
            _myImportFile.OnReportStatus += new ProcessStepDll.ImportFile.m_OnReportStatus(_myImportFile_OnReportStatus);
            tabPage1.Parent = null;
            btnImport.Visible = false;
            
            uctlStepItem1.sUserID = g_sUserID;
            uctlStepItem1.sFunction = g_sFunction;
            uctlStepItem1.sProgram = g_sProgram;
            uctlStepItem1.Initial();
            uctlStepItem1.ShowData();
            uctlStepItem1.OnRefresh += new uctlStepItem.m_OnRefresh(uctlStepItem1_OnRefresh);
        }

        void uctlStepItem1_OnRefresh()
        {
            ShowData();
        }



        void objProcessTooling_OnDeleteToolingSN(DateTime dtDateTime, string sProcessName, string sToolingNO, string sToolingSN)
        {
            ToolUtils.DeleteToolingSN(dtDateTime, sProcessName, sToolingNO);
            string sValue = ToolUtils.sPKFieldName;
            ShowData();
            SetSelectRow(gvData, sValue, "PROCESS_NAME");

        }

        void objProcessTooling_OnUpdateToolingQTY(string sProcessName, string sToolingNO, string iQTY)
        {
            ToolUtils.UpdateToolingQTY(sProcessName, sToolingNO, iQTY);
            string sValue = ToolUtils.sPKFieldName;
            ShowData();
            SetSelectRow(gvData, sValue, "PROCESS_NAME");

        }

        void objProcessTooling_OnDeleteProcess(string sProcessName)
        {
            ToolUtils.DeleteProcess(sProcessName);
            string sValue = ToolUtils.sPKFieldName;
            ShowData();
            SetSelectRow(gvData, sValue, "PROCESS_NAME");
        }

        private void Check_Privilege()
        {
            string sPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram).ToString();
            int iPrivilege = 0;
            try
            {
                iPrivilege = Convert.ToInt32(sPrivilege);
            }
            catch
            {
            }
            btnMaintain.Enabled = (iPrivilege >= 1);
            btnCopy.Enabled = btnMaintain.Enabled;
        }
        public void ShowData()
        {
            gvData.Rows.Clear();
            objProcessTooling.SetClear();
            objProcessTooling.sPKFieldIDValue = "";

            string sSQL = @"  WITH PROCESS_STEP AS 
                         (SELECT PROCESS_ID,COUNT(*) QTY 
                           FROM SAJET.SYS_PROCESS_STEP A,SAJET.SYS_STEP_ITEM B 
                           WHERE A.STEP_ITEM_ID = B.STEP_ITEM_ID 
                            AND B.ENABLED='Y'
                            GROUP BY PROCESS_ID) 
                          SELECT A.PROCESS_ID,A.PROCESS_CODE,A.PROCESS_NAME,A.PROCESS_DESC,NVL(B.QTY,0) STEP_ITEM_COUNT
                            FROM SAJET.SYS_PROCESS A,PROCESS_STEP B 
                          WHERE A.ENABLED='Y' AND A.PROCESS_ID = B.PROCESS_ID(+)   ";
                         
            if (combFilter.SelectedIndex > -1 && editFilter.Text.Trim() != "")
            {
                string sFieldName = combFilterField.Items[combFilter.SelectedIndex].ToString();
                sSQL = sSQL + " and " + sFieldName + " like '" + editFilter.Text.Trim() + "%'";
            }
            sSQL = sSQL + " order by " + g_sOrderField;
            g_sDataSQL = sSQL;
            (new MESGridView.DisplayGridView()).GetGridView(gvData, sSQL, out memoryCache);
            gvData.Columns[1].Frozen = true;
            gvData.Columns[0].Visible = false;
            for (int i = 0; i <= gvData.Columns.Count - 1; i++)
                gvData.Columns[i].HeaderText = SajetCommon.SetLanguage(gvData.Columns[i].HeaderText);
            gvData.Focus();
        }



        private void editFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

          
            
            ShowData();
            SetSelectRow(gvData, "", "PROCESS_NAME");
            editFilter.Focus();
        }

        private void SetSelectRow(DataGridView GridData, string sPrimaryKey, string sField)
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
                    string sText = "select idx from ("
                                 + " Select aa.*,rownum-1 idx from ("
                                 + g_sDataSQL
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

        private void gvData_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = memoryCache.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }
        private void gvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                ShowData();
                SetSelectRow(gvData, "", "PROCESS_NAME");
            }

        }

        private void gvData_SelectionChanged(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            string sModel = gvData.CurrentRow.Cells["PROCESS_ID"].Value.ToString();
            ToolUtils.sPKFieldIDValue = sModel;
            ToolUtils.sPKFieldName = gvData.CurrentRow.Cells["PROCESS_NAME"].Value.ToString();
            ShowProcessTooling(sModel);
        }
        private void ShowProcessTooling(string sModelID)
        {
            objProcessTooling.sPKFieldIDValue = sModelID;
            objProcessTooling.ShowTooling();
        }
        private void btnMaintain_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sModel = gvData.CurrentRow.Cells[0].Value.ToString();
            ToolUtils.sPKFieldIDValue = sModel;
            ToolUtils.sPKFieldName = gvData.CurrentRow.Cells["PROCESS_NAME"].Value.ToString();
            string sTag = (sender as Button).Tag.ToString();
            switch (sTag)
            {
                case "1":
                    fProcessToolingLink f = new fProcessToolingLink(ToolUtils);
                    try
                    {
                        f.ShowDialog();
                        ShowProcessTooling(sModel);
                        string sValue = ToolUtils.sPKFieldName;
                        ShowData();
                        SetSelectRow(gvData, sValue, "PROCESS_NAME");

                    }
                    finally
                    {
                        f.Dispose();
                    }
                    break;
                case "2":
                    if (objProcessTooling == null || objProcessTooling.iRowCount == 0)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Process Name")+" : "+
                            ToolUtils.sPKFieldName + Environment.NewLine
                                               + SajetCommon.SetLanguage("No Step Item Data"), 0);
                        break;
                    }
                    fCopyTo fCopy = new fCopyTo(ToolUtils);
                    try
                    {
                        if (fCopy.ShowDialog() == DialogResult.OK)
                        {
                            if (fCopy.iCopyCount > 0)
                            {
                                SajetCommon.Show_Message("Copy Finished", -1);
                                string sValue = ToolUtils.sPKFieldName;
                                ShowData();
                                SetSelectRow(gvData, sValue, "PROCESS_NAME");
                            }
                        }
                    }
                    finally
                    {
                        fCopy.Dispose();
                    }
                    break;
                default:
                    break;
            }

        }

        private void ImportFile(DataTable dtImportData)
        {
            Dictionary<string, string> dictPart = new Dictionary<string, string>();
            Dictionary<string, string> dictProcess = new Dictionary<string, string>();
            Dictionary<string, string> dictTooling = new Dictionary<string, string>();
            foreach (DataRow dr in dtImportData.Rows)
            {
                string sResult = "";
                string sPartNo = dr["PART_NO"].ToString();
                string sProcessName = dr["PROCESS_NAME"].ToString();
                string sToolingNo = dr["TOOLING_NO"].ToString();
                string sQty = dr["QTY"].ToString();
                string sPartID;
                string sProcessID;
                string sToolingID;
                dictPart.TryGetValue(sPartNo, out sPartID);
                if (sPartID == null)
                {
                    if (ToolUtils.sFunType == "MODEL")
                        sPartID = SajetCommon.GetID("SAJET.SYS_MODEL", "MODEL_ID", "MODEL_NAME", sPartNo);
                    else
                        sPartID = SajetCommon.GetID("SAJET.SYS_PART", "PART_ID", "PART_NO", sPartNo);
                    if (sPartID == "0")
                        sResult = SajetCommon.SetLanguage("Part/Model Error");
                    else
                        dictPart.Add(sPartNo, sPartID);
                }
                dictProcess.TryGetValue(sProcessName, out sProcessID);
                if (sProcessID == null)
                {
                    sProcessID = SajetCommon.GetID("SAJET.SYS_PROCESS", "PROCESS_ID", "PROCESS_NAME", sProcessName);
                    if (sProcessID == "0")
                        sResult = SajetCommon.SetLanguage("Process Error");
                    else
                        dictProcess.Add(sProcessName, sProcessID);
                }
                dictTooling.TryGetValue(sToolingNo, out sToolingID);
                if (sToolingID == null)
                {
                    sToolingID = SajetCommon.GetID("SAJET.SYS_TOOLING", "TOOLING_ID", "TOOLING_NO", sToolingNo);
                    if (sToolingID == "0")
                        sResult = SajetCommon.SetLanguage("Tooling Error");
                    else
                        dictTooling.Add(sToolingNo, sToolingID);
                }
                int iQty = 0;
                try
                {
                    iQty = Convert.ToInt32(sQty);
                }
                catch
                {
                    sResult = SajetCommon.SetLanguage("Qty Error");
                }

                if (sPartID != "0" && sProcessID != "0" && sToolingID != "0" && string.IsNullOrEmpty(sResult))
                {
                    ToolUtils.sPKFieldIDValue = sPartID;
                    try
                    {
                        if (ToolUtils.ToolingExist(sProcessID, sToolingID))
                        {
                            ToolUtils.UpdateToolingQTY(sProcessName, sToolingNo, sQty);
                            sResult = "OK,UPDATE";
                        }
                        else
                        {
                            ToolUtils.InsertToolingSN(sProcessName, sToolingNo, "0", iQty);
                            sResult = "OK,INSERT";
                        }
                    }
                    catch (Exception EX)
                    {
                        sResult = EX.Message;
                    }
                }
                dr["RESULT"] = sResult;
            }
            uctlDataGridView1.dtSourceTable = dtImportData;
            uctlDataGridView1.ShowMode1();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            uctlProgressStatus1.SetFont();
           
            tabControl1.SelectedIndex = 1;
            uctlDataGridView1.Clear();
            lablFileName.Text = "N/A";
            string sFilePath;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();
            sFilePath = openFile.FileName;
            if (sFilePath.Trim() == "")
                return;
            
            string sFileExten = Path.GetExtension(sFilePath);
            //擷取所選取的路徑的檔案的附檔名確認是否為Excel檔
            if (!(sFileExten.Equals(".xls") || sFileExten.Equals(".xlsx")))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select Excel File"), 0);
                return;
            }

            btnImport.Enabled = false;
            btnCopy.Enabled = false;
            btnMaintain.Enabled = false;
            lablFileName.Text = sFilePath;
            //讀取出Excel檔案內容
            uctlProgressStatus1.Visible = true;
            _myImportFile.ToolUtils = ToolUtils;
            _myImportFile.sFileName = sFilePath;
            _myImportFile.Start();
        }
        void _myImportFile_OnReportStatus(int iCount)
        {
            uctlProgressStatus1.AddCount(iCount);
        }

        void _myImportFile_OnCompleted()
        {
            if (!string.IsNullOrEmpty(_myImportFile.sResult))
                SajetCommon.Show_Message(_myImportFile.sResult, 0);
            uctlProgressStatus1.Clear();
            uctlProgressStatus1.Visible = false;
            uctlDataGridView1.dtSourceTable = _myImportFile.dtImport;
            uctlDataGridView1.ShowMode1();
            btnCopy.Enabled = true;
            btnMaintain.Enabled = true;
            btnImport.Enabled = true;
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            

            fDetailData f = new fDetailData();
            try
            {
                f.OnAppend += new fDetailData.m_OnAppend(f_OnAppend);
                f.g_sUpdateType = "APPEND";
                f.g_sformText = btnAppend.Text;  //當按下新增時，將btnAppend的值顯示於主畫面標頭上
                f.g_sProcessID = gvData.CurrentRow.Cells["PROCESS_ID"].Value.ToString();
                f.g_sProcessName = gvData.CurrentRow.Cells["PROCESS_NAME"].Value.ToString();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    uctlStepItem1.ShowData();
                    //SetSelectRow(gvData, "", "STEP_ITEM_ID");
                }
            }
            finally
            {
                f.Dispose();
            }
        }

        void f_OnAppend(string f_StepItemCode)
        {
            
        }

        private void uctlStepItem1_Load(object sender, EventArgs e)
        {

        }
    }
}