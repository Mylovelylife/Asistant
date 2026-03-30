using System;
using System.Windows.Forms;

namespace ProcessStepDll
{
    public partial class fQTY : Form
    {
        public fQTY()
        {
            InitializeComponent();
            SajetClass.SajetCommon.SetLanguageControl(this);
        }
        public fQTY(string sToolingNo,int iQty)
        {
            InitializeComponent();
            SajetClass.SajetCommon.SetLanguageControl(this);
            if (iQty > 0)
                txtQTY.Text = iQty.ToString();
            lablToolingNo.Text = sToolingNo;
        }

        private void txtQTY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))//數字跟控制按鈕
            {
                if (e.KeyChar == (char)13)
                    btnOK_Click(btnOK, new EventArgs());
                return;
            }
            else
                e.KeyChar = (char)Keys.None;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtQTY.Text) || Convert.ToInt16(txtQTY.Text) == 0)
            {
                SajetClass.SajetCommon.Show_Message("QTY is Null or 0", 0);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void fQTY_Load(object sender, EventArgs e)
        {
            txtQTY.Focus();
        }
    }
}
