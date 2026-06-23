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

namespace RepairDll
{
    public partial class fReplace : Form
    {
        public fReplace()
        {
            InitializeComponent();
        }
        public fReplace(string f_sItemPartSN)
        {
            InitializeComponent();
            g_sItemPartSN = f_sItemPartSN;
        }

        public string sServerName;
        //  public string g_sUserID;
        public string g_sTerminalID;
        public string g_sDefectRECID;
        public string g_sType;
        string sSQL;
        DataSet dsTemp;
        string g_sItemPartSN = "N/A";
        fMain fM = new fMain();

        public void Show_KP(string sSN)
        {
            dgvKP.Rows.Clear();
            sSQL = " Select A.work_order, A.ITEM_PART_ID,A.Item_Group, A.Process_Id "
                 + "       ,B.PART_NO ,ITEM_PART_SN ,B.SPEC1 "
                 + "       ,TO_CHAR(A.UPDATE_TIME,'YYYY/MM/DD HH24:MI:SS') UPDATE_TIME  "
                 + "       ,C.PROCESS_NAME ,D.EMP_NAME  "
                 + " From SAJET.G_SN_KEYPARTS A "
                 + " ,SAJET.SYS_PART B "
                 + " ,SAJET.SYS_PROCESS C "
                 + " ,SAJET.SYS_EMP D "
                 + " Where A.SERIAL_NUMBER = '" + sSN + "' "
                 + " and A.ITEM_PART_ID = B.PART_ID(+) "
                 + " AND A.PROCESS_ID = C.PROCESS_ID "
                 + " AND A.UPDATE_USERID = D.EMP_ID(+) "
                 + " Order By B.PART_NO ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                dgvKP.Rows.Add();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["ITEM_PART_NO"].Value = dsTemp.Tables[0].Rows[i]["PART_NO"].ToString();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["ITEM_PART_SN"].Value = dsTemp.Tables[0].Rows[i]["ITEM_PART_SN"].ToString();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["SPEC1"].Value = dsTemp.Tables[0].Rows[i]["SPEC1"].ToString();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["WORK_ORDER"].Value = dsTemp.Tables[0].Rows[i]["WORK_ORDER"].ToString();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["ITEM_GROUP"].Value = dsTemp.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["PROCESS_ID"].Value = dsTemp.Tables[0].Rows[i]["PROCESS_ID"].ToString();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["ITEM_PART_ID"].Value = dsTemp.Tables[0].Rows[i]["ITEM_PART_ID"].ToString();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["ASSY_TIME"].Value = dsTemp.Tables[0].Rows[i]["UPDATE_TIME"].ToString();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["ASSY_PROCESS"].Value = dsTemp.Tables[0].Rows[i]["PROCESS_NAME"].ToString();
                dgvKP.Rows[dgvKP.Rows.Count - 1].Cells["ASSY_EMP"].Value = dsTemp.Tables[0].Rows[i]["EMP_NAME"].ToString();
            }
            if (g_sItemPartSN != "N/A")
                SetSelectRow(dgvKP, g_sItemPartSN, "ITEM_PART_SN");

        }
        private void SetSelectRow(DataGridView GridData, String sPrimaryKey, String sField)
        {
            if (GridData.Rows.Count > 0)
            {
                int iIndex = 0;
                for (int i = 0; i <= GridData.Rows.Count - 1; i++)
                {
                    if (sPrimaryKey == GridData.Rows[i].Cells[sField].Value.ToString())
                    {
                        iIndex = i;
                        break;
                    }
                }
                GridData.Focus();
                GridData.CurrentCell = GridData.Rows[iIndex].Cells[sField];
                GridData.Rows[iIndex].Selected = true;
            }
        }

