using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SajetClass;
namespace ProcessStepDll
{
    public partial class uctlDataGridView : UserControl
    {

        public delegate void m_OnRowSelect(DataGridViewRow dr);
        public event m_OnRowSelect OnRowSelect;
        DataTable _dtTable;
        bool _bBind;
        public string sItemName;
         
        public DataTable dtSourceTable
        {
            get { return _dtTable; }
            set
            {_dtTable = value;}
        }
        
        public bool bBindingDataSource
        {
            get { return _bBind; }
            set
            { _bBind = value; }
        }
       
        public uctlDataGridView()
        {
            InitializeComponent();
           
        }
        public void Clear()
        {
            dgvData.DataSource = null;
            lablCount.Text = "0";          
        }
       
        public void ShowMode1()
        {
           
            lablCount.Text = _dtTable.Rows.Count.ToString();
            dgvData.DataSource = _dtTable;
            dgvData.Dock = DockStyle.Fill;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            for (int i = 0; i <= dgvData.Columns.Count - 1; i++)
                dgvData.Columns[i].HeaderText = SajetCommon.SetLanguage(dgvData.Columns[i].HeaderText,1);

        }
        public void SetUnVisibleCol(List<string> colList)
        {
            foreach (string sCol in colList)
            {
                if (dgvData.Columns.Contains(sCol))
                    dgvData.Columns[sCol].Visible=false;
            }
        }

        private void dgvData_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            showDataToolStripMenuItem.Visible = (!_bBind);
        }

        private void showDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _dtTable.Rows.Clear();
            ShowMode1();           
        }

        private void dgvData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (_dtTable.Columns.IndexOf("RESULT") >= 0)
                SetColor("RESULT", "OK", Color.White, Color.Red);
           
        }
        public DataGridViewRow SelectRow()
        {
            if (dgvData.Rows.Count == 0 || dgvData.CurrentRow == null)
                return null;
            else
                return dgvData.CurrentRow;
        }
        private void dgvData_SelectionChanged(object sender, EventArgs e)
        {
            
        }
        public void SetColor(string sColumnName, string f_sValue, Color f_ForeColor, Color f_BackColor, int iRowIndex)
        {
            int i = iRowIndex;
            string sValue = dgvData.Rows[i].Cells[sColumnName].Value.ToString();
            if (string.IsNullOrEmpty(sValue) || sValue == "N/A")
                return;

            if (f_sValue != sValue)
            {
                dgvData.Rows[i].Cells[sColumnName].Style.BackColor = f_BackColor;
                dgvData.Rows[i].Cells[sColumnName].Style.ForeColor = f_ForeColor;
            }
        }

        private void SetColor(string sColumnName, string f_sValue, Color f_ForeColor, Color f_BackColor)
        {
            for (int i = 0; i <= dgvData.Rows.Count - 1; i++)
            {
                string sValue = "";
                object objValue = dgvData.Rows[i].Cells[sColumnName].Value;
                if (objValue != null)
                    sValue = dgvData.Rows[i].Cells[sColumnName].Value.ToString();
                if (string.IsNullOrEmpty(sValue) || sValue == "N/A")
                    continue;
                if (sValue == "SKIP")
                    dgvData.Rows[i].Cells[sColumnName].Style.BackColor = Color.PowderBlue;
                else if (sValue.ToUpper() == "WARNING")
                    dgvData.Rows[i].Cells[sColumnName].Style.BackColor = Color.Yellow;
                else
                {

                    if (sValue.Length >= 2)
                    {
                        try
                        {
                            sValue = sValue.Substring(0, 2);
                        }
                        catch
                        {
                        }
                    }

                    if (f_sValue != sValue)
                    {
                        dgvData.Rows[i].Cells[sColumnName].Style.BackColor = f_BackColor;
                        dgvData.Rows[i].Cells[sColumnName].Style.ForeColor = f_ForeColor;
                    }
                    else
                    {
                        dgvData.Rows[i].Cells[sColumnName].Style.BackColor = Color.White;
                        dgvData.Rows[i].Cells[sColumnName].Style.ForeColor = Color.Black;
                    }
                }
            }
        }
    }
}
