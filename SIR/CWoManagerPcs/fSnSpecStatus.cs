using SajetClass;
using System;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace CWoManagerPcs
{
    public partial class fSnSpecStatus : Form
    {
        public string g_sWorkOrder;
        private DataSet dsTemp;
        private string sSQL;

        public fSnSpecStatus()
        {
            InitializeComponent();
        }

        private void fSnSpecStatus_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + " - " + g_sWorkOrder;
            SajetCommon.SetLanguageControl(this);
            LoadData();
        }

        private void LoadData()
        {
            sSQL = $"SELECT WORK_ORDER, SERIAL_NUMBER, SERIAL_STATUS, UPDATE_USERID, CREATE_TIME " +
                  $"FROM SAJET.G_SN_SPEC_STATUS " +
                  $"WHERE WORK_ORDER = :WORK_ORDER " +
                  $"ORDER BY CREATE_TIME DESC";

            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", g_sWorkOrder };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            dgvSpecStatus.DataSource = dsTemp.Tables[0];
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show(
                $"Delete all records for Work Order: {g_sWorkOrder}?",
                SajetCommon.SetLanguage("Warning"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                try
                {
                    sSQL = "DELETE FROM SAJET.G_SN_SPEC_STATUS " +
                           "WHERE WORK_ORDER = :WORK_ORDER";

                    object[][] Params = new object[1][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", g_sWorkOrder };

                    ClientUtils.ExecuteSQL(sSQL, Params);

                    SajetCommon.Show_Message("Delete All OK", 3);
                    LoadData();
                }
                catch (Exception ex)
                {
                    SajetCommon.Show_Message(ex.Message, 0);
                }
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Files|*.xls";
                sfd.FileName = $"SN_SPEC_STATUS_{g_sWorkOrder}_{DateTime.Now:yyyyMMddHHmmss}.xls";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = sfd.FileName;

                    // Write to Excel using HTML format
                    using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                    {
                        // HTML header for Excel
                        sw.WriteLine("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head><body>");
                        sw.WriteLine("<table border='1' cellpadding='3' cellspacing='0' style='border-collapse:collapse; font-size:12px;'>");

                        // Header
                        sw.WriteLine("<tr style='background-color:#CCCCCC; font-weight:bold;'>");
                        foreach (DataGridViewColumn col in dgvSpecStatus.Columns)
                        {
                            sw.WriteLine($"<td>{col.HeaderText}</td>");
                        }
                        sw.WriteLine("</tr>");

                        // Data rows
                        foreach (DataGridViewRow row in dgvSpecStatus.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                sw.WriteLine("<tr>");
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    string value = cell.Value?.ToString() ?? "";
                                    sw.WriteLine($"<td>{System.Security.SecurityElement.Escape(value)}</td>");
                                }
                                sw.WriteLine("</tr>");
                            }
                        }

                        sw.WriteLine("</table></body></html>");
                    }

                    SajetCommon.Show_Message("Export OK: " + filePath, 3);

                    // Open the file
                    Process.Start(filePath);
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
        }
    }
}