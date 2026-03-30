using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;

namespace ProcessStepDll
{
    public partial class fCopyTo : Form
    {
        public int iCopyCount;
        ToolingUtils ToolUtils;
        uctlMultiSelectItem _ojbMultiItem;
        public fCopyTo(ToolingUtils _ToolUtils)
        {
            InitializeComponent();
            ToolUtils = _ToolUtils;
        }
        private void fCopyTo_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            _ojbMultiItem = new uctlMultiSelectItem();


            _ojbMultiItem.sTableName = "SAJET.SYS_PROCESS";
            _ojbMultiItem.sFieldName = "PROCESS_NAME";
            _ojbMultiItem.Dock = DockStyle.Fill;
            PanelMain.Controls.Add(_ojbMultiItem);
            _ojbMultiItem.SetFocus();
            _ojbMultiItem.ShowData();
            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            iCopyCount = 0;
            ToolUtils.dtDateTime = ClientUtils.GetSysDate();
            ToolUtils.sPKFieldID = "PROCESS_ID";
            for (int i = 0; i <= _ojbMultiItem.lstSelect.Items.Count - 1; i++)
            {
                string sFieldName = _ojbMultiItem.lstSelect.Items[i].Name;
                string sFieldID = "";
               
                    sFieldID = SajetCommon.GetID("SAJET.SYS_PROCESS", "PROCESS_ID", "PROCESS_NAME", sFieldName);

                ToolUtils.sDestKeyValue = sFieldID;
                if (ToolUtils.sPKFieldIDValue == ToolUtils.sDestKeyValue) //複制來源與目的地相同時,則跳過不處理
                    continue;
                
                ToolUtils.Copy();
                iCopyCount += 1;
            }
            DialogResult = DialogResult.OK;
        }

        private void fCopyTo_Shown(object sender, EventArgs e)
        {
            _ojbMultiItem.SetFocus();
        }
    }
}
