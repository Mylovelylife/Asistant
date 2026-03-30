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
    public partial class fProcessToolingLink : Form
    {
        ToolingUtils ToolUtils;
        uctlALLProcess objAllProcess;
        uctlALLTooling objAllTooling;
        uctlProcessTooling objProcessTooling;
        public fProcessToolingLink()
        {
            InitializeComponent();
        }
        public fProcessToolingLink(ToolingUtils _ToolUtils)
        {
            InitializeComponent();
            ToolUtils = _ToolUtils;
        }
        private void fProcessToolingLink_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            objAllProcess = new uctlALLProcess();
            objAllProcess.Dock = DockStyle.Fill;
            PanelProcess.Controls.Add(objAllProcess);
            objAllProcess.GetALLProcess();
            objAllTooling = new uctlALLTooling();
            objAllTooling.OnModify += new uctlALLTooling.m_OnModify(objAllTooling_OnModify);
            objAllTooling.Dock = DockStyle.Fill;
            panelTooling.Controls.Add(objAllTooling);
            objAllTooling.GetALLTooling();
            objProcessTooling = new uctlProcessTooling();
            objProcessTooling.sFunType = ToolUtils.sFunType;
            objProcessTooling.sPKFieldIDValue = ToolUtils.sPKFieldIDValue;
            objProcessTooling.Dock = DockStyle.Fill;
            objProcessTooling.Parent = tabPage3;
            objProcessTooling.ShowTooling();
            objProcessTooling.OnDeleteProcess += new uctlProcessTooling.m_OnDeleteProcess(objProcessTooling_OnDeleteProcess);
            objProcessTooling.OnDeleteToolingSN += new uctlProcessTooling.m_OnDeleteToolingSN(objProcessTooling_OnDeleteToolingSN);
            objProcessTooling.OnUpdateToolingQTY += new uctlProcessTooling.m_OnUpdateToolingQTY(objProcessTooling_OnUpdateToolingQTY);
            objProcessTooling.OnInsertToolingSN += new uctlProcessTooling.m_OnInsertToolingSN(objProcessTooling_OnInsertToolingSN);
            objProcessTooling.OnAppendStepItem += new uctlProcessTooling.m_OnAppendStepItem(objProcessTooling_OnAppendStepItem);
            LabModel.Text = ToolUtils.sPKFieldName;
            tabPage1.Parent = null;
            if (objProcessTooling.iRowCount == 0)
            {
                objProcessTooling.AddRoot(LabModel.Text);
            }
            //lablModelTitle.Text = SajetCommon.SetLanguage(ToolUtils.sFunType);
        }

        void objAllTooling_OnModify()
        {
            objProcessTooling.ShowTooling();
        }

        void objProcessTooling_OnAppendStepItem()
        {
            objAllTooling.GetALLTooling();
        }

        void objProcessTooling_OnInsertToolingSN(string sProcessName, string sToolingNO, string sToolingSN, int iQTY)
        {
            ToolUtils.InsertToolingSN(sProcessName, sToolingNO, sToolingSN, iQTY);
        }

        void objProcessTooling_OnUpdateToolingQTY(string sProcessName, string sToolingNO, string iQTY)
        {
            ToolUtils.UpdateToolingQTY(sProcessName, sToolingNO, iQTY);
        }

        void objProcessTooling_OnDeleteToolingSN(DateTime dtDateTime, string sProcessName, string sToolingNO, string sToolingSN)
        {

            ToolUtils.DeleteToolingSN(dtDateTime, sProcessName, sToolingNO);

        }

        void objProcessTooling_OnDeleteProcess(string sProcessName)
        {
            ToolUtils.dtDateTime = ClientUtils.GetSysDate();
            ToolUtils.DeleteProcess(sProcessName);
        }

        private void bbtnSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
