using Excel;
using SajetClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Application = System.Windows.Forms.Application;


namespace WMSPick
{

    public partial class fMain : Form
    {
        #region 参数
        public string g_sProgram, g_sExeName, g_sFunction, g_sUserID, g_sUserNo;
        private DataSet dsTemp;
        private string sSQL;
        private string sFromCartonQty;
        private string sToCartonQty;
        private string sPartID;

        internal static string[] slParam = { "", "" };
        ArrayList alWHNo = new ArrayList();
        public bool g_bPopUp;
        #endregion
        public fMain()
        {
            InitializeComponent();
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            g_sProgram = ClientUtils.fProgramName;
            g_sExeName = ClientUtils.fCurrentProject;
            g_sFunction = ClientUtils.fFunctionName;
            g_sUserID = ClientUtils.UserPara1;
            g_sUserNo = ClientUtils.fLoginUser;
            string[] sParam = ClientUtils.fParameter.Split(';');


            for (int i = 0; i < sParam.Length; i++)
                slParam[i] = sParam[i];
            if (slParam[0] == "OUT")
            {
                btnToERP.Visible = false;
            }
            SajetCommon.SetLanguageControl(this);
            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";
            this.WindowState = FormWindowState.Maximized;
            SetDataGridViewColumesAutosize(dgv_PartNo);



            RefreshRequest();

        }

        private void RefreshRequest()
        {
            ClearData();
            combRequest.Text = "";
            combRequest.Items.Clear();
            string sSQL = "SELECT DISTINCT A.REQUEST_ID "
                            + "   FROM SAJET.WMS_PICK_INFO A,SAJET.WMS_WAREHOUSE B "
                            + "WHERE A.INTERFACE_TYPE = :INTERFACE_TYPE AND A.STATUS < 2 "
                            + "  AND A.WAREHOUSE_ID=B.WAREHOUSE_ID "
                            + "    AND (A.GROUP_ID = :GROUP_ID OR A.GROUP_ID = 'N/A' OR A.GROUP_ID IS NULL)  ORDER BY A.REQUEST_ID  ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { "INPUT", "1", "INTERFACE_TYPE", slParam[0] };
            Params[1] = new object[] { "INPUT", "1", "GROUP_ID", g_sUserID };
            DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params);
            foreach (DataRow dr in ds.Tables[0].Rows)
                combRequest.Items.Add(dr["REQUEST_ID"].ToString());
            ds.Dispose();
        }


        #region 清除数据
        private void ClearData()
        {
            this.dgv_PartNo.Rows.Clear();
            lablMsg.Text = "";
            txtData.Text = "";

            combRequest.Focus();


        }
        #endregion

