using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Collections.Specialized;
using SajetTable;

namespace CWoManagerPcs
{
    public partial class fFilterSelect : Form
    {
        StringCollection g_tsField = new StringCollection();
        DataSet dsSearch;//資料主體
        DataTable dsTemp;//顯示主體

        public fFilterSelect(DataSet ds)
        {
            InitializeComponent();
            dsSearch = ds;
        }
        int startIndex = -1;//第一個可顯示的欄位index
        private void dgvData_DoubleClick(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count > 0 && dgvData.CurrentRow != null)
                DialogResult = DialogResult.OK;
        }
        private void fFilterSelect_Load(object sender, EventArgs e)
        {
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;

            combField.Items.Clear();
            dsTemp = dsSearch.Tables[0].Copy();//複製原始資料用來顯示
            dgvData.DataSource = dsTemp;//使用顯示table 而不是資料table

            foreach (DataGridViewColumn dgvC in dgvData.Columns) { dgvC.Visible = false; }

            string sGridField;

            int displayIndex = 0;
            foreach (TableDefine.TGrid_Field item in TableDefine.tGridField)
            {
                sGridField = item.sFieldName;
                if (dgvData.Columns.Contains(sGridField))
                {
                    dgvData.Columns[sGridField].HeaderText = item.sCaption;
                    dgvData.Columns[sGridField].DisplayIndex = displayIndex; //欄位顯示順序
                    dgvData.Columns[sGridField].Visible = true;
                    displayIndex++;
                }
            }

            for (int i = 0; i <= dgvData.ColumnCount - 1; i++)
            {
                if (!dgvData.Columns[i].Visible) continue;
                combField.Items.Add(dgvData.Columns[i].HeaderText);
                g_tsField.Add(dgvData.Columns[i].Name);
                if (startIndex < 0) startIndex = i;
            }

            if (combField.Items.Count > 0) combField.SelectedIndex = 0;

            if (dgvData.Rows.Count > 0) dgvData.CurrentCell = dgvData.Rows[0].Cells[startIndex];

            SajetCommon.SetLanguageControl(this);
            editValue.Focus();
        }

        private void editValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13) return;
            if (dgvData.Rows.Count < 1) return;

            ///<summary>
            ///清除顯示table的row , 並用資料table重新搜尋後 並加入
            ///速度會快上很多
            ///</summary>
            dsTemp.Rows.Clear();
            DataRow[] dataRow = dsSearch.Tables[0].Select("" + g_tsField[combField.SelectedIndex] + " like '" + editValue.Text + "%'", "" + g_tsField[combField.SelectedIndex] + " asc");
            //($"{g_tsField[combField.SelectedIndex]} like '{editValue.Text}%'", $"{g_tsField[combField.SelectedIndex]} asc");
            foreach (DataRow dr in dataRow)
            {
                dsTemp.Rows.Add(dr.ItemArray);
            }
        }
    }
}
