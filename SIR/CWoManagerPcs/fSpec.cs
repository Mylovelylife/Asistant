using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;
using System.IO;
using NPOI.SS.UserModel;

namespace CWoManagerPcs
{
    public partial class fSpec : Form
    {
        public string _WO = string.Empty;

        private string _SQL = string.Empty;

        private bool _Result = true;
        public fSpec()
        {
            InitializeComponent();
        }

        private void fSpec_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;

            // 初始化選項
            panelImportExcel.Visible = true;
            panelSpecCode.Visible = true;
            panelWorkOrder.Visible = true;



            GB.Text += $@"({_WO})";
        }

        private void rbtImportExcel_CheckedChanged(object sender, EventArgs e)
        {
            panelImportExcel.Visible = true;
            panelSpecCode.Visible = true;
            panelWorkOrder.Visible = true;
        }

        private void rbtSpecCode_CheckedChanged(object sender, EventArgs e)
        {
            panelImportExcel.Visible = true;
            panelSpecCode.Visible = true;
            panelWorkOrder.Visible = true;
        }

        private void rbtWorkOrder_CheckedChanged(object sender, EventArgs e)
        {
            panelImportExcel.Visible = true;
            panelSpecCode.Visible = true;
            panelWorkOrder.Visible = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xls;*.xlsx";
            ofd.Title = "Select Excel File";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtExcelPath.Text = ofd.FileName;
            }
        }


        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            // 1. 保留原本的防呆檢查
            if (string.IsNullOrEmpty(txtExcelPath.Text))
            {
                SajetCommon.Show_Message("Please select Excel file", 0);
                return;
            }

            if (!File.Exists(txtExcelPath.Text))
            {
                SajetCommon.Show_Message("File not exist", 0);
                return;
            }

            var _Result = true;
            // 2. 核心 NPOI 讀取與處理邏輯
            try
            {
                string filePath = txtExcelPath.Text.Trim();
                int iImported = 0;
                int iSkipped = 0;

                // 使用 FileStream 開啟檔案，FileShare.ReadWrite 可避免檔案被其他程式鎖定時出錯
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    IWorkbook wb;

                    // 自動判斷是新版副檔名 (.xlsx) 還是舊版 (.xls)
                    if (Path.GetExtension(filePath).ToLower() == ".xls")
                    {
                        wb = new NPOI.HSSF.UserModel.HSSFWorkbook(fs); // 處理 97-2003 格式
                    }
                    else
                    {
                        wb = new NPOI.XSSF.UserModel.XSSFWorkbook(fs); // 處理 2007 以上格式
                    }

                    ISheet ws = wb.GetSheetAt(0); // 取得第一個工作表 (NPOI 索引從 0 開始)

                    if (ws != null)
                    {
                        // LastRowNum 是最後一行的索引（從 0 開始）
                        int iRowCount = ws.LastRowNum;

                        // 從第 2 行開始（原本 Interop 的 i = 2，在 NPOI 索引中對應 i = 1，跳過標題）
                        for (int i = 1; i <= iRowCount; i++)
                        {
                            IRow row = ws.GetRow(i);
                            if (row == null) continue; // 略過空行

                            string sWorkOrder = "";
                            string sSerialNumber = "";

                            try
                            {
                                // 原本的 Cells[i, 1] (第一欄) 對應 NPOI 的 GetCell(0)
                                // 原本的 Cells[i, 2] (第二欄) 對應 NPOI 的 GetCell(1)
                                // ?. 與 ?? "" 語法可完美替代 ToString() 並防止 NullReferenceException
                                sWorkOrder = row.GetCell(0)?.ToString()?.Trim() ?? "";
                                sSerialNumber = row.GetCell(1)?.ToString()?.Trim() ?? "";
                            }
                            catch
                            {
                                continue; // 單列讀取失敗則跳過，繼續下一列
                            }

                            if (string.IsNullOrEmpty(sWorkOrder) && string.IsNullOrEmpty(sSerialNumber))
                                continue;


                            InsertSpecStatus(sWorkOrder, sSerialNumber, "");
                            iImported++;

                            //和Hunter討論後，決定不需要檢查SN  ~~ by Jim 20260615
                            // 檢查資料是否存在於 G_SN_STATUS
                            //if (CheckSNStatusExists(sWorkOrder, sSerialNumber))
                            //{

                            //}
                            //else
                            //{
                            //    iSkipped++;
                            //}
                        }
                    }
                } // 離開 using 區塊時，FileStream 會自動關閉，不佔用記憶體，也完全不需要手動 app.Quit()



                // 3. 保留原本的成功提示訊息
                SajetCommon.Show_Message("Import Success: " + iImported + " records", 3);
            }
            catch (Exception ex)
            {
                _Result = false;
                // 4. 保留原本的異常提示訊息
                SajetCommon.Show_Message("Import Error: " + ex.Message, 0);
            }
            finally 
            {
                if (_Result)
                {
                    if (!string.IsNullOrEmpty(_WO))
                    {
                        _SQL = $@"UPDATE SAJET.G_WO_BASE SET WO_OPTION2 = 1 WHERE WORK_ORDER = '{_WO}'";
                        ClientUtils.ExecuteSQL(_SQL);
                    }
                }
            }
        }

        private void btnGenerateSpec_Click(object sender, EventArgs e)
        {
            string sSpecCode = txtSpecCode.Text.Trim();
            string sStartNo = txtStartNo.Text.Trim();
            string sEndNo = txtEndNo.Text.Trim();

            if (string.IsNullOrEmpty(sSpecCode))
            {
                SajetCommon.Show_Message("Please enter Spec Code", 0);
                return;
            }

            if (string.IsNullOrEmpty(sStartNo) || string.IsNullOrEmpty(sEndNo))
            {
                SajetCommon.Show_Message("Please enter Start No and End No", 0);
                return;
            }

            // 解析起始和結束編號
            int iStart, iEnd;
            if (!int.TryParse(sStartNo, out iStart) || !int.TryParse(sEndNo, out iEnd))
            {
                SajetCommon.Show_Message("Start No and End No must be numeric", 0);
                return;
            }

            if (iStart > iEnd)
            {
                SajetCommon.Show_Message("Start No must <= End No", 0);
                return;
            }

            int iCount = iEnd - iStart + 1;
            if (iCount > 1000000)
            {
                SajetCommon.Show_Message("Too many records (max 1000000)", 0);
                return;
            }

            try
            {
                int iGenerated = 0;
                int iPadding = sStartNo.Length;

                for (int i = iStart; i <= iEnd; i++)
                {
                    string sSerialNumber = sSpecCode + i.ToString().PadLeft(iPadding, '0');

                    // 直接寫入 G_SN_SPEC_STATUS，不檢查 G_SN_STATUS
                    InsertSpecStatus(_WO, sSerialNumber, "");
                    iGenerated++;
                }

                SajetCommon.Show_Message("Generate Success: " + iGenerated + " records", 3);
            }
            catch (Exception ex)
            {
                _Result = false;
                SajetCommon.Show_Message("Generate Error: " + ex.Message, 0);
            }
            finally
            {
                if (_Result)
                {
                    if (!string.IsNullOrEmpty(_WO))
                    {
                        _SQL = $@"UPDATE SAJET.G_WO_BASE SET WO_OPTION2 = 2 WHERE WORK_ORDER = '{_WO}'";
                        ClientUtils.ExecuteSQL(_SQL);
                    }
                }
            }
        }

        private void btnQueryWorkOrder_Click(object sender, EventArgs e)
        {
            string sWorkOrder = txtWorkOrder.Text.Trim();

            if (string.IsNullOrEmpty(sWorkOrder))
            {
                SajetCommon.Show_Message("Please enter Work Order", 0);
                return;
            }

            try
            {
                // 從 G_SN_STATUS 取得該工單的所有序號
                string sSQL = @"SELECT WORK_ORDER, SERIAL_NUMBER 
                               FROM SAJET.G_SN_STATUS 
                               WHERE WORK_ORDER = :WORK_ORDER 
                               ORDER BY SERIAL_NUMBER";
                object[][] Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
                DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    SajetCommon.Show_Message("Work Order not found in G_SN_STATUS", 0);
                    return;
                }

                int iImported = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string sSerialNumber = dr["SERIAL_NUMBER"].ToString();
                    InsertSpecStatus(_WO, sSerialNumber, "");
                    iImported++;
                }

                SajetCommon.Show_Message("Import Success: " + iImported + " records", 3);
            }
            catch (Exception ex)
            {
                _Result = false;
                SajetCommon.Show_Message("Error: " + ex.Message, 0);
            }
            finally
            {
                if (_Result)
                {
                    if (!string.IsNullOrEmpty(_WO))
                    {
                        _SQL = $@"UPDATE SAJET.G_WO_BASE SET WO_OPTION2 = 3 WHERE WORK_ORDER = '{_WO}'";
                        ClientUtils.ExecuteSQL(_SQL);
                    }
                }
            }
        }

        /// <summary>
        /// 檢查資料是否存在於 G_SN_STATUS
        /// </summary>
        private bool CheckSNStatusExists(string sWorkOrder, string sSerialNumber)
        {
            string sSQL = @"SELECT COUNT(*) as CNT 
                          FROM SAJET.G_SN_STATUS 
                          WHERE WORK_ORDER = :WORK_ORDER 
                          AND SERIAL_NUMBER = :SERIAL_NUMBER";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", sSerialNumber };
            DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params);
            
            if (ds.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0]["CNT"]) > 0;
            }
            return false;
        }

        /// <summary>
        /// 透過序號取得工單編號
        /// </summary>
        private string GetWorkOrderBySerialNumber(string sSerialNumber)
        {
            string sSQL = @"SELECT WORK_ORDER 
                          FROM SAJET.G_SN_STATUS 
                          WHERE SERIAL_NUMBER = :SERIAL_NUMBER";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", sSerialNumber };
            DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["WORK_ORDER"].ToString();
            }
            return "";
        }

        private void InsertSpecStatus(string sWorkOrder, string sSerialNumber, string sSpecCode)
        {
            string sSQL = @"INSERT INTO SAJET.G_SN_SPEC_STATUS 
                          (WORK_ORDER, SERIAL_NUMBER, SERIAL_STATUS, CREATE_TIME, UPDATE_USERID)
                          VALUES 
                          (:WORK_ORDER, :SERIAL_NUMBER, :SERIAL_STATUS, SYSDATE, :UPDATE_USERID)";
            object[][] Params = new object[4][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", sSerialNumber };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_STATUS", sSpecCode };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
            ClientUtils.ExecuteSQL(sSQL, Params);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtExcelPath.Text = "";
            txtSpecCode.Text = "";
            txtStartNo.Text = "";
            txtEndNo.Text = "";
            txtWorkOrder.Text = "";
        }

        private void txtExcelPath_Enter(object sender, EventArgs e)
        {
            txtExcelPath.Enabled = true;
            btnGenerateSpec.Enabled = false;
            btnQueryWorkOrder.Enabled = false;
        }

        private void txtSpecCode_Enter(object sender, EventArgs e)
        {
            btnImportExcel.Enabled = false;
            btnQueryWorkOrder.Enabled = false;
            txtSpecCode.Enabled = true;
        }

        private void btnQueryWorkOrder_Enter(object sender, EventArgs e)
        {
            btnImportExcel.Enabled = false;
            btnGenerateSpec.Enabled = false;
            btnQueryWorkOrder.Enabled = true;
        }

        private void txtStartNo_Enter(object sender, EventArgs e)
        {
            btnImportExcel.Enabled = false;
            btnQueryWorkOrder.Enabled = false;
            txtSpecCode.Enabled = true;
        }

        private void txtEndNo_Enter(object sender, EventArgs e)
        {
            btnImportExcel.Enabled = false;
            btnQueryWorkOrder.Enabled = false;
            txtSpecCode.Enabled = true;
        }
    }
}