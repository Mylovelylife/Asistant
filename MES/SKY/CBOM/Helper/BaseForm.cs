using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CBOM.Helper
{
    public partial class BaseForm : Form
    {
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close(); // 關閉目前表單
                return true;  // 表示已處理按鍵
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
