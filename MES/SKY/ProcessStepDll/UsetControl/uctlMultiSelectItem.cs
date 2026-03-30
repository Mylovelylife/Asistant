using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;

namespace ProcessStepDll
{
    public partial class uctlMultiSelectItem : UserControl
    {
        private string _sTableName;
        private string _sFieldName;
        
        public string sTableName
        {
            set { _sTableName = value; }

        }
        public string sFieldName
        {
            set { _sFieldName = value; }
        }
        public void SetFocus()
        {
            textBox1.Focus();
        }

        public uctlMultiSelectItem()
        {
            InitializeComponent();
        }
        public void ShowData()
        {
            GetFilter("%");
        }

        private void btnAddOne_Click(object sender, EventArgs e)
        {
            while (lstAll.SelectedItems.Count > 0)
            {
                ListViewItem item = lstAll.SelectedItems[0];
                if (lstSelect.Items.Find(item.Name, false).Length == 0)
                {
                    lstAll.Items.Remove(item);
                    lstSelect.Items.Add(item);
                }
                else
                    lstAll.Items.Remove(item);
            }
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            while (lstAll.Items.Count > 0)
            {
                ListViewItem item = lstAll.Items[0];
                lstAll.Items.Remove(item);
                lstSelect.Items.Add(item);
            }
        }

        private void btnRemoveOne_Click(object sender, EventArgs e)
        {
            while (lstSelect.SelectedItems.Count > 0)
            {
                ListViewItem item = lstSelect.SelectedItems[0];
                lstSelect.Items.Remove(item);
                lstAll.Items.Add(item);
            }
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            while (lstSelect.Items.Count > 0)
            {
                ListViewItem item = lstSelect.Items[0];
                lstSelect.Items.Remove(item);
                lstAll.Items.Add(item);
            }
        }
        private void GetFilter(string f_sFilter)
        {
            string sSQL = " Select " + _sFieldName
                                  + " from " + _sTableName
                                  + " WHERE " + _sFieldName + " LIKE :FIELDNAME "
                                  + "  AND ENABLED='Y' "
                                  + " ORDER BY " + _sFieldName;
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FIELDNAME", f_sFilter };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            lstAll.Items.Clear();
            foreach (DataColumn dc in dsTemp.Tables[0].Columns)
            {
                lstAll.Columns.Add(SajetCommon.SetLanguage(dc.ColumnName, 1), -2);
                lstSelect.Columns.Add(SajetCommon.SetLanguage(dc.ColumnName, 1), -2);
            }
            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
            {
                lstAll.Items.Add(dsTemp.Tables[0].Rows[i][0].ToString());
                lstAll.Items[lstAll.Items.Count - 1].Name = dsTemp.Tables[0].Rows[i][0].ToString();
                for (int j = 1; j < dsTemp.Tables[0].Columns.Count; j++)
                    lstAll.Items[lstAll.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i][j].ToString());
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (Char)Keys.Enter)
                return;

            string sPartFilter = textBox1.Text.Trim();
            sPartFilter = sPartFilter + "%";
            GetFilter(sPartFilter);
        }
    }
}
