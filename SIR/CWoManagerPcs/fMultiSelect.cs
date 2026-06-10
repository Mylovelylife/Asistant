using SajetClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CWoManagerPcs
{
    public partial class fMultiSelect : Form
    {
        private fData fMainControl;
        public int iCurrent;
        private string[] sSelected;
        public string sNECESSARY;
        public fMultiSelect()
        {
            InitializeComponent();
        }
        public fMultiSelect(fData f)
        {
            fMainControl = f;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string sSelect = string.Empty;
            int iIndex = 0;
            for (int i = 0; i < dgv_Select.Rows.Count; i++)
            {
                string sChecked = dgv_Select.Rows[i].Cells[dgv_Select.CheckedColumnName].Value.ToString();
                if (!sChecked.Equals(string.Empty) && Convert.ToBoolean(dgv_Select.Rows[i].Cells[dgv_Select.CheckedColumnName].Value))
                {
                    if (iIndex == 0)
                    {
                        sSelect = dgv_Select.Rows[i].Cells["Option"].Value.ToString();
                    }
                    else
                    {
                        sSelect += ',' + dgv_Select.Rows[i].Cells["Option"].Value.ToString();
                    }
                    iIndex++;
                }
            }
            if (!sSelect.Equals(string.Empty))
            {
                fMainControl.dgvProperty.Rows[iCurrent].Cells["PropertyValue"].Value = sSelect;
            }
            else
            {
                if (sNECESSARY.Equals("Y"))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please pick option to configration."), 1);
                    return;
                }
                else
                {
                    fMainControl.dgvProperty.Rows[iCurrent].Cells["PropertyValue"].Value = "...";
                }
            }
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void fMultiSelect_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            string sfDataProperty_value = fMainControl.dgvProperty.Rows[iCurrent].Cells["PropertyValue"].Value.ToString();
            if (!sfDataProperty_value.Equals("..."))
            {
                sSelected = sfDataProperty_value.Split(',');
                for (int i = 0; i < sSelected.Length; i++)
                {
                    for (int j = 0; j < dgv_Select.Rows.Count; j++)
                    {
                        if (dgv_Select.Rows[j].Cells["Option"].Value.ToString().Equals(sSelected[i]))
                        {
                            dgv_Select.Rows[j].Cells[dgv_Select.CheckedColumnName].Value = true;
                        }
                    }
                }
            }
        }
    }
}
