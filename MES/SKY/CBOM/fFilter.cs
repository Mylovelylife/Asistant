using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Collections.Specialized;

namespace SajetFilter
{
    public partial class fFilter : Form
    {
        public object[][] g_Params;

        public List<string> g_ListTrsnField;
        DataTable dtSourceTable;
        public string sSQL;
        StringCollection g_tsField = new StringCollection();
        public fFilter()
        {
            InitializeComponent();
            g_ListTrsnField = new List<string>();
        }
        private void dgvData_DoubleClick(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count > 0 && dgvData.CurrentRow != null)
                DialogResult = DialogResult.OK;
        }

        public fFilter(object[][] Param)
        {
            g_Params = Param;            
            InitializeComponent();
            g_ListTrsnField = new List<string>();
        }
        private void fFilter_Load(object sender, EventArgs e)
        {           
            combField.Items.Clear();
              DataSet dsSearch = new DataSet();
            if (g_Params != null)
                dsSearch = ClientUtils.ExecuteSQL(sSQL, g_Params);
            else
                dsSearch = ClientUtils.ExecuteSQL(sSQL);
            dgvData.DataSource = dsSearch;
            dgvData.DataMember = dsSearch.Tables[0].ToString();
            dtSourceTable = dsSearch.Tables[0];
            for (int i = 0; i <= dsSearch.Tables[0].Columns.Count - 1; i++)
            {
                combField.Items.Add(dsSearch.Tables[0].Columns[i].ToString());
                g_tsField.Add(dsSearch.Tables[0].Columns[i].ToString());
            }
            if (combField.Items.Count > 0)
                combField.SelectedIndex = 0;

            if (dgvData.Rows.Count > 0)
                dgvData.CurrentCell = dgvData.Rows[0].Cells[0];

            SajetCommon.SetLanguageControl(this);

            for (int j = 0; j < dgvData.Rows.Count; j++)
            {
                foreach (string sFieldName in g_ListTrsnField)
                {
                    dgvData.Rows[j].Cells[sFieldName].Value = SajetCommon.SetLanguage(dgvData.Rows[j].Cells[sFieldName].Value.ToString(), 1);
                }
            }

           
            editValue.Focus();            
        }

        private void editValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            if (dtSourceTable.Rows.Count == 0) return;
            dgvData.DataSource = null;
            try
            {
                string sFilterValue = editValue.Text.Trim();
                if (string.IsNullOrEmpty(sFilterValue))
                {
                    dgvData.DataSource = dtSourceTable;
                    return;
                }
                sFilterValue = editValue.Text + "%";
                string sField = g_tsField[combField.SelectedIndex];
                DataRow[] drList = dtSourceTable.Select(sField + " Like '" + sFilterValue + "' ");
                DataTable dTable = new DataTable();
                for (int i = 0; i <= dtSourceTable.Columns.Count - 1; i++)
                {
                    string sColumnname = dtSourceTable.Columns[i].ColumnName;
                    dTable.Columns.Add(sColumnname, typeof(string));
                }
                for (int i = 0; i <= drList.Length - 1; i++)
                {
                    DataRow dr = dTable.NewRow();
                    for (int j = 0; j <= dTable.Columns.Count - 1; j++)
                        dr[j] = drList[i][j];
                    dTable.Rows.Add(dr);
                }

                dgvData.DataSource = dTable;
            }
            finally
            {
                for (int i = 0; i <= dgvData.Columns.Count - 1; i++)
                    dgvData.Columns[i].HeaderText = SajetCommon.SetLanguage(dgvData.Columns[i].HeaderText);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}