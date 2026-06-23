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
    public partial class fReplaceItem : Form
    {
        public fReplaceItem()
        {
            InitializeComponent();
        }
        public string g_sLoc;
        public string g_sItemNo;
        //public string g_sUserID;
        public string g_sDefectRECID;
        //public string g_sRTerminalID;
        //public int g_iLocateItem;
        public string g_sLotNo;
        public string g_sVendorCode;
        public string g_sDateCode;
        public bool g_bBGAFlag;
        public string g_sBGAType;
        public string g_sMaterialInTime;
        public string g_sReelNo;
        public string g_sWO;
        public string g_sSN;
        bool g_bReelInput;
        string sSQL;
        DataSet dsTemp;

        private void fReplaceItem_Load(object sender, EventArgs e)
        {
            dtMaterialDate.Value = DateTime.Now;
            string sMsg = string.Empty;
            chkbBGA.Checked = (SajetCommon.GetSysBaseData(ClientUtils.fProgramName, "BGA Checked", ref sMsg) == "Y");
            chkbBGA.Enabled = (!chkbBGA.Checked);

            ClientUtils.SetLanguage(this, fMain.g_sExeName);

            editLC.Enabled = true;
            editItemNo.Enabled = true;
            btnSearchKP.Visible = true;
                        /*
            switch (g_iLocateItem)
            {
                case 1://Location一定要輸入
                    LabLC.ForeColor = Color.Red;
                    editLC.Enabled = false;
                    break;
                case 2://Item一定要輸入
                    LabItem.ForeColor = Color.Red;
                    editItemNo.Enabled = false;
                    btnSearchKP.Visible = false;
                    break;
                case 4://二者都要輸入
                    LabLC.ForeColor = Color.Red;
                    LabItem.ForeColor = Color.Red;
                    editItemNo.Enabled = false;
                    btnSearchKP.Visible = false;
                    editLC.Enabled = false;
                    break;
            }
             */ 
            editLC.Text = g_sLoc;
            if (!string.IsNullOrEmpty(editLC.Text))
            {
                editLC.Enabled = false;
            }

            editItemNo.Text = g_sItemNo;
            if (!string.IsNullOrEmpty(editItemNo.Text))
            {
                editItemNo.Enabled = false;
                btnSearchKP.Visible = false;
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editItemNo_KeyPress(sender, Key);
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
        private bool CheckVendor(string sVendorCode, ref string sVendorName)
        {

            sSQL = " Select VENDOR_CODE,VENDOR_NAME,ENABLED "
                 + " From SAJET.SYS_VENDOR "
                 + "  WHERE VENDOR_CODE =:VENDOR_CODE "
                 + "   AND ROWNUM = 1 ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_CODE", sVendorCode };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                ClientUtils.ShowMessage("Vendor Code Error", 0);
                return false;
            }
            DataRow dr = dsTemp.Tables[0].Rows[0];
            if (dr["ENABLED"].ToString() != "Y")
            {
                ClientUtils.ShowMessage("Vendor Code is Invalid", 0);
                return false;
            }
            sVendorName = dr["VENDOR_NAME"].ToString();
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            editLC.Text = editLC.Text.Trim();
            editItemNo.Text = editItemNo.Text.Trim();
            editDateCode.Text = editDateCode.Text.Trim();
            editLotNo.Text = editLotNo.Text.Trim();
            editVendorCode.Text = editVendorCode.Text.Trim();
            editReelNo.Text = editReelNo.Text.Trim();
            switch (RepairUtility.iLocationParams)
            {
                case 1://Location一定要輸入
                    if (editLC.Text == "")
                    {
                        ClientUtils.ShowMessage("Please input Location", 1);
                        editLC.Focus();
                        editLC.SelectAll();
                        return;
                    }
                    break;
                case 2://Item一定要輸入
                    if (editItemNo.Text == "")
                    {
                        ClientUtils.ShowMessage("Please input Item No", 1);
                        editItemNo.Focus();
                        editItemNo.SelectAll();
                        return;
                    }
                    break;
                case 3://二者要輸入一個
                    if (editLC.Text == "" && editItemNo.Text == "")
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
                    if (editItemNo.Text == "")
                    {
                        ClientUtils.ShowMessage("Please input Item No", 1);
                        editItemNo.Focus();
                        return;
                    }
                    break;
            }
            string sItemPartID = "0";
            string sItemSpec = string.Empty;
            if (!string.IsNullOrEmpty(editItemNo.Text))
            {
                if (!CheckItem(editItemNo.Text, ref sItemPartID, ref sItemSpec))
                {
                    editItemNo.Focus();
                    editItemNo.SelectAll();
                    return;
                }
            }
            if (!string.IsNullOrEmpty(editReelNo.Text))
            {
                if (!g_bReelInput)
                {
                    KeyPressEventArgs sKey = new KeyPressEventArgs((char)Keys.Return);
                    editReelNo_KeyPress(sender, sKey);
                    if (!g_bReelInput)
                        return;
                }

            }
            if (!string.IsNullOrEmpty(editVendorCode.Text))
            {
                string sVendorName = string.Empty;
                if (!CheckVendor(editVendorCode.Text, ref sVendorName))
                {
                    editVendorCode.Focus();
                    editVendorCode.SelectAll();
                    return;
                }
            }
            if (!string.IsNullOrEmpty(editReelNo.Text))
            {
               sSQL = "SELECT A.DATECODE,A.LOT,TO_CHAR(A.RT_RECEIVE_TIME,'YYYY/MM/DD') RT_RECEIVE_TIME "
                   + "      ,B.VENDOR_CODE,B.VENDOR_NAME "
                   + "      ,C.PART_NO,C.SPEC1 "
                   + "  FROM SAJET.G_MATERIAL A "
                   + "      ,SAJET.SYS_VENDOR B "
                   + "      ,SAJET.SYS_PART C "
                   + " WHERE A.REEL_NO =:REEL_NO "
                   + "   AND A.VENDOR_ID =B.VENDOR_ID(+) "
                   + "   AND A.PART_ID = C.PART_ID(+) ";
               object[][] Params = new object[1][];
               Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REEL_NO", editReelNo.Text };
               dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
               if (dsTemp.Tables[0].Rows.Count == 0)
               {
                    ClientUtils.ShowMessage("Reel No Error", 0);
                   editReelNo.Focus();
                   editReelNo.SelectAll();
               }
               else
               {
                   lablReelPartNo.Text = dsTemp.Tables[0].Rows[0]["PART_NO"].ToString();
                   lablReelPartSpec.Text = dsTemp.Tables[0].Rows[0]["SPEC1"].ToString();
               }

            }
            //傳回前一個畫面做處理
            g_sItemNo = editItemNo.Text;
            if (!string.IsNullOrEmpty(lablReelPartNo.Text))
            {
                g_sItemNo = lablReelPartNo.Text;
            }
            g_sLoc = editLC.Text;
            g_sDateCode = editDateCode.Text;
            g_sLotNo = editLotNo.Text;
            g_sVendorCode = editVendorCode.Text;
            g_bBGAFlag = chkbBGA.Checked;
            
            g_sReelNo = editReelNo.Text;
            g_sMaterialInTime = "N/A";
            if (chkbDate.Checked)
                g_sMaterialInTime = dtMaterialDate.Value.ToString("yyyy/MM/dd");
            g_sBGAType = "N/A";
            if (g_bBGAFlag)
            {
                if (combBGAType.SelectedIndex <= 0)
                {
                    ClientUtils.ShowMessage("Please Select BGA Operate Type", 0);
                    combBGAType.Focus();
                    return;
                }
                g_sBGAType = combBGAType.Text;
            }
            DialogResult = DialogResult.OK;
        }

        private void btnFilterVendor_Click(object sender, EventArgs e)
        {
            editVendorCode.Text = editVendorCode.Text.Trim();
            sSQL = " Select VENDOR_CODE,VENDOR_NAME "
                 + " From SAJET.SYS_VENDOR "
                 + " Where Enabled='Y' "
                 + "   AND VENDOR_CODE LIKE '" + editVendorCode.Text + "%' " 
                 + " Order By VENDOR_CODE ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editVendorCode.Text = f.dgvData.CurrentRow.Cells["VENDOR_CODE"].Value.ToString();
                KeyPressEventArgs sKey = new KeyPressEventArgs((char)Keys.Return);
                editVendorCode_KeyPress(sender, sKey);
            }
            f.Dispose();
        }

        private void btnSearchKP_Click(object sender, EventArgs e)
        {
            editItemNo.Text = editItemNo.Text.Trim();

            if (editItemNo.Text.Length < 5)
            {
                ClientUtils.ShowMessage(SajetCommon.SetLanguage("Please Input Part No Prefix(5 char)"), 0);
                return;
            }
            sSQL = " Select Part_No,SPEC1,SPEC2 "
                 + " From SAJET.SYS_PART"
                 + " Where Enabled='Y' "
                 + " and PART_NO Like '" + editItemNo.Text + "%'"
                 + " Order By Part_No ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;

            if (f.ShowDialog() == DialogResult.OK)
            {
                editItemNo.Text = f.dgvData.CurrentRow.Cells["PART_NO"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editItemNo_KeyPress(sender, Key);
            }
            f.Dispose();
        }

        private void editItemNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            lablItemSpec.Text = string.Empty;
            editItemNo.Text = editItemNo.Text.Trim();

            string sItemPartID = "0";
            string sItemSpec = string.Empty;
            if (!CheckItem(editItemNo.Text, ref sItemPartID, ref sItemSpec))
            {
                editItemNo.Focus();
                editItemNo.SelectAll();
                return;
            }
            lablItemSpec.Text = sItemSpec;
            editReelNo.Focus();
            editReelNo.SelectAll();
        }

        private void editItemNo_TextChanged(object sender, EventArgs e)
        {
            lablItemSpec.Text = string.Empty;
        }

        private void editVendorCode_TextChanged(object sender, EventArgs e)
        {
            lablVendorName.Text = string.Empty;
        }

        private void editVendorCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            lablVendorName.Text = string.Empty;
            editVendorCode.Text = editVendorCode.Text.Trim();
            if (string.IsNullOrEmpty(editVendorCode.Text))
                return;
            string sVendorName = string.Empty;
            if (!CheckVendor(editVendorCode.Text, ref sVendorName))
            {
                editVendorCode.Focus();
                editVendorCode.SelectAll();
                return;
            }
            lablVendorName.Text = sVendorName;
        }

        private void editLC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editItemNo.Focus();
            editItemNo.SelectAll();
        }

        private void editDateCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editLotNo.Focus();
            editLotNo.SelectAll();
        }

        private void editLotNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                    return;
            editVendorCode.Focus();
            editVendorCode.SelectAll();
        }

        private void fReplaceItem_Shown(object sender, EventArgs e)
        {
            editLC.Focus();
        }

        private void chkbBGA_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private bool CheckPartID(string sPartNo, ref string sPartID)
        {
            sSQL = " SELECT PART_ID FROM SAJET.SYS_PART "
                    + " WHERE PART_NO = :PART_NO "
                    + " AND ROWNUM = 1 ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", sPartNo };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            sPartID = dsTemp.Tables[0].Rows[0]["PART_ID"].ToString();
            return true;
        }
        private bool GetMaterialPartID(string sReelNo, ref string sReelPartID)
        {
            sSQL = " SELECT B.PART_ID FROM SAJET.G_MATERIAL A "
                    + " ,SAJET.SYS_PART B "
                    + " WHERE A.REEL_NO = :REEL_NO "
                    + " AND A.PART_ID = B.PART_ID "
                    + " AND ROWNUM = 1 ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REEL_NO", sReelNo };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            sReelPartID = dsTemp.Tables[0].Rows[0]["PART_ID"].ToString();
            return true;
        }
        private bool CheckReelInBom(string sSN, string sWO, string sItemPartNo, string sReelNo, ref string sMsg)
        {
            string sItemPartID = "";
            string sReelPartID = "";
            string sGroupID = "";
            if (!CheckPartID(sItemPartNo, ref sItemPartID))
            {
                sMsg = "Part ID Error";
                return false;
            }

            if (!GetMaterialPartID(sReelNo, ref sReelPartID))
            {
                sMsg = "Reel No Error";
                return false;
            }

            object[][] Params = new object[2][];
            sSQL = " SELECT C.PART_NO,B.ITEM_GROUP "
                     + " FROM SAJET.SYS_BOM_INFO A "
                     + " ,SAJET.SYS_BOM B "
                     + " ,SAJET.SYS_PART C "
                     + " ,SAJET.G_SN_TRAVEL D "
                     + " WHERE D.SERIAL_NUMBER = :SERIAL_NUMBER "
                     + " AND B.ITEM_PART_ID = :ITEM_PART_ID "
                     + " AND A.PART_ID =D.PART_ID "
                     + " AND A.VERSION = D.VERSION "
                     + " AND A.BOM_ID = B.BOM_ID "
                     + " AND B.ITEM_PART_ID = C.PART_ID "
                     + " GROUP BY C.PART_NO,B.ITEM_GROUP  ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", sSN };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                //檢查此料是否為替代料
                if (dsTemp.Tables[0].Rows[0]["ITEM_GROUP"].ToString().Trim() == "0")
                {
                    if (sItemPartID.ToString().Trim() == sReelPartID.ToString().Trim())
                        return true;
                    else
                    {
                        sMsg = "Item No Do Not Have Replace Item";
                        return false;
                    }
                }
                else
                {
                    sGroupID = dsTemp.Tables[0].Rows[0]["ITEM_GROUP"].ToString().Trim();
                    sSQL = " SELECT C.PART_NO,B.ITEM_GROUP "
                            + " FROM SAJET.SYS_BOM_INFO A "
                            + " ,SAJET.SYS_BOM B "
                            + " ,SAJET.SYS_PART C "
                            + " ,SAJET.G_SN_TRAVEL D "
                            + " WHERE D.SERIAL_NUMBER = :SERIAL_NUMBER "
                            + " AND B.ITEM_PART_ID = :ITEM_PART_ID "
                            + " AND B.ITEM_GROUP = :ITEM_GROUP "
                            + " AND A.PART_ID =D.PART_ID "
                            + " AND A.VERSION = D.VERSION "
                            + " AND A.BOM_ID = B.BOM_ID "
                            + " AND B.ITEM_PART_ID = C.PART_ID "
                            + " GROUP BY C.PART_NO,B.ITEM_GROUP  ";
                    Params = new object[3][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", sSN };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sReelPartID.ToString().Trim() };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_GROUP", sGroupID.ToString().Trim() };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                    if (dsTemp.Tables[0].Rows.Count == 0)
                    {
                        sMsg = "Reel No Can Not Replace";
                        return false;
                    }
                    else
                        return true;
                }
            }
            else
            {
                if (sItemPartID != sReelPartID)
                {
                    string sPartNo = SajetCommon.GetID("SAJET.SYS_PART", "PART_NO", "PART_ID", sReelPartID);

                    sMsg = SajetCommon.SetLanguage("Reel No") + " : " + sReelNo + Environment.NewLine
                         + SajetCommon.SetLanguage("Part No") + " : " + sPartNo + Environment.NewLine
                         + SajetCommon.SetLanguage("Part Not Match");
                    return false;
                }
                return true;
            }


        }
        private void editReelNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editReelNo.Text = editReelNo.Text.Trim();
            lablReelPartSpec.Text = "";
            lablReelPartNo.Text = "";
            if (string.IsNullOrEmpty(editReelNo.Text))
            {
                editDateCode.Focus();
                editDateCode.SelectAll();
                return;
            }

            string sMsg = "";
            if (!string.IsNullOrEmpty(editItemNo.Text)) //若有輸入要替換的零件料號,才要檢查BOM表
            {
                if (!CheckReelInBom(g_sSN, g_sWO, editItemNo.Text.Trim(), editReelNo.Text.Trim(), ref sMsg))
                {
                    ClientUtils.ShowMessage(sMsg, 0);
                    editReelNo.Focus();
                    editReelNo.SelectAll();
                    return;
                }
            }

            sSQL = "SELECT A.DATECODE,A.LOT,TO_CHAR(A.RT_RECEIVE_TIME,'YYYY/MM/DD') RT_RECEIVE_TIME "
                + "      ,B.VENDOR_CODE,B.VENDOR_NAME "
                + "      ,C.PART_NO,C.SPEC1 "
                + "  FROM SAJET.G_MATERIAL A "
                + "      ,SAJET.SYS_VENDOR B "
                + "      ,SAJET.SYS_PART C "
                + " WHERE A.REEL_NO =:REEL_NO "
                + "   AND A.VENDOR_ID =B.VENDOR_ID(+) "
                + "   AND A.PART_ID = C.PART_ID(+) ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REEL_NO", editReelNo.Text };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                g_bReelInput = true;
                editDateCode.Text = dsTemp.Tables[0].Rows[0]["DATECODE"].ToString();
                editLotNo.Text = dsTemp.Tables[0].Rows[0]["LOT"].ToString();
                editVendorCode.Text = dsTemp.Tables[0].Rows[0]["VENDOR_CODE"].ToString();
                dtMaterialDate.Text = dsTemp.Tables[0].Rows[0]["RT_RECEIVE_TIME"].ToString();
                lablReelPartSpec.Text = dsTemp.Tables[0].Rows[0]["SPEC1"].ToString();
                lablReelPartNo.Text = dsTemp.Tables[0].Rows[0]["PART_NO"].ToString();
                chkbDate.Checked = true;
                editDateCode.Focus();
                editDateCode.SelectAll();
                KeyPressEventArgs sKey = new KeyPressEventArgs((char)Keys.Return);
                editVendorCode_KeyPress(sender, sKey);                
            }
            else
            {
                g_bReelInput = false;
                ClientUtils.ShowMessage("Reel No Error", 0);
                editReelNo.Focus();
                editReelNo.SelectAll();
            }
        }

        private void editReelNo_TextChanged(object sender, EventArgs e)
        {
            lablReelPartNo.Text = "";
            lablReelPartSpec.Text = "";
            g_bReelInput = false;
        }
    }
}
