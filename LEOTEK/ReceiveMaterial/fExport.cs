using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReceiveMaterial
{
	public partial class fExport : Form
	{
		public string StoreType;
		public fExport()
		{
			InitializeComponent();

			DataTable dt = new DataTable();
			dt.Columns.Add("TBText", typeof(string));
			dt.Columns.Add("TBValue", typeof(string));
			
			dt.Rows.Add("Please Choose", "0");
			dt.Rows.Add("001", "001");
			dt.Rows.Add("002", "002");
			dt.Rows.Add("003", "003");
			dt.Rows.Add("ALL", "ALL");

			cb_Type.Items.Clear();
			cb_Type.DataSource = dt;
			cb_Type.DisplayMember = "TBText";
			cb_Type.ValueMember = "TBValue";
			cb_Type.DropDownStyle = ComboBoxStyle.DropDownList;
		}

		private void cb_Type_MouseDown(object sender, MouseEventArgs e)
		{
			StoreType = cb_Type.SelectedValue.ToString();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			StoreType = cb_Type.SelectedValue.ToString();

			if (StoreType != "0")
			{
				DialogResult = DialogResult.OK;

				this.Close();
			}
			else
			{
				MessageBox.Show("Please Choose Type");
			}
		}
	}
}
