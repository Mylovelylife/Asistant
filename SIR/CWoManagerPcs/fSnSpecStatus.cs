using SajetClass;
using System;
using System.Data;
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

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilter.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(filterText))
            {
                dsTemp.Tables[0].DefaultView.RowFilter = "";
            }
            else
            {
                try
                {
                    dsTemp.Tables[0].DefaultView.RowFilter = 
                        $"SERIAL_NUMBER LIKE '%{filterText}%' OR SERIAL_STATUS LIKE '%{filterText}%'";
                }
                catch
                {
                    // Invalid filter expression, ignore
                }
            }
        }

        private void dgvSpecStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (dgvSpecStatus.CurrentRow == null)
                {
                    return;
                }

                var dr = MessageBox.Show(
                    SajetCommon.SetLanguage("Confirm Delete?"),
                    SajetCommon.SetLanguage("Warning"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    DeleteSelectedRow();
                }
            }
        }

        private void DeleteSelectedRow()
        {
            if (dgvSpecStatus.CurrentRow == null)
                return;

            try
            {
                string sWorkOrder = dgvSpecStatus.CurrentRow.Cells["WORK_ORDER"].Value?.ToString();
                string sSerialNumber = dgvSpecStatus.CurrentRow.Cells["SERIAL_NUMBER"].Value?.ToString();

                if (string.IsNullOrEmpty(sWorkOrder) || string.IsNullOrEmpty(sSerialNumber))
                {
                    SajetCommon.Show_Message("Data error", 0);
                    return;
                }

                sSQL = "DELETE FROM SAJET.G_SN_SPEC_STATUS " +
                       "WHERE WORK_ORDER = :WORK_ORDER AND SERIAL_NUMBER = :SERIAL_NUMBER";

                object[][] Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", sSerialNumber };

                ClientUtils.ExecuteSQL(sSQL, Params);

                SajetCommon.Show_Message("Delete OK", 3);
                LoadData();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
        }
    }
}