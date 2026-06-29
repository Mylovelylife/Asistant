using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;
using SajetFilter;
using System.Reflection;
using SajetAuto;
using System.Collections;
using System.IO;

namespace RepairDll
{
    public partial class fRepairData : Form
    {
        
        public fRepairData()
        {
            InitializeComponent();
        }


        public bool g_RepairType = false;
        public string g_sDupReason;
        public string g_sDefectRecID;
        public string sServerName;
        //public string g_sUserID;
        string g_sSN;
        string g_sWO;
        string g_sPartID;
        /*
        public string g_sRTerminalID;
        public string g_sRProcessID;
        public string g_sRStageID;
        public string g_sRPDLineID;
        public string g_sProgram;
         */
        bool g_bCheckItemRecord;

        //public int g_iLocateItem;
        string sSQL;
        DataSet dsTemp;
        string g_sReasonID = "0";
        string g_sDutyID = "0";
        string g_sRepairID = "0";
        fMain fM = new fMain();
        //add by rita 2014/11/24
        Dictionary<string, string> g_dtItemPartSN = new Dictionary<string, string>();

        //提供資料給開啟該頁面的視窗
        public List<Dictionary<string, string>> innerList;


        private void btnSearchReason_Click(object sender, EventArgs e)
        {
            sSQL = " Select Reason_Code,Reason_Desc,Reason_desc2 "
                 + " From SAJET.SYS_REASON "
                 + " Where Enabled='Y' "
                 + " Order By Reason_Code ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editReason.Text = f.dgvData.CurrentRow.Cells["Reason_Code"].Value.ToString();
                KeyPressEventArgs sKey = new KeyPressEventArgs((char)Keys.Return);
                editReason_KeyPress(editReason, sKey);
            }
            f.Dispose();
        }

        private void btnSearchDuty_Click(object sender, EventArgs e)
        {
            sSQL = " Select Duty_Code,Duty_Desc,DUTY_DESC2 "
                 + " From SAJET.SYS_DUTY "
                 + " Where Enabled='Y' "
                 + " Order By Duty_Code ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editDuty.Text = f.dgvData.CurrentRow.Cells["Duty_Code"].Value.ToString();
                KeyPressEventArgs sKey = new KeyPressEventArgs((char)Keys.Return);
                editDuty_KeyPress(editDuty, sKey);
            }
            f.Dispose();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //檢查是否輸入維修零件記錄 2013/03/11 - Lance
            if (g_bCheckItemRecord)
            {
                if (dgvItem.Rows.Count == 0)
                {
                    ClientUtils.ShowMessage(SajetCommon.SetLanguage("Please Input Item Repair Data"), 0);
                    return;
                }
            }
            if (g_sReasonID == "0")
            {
                if (!Check_Reason())
                    return;
            }

            if (g_sDutyID == "0")
            {
                if (!Check_Duty())
                    return;
            }

