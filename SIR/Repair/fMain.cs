using SajetClass;
using SajetFilter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;


namespace RepairDll
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }
        public fMain(string TerminalID)
        {
            programInfo.sRTerminalID = TerminalID;
            InitializeComponent();
        }

        struct ProgramInfo
        {
            public string sRTerminalID;
        }
        ProgramInfo programInfo;

        public int g_iPrivilege = 0;
        public string g_sUserID;
        public string g_sLoginUserID;
        public static string g_sExeName;
        string g_sProgram, g_sFunction;

        /*public string g_sRTerminalID;
        public string g_sRProcessID;
        public string g_sRStageID;
        public string g_sRPDLineID;
         */
        public string g_sSN = "";
        public string g_sRouteID;
        public string g_sPartID;
        public string g_sOutTime;
        public string g_sRouteStep;
        //public int g_iLocateItem;
        public string g_sPanel_SN; //供Pepair Panel使用
        public string g_sComfirmID; //維修確認人員
        public string g_sDefectSNID;
        string g_sRepairType;//PANEL or SN

        string sSQL;
        DataSet dsTemp;

        public void check_privilege()
        {
            btnFinish.Enabled = false;
            editSN.Enabled = btnFinish.Enabled;
            btnScrap.Enabled = false;
            editRepairer.Enabled = false;

            //                        
            g_iPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram);
            btnFinish.Enabled = (g_iPrivilege >= 1);
            editSN.Enabled = (g_iPrivilege >= 1);

            //Scrap                         
            g_iPrivilege = ClientUtils.GetPrivilege(g_sUserID, "Scrap", g_sProgram);
            btnScrap.Enabled = (g_iPrivilege >= 1);

            //Change Repairer           
            g_iPrivilege = ClientUtils.GetPrivilege(g_sUserID, "Change Repairer", g_sProgram);
            editRepairer.Enabled = (g_iPrivilege >= 1);

            g_iPrivilege = ClientUtils.GetPrivilege(g_sUserID, "ATE", g_sProgram);
            if (g_iPrivilege >= 1)
            {
                editRepairer.Enabled = (g_iPrivilege >= 1);
                btnRepair.Enabled = (g_iPrivilege >= 1);
                btnReplace.Enabled = (g_iPrivilege >= 1);
                btnRemove.Enabled = (g_iPrivilege >= 1);
                btnFinish.Enabled = (g_iPrivilege >= 1);
                btnScrap.Enabled = (g_iPrivilege >= 1);
            }
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            btnRepairHistory.BackgroundImage = SajetClass.SajetCommon.LoadImage("ImgFilter.jpg");
            btnRepairHistory.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = SajetClass.SajetCommon.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            panelFinish.BackgroundImage = SajetClass.SajetCommon.LoadImage("ImgButton.jpg");
            panelFinish.BackgroundImageLayout = ImageLayout.Stretch;

            g_sExeName = ClientUtils.fCurrentProject;
            g_sFunction = ClientUtils.fFunctionName;
            g_sProgram = ClientUtils.fProgramName;

            ClientUtils.SetLanguage(this, g_sExeName);

            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";
            btnFinish.BackColor = Color.Green;
            btnScrap.BackColor = Color.Red;

            //Employee
            g_sUserID = ClientUtils.UserPara1;
            g_sLoginUserID = g_sUserID;
            RepairUtility.sUserID = g_sUserID;
            string sEmpNo = "";
            string sEmpName = "";
            Get_EmpNo(g_sUserID, out sEmpNo, out sEmpName);
            editRepairer.Text = sEmpNo;
            LabEmpName.Text = sEmpName;

            ClearData();
            check_privilege();

            LabAlarm.Visible = false;
            btnRepairHistory.Enabled = false;
            panelFinish.Enabled = false;
            splitContainer1.Enabled = false;
            //            splitContainer2.Enabled = false;
            tabControl1.SelectedIndex = 0;
            //讀取本站Terminal
            if (!GetTerminalID())
            {
                return;
            }

            //讀取SYS_BASE設定            
            string sMsg = "";
            btnSearchSN.Visible = (SajetCommon.GetSysBaseData(g_sProgram, "Search SN", ref sMsg) == "Y"); //SN是否可用選的
            string sLoc = SajetCommon.GetSysBaseData(g_sProgram, "Location@Item Input", ref sMsg); ////維修時Location&Item是否一定要輸入
            if (!string.IsNullOrEmpty(sMsg))
            {
                sMsg = "Please Setup System Parameter:" + Environment.NewLine + Environment.NewLine + sMsg;
                ClientUtils.ShowMessage(sMsg, 0);
                //SajetCommon.Show_Message(sMsg, 0);
                return;
            }
            btnRepairHistory.Enabled = true;
            panelFinish.Enabled = true;

            splitContainer1.Enabled = true;
            //            splitContainer2.Enabled = true;

            //維修時Location&Item是否一定要輸入            
            int iLocationParams = 0;
            switch (sLoc)
            {
                case "Location":

                    iLocationParams = 1;
                    break;
                case "Item":
                    iLocationParams = 2;
                    break;
                case "One":
                    iLocationParams = 3;
                    break;
                case "Both":
                    iLocationParams = 4;
                    break;
                default:
                    iLocationParams = 0;
                    break;
            }
            RepairUtility.iLocationParams = iLocationParams;

            if (editSN.Enabled)
            {
                if (!string.IsNullOrEmpty(g_sPanel_SN))
                {
                    editSN.Text = g_sPanel_SN;
                    Show_SNData();
                }
                editSN.Focus();
            }

            //add by rita 2014/01
            g_sRepairType = "SN";
            if (!string.IsNullOrEmpty(g_sPanel_SN))
            {
                g_sRepairType = "PANEL";
                editSN.Enabled = false;
                btnSearchSN.Enabled = false;
            }
        }

        public bool GetTerminalID()
        {
            if (string.IsNullOrEmpty(programInfo.sRTerminalID))
            {
                string sIniFile = Application.StartupPath + "\\Sajet.ini";
                SajetInifile sajetInifile1 = new SajetInifile();
                programInfo.sRTerminalID = sajetInifile1.ReadIniFile(sIniFile, g_sProgram, "Terminal", "");

                if (string.IsNullOrEmpty(programInfo.sRTerminalID))
                {
                    ClientUtils.ShowMessage("Terminal not be assign", 0);
                    //  SajetCommon.Show_Message("Terminal not be assign", 0);
                    return false;
                }
            }

            sSQL = "Select A.TERMINAL_NAME,B.PROCESS_NAME "
                 + "      ,A.PDLINE_ID,A.Stage_ID,A.PROCESS_ID "
                 + " From SAJET.SYS_TERMINAL A "
                 + "     ,SAJET.SYS_PROCESS B "
                 + "Where A.TERMINAL_ID = '" + programInfo.sRTerminalID + "' "
                 + "AND A.PROCESS_ID = B.PROCESS_ID ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                ClientUtils.ShowMessage("Terminal data Error", 0);
                //SajetCommon.Show_Message("Terminal data Error", 0);
                return false;
            }
            RepairUtility.sTerminalID = programInfo.sRTerminalID;
            RepairUtility.sStageID = dsTemp.Tables[0].Rows[0]["Stage_ID"].ToString(); ;
            RepairUtility.sProcessID = dsTemp.Tables[0].Rows[0]["PROCESS_ID"].ToString();
            RepairUtility.sPDLineID = dsTemp.Tables[0].Rows[0]["PDLINE_ID"].ToString();

            this.Text = this.Text + " ("
                      + dsTemp.Tables[0].Rows[0]["PROCESS_NAME"].ToString() + " / "
                      + dsTemp.Tables[0].Rows[0]["TERMINAL_NAME"].ToString() + ")";
            return true;

        }

        private void editRepairer_KeyPress(object sender, KeyPressEventArgs e)
        {
            LabEmpName.Text = "";
            g_sUserID = "0";
            if (e.KeyChar != (char)Keys.Return)
                return;

            if (!Check_Repairer())
            {
                editRepairer.Focus();
                editRepairer.SelectAll();
                return;
            }
            RepairUtility.sUserID = g_sUserID;

            check_privilege();

            if (editSN.Enabled)
            {
                editSN.Focus();
                editSN.SelectAll();
            }
            else
            {
                ClearData();
                editSN.Text = string.Empty;
            }
        }

        private bool Check_Repairer()
        {
            //若Repairer空白,則自動帶出login user
            editRepairer.Text = editRepairer.Text.Trim();
            if (editRepairer.Text == "")
            {
                string sEmpNo = "";
                string sEmpName = "";
                g_sUserID = g_sLoginUserID;
                Get_EmpNo(g_sUserID, out sEmpNo, out sEmpName);
                editRepairer.Text = sEmpNo;
                LabEmpName.Text = sEmpName;
            }
            else
            {
                sSQL = "Select EMP_NAME,EMP_ID,ENABLED from sajet.sys_emp "
                     + "Where EMP_NO = '" + editRepairer.Text + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    ClientUtils.ShowMessage("Employee (" + editRepairer.Text + ") Error", 0);
                    //SajetCommon.Show_Message("Employee (" + editRepairer.Text + ") Error", 0);
                    return false;
                }
                else if (dsTemp.Tables[0].Rows[0]["ENABLED"].ToString() != "Y")
                {
                    ClientUtils.ShowMessage("Employee (" + editRepairer.Text + ") Disabled", 0);
                    //SajetCommon.Show_Message("Employee (" + editRepairer.Text + ") Disabled", 0);
                    return false;
                }
                g_sUserID = dsTemp.Tables[0].Rows[0]["EMP_ID"].ToString();
                LabEmpName.Text = dsTemp.Tables[0].Rows[0]["EMP_NAME"].ToString();
            }
            return true;
        }

        public void Get_EmpNo(string sEmpID, out string sEmpNo, out string sEmpName)
        {
            sSQL = "Select EMP_NO,EMP_NAME from sajet.sys_emp "
                 + "where EMP_ID = '" + sEmpID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
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

        private void btnSNSearch_Click(object sender, EventArgs e)
        {
            sSQL = " Select a.work_order,a.Serial_Number "
                  + " From SAJET.G_SN_STATUS a "
                  + "     ,sajet.sys_route_detail b "
                  + "     ,sajet.g_wo_base c "
                  + " Where a.CURRENT_STATUS = '1' "
                  + " and a.WORK_FLAG = '0' "
                  + " and b.next_process_id = " + RepairUtility.sProcessID
                  + " and c.wo_status in ('2','3') "
                  + " and a.process_id = b.process_id "
                  + " and a.route_id = b.route_id "
                  + " and a.work_order = c.work_order "
                  + " Order By a.work_order,a.Serial_Number ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                ClearData();
                editSN.Text = f.dgvData.CurrentRow.Cells["SERIAL_NUMBER"].Value.ToString();
                Show_SNData();
            }
            f.Dispose();
        }

        private void editSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClearData();
            if (e.KeyChar != (char)Keys.Return)
                return;
            Show_SNData();
        }
        private void Show_KP()
        {
            dgvKP.Rows.Clear();
            sSQL = " Select A.SERIAL_NUMBER, A.work_order, A.ITEM_PART_ID,A.Item_Group, A.Process_Id "
                 + "       ,B.PART_NO ,ITEM_PART_SN ,B.SPEC1 "
                 + "       ,C.PROCESS_NAME ,D.EMP_NAME "
                 + "       ,TO_CHAR(A.UPDATE_TIME,'YYYY/MM/DD HH24:MI:SS') UPDATE_TIME "
                 + " From SAJET.G_SN_KEYPARTS A "
                 + "     ,SAJET.SYS_PART B "
                 + "     ,SAJET.SYS_PROCESS C "
                 + "     ,SAJET.SYS_EMP D "
                 + " Where A.SERIAL_NUMBER = :SERIAL_NUMBER "
                 + " and A.ITEM_PART_ID = B.PART_ID(+) "
                 + " AND A.PROCESS_ID = C.PROCESS_ID(+) "
                 + " AND A.UPDATE_USERID = D.EMP_ID(+) "
                 + " Order By A.UPDATE_TIME ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", g_sSN };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvKP.Rows.Add();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["SERIAL_NUMBER"].Value = dr["SERIAL_NUMBER"].ToString();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["WORK_ORDER"].Value = dr["WORK_ORDER"].ToString();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["ITEM_PART_SN"].Value = dr["ITEM_PART_SN"].ToString();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["ITEM_PART_ID"].Value = dr["ITEM_PART_ID"].ToString();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["ITEM_PART_NO"].Value = dr["PART_NO"].ToString();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["ASSY_EMP"].Value = dr["EMP_NAME"].ToString();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["ASSY_PROCESS"].Value = dr["PROCESS_NAME"].ToString();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["ASSY_TIME"].Value = dr["UPDATE_TIME"].ToString();
                int iDefectCount = GetSNDefectCount(dr["ITEM_PART_SN"].ToString());
                if (iDefectCount > 0)
                    dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["ITEM_PART_SN"].Style.BackColor = Color.Yellow;
            }
        }
        private int GetSNDefectCount(string sSN)
        {
            string sSQL = " Select COUNT(A.SERIAL_NUMBER) DEFECT_COUNT "
                + " From SAJET.G_SN_DEFECT_HEADER A "
                + " Where A.SERIAL_NUMBER = :SERIAL_NUMBER ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", sSN };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            return Convert.ToInt32(dsTemp.Tables[0].Rows[0]["DEFECT_COUNT"].ToString());
        }
        public void Show_SNData()
        {
            LabAlarm.Visible = false;

            //檢查SN是否正確
            if (!CheckSN())
            {
                editSN.Focus();
                editSN.SelectAll();
                g_sSN = "";
                return;
            }
            //顯示Defect Data
            ShowDefect();

            if (LVDefect.Items.Count > 0)
            {
                LVDefect.Focus();
                LVDefect.Items[0].Selected = true;
                ShowReason(LVDefect.Items[0].SubItems[3].Text);
            }

            ShowReplace();
            ShowRepairHistory();
            ShowItemReplace();
            Show_KP();

            //check Marry
            sSQL = @" SELECT COUNT(*) FROM SAJET.G_SN_TRAVEL
                       WHERE PROCESS_ID IN (SELECT PROCESS_ID FROM SAJET.SYS_PROCESS WHERE MARRY='Y')
                         AND SERIAL_NUMBER = '" + g_sSN + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL, null);
            if (dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
            {
                if (dsTemp.Tables[0].Rows[0][0].ToString() != "0")
                {
                    LabAlarm.Text = SajetCommon.SetLanguage("Serial Number has been Marry!");
                    LabAlarm.Visible = true;
                }
            }
            panel2.Enabled = true;
            panel1.Enabled = true;
            LVDefect.Enabled = true;

            sSQL = $@"SELECT T.PARAM_VALUE
  FROM SAJET.SYS_BASE T
 WHERE PROGRAM = 'Repair'
   AND PARAM_NAME = 'Scrap EC'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                var sDefect = dsTemp.Tables[0].Rows[0][0].ToString().Split(',');
                sSQL = $@"SELECT COUNT(1)
  FROM SAJET.G_SN_DEFECT
 WHERE PROCESS_ID = '{RepairUtility.sPreviousProcessID}'
   AND DEFECT_ID IN
       (SELECT DEFECT_ID
          FROM SAJET.SYS_DEFECT
         WHERE DEFECT_CODE IN ('{string.Join("', '", sDefect)}')
           AND DEFECT_ID IN ('{string.Join("', '", RepairUtility.sDefectIDList)}'))
   AND SERIAL_NUMBER = '{g_sSN}'
   --AND RP_STATUS = '0'
 --GROUP BY DEFECT_ID";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                if (int.Parse(dsTemp.Tables[0].Rows[0][0].ToString()) >= 3)
                {
                    panel2.Enabled = false;
                    panel1.Enabled = false;
                    LVDefect.Enabled = false;

                    //SajetCommon.Show_Message("The same type of defect is judged more than 3 times", 3);
                    btnScrap_Click(null, null);
                }
            }


        }

        public bool CheckSN()
        {
            //Check SN=========================================================
            try
            {
                object[][] Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREV", editSN.Text };
                Params[1] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "PSN", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_CKRT_SN_PSN", Params);

                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    ClientUtils.ShowMessage(sRes, 0);
                    //SajetCommon.Show_Message(sRes, 0);
                    editSN.SelectAll();
                    return false;
                }
                g_sSN = ds.Tables[0].Rows[0]["PSN"].ToString();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowMessage(ex.Message, 0);
                //SajetCommon.Show_Message(ex.Message, 0);
                editSN.SelectAll();
                return false;
            }

            //Check Route===================================================
            try
            {
                object[][] Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TERMINALID", RepairUtility.sTerminalID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", g_sSN };
                Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_CKRT_ROUTE", Params);

                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    ClientUtils.ShowMessage(sRes, 0);
                    // SajetCommon.Show_Message(sRes, 0);
                    editSN.SelectAll();
                    return false;
                }
            }
            catch (Exception ex)
            {
                ClientUtils.ShowMessage(ex.Message, 0);
                //SajetCommon.Show_Message(ex.Message, 0);
                editSN.SelectAll();
                return false;
            }

            //此SN的紀錄
            sSQL = " Select A.PROCESS_ID,A.WORK_ORDER,A.PART_ID, to_char(A.OUT_PROCESS_TIME,'yyyy/mm/dd hh24:mi:ss') OUT_PROCESS_TIME, a.route_id ,d.remark "
                 + "       ,B.PART_NO,c.process_name,NVL(A.PANEL_NO,'N/A') PANEL_NO "
                 + " From SAJET.G_SN_STATUS A "
                 + "     ,SAJET.SYS_PART B "
                 + "     ,sajet.sys_process c "
                 + "     ,sajet.g_wo_base d "
                 + " Where A.SERIAL_NUMBER = '" + g_sSN + "' "
                 + " and A.PART_ID = B.PART_ID "
                 + " and a.process_id = c.process_id "
                 + " and a.work_order = d.work_order "
                 + " and rownum = 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows[0]["PANEL_NO"].ToString() != "N/A" && g_sRepairType == "SN")
            {
                ClientUtils.ShowMessage("Please use [Repair for Panel] Function", 0);
                //SajetCommon.Show_Message("Please use [Repair for Panel] Function", 0);
                return false;
            }

            LabWO.Text = dsTemp.Tables[0].Rows[0]["WORK_ORDER"].ToString();
            LabPart.Text = dsTemp.Tables[0].Rows[0]["PART_NO"].ToString();
            g_sPartID = dsTemp.Tables[0].Rows[0]["PART_ID"].ToString();
            g_sRouteID = dsTemp.Tables[0].Rows[0]["ROUTE_ID"].ToString();
            g_sOutTime = dsTemp.Tables[0].Rows[0]["OUT_PROCESS_TIME"].ToString();
            LabRemark.Text = dsTemp.Tables[0].Rows[0]["REMARK"].ToString();
            RepairUtility.sPreviousProcessID = dsTemp.Tables[0].Rows[0]["PROCESS_ID"].ToString();

            //找Route中的Step,Finish時找回流站使用
            sSQL = " Select Step "
                 + " From sajet.sys_route_detail "
                 + " Where route_id = '" + g_sRouteID + "' "
                 + " and process_id = '" + RepairUtility.sPreviousProcessID + "' "
                 + " and next_process_id = '" + RepairUtility.sProcessID + "'"
                 + " and rownum = 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                ClientUtils.ShowMessage("Route Step Error", 0);
                //  SajetCommon.Show_Message("Route Step Error", 0);
                return false;
            }
            g_sRouteStep = dsTemp.Tables[0].Rows[0]["Step"].ToString();
            return true;
        }

        public void ShowDefect()
        {
            LVDefect.Items.Clear();
            sSQL = " Select A.DEFECT_SN_ID,A.RECID,A.Location,A.RP_STATUS,A.Process_Id,A.DEFECT_ID "
                 + "       ,B.DEFECT_CODE,B.DEFECT_DESC  "
                 + "       ,C.PDLINE_NAME,D.TERMINAL_NAME,E.PROCESS_NAME,NVL(G.REASON_CODE,'N/A') REASON_CODE "
                 + " From SAJET.G_SN_DEFECT A "
                 + " ,SAJET.SYS_DEFECT B "
                 + " ,SAJET.SYS_PDLINE C "
                 + " ,SAJET.SYS_TERMINAL D "
                 + " ,SAJET.SYS_PROCESS E "
                 + " ,SAJET.G_SN_REPAIR F "
                 + " ,SAJET.SYS_REASON G "
                 + " Where A.Serial_Number = '" + g_sSN + "' "
                 + " and a.rec_time >= to_date('" + g_sOutTime + "','yyyy/mm/dd hh24:mi:ss') "
                 + " and A.DEFECT_ID = B.DEFECT_ID(+) "
                 + " and A.PDLINE_ID = C.PDLINE_ID(+) "
                 + " and A.TERMINAL_ID = D.TERMINAL_ID(+) "
                 + " and A.PROCESS_ID = E.PROCESS_ID(+) "
                 + " and A.RECID = F.RECID(+) "
                 + " and F.REASON_ID = G.REASON_ID(+) "
                 + " Order By B.Defect_Code ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                return;
            }
            LabDefectLine.Text = dsTemp.Tables[0].Rows[0]["PDLINE_NAME"].ToString();
            LabDefectProcess.Text = dsTemp.Tables[0].Rows[0]["PROCESS_NAME"].ToString();
            LabDefectTerminal.Text = dsTemp.Tables[0].Rows[0]["TERMINAL_NAME"].ToString();
            g_sDefectSNID = dsTemp.Tables[0].Rows[0]["DEFECT_SN_ID"].ToString();
            RepairUtility.sDefectSNID = g_sDefectSNID;
            if (RepairUtility.sDefectIDList == null)
            {
                RepairUtility.sDefectIDList = new List<string>();
            }
            RepairUtility.sDefectIDList.Clear();
            string S = "";
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                string sDefectCode = dsTemp.Tables[0].Rows[i]["DEFECT_CODE"].ToString();
                if (S != sDefectCode)
                {
                    LVDefect.Items.Add(sDefectCode);
                    LVDefect.Items[LVDefect.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["DEFECT_DESC"].ToString());
                    //LVDefect.Items[LVDefect.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["DEFECT_DESC2"].ToString());
                    LVDefect.Items[LVDefect.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["LOCATION"].ToString());
                    LVDefect.Items[LVDefect.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["RECID"].ToString());
                    LVDefect.Items[LVDefect.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["PROCESS_ID"].ToString());
                    if (dsTemp.Tables[0].Rows[i]["REASON_CODE"].ToString() == "N/A")
                        LVDefect.Items[LVDefect.Items.Count - 1].ImageIndex = 1;
                    else
                        LVDefect.Items[LVDefect.Items.Count - 1].ImageIndex = 0;
                    S = dsTemp.Tables[0].Rows[i]["DEFECT_CODE"].ToString();
                    RepairUtility.sDefectIDList.Add(dsTemp.Tables[0].Rows[i]["DEFECT_ID"].ToString());
                }
            }
        }

        public void ShowReason(string sRECID)
        {
            dgvReason.Rows.Clear();
            dgvRepairData.Rows.Clear();
            sSQL = "SELECT  C.REASON_CODE,C.REASON_DESC"
                + "        ,D.DUTY_CODE,D.DUTY_DESC "
                + "        ,B.REASON_ID,B.DUTY_ID "
                + " FROM SAJET.G_SN_DEFECT A,SAJET.G_SN_REPAIR B  "
                + "     ,SAJET.SYS_REASON C ,SAJET.SYS_DUTY D "
                + " WHERE A.RECID =:RECID "
                + "  AND A.RECID = B.RECID "
                + "  AND B.REASON_ID = C.REASON_ID "
                + "  AND B.DUTY_ID = D.DUTY_ID "
                + " ORDER BY C.REASON_CODE ";

            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", sRECID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvReason.Rows.Add();
                dgvReason.Rows[dgvReason.Rows.Count - 1].Cells["REASON_CODE_1"].Value = dr["REASON_CODE"].ToString();
                dgvReason.Rows[dgvReason.Rows.Count - 1].Cells["REASON_DESC_1"].Value = dr["REASON_DESC"].ToString();
                // dgvReason.Rows[dgvReason.Rows.Count - 1].Cells["REASON_DESC_2"].Value = dr["REASON_DESC2"].ToString();
                dgvReason.Rows[dgvReason.Rows.Count - 1].Cells["DUTY_CODE"].Value = dr["DUTY_CODE"].ToString();
                dgvReason.Rows[dgvReason.Rows.Count - 1].Cells["DUTY_DESC_1"].Value = dr["DUTY_DESC"].ToString();
                // dgvReason.Rows[dgvReason.Rows.Count - 1].Cells["DUTY_DESC_2"].Value = dr["DUTY_DESC2"].ToString();
            }

            sSQL = "SELECT A.LOCATION,B.PART_NO ITEM_NO,A.IS_MAIN_DEFECT "
                       + "      ,C.REASON_CODE,C.REASON_DESC "
                       + "      ,D.repair_code,D.repair_type,D.repair_desc"
                       + " FROM SAJET.G_SN_REPAIR_LOCATION A "
                       + "      ,SAJET.SYS_PART B "
                       + "      ,SAJET.SYS_REASON C "
                       + "      ,SAJET.SYS_REPAIR_CODE D "
                       + " WHERE A.RECID =:RECID "
                       + "  AND A.ITEM_ID = B.PART_ID(+) "
                       + "  AND A.REASON_ID = C.REASON_ID(+) "
                       + "  AND A.REPAIR_ID = D.REPAIR_ID(+) "
                       + " ORDER BY A.UPDATE_TIME ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvRepairData.Rows.Add();
                dgvRepairData.Rows[dgvRepairData.Rows.Count - 1].Cells["REASON_CODE"].Value = dr["REASON_CODE"].ToString();
                dgvRepairData.Rows[dgvRepairData.Rows.Count - 1].Cells["REASON_DESC"].Value = dr["REASON_DESC"].ToString();
                //dgvRepairData.Rows[dgvRepairData.Rows.Count - 1].Cells["REASON_DESC"].Value = dr["REASON_DESC2"].ToString();
                dgvRepairData.Rows[dgvRepairData.Rows.Count - 1].Cells["LOCATION"].Value = dr["LOCATION"].ToString();
                dgvRepairData.Rows[dgvRepairData.Rows.Count - 1].Cells["ITEM_NO"].Value = dr["ITEM_NO"].ToString();
                dgvRepairData.Rows[dgvRepairData.Rows.Count - 1].Cells["IS_MAIN_DEFECT"].Value = dr["IS_MAIN_DEFECT"].ToString();
                dgvRepairData.Rows[dgvRepairData.Rows.Count - 1].Cells["REPAIR_TYPE"].Value = dr["REPAIR_TYPE"].ToString();
                dgvRepairData.Rows[dgvRepairData.Rows.Count - 1].Cells["REPAIR_CODE"].Value = dr["REPAIR_CODE"].ToString();
                dgvRepairData.Rows[dgvRepairData.Rows.Count - 1].Cells["REPAIR_DESC"].Value = dr["REPAIR_DESC"].ToString();
                // dgvRepairData.Rows[dgvRepairData.Rows.Count - 1].Cells["REPAIR_DESC2"].Value = dr["REPAIR_DESC2"].ToString();
            }



            LVReason.Items.Clear();
            sSQL = " Select  c.recid,c.reason_id from SAJET.G_SN_DEFECT A,SAJET.SYS_DEFECT B,SAJET.G_SN_REPAIR C "
                 + " WHERE A.Serial_Number = '" + g_sSN + "' "
                 + " and A.RECID = '" + sRECID + "' "
                 + " and A.DEFECT_ID = B.DEFECT_ID and A.RECID = C.RECID "
                 + " group by  c.recid,c.reason_id ";
            sSQL = " select c.reason_code,c.reason_desc "
                 + " ,b.location,b.item_no,b.is_main_defect "
                 + " ,d.repair_code,d.repair_type,d.repair_desc"
                 + " from (" + sSQL + ") a"
                 + " ,sajet.g_sn_repair_location b,sajet.sys_reason c,sajet.sys_repair_code d "
                 + " where a.recid = b.recid(+) "
                 + " and a.reason_id = b.reason_id(+) "
                 + " and a.reason_id = c.reason_id "
                 + " and b.repair_id = d.repair_id(+) "
                 + " order by c.reason_code,c.reason_desc,b.location,b.item_no ";

            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            string S = "";
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                string sReasonCode = dsTemp.Tables[0].Rows[i]["REASON_CODE"].ToString();
                if (S != sReasonCode)
                {
                    LVReason.Items.Add(dsTemp.Tables[0].Rows[i]["REASON_CODE"].ToString());
                    LVReason.Items[LVReason.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["REASON_DESC"].ToString());
                    // LVReason.Items[LVReason.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["REASON_DESC2"].ToString());
                    LVReason.Items[LVReason.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["LOCATION"].ToString());
                    LVReason.Items[LVReason.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["ITEM_NO"].ToString());
                    LVReason.Items[LVReason.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["IS_MAIN_DEFECT"].ToString());
                    LVReason.Items[LVReason.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["REPAIR_CODE"].ToString());
                    LVReason.Items[LVReason.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["REPAIR_TYPE"].ToString());
                    LVReason.Items[LVReason.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["REPAIR_DESC"].ToString());
                    // LVReason.Items[LVReason.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["REPAIR_DESC2"].ToString());
                }
                else
                {
                    LVReason.Items.Add("");
                    LVReason.Items[LVReason.Items.Count - 1].SubItems.Add("");
                    LVReason.Items[LVReason.Items.Count - 1].SubItems.Add("");
                    LVReason.Items[LVReason.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["LOCATION"].ToString());
                    LVReason.Items[LVReason.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["ITEM_NO"].ToString());
                    LVReason.Items[LVReason.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["IS_MAIN_DEFECT"].ToString());
                    LVReason.Items[LVReason.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["REPAIR_CODE"].ToString());
                    LVReason.Items[LVReason.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["REPAIR_TYPE"].ToString());
                    LVReason.Items[LVReason.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["REPAIR_DESC"].ToString());
                    // LVReason.Items[LVReason.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["REPAIR_DESC2"].ToString());
                }
                S = dsTemp.Tables[0].Rows[i]["REASON_CODE"].ToString();
            }
        }

        public void ClearData()
        {
            LVDefect.Items.Clear();
            LVReason.Items.Clear();
            LabWO.Text = "";
            LabDefectLine.Text = "";
            LabDefectProcess.Text = "";
            LabDefectTerminal.Text = "";
            LabPart.Text = "";
            LabRemark.Text = "";
            g_sSN = "";

            LVReplaceHistory.Items.Clear();
            LVRepairHistory.Items.Clear();
            lvItemReplace.Items.Clear();
            dgvKP.Rows.Clear();
            dgvReason.Rows.Clear();
            dgvRepairData.Rows.Clear();
        }

        private void LVDefect_Click(object sender, EventArgs e)
        {
            if (LVDefect.SelectedItems.Count == 0)
                return;
            ShowReason(LVDefect.SelectedItems[0].SubItems[3].Text);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (g_sSN == "")
                return;
            if (LVDefect.SelectedItems.Count == 0)
                return;

            //只可刪除本站增加的Defect
            if (LVDefect.SelectedItems[0].SubItems[4].Text != RepairUtility.sProcessID)
            {
                ClientUtils.ShowMessage("Can't Delete this Defect Code", 1);
                // SajetCommon.Show_Message("Can't Delete this Defect Code", 1);
                return;
            }

            string sDefectCode = LVDefect.SelectedItems[0].Text;
            string sRECID = LVDefect.SelectedItems[0].SubItems[3].Text;
            if (ClientUtils.ShowMessage(SajetCommon.SetLanguage("Delete Defect Code ?") + Environment.NewLine
                                       + SajetCommon.SetLanguage("Defect Code") + " : " + sDefectCode, 2) != DialogResult.Yes)
                return;

            sSQL = " Delete SAJET.G_SN_REPAIR_REMARK "
                 + " Where RECID = '" + sRECID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            sSQL = " Delete SAJET.G_SN_REPAIR "
                 + " Where RECID = '" + sRECID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            sSQL = " Delete SAJET.G_SN_DEFECT "
                 + " Where RECID = '" + sRECID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            LVDefect.SelectedItems[0].Remove();
            LVReason.Items.Clear();
            if (LVDefect.Items.Count > 0)
            {
                LVDefect.Items[LVDefect.Items.Count - 1].Selected = true;
                LVDefect.Focus();
                ShowReason(LVDefect.SelectedItems[0].SubItems[3].Text);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (g_sSN == "")
                return;

            fAddDefect f = new fAddDefect();
            if (f.ShowDialog() != DialogResult.OK)
                return;

            string sDefectCode = f.editDefect.Text;
            string sLocation = f.editLocation.Text;
            f.Dispose();

            sSQL = " Select Defect_ID,Defect_Desc from sajet.sys_defect "
                 + " where Defect_Code = '" + sDefectCode + "'"
                 + " and enabled = 'Y' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                ClientUtils.ShowMessage(SajetCommon.SetLanguage("Defect Code Error") + Environment.NewLine + Environment.NewLine + SajetCommon.SetLanguage("Defect Code") + " : " + sDefectCode, 0);
                return;
            }

            if (LVDefect.Items.Count > 0)
            {
                if (LVDefect.FindItemWithText(sDefectCode, false, 0) != null)
                {
                    ClientUtils.ShowMessage(SajetCommon.SetLanguage("Defect Code Duplicate") + Environment.NewLine + Environment.NewLine + SajetCommon.SetLanguage("Defect Code") + " : " + sDefectCode, 0);
                    return;
                }
            }

            string sDefectDesc = dsTemp.Tables[0].Rows[0]["Defect_Desc"].ToString();
            string sDefectID = dsTemp.Tables[0].Rows[0]["Defect_ID"].ToString();
            //  string sDefectDesc2 = dsTemp.Tables[0].Rows[0]["Defect_Desc2"].ToString();
            string sRecID = GetDefectRECID();
            if (sRecID == "0")
            {
                ClientUtils.ShowMessage("Get Defect RECID Error", 0);
                return;
            }

            sSQL = " Insert Into SAJET.G_SN_DEFECT "
                 + " (DEFECT_SN_ID,RECID,SERIAL_NUMBER,WORK_ORDER,PART_ID,DEFECT_ID "
                 + " ,TERMINAL_ID,PROCESS_ID,STAGE_ID,PDLINE_ID,TEST_EMP_ID,RP_STATUS,LOCATION) "
                 + " Select  '" + g_sDefectSNID + "','" + sRecID + "','" + g_sSN + "','" + LabWO.Text + "','" + g_sPartID + "','" + sDefectID + "'"
                 + " ,TERMINAL_ID,PROCESS_ID,STAGE_ID,PDLINE_ID,'" + g_sUserID + "','1','" + sLocation + "' "
                 + " From SAJET.SYS_TERMINAL "
                 + " Where TERMINAL_ID = '" + RepairUtility.sTerminalID + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            LVDefect.Items.Add(sDefectCode);
            LVDefect.Items[LVDefect.Items.Count - 1].SubItems.Add(sDefectDesc);
            //  LVDefect.Items[LVDefect.Items.Count - 1].SubItems.Add(sDefectDesc2);
            LVDefect.Items[LVDefect.Items.Count - 1].SubItems.Add(sLocation);
            LVDefect.Items[LVDefect.Items.Count - 1].SubItems.Add(sRecID);
            LVDefect.Items[LVDefect.Items.Count - 1].SubItems.Add(RepairUtility.sProcessID);
            LVDefect.Items[LVDefect.Items.Count - 1].ImageIndex = 1;
            LVDefect.Items[LVDefect.Items.Count - 1].Selected = true;
            LVDefect.Focus();
            ShowReason(LVDefect.SelectedItems[0].SubItems[3].Text);

        }

        private string GetDefectRECID()
        {
            string sID = "0";
            sSQL = "Select RPAD(NVL(PARAM_VALUE,'1'),2,'0') || TO_CHAR(SYSDATE,'YYMMDD') || LPAD(SAJET.S_DEF_CODE.NEXTVAL,5,'0') SNID "
                 + "From SAJET.SYS_BASE "
                 + "Where PARAM_NAME = 'DBID' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            sID = dsTemp.Tables[0].Rows[0]["SNID"].ToString();
            return sID;
        }

        private void btnRepair_Click(object sender, EventArgs e)
        {
            if (g_sSN == "")
                return;
            if (LVDefect.SelectedItems.Count == 0)
                return;

            RepairUtility.sDefectSN = g_sSN;
            RepairUtility.sDefectSNPartID = g_sPartID;
            RepairUtility.sDefectSNWO = LabWO.Text;
            RepairUtility.sProgram = g_sProgram;
            RepairUtility.sRepairSN = g_sSN;
            RepairUtility.sRepairType = "SERIAL NUMBER";
            RepairUtility.sDefectRecID = LVDefect.SelectedItems[0].SubItems[3].Text;
            RepairUtility.sDefectCode = LVDefect.SelectedItems[0].Text;
            RepairUtility.sDefectLoc = LVDefect.SelectedItems[0].SubItems[2].Text;
            RepairUtility.sRepairSN = g_sSN;
            RepairUtility.sRepairSNPartID = g_sPartID;
            RepairUtility.sRepairSNWO = LabWO.Text;

            fRepairData fRepair = new fRepairData();
            try
            {
                //fRepair.g_sDefectRecID = LVDefect.SelectedItems[0].SubItems[4].Text;
                //fRepair.LabDefCode.Text = LVDefect.SelectedItems[0].Text;
                //fRepair.LabDefDesc.Text = LVDefect.SelectedItems[0].SubItems[1].Text;
                // fRepair.editLC.Text = LVDefect.SelectedItems[0].SubItems[3].Text;
                // if (LVDefect.SelectedItems[0].SubItems[3].Text == "N/A" || LVDefect.SelectedItems[0].SubItems[3].Text == "NA")
                //     fRepair.editErrorPoint.Text = "0";                                                   
                //   fRepair.LabSN.Text = g_sSN;
                //fRepair.LabReasonDesc.Text = "";
                // fRepair.LabDutyDesc.Text = "";
                // fRepair.g_sSN = g_sSN;
                // fRepair.g_sWO = LabWO.Text;
                //fRepair.g_iLocateItem = g_iLocateItem;
                //  fRepair.g_sUserID = g_sUserID;
                //  fRepair.g_sPartID = g_sPartID;
                /*
                fRepair.g_sRTerminalID = g_sRTerminalID;
                fRepair.g_sRProcessID = g_sRProcessID;
                fRepair.g_sRStageID = g_sRStageID;
                fRepair.g_sRPDLineID = g_sRPDLineID;
                fRepair.g_sProgram = g_sProgram;
                 */
                if (fRepair.ShowDialog() == DialogResult.OK)
                {
                    LVDefect.SelectedItems[0].ImageIndex = 0;
                    ShowReason(LVDefect.SelectedItems[0].SubItems[3].Text);
                    ShowItemReplace();
                }
            }
            finally
            {
                fRepair.Dispose();
            }
        }
        private bool CheckBGAReplaceCount()
        {
            try
            {
                object[][] Params = new object[4][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", g_sSN };
                Params[1] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TLOCATION", "" };
                Params[2] = new object[] { ParameterDirection.Output, OracleType.Number, "TQTY", 0 };
                Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SP_GET_BGA_REPLACE_COUNT", Params);
                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                string sLoc = ds.Tables[0].Rows[0]["TLOCATION"].ToString();
                int iQty = Convert.ToInt32(ds.Tables[0].Rows[0]["TQTY"].ToString());
                if (sRes == "NG")
                {

                    ClientUtils.ShowMessage(SajetCommon.SetLanguage("Location", 1) + " : " + sLoc + "     "
                                           + SajetCommon.SetLanguage("Replace Times", 1) + " : " + iQty.ToString() + Environment.NewLine + Environment.NewLine
                                           + SajetCommon.SetLanguage("BGA Item Replaced Over 3 Times", 1) + Environment.NewLine + Environment.NewLine
                                           + SajetCommon.SetLanguage("Serial Number must be scrapped", 1), 0);
                    return false;
                }
                if (sRes != "OK")
                {
                    ClientUtils.ShowMessage(sRes, 0);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ClientUtils.ShowMessage(ex.Message, 0);
                return false;
            }
        }
        private void btnFinish_Click(object sender, EventArgs e)
        {
            if (g_sSN == "")
                return;
            if (g_sUserID == "0")
            {
                if (!Check_Repairer())
                    return;
            }

            //是否所有Defect都已修完
            for (int i = 0; i <= LVDefect.Items.Count - 1; i++)
            {
                if (LVDefect.Items[i].ImageIndex != 0)
                {
                    ClientUtils.ShowMessage("Repair not Complete", 0);
                    return;
                }
            }
            if (!CheckBGAReplaceCount())
                return;
            string sMsg = "";
            string sComfirmEmp = (SajetCommon.GetSysBaseData(g_sProgram, "Confirm Employee", ref sMsg));

            //找回流站
            string sReturn_ProcessID = "0";
            string sReturnProcessName = String.Empty;
            sSQL = " select  b.process_name,a.next_process_id "
                 + " from sajet.sys_route_detail a "
                 + "    , sajet.sys_process b "
                 + " Where a.route_id = '" + g_sRouteID + "' "
                 + " and a.process_id = '" + RepairUtility.sProcessID + "' "
                 + " and a.step = '" + g_sRouteStep + "' "
                 + " and a.next_process_id = b.process_id "
                 + " order by b.process_name ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                ClientUtils.ShowMessage("No Define Return Process", 0);
                return;
            }
            else if (dsTemp.Tables[0].Rows.Count == 1 && (sComfirmEmp == "0" || sComfirmEmp == ""))
            {
                sReturn_ProcessID = dsTemp.Tables[0].Rows[0]["NEXT_PROCESS_ID"].ToString();
                sReturnProcessName = dsTemp.Tables[0].Rows[0]["PROCESS_NAME"].ToString();
                g_sComfirmID = g_sUserID;
            }
            else
            {
                //fFilter f = new fFilter();
                fCheck f = new fCheck(g_sProgram);
                try
                {
                    f.sSQL = sSQL;
                    f.dgvData.DataSource = dsTemp;
                    f.dgvData.DataMember = dsTemp.Tables[0].ToString();
                    f.dgvData.Columns["next_process_id"].Visible = false;

                    f.sSN = g_sSN;
                    f.sUserID = g_sUserID;

                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        sReturn_ProcessID = f.dgvData.CurrentRow.Cells["next_process_id"].Value.ToString();
                        sReturnProcessName = f.dgvData.CurrentRow.Cells["process_name"].Value.ToString();
                        g_sComfirmID = f.sComfirmID;
                    }
                    else
                    {
                        return;
                    }
                }
                finally
                {
                    f.Dispose();
                }
            }

            // 過站紀錄
            //====SAJET.SJ_REPAIR_GO                         
            try
            {
                object[][] Params = new object[6][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTERMINALID", RepairUtility.sTerminalID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", g_sSN };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", g_sUserID };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NPROCESSID", sReturn_ProcessID };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TCOMFIRMID", g_sComfirmID };
                Params[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_REPAIR_GO", Params);

                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();

                if (sRes != "OK")
                {
                    ClientUtils.ShowMessage(sRes, 0);
                    return;
                }
                if (!string.IsNullOrEmpty(sReturnProcessName))
                {
                    ClientUtils.ShowMessage(SajetCommon.SetLanguage("Serial Number") + " : " + g_sSN + Environment.NewLine + Environment.NewLine
                                           + SajetCommon.SetLanguage("Repair Finish") + Environment.NewLine + Environment.NewLine
                                           + SajetCommon.SetLanguage("Next Process Is") + " : " + sReturnProcessName, 3);
                }
            }
            catch (Exception ex)
            {
                ClientUtils.ShowMessage(ex.Message, 0);
                return;
            }

            ClearData();
            editSN.Focus();
            editSN.SelectAll();

            if (!string.IsNullOrEmpty(g_sPanel_SN))
                DialogResult = DialogResult.OK;
        }

        private void btnScrap_Click(object sender, EventArgs e)
        {
            if (g_sSN == "")
                return;
            if (ClientUtils.ShowMessage(SajetCommon.SetLanguage("Scrap SN", 1) + " : " + g_sSN + " ?", 2) != DialogResult.Yes)
                return;

            fScrap fData = new fScrap();
            string sScrapMemo = string.Empty;
            string sScrapType = "1";
            try
            {
                fData.g_sSN = g_sSN;
                fData.g_sScrapType = sScrapType;
                if (fData.ShowDialog() != DialogResult.OK)
                    return;
                sScrapMemo = fData.g_sMemo;
                sScrapType = fData.g_sScrapType;
            }
            finally
            {
                fData.Dispose();
            }


            //====SAJET.SJ_REPAIR_SCRAP  
            try
            {
                object[][] Params = new object[7][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTERMINALID", RepairUtility.sTerminalID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", g_sSN };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TWO", LabWO.Text };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", g_sUserID };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSCRAPMEMO", sScrapMemo };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSCRAPTYPE", sScrapType };
                Params[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_REPAIR_SCRAP", Params);

                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    ClientUtils.ShowMessage(sRes, 0);
                    return;
                }
                ClearData();
                editSN.Focus();
                editSN.SelectAll();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowMessage(ex.Message, 0);
                return;
            }

            if (!string.IsNullOrEmpty(g_sPanel_SN))
                DialogResult = DialogResult.OK;
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (g_sSN == "")
                return;
            if (LVDefect.SelectedItems.Count == 0)
            {
                ClientUtils.ShowMessage(SajetCommon.SetLanguage("Please Select Defect"), 0);
                return;
            }
            if (dgvKP.Rows.Count == 0 || dgvKP.CurrentRow == null)
            {
                ClientUtils.ShowMessage(SajetCommon.SetLanguage("Please Select Keypart"), 0);
                return;
            }

            string sKPSN = dgvKP.CurrentRow.Cells["ITEM_PART_SN"].Value.ToString();
            string sKPPartID = dgvKP.CurrentRow.Cells["ITEM_PART_ID"].Value.ToString();

            fReplace fR = new fReplace(sKPSN);
            try
            {
                //  fR.g_sUserID = g_sUserID;                
                fR.g_sTerminalID = programInfo.sRTerminalID;
                fR.g_sDefectRECID = LVDefect.SelectedItems[0].SubItems[3].Text;
                fR.LabSN.Text = g_sSN;
                if (sender == btnReplace)
                    fR.g_sType = "REPLACE";
                else if (sender == btnRemove)
                    fR.g_sType = "REMOVE";
                if (fR.ShowDialog() == DialogResult.OK)
                {
                    Show_KP();
                    ShowReplace();
                }
            }
            finally
            {
                fR.Dispose();
            }
        }

        public void ShowReplace()
        {
            DataSet DsReplace;
            LVReplaceHistory.Items.Clear();
            if (chkbAll.Checked)
            {
                //SN所有Replace紀錄(曾經屬於此SN的Keyparts都算)
                sSQL = "Select c.PART_NO , b.old_part_sn , b.new_part_sn,b.replace_time , Remark "
                     + "From SAJET.G_sn_repair_replace_kp b "
                     + "    ,sajet.sys_part c "
                     + "Where b.SERIAL_NUMBER = '" + g_sSN + "' "
                     + "and b.item_part_id = c.part_id "
                     + "order by b.replace_time ";
                DsReplace = ClientUtils.ExecuteSQL(sSQL);
            }
            else
            {
                //SN目前的Keyparts之前的Replace紀錄
                sSQL = "Select c.PART_NO , b.old_part_sn , b.new_part_sn ,b.replace_time ,Remark  "
                     + "From SAJET.g_sn_keyparts a "
                     + "    ,SAJET.G_sn_repair_replace_kp b "
                     + "    ,sajet.sys_part c "
                     + "Where a.SERIAL_NUMBER = '" + g_sSN + "' "
                     + "and a.item_part_sn = B.new_part_sn "
                     + "and b.item_part_id = c.part_id "
                     + "order by b.replace_time ";
                DsReplace = ClientUtils.ExecuteSQL(sSQL);
            }
            for (int i = 0; i <= DsReplace.Tables[0].Rows.Count - 1; i++)
            {
                LVReplaceHistory.Items.Add(DsReplace.Tables[0].Rows[i]["PART_NO"].ToString());
                LVReplaceHistory.Items[i].SubItems.Add(DsReplace.Tables[0].Rows[i]["old_part_sn"].ToString());
                LVReplaceHistory.Items[i].SubItems.Add(DsReplace.Tables[0].Rows[i]["new_part_sn"].ToString());
                LVReplaceHistory.Items[i].SubItems.Add(DsReplace.Tables[0].Rows[i]["replace_time"].ToString());
                LVReplaceHistory.Items[i].SubItems.Add(DsReplace.Tables[0].Rows[i]["Remark"].ToString());
            }
        }

        public void ShowRepairHistory()
        {
            /*
            LVRepairHistory.Items.Clear();
            //SN所有Replace紀錄(曾經屬於此SN的Keyparts都算)
            sSQL = "SELECT C.PROCESS_NAME \"Defect Process\", E.DEFECT_CODE||','||E.DEFECT_DESC \"Defect\" "
                 + "       ,D.PROCESS_NAME \"RP Process\", F.REASON_DESC, G.DUTY_CODE||','||G.DUTY_DESC  \"Duty\" "
                 + "       ,A.REC_TIME "
                 + "  FROM sajet.G_SN_DEFECT A,sajet.G_SN_REPAIR B,SAJET.SYS_PROCESS C ,SAJET.SYS_PROCESS D , "
                 + "       SAJET.SYS_DEFECT E,SAJET.SYS_REASON F, SAJET.SYS_DUTY G "
                 + " WHERE A.SERIAL_NUMBER = '" + g_sSN + "' "
                 + " AND A.RP_STATUS = '0' "
                 + " AND A.RECID = B.RECID "
                 + " AND A.PROCESS_ID = C.PROCESS_ID "
                 + " AND A.DEFECT_ID = E.DEFECT_ID "
                 + " AND B.RP_PROCESS_ID = D.PROCESS_ID "
                 + " AND B.REASON_ID = F.REASON_ID "
                 + " AND B.DUTY_ID=G.DUTY_ID "
                 + " ORDER BY A.REC_TIME,\"Defect\" ";
            DataSet DsRepair = ClientUtils.ExecuteSQL(sSQL);

            for (int i = 0; i <= DsRepair.Tables[0].Rows.Count - 1; i++)
            {
                LVRepairHistory.Items.Add(DsRepair.Tables[0].Rows[i]["Defect Process"].ToString());
                LVRepairHistory.Items[i].SubItems.Add(DsRepair.Tables[0].Rows[i]["Defect"].ToString());
                LVRepairHistory.Items[i].SubItems.Add(DsRepair.Tables[0].Rows[i]["RP Process"].ToString());
                LVRepairHistory.Items[i].SubItems.Add(DsRepair.Tables[0].Rows[i]["REASON_DESC"].ToString());
                LVRepairHistory.Items[i].SubItems.Add(DsRepair.Tables[0].Rows[i]["Duty"].ToString());
                LVRepairHistory.Items[i].SubItems.Add(DsRepair.Tables[0].Rows[i]["REC_TIME"].ToString());
            }
             */
            /*
            string sSQL = @" Select D.Process_Name ""Defect Process"" ,TO_CHAR(A.REC_TIME,'YYYY/MM/DD HH24:MI:SS') ""Defect Time"" ,
                      F.DEFECT_CODE ""Defect Code"",
                      F.DEFECT_DESC ""Defect Desc"" ,
                      G.REASON_CODE ""Reason Code"",
                      G.REASON_DESC ""Reason Desc"",
                      H.DUTY_CODE ""Duty Code"",
                      H.DUTY_DESC ""Duty Desc"" ,
                      L.lOCATION ""Location"",L.ITEM_NO ""Item No"",
                      I.EMP_NAME ""Repairer"",TO_CHAR(B.REPAIR_TIME,'YYYY/MM/DD HH24:MI:SS') ""Repair Time"" 
                    From SAJET.G_SN_DEFECT A,
                      SAJET.G_SN_REPAIR B,
                      SAJET.SYS_PROCESS D,
                      SAJET.SYS_DEFECT F,
                      SAJET.SYS_REASON G,
                      SAJET.SYS_DUTY H,
                      SAJET.SYS_EMP I,
                      SAJET.G_SN_REPAIR_LOCATION L
                    Where A.PROCESS_ID = D.PROCESS_ID
                      and A.DEFECT_ID = F.DEFECT_ID
                      and A.RECID = B.RECID(+)
                      and B.REASON_ID = G.REASON_ID(+)
                      and B.DUTY_ID = H.DUTY_ID(+)
                      and B.REPAIR_EMP_ID = I.EMP_ID(+)
                      and A.SERIAL_NUMBER = :SERIAL_NUMBER
                      and B.RECID=L.RECID(+)  
                    ORDER BY A.REC_TIME,B.REPAIR_TIME ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", g_sSN };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            dgvRepairHistory.DataSource = dsTemp;
            dgvRepairHistory.DataMember = dsTemp.Tables[0].ToString();
            for (int i = 0; i <= dgvRepairHistory.Columns.Count - 1; i++)
            {
                dgvRepairHistory.Columns[i].HeaderText = SajetCommon.SetLanguage(dgvRepairHistory.Columns[i].HeaderText);
            }
            dgvRepairHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            /*
            for (int i = 0; i <= dgvRepairHistory.Rows.Count - 1; i++)
            {
                string sReCID = dgvRepairHistory.Rows[i].Cells[dgvRepairHistory.Columns.Count - 1].Value.ToString();
                if (g_dtRECID.ContainsKey(sReCID))
                    dgvRepairHistory.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
            }
             */
        }



        private void ShowItemReplace()
        {

            lvItemReplace.Items.Clear();
            sSQL = "Select A.SERIAL_NUMBER,C.PART_NO,NVL(B.LOCATION,'N/A') LOCATION "
                  + "      ,NVL(B.DATECODE,'N/A') DATECODE ,NVL(B.LOT_NO,'N/A') LOT_NO ,B.UPDATE_TIME "
                  + "      ,NVL(B.BGA_FLAG,'N') BGA_FLAG,NVL(B.BGA_TYPE,'N/A') BGA_TYPE "
                  + "      ,NVL(E.EMP_NAME,'N/A') EMP_NAME "
                  + "      ,D.VENDOR_CODE,D.VENDOR_NAME "
                + "  From SAJET.G_SN_DEFECT A "
                + "      ,SAJET.G_SN_REPAIR_REPLACE_ITEM B "
                + "      ,SAJET.SYS_PART C "
                + "      ,SAJET.SYS_VENDOR D "
                + "      ,SAJET.SYS_EMP E "
                + "Where A.SERIAL_NUMBER = '" + g_sSN + "' "
                + "  AND A.RECID = B.RECID "
                + "  AND B.ITEM_ID = C.PART_ID(+) "
                + "  AND B.VENDOR_ID = D.VENDOR_ID(+) "
                + "  AND B.UPDATE_USERID = E.EMP_ID(+) "
                + " ORDER BY B.UPDATE_TIME  ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];

                lvItemReplace.Items.Add(dr["PART_NO"].ToString());
                lvItemReplace.Items[i].SubItems.Add(dr["LOCATION"].ToString());
                lvItemReplace.Items[i].SubItems.Add(dr["DATECODE"].ToString());
                lvItemReplace.Items[i].SubItems.Add(dr["LOT_NO"].ToString());
                lvItemReplace.Items[i].SubItems.Add(dr["VENDOR_NAME"].ToString());
                lvItemReplace.Items[i].SubItems.Add(dr["UPDATE_TIME"].ToString());
                lvItemReplace.Items[i].SubItems.Add(dr["EMP_NAME"].ToString());
                lvItemReplace.Items[i].SubItems.Add(dr["BGA_FLAG"].ToString());
                lvItemReplace.Items[i].SubItems.Add(dr["BGA_TYPE"].ToString());
            }
        }
        private void chkbAll_CheckedChanged(object sender, EventArgs e)
        {
            ShowReplace();
        }

        private void editSN_EnabledChanged(object sender, EventArgs e)
        {
            btnSearchSN.Enabled = editSN.Enabled;
        }

        private void fMain_Shown(object sender, EventArgs e)
        {
            editSN.Focus();
        }

        private void editRepairer_TextChanged(object sender, EventArgs e)
        {
            ClearData();
            editSN.Text = string.Empty;
        }

        private void btnMaintain_Click(object sender, EventArgs e)
        {

        }

        private void LabDefectProcess_Click(object sender, EventArgs e)
        {

        }

        private void btnRepairKP_Click(object sender, EventArgs e)
        {
            if (g_sSN == "")
                return;

            if (LVDefect.SelectedItems.Count == 0)
            {
                ClientUtils.ShowMessage(SajetCommon.SetLanguage("Please Select Defect"), 0);
                return;
            }
            if (dgvKP.Rows.Count == 0 || dgvKP.CurrentRow == null)
            {
                ClientUtils.ShowMessage(SajetCommon.SetLanguage("Please Select Keypart"), 0);
                return;
            }
            string sKPSN = dgvKP.CurrentRow.Cells["ITEM_PART_SN"].Value.ToString();
            string sKPPartID = dgvKP.CurrentRow.Cells["ITEM_PART_ID"].Value.ToString();
            RepairUtility.sDefectSN = g_sSN;
            RepairUtility.sDefectSNPartID = g_sPartID;
            RepairUtility.sDefectSNWO = LabWO.Text;
            RepairUtility.sProgram = g_sProgram;
            RepairUtility.sRepairType = "KEYPART";
            RepairUtility.sDefectRecID = LVDefect.SelectedItems[0].SubItems[3].Text;
            RepairUtility.sDefectCode = LVDefect.SelectedItems[0].Text;
            RepairUtility.sDefectLoc = LVDefect.SelectedItems[0].SubItems[3].Text;
            RepairUtility.sRepairSN = sKPSN;
            RepairUtility.sRepairSNPartID = sKPPartID;
            RepairUtility.sRepairSNWO = "N/A";
            fRepairData fRepair = new fRepairData();
            try
            {
                fRepair.ShowDialog();
            }
            finally
            {
                fRepair.Dispose();
            }
        }

        private void btnRepairSNHistory_Click(object sender, EventArgs e)
        {
            ShowRepairSNHistory(g_sSN);
        }
        private void ShowRepairSNHistory(string sSN)
        {
            Assembly assembly = null;
            object obj = null;
            Type type = null;
            string strApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!File.Exists(strApplicationPath + "\\RepairSNHistorydll.dll"))//在本地端發現DLL檔案則不另外組字串，否則從資料庫中搜尋程式在哪!
            {
                ClientUtils.ShowMessage(SajetCommon.SetLanguage("File Not Exist") + Environment.NewLine
                                       + SajetCommon.SetLanguage("File") + " : " + strApplicationPath + "\\RepairSNHistorydll.dll", 0);
                return;
            }
            try
            {
                //組裝資訊
                assembly = Assembly.LoadFrom(strApplicationPath + "\\RepairSNHistorydll.dll");
                type = assembly.GetType(("RepairSNHistorydll.fMain"));
                obj = assembly.CreateInstance(type.FullName, true, BindingFlags.CreateInstance, null, new object[] { g_sExeName, g_sProgram, g_sUserID, sSN }, null, null);
                ((Form)obj).StartPosition = FormStartPosition.CenterScreen;
                ((Form)obj).WindowState = FormWindowState.Normal;
                ((Form)obj).ShowDialog();

            }
            catch (Exception ex)
            { ClientUtils.ShowMessage("Load Function Error" + Environment.NewLine + ex.Message, 0); }
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {

        }

        private void spbtnRepairHistory_ButtonClick(object sender, EventArgs e)
        {

        }

        private void spbtnRepairHistory_ButtonClick_1(object sender, EventArgs e)
        {

        }

        private void btnRemoveALLKP_Click(object sender, EventArgs e)
        {
            if (g_sSN == "")
                return;
            if (dgvKP.Rows.Count == 0 || dgvKP.CurrentRow == null)
                return;
            int iKPCount = dgvKP.Rows.Count;

            if (ClientUtils.ShowMessage(SajetCommon.SetLanguage("Remove All Keyparts ?") + Environment.NewLine
                                        + SajetCommon.SetLanguage("Keyparts Count") + " : " + iKPCount.ToString(), 2) != DialogResult.Yes)
                return;
            try
            {
                object[][] Params = new object[4][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTERMINALID", RepairUtility.sTerminalID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", g_sSN };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", RepairUtility.sUserID };
                Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_REPAIR_REMOVE_KP_ALL", Params);
                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    ClientUtils.ShowMessage(sRes, 0);
                }
                Show_KP();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowMessage(ex.Message, 0);

            }
        }

        private void spbtnTravelCard_ButtonClick(object sender, EventArgs e)
        {

        }
        private void ShowTravelCard(string sParam, string strSN)
        {
            string sDirectoryName = SajetCommon.g_sExeName;
            if (!File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + SajetCommon.g_sExeName + Path.DirectorySeparatorChar + "CTRAVELCARD.DLL"))
            {
                sDirectoryName = "Query";
                if (!File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + sDirectoryName + Path.DirectorySeparatorChar + "CTRAVELCARD.DLL"))
                {
                    return;
                }

            }


            Assembly assembly = Assembly.LoadFrom(Application.StartupPath + Path.DirectorySeparatorChar + sDirectoryName + Path.DirectorySeparatorChar + "CTRAVELCARD.DLL");
            string[] Name = assembly.FullName.ToString().Split(',');
            int iParam = int.Parse(sParam);
            Type type = assembly.GetType(Name[0] + ".fMain");
            if (type != null)
            {
                object obj = Activator.CreateInstance(type);
                obj = assembly.CreateInstance(type.FullName, true, BindingFlags.CreateInstance, null, new object[] { iParam, strSN }, null, null);
                if (obj != null)
                {
                    string formTag = "Customer Report*Travel Card";
                    Form formChild = (Form)obj;
                    formChild.MdiParent = this.MdiParent;
                    formChild.Tag = formTag;
                    formChild.Show();
                }
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (g_sSN == "")
                return;
            if (LVDefect.SelectedItems.Count == 0)
                return;
            if (dgvKP.Rows.Count == 0 || dgvKP.CurrentRow == null)
                return;

            string sKPSN = dgvKP.CurrentRow.Cells["ITEM_PART_SN"].Value.ToString();
            ShowRepairSNHistory(sKPSN);
        }


        private void btnSNHistory_Click(object sender, EventArgs e)
        {
            if (g_sSN == "")
                return;

            string sTag = (sender as Button).Tag.ToString();
            switch (sTag)
            {
                case "1": ShowRepairSNHistory(g_sSN); break;
                case "2": ShowTravelCard("0", g_sSN); ; break;
                default:
                    break;
            }
        }
    }
}



