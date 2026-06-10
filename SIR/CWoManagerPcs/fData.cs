using NPOI.SS.Formula.Functions;
using SajetClass;
using SajetFilter;
using SajetTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CWoManagerPcs
{
    public partial class fData : Form
    {
        class PartData
        {
            private string _PartID = "0";
            private string _Version = "N/A";

            public string PartID
            {
                get => _PartID;
                set => _PartID = value;
            }

            public string Version
            {
                get => _Version;
                set => _Version = value;
            }
        }
        Dictionary<string, fMain.TDBInitial> g_DBInitial = new Dictionary<string, fMain.TDBInitial>();

        public fData(Dictionary<string, fMain.TDBInitial> DBInitial)
        {
            InitializeComponent();
            g_DBInitial = DBInitial;
            CreateTableControl(); //動態建立TableControl行列數
        }
        public string g_sUpdateType, g_sformText;
        public string g_sKeyID, g_sPartID, g_sFactory, g_sExtend6;
        public int g_iBindingBtnPri;
        public DataGridViewRow dataCurrentRow;
        public DataGridViewColumnCollection dataGridColumn;
        public List<string> g_sControl = new List<string>();
        Dictionary<int, int> g_slColumn = new Dictionary<int, int>();
        int g_iWoStatus;
        const string g_sProgramType = "PCS_";

        public struct TControlData
        {
            public string strNecessary;
            public bool bNecessary;
            public string strType;
            public string strField;
            public string strPartField;
            public string strParamField;
            public TextBox txtControl;
            public List<string> ddlValue;
            public ComboBox ddlControl;
            public CheckBox chkControl;
            public DateTimePicker calExtender;
            public Label lablControl;
            public RichTextBox richControl;
        }
        public TControlData[] tControlAdd;

        private void CreateTableControl()
        {
            string sSQL = "SELECT POSITION, COUNT(POSITION) FROM SAJET.SYS_PROGRAM_FUN_MAINTAIN WHERE PROGRAM = :PROGRAM AND FUN_NAME = :FUN_NAME GROUP BY POSITION ORDER BY POSITION";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROGRAM", fMain.g_sProgram };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FUN_NAME", fMain.g_sFunction };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsTemp.Tables[0].Rows)
                {
                    int iPos = int.Parse(dr[0].ToString());
                    int iField = int.Parse(dr[1].ToString());
                    switch (iPos)
                    {
                        case 1:
                        case 2:
                            g_slColumn.Add(iPos, 7);
                            break;
                        default:
                            g_slColumn.Add(iPos, 0);
                            break;
                    }
                    if (tableLayoutPanel1.RowCount < 8 + iField)
                        tableLayoutPanel1.RowCount = 8 + iField;
                }
                if (tableLayoutPanel1.RowCount > 8)
                {
                    panel1.Height = 32 * tableLayoutPanel1.RowCount + 4;
                    this.Height = 208 + tableLayoutPanel1.Height;
                    for (int i = 9; i < tableLayoutPanel1.RowCount; i++)
                        tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32));
                }
                for (int i = 2; i < g_slColumn.Count; i++)
                {
                    tableLayoutPanel1.ColumnCount += 3;
                    tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                    tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100 / 3));
                    tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50));
                    this.Width += 280;
                }
                btnCancel.Left = panel2.Width - 100;
                btnOK.Left = panel2.Width - 187;
                tableLayoutPanel1.ColumnStyles[1].Width = 100 / g_slColumn.Count;
                tableLayoutPanel1.ColumnStyles[4].Width = 100 / g_slColumn.Count;
            }
            dsTemp.Dispose();
        }
        private void CreateOptionControl()
        {
            //動態建立元件
            int iRowNo;
            Font fFont = LabWO.Font;
            string sSQL = "SELECT * FROM SAJET.SYS_PROGRAM_FUN_MAINTAIN WHERE PROGRAM = :PROGRAM AND FUN_NAME = :FUN_NAME ORDER BY POSITION, DISPLAY_SEQ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROGRAM", fMain.g_sProgram };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FUN_NAME", fMain.g_sFunction };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            tControlAdd = new TControlData[dsTemp.Tables[0].Rows.Count];
            int iCount = 0;
            Label labTemp = new Label();
            ComboBox ddlTemp;
            TextBox txtTemp;
            CheckBox chkTemp;
            RichTextBox richTemp;
            Button btnTemp;
            DataSet dsList = new DataSet();
            foreach (DataRow dr in dsTemp.Tables[0].Rows)
            {
                int iPos = int.Parse(dr["POSITION"].ToString());
                g_slColumn[iPos] += 1;
                iRowNo = g_slColumn[iPos];
                //Label =============================               
                Label labelTemp = new Label();
                labelTemp.Text = SajetCommon.SetLanguage(dr["DISPLAY_NAME"].ToString(), 1);
                labelTemp.Font = fFont;
                labelTemp.Dock = DockStyle.Fill;
                labelTemp.TextAlign = ContentAlignment.MiddleLeft;
                labelTemp.BackColor = Color.Transparent;
                tableLayoutPanel1.Controls.Add(labelTemp, (iPos - 1) * 3, iRowNo);
                //0: Text Box, 1: SQL, 2: Fixed, 3: Date, 4: Label, 5: Check Box, 6: Numeric, 7: Rich Text Box, 8: 只會顯示Label說明使用, 9: 小數, 10: Filter
                switch (dr["FIELD_TYPE"].ToString())
                {
                    case "8":
                        tableLayoutPanel1.SetColumnSpan(labelTemp, 3);
                        Array.Resize(ref tControlAdd, tControlAdd.Length - 1);
                        continue;
                    case "3": // Date
                        DateTimePicker calExtender = new DateTimePicker();
                        calExtender.Dock = DockStyle.Fill;
                        calExtender.Font = fFont;
                        calExtender.Tag = dr["FIELD_NAME"].ToString();
                        calExtender.Format = DateTimePickerFormat.Custom;
                        calExtender.CustomFormat = "yyyy/MM/dd";
                        if (dataCurrentRow != null)
                            if (dataGridColumn.Contains(dr["FIELD_NAME"].ToString()))
                                if (!string.IsNullOrEmpty(dataCurrentRow.Cells[dr["FIELD_NAME"].ToString()].Value.ToString()))
                                    calExtender.Value = (DateTime)dataCurrentRow.Cells[dr["FIELD_NAME"].ToString()].Value;
                        tableLayoutPanel1.Controls.Add(calExtender, (iPos - 1) * 3 + 1, iRowNo);
                        if (string.IsNullOrEmpty(dr["UNIT_LABEL"].ToString()))
                            tableLayoutPanel1.SetColumnSpan(calExtender, 2);
                        else
                        {
                            labTemp = new Label();
                            labTemp.Text = SajetCommon.SetLanguage(dr["UNIT_LABEL"].ToString(), 1);
                            tableLayoutPanel1.Controls.Add(calExtender, (iPos - 1) * 3 + 2, iRowNo);
                        }
                        tControlAdd[iCount].calExtender = calExtender;
                        break;
                    case "1": // SQL 
                        dsList = ClientUtils.ExecuteSQL(dr["FIELD_VALUE"].ToString());
                        ddlTemp = new ComboBox();
                        ddlTemp.Dock = DockStyle.Fill;
                        ddlTemp.Font = fFont;
                        ddlTemp.Tag = dr["FIELD_NAME"].ToString();
                        tControlAdd[iCount].ddlValue = new List<string>();
                        foreach (DataRow dr1 in dsList.Tables[0].Rows)
                        {
                            ddlTemp.Items.Add(dr1[1].ToString());
                            tControlAdd[iCount].ddlValue.Add(dr1[0].ToString());
                        }
                        tableLayoutPanel1.Controls.Add(ddlTemp, (iPos - 1) * 3 + 1, iRowNo);
                        if (dataCurrentRow != null)
                            if (dataGridColumn.Contains(dr["FIELD_NAME"].ToString()))
                                ddlTemp.SelectedIndex = ddlTemp.Items.IndexOf(dataCurrentRow.Cells[dr["FIELD_NAME"].ToString()].Value.ToString());
                        if (string.IsNullOrEmpty(dr["UNIT_LABEL"].ToString()))
                            tableLayoutPanel1.SetColumnSpan(ddlTemp, 2);
                        else
                        {
                            labTemp = new Label();
                            labTemp.Text = SajetCommon.SetLanguage(dr["UNIT_LABEL"].ToString(), 1);
                            tableLayoutPanel1.Controls.Add(ddlTemp, (iPos - 1) * 3 + 2, iRowNo);
                        }
                        tControlAdd[iCount].ddlControl = ddlTemp;
                        break;
                    case "2": // Fixed
                        string[] slDisplay = dr["DISPLAY_VALUE"].ToString().Split(',');
                        string[] slValue = dr["FIELD_VALUE"].ToString().Split(',');
                        ddlTemp = new ComboBox();
                        ddlTemp.Dock = DockStyle.Fill;
                        ddlTemp.DropDownStyle = ComboBoxStyle.DropDownList;
                        ddlTemp.Tag = dr["FIELD_NAME"].ToString();
                        ddlTemp.Font = fFont;
                        tControlAdd[iCount].ddlValue = new List<string>();
                        for (int iCol = 0; iCol < slValue.Length; iCol++)
                        {
                            ddlTemp.Items.Add(slDisplay[iCol]);
                            tControlAdd[iCount].ddlValue.Add(slValue[iCol]);
                        }
                        if (dataCurrentRow != null)
                            if (dataGridColumn.Contains(dr["FIELD_NAME"].ToString()))
                                ddlTemp.SelectedIndex = tControlAdd[iCount].ddlValue.IndexOf(dataCurrentRow.Cells[dr["FIELD_NAME"].ToString()].Value.ToString());
                        tableLayoutPanel1.Controls.Add(ddlTemp, (iPos - 1) * 3 + 1, iRowNo);
                        if (string.IsNullOrEmpty(dr["UNIT_LABEL"].ToString()))
                            tableLayoutPanel1.SetColumnSpan(ddlTemp, 2);
                        else
                        {
                            labTemp = new Label();
                            labTemp.Text = SajetCommon.SetLanguage(dr["UNIT_LABEL"].ToString(), 1);
                            tableLayoutPanel1.Controls.Add(ddlTemp, (iPos - 1) * 3 + 2, iRowNo);
                        }
                        tControlAdd[iCount].ddlControl = ddlTemp;
                        break;
                    case "4": // Label
                        labTemp = new Label();
                        labTemp.TextAlign = ContentAlignment.MiddleLeft;
                        labTemp.Dock = DockStyle.Fill;
                        labTemp.Text = string.Empty;
                        labTemp.Font = LabFactory.Font;
                        labTemp.ForeColor = Color.FromArgb(0, 0, 192);
                        if (dataCurrentRow != null)
                            if (dataGridColumn.Contains(dr["FIELD_NAME"].ToString()))
                                labTemp.Text = dataCurrentRow.Cells[dr["FIELD_NAME"].ToString()].Value.ToString();
                        tableLayoutPanel1.Controls.Add(labTemp, (iPos - 1) * 3 + 1, iRowNo);
                        if (string.IsNullOrEmpty(dr["UNIT_LABEL"].ToString()))
                            tableLayoutPanel1.SetColumnSpan(labTemp, 2);
                        else
                        {
                            labTemp = new Label();
                            labTemp.Text = SajetCommon.SetLanguage(dr["UNIT_LABEL"].ToString(), 1);
                            tableLayoutPanel1.Controls.Add(labTemp, (iPos - 1) * 3 + 2, iRowNo);
                        }
                        Array.Resize(ref tControlAdd, tControlAdd.Length - 1);
                        continue;
                    case "5": // CheckBox
                        chkTemp = new CheckBox();
                        chkTemp.Dock = DockStyle.Fill;
                        chkTemp.Font = fFont;
                        if (dataCurrentRow != null)
                            if (dataGridColumn.Contains(dr["FIELD_NAME"].ToString()))
                                chkTemp.Checked = dataCurrentRow.Cells[dr["FIELD_NAME"].ToString()].Value.ToString() == "1";
                        chkTemp.Tag = dr["FIELD_NAME"].ToString();
                        tableLayoutPanel1.Controls.Add(chkTemp, (iPos - 1) * 3 + 1, iRowNo);
                        if (string.IsNullOrEmpty(dr["UNIT_LABEL"].ToString()))
                            tableLayoutPanel1.SetColumnSpan(chkTemp, 2);
                        else
                        {
                            labTemp = new Label();
                            labTemp.Text = SajetCommon.SetLanguage(dr["UNIT_LABEL"].ToString(), 1);
                            tableLayoutPanel1.Controls.Add(chkTemp, (iPos - 1) * 3 + 2, iRowNo);
                        }
                        tControlAdd[iCount].chkControl = chkTemp;
                        break;
                    /*case "6": // Numeric 改為TextBox判斷, 不然會有最大值的問題
                        numTemp = new NumericUpDown();
                        numTemp.Dock = DockStyle.Fill;
                        numTemp.Font = fFont;
                        if (dataCurrentRow != null)
                            if (dataGridColumn.Contains(dr["FIELD_NAME"].ToString()))
                                numTemp.Text = dataCurrentRow.Cells[dr["FIELD_NAME"].ToString()].Value.ToString();
                        numTemp.Tag = dr["FIELD_NAME"].ToString();
                        tableLayoutPanel1.Controls.Add(numTemp, (iPos - 1) * 3 + 1, iRowNo);
                        if (string.IsNullOrEmpty(dr["UNIT_LABEL"].ToString()))
                            tableLayoutPanel1.SetColumnSpan(numTemp, 2);
                        else
                        {
                            labTemp = new Label();
                            labTemp.Text = SajetCommon.SetLanguage(dr["UNIT_LABEL"].ToString(), 1);
                            tableLayoutPanel1.Controls.Add(numTemp, (iPos - 1) * 3 + 2, iRowNo);
                        }
                        tControlAdd[iCount].numControl = numTemp;
                        break;*/
                    case "7": // RichTextBox
                        richTemp = new RichTextBox();
                        richTemp.Dock = DockStyle.Fill;
                        richTemp.Font = fFont;
                        if (dataCurrentRow != null)
                            if (dataGridColumn.Contains(dr["FIELD_NAME"].ToString()))
                                richTemp.Text = dataCurrentRow.Cells[dr["FIELD_NAME"].ToString()].Value.ToString();
                        richTemp.Tag = dr["FIELD_NAME"].ToString();
                        tableLayoutPanel1.Controls.Add(richTemp, (iPos - 1) * 3 + 1, iRowNo);
                        if (string.IsNullOrEmpty(dr["UNIT_LABEL"].ToString()))
                            tableLayoutPanel1.SetColumnSpan(richTemp, 2);
                        else
                        {
                            labTemp = new Label();
                            labTemp.Text = SajetCommon.SetLanguage(dr["UNIT_LABEL"].ToString(), 1);
                            tableLayoutPanel1.Controls.Add(richTemp, (iPos - 1) * 3 + 2, iRowNo);
                        }
                        tControlAdd[iCount].richControl = richTemp;
                        break;
                    default: // TextBox
                        txtTemp = new TextBox();
                        txtTemp.Dock = DockStyle.Fill;
                        txtTemp.Font = fFont;
                        if (dataCurrentRow != null)
                            if (dataGridColumn.Contains(dr["FIELD_NAME"].ToString()))
                                txtTemp.Text = dataCurrentRow.Cells[dr["FIELD_NAME"].ToString()].Value.ToString();
                        txtTemp.Tag = dr["FIELD_NAME"].ToString();
                        if (dr["DISPLAY_NAME"].ToString() == "Seq Position")
                        {
                            if (dataCurrentRow == null || string.IsNullOrWhiteSpace(dataCurrentRow.Cells[dr["FIELD_NAME"].ToString()].Value.ToString()))
                            {
                                txtTemp.Text = "[霜阨?嶱宎弇],[霜阨?墿僅]";
                                txtTemp.ForeColor = Color.Gray;
                            }


                            txtTemp.Enter += SeqIndexLength_Enter;
                            txtTemp.Leave += SeqIndexLength_Leave;
                        }
                        tableLayoutPanel1.Controls.Add(txtTemp, (iPos - 1) * 3 + 1, iRowNo);
                        if (dr["FIELD_TYPE"].ToString() == "10")
                        {
                            btnTemp = new Button();
                            btnTemp.Text = "...";
                            btnTemp.Tag = dr["FIELD_VALUE"].ToString();
                            btnTemp.Click += new EventHandler(btnTemp_Click);
                            btnTemp.AccessibleName = iCount.ToString();
                            btnTemp.Dock = DockStyle.Fill;
                            btnTemp.Name = dr["FIELD_NAME"].ToString();
                            tableLayoutPanel1.Controls.Add(btnTemp, (iPos - 1) * 3 + 2, iRowNo);
                        }
                        else if (string.IsNullOrEmpty(dr["UNIT_LABEL"].ToString()))
                            tableLayoutPanel1.SetColumnSpan(txtTemp, 2);
                        else
                        {
                            labTemp = new Label();
                            labTemp.Text = SajetCommon.SetLanguage(dr["UNIT_LABEL"].ToString(), 1);
                            labTemp.TextAlign = ContentAlignment.MiddleLeft;
                            labTemp.Dock = DockStyle.Fill;
                            labTemp.Font = fFont;
                            tableLayoutPanel1.Controls.Add(labTemp, (iPos - 1) * 3 + 2, iRowNo);
                        }
                        tControlAdd[iCount].txtControl = txtTemp;
                        break;
                }
                g_sControl.Add(dr["FIELD_NAME"].ToString());
                tControlAdd[iCount].lablControl = labelTemp;
                tControlAdd[iCount].strField = dr["FIELD_NAME"].ToString();
                tControlAdd[iCount].strType = dr["FIELD_TYPE"].ToString();
                tControlAdd[iCount].bNecessary = dr["NECESSARY_FLAG"].ToString() == "1";
                tControlAdd[iCount].strNecessary = dr["NECESSARY_ITEM"].ToString();
                tControlAdd[iCount].strPartField = dr["PART_FIELD"].ToString();
                tControlAdd[iCount].strParamField = dr["PARAM_FIELD"].ToString();
                iCount++;
            }
            dsTemp.Dispose();
        }

        private void SeqIndexLength_Leave(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            if (string.IsNullOrEmpty(t.Text))
            {
                t.Text = "[霜阨?嶱宎弇],[霜阨?墿僅]";
                t.ForeColor = Color.Gray;
            }

        }

        private void SeqIndexLength_Enter(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            if (t.Text == "[霜阨?嶱宎弇],[霜阨?墿僅]")
            {
                t.Text = "";
                t.ForeColor = Color.Black;
            }
        }

        private void btnTemp_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            fFilter f = new fFilter();
            var c = tControlAdd[int.Parse(btn.AccessibleName)];
            if (!string.IsNullOrEmpty(c.strParamField))
            {
                if (c.strParamField == "ROUTE_NAME")
                {
                    if (string.IsNullOrEmpty(combRoute.SelectedItem.ToString()))
                        return;
                    f.sSQL = btn.Tag.ToString().Replace($":{c.strParamField}", combRoute.SelectedItem.ToString());
                }
            }
            else
                f.sSQL = btn.Tag.ToString();
            if (f.ShowDialog() == DialogResult.OK)
                c.txtControl.Text = f.dgvData.CurrentRow.Cells[c.strField].Value.ToString();
        }
        private List<WOProperty> properties = new List<WOProperty>();
        public bool IsModifyProperty
        {
            // 新增工單 或 工單狀態為準備時，可編輯屬性
            get => string.IsNullOrEmpty(LabWoStatus.Text) || (SajetCommon.SetLanguage(LabWoStatus.Text) == SajetCommon.SetLanguage(WorkOrderStatus.Prepare.ToString()) && fMain.g_iPrivilege != 0);
        }

        private void fData_Load(object sender, EventArgs e)
        {
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            CreateOptionControl();
            LVPkSPec.Columns.Clear();
            string[] slValue, slWidth;
            DMultioption = new Dictionary<string, string>();
            if (g_DBInitial.ContainsKey(g_sProgramType + "Packing Spec Title"))
            {
                slValue = g_DBInitial[g_sProgramType + "Packing Spec Title"].sValue.ToString().Split(',');
                slWidth = g_DBInitial[g_sProgramType + "Packing Spec Title"].sDefault.ToString().Split(',');
            }
            else
            {
                slValue = new string[] { "PKSPEC_NAME", "BOX_QTY", "CARTON_QTY", "PALLET_QTY" };
                slWidth = new string[] { "200", "90", "90", "90" };
            }
            for (int i = 0; i < slValue.Length; i++)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Name = slValue[i];
                ch.Text = slValue[i];
                ch.Width = int.Parse(slWidth[i]);
                LVPkSPec.Columns.Add(ch);
            }
            SajetCommon.SetLanguageControl(this);
            this.Text = g_sformText;
            LabFactory.Text = g_sFactory;
            LabWoStatus.Text = string.Empty;

            dtScheduleDate.Value = DateTime.Today;
            dtDueDate.Value = DateTime.Today;

            //Select WO Rule            
            combWoRule.Items.Clear();
            combWoRule.Items.Add("");
            string sSQL = @" Select FUNCTION_NAME 
                 From SAJET.SYS_MODULE_PARAM 
                 Where MODULE_NAME = 'W/O RULE' 
                 Group By FUNCTION_NAME 
                 ORDER BY FUNCTION_NAME";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combWoRule.Items.Add(dsTemp.Tables[0].Rows[i]["FUNCTION_NAME"].ToString());
            }

            //Select Route            
            combRoute.Items.Clear();
            combRoute.Items.Add("");
            sSQL = @" SELECT ROUTE_ID, ROUTE_NAME 
                 FROM SAJET.SYS_ROUTE 
                 WHERE ENABLED = 'Y' 
                 ORDER BY ROUTE_NAME ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combRoute.Items.Add(dsTemp.Tables[0].Rows[i]["ROUTE_NAME"].ToString());
            }
            //Select WO TYPE
            combWoType.Items.Clear();
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FACTORY_ID", fMain.g_sFactoryID };
            if (g_DBInitial.ContainsKey(g_sProgramType + "WO TYPE"))
            {
                if (g_DBInitial[g_sProgramType + "WO TYPE"].sType == "L")
                {
                    slValue = g_DBInitial[g_sProgramType + "WO TYPE"].sDefault.Split(',');
                    foreach (string sValue in slValue)
                        combWoType.Items.Add(sValue);
                }
                else
                {
                    sSQL = g_DBInitial[g_sProgramType + "WO TYPE"].sValue;
                    if (!string.IsNullOrEmpty(g_DBInitial[g_sProgramType + "WO TYPE"].sDefault))
                        dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                    else
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                }
            }
            else
            {
                sSQL = @" Select WO_TYPE FROM SAJET.G_WO_BASE 
                    WHERE FACTORY_ID = :FACTORY_ID AND WO_TYPE IS NOT NULL
                    GROUP BY WO_TYPE ORDER BY WO_TYPE ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }
            if (combWoType.Items.Count == 0)
                for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                {
                    combWoType.Items.Add(dsTemp.Tables[0].Rows[i]["WO_TYPE"].ToString());
                }
            //Select Line            
            combLine.Items.Clear();
            combLine.Items.Add("");
            sSQL = @" select PDLINE_ID,PDLINE_NAME 
                 from SAJET.SYS_PDLINE 
                 where enabled = 'Y' 
                 and Factory_id = :FACTORY_ID
                 order by PDLINE_NAME ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combLine.Items.Add(dsTemp.Tables[0].Rows[i]["PDLINE_NAME"].ToString());
            }
            dsTemp.Dispose();
            if (g_sUpdateType == "MODIFY" || g_sUpdateType == "VIEW")
            {
                g_sExtend6 = dataCurrentRow.Cells["EXTEND6"].Value.ToString();
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                LabWoStatus.Text = dataCurrentRow.Cells["WOSTATUS"].Value.ToString();

                editWO.Text = dataCurrentRow.Cells["WORK_ORDER"].Value.ToString();
                editPart.Text = dataCurrentRow.Cells["PART_NO"].Value.ToString();
                //add by rita 2010/01/20 ERP下來的工單版本若還不在BOM表中,先加入下拉選單,防止版本變為空白
                if (combVersion.Items.IndexOf(dataCurrentRow.Cells["VERSION"].Value.ToString()) < 0)
                    combVersion.Items.Add(dataCurrentRow.Cells["VERSION"].Value.ToString());
                combVersion.Text = dataCurrentRow.Cells["VERSION"].Value.ToString();
                LabVersion.Tag = dataCurrentRow.Cells["VERSION"].Value.ToString();

                combWoRule.SelectedIndex = combWoRule.Items.IndexOf(dataCurrentRow.Cells["WO_RULE"].Value.ToString());
                combWoType.SelectedIndex = combWoType.Items.IndexOf(dataCurrentRow.Cells["WO_TYPE"].Value.ToString());
                if (g_DBInitial.ContainsKey(g_sProgramType + "TARGET_QTY") && !string.IsNullOrEmpty(g_DBInitial[g_sProgramType + "TARGET_QTY"].sValue))
                    editTargetQty.Text = dataCurrentRow.Cells[g_DBInitial[g_sProgramType + "TARGET_QTY"].sValue].Value.ToString();
                else
                    editTargetQty.Text = dataCurrentRow.Cells["TARGET_QTY"].Value.ToString();
                combLine.SelectedIndex = combLine.Items.IndexOf(dataCurrentRow.Cells["PDLINE_NAME"].Value.ToString());
                combRoute.SelectedIndex = combRoute.Items.IndexOf(dataCurrentRow.Cells["ROUTE_NAME"].Value.ToString());
                if (combInProcess.Items.IndexOf(dataCurrentRow.Cells["START_PROCESS"].Value.ToString()) > -1)
                    combInProcess.SelectedIndex = combInProcess.Items.IndexOf(dataCurrentRow.Cells["START_PROCESS"].Value.ToString());
                if (combOutProcess.Items.IndexOf(dataCurrentRow.Cells["END_PROCESS"].Value.ToString()) > -1)
                    combOutProcess.SelectedIndex = combOutProcess.Items.IndexOf(dataCurrentRow.Cells["END_PROCESS"].Value.ToString());
                editRemark.Text = dataCurrentRow.Cells["REMARK"].Value.ToString();
                if (!string.IsNullOrEmpty(dataCurrentRow.Cells["WO_SCHEDULE_DATE"].Value.ToString()))
                {
                    dtScheduleDate.Value = DateTime.Parse(dataCurrentRow.Cells["WO_SCHEDULE_DATE"].Value.ToString());
                }
                if (!string.IsNullOrEmpty(dataCurrentRow.Cells["WO_DUE_DATE"].Value.ToString()))
                {
                    dtDueDate.Value = DateTime.Parse(dataCurrentRow.Cells["WO_DUE_DATE"].Value.ToString());
                }

                ShowWOPackSpecData(g_sKeyID);
                if (g_sUpdateType == "MODIFY")
                {
                    editWO.Enabled = false;
                    g_iWoStatus = Convert.ToInt32(dataCurrentRow.Cells["WO_STATUS"].Value.ToString());

                    //料號按enter
                    var key = new KeyPressEventArgs((char)Keys.Enter);
                    editPart_KeyPress(sender, key);

                    //當狀態為WIP時,不允許修改某些欄位
                    if (g_iWoStatus >= 3)
                    {
                        editPart.Enabled = false;
                        if (g_iWoStatus != 4)
                            editTargetQty.Enabled = false;
                        combVersion.Enabled = false;
                        combRoute.Enabled = false;
                        btnSearchRoute.Enabled = false;
                        if (g_iWoStatus >= 6)
                            btnBindingSEQ.Enabled = false;
                    }


                    //當權限為ReadOnly時,不可以修改任何欄位,只可按Save來準備Wo必要資料               
                    if (fMain.g_iPrivilege == 0)
                    {
                        for (int i = 0; i <= tableLayoutPanel1.Controls.Count - 1; i++)
                        {
                            if (!(tableLayoutPanel1.Controls[i] is Label))
                                tableLayoutPanel1.Controls[i].Enabled = false;
                        }
                    }
                    //當權限為Allow To Change時,只可修改line,Route欄位
                    else if (fMain.g_iPrivilege == 1)
                    {
                        for (int i = 0; i <= tableLayoutPanel1.Controls.Count - 1; i++)
                            tableLayoutPanel1.Controls[i].Enabled = false;

                        combLine.Enabled = true;
                        combRoute.Enabled = true;
                        btnSearchRoute.Enabled = true;
                    }
                }
                else if (g_sUpdateType == "VIEW")
                {
                    btnOK.Enabled = false;
                    for (int i = 0; i <= tableLayoutPanel1.Controls.Count - 1; i++)
                    {
                        if (!(tableLayoutPanel1.Controls[i] is Label))
                            tableLayoutPanel1.Controls[i].Enabled = false;
                    }
                }
            }
            if (LVPkSPec.Items.Count > 0)
                LVPkSPec.Items[0].Selected = true;

            // 載入工單屬性
            properties = GetWOProperty(g_sKeyID, g_sUpdateType == "APPEND");
            if (!IsModifyProperty)
            {
                properties = properties.Where(p => !string.IsNullOrEmpty(p.PropertyValue)).ToList();
                dgvProperty.CellValueChanged -= dgvProperty_CellValueChanged;
            }
            dgvProperty.DataSource = properties.OrderBy(wp => wp.PropertyName).ToList();


            foreach (var r in properties)
                try
                {
                    if (r.INPUT_TYPE == InputType.Query && !string.IsNullOrEmpty(r.SQL_SYNTAX))
                        using (DataTable dt = ClientUtils.ExecuteSQL(r.SQL_SYNTAX).Tables[0])
                        {
                            List<string> ls = dt.AsEnumerable().Select(c => c.Field<string>(0)).ToList();
                            if (!ls.Contains(r.PROPERTY_VALUE) && !string.IsNullOrEmpty(r.PropertyValue))
                                ClientUtils.ShowMessage(
                                    $"{SajetCommon.SetLanguage("Property")}-{SajetCommon.SetLanguage(r.PROPERTY_NAME)}:{r.PROPERTY_VALUE}{Environment.NewLine}{SajetCommon.SetLanguage("NOT Match Rule")}"
                                    , -1);
                        }
                }
                catch { }
            ArrangeTableLayout();

            btnBindingSEQ.Visible = g_iBindingBtnPri > 1;
        }
        public static List<WOProperty> GetWOProperty(string workOrder, bool isDefault = false)
        {
            string sql = @"SELECT WP.PROPERTY_VALUE, P.PROPERTY_ID, P.PROPERTY_NAME, P.VALUE_DEFAULT, P.VALUE_TYPE, P.INPUT_TYPE, P.VALUE_LIST, P.NECESSARY, P.CONVERT_TYPE, P.SQL_SYNTAX, P.PROPERTY_DESC, P.ISREADONLY,P.IS_MULTI
                            FROM SAJET.SYS_PROPERTY P, (SELECT PROPERTY_ID, PROPERTY_VALUE FROM SAJET.G_WO_PROPERTY WHERE WORK_ORDER=:WORK_ORDER) WP 
                            WHERE P.ENABLED='Y' AND P.PROPERTY_TYPE='2' AND P.PROPERTY_ID=WP.PROPERTY_ID(+)
                            ORDER BY P.PROPERTY_NAME";

            using (DataTable dtTemp = ClientUtils.ExecuteSQL(sql, new object[][] {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", workOrder ?? string.Empty }}).Tables[0])
            {
                return dtTemp.AsEnumerable().Select(row => new WOProperty(row, isDefault)).ToList();
            }
        }

        public static void UpdateWOProperty(string workOrder, List<WOProperty> workOrderProps)
        {
            //先確認是否已有資料
            string sql = @"SELECT * FROM SAJET.G_WO_PROPERTY
                                  WHERE WORK_ORDER=:WORK_ORDER";
            DataSet dsWOProperty = ClientUtils.ExecuteSQL(sql, new object[][] { new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", workOrder } });

            //先記錄歷史資訊
            if (dsWOProperty.Tables[0].Rows.Count > 0)
            {
                //已有資料TYPE為M修改
                sql = @"INSERT INTO SAJET.G_HT_WO_PROPERTY 
                           SELECT WORK_ORDER,
                                  PROPERTY_ID,
                                  PROPERTY_VALUE,
                                  UPDATE_USERID,
                                  UPDATE_TIME,
                                  'M' TYPE
                             FROM SAJET.G_WO_PROPERTY
                            WHERE WORK_ORDER=:WORK_ORDER";
                ClientUtils.ExecuteSQL(sql, new object[][] { new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", workOrder } });
            }
            sql = "DELETE SAJET.G_WO_PROPERTY WHERE WORK_ORDER=:WORK_ORDER";
            ClientUtils.ExecuteSQL(sql, new object[][] { new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", workOrder } });

            DateTime dateTime = DateTime.Now;
            sql = @"INSERT INTO SAJET.G_WO_PROPERTY(WORK_ORDER, PROPERTY_ID, PROPERTY_VALUE, UPDATE_USERID, UPDATE_TIME)
                    VALUES(:WORK_ORDER, :PROPERTY_ID, :PROPERTY_VALUE, :UPDATE_USERID, :UPDATE_TIME)";
            foreach (WOProperty WOProp in workOrderProps.Where(p => !string.IsNullOrWhiteSpace(p.PropertyValue)))
            {
                ClientUtils.ExecuteSQL(sql, new object[][] {
                    new object[]{ ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", workOrder },
                    new object[]{ ParameterDirection.Input, OracleType.Number, "PROPERTY_ID", WOProp.PROPERTY_ID },
                    new object[]{ ParameterDirection.Input, OracleType.VarChar, "PROPERTY_VALUE", WOProp.PropertyValue },
                    new object[]{ ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", ClientUtils.UserPara1 },
                    new object[]{ ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dateTime }});
            }
            if (dsWOProperty.Tables[0].Rows.Count == 0)
            {
                //無資料首度新增 TYPE為A新增
                sql = @"INSERT INTO SAJET.G_HT_WO_PROPERTY 
                           SELECT WORK_ORDER,
                                  PROPERTY_ID,
                                  PROPERTY_VALUE,
                                  UPDATE_USERID,
                                  UPDATE_TIME,
                                  'A' TYPE
                             FROM SAJET.G_WO_PROPERTY
                            WHERE WORK_ORDER=:WORK_ORDER";
                ClientUtils.ExecuteSQL(sql, new object[][] { new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", workOrder } });
            }
        }

        public static bool ExistWorkOrder(string workOrder)
        {
            string sql = "SELECT * FROM SAJET.G_WO_BASE WHERE UPPER(WORK_ORDER)=UPPER(:WORK_ORDER)";
            DataSet dataSet = ClientUtils.ExecuteSQL(sql, new object[][] {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", workOrder.Trim() } });

            return dataSet.Tables[0].Rows.Count > 0;
        }
        private void ArrangeTableLayout()
        {
            TableLayoutRowStyleCollection styles = tableLayoutPanel1.RowStyles;
            foreach (RowStyle style in styles)
            {
                // Set the row height to 30 pixels.
                style.SizeType = SizeType.Absolute;
                style.Height = 32;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= tableLayoutPanel1.Controls.Count - 1; i++)
            {
                if (tableLayoutPanel1.Controls[i] is TextBox)
                {
                    tableLayoutPanel1.Controls[i].Text = tableLayoutPanel1.Controls[i].Text.Trim();
                }
            }

            if (string.IsNullOrEmpty(fMain.g_sFactoryID))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Factory cannot be empty", 2), 0);
                return;
            }

            string sMsg;
            if (string.IsNullOrEmpty(editWO.Text))
            {
                string sData = LabWO.Text;
                sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editWO.Focus();
                editWO.SelectAll();
                return;
            }

            //檢查必填欄位是否都已輸入
            if (!Check_Keyin())
            {
                return;
            }

            string sSQL = $@"SELECT OPERATION_SEQ
  FROM SAJET.G_WO_ROUTE
 WHERE WORK_ORDER = '{editWO.Text}'
   AND OPERATION_SEQ NOT LIKE '9%'";
            DataTable dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];
            if (dtTemp.Rows.Count > 0)
            {
                List<string> opSeqList = new List<string>();
                foreach (DataRow item in dtTemp.Rows)
                {
                    opSeqList.Add(item[0].ToString());
                }

                if (g_sExtend6.ToUpper() == "STANDARD")
                {
                    sSQL = $@"SELECT MODEL_ID
  FROM SAJET.SYS_MODEL A
 WHERE A.ROUTE_ID IN
       (SELECT B.ROUTE_ID FROM SAJET.SYS_ROUTE B WHERE B.ROUTE_NAME = '{combRoute.SelectedItem}')";

                    dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];
                    sSQL = string.Empty;
                    if (dtTemp.Rows.Count > 0)
                    {
                        List<string> modelID = new List<string>();
                        for (int i = 0; i < dtTemp.Rows.Count; i++)
                        {
                            modelID.Add(dtTemp.Rows[i][0].ToString());
                        }


                        sSQL = $"SELECT OPERATION_SEQ FROM SAJET.SYS_MODEL_PROCESS WHERE MODEL_ID IN ('{string.Join("','", modelID)}')";
                    }

                }
                else
                {
                    sSQL = $"SELECT OPERATION_SEQ FROM SAJET.SYS_MODEL_PROCESS WHERE WORK_ORDER = '{editWO.Text}'";
                }

                if (!string.IsNullOrWhiteSpace(sSQL))
                {
                    dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];
                    if (dtTemp.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtTemp.Rows)
                        {
                            opSeqList.Remove(item[0].ToString());
                        }
                    }

                    if (opSeqList.Count > 0)
                    {
                        opSeqList = opSeqList.OrderBy(x => x).ToList();
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("SEQ not binding") + "\n" + SajetCommon.SetLanguage("Operation SEQ") + $": {string.Join(",", opSeqList)}", 0);
                        btnBindingSEQ.Focus();
                        return;
                    }
                }
            }

            //檢查是否重複
            sSQL = @" Select * from SAJET.G_WO_BASE 
                 Where WORK_ORDER = :WORK_ORDER ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", editWO.Text };
            if (g_sUpdateType == "MODIFY")
            {
                sSQL = sSQL + " AND WORK_ORDER <> :OLD_WO ";
                Array.Resize(ref Params, 2);
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OLD_WO", g_sKeyID };
            }
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = LabWO.Text + " : " + editWO.Text;
                sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editWO.Focus();
                editWO.SelectAll();
                dsTemp.Dispose();
                return;
            }
            if (IsModifyProperty)
            {
                // 屬性值驗證
                string errorMsg = string.Empty;
                int firstErrorIndex = 0;
                int valueColIndex = dgvProperty.Columns[nameof(WOProperty.PropertyValue)].Index;
                int nameColIndex = dgvProperty.Columns[nameof(WOProperty.PropertyName)].Index;

                foreach (DataGridViewRow row in dgvProperty.Rows)
                {
                    string error = row.Cells[valueColIndex].ErrorText;
                    WOProperty WOProp = (WOProperty)row.DataBoundItem;
                    if (!string.IsNullOrEmpty(error))
                    {
                        if (string.IsNullOrEmpty(errorMsg))
                            firstErrorIndex = row.Index;

                        errorMsg += $"{dgvProperty.Columns[nameColIndex].HeaderText} : {row.Cells[nameColIndex].Value} {error}{Environment.NewLine}";
                    }
                    if (row.Cells[valueColIndex].Value.ToString().Equals("..."))
                    {
                        if (WOProp.NECESSARY)
                        {
                            errorMsg += $"{dgvProperty.Columns[nameColIndex].HeaderText} : {row.Cells[nameColIndex].Value} {error}{Environment.NewLine}";
                        }
                        else
                        {
                            row.Cells[valueColIndex].Value = string.Empty;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    tabControl1.SelectedTab = tpProperty;
                    dgvProperty.CurrentCell = dgvProperty.Rows[firstErrorIndex].Cells[valueColIndex];
                    dgvProperty.BeginEdit(true);

                    SajetCommon.Show_Message(errorMsg, (int)SajetCommon.MessageType.Error);
                    foreach (DataGridViewRow row in dgvProperty.Rows)
                    {
                        WOProperty WOProp = (WOProperty)row.DataBoundItem;
                        if (WOProp.INPUT_TYPE == InputType.SelectList && !WOProp.NECESSARY && WOProp.IS_MULTI.Equals("Y"))
                        {
                            row.Cells[valueColIndex].Value = "...";
                        }
                    }
                    return;
                }
            }
            try
            {
                if (g_sUpdateType == "MODIFY")
                {
                    string sOldRouteID = dataCurrentRow.Cells["ROUTE_ID"].Value.ToString();
                    if (combRoute.Text != dataCurrentRow.Cells["ROUTE_NAME"].Value.ToString())
                    {
                        if (dataCurrentRow.Cells["INPUT_QTY"].Value.ToString() != "0")
                        {
                            sMsg = SajetCommon.SetLanguage("Some SN had Input,", 1);
                            string sConfirm = SajetCommon.SetLanguage("Sure to Change Route", 1);
                            if (SajetCommon.Show_Message(sMsg + Environment.NewLine + sConfirm + " ?", 2) != DialogResult.Yes)
                                return;
                        }
                    }
                }
                int iFixParam = 18;
                Params = new object[iFixParam + tControlAdd.Length][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TACTIVE", g_sUpdateType };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TWO", editWO.Text };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTYPE", combWoType.Text };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRULE", combWoRule.Text };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPART", editPart.Text };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TVERSION", combVersion.Text };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TFACTORYID", fMain.g_sFactoryID };
                Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTARGET", editTargetQty.Text };
                Params[8] = new object[] { ParameterDirection.Input, OracleType.DateTime, "TSCHEDULE", dtScheduleDate.Value };
                Params[9] = new object[] { ParameterDirection.Input, OracleType.DateTime, "TDUE", dtDueDate.Value };
                Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TROUTE", combRoute.Text };
                Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPDLINE", combLine.Text };
                Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSTART", combInProcess.Text };
                Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEND", combOutProcess.Text };
                Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREMARK", editRemark.Text };
                Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", ClientUtils.UserPara1 };
                Params[16] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                Params[17] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TFIELD", "" };
                for (int i = 0; i < tControlAdd.Length; i++)
                {
                    //0: Text Box, 1: SQL, 2: Fixed, 3: Date, 4: Label, 5: Check Box, 6: Numeric, 7: Rich Text Box, 8: 只會顯示Label說明使用. 9: 小數,10: Filter
                    if (tControlAdd[i].txtControl?.Tag.ToString() == "WO_OPTION8" && tControlAdd[i].txtControl?.ForeColor == Color.Gray)
                    {
                        tControlAdd[i].txtControl.Text = "";
                    }
                    switch (tControlAdd[i].strType)
                    {
                        case "1":
                        case "2":
                            if (string.IsNullOrEmpty(tControlAdd[i].ddlControl.Text))
                                Params[iFixParam + i] = new object[] { ParameterDirection.Input, OracleType.VarChar, "T" + tControlAdd[i].strField, string.Empty };
                            else
                            {
                                int iIndex = tControlAdd[i].ddlControl.Items.IndexOf(tControlAdd[i].ddlControl.Text);
                                if (iIndex == -1)
                                {
                                    SajetCommon.Show_Message(string.Format("{0} {1}", tControlAdd[i].lablControl.Text, SajetCommon.SetLanguage("Error", 1)), 0);
                                    editPart.Focus();
                                    return;
                                }
                                Params[iFixParam + i] = new object[] { ParameterDirection.Input, OracleType.VarChar, "T" + tControlAdd[i].strField, tControlAdd[i].ddlValue[iIndex] };
                            }
                            break;
                        case "3":
                            Params[iFixParam + i] = new object[] { ParameterDirection.Input, OracleType.DateTime, "T" + tControlAdd[i].strField, tControlAdd[i].calExtender.Value };
                            break;
                        case "5":
                            Params[iFixParam + i] = new object[] { ParameterDirection.Input, OracleType.VarChar, "T" + tControlAdd[i].strField, tControlAdd[i].chkControl.Checked ? 1 : 0 };
                            break;
                        /*case "6":
                            Params[iFixParam + i] = new object[] { ParameterDirection.Input, OracleType.VarChar, "T" + tControlAdd[i].strField, tControlAdd[i].numControl.Value };
                            break;*/
                        case "7":
                            Params[iFixParam + i] = new object[] { ParameterDirection.Input, OracleType.VarChar, "T" + tControlAdd[i].strField, tControlAdd[i].richControl.Text };
                            break;
                        default:
                            if (tControlAdd[i].strField.ToUpper() == "CUSTOMER_CODE")
                            {
                                string sCustomerCode = tControlAdd[i].txtControl.Text;
                                if (!string.IsNullOrWhiteSpace(sCustomerCode))
                                {
                                    sCustomerCode = sCustomerCode.Split('/')[0];
                                }
                                Params[iFixParam + i] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TCUSTOMER_CODE", sCustomerCode };
                            }
                            else
                                Params[iFixParam + i] = new object[] { ParameterDirection.Input, OracleType.VarChar, "T" + tControlAdd[i].strField, tControlAdd[i].txtControl.Text };
                            break;
                    }
                }
                dsTemp = ClientUtils.ExecuteProc("SAJET.MAINTAIN_WO_PCS", Params);
                sMsg = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
                if (sMsg == "OK")
                {
                    SaveWOPackSpecData();

                    if (IsModifyProperty)
                        UpdateWOProperty(editWO.Text, properties);
                    //Copy Bom
                    if (!CheckBomExist(editWO.Text, g_sPartID))
                    {
                        CopyToWOBom(editWO.Text, g_sPartID, combVersion.Text);
                        CopyToWoBomLoc(editWO.Text, g_sPartID, combVersion.Text);
                    }
                    //Copy WO Rule
                    CopyToWORule(editWO.Text, combWoRule.Text);
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    if (sMsg == "NOT FOUND")
                        SajetCommon.Show_Message(string.Format("{0} {1}", SajetCommon.SetLanguage(sMsg, 1), SajetCommon.SetLanguage(dsTemp.Tables[0].Rows[0]["TFIELD"].ToString(), 1)), 0);
                    else
                        SajetCommon.Show_Message(sMsg, 0);
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                return;
            }
            finally
            {
                dsTemp.Dispose();
            }
        }
        private string GetID(string sTable, string sFieldID, string sFieldName, string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
                return "0";
            string sSQL = "select " + sFieldID + " from " + sTable + " "
                 + "where " + sFieldName + " = '" + sValue + "' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return dsTemp.Tables[0].Rows[0][sFieldID].ToString();
            else
                return "0";
        }

        private void combRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetRouteProcess();

            CheckModelProcess();
        }

        private void GetRouteProcess()
        {
            //帶出此Route一定要過的站  
            string sDefProcess = "";
            combInProcess.Items.Clear();
            combOutProcess.Items.Clear();
            string sRouteName = combRoute.Text;
            string sSQL = @" Select C.PROCESS_NAME,B.RESULT,B.SEQ,B.NECESSARY,B.DEFAULT_INPROCESS 
                From SAJET.SYS_ROUTE A 
                ,SAJET.SYS_ROUTE_DETAIL B 
                ,SAJET.SYS_PROCESS C 
                Where A.ROUTE_NAME = :ROUTE_NAME 
                and B.ENABLED = 'Y'  
                and A.ROUTE_ID = B.ROUTE_ID 
                and B.NEXT_PROCESS_ID = C.PROCESS_ID 
                Order By B.SEQ ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_NAME", sRouteName };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                if (dsTemp.Tables[0].Rows[i]["RESULT"].ToString() == "1")
                    break;
                string sProcess = dsTemp.Tables[0].Rows[i]["PROCESS_NAME"].ToString();
                if (combInProcess.FindString(sProcess) == -1)
                {
                    combInProcess.Items.Add(sProcess);
                    if (dsTemp.Tables[0].Rows[i]["NECESSARY"].ToString() == "Y")
                        combOutProcess.Items.Add(sProcess);
                }
                if (dsTemp.Tables[0].Rows[i]["DEFAULT_INPROCESS"].ToString() == "Y") //Route中設定的預設投入站
                    sDefProcess = dsTemp.Tables[0].Rows[i]["PROCESS_NAME"].ToString();
            }
            if (combInProcess.Items.Count > 0)
            {
                if (!string.IsNullOrEmpty(sDefProcess))
                    combInProcess.SelectedIndex = combInProcess.Items.IndexOf(sDefProcess);
                else
                    combInProcess.SelectedIndex = 0;

                combOutProcess.SelectedIndex = combOutProcess.Items.Count - 1;
            }
            dsTemp.Dispose();
        }
        private void CheckModelProcess()
        {
            if (!btnBindingSEQ.Visible)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(combRoute.Text)
                || string.IsNullOrWhiteSpace(editPart.Text))
            {
                return;
            }

            string sRouteName = combRoute.Text;
            string sPartNo = editPart.Text;
            string sSQL = $@"SELECT OPERATION_SEQ
  FROM SAJET.G_WO_ROUTE
 WHERE WORK_ORDER = '{editWO.Text}'
   AND OPERATION_SEQ NOT LIKE '9%'";
            DataTable dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];
            List<string> lstSeq = new List<string>();
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                lstSeq.Add(dtTemp.Rows[i].ToString());
            }
            if (lstSeq.Count <= 0)
            {
                return;
            }
            if (g_sExtend6.ToUpper() == "STANDARD")
            {

                sSQL = $@"SELECT C.OPERATION_SEQ
  FROM SAJET.SYS_MODEL A
  LEFT JOIN SAJET.SYS_ROUTE B
    ON A.ROUTE_ID = B.ROUTE_ID
  LEFT JOIN SAJET.SYS_MODEL_PROCESS C
    ON A.MODEL_ID = C.MODEL_ID
  LEFT JOIN SAJET.SYS_PART D
    ON A.MODEL_ID = D.MODEL_ID
 WHERE B.ROUTE_NAME = '{sRouteName}'
   AND D.PART_NO = '{sPartNo}'";
            }
            else
            {
                sSQL = $"SELECT OPERATION_SEQ FROM SAJET.SYS_MODEL_PROCESS WHERE WORK_ORDER = '{editWO.Text}'";
            }
            dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];

            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                lstSeq.Remove(dtTemp.Rows[i].ToString());
            }

            if (lstSeq.Count > 0)
            {
                btnBindingSEQ.BackColor = Color.Red;
            }
            else
            {
                btnBindingSEQ.BackColor = SystemColors.Control;
            }
        }

        public bool Check_Keyin()
        {
            //檢查必填欄位(當combobox或edit欄位為黃色時,檢查是否有輸入值)           
            Color RGBColor = Color.FromArgb(255, 255, 128);
            string sControlName;

            for (int i = 0; i <= tableLayoutPanel1.Controls.Count - 1; i++)
            {
                if (tableLayoutPanel1.Controls[i].BackColor != RGBColor) continue;
                switch (tableLayoutPanel1.Controls[i].GetType().Name.ToString())
                {
                    case "TextBox":
                    case "ComboBox":
                    case "RichTextBox":
                        if (string.IsNullOrEmpty(tableLayoutPanel1.Controls[i].Text))
                        {
                            string sData;
                            if (tableLayoutPanel1.Controls[i].Tag == null)
                            {
                                //將元件前面Label的值當message顯示出來
                                if (tableLayoutPanel1.Controls[i] is TextBox)
                                    sControlName = tableLayoutPanel1.Controls[i].Name.ToString().Replace("edit", "Lab");
                                else
                                    sControlName = tableLayoutPanel1.Controls[i].Name.ToString().Replace("comb", "Lab");
                                Control[] c = tableLayoutPanel1.Controls.Find(sControlName, true);
                                sData = c[0].Text;
                            }
                            else
                            {
                                int iIndex = g_sControl.IndexOf(tableLayoutPanel1.Controls[i].Tag.ToString());
                                sData = tControlAdd[iIndex].lablControl.Text;
                            }
                            string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                            SajetCommon.Show_Message(sMsg, 0);
                            tableLayoutPanel1.Controls[i].Focus();
                            return false;
                        }
                        break;
                }
            }
            foreach (TControlData ctrl in tControlAdd)
            {
                if (ctrl.strField == "MASTER_WO" && !string.IsNullOrWhiteSpace(ctrl.txtControl.Text))
                {
                    string sSQL = $@"SELECT WO_STATUS
  FROM SAJET.G_WO_BASE
 WHERE WORK_ORDER = '{ctrl.txtControl.Text}'";
                    DataTable dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];
                    if (dtTemp.Rows.Count == 0)
                    {
                        SajetCommon.Show_Message("Master WO Error", 0);
                        return false;
                    }
                }
                switch (ctrl.strType)
                {
                    case "6":
                        if (!string.IsNullOrEmpty(ctrl.txtControl.Text))
                        {
                            try
                            {
                                int.Parse(ctrl.txtControl.Text);
                            }
                            catch
                            {
                                SajetCommon.Show_Message(string.Format("{0}{1}", ctrl.lablControl.Text, SajetCommon.SetLanguage("Must be integer", 1)), 0);
                                ctrl.txtControl.Focus();
                                ctrl.txtControl.SelectAll();
                                return false;
                            }
                        }
                        break;
                    case "9":
                        if (!string.IsNullOrEmpty(ctrl.txtControl.Text))
                        {
                            try
                            {
                                decimal.Parse(ctrl.txtControl.Text);
                            }
                            catch
                            {
                                SajetCommon.Show_Message(string.Format("{0}{1}", ctrl.lablControl.Text, SajetCommon.SetLanguage("Must be decimal", 1)), 0);
                                ctrl.txtControl.Focus();
                                ctrl.txtControl.SelectAll();
                                return false;
                            }
                        }
                        break;
                }
            }
            //Check Part
            PartData partData = GetPartData(editPart.Text.Trim(), combVersion.Text.Trim());
            g_sPartID = partData.PartID;
            if (g_sPartID == "0")
            {
                SajetCommon.Show_Message("Part No Error", 0);
                editPart.Focus();
                return false;
            }
            //Check Target=====
            if (g_DBInitial.ContainsKey(g_sProgramType + "TARGET_QTY") && g_DBInitial[g_sProgramType + "TARGET_QTY"].sDefault.ToLower() == "decimal")
            {
                decimal iTargetQty = 0;
                //n1226 if (!int.TryParse(editTargetQty.Text, out iTargetQty))
                if (!decimal.TryParse(editTargetQty.Text, out iTargetQty)) //1226
                {
                    SajetCommon.Show_Message("Target Qty must be decimal", 0);
                    editTargetQty.Focus();
                    editTargetQty.SelectAll();
                    return false;
                }
                if (iTargetQty == 0)
                {
                    SajetCommon.Show_Message("Target Qty Error", 0);
                    editTargetQty.Focus();
                    editTargetQty.SelectAll();
                    return false;
                }
            }
            else
            {
                int iTargetQty = 0;
                if (!int.TryParse(editTargetQty.Text, out iTargetQty))
                {
                    SajetCommon.Show_Message("Target Qty must be integer", 0);
                    editTargetQty.Focus();
                    editTargetQty.SelectAll();
                    return false;
                }
                if (iTargetQty == 0)
                {
                    SajetCommon.Show_Message("Target Qty Error", 0);
                    editTargetQty.Focus();
                    editTargetQty.SelectAll();
                    return false;
                }
            }
            return true;
        }

        private void combWoRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            Color RGBColor = Color.FromArgb(255, 255, 128);

            combVersion.BackColor = Color.White;
            combWoType.BackColor = Color.White;
            combLine.BackColor = Color.White;
            editRemark.BackColor = Color.White;
            Color colorTemp = Color.White;
            for (int i = 0; i < tControlAdd.Length; i++)
            {
                //0: Text Box, 1: SQL, 2: Fixed, 3: Date, 4: Label, 5: Check Box, 6: Numeric, 7: Rich Text Box, 8: 只會顯示Label說明使用. 9: 小數,10: Filter
                colorTemp = tControlAdd[i].bNecessary ? RGBColor : Color.White;
                switch (tControlAdd[i].strType)
                {
                    case "1":
                    case "2":
                        tControlAdd[i].ddlControl.BackColor = colorTemp;
                        break;
                    case "3":
                        tControlAdd[i].calExtender.BackColor = colorTemp;
                        break;
                    case "5":
                        tControlAdd[i].chkControl.BackColor = colorTemp;
                        break;
                    /*case "6":
                        tControlAdd[i].numControl.BackColor = colorTemp;
                        break;*/
                    case "7":
                        tControlAdd[i].richControl.BackColor = colorTemp;
                        break;
                    default:
                        tControlAdd[i].txtControl.BackColor = colorTemp;
                        break;
                }
            }
            //哪些欄位為必填
            string sSQL = @"Select PARAME_ITEM,PARAME_VALUE
                From SAJET.SYS_MODULE_PARAM 
                Where MODULE_NAME = 'W/O RULE' 
                and FUNCTION_NAME = :FUNCTION_NAME 
                and PARAME_NAME = 'Necessary Information' ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FUNCTION_NAME", combWoRule.Text };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            foreach (DataRow dr in dsTemp.Tables[0].Rows)
            {
                string sParam_Item = dr["PARAME_ITEM"].ToString().ToUpper();
                string sParam_Value = dr["PARAME_VALUE"].ToString().ToUpper();
                if (sParam_Value != "Y") continue;
                switch (sParam_Item)
                {
                    case "VERSION":
                        combVersion.BackColor = RGBColor;
                        break;
                    case "WO TYPE":
                        combWoType.BackColor = RGBColor;
                        break;
                    case "LINE":
                        combLine.BackColor = RGBColor;
                        break;
                    case "REMARK":
                        editRemark.BackColor = RGBColor;
                        break;
                    default:
                        for (int i = 0; i < tControlAdd.Length; i++)
                        {
                            if (tControlAdd[i].strNecessary == sParam_Item)
                            {
                                //0: Text Box, 1: SQL, 2: Fixed, 3: Date, 4: Label, 5: Check Box, 6: Numeric, 7: Rich Text Box, 8: 只會顯示Label說明使用. 9: 小數,10: Filter
                                switch (tControlAdd[i].strType)
                                {
                                    case "1":
                                    case "2":
                                        tControlAdd[i].ddlControl.BackColor = RGBColor;
                                        break;
                                    case "3":
                                        tControlAdd[i].calExtender.BackColor = RGBColor;
                                        break;
                                    case "5":
                                        tControlAdd[i].chkControl.BackColor = RGBColor;
                                        break;
                                    /*case "6":
                                        tControlAdd[i].numControl.BackColor = RGBColor;
                                        break;*/
                                    case "7":
                                        tControlAdd[i].richControl.BackColor = RGBColor;
                                        break;
                                    default:
                                        tControlAdd[i].txtControl.BackColor = RGBColor;
                                        break;
                                }
                                break;
                            }
                        }
                        break;
                }
            }
            dsTemp.Dispose();
        }

        private void Get_Part_Default_Data()
        {
            //找出此料號的所有預設資料
            string sPartID = GetID("SAJET.SYS_PART", "PART_ID", "PART_NO", editPart.Text.Trim());
            if (sPartID != "0")
            {
                //為空時 才讀取料號預設
                //if (string.IsNullOrEmpty(combVersion.Text))
                GetDefault_Version(editPart.Text.Trim(), LabVersion.Tag.ToString()); //版本

                GetDefault_Route(sPartID); //Route,Rule,BurninTime,Customer 

                GetDefault_Option(sPartID);

                if (LVPkSPec.Items.Count == 0)
                    GetDefault_PKSpec(sPartID);//包裝方式           
            }
        }
        private void GetDefault_Option(string sPartID)
        {
            if (tControlAdd == null) return;
            string sSQL = string.Empty;
            if (g_DBInitial.ContainsKey(g_sProgramType + "PART LIST"))
                sSQL = g_DBInitial[g_sProgramType + "PART LIST"].sValue;
            else
                sSQL = @"SELECT A.*,
       (SELECT B.CUSTOMER_CODE || '/' || B.CUSTOMER_NAME
          FROM SAJET.SYS_CUSTOMER B
         WHERE B.CUSTOMER_CODE = A.OPTION17
           AND ROWNUM = 1) CUSTOMER
  FROM SAJET.SYS_PART A
 WHERE A.PART_ID = :PART_ID
   AND ROWNUM = 1";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            for (int i = 0; i <= tControlAdd.Length - 1; i++)
            {
                if (dsTemp.Tables[0].Columns.Contains(tControlAdd[i].strPartField))
                {
                    switch (tControlAdd[i].strType)
                    {
                        default:
                            //tControlAdd[i].txtControl.Text = dsTemp.Tables[0].Rows[0][tControlAdd[i].strPartField].ToString();
                            //為空時 才讀取料號預設
                            if (string.IsNullOrEmpty(tControlAdd[i].txtControl.Text))
                                tControlAdd[i].txtControl.Text = dsTemp.Tables[0].Rows[0][tControlAdd[i].strPartField].ToString();
                            break;
                    }
                }
            }
            dsTemp.Dispose();
        }
        private void GetDefault_PKSpec(string sPartID)
        {
            string sModelID = "0";
            LVPkSPec.Items.Clear();
            LVPkSPec.Sorting = SortOrder.None;
            string sSQL = string.Empty;
            if (g_DBInitial.ContainsKey(g_sProgramType + "Packing Spec Part"))
                sSQL = g_DBInitial[g_sProgramType + "Packing Spec Part"].sValue;
            if (string.IsNullOrEmpty(sSQL))
                sSQL = @"SELECT C.PKSPEC_ID,C.PKSPEC_NAME,C.BOX_QTY,C.CARTON_QTY,C.PALLET_QTY
                    FROM SAJET.SYS_PART_PKSPEC B, SAJET.SYS_PKSPEC C 
                    WHERE B.PART_ID = :PART_ID
                    AND B.PKSPEC_ID = C.PKSPEC_ID
                    AND C.ENABLED = 'Y'
                    GROUP BY C.PKSPEC_ID,C.PKSPEC_NAME,C.BOX_QTY,C.CARTON_QTY,C.PALLET_QTY
                    ORDER BY C.BOX_QTY DESC, C.CARTON_QTY DESC, C.PALLET_QTY DESC ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            foreach (DataRow dr in dsTemp.Tables[0].Rows)
            {
                LVPkSPec.Items.Add(dr["PKSPEC_NAME"].ToString());
                for (int j = 1; j < LVPkSPec.Columns.Count; j++)
                    LVPkSPec.Items[LVPkSPec.Items.Count - 1].SubItems.Add(dr[LVPkSPec.Columns[j].Name].ToString());
                LVPkSPec.Items[LVPkSPec.Items.Count - 1].Tag = dr["PKSPEC_ID"].ToString();
            }

            //若Part沒預設則找Model的預設值            
            if (LVPkSPec.Items.Count == 0)
            {
                sSQL = "Select Model_ID from SAJET.SYS_PART WHERE PART_ID = '" + sPartID + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                sModelID = dsTemp.Tables[0].Rows[0]["Model_ID"].ToString();
                sSQL = string.Empty;
                if (g_DBInitial.ContainsKey(g_sProgramType + "Packing Spec Model"))
                {
                    if (!string.IsNullOrEmpty(g_DBInitial[g_sProgramType + "Packing Spec Model"].sValue))
                        sSQL = g_DBInitial[g_sProgramType + "Packing Spec Model"].sValue;
                }
                else
                    sSQL = @" Select C.PKSPEC_ID,C.PKSPEC_NAME,C.BOX_QTY,C.CARTON_QTY,C.PALLET_QTY
                     From SAJET.SYS_MODEL_PKSPEC B, SAJET.SYS_PKSPEC C 
                     Where B.MODEL_ID = :MODEL_ID 
                     and B.PKSPEC_ID = C.PKSPEC_ID 
                     and C.ENABLED = 'Y' 
                     Group By C.PKSPEC_ID,C.PKSPEC_NAME,C.BOX_QTY,C.CARTON_QTY,C.PALLET_QTY 
                     ORDER BY C.BOX_QTY desc,C.CARTON_QTY desc,C.PALLET_QTY desc ";
                if (!string.IsNullOrEmpty(sSQL))
                {
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODEL_ID", sModelID };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                    foreach (DataRow dr in dsTemp.Tables[0].Rows)
                    {
                        LVPkSPec.Items.Add(dr["PKSPEC_NAME"].ToString());
                        for (int i = 1; i < LVPkSPec.Columns.Count; i++)
                            LVPkSPec.Items[LVPkSPec.Items.Count - 1].SubItems.Add(dr[LVPkSPec.Columns[i].Name].ToString());
                        LVPkSPec.Items[LVPkSPec.Items.Count - 1].Tag = dr["PKSPEC_ID"].ToString();
                    }
                }
            }
            dsTemp.Dispose();
            LVPkSPec.Sorting = SortOrder.Ascending;
        }
        private void GetDefault_Route(string sPartID)
        {
            string sModelID = "0";
            string sSQL = " Select B.ROUTE_NAME,A.BURNIN_TIME,A.MODEL_ID, A.RULE_SET "
                 + " From SAJET.SYS_PART A "
                 + "     ,SAJET.SYS_ROUTE B "
                 + " Where A.ROUTE_ID = B.ROUTE_ID(+) "
                 + " and A.PART_ID = '" + sPartID + "' ";
            DataSet dsTemp1 = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp1.Tables[0].Rows.Count > 0)
            {
                //為空時 才讀取料號預設
                if (string.IsNullOrEmpty(combRoute.Text))
                {
                    //Route               
                    if (string.IsNullOrEmpty(dsTemp1.Tables[0].Rows[0]["ROUTE_NAME"].ToString()))
                        combRoute.SelectedIndex = -1;
                    else
                    {
                        int iIndex = combRoute.FindString(dsTemp1.Tables[0].Rows[0]["ROUTE_NAME"].ToString());
                        int iOldIndex = combRoute.SelectedIndex;
                        combRoute.SelectedIndex = iIndex;
                        if (iOldIndex == iIndex)
                            GetRouteProcess();
                    }
                }

                //為空時 才讀取料號預設
                if (string.IsNullOrEmpty(combWoRule.Text))
                {
                    //WoRule
                    if (string.IsNullOrEmpty(dsTemp1.Tables[0].Rows[0]["RULE_SET"].ToString()))
                        combWoRule.SelectedIndex = -1;
                    else
                        combWoRule.SelectedIndex = combWoRule.FindString(dsTemp1.Tables[0].Rows[0]["RULE_SET"].ToString());
                    sModelID = dsTemp1.Tables[0].Rows[0]["MODEL_ID"].ToString();
                }
            }
            else
            {
                //若Part沒預設則找Model的預設值
                sSQL = " Select B.ROUTE_NAME,A.BURNIN_TIME,A.RULE_SET "
                     + "       ,A.CUSTOMER_ID,C.customer_code "
                     + " From SAJET.SYS_MODEL A "
                     + "     ,SAJET.SYS_ROUTE B "
                     + "     ,SAJET.SYS_CUSTOMER C "
                     + " Where A.ROUTE_ID = B.ROUTE_ID(+) "
                     + " and A.MODEL_ID = '" + sModelID + "' "
                     + " and A.CUSTOMER_ID = C.CUSTOMER_ID(+) ";
                dsTemp1 = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp1.Tables[0].Rows.Count > 0)
                {
                    if (combRoute.SelectedIndex == -1 && !string.IsNullOrEmpty(dsTemp1.Tables[0].Rows[0]["ROUTE_NAME"].ToString()))
                        combRoute.SelectedIndex = combRoute.FindString(dsTemp1.Tables[0].Rows[0]["ROUTE_NAME"].ToString());
                    /*if (string.IsNullOrEmpty(editBurninTime.Text) || editBurninTime.Text == "0")
                        editBurninTime.Text = dsTemp1.Tables[0].Rows[0]["BURNIN_TIME"].ToString();*/
                    if (combWoRule.SelectedIndex == -1 && !string.IsNullOrEmpty(dsTemp1.Tables[0].Rows[0]["RULE_SET"].ToString()))
                        combWoRule.SelectedIndex = combWoRule.FindString(dsTemp1.Tables[0].Rows[0]["RULE_SET"].ToString());
                    /*if (string.IsNullOrEmpty(dsTemp1.Tables[0].Rows[0]["CUSTOMER_ID"].ToString()))
                        editCustomer.Text = "";
                    else
                        editCustomer.Text = dsTemp1.Tables[0].Rows[0]["customer_code"].ToString();*/
                }
            }
            dsTemp1.Dispose();
        }
        private void GetDefault_Version(string sPartNo, string sVersion)
        {
            combVersion.Items.Clear();
            string sSQL = " Select nvl(b.Version,'N/A') Bom_Version ,b.update_time,a.version Part_Ver "
                        + " From SAJET.SYS_PART A "
                        + "     ,SAJET.SYS_BOM_INFO B "
                        + " Where A.PART_ID = B.PART_ID(+) "
                        + " and A.PART_NO = '" + sPartNo + "' "
                        + " order by b.update_time ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                combVersion.Items.Add(dsTemp.Tables[0].Rows[0]["Part_Ver"].ToString());
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                string sBomVer = dsTemp.Tables[0].Rows[i]["Bom_Version"].ToString();
                if (combVersion.FindString(sBomVer) == -1)
                    combVersion.Items.Add(sBomVer);
            }

            if (combVersion.Items.Count > 0)
                combVersion.SelectedIndex = combVersion.Items.IndexOf(sVersion);
            dsTemp.Dispose();
        }

        private void editPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            var dtRes = ClientUtils.ExecuteSQL(
                @"SELECT PART_NO, VERSION
                    FROM SAJET.SYS_PART 
                    WHERE ENABLED = :ENABLED AND UPPER(PART_NO) = UPPER(:PART_NO)",
                new object[][] {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "ENABLED", "Y" },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", editPart.Text.Trim() }
                }).Tables[0];

            if (dtRes.Rows.Count == 0)
            {
                SajetCommon.Show_Message("Part No Error", 0);
                editPart.Focus();
                editPart.SelectAll();
                return;
            }

            editPart.Text = dtRes.Rows[0]["PART_NO"].ToString();
            LabVersion.Tag = dtRes.Rows[0]["VERSION"].ToString();

            Get_Part_Default_Data();

            CheckModelProcess();
        }

        private void editPart_TextChanged(object sender, EventArgs e)
        {
            combVersion.Items.Clear();
            combWoRule.SelectedIndex = -1;
            combRoute.SelectedIndex = -1;
        }

        private void btnSearchPart_Click(object sender, EventArgs e)
        {
            editPart.Text = editPart.Text.Trim();
            string sMsg = "";
            if (editPart.Text.Length < 1)
            {
                sMsg = SajetCommon.SetLanguage("Please enter less one word to search", 1);
                SajetCommon.Show_Message(sMsg, 0);
                editPart.Text = "";
                return;
            }
            string sSQL = " select part_no,version,spec1,spec2,option6, option4 "
                 + " from sajet.sys_part "
                 + " where enabled = 'Y' "
                 + " and part_no Like '" + editPart.Text + "%' "
                 + " Order By part_no ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            f.Width = (int)(this.Width * 0.7);
            if (f.ShowDialog() == DialogResult.OK)
            {
                editPart.Text = f.dgvData.CurrentRow.Cells["part_no"].Value.ToString();
                LabVersion.Tag = f.dgvData.CurrentRow.Cells["version"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editPart_KeyPress(sender, Key);
            }
        }
        public void ShowWOPackSpecData(string sWO)
        {
            //顯示此工單設定的包裝方式
            LVPkSPec.Items.Clear();
            string sSQL = g_DBInitial[g_sProgramType + "Packing Spec WO"].sValue;
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", editWO.Text };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            LVPkSPec.Sorting = SortOrder.None;
            foreach (DataRow dr in dsTemp.Tables[0].Rows)
            {
                LVPkSPec.Items.Add(dr["pkspec_name"].ToString());
                for (int i = 1; i < LVPkSPec.Columns.Count; i++)
                    LVPkSPec.Items[LVPkSPec.Items.Count - 1].SubItems.Add(dr[LVPkSPec.Columns[i].Name].ToString());
                LVPkSPec.Items[LVPkSPec.Items.Count - 1].Tag = dr["PKSPEC_ID"].ToString();
            }
            LVPkSPec.Sorting = SortOrder.Ascending;
            dsTemp.Dispose();
        }
        public void SaveWOPackSpecData()
        {
            string sSQL = "DELETE FROM SAJET.G_PACK_SPEC WHERE WORK_ORDER = :WORK_ORDER";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", editWO.Text };
            ClientUtils.ExecuteSQL(sSQL, Params);
            if (g_DBInitial.ContainsKey(g_sProgramType + "Packing Spec Part"))
            {
                Params = new object[4 + LVPkSPec.Columns.Count][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TWO", editWO.Text };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", ClientUtils.UserPara1 };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPART", g_sPartID };
                Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                for (int i = 0; i <= LVPkSPec.Items.Count - 1; i++)
                {
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPKSPEC", LVPkSPec.Items[i].Tag.ToString() };
                    for (int j = 1; j < LVPkSPec.Columns.Count; j++)
                        Params[4 + j] = new object[] { ParameterDirection.Input, OracleType.VarChar, "T" + LVPkSPec.Columns[j].Name, LVPkSPec.Items[i].SubItems[j].Text.ToString() };
                    ClientUtils.ExecuteProc("SAJET.MAINTAIN_WO_PKSPEC", Params);
                }
            }
            else
            {
                Params = new object[7][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", editWO.Text };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", g_sPartID };
                for (int i = 0; i <= LVPkSPec.Items.Count - 1; i++)
                {
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOX_CAPACITY", LVPkSPec.Items[i].SubItems[1].Text };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CARTON_CAPACITY", LVPkSPec.Items[i].SubItems[2].Text };
                    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PALLET_CAPACITY", LVPkSPec.Items[i].SubItems[3].Text };
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PKSPEC_ID", LVPkSPec.Items[i].Tag.ToString() };
                    sSQL = @" Insert Into SAJET.G_PACK_SPEC (WORK_ORDER,PART_ID,PKSPEC_ID,PALLET_CAPACITY,CARTON_CAPACITY,BOX_CAPACITY,UPDATE_USERID) 
                        VALUES (:WORK_ORDER, :PART_ID, :PKSPEC_ID, :PALLET_CAPACITY, :CARTON_CAPACITY, :BOX_CAPACITY, :UPDATE_USERID)";
                    ClientUtils.ExecuteSQL(sSQL, Params);
                }
            }
        }
        private void MenuAppend_Click(object sender, EventArgs e)
        {
            fPKSpec fPkSpec = new fPKSpec(g_DBInitial);
            try
            {
                if (fPkSpec.ShowDialog() == DialogResult.OK)
                {
                    LVPkSPec.Sorting = SortOrder.None;
                    string sPKSPEC_ID = fPkSpec.grdViewData.CurrentRow.Cells["PKSPEC_ID"].Value.ToString();
                    if (LVPkSPec.Items.Find(sPKSPEC_ID, false).Length > 0)
                    {
                        SajetCommon.Show_Message("Packing Spec Duplicate", 0);
                        return;
                    }
                    LVPkSPec.Items.Add(fPkSpec.grdViewData.CurrentRow.Cells["PKSPEC_NAME"].Value.ToString());
                    for (int i = 1; i < LVPkSPec.Columns.Count; i++)
                        LVPkSPec.Items[LVPkSPec.Items.Count - 1].SubItems.Add(fPkSpec.grdViewData.CurrentRow.Cells[LVPkSPec.Columns[i].Name].Value.ToString());
                    LVPkSPec.Items[LVPkSPec.Items.Count - 1].Tag = sPKSPEC_ID;
                }
            }
            finally
            {
                fPkSpec.Dispose();
                LVPkSPec.Sorting = SortOrder.Ascending;
            }
        }
        private void MenuRemove_Click(object sender, EventArgs e)
        {
            if (LVPkSPec.SelectedItems.Count > 0)
                if (SajetCommon.Show_Message("Delete?", 2) == DialogResult.Yes)
                {
                    LVPkSPec.Items.RemoveAt(LVPkSPec.SelectedItems[0].Index);
                    if (LVPkSPec.Items.Count > 0)
                        LVPkSPec.Items[0].Selected = true;
                }
        }

        private void editPart_EnabledChanged(object sender, EventArgs e)
        {
            btnSearchPart.Enabled = editPart.Enabled;
        }

        public bool CheckBomExist(string sWO, string sPartId)
        {
            string sSQL = "SELECT WORK_ORDER FROM SAJET.G_WO_BOM "
                 + "WHERE WORK_ORDER ='" + sWO + "' "
                 + "AND PART_ID ='" + sPartId + "' "                   //add by barron 2019/5/9
                 + "AND ROWNUM = 1 ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }
        public void CopyToWOBom(string sWO, string sPartId, string sVer)
        {
            string sSQL;
            string sBomID = "0";

            sSQL = "DELETE FROM SAJET.G_WO_BOM WHERE WORK_ORDER = :WORK_ORDER";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
            ClientUtils.ExecuteSQL(sSQL, Params);

            if (string.IsNullOrEmpty(sVer))
                sVer = "N/A";

            sSQL = @"SELECT BOM_ID FROM SAJET.SYS_BOM_INFO
                WHERE PART_ID = :PART_ID and Version = :VERSION";
            Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartId };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sVer };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sBomID = dsTemp.Tables[0].Rows[0]["BOM_ID"].ToString();
                sSQL = @"INSERT INTO SAJET.G_WO_BOM 
                    (WORK_ORDER, PART_ID, ITEM_PART_ID, ITEM_GROUP, ITEM_COUNT, PROCESS_ID, VERSION, UPDATE_USERID, BOM_OPTION1, BOM_OPTION2, BOM_OPTION3) 
                    SELECT :WORK_ORDER, :PART_ID,ITEM_PART_ID, ITEM_GROUP, ITEM_COUNT, PROCESS_ID, VERSION, :EMP_ID, BOM_OPTION1, BOM_OPTION2, BOM_OPTION3
                    from SAJET.SYS_BOM 
                    WHERE BOM_ID = :BOM_ID 
                    And Enabled='Y' ";
                Params = new object[4][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartId };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBomID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_ID", ClientUtils.UserPara1 };
                ClientUtils.ExecuteSQL(sSQL, Params);
            }
            dsTemp.Dispose();
        }
        public void CopyToWoBomLoc(string sWO, string sPartId, string sVer)
        {
            string sBomID = "0";

            string sSQL = "DELETE SAJET.G_WO_BOM_LOCATION WHERE WORK_ORDER = :WORK_ORDER";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
            ClientUtils.ExecuteSQL(sSQL, Params);

            if (string.IsNullOrEmpty(sVer))
                sVer = "N/A";

            sSQL = "SELECT BOM_ID FROM SAJET.SYS_BOM_INFO "
                 + "WHERE PART_ID = :PART_ID and Version = :VERSION";
            Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartId };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sVer };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sBomID = dsTemp.Tables[0].Rows[0]["BOM_ID"].ToString();
                sSQL = @"Insert Into SAJET.G_WO_BOM_LOCATION 
                    (WORK_ORDER, PART_ID, ITEM_PART_ID, ITEM_GROUP, VERSION, LOCATION, UPDATE_USERID) 
                    Select :WORK_ORDER, :PART_ID,ITEM_PART_ID, ITEM_GROUP, VERSION, LOCATION,:EMP_ID
                    from SAJET.SYS_BOM_LOCATION 
                    WHERE BOM_ID = :BOM_ID
                    And Enabled='Y' ";
                Params = new object[4][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartId };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBomID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_ID", ClientUtils.UserPara1 };
                ClientUtils.ExecuteSQL(sSQL, Params);
            }
            dsTemp.Dispose();
        }
        public void CopyToWORule(string sWO, string sRule)
        {
            string sSQL = @"DELETE FROM SAJET.G_WO_PARAM
                WHERE WORK_ORDER = :WORK_ORDER
                And MODULE_NAME in (select upper(label_name) || ' RULE' from sajet.sys_label) ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
            ClientUtils.ExecuteSQL(sSQL, Params);

            sSQL = @"INSERT INTO SAJET.G_WO_PARAM 
                (WORK_ORDER,MODULE_NAME,FUNCTION_NAME,PARAME_NAME,PARAME_ITEM,PARAME_VALUE,UPDATE_USERID,UPDATE_TIME)
                SELECT :WORK_ORDER, B.RULE_TYPE || ' RULE', B.RULE_NAME, D.PARAME_NAME, D.PARAME_ITEM, D.PARAME_VALUE, :EMP_ID, SYSDATE 
                From SAJET.SYS_MODULE_PARAM A  
                    ,SAJET.SYS_RULE_NAME B 
                	 ,SAJET.SYS_RULE_PARAM D 
                    ,sajet.sys_label c 
                Where A.MODULE_NAME = 'W/O RULE' 
                and A.FUNCTION_NAME = :FUNCTION_NAME 
                and A.PARAME_NAME = c.label_name || ' Rule'  
                and A.PARAME_ITEM = B.RULE_NAME  
                and B.RULE_TYPE = upper(c.label_name) 
                and c.type <> 'U' 
                and B.RULE_ID = D.RULE_ID ";
            Params = new object[3][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_ID", ClientUtils.UserPara1 };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FUNCTION_NAME", sRule };
            ClientUtils.ExecuteSQL(sSQL, Params);
        }

        private void btnBOM_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(editPart.Text))
                return;
            PartData partData = GetPartData(editPart.Text.Trim(), combVersion.Text.Trim());
            string sPartID = partData.PartID;
            string sVer = partData.Version;
            if (sPartID == "0")
                return;
            string sRouteID = string.Empty;

            string sSQL = $@"SELECT B.ROUTE_ID FROM SAJET.SYS_ROUTE B WHERE B.ROUTE_NAME = '{combRoute.SelectedItem}'";
            var dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];
            if (dtTemp.Rows.Count > 0)
            {
                sRouteID = dtTemp.Rows[0][0].ToString();
            }

            fWoBom fWoBom = new fWoBom();
            fWoBom.LabWO.Text = editWO.Text;
            fWoBom.LabPartNo.Text = editPart.Text;
            fWoBom.LabVer.Text = sVer;
            fWoBom.g_sPartID = sPartID;
            fWoBom.g_sRouteID = sRouteID;

            //WIP只可以讀,不可修改            
            if (/*g_iWoStatus == 3 ||*/ g_iWoStatus > 4)
            {
                fWoBom.LabType.Visible = true;
                fWoBom.TreeBomData.AllowDrop = false;
                fWoBom.LVPart.AllowDrop = false;
                fWoBom.MenuItemDelete.Visible = false;
                fWoBom.MenuItemModify.Visible = false;

                fWoBom.PopMenu2.Opening -= fWoBom.PopMenu2_Opening;

            }
            else
            {
                fWoBom.PopMenu2.Opening += fWoBom.PopMenu2_Opening;
                fWoBom.LabType.Visible = false;
            }

            fWoBom.ShowBom(sPartID, sVer);
            fWoBom.ShowDialog();
            fWoBom.Dispose();
        }

        private void fData_Shown(object sender, EventArgs e)
        {
            if (g_sUpdateType == "APPEND")
                editWO.Focus();
        }

        private void editTargetQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            //輸入字母數字、部份符號                 
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9') || (e.KeyChar == '\b')))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == '.' || e.KeyChar == '-' || e.KeyChar == '_')
            {
                e.KeyChar = (char)Keys.None;
            }
            else
            {
                e.Handled = true;
            }

        }
        private void dgvProperty_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            WOProperty WOProp = (WOProperty)dgvProperty.Rows[e.RowIndex].DataBoundItem;
            //string sTest = nameof(WOProperty.PROPERTY_VALUE);
            if (dgvProperty.Columns[e.ColumnIndex].Name.Equals("PropertyValue") && grid[e.ColumnIndex, e.RowIndex] is DataGridViewButtonCell)
            {
                fMultiSelect f = new fMultiSelect(this);
                string[] sOptions = DMultioption[dgvProperty.Rows[e.RowIndex].Cells["PropertyName"].Value.ToString()].Split(',');
                DataTable dtOption = new DataTable();
                dtOption.Columns.Add("Option", typeof(string));
                dtOption.Columns.Add(f.dgv_Select.CheckedColumnName, typeof(bool));
                f.iCurrent = e.RowIndex;
                for (int i = 0; i < sOptions.Length; i++)
                {
                    dtOption.Rows.Add(sOptions[i]);
                }
                f.dgv_Select.DataSource = dtOption;
                f.dgv_Select.VisibleColumns = new SajetMES.UI.DataViewer.ColumnDefine[]{
                    new SajetMES.UI.DataViewer.ColumnDefine(){ ColumnName = f.dgv_Select.CheckedColumnName, HeaderText="" },
                     new SajetMES.UI.DataViewer.ColumnDefine(){ ColumnName = "Option", HeaderText=SajetCommon.SetLanguage("Option") }
                };
                f.dgv_Select.DataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                if (WOProp.NECESSARY)
                {
                    f.sNECESSARY = "Y";
                }
                else
                {
                    f.sNECESSARY = "N";
                }
                f.ShowDialog();
            }

        }
        Dictionary<string, string> DMultioption;
        private string ValidateProp(WOProperty WOProp, string value = null)
        {
            string errorMsg = string.Empty;
            string info = string.Empty;

            ErrorType error = WOProp.Validate(value);

            switch (error)
            {
                case ErrorType.Necessary:
                    errorMsg = "Data is null";
                    break;
                case ErrorType.ValueType:
                    errorMsg = "Data Type Error";
                    if (WOProp.INPUT_TYPE == InputType.SelectList)
                    {
                        if (WOProp.IS_MULTI.Equals("Y"))
                        {
                            errorMsg = string.Empty;
                        }
                    }
                    break;
                case ErrorType.InputType:
                    errorMsg = "Data Range Error";
                    if (WOProp.INPUT_TYPE == InputType.Range)
                        info = WOProp.VALUE_LIST.Replace(",", "~");
                    if (WOProp.INPUT_TYPE == InputType.SelectList)
                    {
                        if (WOProp.IS_MULTI.Equals("Y"))
                        {
                            errorMsg = string.Empty;
                        }
                    }
                    break;
            }

            if (string.IsNullOrEmpty(errorMsg))
                return string.Empty;
            else
                return $"{SajetCommon.SetLanguage(errorMsg)} {info}";
        }
        private void dgvProperty_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView dgv = ((DataGridView)sender);

            int valueColIndex = dgv.Columns[nameof(WOProperty.PropertyValue)].Index;
            int nameColIndex = dgv.Columns[nameof(WOProperty.PropertyName)].Index;

            dgv.Columns[valueColIndex].MinimumWidth = 150;

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.HeaderText = SajetCommon.SetLanguage(col.HeaderText);

                if (col.Index == valueColIndex)
                {
                    col.ReadOnly = !IsModifyProperty;
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
            }

            if (IsModifyProperty)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    WOProperty WOProp = (WOProperty)row.DataBoundItem;
                    // 輸入方式: 數值列表
                    if (WOProp.INPUT_TYPE == InputType.SelectList)
                    {
                        if (WOProp.IS_MULTI.Equals("Y"))
                        {
                            DataGridViewButtonCell tCellButton = new DataGridViewButtonCell();
                            row.Cells[valueColIndex] = tCellButton;
                            if (row.Cells[valueColIndex].Value.ToString().Equals(string.Empty))
                            {
                                row.Cells[valueColIndex].Value = "...";
                            }
                            dgvProperty.CellContentClick -= dgvProperty_CellContentClick;
                            dgvProperty.CellContentClick += dgvProperty_CellContentClick;
                            if (!DMultioption.ContainsKey(row.Cells[nameColIndex].Value.ToString()))
                            {
                                DMultioption.Add(row.Cells[nameColIndex].Value.ToString(), WOProp.VALUE_LIST);
                            }
                        }
                        else
                        {
                            DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
                            comboBoxCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                            comboBoxCell.Items.AddRange(WOProp.ValueList());
                            row.Cells[valueColIndex] = comboBoxCell;
                        }
                    }
                    if (WOProp.INPUT_TYPE == InputType.Query)
                    {
                        try
                        {
                            DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
                            comboBoxCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                            comboBoxCell.Items.AddRange(WOProp.ListByQuery());
                            row.Cells[valueColIndex] = comboBoxCell;
                        }
                        catch { }
                    }
                    // 屬性值驗證
                    row.Cells[valueColIndex].ErrorText = ValidateProp(WOProp) + $"{(string.IsNullOrEmpty(ValidateProp(WOProp)) ? string.Empty : (" : " + WOProp.PROPERTY_VALUE))}";


                }
            }
            #region 唯獨鎖定
            for (int i = 0; i < dgvProperty.RowCount; i++)
            {
                if (((CWoManagerPcs.WOProperty)dgvProperty.Rows[i].DataBoundItem).ISREADONLY == "Y" & g_sUpdateType == "MODIFY")
                {
                    string sql = @" SELECT *
                                      FROM SAJET.G_WO_PROPERTY 
                                     WHERE WORK_ORDER = :WORK_ORDER 
                                       AND PROPERTY_ID = :PROPERTY_ID";
                    object[][] Param = new object[2][];
                    Param[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", editWO.Text };
                    Param[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROPERTY_ID", ((CWoManagerPcs.WOProperty)dgvProperty.Rows[i].DataBoundItem).PROPERTY_ID };
                    DataSet ds = ClientUtils.ExecuteSQL(sql, Param);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        dgvProperty.Rows[i].Cells["PropertyValue"].ReadOnly = true;
                    }


                }
            }
            #endregion
        }
        private Color requiredColor = Color.FromArgb(255, 255, 192);
        private Color optionalColor = Color.White;
        private void dgvProperty_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvProperty.Columns[nameof(WOProperty.PropertyValue)].Index)
            {
                WOProperty WOProp = (WOProperty)dgvProperty.Rows[e.RowIndex].DataBoundItem;
                // 必要輸入欄位，淡黃底色
                if (WOProp.NECESSARY && IsModifyProperty)
                    e.CellStyle.BackColor = requiredColor;
                // 數值類型: 數字，靠右
                if (WOProp.VALUE_TYPE == ValueType.Number)
                    if (WOProp.IS_MULTI.Equals("N"))
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void dgvProperty_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1
            && e.ColumnIndex == dgvProperty.Columns[nameof(WOProperty.PropertyValue)].Index)
            {
                WOProperty WOProp = (WOProperty)dgvProperty.Rows[e.RowIndex].DataBoundItem;
                // 屬性值驗證
                dgvProperty.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = ValidateProp(WOProp);
            }
        }

        private void btnBindingSEQ_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(editWO.Text))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + LabWO.Text, 1);
                return;
            }

            if (string.IsNullOrEmpty(combRoute.SelectedItem?.ToString()))
            {
                SajetCommon.Show_Message("Pls select route", 1);
                return;
            }

            string sSQL = $@"SELECT B.ROUTE_ID FROM SAJET.SYS_ROUTE B WHERE B.ROUTE_NAME = '{combRoute.SelectedItem}'";
            DataTable dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];
            string sRouteID = dtTemp.Rows[0][0].ToString();

            string sModelID = string.Empty, sModelName = string.Empty;

            if (g_sExtend6.ToUpper() == "STANDARD")
            {
                if (string.IsNullOrEmpty(editPart.Text))
                {
                    SajetCommon.Show_Message("Pls select part", 1);
                    return;
                }

                sSQL = $@"SELECT MODEL_ID FROM SAJET.SYS_PART WHERE PART_NO = '{editPart.Text}'";
                dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];

                if (dtTemp.Rows.Count == 0)
                {
                    SajetCommon.Show_Message("Part No Error", 1);
                    return;
                }

                if (string.IsNullOrEmpty(dtTemp.Rows[0][0].ToString()))
                {
                    SajetCommon.Show_Message("Part not model", 1);
                    return;
                }

                sModelID = dtTemp.Rows[0][0].ToString();


                sSQL = $@"SELECT MODEL_NAME
  FROM SAJET.SYS_MODEL A
 WHERE A.ROUTE_ID = '{sRouteID}'
   AND A.MODEL_ID = '{sModelID}'";
                dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];
                if (dtTemp.Rows.Count == 0)
                {
                    SajetCommon.Show_Message("Route not model", 1);
                    return;
                }
                sModelName = dtTemp.Rows[0][0].ToString();
            }

            fProcessLink f = new fProcessLink();
            f.g_sModelID = sModelID;
            f.g_sModelName = sModelName;
            f.g_sRouteID = sRouteID;
            f.g_sWorkOrder = editWO.Text;
            f.ShowDialog();

        }

        //新增搜尋
        private void btnSearchRule_Click(object sender, EventArgs e)
        {
            string sMsg = "";
            string sSQL = @"Select FUNCTION_NAME WO_RULE
                 From SAJET.SYS_MODULE_PARAM
                 Where MODULE_NAME = 'W/O RULE' 
                 Group By FUNCTION_NAME
                 Order By FUNCTION_NAME";

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count < 1)
            {
                sMsg = SajetCommon.SetLanguage("material_part_search_fail", 1);
                SajetCommon.Show_Message(sMsg, 0);
                return;
            }
            //fFilter f = new fFilter();
            //f.sSQL = sSQL;
            fFilterSelect f = new fFilterSelect(dsTemp);
            if (f.ShowDialog() == DialogResult.OK)
            {
                combWoRule.Text = f.dgvData.CurrentRow.Cells["WO_RULE"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                // editPart_KeyPress(sender, Key);
                combWoRule_SelectedIndexChanged(sender, Key);
            }
        }

        private void btnSearchRoute_Click(object sender, EventArgs e)
        {
            string sMsg = "";
            string sSQL = @" SELECT ROUTE_ID, ROUTE_NAME 
                 FROM SAJET.SYS_ROUTE 
                 WHERE ENABLED = 'Y' 
                 ORDER BY ROUTE_NAME ";

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count < 1)
            {
                sMsg = SajetCommon.SetLanguage("material_part_search_fail", 1);
                SajetCommon.Show_Message(sMsg, 0);
                return;
            }

            fFilterSelect f = new fFilterSelect(dsTemp);
            //f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                combRoute.Text = f.dgvData.CurrentRow.Cells["ROUTE_NAME"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                combRoute_SelectedIndexChanged(sender, Key);
            }
        }

        private PartData GetPartData(string sPartNo, string sVersion)
        {
            PartData partData = new PartData();
            try
            {
                string sSQL = @"SELECT *
                              FROM SAJET.SYS_PART
                             WHERE PART_NO = :PART_NO
                               AND VERSION = :VERSION ";
                object[][] Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", sPartNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sVersion };
                using (DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params))
                {
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        partData.PartID = dsTemp.Tables[0].Rows[0]["PART_ID"].ToString();
                        partData.Version = sVersion;
                    }
                }
            }
            catch
            {
            }

            return partData;
        }
    }
}