            //判斷Reason重複
            sSQL = "select serial_number from sajet.g_sn_repair "
               + " where serial_number='" + g_sSN + "' "
               + " and recid = '" + g_sDefectRecID + "' "
               + " and reason_id='" + g_sReasonID + "' "
               + " and duty_id = '" + g_sDutyID + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            g_sDupReason = "N";
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                g_sDupReason = "Y";
                if (dgvItem.Rows.Count == 0)
                {
                    ClientUtils.ShowMessage(SajetCommon.SetLanguage("Reason Code Duplicate", 1) + Environment.NewLine
                                   + SajetCommon.SetLanguage("Reason Code", 1) + " : " + editReason.Text, 0);
                    return;
                }
            }

            //組合Location & Item
            string sLocationData = "";
            if (dgvItem.Rows.Count == 0)
            {
                sLocationData = "N/A";
            }
            else
            {
                for (int i = 0; i <= dgvItem.Rows.Count - 1; i++)
                {
                    DataGridViewRow dr = dgvItem.Rows[i];
                    sLocationData += dr.Cells["LOCATION"].Value + "@"
                                    + dr.Cells["ITEM_NO"].Value + "@"
                                    + dr.Cells["MAIN_REASON"].Value + "@"
                                    + dr.Cells["REPAIR_CODE_ID"].Value + "@"
                                    + dr.Cells["ERROR_POINT"].Value + "@"
                                    + dr.Cells["ITEM_REMARK_1"].Value + "@"
                                    + dr.Cells["DATECODE"].Value + "@"
                                    + dr.Cells["LOT_NO"].Value + "@"
                                    + dr.Cells["VENDOR_CODE"].Value + "@"
                                    + dr.Cells["BGA"].Value + "@"
                                    + dr.Cells["BGA_TYPE"].Value + "@"
                                    + dr.Cells["REEL_NO"].Value + "@"
                                    + dr.Cells["MATERIAL_DATE"].Value + "@";
                    //+ dr.Cells["REPAIR_SN"].Value + "@"
                    //+ dr.Cells["REPAIR_SN_PART_ID"].Value + "@";
                }
            }

            try
            {
                DataSet ds;
                if (RepairUtility.sRepairType == "SERIAL NUMBER")
                {
                    //批次輸入的時候把相關資料先記錄下來 ~~ by Jim 20260626
                    if (g_RepairType)
                    {
                        innerList = new List<Dictionary<string, string>>();

                        Dictionary<string, string> paramDict = new Dictionary<string, string>();

                        paramDict.Add("TSN", g_sSN);
                        paramDict.Add("TWO", g_sWO);
                        paramDict.Add("TPARTID", g_sPartID);
                        paramDict.Add("TRECID", g_sDefectRecID);
                        paramDict.Add("TREASONID", g_sReasonID);
                        paramDict.Add("TDUTYID", g_sDutyID);
                        paramDict.Add("TEMPID", RepairUtility.sUserID);
                        paramDict.Add("TTERMINALID", RepairUtility.sTerminalID);
                        paramDict.Add("TREMARK", RTextRemark.Text.Trim());
                        paramDict.Add("TREPAIRMETHOD", rtRepairMethod.Text.Trim());
                        paramDict.Add("TLOCATIONDATA", sLocationData);

                        innerList.Add(paramDict);
                    }
                    else
                    {
                        object[][] Params = new object[12][];
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", g_sSN };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TWO", g_sWO };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPARTID", g_sPartID };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRECID", g_sDefectRecID };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREASONID", g_sReasonID };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TDUTYID", g_sDutyID };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", RepairUtility.sUserID };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTERMINALID", RepairUtility.sTerminalID };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREMARK", RTextRemark.Text.Trim() };
                        Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREPAIRMETHOD", rtRepairMethod.Text.Trim() };
                        Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOCATIONDATA", sLocationData };
                        Params[11] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                        ds = ClientUtils.ExecuteProc("SAJET.SJ_REPAIR_REASON", Params);

                        string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                        if (sRes != "OK")
                        {
                            ClientUtils.ShowMessage( $@"{g_sSN}：{sRes}" , 0);
                            return;
                        }
                    }
                }
                else
                {
                    object[][] Params = new object[12][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TDEFECTSNID", RepairUtility.sDefectSNID };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRECID", RepairUtility.sDefectRecID };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TDEFECTID", RepairUtility.sDefectID };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", RepairUtility.sRepairSN };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREASONID", g_sReasonID };
                    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TDUTYID", g_sDutyID };
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", RepairUtility.sUserID };
                    Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTERMINALID", RepairUtility.sTerminalID };
                    Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREMARK", RTextRemark.Text.Trim() };
                    Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREPAIRMETHOD", rtRepairMethod.Text.Trim() };
                    Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOCATIONDATA", sLocationData };
                    Params[11] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                    ds = ClientUtils.ExecuteProc("SAJET.SJ_REPAIR_REASON_SUBPARTS", Params);

                    string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                    if (sRes != "OK")
                    {
                        ClientUtils.ShowMessage(sRes, 0);
                        return;
                    }
                }

                
            }
            catch (Exception ex)
            {
                ClientUtils.ShowMessage(ex.Message, 0);
                return;
            }
            //====            
            DialogResult = DialogResult.OK;
        }
        private void editLC_KeyPress(object sender, KeyPressEventArgs e)
        {
            //輸入Location帶出Item
            if (e.KeyChar != (char)Keys.Return)
                return;
            editLC.Text = editLC.Text.Trim();
            string sLoc = editLC.Text;
            combItem.Items.Clear();
            combItem.Text = string.Empty;
            lablItemSpec.Text = string.Empty;
            string sSN = g_sSN;

            string strSQL = "";
            strSQL = " SELECT PARAM_VALUE "
                 + "   FROM SAJET.SYS_BASE "
                 + "  WHERE Upper(PROGRAM) = '" + "Repair".ToUpper() + "' "
                 + "    and Upper(PARAM_NAME) = '" + "Location Table".ToUpper() + "' ";
            DataSet dsTemq = ClientUtils.ExecuteSQL(strSQL);
            string[] sResultList;
            if (dsTemq.Tables[0].Rows.Count > 0)
                sResultList = RepairUtility.GET_LOCATION_ITEM_PART_FROM_WO(sSN, sLoc);
            else
                sResultList = RepairUtility.GET_LOCATION_ITEM_PART(sSN, sLoc);
            for (int i = 0; i <= sResultList.Length - 1; i++)
                combItem.Items.Add(sResultList[i]);
            if (combItem.Items.Count == 1)
            {
                combItem.SelectedIndex = 0;
            }
            else
            {
                combItem.Focus();
                combItem.SelectAll();
            }
        }

        private bool CheckItem(string sItemNo, ref string sItemPartID, ref string sItemSpec)
        {
            sSQL = " SELECT PART_ID,SPEC1 FROM SAJET.SYS_PART "
                + "  WHERE PART_NO =:PART_NO "
                + "    AND ROWNUM = 1 ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", sItemNo };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                ClientUtils.ShowMessage("Item Error", 0);
                return false;
            }
            sItemPartID = dsTemp.Tables[0].Rows[0]["PART_ID"].ToString();
            sItemSpec = dsTemp.Tables[0].Rows[0]["SPEC1"].ToString();
            return true;
        }

        private void combItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            lablItemSpec.Text = string.Empty;
            //lablDateCode.Text = string.Empty;
            combItem.Text = combItem.Text.Trim();
            string sItemPartID = "0";
            string sItemSpec = string.Empty;
            if (!CheckItem(combItem.Text, ref sItemPartID, ref sItemSpec))
            {
                combItem.Focus();
                combItem.SelectAll();
                return;
            }
            lablItemSpec.Text = sItemSpec;
            if (editErrorPoint.Visible)
            {
                editErrorPoint.Focus();
                editErrorPoint.SelectAll();
            }
        }

        private void btnSearchKP_Click(object sender, EventArgs e)
        {
            if (combItem.Text.Trim().Length < 5)
            {
                ClientUtils.ShowMessage(SajetCommon.SetLanguage("Please Input Part No Prefix(5 char)"), 0);
                return;
            }
            sSQL = " Select Part_No,SPEC1,SPEC2 "
                 + " From SAJET.SYS_PART"
                 + " Where Enabled='Y' "
                 + " and PART_NO Like '" + combItem.Text + "%'"
                 + " Order By Part_No ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;

            if (f.ShowDialog() == DialogResult.OK)
            {
                combItem.Text = f.dgvData.CurrentRow.Cells["Part_No"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                combItem_KeyPress(sender, Key);
            }
            f.Dispose();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            editLC.Text = editLC.Text.Trim();
            combItem.Text = combItem.Text.Trim();
            editErrorPoint.Text = editErrorPoint.Text.Trim();
            string sSN = g_sSN;
            string sSNPartID = g_sPartID;

            switch (RepairUtility.iLocationParams)
            {
                case 1://Location一定要輸入
                    if (editLC.Text == "")
                    {
                        ClientUtils.ShowMessage("Please input Location", 1);
                        editLC.Focus();
                        return;
                    }
                    break;
                case 2://Item一定要輸入
                    if (combItem.Text == "")
                    {
                        ClientUtils.ShowMessage("Please input Item No", 1);
                        combItem.Focus();
                        return;
                    }
                    break;
                case 3://二者要輸入一個
                    if (editLC.Text == "" && combItem.Text == "")
                    {
                        ClientUtils.ShowMessage("Please input Location or Item No", 1);
                        return;
                    }
                    break;
                case 4://二者都要輸入
                    if (editLC.Text == "")
                    {
                        ClientUtils.ShowMessage("Please input Location", 1);
                        editLC.Focus();
                        return;
                    }
                    if (combItem.Text == "")
                    {
                        ClientUtils.ShowMessage("Please input Item No", 1);
                        combItem.Focus();
                        return;
                    }
                    break;
                default:
                    if (editLC.Text == "" && combItem.Text == "")
                    {
                        ClientUtils.ShowMessage("Please input Location or Item No", 1);
                        return;
                    }
                    break;
            }
            if (txtRepairCode.Text == "")
            {
                ClientUtils.ShowMessage("Please input Repair Method", 1);
                return;
            }
            // Error Point為非必要輸入 20130321-Lance
            if (string.IsNullOrEmpty(editErrorPoint.Text))
            {
                //SajetCommon.Show_Message("Please input Err Point", 1);
                //return;
            }
            //check RepairCode 錯誤
            if (!chk_Repair_Code()) return;
            if (!Chk_Error_Point()) return;

            string sItemPartID = "0";
            string sItemSpec = string.Empty;
            if (!string.IsNullOrEmpty(combItem.Text))
            {
                if (!CheckItem(combItem.Text, ref sItemPartID, ref sItemSpec))
                {
                    combItem.Focus();
                    combItem.SelectAll();
                    return;
                }
            }

            for (int i = 0; i <= dgvItem.Rows.Count - 1; i++)
            {
                string sLoc = dgvItem.Rows[i].Cells["LOCATION"].Value.ToString();
                string sItemNo = dgvItem.Rows[i].Cells["ITEM_NO"].Value.ToString();
                if (sLoc == editLC.Text && sItemNo == combItem.Text)
                {
                    ClientUtils.ShowMessage("Item Duplicate", 0);
                    combItem.Focus();
                    combItem.SelectAll();
                    return;
                }
            }
            string sTag = (sender as Button).Tag.ToString();
            string sReplaceItemNo = string.Empty;
            string sReplaceLoc = string.Empty;
            string sReplaceDateCode = string.Empty;
            string sReplaceLotNo = string.Empty;
            string sReplaceVendorCode = string.Empty;
            string sReelNo = string.Empty;
            string sMaterialDate = string.Empty;
            bool bBGAFlag = false;
            string sBGAType = string.Empty;
            if (sTag == "2")
            {
                fReplaceItem fData = new fReplaceItem();
                try
                {
                    //fData.g_sUserID = g_sUserID;
                    // fData.g_sRTerminalID = g_sRTerminalID;
                    fData.g_sDefectRECID = g_sDefectRecID;
                    fData.LabSN.Text = g_sSN;
                    fData.g_sItemNo = combItem.Text;
                    fData.g_sLoc = editLC.Text;
                    //fData.g_iLocateItem = g_iLocateItem;
                    fData.g_sWO = g_sWO;
                    fData.g_sSN = g_sSN;
                    if (fData.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    sReplaceLoc = fData.g_sLoc;
                    sReplaceItemNo = fData.g_sItemNo;
                    sReplaceDateCode = fData.g_sDateCode;
                    sReplaceLotNo = fData.g_sLotNo;
                    sReplaceVendorCode = fData.g_sVendorCode;
                    bBGAFlag = fData.g_bBGAFlag;
                    sBGAType = fData.g_sBGAType;
                    sReelNo = fData.g_sReelNo;
                    sMaterialDate = fData.g_sMaterialInTime;
                }
                finally
                {
                    fData.Dispose();
                }
            }
            //之前有 BUG !!
            if (!combItem.Text.Equals(string.Empty))
                sReplaceItemNo = combItem.Text;
            dgvItem.Rows.Add();
            int iRow = dgvItem.Rows.Count - 1;
            dgvItem.Rows[iRow].Cells["ITEM_FLAG"].Value = (sTag.Equals("2") ? "Y" : "N");
            dgvItem.Rows[iRow].Cells["LOCATION"].Value = editLC.Text;
            dgvItem.Rows[iRow].Cells["ITEM_NO"].Value = sReplaceItemNo;
            dgvItem.Rows[iRow].Cells["ERROR_POINT"].Value = editErrorPoint.Text;
            dgvItem.Rows[iRow].Cells["MAIN_REASON"].Value = (chkMainReson.Checked ? "Y" : "N");
            dgvItem.Rows[iRow].Cells["REPAIR_CODE"].Value = txtRepairCode.Text;
            dgvItem.Rows[iRow].Cells["REPAIR_CODE_DESC"].Value = txtRepairCodeDesc.Text;
            dgvItem.Rows[iRow].Cells["REPAIR_CODE_DESC2"].Value = txtRepairCodeDesc2.Text;
            dgvItem.Rows[iRow].Cells["REPAIR_CODE_ID"].Value = g_sRepairID;
            dgvItem.Rows[iRow].Cells["ITEM_REMARK_1"].Value = comboRemark.Text;
            dgvItem.Rows[iRow].Cells["LOT_NO"].Value = sReplaceLotNo;
            dgvItem.Rows[iRow].Cells["VENDOR_CODE"].Value = sReplaceVendorCode;
            dgvItem.Rows[iRow].Cells["DATECODE"].Value = sReplaceDateCode;
            dgvItem.Rows[iRow].Cells["BGA"].Value = bBGAFlag ? "Y" : "N";
            dgvItem.Rows[iRow].Cells["BGA_TYPE"].Value = sBGAType;
            dgvItem.Rows[iRow].Cells["REEL_NO"].Value = sReelNo;
            dgvItem.Rows[iRow].Cells["MATERIAL_DATE"].Value = sMaterialDate;
            dgvItem.Rows[iRow].Cells["REPAIR_SN"].Value = sSN;
            dgvItem.Rows[iRow].Cells["REPAIR_SN_PART_ID"].Value = sSNPartID;

            editLC.Text = string.Empty;
            combItem.Items.Clear();
            combItem.Text = string.Empty;
            lablItemSpec.Text = string.Empty;

            txtRepairCode.Text = string.Empty;
            txtRepairCodeDesc.Text = string.Empty;
            txtRepairCodeDesc2.Text = string.Empty;
            editErrorPoint.Text = string.Empty;

            editLC.BackColor = Color.White;
            txtRepairCode.BackColor = Color.White;

            //add by rita 2014/11/24
            combKPSN.SelectedIndex = -1;
            combKPSN.Text = string.Empty;

            editLC.Focus();
            editLC.SelectAll();
        }

        public void GetWorkStaionOption()
        {
            try
            {
                sSQL = "SELECT * "
                    + " FROM SAJET.SYS_MODULE_PARAM "
                    + " WHERE MODULE_NAME ='" + RepairUtility.sProgram + "' "
                    + "   AND FUNCTION_NAME ='Work Station Configuration' "
                    + "   AND PARAME_NAME = '" + RepairUtility.sTerminalID + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                g_bCheckItemRecord = true;
                for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                {
                    string sParamItem = dsTemp.Tables[0].Rows[i]["PARAME_ITEM"].ToString();
                    string sParamValue = dsTemp.Tables[0].Rows[i]["PARAME_VALUE"].ToString();
                    switch (sParamItem)
                    {
                        case "Check Item Repair Record":
                            g_bCheckItemRecord = (sParamValue == "Y");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientUtils.ShowMessage(ex.Message, 0);
            }
        }

        private void fRepairData_Load(object sender, EventArgs e)
        {
            //AutoCompleteSetting
            //SajetAutoComplete.AutoCompleteFunction("Repair_Code", "SAJET.SYS_REPAIR_CODE", txtRepairCode);
            //SajetAutoComplete.AutoCompleteFunction("Reason_Code", "SAJET.SYS_REASON", editReason);
            //SajetAutoComplete.AutoCompleteFunction("Duty_Code", "SAJET.SYS_DUTY", editDuty);
            //SajetAutoComplete.AutoCompleteFunction("LOCATION", "sajet.sys_bom_location", editLC);
            g_sDefectRecID = RepairUtility.sDefectRecID;



            g_sSN = RepairUtility.sRepairSN;
            g_sWO = RepairUtility.sRepairSNWO;
            g_sPartID = RepairUtility.sRepairSNPartID;
            LabSN.Text = g_sSN;
            LabDefCode.Text = RepairUtility.sDefectCode;
            editLC.Text = RepairUtility.sDefectLoc;


            ClientUtils.SetLanguage(this, fMain.g_sExeName);
            tabControl1.SelectedIndex = 1;
            //==================讀取工作站的設定================
            GetWorkStaionOption();
            editLC.BackColor = Color.White;
            combItem.BackColor = Color.White;
            switch (RepairUtility.iLocationParams)
            {
                case 1://Location一定要輸入
                    LabLC.ForeColor = Color.Red;
                    if (g_bCheckItemRecord)
                        editLC.BackColor = Color.FromArgb(255, 255, 128);
                    break;
                case 2://Item一定要輸入
                    LabItem.ForeColor = Color.Red;
                    if (g_bCheckItemRecord)
                        combItem.BackColor = Color.FromArgb(255, 255, 128);
                    break;
                case 4://二者都要輸入
                    LabLC.ForeColor = Color.Red;
                    LabItem.ForeColor = Color.Red;
                    if (g_bCheckItemRecord)
                    {
                        editLC.BackColor = Color.FromArgb(255, 255, 128);
                        combItem.BackColor = Color.FromArgb(255, 255, 128);
                    }
                    break;
            }
            label3.ForeColor = LabLC.ForeColor;
            editLC.BackColor = Color.FromArgb(255, 255, 128);
            sSQL = "SELECT DEFECT_DESC,DEFECT_ID FROM SAJET.SYS_DEFECT "
                + " WHERE DEFECT_CODE ='" + LabDefCode.Text + "'  AND ROWNUM = 1";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                LabDefDesc.Text = dsTemp.Tables[0].Rows[0]["DEFECT_DESC"].ToString();
                //LabDefDesc1.Text = dsTemp.Tables[0].Rows[0]["DEFECT_DESC2"].ToString();
                RepairUtility.sDefectID = dsTemp.Tables[0].Rows[0]["DEFECT_ID"].ToString();
            }
            lablRepairType.Text = RepairUtility.sRepairType;
            switch (RepairUtility.sRepairType)
            {
                case "SERIAL NUMBER":
                    lablRepairType.Text = SajetCommon.SetLanguage("Repair Self");
                    lablRepairType.BackColor = Color.Brown;
                    lablRepairType.ForeColor = Color.White;
                    break;
                case "KEYPART":
                    lablRepairType.Text = SajetCommon.SetLanguage("Repair Keypart");
                    lablRepairType.BackColor = Color.Teal;
                    lablRepairType.ForeColor = Color.White;
                    break;
                default: break;
            }

            editReason.Focus();
            GetItemRemark();

            for (int i = 0; i <= dgvItem.Columns.Count - 1; i++)
                dgvItem.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        private void GetItemRemark()
        {
            string sSQL = "SELECT ITEM_REMARK FROM SAJET.SYS_REPAIR_ITEM_REMARK "
                  + " WHERE ENABLED='Y' "
                  + " ORDER BY ITEM_REMARK ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            comboRemark.Items.Clear();
            comboRemark.Items.Add("");
            comboRemark.Items.Add("N/A");
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dsRow = dsTemp.Tables[0].Rows[i];
                comboRemark.Items.Add(dsRow["ITEM_REMARK"].ToString());
            }
        }

        private void editReason_KeyPress(object sender, KeyPressEventArgs e)
        {
            LabReasonDesc.Text = "";
            LabReasonDesc1.Text = "";
            g_sReasonID = "0";
            if (e.KeyChar != (char)Keys.Return)
                return;

            if (Check_Reason())
            {
                editDuty.Focus();
                editDuty.SelectAll();
            }
        }

        public bool Check_Reason()
        {
            sSQL = " Select Reason_Id, Reason_Code, Reason_Desc,REASON_DESC2 "
                     + " From SAJET.SYS_REASON "
                     + " Where Enabled = 'Y' "
                     + " and REASON_CODE = '" + editReason.Text + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                ClientUtils.ShowMessage(SajetCommon.SetLanguage("Reason Code Error", 1) + Environment.NewLine
                                       + SajetCommon.SetLanguage("Reason Code", 1) + " : " + editReason.Text, 0);
                editReason.Focus();
                editReason.SelectAll();
                return false;
            }
            g_sReasonID = dsTemp.Tables[0].Rows[0]["Reason_ID"].ToString();
            LabReasonDesc.Text = dsTemp.Tables[0].Rows[0]["Reason_Desc"].ToString();
            LabReasonDesc1.Text = dsTemp.Tables[0].Rows[0]["REASON_DESC2"].ToString();

            return true;
        }

        private void editDuty_KeyPress(object sender, KeyPressEventArgs e)
        {
            LabDutyDesc.Text = "";
            LabDutyDesc1.Text = "";
            g_sDutyID = "0";
            if (e.KeyChar != (char)Keys.Return)
                return;

            if (Check_Duty())
            {
                editLC.Focus();
                editLC.SelectAll();
            }
        }

        public bool Check_Duty()
        {
            sSQL = " Select Duty_ID, Duty_Code, Duty_Desc,DUTY_DESC2 "
                 + " From SAJET.SYS_Duty "
                 + " Where Enabled = 'Y' "
                 + " and Duty_Code = '" + editDuty.Text + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                ClientUtils.ShowMessage(SajetCommon.SetLanguage("Duty Code Error", 1) + Environment.NewLine
                               + SajetCommon.SetLanguage("Duty Code", 1) + " : " + editDuty.Text, 0);
                editDuty.Focus();
                editDuty.SelectAll();
                return false;
            }
            g_sDutyID = dsTemp.Tables[0].Rows[0]["Duty_ID"].ToString();
            LabDutyDesc.Text = dsTemp.Tables[0].Rows[0]["Duty_Desc"].ToString();
            LabDutyDesc1.Text = dsTemp.Tables[0].Rows[0]["DUTY_DESC2"].ToString();
            return true;
        }

        private void MenuItemDelete_Click(object sender, EventArgs e)
        {
            if (dgvItem.Rows.Count == 0)
                return;
            int iIndex = dgvItem.CurrentRow.Index;
            dgvItem.Rows.RemoveAt(iIndex);
        }

        private void fRepairData_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
        }

        private void editLC_TextChanged(object sender, EventArgs e)
        {
            combItem.Items.Clear();
            combItem.Text = string.Empty;
            lablItemSpec.Text = string.Empty;
        }

        private void combItem_TextChanged(object sender, EventArgs e)
        {
            lablItemSpec.Text = string.Empty;
        }

        private void combItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
            combItem_KeyPress(sender, Key);
        }

        private void btnReplaceItem_Click(object sender, EventArgs e) //Replace Button
        {
            editLC.Text = editLC.Text.Trim();
            combItem.Text = combItem.Text.Trim();
            string sItemPartID = "0";
            string sItemSpec = string.Empty;
            if (!CheckItem(combItem.Text, ref sItemPartID, ref sItemSpec))
                return;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void editReason_TextChanged(object sender, EventArgs e)
        {
            g_sReasonID = "0";
            LabReasonDesc.Text = string.Empty;
            LabReasonDesc1.Text = string.Empty;
        }

        private void editDuty_TextChanged(object sender, EventArgs e)
        {
            g_sDutyID = "0";
            LabDutyDesc.Text = string.Empty;
            LabDutyDesc1.Text = string.Empty;
        }

        private void txtRepairCode_TextChanged(object sender, EventArgs e)
        {
            g_sRepairID = "0";
            txtRepairCodeDesc.Text = string.Empty;
            txtRepairCodeDesc2.Text = string.Empty;
        }

        private void txtRepairCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            if (chk_Repair_Code())
            {
                editLC.Focus();
                editLC.SelectAll();
            }
        }

        private void btnSearchRepair_Click(object sender, EventArgs e)
        {
            sSQL = " Select Repair_ID, Repair_Code,Repair_Desc,Repair_DESC2 "
                 + " From SAJET.SYS_REPAIR_CODE "
                 + " Where Enabled='Y' "
                 + " Order By Repair_Code ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            f.strHideColumn = "Repair_id";
            if (f.ShowDialog() == DialogResult.OK)
            {
                txtRepairCode.Text = f.dgvData.CurrentRow.Cells["Repair_Code"].Value.ToString();
                g_sRepairID = f.dgvData.CurrentRow.Cells["Repair_id"].Value.ToString();
                txtRepairCodeDesc.Text = f.dgvData.CurrentRow.Cells["Repair_Desc"].Value.ToString();
                txtRepairCodeDesc2.Text = f.dgvData.CurrentRow.Cells["Repair_Desc2"].Value.ToString();
                //KeyPressEventArgs sKey = new KeyPressEventArgs((char)Keys.Return);
                //editDuty_KeyPress(editDuty, sKey);
                btnAdd.Focus();
            }
            f.Dispose();
        }

        public bool chk_Repair_Code()
        {
            sSQL = " Select Repair_id,Repair_Code,Repair_Desc,Repair_DESC2 "
                 + " From SAJET.SYS_REPAIR_CODE "
                 + " Where Enabled = 'Y' "
                 + " and Repair_Code = :Repair_Code";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "Repair_Code", txtRepairCode.Text };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                ClientUtils.ShowMessage(SajetCommon.SetLanguage("Repair Code Error", 1) + Environment.NewLine
                               + SajetCommon.SetLanguage("Repair Code", 1) + " : " + txtRepairCode.Text, 0);
                txtRepairCode.Focus();
                txtRepairCode.SelectAll();
                return false;
            }
            g_sRepairID = dsTemp.Tables[0].Rows[0]["Repair_id"].ToString();
            txtRepairCodeDesc.Text = dsTemp.Tables[0].Rows[0]["Repair_Desc"].ToString();
            txtRepairCodeDesc2.Text = dsTemp.Tables[0].Rows[0]["Repair_Desc2"].ToString();
            return true;
        }

        private bool Chk_Error_Point()
        {
            if (string.IsNullOrEmpty(editErrorPoint.Text))
                return true;
            try
            {
                Convert.ToInt32(editErrorPoint.Text);
                return true;
            }
            catch
            {
                ClientUtils.ShowMessage("Error Point is Invalid", 0);
                return false;
            }
        }

        private void txtRepairCodeDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void editErrorPoint_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == 13))
            {
                e.KeyChar = (char)Keys.None;
            }
            if (e.KeyChar == (char)Keys.Return)
            {
                txtRepairCode.Focus();
                txtRepairCode.SelectAll();
            }
        }

        private void fRepairData_Shown(object sender, EventArgs e)
        {
            editReason.Focus();
        }



    }
}