        #region 列宽自适应
        /// <summary>
        /// 设置DataGridView列宽自适应
        /// </summary>
        /// <param name="dgv">DataGridView</param>
        private void SetDataGridViewColumesAutosize(DataGridView dgv)
        {

            foreach (DataGridViewColumn dgvCol in dgv.Columns)
            {
                dgvCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }
        #endregion


        #region RollBack
        private void rollBack(string sMixID)
        {
            try
            {
                sSQL = " DELETE SAJET.APOLLO_PACK_MIX T WHERE T.MIX_ID = '" + sMixID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
                sSQL = " DELETE SAJET.APOLLO_PACK_MIX_DETAIL T WHERE T.MIX_ID = '" + sMixID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
            }
            catch (System.Exception ex)
            {
                SajetCommon.Show_Message(sSQL + Environment.NewLine + ex.Message, 0);
            }
        }
        #endregion


        private void RefreshData()
        {
            dgv_PartNo.Rows.Clear();
            //dgv_BarcodeDetail.Rows.Clear();
            bool bFinish = true;



            string sSQL = $@"SELECT A.PART_ID, B.PART_NO, B.SPEC1, C.WAREHOUSE_NO, 
                                       SUM(QTY) QTY, SUM(PICK_QTY) PICK_QTY, SUM(QTY - PICK_QTY) UNPICK_QTY ,A.STATUS
                            FROM SAJET.WMS_PICK_INFO A, SAJET.SYS_PART B, SAJET.WMS_WAREHOUSE C 
                            WHERE A.REQUEST_ID = '{combRequest.Text}'
                            AND A.PART_ID = B.PART_ID 
                            AND INTERFACE_TYPE = '{slParam[0]}'
                            AND A.WAREHOUSE_ID = C.WAREHOUSE_ID(+) 


                            AND (GROUP_ID = {g_sUserID} OR GROUP_ID = 'N/A' OR GROUP_ID IS NULL) 
                            GROUP BY A.PART_ID, B.PART_NO, B.SPEC1, C.WAREHOUSE_NO ,A.STATUS
                            ORDER BY PICK_QTY "; //优先显示发料数量少的那笔


            //object[][] Params = new object[3][];
            //Params[0] = new object[] { "INPUT", "1", "TPARAM", slParam[0] };
            //Params[1] = new object[] { "INPUT", "1", "REQUEST_ID", combRequest.Text };
            //Params[2] = new object[] { "INPUT", "1", "GROUP_ID", g_sUserID };
            //DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params);
            DataSet ds = ClientUtils.ExecuteSQL(sSQL);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dgv_PartNo.Rows.Insert(i, 1);
                dgv_PartNo.Rows[i].Cells[0].Value = ds.Tables[0].Rows[i]["PART_NO"].ToString().Trim();
                dgv_PartNo.Rows[i].Cells[1].Value = ds.Tables[0].Rows[i]["WAREHOUSE_NO"].ToString().Trim();
                dgv_PartNo.Rows[i].Cells[2].Value = ds.Tables[0].Rows[i]["QTY"].ToString().Trim();
                dgv_PartNo.Rows[i].Cells[3].Value = ds.Tables[0].Rows[i]["PICK_QTY"].ToString().Trim();
                dgv_PartNo.Rows[i].Cells[4].Value = ds.Tables[0].Rows[i]["UNPICK_QTY"].ToString().Trim();
                dgv_PartNo.Rows[i].Cells[5].Value = ds.Tables[0].Rows[i]["SPEC1"].ToString().Trim();

                dgv_PartNo.Rows[i].DefaultCellStyle.ForeColor = (ds.Tables[0].Rows[i]["STATUS"].ToString() == "1") ? Color.Red : Color.Black;

                btnToERP.Enabled = (ds.Tables[0].Rows[i]["STATUS"].ToString() == "1") ? false : true;

                gv_location.Enabled = (ds.Tables[0].Rows[i]["STATUS"].ToString() == "1") ? false : true;


                if (decimal.TryParse(ds.Tables[0].Rows[i]["UNPICK_QTY"]?.ToString(), out decimal qty) && qty <= 0)
                {
                    dgv_PartNo.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                }

            }

            if (txtData.Text == "")
            {
                Show_Message(SajetCommon.SetLanguage("OK"), 3);
            }
            else
            {
                Show_Message(txtData.Text + SajetCommon.SetLanguage("INV OK"), 3);
            }

            //2014.12.26 修改结单方式
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    if (ds.Tables[0].Rows[i]["UNPICK_QTY"].ToString() != "0")
            //    {
            //        bFinish = false;
            //        break;
            //    }
            //}
            //txtData.Text = "";
            txtData.Focus();
            txtData.SelectAll();
            //if (bFinish)
            //    ConfirmRequest();

            ds.Dispose();
        }

        private void ConfirmRequest()
        {
            object[][] Params;
            string sRes = "";

            //Params = new object[4][];
            //Params[0] = new object[] { "INPUT", "1", "TREQUEST", combRequest.Text };
            //Params[1] = new object[] { "INPUT", "1", "TPARAM", slParam[0] };
            //Params[2] = new object[] { "INPUT", "1", "TEMPID", ClientUtils.fUserID };
            //Params[3] = new object[] { "OUTPUT", "1", "TRES", "" };
            //DataSet ds = ClientUtils.ExecuteProc("SAJET.WMS_STOCK_CONFIRM", Params);
            //sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
            //if (sRes == "OK")
            //{
            combRequest.Text = "";
            RefreshRequest();
            ClearData();
            combRequest.Focus();
            //ClientUtils.ShowMessage("Finish!", 3);
            ///lablMsg.Text = ClientUtils.SetLanguage("Finish!");
            Show_Message(SajetCommon.SetLanguage("Finish!"), 3);
            //}
            //else
            //    ///lablMsg.Text = ClientUtils.SetLanguage(sRes);
            //    Show_Message(ClientUtils.SetLanguage(sRes), 0);
            //    //ClientUtils.ShowMessage(sRes, 0);
            //ds.Dispose();
        }