        private void LVECLoadDefaultValues()
        {
            string sDefectCode = "E000";
            string sSQL = "SELECT DEFECT_ID,DEFECT_DESC "
                       + "  FROM SAJET.SYS_DEFECT "
                       + " WHERE DEFECT_CODE='" + sDefectCode + "' "
                       + "   AND ROWNUM = 1 ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                LVEC.Items.Add(sDefectCode);
                LVEC.Items[LVEC.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["DEFECT_DESC"].ToString());
                LVEC.Items[LVEC.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["DEFECT_ID"].ToString());
            }
        }
        private void fReplace_Load(object sender, EventArgs e)
        {
            ClientUtils.SetLanguage(this, fMain.g_sExeName);
            Show_KP(LabSN.Text);
            LVECLoadDefaultValues();
            rdbtnNo.Checked = true;
            rdbtnYes.Checked = LVEC.Items.Count > 0;
            if (g_sType == "REPLACE")
            {
                btnRemove.Visible = false;
                btnReplace.Visible = true;
                LabNewKPSN.Visible = true;
                gbNewKP.Visible = true;

                btnReplace.Left = btnRemove.Left;
                this.Text = SajetCommon.SetLanguage("Replace Keyparts");
            }
            else if (g_sType == "REMOVE")
            {
                btnRemove.Visible = true;
                btnReplace.Visible = false;
                LabNewKPSN.Visible = false;
                gbNewKP.Visible = false;
                this.Text = SajetCommon.SetLanguage("Remove Keyparts");
            }

            editNewKPSN.Visible = LabNewKPSN.Visible;
            LabRemark.Visible = LabNewKPSN.Visible;
            RichTextRemark.Visible = LabNewKPSN.Visible;
        }

        private void editDefect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            string sDefectCode = editDefect.Text.Trim();
            sSQL = " SELECT DEFECT_ID,DEFECT_CODE, DEFECT_DESC "
                 + " FROM SAJET.SYS_DEFECT "
                 + " WHERE DEFECT_CODE = '" + sDefectCode + "' "
                 + " AND ENABLED = 'Y' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                ClientUtils.ShowMessage("Defect Code Error", 0);
                editDefect.Focus();
                editDefect.SelectAll();
                return;
            }
            if (LVEC.FindItemWithText(sDefectCode) != null)
            {
                ClientUtils.ShowMessage("Defect Code Duplicate", 0);
                editDefect.Focus();
                editDefect.SelectAll();
                return;
            }
            LVEC.Items.Add(dsTemp.Tables[0].Rows[0]["DEFECT_CODE"].ToString());
            LVEC.Items[LVEC.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["DEFECT_DESC"].ToString());
            LVEC.Items[LVEC.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["DEFECT_ID"].ToString());

            editDefect.Focus();
            editDefect.SelectAll();
        }

        private void rdbtnYes_Click(object sender, EventArgs e)
        {
            if (rdbtnYes.Checked)
            {
                editDefect.Enabled = true;
                editDefect.Text = "";
                editDefect.Focus();
                LVEC.Items.Clear();
            }
        }

        private void rdbtnNo_Click(object sender, EventArgs e)
        {
            if (rdbtnNo.Checked)
            {
                editDefect.Enabled = false;
                editDefect.Text = "";
                LVEC.Items.Clear();
            }
        }

        private void MenuItemDelete_Click(object sender, EventArgs e)
        {
            if (LVEC.SelectedItems.Count == 0)
                return;

            for (int i = LVEC.SelectedItems.Count - 1; i >= 0; i--)
            {
                LVEC.SelectedItems[i].Remove();
            }
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvKP.Rows.Count == 0)
            {
                ClientUtils.ShowMessage("No any Keypart", 0);
                return;
            }

            if (dgvKP.CurrentRow == null)
            {
                ClientUtils.ShowMessage("Please select a Keypart to replace", 0);
                return;
            }

            if (rdbtnYes.Checked)
            {
                if (LVEC.Items.Count == 0)
                {
                    ClientUtils.ShowMessage("Please Input Defect Code", 0);
                    return;
                }
            }
            string sWO = dgvKP.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
            string sKPSN = dgvKP.CurrentRow.Cells["ITEM_PART_SN"].Value.ToString();
            string sKPNO = dgvKP.CurrentRow.Cells["ITEM_PART_NO"].Value.ToString();
            string sKPID = dgvKP.CurrentRow.Cells["ITEM_PART_ID"].Value.ToString();
            string sProcessID = dgvKP.CurrentRow.Cells["PROCESS_ID"].Value.ToString();
            string sItemGroup = dgvKP.CurrentRow.Cells["ITEM_GROUP"].Value.ToString();
            bool bTransOK = false;

            if (g_sType == "REPLACE")
            {
                editNewKPSN.Text = editNewKPSN.Text.ToUpper();
                editNewKPSN.Text = editNewKPSN.Text.Trim();
                if (editNewKPSN.Text == "")
                {
                    ClientUtils.ShowMessage("Please Input New Keypart SN", 0);
                    editNewKPSN.Focus();
                    editNewKPSN.SelectAll();
                    return;
                }

                //檢查New KPSN是否合法
                if (!Check_NewKPSN(sKPSN, LabSN.Text, sWO, sKPID))
                {
                    editNewKPSN.Focus();
                    editNewKPSN.SelectAll();
                    return;
                }

                //檢查 KPSN 相關項目
                //string sNewPartID;
                //string sNewPartSN;
                //string sNewItem;                
                //if (!Check_RepairPart(sKPSN,sKPID,sItemGroup,sProcessID, out sNewPartID, out sNewItem,out sNewPartSN))
                //{
                //    editNewKPSN.Focus();
                //    editNewKPSN.SelectAll();
                //    return;
                //}

                //if (ClientUtils.ShowMessage(SajetCommon.SetLanguage("Replace Keyparts ?",1) + Environment.NewLine
                //                   + SajetCommon.SetLanguage("Keypart SN",1)+" : " + sKPSN + Environment.NewLine
                //                   + SajetCommon.SetLanguage("New Keypart SN", 1) + " : " + sNewPartSN,2) != DialogResult.Yes)
                //{
                //    return;
                //}

                bTransOK = Replace_KP(sKPNO, sKPSN, sKPID, sKPID, sItemGroup, editNewKPSN.Text);
                if (bTransOK)
                {
                    editNewKPSN.Text = "";
                    RichTextRemark.Text = "";
                }
            }
            else if (g_sType == "REMOVE")
            {
                if (ClientUtils.ShowMessage(SajetCommon.SetLanguage("Remove Keyparts ?", 1) + Environment.NewLine
                                   + SajetCommon.SetLanguage("Keypart SN", 1) + " : " + sKPSN + Environment.NewLine
                                   + SajetCommon.SetLanguage("Keypart No", 1) + " : " + sKPNO, 2) != DialogResult.Yes)
                {
                    return;
                }
                bTransOK = Remove_KP(sKPSN, sKPNO, sKPID);
            }
            Show_KP(LabSN.Text);
            if (bTransOK)
            {
                LVEC.Items.Clear();
                editDefect.Text = "";
                DialogResult = DialogResult.OK;
            }
        }

        public bool Check_NewKPSN(string sOLDKPSN, string sSN, string sWO, string sPartID)
        {
            string sKPSN = sOLDKPSN;
            // LVKP.SelectedItems[0].SubItems[1].Text;             

            //====SAJET.SJ_REPAIR_CHK_NEWKP  
            try
            {
                object[][] Params = new object[8][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_NEW_KPSN", editNewKPSN.Text };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_OLD_KPSN", sKPSN };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_SN", sSN };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_WO", sWO };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_EMP", ClientUtils.UserNO };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_TERMINAL_ID", g_sTerminalID };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_PART_ID", sPartID };
                Params[7] = new object[] { ParameterDirection.Output, OracleType.VarChar, "O_RESULT", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_REPAIR_CHK_NEWKP", Params);

                string sRes = ds.Tables[0].Rows[0]["O_RESULT"].ToString();
                if (sRes != "OK")
                {
                    ClientUtils.ShowMessage(sRes, 0);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ClientUtils.ShowMessage(ex.Message, 0);
                return false;
            }
            return true;
        }

        public bool Check_RepairPart(string sKPSN, string sKPID, string sItemGroup, string sProcessID,
            out string sU_PARTID, out string sU_ITEM, out string sU_PartSN)
        {


            sU_PARTID = "0";
            sU_ITEM = "0";
            sU_PartSN = "N/A";

            //====SAJET.SJ_REPAIR_PART 
            try
            {
                object[][] Params = new object[11][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPROCESSID", sProcessID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREV", editNewKPSN.Text };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", LabSN.Text };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TKPSN", sKPSN };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TKPID", sKPID };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMG", sItemGroup };
                Params[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                Params[7] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TPASS", "" };
                Params[8] = new object[] { ParameterDirection.Output, OracleType.VarChar, "U_PARTID", "" };
                Params[9] = new object[] { ParameterDirection.Output, OracleType.VarChar, "U_ITEM", "" };
                Params[10] = new object[] { ParameterDirection.Output, OracleType.VarChar, "U_PARTSN", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_REPAIR_PART", Params);

                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                string sU_Pass = ds.Tables[0].Rows[0]["TPASS"].ToString();
                sU_PARTID = ds.Tables[0].Rows[0]["U_PARTID"].ToString();
                sU_ITEM = ds.Tables[0].Rows[0]["U_ITEM"].ToString();
                sU_PartSN = ds.Tables[0].Rows[0]["U_PARTSN"].ToString();

                if (sRes != "OK" & sU_Pass != "1")
                {
                    ClientUtils.ShowMessage(sRes, 0);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ClientUtils.ShowMessage(ex.Message, 0);
                return false;
            }
            return true;
        }

        public bool Replace_KP(string sOldKPNO, string sOldKPSN, string sOldPartID, string sNewPartID, string sNewItem, string sNewPartSN)
        {
            string sKPFlag = "N";
            string sKPDefectData = "";
            if (rdbtnYes.Checked)
            {
                sKPFlag = "Y";
                for (int i = 0; i <= LVEC.Items.Count - 1; i++)
                    sKPDefectData = sKPDefectData + LVEC.Items[i].SubItems[2].Text + "@"
                                                  + LVEC.Items[i].SubItems[0].Text + "@";
            }
            if (sKPDefectData == "")
                sKPDefectData = "N/A";

            //====SAJET.SJ_REPAIR_REPLACE_KP 
            try
            {
                object[][] Params = new object[13][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTERMINALID", RepairUtility.sTerminalID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", LabSN.Text };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TDEFECT_RECID", g_sDefectRECID };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOLD_KPSN", sOldKPSN };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOLD_PARTID", sOldPartID };
                //-     if (sOldKPSN == "N/A")
                //          Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEW_KPSN", "N/A" };
                //      else
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEW_KPSN", sNewPartSN };
                if (sNewPartID == "" || sNewPartID == "0")
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEW_PARTID", sOldPartID };
                else
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEW_PARTID", sNewPartID };

                Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEW_ITEMGROUP", sNewItem };
                Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TKPFLAG", sKPFlag };
                Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TKPDEFECT_DATA", sKPDefectData };
                Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREMARK", RichTextRemark.Text };
                Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", RepairUtility.sUserID };

                Params[12] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_REPAIR_REPLACE_KP", Params);

                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
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

        public bool Remove_KP(string sKPSN, string sKPNO, string sPartID)
        {
            string sKPFlag = "N";
            string sKPDefectData = "";
            if (rdbtnYes.Checked)
            {
                sKPFlag = "Y";
                for (int i = 0; i <= LVEC.Items.Count - 1; i++)
                    sKPDefectData = sKPDefectData + LVEC.Items[i].SubItems[2].Text + "@"
                                                  + LVEC.Items[i].SubItems[0].Text + "@";
            }
            if (sKPDefectData == "")
                sKPDefectData = "N/A";

            //====SAJET.SJ_REPAIR_REMOVE_KP
            try
            {
                object[][] Params = new object[9][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTERMINALID", RepairUtility.sTerminalID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", LabSN.Text };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TDEFECT_RECID", g_sDefectRECID };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TKPSN", sKPSN };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPARTID", sPartID };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TKPFLAG", sKPFlag };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TKPDEFECT_DATA", sKPDefectData };
                Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", RepairUtility.sUserID };
                Params[8] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_REPAIR_REMOVE_KP", Params);
                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
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

        private void LVKP_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnSearchDefect_Click(object sender, EventArgs e)
        {
            sSQL = " Select Defect_Code,Defect_Desc,DEFECT_DESC2 "
                 + " From SAJET.SYS_DEFECT "
                 + " Where Enabled='Y' "
                 + " Order By Defect_Code ";
            fFilter fFr = new fFilter();
            fFr.sSQL = sSQL;
            if (fFr.ShowDialog() == DialogResult.OK)
            {
                editDefect.Text = fFr.dgvData.CurrentRow.Cells["Defect_Code"].Value.ToString();
                KeyPressEventArgs sKey = new KeyPressEventArgs((char)Keys.Return);
                editDefect_KeyPress(editDefect, sKey);
            }
            fFr.Dispose();
            DialogResult = DialogResult.None; //若不加此行,選完Defect後就把Replace畫面關掉
        }

        private void gbNewKP_Enter(object sender, EventArgs e)
        {

        }

        private void dgvKP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Part No 不可以輸入Defct

            if (dgvKP.Rows.Count == 0)
                return;

            if (dgvKP.Rows.Count == 0 || dgvKP.CurrentRow == null)
                return;

            rdbtnYes.Enabled = true;
            editDefect.Enabled = true;
            //            rdbtnNo.Checked = true;
            string sKPSN = dgvKP.CurrentRow.Cells["ITEM_PART_SN"].Value.ToString();
            if (sKPSN == "N/A")
            {
                rdbtnYes.Enabled = false;
                editDefect.Enabled = false;
                rdbtnNo.Checked = true;
            }
            if (editNewKPSN.Visible)
                editNewKPSN.Focus();

        }
    }
}