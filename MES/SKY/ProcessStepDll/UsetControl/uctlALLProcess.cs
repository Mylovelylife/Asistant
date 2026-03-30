using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace ProcessStepDll
{
    public partial class uctlALLProcess : UserControl
    {
        public uctlALLProcess()
        {
            InitializeComponent();
            SajetCommon.SetLanguageControl(this);
        }
        public void GetALLProcess()
        {
            //Show Process
            string sStage = "";
            int iCnt = 0;
            string sSQL = string.Empty;
            sSQL = "Select B.STAGE_CODE,B.STAGE_NAME,A.PROCESS_CODE,A.PROCESS_NAME,Upper(C.TYPE_NAME) TYPE_NAME "
                 + "    ,A.PROCESS_ID "
                 + " From SAJET.SYS_PROCESS A, "
                 + " SAJET.SYS_STAGE B, "
                 + " SAJET.SYS_OPERATE_TYPE C "
                 + " Where A.STAGE_ID = B.STAGE_ID "
                 + " and A.OPERATE_ID = C.OPERATE_ID(+) "
                 + " and A.ENABLED = 'Y' "
                 + " and B.ENABLED = 'Y' "
                 + " and (C.TYPE_NAME='Input' or C.TYPE_NAME='Assembly') "
                 + " Order By B.STAGE_NAME,A.PROCESS_NAME ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);

            TreeViewProcess.Nodes.Clear();
            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
            {
                if (sStage != DS.Tables[0].Rows[i]["STAGE_NAME"].ToString())
                {
                    sStage = DS.Tables[0].Rows[i]["STAGE_NAME"].ToString();
                    TreeNode Node1 = new TreeNode();
                    Node1.Text = sStage;
                    Node1.ImageIndex = 0;
                    Node1.SelectedImageIndex = Node1.ImageIndex;
                    TreeViewProcess.Nodes.Add(Node1);
                    iCnt = iCnt + 1;
                }

                TreeNode NodeProcess = new TreeNode();
                NodeProcess.Text = DS.Tables[0].Rows[i]["PROCESS_NAME"].ToString();
                NodeProcess.Tag = DS.Tables[0].Rows[i]["PROCESS_ID"].ToString();
                if (DS.Tables[0].Rows[i]["TYPE_NAME"].ToString() == "REPAIR"
                    || DS.Tables[0].Rows[i]["TYPE_NAME"].ToString() == "SP-REPAIR")
                {
                    NodeProcess.ImageIndex = 2;
                }
                else
                {
                    NodeProcess.ImageIndex = 1;
                }
                NodeProcess.SelectedImageIndex = NodeProcess.ImageIndex;
                TreeViewProcess.Nodes[iCnt - 1].Nodes.Add(NodeProcess);
            }
            //TreeViewProcess.ExpandAll();
        }

        private void collapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeViewProcess.CollapseAll();
        }

        private void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeViewProcess.ExpandAll();
        }

        private void TreeViewProcess_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void TreeViewProcess_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeViewProcess.SelectedNode.SelectedImageIndex = TreeViewProcess.SelectedNode.ImageIndex;
        }

    }
}
