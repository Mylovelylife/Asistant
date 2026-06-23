using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetTable;
using System.IO;
using System.Data.OracleClient;
using System.Reflection;
using System.Collections.Specialized;

namespace CWoManagerPcs
{
    public partial class fMain : Form
    {
        private MESGridView.Cache memoryCache = null;
        CheckBox chkWOBom = new CheckBox();
        DateTimePicker dtpCreateDate1 = new DateTimePicker();
        DateTimePicker dtpCreateDate2 = new DateTimePicker();

        struct TControlData
        {
            public string sFieldName;
            public TextBox txtControl;
            public ComboBox combControl;
        }
        TControlData[] m_tControlData;
        string sIniFile = Application.StartupPath + Path.DirectorySeparatorChar + "Sajet.Ini";
        List<string> combFilterField = new List<string>();

        public struct TDBInitial
        {
            public string sValue;
            public string sDefault;
            public string sType;
            public List<string> slValue;
        }
        Dictionary<string, TDBInitial> g_DBInitial = new Dictionary<string, TDBInitial>();
        Dictionary<string, string[]> g_MainField = new Dictionary<string, string[]>();

        public fMain()
        {
            InitializeComponent();
        }
        public static String g_sProgram, g_sFunction, g_sExeName;
        public String g_sOrderField;
        object[][] g_Params;
        public static String g_sFactoryID;
        public static int g_iPrivilege, g_iReleasePri, g_iEditWOBomPri, g_iBindingBtnPri;

        string g_sDataSQL;
        const string g_sProgramType = "PCS_";

