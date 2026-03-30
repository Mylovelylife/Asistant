using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CBOM
{
    public partial class fMessage : Form
    {
        public string g_Message = string.Empty;
        public fMessage()
        {
            InitializeComponent();
        }

        private void bt_confirm_Click(object sender, EventArgs e)
        {
            g_Message = txt_message.Text;

            DialogResult = DialogResult.OK;
        }
    }
}
