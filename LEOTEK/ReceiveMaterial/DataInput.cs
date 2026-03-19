using Newtonsoft.Json;
using ReceiveMaterial.Helper;
using ReceiveMaterial.Model;
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
	public partial class DataInput : Form
	{
		private CommonHelper myHelper;

		private ReceiveModel _ReceiveModel;

		public string g_Barcode;
		public DataInput()
		{
			InitializeComponent();
		}
		public DataInput(string Title, ReceiveModel Data)
		{
			InitializeComponent();

			myHelper = new CommonHelper();

			_ReceiveModel = Data;

			//txt_UserName.ImeMode = System.Windows.Forms.ImeMode.Disable;

			this.Text = Title;
		}

		private void DataInput_Load(object sender, EventArgs e)
		{
			myHelper.SettingData(this.Controls);

			txt_Carton.Text = _ReceiveModel.CARTON.ToString();
			txt_QTY.Text = _ReceiveModel.QTY.ToString();
			txt_PALLET.Text = _ReceiveModel.PALLET;
			lb_Part_No.Text = _ReceiveModel.PART;
			lb_Item.Text = _ReceiveModel.DNITEM.ToString();
			lb_WO.Text = _ReceiveModel.WO.ToString();
			lb_PO.Text = _ReceiveModel.DN.ToString();

		}

		private void bt_confirm_Click(object sender, EventArgs e)
		{
			_ReceiveModel.CARTON = int.Parse(txt_Carton.Text);
			_ReceiveModel.QTY = int.Parse(txt_QTY.Text);
			_ReceiveModel.PALLET = txt_PALLET.Text;

			if (myHelper.isNumber(txt_Carton.Text) && myHelper.isNumber(txt_QTY.Text))
			{
				g_Barcode = JsonConvert.SerializeObject(_ReceiveModel, Formatting.Indented);

				DialogResult = DialogResult.OK;
				this.Close();
			}
			else 
			{
				MessageBox.Show("請輸入數字");
				return;
			}			
		}

		private void txt_QTY_TextChanged(object sender, EventArgs e)
		{
			if (myHelper.isNumber(txt_QTY.Text))
			{
				if (int.Parse(txt_QTY.Text)> _ReceiveModel.QTY)
				{					
					MessageBox.Show("輸入數量錯誤");
					txt_QTY.Text = _ReceiveModel.QTY.ToString();
				}
			}
			else 
			{
				MessageBox.Show("請輸入數字");
			}
		}
	}
}