        private void combRequest_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();

            if (combRequest.SelectedIndex > -1)
            {
                RefreshData();
                //txtLocation.Focus();
                SearchLocation();
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            if (combRequest.Items.IndexOf(combRequest.Text) == -1)
            {

                Show_Message(SajetCommon.SetLanguage("RECEIPT ERROR"), 0);
                combRequest.Focus();
                return;
            }
            try
            {
                DialogResult dr = MessageBox.Show(SajetCommon.SetLanguage("Confirm finish Request？"), SajetCommon.SetLanguage("Warning"), MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.OK)
                {
                    object[][] Paramt = new object[4][];
                    Paramt[0] = new object[] { "INPUT", "1", "TPARAM", slParam[0] };
                    Paramt[1] = new object[] { "INPUT", "1", "TREQUEST", combRequest.Text };
                    Paramt[2] = new object[] { "INPUT", "1", "TEMPID", g_sUserID };
                    Paramt[3] = new object[] { "OUTPUT", "1", "TRES", "" };

                    DataSet dt = ClientUtils.ExecuteProc("SAJET.SJ_PICK_FINISH", Paramt);
                    string sRet = dt.Tables[0].Rows[0]["TRES"].ToString();

                    if (sRet.Substring(0, 2) != "OK")
                    {
                        Show_Message((SajetCommon.SetLanguage(sRet)), 0);
                        return;
                    }

                    else
                    {
                        txtData.Text = "";
                        RefreshRequest();
                        Show_Message(SajetCommon.SetLanguage("Finish Request OK"), 3);
                        combRequest.Focus();

                    }
                    dt.Dispose();
                }
            }
            catch (Exception ex)
            {
                Show_Message(ex.Message.ToString(), 0);
            }

        }

        private void txtData_Click(object sender, EventArgs e)
        {
            txtData.SelectAll();
        }