        private void Initial_Form()
        {
            SajetCommon.SetLanguageControl(this);
        }
        private void ts_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null) return;
            ToolStripButton ts = (ToolStripButton)sender;
            Assembly assembly = null;
            object obj = null;
            Type type = null;
            try
            {
                assembly = Assembly.LoadFrom(Application.StartupPath + Path.DirectorySeparatorChar + g_sExeName + Path.DirectorySeparatorChar + ts.Tag.ToString());
                string[] Name = assembly.FullName.ToString().Split(',');
                type = assembly.GetType(Name[0] + ".fMain");
                DataSet dsMaster = new DataSet();
                dsMaster.Tables.Add();
                dsMaster.Tables[0].Rows.Add();
                for (int i = 0; i < gvData.Columns.Count; i++)
                {
                    dsMaster.Tables[0].Columns.Add(gvData.Columns[i].Name);
                    dsMaster.Tables[0].Rows[0][i] = gvData.CurrentRow.Cells[i].Value;
                }
                object[] arg = new object[] { dsMaster, g_sExeName };
                obj = assembly.CreateInstance(type.FullName, true, BindingFlags.CreateInstance, null, arg, null, null);
                Form formChild = (Form)obj;
                DialogResult dr = formChild.ShowDialog();
                if (ts.ToolTipText == "Y" && (dr == DialogResult.OK || dr == DialogResult.Yes))
                {
                    string sSelectKeyValue = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                    ShowData();
                    SetSelectRow(g_sDataSQL, g_Params, gvData, sSelectKeyValue, TableDefine.gsDef_KeyField);
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 1);
            }
        }
        private void fMain_Load(object sender, EventArgs e)
        {
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
            g_sOrderField = TableDefine.gsDef_OrderField;
            g_sExeName = ClientUtils.fCurrentProject;
            string sSQL = "SELECT * FROM SAJET.SYS_PROGRAM_FUN_MAINTAIN WHERE PROGRAM = :PROGRAM AND FUN_NAME = :FUN_NAME";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROGRAM", g_sProgram };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FUN_NAME", "W/O Main Field" };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            foreach (DataRow dr in dsTemp.Tables[0].Rows)
                g_MainField.Add(dr["FIELD_NAME"].ToString(), new string[] { dr["FIELD_VALUE"].ToString(), dr["PARAM_FIELD"].ToString(), dr["SELECT_LAST_INDEX"].ToString() });
            sSQL = $@"SELECT PARAM_NAME, PARAM_VALUE, DEFAULT_VALUE FROM SAJET.SYS_BASE_PARAM WHERE PROGRAM = :PROGRAM AND UPPER(PARAM_NAME) LIKE '{g_sProgramType}%' AND PARAM_TYPE = 'Button' ORDER BY PARAM_NAME";
            Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROGRAM", g_sProgram };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            ToolStripButton ts;
            foreach (DataRow dr in dsTemp.Tables[0].Rows)
            {
                ts = new ToolStripButton(dr[0].ToString(), btnSN.Image);
                ts.TextImageRelation = TextImageRelation.ImageAboveText;
                ts.Tag = dr[1].ToString();
                ts.ToolTipText = dr[2].ToString();
                ts.Click += new EventHandler(ts_Click);
                bindingNavigator1.Items.Add(ts);
            }
            sSQL = $@"SELECT * FROM SAJET.SYS_BASE_PARAM WHERE PROGRAM = :PROGRAM AND UPPER(PARAM_NAME) LIKE '{g_sProgramType}%' AND NVL(PARAM_TYPE, 'N/A') <> 'Button'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            combFilter.Items.Clear();
            combFilterField.Clear();
            string sField = string.Empty, sLabel = string.Empty;
            foreach (DataRow dr in dsTemp.Tables[0].Rows)
            {
                TDBInitial dbInitial = new TDBInitial();
                dbInitial.sValue = dr["PARAM_VALUE"].ToString();
                dbInitial.sDefault = dr["DEFAULT_VALUE"].ToString();
                dbInitial.sType = dr["PARAM_TYPE"].ToString();
                dbInitial.slValue = new List<string>();
                dbInitial.slValue.AddRange(dr["PARAM_VALUE"].ToString().Split(','));
                g_DBInitial.Add(dr["PARAM_NAME"].ToString(), dbInitial);
            }
            if (!g_DBInitial.ContainsKey(g_sProgramType + "Grid"))
            {
                TDBInitial dbInitial = new TDBInitial();
                dbInitial.sValue = "WORK_ORDER,PART_NO";
                dbInitial.slValue = new List<string>();
                dbInitial.slValue.AddRange(new string[] { "WORK_ORDER", "PART_NO" });
                g_DBInitial.Add(g_sProgramType + "Grid", dbInitial);
            }
            if (g_DBInitial.ContainsKey(g_sProgramType + "Initial_Table FIELD") && g_DBInitial.ContainsKey(g_sProgramType + "Initial_Table LABEL"))
            {
                sField = g_DBInitial[g_sProgramType + "Initial_Table FIELD"].sValue;
                sLabel = g_DBInitial[g_sProgramType + "Initial_Table LABEL"].sValue;
            }
            TableDefine.Initial_Table(sField, sLabel);
            m_tControlData = new TControlData[TableDefine.tGridField.Length];
            if (!g_DBInitial.ContainsKey(g_sProgramType + "Packing Spec WO"))
            {
                TDBInitial dbInitial = new TDBInitial();
                dbInitial.sValue = @"SELECT B.PKSPEC_NAME, BOX_CAPACITY BOX_QTY, CARTON_CAPACITY CARTON_QTY, PALLET_CAPACITY PALLET_QTY, A.PKSPEC_ID
                    FROM SAJET.G_PACK_SPEC A, SAJET.SYS_PKSPEC B 
                    WHERE WORK_ORDER = :WORK_ORDER AND A.PKSPEC_ID = B.PKSPEC_ID
                    ORDER BY BOX_CAPACITY DESC, CARTON_CAPACITY DESC, PALLET_CAPACITY DESC";
                g_DBInitial.Add(g_sProgramType + "Packing Spec WO", dbInitial);
            }
            lstPack.Columns.Clear();
            string[] slValue, slDefault;
            if (g_DBInitial.ContainsKey(g_sProgramType + "Packing Spec Title"))
            {
                slValue = g_DBInitial[g_sProgramType + "Packing Spec Title"].sValue.ToString().Split(',');
                slDefault = g_DBInitial[g_sProgramType + "Packing Spec Title"].sDefault.ToString().Split(',');
            }
            else
            {
                slValue = new string[] { "PKSPEC_NAME", "BOX_QTY", "CARTON_QTY", "PALLET_QTY" };
                slDefault = new string[] { "200", "90", "90", "90" };
            }
            for (int i = 0; i < slValue.Length; i++)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Name = slValue[i];
                ch.Text = slValue[i];
                ch.Width = int.Parse(slDefault[i]);
                lstPack.Columns.Add(ch);
            }
            if (g_DBInitial.ContainsKey(g_sProgramType + "Filter"))
            {
                slValue = g_DBInitial[g_sProgramType + "Filter"].sValue.ToString().Split(',');
                foreach (string sValue in slValue)
                {
                    combFilter.Items.Add(sValue);
                    combFilterField.Add(sValue);
                }
            }
            else
            {
                combFilter.Items.Add("Work Order"); //Part
                combFilterField.Add("WORK_ORDER");
                combFilter.Items.Add("Part No"); //Part
                combFilterField.Add("PART_NO");
                combFilter.Items.Add("W/O Type"); //WO type
                combFilterField.Add("WO_TYPE");
                combFilter.Items.Add("Customer Code"); //Customer
                combFilterField.Add("CUSTOMER_CODE");
                combFilter.Items.Add("Default Line"); //Line
                combFilterField.Add("PDLINE_NAME");
                combFilter.Items.Add("Route Name"); //Route
                combFilterField.Add("ROUTE_NAME");
            }
            Label lablTemp;
            TextBox txtTemp;
            ComboBox ddlTemp;
            tableLayoutPanel1.RowCount = (int)Math.Ceiling((decimal)m_tControlData.Length / 2) + 1;
            for (int i = 2; i < TableDefine.tGridField.Length - 1; i = i + 2)
                tableLayoutPanel1.RowStyles.Insert(0, new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30));
            int iCol = 0, iRow = 0;
            for (int i = 0; i < TableDefine.tGridField.Length; i++)
            {
                if (!string.IsNullOrEmpty(TableDefine.tGridField[i].sFieldName))
                {
                    lablTemp = new Label();
                    lablTemp.Font = new Font("��?���w", 11);
                    lablTemp.Text = TableDefine.tGridField[i].sCaption;
                    lablTemp.TextAlign = ContentAlignment.MiddleLeft;
                    //lablTemp.Dock = DockStyle.Fill;
                    lablTemp.Anchor = AnchorStyles.Left| AnchorStyles.Right| AnchorStyles.Bottom;

                    tableLayoutPanel1.Controls.Add(lablTemp, iCol, iRow);
                    if (g_MainField.ContainsKey(TableDefine.tGridField[i].sFieldName))
                    {
                        ddlTemp = new ComboBox();
                        //ddlTemp.Dock = DockStyle.Fill;
                        ddlTemp.DropDownStyle = ComboBoxStyle.DropDownList;
                        ddlTemp.Font = new Font("��?���w", 11);
                        ddlTemp.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                        tableLayoutPanel1.Controls.Add(ddlTemp, iCol + 1, iRow);
                        m_tControlData[i].combControl = ddlTemp;
                    }
                    else
                    {
                        txtTemp = new TextBox();
                        txtTemp.ForeColor = Color.Maroon;
                        txtTemp.ReadOnly = true;
                        txtTemp.BackColor = SystemColors.Window;
                        //txtTemp.Dock = DockStyle.Fill;
                        txtTemp.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                        txtTemp.Name = "labl" + TableDefine.tGridField[i].sFieldName;
                        txtTemp.Font = new Font("��?���w", 11, FontStyle.Bold);
                        tableLayoutPanel1.Controls.Add(txtTemp, iCol + 1, iRow);
                        m_tControlData[i].txtControl = txtTemp;
                    }
                }
                m_tControlData[i].sFieldName = TableDefine.tGridField[i].sFieldName;
                iRow++;
                if (iRow == tableLayoutPanel1.RowCount - 1)
                {
                    iRow = 0;
                    iCol = 2;
                }
            }
            tableLayoutPanel1.Controls.Add(gbPack, 0, tableLayoutPanel1.RowCount - 1);
            tableLayoutPanel1.SetColumnSpan(gbPack, 4);
            gbPack.Dock = DockStyle.Fill;

            chkWOBom.Text = "W/O Create Date";
            chkWOBom.Checked = true;
            chkWOBom.CheckStateChanged += chkWOBom_ValueChanged;
            ToolStripControlHost host = new ToolStripControlHost(chkWOBom);
            toolStrip1.Items.Insert(7, host);

            Initial_Form();

            dtpCreateDate1.Width = 112;
            dtpCreateDate1.Height = 25;
            dtpCreateDate1.Format = DateTimePickerFormat.Custom;
            dtpCreateDate1.Name = "dtpCreateDate1";
            ToolStripControlHost host1 = new ToolStripControlHost(dtpCreateDate1);
            toolStrip1.Items.Insert(8, host1);
            dtpCreateDate1.CloseUp += new EventHandler(dtpCreateDate1_CloseUp);

            dtpCreateDate2.Width = 112;
            dtpCreateDate2.Height = 25;
            dtpCreateDate2.Format = DateTimePickerFormat.Custom;
            dtpCreateDate2.Name = "dtpCreateDate2";
            ToolStripControlHost host2 = new ToolStripControlHost(dtpCreateDate2);
            toolStrip1.Items.Insert(10, host2);
            dtpCreateDate2.CloseUp += new EventHandler(dtpCreateDate2_CloseUp);

            //DateTimepicker�w�]�ɶ������Ѥ��
            //�C�ѫe��00:00:00
            this.dtpCreateDate1.Value = DateTime.Today.AddDays(-7);
            //����23:59:59
            this.dtpCreateDate2.Value = DateTime.Today.AddDays(1).AddSeconds(-1);

            this.Text = this.Text + "(" + SajetCommon.g_sFileVersion + ")";

            //Select Emp ID
            sSQL = @" SELECT EMP_ID,NVL(FACTORY_ID,0) FACTORY_ID
                 FROM SAJET.SYS_EMP
                 WHERE EMP_ID = :EMP_ID";
            Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_ID", ClientUtils.UserPara1 };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            string sUserFacID = dsTemp.Tables[0].Rows[0]["FACTORY_ID"].ToString();

            //Select Factory
            combFactory.Items.Clear();
            sSQL = @" SELECT FACTORY_ID,FACTORY_CODE 
                 FROM SAJET.SYS_FACTORY 
                 WHERE ENABLED = 'Y' AND FACTORY_LABEL = 'PCS'
                 ORDER BY FACTORY_CODE ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                g_sFactoryID = dsTemp.Tables[0].Rows[0]["FACTORY_ID"].ToString();
                for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                {
                    combFactory.Items.Add(dsTemp.Tables[0].Rows[i]["FACTORY_CODE"].ToString());
                    if (sUserFacID == dsTemp.Tables[0].Rows[i]["FACTORY_ID"].ToString())
                    {
                        g_sFactoryID = dsTemp.Tables[0].Rows[i]["FACTORY_ID"].ToString();
                        combFactory.SelectedIndex = i;
                        combFactory.Enabled = false;
                    }
                }
            }
            if (sUserFacID == "0")
            {
                if (combFactory.Items.Count > 0)
                    combFactory.SelectedIndex = 0;
                combFactory.Enabled = true;

            }
            combFactory.SelectedIndexChanged += new EventHandler(combFactory_SelectedIndexChanged);
            //Ū��SYS_BASE�]�w��
            int iSelectInx = combWoStatus.Items.Count - 1;
            dsTemp = ClientUtils.GetSysBaseData(g_sProgram, "Default Search Wo Status"); //�w�]���WO Status
            string sDefaultWoStatus = iSelectInx.ToString();
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sDefaultWoStatus = dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString();
                if (sDefaultWoStatus == "-1")
                    combWoStatus.SelectedIndexChanged -= combWoStatus_SelectedIndexChanged;
                else if (!string.IsNullOrEmpty(sDefaultWoStatus))
                {
                    bool bResult = int.TryParse(sDefaultWoStatus, out iSelectInx);
                    if (!bResult)
                        iSelectInx = combWoStatus.Items.Count - 1;
                }
            }
            combWoStatus.SelectedIndex = iSelectInx;
            if (sDefaultWoStatus == "-1")
                combWoStatus.SelectedIndexChanged += new EventHandler(combWoStatus_SelectedIndexChanged);
            dsTemp.Dispose();
            Check_Privilege();

            btnAssignMAC.Visible = GetAssignMac();
            MenuMACRequest.Visible = btnAssignMAC.Visible;
            if (g_DBInitial.ContainsKey(g_sProgramType + "Unvisible Button"))
            {
                slValue = g_DBInitial[g_sProgramType + "Unvisible Button"].sValue.Split(',');
                int iUnvisible = 0;
                foreach (string sValue in slValue)
                {
                    switch (sValue)
                    {
                        case "SN":
                            btnSN.Visible = false;
                            MenuViewSN.Visible = false;
                            iUnvisible++;
                            break;
                        case "PANEL SN":
                            btnPanel.Visible = false;
                            viewToolStripMenuItem.Visible = false;
                            iUnvisible++;
                            break;
                        case "BOM":
                            btnBom.Visible = false;
                            MenuViewBOM.Visible = false;
                            iUnvisible++;
                            break;
                    }
                }
                if (iUnvisible == 3)
                {
                    tsView.Visible = false;
                    toolStripSeparator1.Visible = MenuMACRequest.Visible;
                }
            }
            SajetInifile sini = new SajetInifile();
            string sWidth = sini.ReadIniFile(sIniFile, g_sProgram, g_sFunction + " Splitter", "");
            sini.Dispose();
            if (!string.IsNullOrEmpty(sWidth))
                panel2.Width = int.Parse(sWidth);
            splitter1.SplitterMoved += new SplitterEventHandler(this.splitter1_SplitterMoved);

            gvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        private void Check_Privilege()
        {
            g_iPrivilege = ClientUtils.GetPrivilege(ClientUtils.UserPara1, g_sFunction, g_sProgram);
            btnAppend.Enabled = (g_iPrivilege >= 1);
            btnModify.Enabled = (g_iPrivilege >= 0);

            g_iReleasePri = ClientUtils.GetPrivilege(ClientUtils.UserPara1, "W/O Release", g_sProgram);
            g_iEditWOBomPri = ClientUtils.GetPrivilege(ClientUtils.UserPara1, "EDIT WO BOM", g_sProgram);
            g_iBindingBtnPri = ClientUtils.GetPrivilege(ClientUtils.UserPara1, "Binding Process", g_sProgram);

        }

        private bool GetAssignMac()
        {
            DataSet dsTemp = new DataSet();
            try
            {
                string sSQL = @"SELECT * FROM SAJET.SYS_LABEL 
                    WHERE LABEL_NAME = 'MAC' 
                    AND TYPE = 'S' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    return false;
                }

                sSQL = @"Select SQL_NAME from sajet.sys_sql 
                    where SYSUSE_NAME='MAC REQUEST' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    return false;
                }

                string sFile = Application.StartupPath + "\\" + fMain.g_sExeName + "\\AssignMAC.dll";
                if (!File.Exists(sFile))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                dsTemp.Dispose();
            }
        }

        public void ShowData()
        {
            DataSet dsTemp = new DataSet();
            try
            {
                string sSQL;
                object[][] Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FACTORY_ID", g_sFactoryID };
                string sCondition = string.Empty;
                //if (combWoStatus.SelectedIndex != combWoStatus.Items.Count - 1)
                //{
                //        sCondition = " AND A.WO_STATUS = :WO_STATUS ";
                //        Array.Resize(ref Params, Params.Length + 1);
                //        Params[Params.Length - 1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_STATUS", combWoStatus.SelectedIndex.ToString() };

                //}

                string sCreateDate1 = dtpCreateDate1.Value.ToString("yyyy/MM/dd HH:mm:ss");
                string sCreateDate2 = dtpCreateDate2.Value.ToString("yyyy/MM/dd HH:mm:ss");

                if (chkWOBom.Checked)
                    sCondition += "AND (a.WO_CREATE_DATE >= to_date('" + sCreateDate1 + @"','YYYY/MM/DD HH24:MI:SS') AND a.WO_CREATE_DATE <= to_date(' " + sCreateDate2 + "','YYYY/MM/DD HH24:MI:SS'))";

                //2015/10/5 for�]�T �W�[�@�Ӥu�檬�A�z�����u���Ĥu��v�A�O�]�t�h�Ӥu�檬�A�A�Y�Ҧ����A�����Ȱ������
                if (combWoStatus.SelectedIndex < 7)
                {
                    sCondition += " AND A.WO_STATUS = :WO_STATUS ";
                    Array.Resize(ref Params, Params.Length + 1);
                    Params[Params.Length - 1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_STATUS", combWoStatus.SelectedIndex.ToString() };
                }
                else if (combWoStatus.SelectedIndex > 7)
                {
                    sCondition += " AND A.WO_STATUS IN (0,1,2,3,6) ";
                }

                if (!string.IsNullOrEmpty(editFilter.Text.Trim()))
                {
                    string sFieldName = combFilterField[combFilter.SelectedIndex].ToString();
                    /*if (combFilterField[combFilter.SelectedIndex].ToString().IndexOf("WORK_ORDER") > -1)
                        sCondition += " AND " + sFieldName + " = :FILTER";
                    else*/
                    sCondition += " AND " + sFieldName + " LIKE :FILTER || '%'";
                    Array.Resize(ref Params, Params.Length + 1);
                    Params[Params.Length - 1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FILTER", editFilter.Text.Trim() };
                }
                if (g_DBInitial.ContainsKey(g_sProgramType + "SQL"))
                    sSQL = g_DBInitial[g_sProgramType + "SQL"].sValue.Replace("[CONDITION]", sCondition);
                else
                {
                    sSQL = @"SELECT a.* 
                    ,SAJET.SJ_WOStatus_Result(A.WO_STATUS) WOSTATUS  
                    ,b.part_no,c.route_name,d.pdline_name 
                    ,e.process_name START_PROCESS,f.process_name END_PROCESS ,g.customer_code || '/' || g.customer_name customer_code
                    ,B.MODEL_ID AS MODEL_ID_NEW,
                    (SELECT MODEL_NAME FROM SAJET.SYS_MODEL WHERE MODEL_ID = B.MODEL_ID AND ROWNUM = 1 ) MODEL_NAME_NEW
                    from sajet.g_wo_base a Left JOIN sajet.sys_part b ON a.part_id = b.part_id
                    LEFT JOIN sajet.sys_route c ON a.ROUTE_ID = c.route_id 
                    LEFT JOIN sajet.sys_pdline d ON a.default_pdline_id = d.pdline_id
                    LEFT JOIN sajet.sys_process e ON a.start_process_id = e.process_id
                    LEFT JOIN sajet.sys_process f ON a.end_process_id = f.process_id
                    LEFT JOIN sajet.sys_customer g ON a.customer_id = g.customer_id
                    where a.Factory_id = :FACTORY_ID";
                    sSQL += sCondition + " order by " + g_sOrderField;
                }
                g_sDataSQL = sSQL;
                g_Params = Params;
                gvData.SelectionChanged -= gvData_SelectionChanged;
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                gvData.DataSource = dsTemp.Tables[0];
                //(new MESGridView.DisplayGridView()).GetGridView(gvData, sSQL, Params, out memoryCache);
                gvData.SelectionChanged += new EventHandler(gvData_SelectionChanged);
                //���Title 
                foreach (DataGridViewColumn dc in gvData.Columns)
                {
                    if (g_DBInitial[g_sProgramType + "Grid"].slValue.IndexOf(dc.Name) > -1)
                    {
                        dc.HeaderText = SajetCommon.SetLanguage(dc.HeaderText, 1);
                        dc.DisplayIndex = g_DBInitial[g_sProgramType + "Grid"].slValue.IndexOf(dc.Name);
                    }
                    else
                        dc.Visible = false;
                }

                //�̷�SYS_BASE_PARAM.Gird�Ƨ����
                foreach (string sColName in g_DBInitial[g_sProgramType + "Grid"].slValue)
                {
                    gvData.Columns[sColName].DisplayIndex = g_DBInitial[g_sProgramType + "Grid"].slValue.IndexOf(sColName);
                }

                gvData.Columns["WORK_ORDER"].Frozen = true;
                gvData.Focus();
                if (gvData.Rows.Count == 0)
                    ClearData();
                else
                    ShowDetail();
                tsCount.Text = gvData.Rows.Count.ToString();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dsTemp != null)
                    dsTemp.Dispose();
            }
        }

        private void editFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            ShowData();
            SetSelectRow(g_sDataSQL, g_Params, gvData, "", TableDefine.gsDef_KeyField);
            editFilter.Focus();
            editFilter.SelectAll();
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            fData f = new fData(g_DBInitial);
            try
            {
                f.g_sUpdateType = "APPEND";
                f.g_sformText = btnAppend.Text;
                f.g_sFactory = combFactory.Text;
                f.btnBOM.Visible = btnBom.Visible;
                f.g_iBindingBtnPri = g_iBindingBtnPri;
                if (!btnBom.Visible)
                    f.tableLayoutPanel1.SetColumnSpan(f.combVersion, 2);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    SetSelectRow(g_sDataSQL, g_Params, gvData, "", TableDefine.gsDef_KeyField);
                }
            }
            finally
            {
                f.Dispose();
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            fData f = new fData(g_DBInitial);
            try
            {
                f.g_sUpdateType = "MODIFY";
                f.g_sformText = btnModify.Text;
                f.dataCurrentRow = gvData.CurrentRow;
                f.dataGridColumn = gvData.Columns;
                f.g_sFactory = combFactory.Text;
                f.btnBOM.Visible = btnBom.Visible;
                f.g_iBindingBtnPri = g_iBindingBtnPri;
                if (!btnBom.Visible)
                    f.tableLayoutPanel1.SetColumnSpan(f.combVersion, 2);

                string sSelectKeyValue = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    SetSelectRow(g_sDataSQL, g_Params, gvData, sSelectKeyValue, TableDefine.gsDef_KeyField);
                }
            }
            finally
            {
                f.Dispose();
            }
        }

        public static void CopyToHistory(string sID)
        {
            string sSQL = string.Format(@" Insert into {0}
                Select * from {1}
                where {2} = :sID", TableDefine.gsDef_HTTable, TableDefine.gsDef_Table, TableDefine.gsDef_KeyField);
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "sID", sID };
            ClientUtils.ExecuteSQL(sSQL, Params);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.Filter = "All Files(*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            string sFileName = saveFileDialog1.FileName;

            ExportExcel.CreateExcel Export = new ExportExcel.CreateExcel(sFileName);
            //Export.ExportToExcel(gvData);

            //NPOI
            Export.RenderDataTableToExcel(gvData, sFileName);
        }

        private void SetSelectRow(string sSQL, object[][] Params, DataGridView GridData, String sPrimaryKey, String sField)
        {
            if (GridData.Rows.Count > 0)
            {
                int iIndex = 0;
                string sShowField = GridData.Columns[0].Name;
                for (int i = 0; i <= GridData.Columns.Count - 1; i++)
                {
                    if (GridData.Columns[i].Visible)
                    {
                        //�Ĥ@�Ӧ���ܪ����(focus���������|���~)
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
                    //���SQL��,����GridŪ��,�_�h�t�׷|�C
                    string sText = "select idx from ("
                                 + " Select aa.*,rownum-1 idx from ("
                                 + sSQL
                                 + " ) aa ) "
                                 + sCondition;
                    DataSet ds = ClientUtils.ExecuteSQL(sText, Params);
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

        private void MenuHistory_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null) return;
            string sWO = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
            string sSQL = string.Empty;
            if (g_DBInitial.ContainsKey(g_sProgramType + "History SQL"))
                sSQL = g_DBInitial[g_sProgramType + "History SQL"].sValue;
            else
                sSQL = TableDefine.History_SQL();
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
            fLog fH = new fLog(sSQL, Params);
            fH.txtWo.Text = sWO;
            fH.ShowDialog();
            fH.Dispose();
        }

        private void gvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                g_sOrderField = gvData.Columns[e.ColumnIndex].Name;
                if (g_sOrderField == "VERSION")
                    g_sOrderField = "a." + g_sOrderField;
                ShowData();
                SetSelectRow(g_sDataSQL, g_Params, gvData, "", TableDefine.gsDef_KeyField);
            }
        }

        private void combFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sSQL = @" SELECT FACTORY_ID,FACTORY_CODE,FACTORY_NAME,FACTORY_DESC 
                 FROM SAJET.SYS_FACTORY 
                 WHERE FACTORY_CODE = :FACTORY_CODE ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FACTORY_CODE", combFactory.Text };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            g_sFactoryID = dsTemp.Tables[0].Rows[0]["FACTORY_ID"].ToString();
            dsTemp.Dispose();
            ShowData();
        }

        private void combWoStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowData();
        }

        private void MenuViewSN_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            string sSQL = string.Empty;
            if (g_DBInitial.ContainsKey(g_sProgramType + "SN SQL"))
                sSQL = g_DBInitial[g_sProgramType + "SN SQL"].sValue;
            else
                sSQL = @" Select A.SERIAL_NUMBER,A.Customer_SN,A.PANEL_NO,B.Part_No,A.VERSION 
                ,C.PDLine_Name,D.Stage_Name,E.Process_Name,F.Terminal_Name 
                ,SAJET.SJ_SNSTATUS_RESULT_PCS(CURRENT_STATUS) CURRENT_STATUS 
                ,I.EMP_NAME,nvl(G.Process_Name,M.Process_Name) Next_Process 
                ,decode(A.Work_Flag,1,'Scrap','') Scrap 
                ,to_char(A.In_Process_Time,'yyyy/mm/dd hh24:mi:ss') In_Process_Time,to_char(A.Out_Process_Time,'yyyy/mm/dd hh24:mi:ss') Out_Process_Time 
                ,to_char(A.In_PDLine_Time,'yyyy/mm/dd hh24:mi:ss') In_PDLine_Time,to_char(A.Out_PDLine_Time,'yyyy/mm/dd hh24:mi:ss') Out_PDLine_Time 
                ,A.Box_No,A.CARTON_NO,A.PALLET_NO,A.QC_NO,A.QC_RESULT,H.CUSTOMER_NAME 
                ,J.DN_No,A.Rework_No,K.update_Time Rework_Time,L.emp_name Rework_Employee 
                From SAJET.G_SN_STATUS A 
                LEFT JOIN SAJET.SYS_PDLine C ON A.PDLine_ID = C.PDLine_ID
                LEFT JOIN SAJET.SYS_Stage D ON A.Stage_ID = D.Stage_ID
                LEFT JOIN SAJET.SYS_Process E ON A.Process_ID = E.Process_ID
                LEFT JOIN SAJET.SYS_Terminal F ON A.Terminal_ID = F.Terminal_ID
                LEFT JOIN SAJET.SYS_Process G ON A.Next_Process = G.Process_ID
                LEFT JOIN SAJET.SYS_Customer H ON A.Customer_id = H.Customer_ID
                LEFT JOIN SAJET.SYS_Emp I ON A.Emp_id = I.Emp_ID
                LEFT JOIN SAJET.G_DN_BASE J ON A.Shipping_id = J.DN_ID
                LEFT JOIN SAJET.G_Rework_No K ON A.Rework_No = K.Rework_No
                LEFT JOIN SAJET.SYS_Process M ON A.Wip_Process = M.Process_ID
                LEFT JOIN SAJET.SYS_Emp L ON K.emp_id = L.emp_id
                ,SAJET.SYS_Part B 
                Where A.WORK_ORDER = :WORK_ORDER
                and A.Part_ID = B.Part_ID 
                Order by A.SERIAL_NUMBER ";
            string sFieldID = gvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sFieldID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            fHistory fH = new fHistory();
            try
            {
                fH.Text = "View SN";
                fH.dgvHistory.DataSource = dsTemp;
                fH.dgvHistory.DataMember = dsTemp.Tables[0].ToString();
                fH.LabCount.Text = dsTemp.Tables[0].Rows.Count.ToString();
                fH.LabWO.Text = sFieldID;
                fH.ShowDialog();
            }
            finally
            {
                fH.Dispose();
            }
            dsTemp.Dispose();
        }

        private void MenuViewBOM_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sPartID = gvData.CurrentRow.Cells["PART_ID"].Value.ToString();
            string sVer = gvData.CurrentRow.Cells["VERSION"].Value.ToString();
            string sRouteID = gvData.CurrentRow.Cells["ROUTE_ID"].Value.ToString();

            fWoBom fB = new fWoBom();
            try
            {
                fB.g_sRouteID = sRouteID;
                fB.LabWO.Text = gvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
                fB.LabPartNo.Text = gvData.CurrentRow.Cells["PART_NO"].Value.ToString();
                fB.LabVer.Text = sVer;
                fB.g_sPartID = sPartID;
                if (g_iEditWOBomPri > 1)
                {
                    fB.PopMenu2.Opening += fB.PopMenu2_Opening;
                    fB.LabType.Visible = false;
                }
                else
                {
                    //�u�i�HŪ,���i�ק�
                    fB.LabType.Text = "Read Only";
                    fB.TreeBomData.AllowDrop = false;
                    fB.LVPart.AllowDrop = false;
                    fB.MenuItemDelete.Visible = false;
                    fB.MenuItemModify.Visible = false;
                    fB.PopMenu2.Opening -= fB.PopMenu2_Opening;
                }



                fB.ShowBom(sPartID, sVer);
                fB.ShowDialog();
            }
            finally
            {
                fB.Dispose();
            }
        }

        private void MenuStatus_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sFieldID = gvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
            string sSQL = @" Select A.WORK_ORDER ,A.UPDATE_TIME 
                ,SAJET.SJ_WOStatus_Result(A.WO_STATUS) WOSTATUS 
                ,A.MEMO REMARK,B.EMP_NAME UPDATE_USER  
                From SAJET.G_WO_STATUS A 
                LEFT JOIN SAJET.SYS_EMP B ON A.update_userid = b.emp_id
                Where A.WORK_ORDER = :WORK_ORDER 
                Order by UPDATE_TIME ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sFieldID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            fHistory fH = new fHistory();
            try
            {
                fH.Text = "Status Log";
                fH.dgvHistory.DataSource = dsTemp;
                fH.dgvHistory.DataMember = dsTemp.Tables[0].ToString();
                fH.LabCount.Text = dsTemp.Tables[0].Rows.Count.ToString();
                fH.LabWO.Text = sFieldID;

                //�������W��
                for (int i = 0; i <= fH.dgvHistory.Columns.Count - 1; i++)
                {
                    string sGridField = fH.dgvHistory.Columns[i].HeaderText;
                    string sField = "";
                    for (int j = 0; j <= gvData.Columns.Count - 1; j++)
                    {
                        sField = gvData.Columns[j].Name;
                        if (sGridField == sField)
                        {
                            fH.dgvHistory.Columns[i].HeaderText = gvData.Columns[j].HeaderText;
                            break;
                        }
                    }
                }

                fH.ShowDialog();
            }
            finally
            {
                fH.Dispose();
            }
            dsTemp.Dispose();
        }

        private void MenuViewWoData_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            fData f = new fData(g_DBInitial);
            try
            {
                f.g_sUpdateType = "VIEW";
                f.g_sformText = MenuViewWoData.Text;
                f.dataCurrentRow = gvData.CurrentRow;
                f.dataGridColumn = gvData.Columns;
                f.g_sFactory = combFactory.Text;
                f.g_iBindingBtnPri = g_iBindingBtnPri;
                f.ShowDialog();
            }
            finally
            {
                f.Dispose();
            }
        }

        private void gvData_SelectionChanged(object sender, EventArgs e)
        {
            ShowDetail();

            button1.Enabled = true;
        }
        private void ShowDetail()
        {
            btnRelease.Visible = false;
            tsReleaseSep.Visible = btnRelease.Visible;
            lstPack.Items.Clear();
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null) return;

            string sWO = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
            string sStatus = gvData.CurrentRow.Cells["WO_STATUS"].Value.ToString();
            if (g_iReleasePri >= 1)
            {
                //if ((combWoStatus.SelectedIndex == combWoStatus.Items.Count - 1 && sStatus == "1") || (combWoStatus.SelectedIndex == 1))
                //�קאּcombWoStatus.Items.Count - 2  �O�W�[�u��z�ﶵ�ةҽվ㪺  (�����P���Ĥu��ҭn�i�i�}�Ǹ�)
                if ((combWoStatus.SelectedIndex >= combWoStatus.Items.Count - 2 && sStatus == "1") || (combWoStatus.SelectedIndex == 1))
                    btnRelease.Visible = true;
            }
            tsReleaseSep.Visible = btnRelease.Visible;
            object[][] Params;
            string sSQL = string.Empty;
            DataSet dsTemp;
            for (int i = 0; i < m_tControlData.Length; i++)
            {
                string sFieldName = m_tControlData[i].sFieldName;
                if (g_MainField.ContainsKey(sFieldName))
                {
                    sSQL = g_MainField[sFieldName][0];
                    Params = new object[1][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, g_MainField[sFieldName][1], gvData.CurrentRow.Cells[g_MainField[sFieldName][1]].Value.ToString() };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                    m_tControlData[i].combControl.Items.Clear();
                    foreach (DataRow dr in dsTemp.Tables[0].Rows)
                        m_tControlData[i].combControl.Items.Add(dr[0].ToString());
                    if (m_tControlData[i].combControl.Items.Count > 0)
                    {
                        int iIndex = 0;
                        if (gvData.Columns.Contains(sFieldName))
                        {
                            iIndex = m_tControlData[i].combControl.Items.IndexOf(gvData.CurrentRow.Cells[sFieldName].Value.ToString());
                            if (iIndex == -1)
                                if (g_MainField[sFieldName][2] == "1")
                                    iIndex = m_tControlData[i].combControl.Items.Count - 1;
                                else
                                    iIndex = 0;
                        }
                        else
                        {
                            if (g_MainField[sFieldName][2] == "1")
                                iIndex = m_tControlData[i].combControl.Items.Count - 1;
                        }
                        m_tControlData[i].combControl.SelectedIndex = iIndex;
                    }
                }
                else if (gvData.Columns.Contains(sFieldName))
                {
                    switch (sFieldName)
                    {
                        case "WO_SCHEDULE_DATE":
                        case "WO_DUE_DATE":
                            if (!string.IsNullOrEmpty(gvData.CurrentRow.Cells[sFieldName].Value.ToString()))
                                m_tControlData[i].txtControl.Text = DateTime.Parse(gvData.CurrentRow.Cells[sFieldName].Value.ToString()).ToString("yyyy/MM/dd");
                            else
                                m_tControlData[i].txtControl.Text = string.Empty;
                            break;
                        default:
                            m_tControlData[i].txtControl.Text = gvData.CurrentRow.Cells[sFieldName].Value.ToString();
                            break;
                    }
                }
            }
            Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
            sSQL = g_DBInitial[g_sProgramType + "Packing Spec WO"].sValue;
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            ListViewItem item;
            foreach (DataRow dr in dsTemp.Tables[0].Rows)
            {
                item = new ListViewItem();
                item.Text = dr["PKSPEC_NAME"].ToString();
                lstPack.Items.Add(item);
                for (int i = 1; i < lstPack.Columns.Count; i++)
                    lstPack.Items[lstPack.Items.Count - 1].SubItems.Add(dr[lstPack.Columns[i].Name].ToString());
            }
            dsTemp.Dispose();
        }
        private void ClearData()
        {
            for (int i = 0; i < m_tControlData.Length; i++)
                if (!string.IsNullOrEmpty(m_tControlData[i].sFieldName))
                {
                    if (m_tControlData[i].txtControl != null)
                        m_tControlData[i].txtControl.Text = string.Empty;
                    else if (m_tControlData[i].combControl != null)
                    {
                        m_tControlData[i].combControl.Items.Clear();
                        m_tControlData[i].combControl.Text = string.Empty;
                    }
                }
            lstPack.Items.Clear();
        }
        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sWO = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
            string sSQL = $@"SELECT TARGET_QTY, ROUTE_ID, PART_ID FROM SAJET.G_WO_BASE WHERE WORK_ORDER = '{sWO}'";
            var dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];
            if (dtTemp.Rows.Count>0)
            {
                if (string.IsNullOrEmpty(dtTemp.Rows[0]["TARGET_QTY"].ToString()))
                {
                    SajetCommon.Show_Message("Target QTY is null", 0);
                    return;
                }
                else if (string.IsNullOrEmpty(dtTemp.Rows[0]["ROUTE_ID"].ToString()))
                {
                    SajetCommon.Show_Message("Route is null", 0);
                    return;
                }
                else if (string.IsNullOrEmpty(dtTemp.Rows[0]["PART_ID"].ToString()))
                {
                    SajetCommon.Show_Message("Part is null", 0);
                    return;
                }
            }
            string sMsg = SajetCommon.SetLanguage("Change Work Order Status to Release", 1);
            string sData = gvData.Columns[TableDefine.gsDef_KeyData].HeaderText + " : " + sWO;
            if (SajetCommon.Show_Message(sMsg + " ?" + Environment.NewLine + sData, 2) != DialogResult.Yes)
                return;

            //��窱�A
            sSQL = @"UPDATE SAJET.G_WO_BASE
   SET WO_STATUS     = '2',
       UPDATE_USERID = :UPDATE_USERID,
       UPDATE_TIME   = SYSDATE
 WHERE WORK_ORDER = :WORK_ORDER";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
            ClientUtils.ExecuteSQL(sSQL, Params);

            // �������A�ܧ�                     
            sSQL = @"Insert into SAJET.G_WO_STATUS 
                 (Work_Order,WO_Status,Memo,update_userid) 
                 values 
                 (:WORK_ORDER,'2','',:UPDATE_USERID)";
            ClientUtils.ExecuteSQL(sSQL, Params);

            CopyToHistory(sWO);
            ShowData();
        }

        private void fMain_Shown(object sender, EventArgs e)
        {
            combFilter.SelectedIndex = 0;
            editFilter.Focus();
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sFieldID = gvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
            string sSQL;
            if (g_DBInitial.ContainsKey(g_sProgramType + "PANEL SN SQL"))
                sSQL = g_DBInitial[g_sProgramType + "PANEL SN SQL"].sValue;
            else
                sSQL = @" Select A.WORK_ORDER,A.SERIAL_NUMBER,B.EMP_NAME
                From SAJET.G_WO_SN A LEFT JOIN SAJET.SYS_EMP B ON A.emp_id = B.emp_id
                Where A.WORK_ORDER = :WORK_ORDER 
                Order by A.SERIAL_NUMBER ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sFieldID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            fHistory fH = new fHistory();
            try
            {
                fH.Text = "View Panel SN";
                fH.dgvHistory.DataSource = dsTemp;
                fH.dgvHistory.DataMember = dsTemp.Tables[0].ToString();
                fH.LabCount.Text = dsTemp.Tables[0].Rows.Count.ToString();
                fH.LabWO.Text = sFieldID;
                fH.ShowDialog();
            }
            finally
            {
                fH.Dispose();
            }
            dsTemp.Dispose();
        }

        private void btnAssignMAC_Click(object sender, EventArgs e)
        {
            string sFieldName = "";
            string sSQL = @"Select SQL_NAME from sajet.sys_sql
                where SYSUSE_NAME='MAC REQUEST' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sFieldName = dsTemp.Tables[0].Rows[0]["SQL_NAME"].ToString();
            }
            dsTemp.Dispose();
            fAssignMAC f = new fAssignMAC();
            f.g_sFieldName = sFieldName;
            f.ShowDialog();
            f.Dispose();
        }
        private string GetWOMACRequest(string sWorkOrder)
        {
            string sSQL = @"SELECT WORK_ORDER,NVL(WO_OPTION7,'N') FLAG FROM SAJET.G_WO_BASE
                WHERE WORK_ORDER =:WORK_ORDER ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count == 0)
                return "N/A";
            else
                return dsTemp.Tables[0].Rows[0]["FLAG"].ToString();
        }
        private void MenuMACRequestYes_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem))
                return;
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sTag = (sender as ToolStripMenuItem).Tag.ToString();
            string sValue = (sender as ToolStripMenuItem).Text.ToString();
            string sWorkOrder = gvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
            string sFlag = GetWOMACRequest(sWorkOrder);
            if (sFlag == "N/A")
                return;
            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Work Order") + " : " + sWorkOrder + Environment.NewLine
                                       + SajetCommon.SetLanguage("MAC Request Change to") + " : " + SajetCommon.SetLanguage(sValue) + " ?", 2) != DialogResult.Yes)
                return;

            string sMaxID = "";
            string sSQL = @"SELECT * FROM SAJET.G_WO_BASE_TEMP
                WHERE WORK_ORDER = :WORK_ORDER ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            DateTime dtSysdate = ClientUtils.GetSysDate();
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sSQL = @"UPDATE SAJET.G_WO_BASE_TEMP 
                      SET UPDATE_TIME = :UPDATE_TIME 
                         ,FLAG =:FLAG 
                    WHERE WORK_ORDER =:WORK_ORDER ";
                Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtSysdate };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FLAG", sTag };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
                ClientUtils.ExecuteSQL(sSQL, Params);
            }
            else
            {
                sMaxID = SajetCommon.GetMaxID("SAJET.G_WO_BASE_TEMP", "TXN_ID", 10);
                sSQL = @"INSERT INTO SAJET.G_WO_BASE_TEMP 
                    (TXN_ID,WORK_ORDER,UPDATE_TIME,FLAG ) 
                    VALUES 
                    (:TXN_ID,:WORK_ORDER,:UPDATE_TIME,:FLAG ) ";
                Params = new object[4][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TXN_ID", sMaxID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtSysdate };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FLAG", sTag };
                ClientUtils.ExecuteSQL(sSQL, Params);
            }
            sSQL = @"Select SQL_NAME from sajet.sys_sql
                where SYSUSE_NAME='MAC REQUEST' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sFieldName = dsTemp.Tables[0].Rows[0]["SQL_NAME"].ToString();
                sSQL = string.Format(@"UPDATE SAJET.G_WO_BASE 
                      SET {0}=:FLAG 
                         ,UPDATE_USERID =:UPDATE_USERID 
                         ,UPDATE_TIME =:UPDATE_TIME 
                    WHERE WORK_ORDER =:WORK_ORDER ", sFieldName);
                Params = new object[4][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FLAG", sTag };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtSysdate };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
                ClientUtils.ExecuteSQL(sSQL, Params);
                fMain.CopyToHistory(sWorkOrder);
                ShowData();
                SetSelectRow(g_sDataSQL, g_Params, gvData, sWorkOrder, TableDefine.gsDef_KeyField);
            }
            dsTemp.Dispose();
        }

        private void btnBindingSEQ_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (gvData.SelectedRows.Count > 0)
            {
                var _row = gvData.CurrentRow;



                var _SQL = $@"SELECT DISTINCT TB1.MODEL_ID,TB2.MODEL_NAME FROM SAJET.SYS_PART TB1, SAJET.SYS_MODEL TB2 
                WHERE TB1.MODEL_ID = TB2.MODEL_ID AND TB1.PART_ID = {_row.Cells["PART_ID"].ToString()}";
                var ds =ClientUtils.ExecuteSQL(_SQL);


                if (ds.Tables[0].Rows.Count > 0)
                {

                    string _ModelID = _row.Cells["MODEL_ID_NEW"].Value.ToString();
                    string _ModelName = _row.Cells["MODEL_NAME_NEW"].Value.ToString();
                    string _RouteID = _row.Cells["ROUTE_ID"].Value.ToString();
                    string _WO = _row.Cells["WORK_ORDER"].Value.ToString();

                    fProcessLink f = new fProcessLink();
                    f.g_sModelID = _ModelID;
                    f.g_sModelName = _ModelName;
                    f.g_sRouteID = _RouteID;
                    f.g_sWorkOrder = _WO;
                    f.ShowDialog();
                }
            }
            else 
            {
                MessageBox.Show("�п�ܤu��");
            }
        }

        private void MenuMACRequest_DropDownOpened(object sender, EventArgs e)
        {
            string sWorkOrder = gvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
            string sFlag = GetWOMACRequest(sWorkOrder);
            MenuMACRequestYes.Enabled = false;
            MenuMACRequestNo.Enabled = false;
            if (sFlag == "N")
            {
                MenuMACRequestYes.Enabled = true;
            }
            if (sFlag == "Y")
                MenuMACRequestNo.Enabled = true;

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var _CurrentRwo = gvData.CurrentRow;

            if (_CurrentRwo != null)
            {
                var _WO = _CurrentRwo.Cells["WORK_ORDER"].Value.ToString();
                fSpec f = new fSpec();
                f._WO = _WO;
                f.ShowDialog();
                f.Dispose();
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
            {
                SajetCommon.Show_Message("Please select a work order first", 0);
                return;
            }

            string sWorkOrder = gvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();

            fSnSpecStatus f = new fSnSpecStatus();
            f.g_sWorkOrder = sWorkOrder;
            f.ShowDialog();
            f.Dispose();
        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            SajetInifile sini = new SajetInifile();
            sini.WriteIniFile(sIniFile, g_sProgram, g_sFunction + " Splitter", panel2.Width.ToString());
            sini.Dispose();
        }

        private void dtpCreateDate1_CloseUp(object sender, EventArgs e)
        {
            if (dtpCreateDate1.Value > dtpCreateDate2.Value)
                SajetCommon.Show_Message("Start Date More End Date", 0);
            else ShowData();
        }

        private void dtpCreateDate2_CloseUp(object sender, EventArgs e)
        {
            if (dtpCreateDate1.Value > dtpCreateDate2.Value)
                SajetCommon.Show_Message("End Date Less Start Date", 0);
            else
                ShowData();
        }

        private void chkWOBom_ValueChanged(object sender, EventArgs e)
        {
            ShowData();
        }

        private void btnSpec_Click(object sender, EventArgs e)
        {
            var _CurrentRwo = gvData.CurrentRow;

            if (_CurrentRwo != null)
            {
                var _WO = _CurrentRwo.Cells["WORK_ORDER"].Value.ToString();
                fSpec f = new fSpec();
                f._WO = _WO;
                f.ShowDialog();
                f.Dispose();
            }



        }
    }
}