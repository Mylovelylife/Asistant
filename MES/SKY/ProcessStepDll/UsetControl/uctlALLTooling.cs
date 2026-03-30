using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;


namespace ProcessStepDll
{
    public partial class uctlALLTooling : UserControl
    {
        public delegate void m_OnModify();
        public event m_OnModify OnModify;
        public uctlALLTooling()
        {
            InitializeComponent();
            SajetCommon.SetLanguageControl(this);
        }
        public void GetALLTooling()
        {
            //Show Process
            string sStage = "";
            int iCnt = 0;
            string sSQL = string.Empty;

            sSQL = "Select A.STEP_ITEM_CODE,A.STEP_ITEM_NAME  "
                 + " From SAJET.SYS_STEP_ITEM A "
                 + " WHERE A.ENABLED = 'Y' "
                 + " Order By A.STEP_ITEM_CODE";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            TreeViewTooling.Nodes.Clear();
            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
                if (sStage != DS.Tables[0].Rows[i]["STEP_ITEM_CODE"].ToString())
                {
                    sStage = DS.Tables[0].Rows[i]["STEP_ITEM_CODE"].ToString();
                    string sStepItemName = DS.Tables[0].Rows[i]["STEP_ITEM_NAME"].ToString();
                    TreeNode NodeToolingNo = new TreeNode();
                    NodeToolingNo.Text ="["+sStage+"]"+sStepItemName;
                    NodeToolingNo.ImageIndex = 3;
                    NodeToolingNo.SelectedImageIndex = NodeToolingNo.ImageIndex;
                    TreeViewTooling.Nodes.Add(NodeToolingNo);
                    iCnt = iCnt + 1;
                }      

            
            //TreeViewTooling.ExpandAll();
        }
        private void collapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeViewTooling.CollapseAll();
        }

        private void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeViewTooling.ExpandAll();
        }

        private void TreeViewProcess_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void TreeViewTooling_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeViewTooling.SelectedNode.SelectedImageIndex = TreeViewTooling.SelectedNode.ImageIndex;
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TreeViewTooling.Nodes.Count == 0)
                return;
            if (TreeViewTooling.SelectedNode == null)
                return;
            TreeNode treeNode = TreeViewTooling.SelectedNode;
            int iLevel = treeNode.Level;
            string sNodeText = treeNode.Text;
            string sStepItemCode = sNodeText;
            string sStepItemName = sNodeText;
            int iIndex = sNodeText.IndexOf("]");
            if (iIndex >= 0)
            {
                sStepItemCode = sNodeText.Substring(1, iIndex - 1);
                sStepItemName = sNodeText.Substring(iIndex + 1);
            }


            fDetailData f = new fDetailData();
            try
            {
                f.g_sUpdateType = "MODIFY";
                f.g_sformText = "MODIFY";
                f.g_sProcessID = "0";
                f.g_sProcessName = "N/A";
                if (f.ShowDialog() == DialogResult.OK)
                {
                    GetALLTooling();
                    OnModify();

                }
            }
            finally
            {
                f.Dispose();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fDetailData f = new fDetailData();
            try
            {
                f.g_sUpdateType = "APPEND";
                f.g_sformText = "APPEND";
                f.g_sProcessID = "0";
                f.g_sProcessName = "N/A";
                if (f.ShowDialog() == DialogResult.OK)
                {
                    GetALLTooling();

                }
            }
            finally
            {
                f.Dispose();
            }
        }
    }
}