        private void btnToERP_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(combRequest.Text))
                {
                    return;
                }
                var Params = new object[4][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREQUESTID", combRequest.Text };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTABLE", "SAJET.WMS_PICK_INFO" };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", g_sUserID };
                Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_DETAIL_TO_ERP", Params);

                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes.StartsWith("OK"))
                {
                    Show_Message(SajetCommon.SetLanguage("To ERP OK"), 3);
                }
                else
                {
                    Show_Message(SajetCommon.SetLanguage(sRes), 0);
                }

                ds.Dispose();
                txtData.SelectAll();

            }
            catch (Exception ex)
            {
                Show_Message(ex.Message, 0);
                txtData.SelectAll();
            }
        }

        private void dgv_PartNo_SelectionChanged(object sender, EventArgs e)
        {

            SearchLocation();

        }

        private void SearchLocation() 
        {
            var _CurrentRow = dgv_PartNo.CurrentRow;


            if (dgv_PartNo != null && _CurrentRow != null)
            {
                var _PART_NO = _CurrentRow.Cells[0].Value;

                if (_PART_NO != null && _PART_NO != DBNull.Value)
                {

                    //客戶要能超發 ~~ 20260616 by Jim
                    //gv_location.Enabled = (_CurrentRow.DefaultCellStyle.BackColor == Color.Green) ? false : true;

                    //var _SQL = $@"
                    //WITH NODE AS 
                    //(
                    //    SELECT TB3.PART_NO,TB3.PART_ID ,TB1.BOX_NO,TB1.BOX_QTY, SUBSTR(TB2.LOCATION_NO, INSTR(TB2.LOCATION_NO, '-') + 1) LOCATION_NO,
                    //    DECODE(TB1.STATUS, '0', '在架', '下架') STATUS,TB1.DATECODE 
                    //    FROM  SAJET.WMS_STOCK TB1, SAJET.WMS_LOCATION TB2,SAJET.SYS_PART TB3 
                    //    WHERE TB1.LOCATION_ID = TB2.LOCATION_ID  AND TB1.PART_ID = TB3.PART_ID 
                    //)

                    //SELECT BOX_NO,BOX_QTY ,LOCATION_NO ,STATUS , DATECODE  FROM NODE TB1,SAJET.WMS_PICK_LIST TB2
                    //WHERE TB1.PART_ID = TB2.PART_ID AND TB2.REQUEST_ID = '{combRequest.Text}' AND PART_NO = '{_PART_NO}' ORDER BY PART_NO,DATECODE DESC
                    //";


                    //var _SQL = $@"
                    //SELECT TB3.PART_NO,
                    //TB1.BOX_NO,
                    //TB1.BOX_QTY,
                    //SUBSTR(TB2.LOCATION_NO, INSTR(TB2.LOCATION_NO, '-') + 1) LOCATION_NO,
                    //DECODE(TB1.STATUS, '0', '在架',
                    //'下架')                             STATUS,
                    //TB1.DATECODE
                    //FROM   SAJET.WMS_STOCK TB1,
                    //SAJET.WMS_LOCATION TB2,
                    //SAJET.SYS_PART TB3,
                    //SAJET.WMS_PICK_LIST TB4
                    //WHERE  TB4.REQUEST_ID = '{combRequest.Text}'
                    //AND TB3.PART_NO = '{_PART_NO}'
                    //AND TB1.LOCATION_ID = TB2.LOCATION_ID
                    //AND TB1.LOCATION_ID = TB4.LOCATION_ID
                    //AND TB1.DATECODE = TB4.DATECODE
                    //AND TB1.PART_ID = TB3.PART_ID
                    //            ";

                    var _SQL = $@"
                    SELECT TB2.BOX_NO,TB2.BOX_QTY,SUBSTR(TB3.LOCATION_NO, INSTR(TB3.LOCATION_NO, '-') + 1) LOCATION_NO,
                    DECODE(TB2.STATUS, '0', '在架', '下架') STATUS,
                    TB2.DATECODE,TB4.DC_FACTORY
                    FROM SAJET.WMS_PICK_INFO TB1,SAJET.WMS_STOCK TB2,
                    SAJET.WMS_LOCATION TB3,SAJET.WMS_LABEL TB4
                    WHERE TB1.PART_ID = (SELECT PART_ID FROM SAJET.SYS_PART WHERE PART_NO = '{_PART_NO}') 
                    AND TB1.PART_ID = TB2.PART_ID AND TB2.LOCATION_ID = TB3.LOCATION_ID 
                    AND TB2.BOX_NO = TB4.BOX_NO
                    AND  TB1.REQUEST_ID =  '{combRequest.Text}'
                    ";


                    var ds = ClientUtils.ExecuteSQL(_SQL);

                    gv_location.DataSource = ds.Tables[0];
                }

            }
        }

        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return) return;

            lablQty.Text = "";
            lablMsg.Text = "";

            if (combRequest.Items.IndexOf(combRequest.Text) == -1)
            {
                Show_Message(SajetCommon.SetLanguage("RECEIPT ERROR"), 0);
                combRequest.Focus();
                return;
            }

            txtData.Text = txtData.Text.Trim().ToUpper();
            string scannedValue = txtData.Text;

            // 收集 gv_location 中已勾選的 ReelID
            var checkedReelIDs = new List<string>();
            foreach (DataGridViewRow row in gv_location.Rows)
            {
                var cellValue = row.Cells["SELECT"].Value;
                if (cellValue != null && Convert.ToString(cellValue) == "Y")
                {
                    string reelID = row.Cells["ReelID"].Value?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(reelID))
                        checkedReelIDs.Add(reelID);
                }
            }

            if (checkedReelIDs.Count > 0)
            {
                // 有勾選清單時，驗證掃入值是否在清單中
                if (!checkedReelIDs.Contains(scannedValue))
                {
                    Show_Message(SajetCommon.SetLanguage("不在勾選清單中") + ": " + scannedValue, 0);
                    txtData.SelectAll();
                    return;
                }

                ProcessPick(scannedValue);

                // 取消該筆勾選
                foreach (DataGridViewRow row in gv_location.Rows)
                {
                    if (Convert.ToString(row.Cells["ReelID"].Value) == scannedValue)
                    {
                        row.Cells["SELECT"].Value = "N";
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("請勾選在庫物料");
            }

            txtData.SelectAll();
        }

        private void ProcessPick(string boxNo)
        {
            string sRes = "";
            try
            {
                object[][] Params = new object[8][];
                Params[0] = new object[] { "INPUT", "1", "TREV", boxNo };
                Params[1] = new object[] { "INPUT", "1", "TREQUEST", combRequest.Text };
                Params[2] = new object[] { "INPUT", "1", "TEMPID", g_sUserID };
                Params[3] = new object[] { "INPUT", "1", "TPARAM", slParam[0] };
                Params[4] = new object[] { "OUTPUT", "1", "TRES", "" };
                Params[5] = new object[] { "OUTPUT", "1", "TQTY", "" };
                Params[6] = new object[] { "OUTPUT", "1", "TPART", "" };
                Params[7] = new object[] { "OUTPUT", "1", "TUNPICK", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.WMS_PICK", Params);

                sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                // IndexOf 找不到時會回傳 -1，所以小於 0 就代表字串中沒有 "OK"
                if (sRes.IndexOf("OK") < 0)
                {
                    Show_Message(SajetCommon.SetLanguage(sRes) + ": " + boxNo, 0);
                }
                else
                {
                    if (sRes == "OK")
                    {
                        sRes = sRes + "-" + ds.Tables[0].Rows[0]["TPART"].ToString();
                        lablQty.Text = ds.Tables[0].Rows[0]["TQTY"].ToString();
                        RefreshData();
                        Show_Message(SajetCommon.SetLanguage(sRes) + ": " + boxNo, 3);
                    }
                    else
                    {
                        if (sRes.IndexOf("FINISHED") != -1)
                        {
                            txtData.Text = "";
                            RefreshRequest();
                            Show_Message(SajetCommon.SetLanguage(sRes), 3);
                        }
                    }
                }
                ds.Dispose();
            }
            catch (Exception ex)
            {
                sRes = ex.Message;
                Show_Message(sRes, 0);
            }
            finally 
            {
                SearchLocation();
            }
        }

        private void gv_location_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // 讓 checkbox 點擊後立即提交值
            if (gv_location.IsCurrentCellDirty && gv_location.CurrentCell is DataGridViewCheckBoxCell)
                gv_location.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshRequest();
        }



        public void PlayWev(string sWavFile)
        {
            if (File.Exists(Application.StartupPath + sWavFile))
            {
                SoundPlayer sound = new SoundPlayer();
                sound.SoundLocation = Application.StartupPath + sWavFile;
                sound.Play();
            }
        }

        public DialogResult Show_Message(string sText, int iType)
        {
            int ifreq = 800; int iduration = 200;
            lablMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.30189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            if (iType == 0)
            {
                PlayWev("\\NG.wav");
                if (!File.Exists(Application.StartupPath + "\\NG.wav"))
                {
                    Console.Beep(ifreq, iduration);
                }
            }
            else
            {
                PlayWev("\\OK.wav");
            }
            if (g_bPopUp)
            {
                switch (iType)
                {
                    case 0: //Error
                        //Console.Beep(ifreq, iduration);
                        return MessageBox.Show(sText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    case 1: //Warning
                        //Console.Beep(ifreq, iduration);
                        return MessageBox.Show(sText, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    case 2: //Confirm
                        return MessageBox.Show(sText, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    case 3: //OK
                        lablMsg.Text = sText;
                        lablMsg.ForeColor = Color.Green;
                        lablMsg.BackColor = Color.White;
                        return DialogResult.None;
                    default:
                        return MessageBox.Show(sText, "", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }
            else
            {
                lablMsg.Text = sText;
                switch (iType)
                {
                    case 0: //Error                        
                        lablMsg.ForeColor = Color.Red;
                        lablMsg.BackColor = Color.Silver;
                        //Console.Beep(ifreq, iduration);   
                        return DialogResult.None;
                    case 1: //Warning                        
                        lablMsg.ForeColor = Color.Blue;
                        lablMsg.BackColor = Color.FromArgb(255, 255, 128);

                        //Console.Beep(ifreq, iduration);
                        return DialogResult.None;
                    //case 2: //Confirm
                    //    return MessageBox.Show(sText, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    default:
                        lablMsg.ForeColor = Color.Green;
                        lablMsg.BackColor = Color.White;
                        return DialogResult.None;
                }
            }
        }

    }
}
