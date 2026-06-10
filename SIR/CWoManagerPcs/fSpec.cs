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
using Excel = Microsoft.Office.Interop.Excel;

namespace CWoManagerPcs
{
    public partial class fSpec : Form
    {
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
            rbtImportExcel.Checked = true;
            panelImportExcel.Visible = true;
            panelSpecCode.Visible = false;
            panelWorkOrder.Visible = false;
        }

        private void rbtImportExcel_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtImportExcel.Checked)
            {
                panelImportExcel.Visible = true;
                panelSpecCode.Visible = false;
                panelWorkOrder.Visible = false;
            }
        }

        private void rbtSpecCode_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtSpecCode.Checked)
            {
                panelImportExcel.Visible = false;
                panelSpecCode.Visible = true;
                panelWorkOrder.Visible = false;
            }
        }

        private void rbtWorkOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtWorkOrder.Checked)
            {
                panelImportExcel.Visible = false;
                panelSpecCode.Visible = false;
                panelWorkOrder.Visible = true;
            }
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

            try
            {
                Excel.Application app = new Excel.Application();
                Excel.Workbook wb = app.Workbooks.Open(txtExcelPath.Text);
                Excel.Worksheet ws = (Excel.Worksheet)wb.Sheets[1];
                Excel.Range range = ws.UsedRange;

                int iRowCount = range.Rows.Count;
                int iImported = 0;

                for (int i = 2; i <= iRowCount; i++) // 從第2行開始（跳過標題）
                {
                    string sWorkOrder = "";
                    string sSerialNumber = "";

                    try
                    {
                        sWorkOrder = ((Excel.Range)range.Cells[i, 1]).Text.ToString();
                        sSerialNumber = ((Excel.Range)range.Cells[i, 2]).Text.ToString();
                    }
                    catch
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(sWorkOrder) && string.IsNullOrEmpty(sSerialNumber))
                        continue;

                    // 寫入 G_SN_SPEC_STATUS
                    InsertSpecStatus(sWorkOrder, sSerialNumber, "");
                    iImported++;
                }

                wb.Close(false);
                app.Quit();

                SajetCommon.Show_Message("Import Success: " + iImported + " records", 3);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Import Error: " + ex.Message, 0);
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
            if (iCount > 10000)
            {
                SajetCommon.Show_Message("Too many records (max 10000)", 0);
                return;
            }

            try
            {
                int iGenerated = 0;
                for (int i = iStart; i <= iEnd; i++)
                {
                    string sSerialNumber = sSpecCode + i.ToString().PadLeft(sStartNo.Length, '0');
                    InsertSpecStatus("", sSerialNumber, sSpecCode);
                    iGenerated++;
                }

                SajetCommon.Show_Message("Generate Success: " + iGenerated + " records", 3);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Generate Error: " + ex.Message, 0);
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
                string sSQL = @"SELECT SERIAL_NUMBER FROM SAJET.G_SN_STATUS 
                               WHERE WORK_ORDER = :WORK_ORDER 
                               ORDER BY SERIAL_NUMBER";
                object[][] Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
                DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params);

                int iImported = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string sSerialNumber = dr["SERIAL_NUMBER"].ToString();
                    InsertSpecStatus(sWorkOrder, sSerialNumber, "");
                    iImported++;
                }

                SajetCommon.Show_Message("Import Success: " + iImported + " records", 3);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Error: " + ex.Message, 0);
            }
        }

        private void InsertSpecStatus(string sWorkOrder, string sSerialNumber, string sSpecCode)
        {
            string sSQL = @"INSERT INTO SAJET.G_SN_SPEC_STATUS 
                          (WORK_ORDER, SERIAL_NUMBER, SPEC_CODE, UPDATE_TIME, UPDATE_USERID)
                          VALUES 
                          (:WORK_ORDER, :SERIAL_NUMBER, :SPEC_CODE, SYSDATE, :UPDATE_USERID)";
            object[][] Params = new object[4][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", sSerialNumber };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC_CODE", sSpecCode };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
            ClientUtils.ExecuteSQL(sSQL, Params);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}