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
    public partial class uctlProcessTooling : UserControl
    {
        private string _sPKFieldIDValue;

        private string _sFunType;
        private string _sTableName;
        private string _sPKFieldID;
        private string _sHTTableName;
        private bool _bDeletePrivilege;
        private int _iRowCount;
        public delegate void m_OnDeleteProcess(string sProcessName);
        public event m_OnDeleteProcess OnDeleteProcess;
        public delegate void m_OnDeleteToolingSN(DateTime dtDateTime, string sProcessName, string sToolingNO, string sToolingSN);
        public event m_OnDeleteToolingSN OnDeleteToolingSN;
        public delegate void m_OnInsertToolingSN(string sProcessName, string sToolingNO, string sToolingSN, int iQTY);
        public event m_OnInsertToolingSN OnInsertToolingSN;
        public delegate void m_OnUpdateToolingQTY(string sProcessName, string sToolingNO, string iQTY);
        public event m_OnUpdateToolingQTY OnUpdateToolingQTY;
        public delegate void m_OnAppendStepItem();
        public event m_OnAppendStepItem OnAppendStepItem;
        private string _sProcessName;

        public string sPKFieldIDValue
        {
            set { _sPKFieldIDValue = value; }
        }
        public void SetClear()
        {
            TreeToolingData.Nodes.Clear();
        }


        public bool bDeletePrivilege
        {
            set
            {
                _bDeletePrivilege = value;
                deleteToolStripMenuItem.Enabled = value;
            }
        }
        public int iRowCount
        {
            set
            {
                _iRowCount = value;
            }
            get
            {
                return _iRowCount;
            }

        }
        public string sFunType
        {
            set
            {
                _sFunType = value;
                if (value == "MODEL")
                {
                    _sTableName = "SAJET.SYS_MODEL_PROCESS_TOOLING";
                    _sPKFieldID = "MODEL_ID";
                    _sHTTableName = "SAJET.SYS_HT_MODEL_PROCESS_TOOLING";
                }
                if (value == "PART")
                {
                    _sTableName = "SAJET.SYS_PART_PROCESS_TOOLING";
                    _sPKFieldID = "PART_ID";
                    _sHTTableName = "SAJET.SYS_HT_PART_PROCESS_TOOLING";
                }

            }
        }
        Dictionary<string, Dictionary<string, string>> _dictProcess = new Dictionary<string, Dictionary<string, string>>();
        public uctlProcessTooling()
        {
            InitializeComponent();
            SajetCommon.SetLanguageControl(this);
            _iRowCount = 0;
        }
        public void AddRoot(string f_sProcessName)
        {
            TreeNode NodeProcess = new TreeNode();
            NodeProcess.Text = f_sProcessName;
            NodeProcess.ImageIndex = 2;
            NodeProcess.SelectedImageIndex = NodeProcess.ImageIndex;
            TreeToolingData.Nodes.Add(NodeProcess);
            _sProcessName = f_sProcessName;
        }
        public void ShowTooling()
        {
            TreeToolingData.Nodes.Clear();
            _dictProcess.Clear();
            /* Formatted on 2017/10/2 上午 10:25:04 (QP5 v5.240.12305.39476) */
            string sSQL = @" SELECT C.PROCESS_NAME,B.STEP_ITEM_CODE,B.STEP_ITEM_NAME 
                            FROM SAJET.SYS_PROCESS_STEP A,SAJET.SYS_STEP_ITEM B ,SAJET.SYS_PROCESS C 
                          WHERE A.PROCESS_ID =:MODEL_ID  
                            AND A.STEP_ITEM_ID = B.STEP_ITEM_ID
                            AND A.PROCESS_ID = C.PROCESS_ID
                            AND B.ENABLED='Y' 
                            AND C.ENABLED='Y' 
                           ORDER BY B.STEP_ITEM_CODE ";
           
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODEL_ID", _sPKFieldIDValue };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            _iRowCount = dsTemp.Tables[0].Rows.Count;
            string sProcessName = "";
            string sToolingNo = "";
            string sToolingSN = "";
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                TreeNode NodeProcess = new TreeNode();
                TreeNode NodeToolingNo = new TreeNode();
                TreeNode NodeToolingSN = new TreeNode();
                Dictionary<string, string> dictTooling = new Dictionary<string, string>();
                if (dr["PROCESS_NAME"].ToString() != sProcessName)
                {
                    sProcessName = dr["PROCESS_NAME"].ToString();
                    NodeProcess.Text = sProcessName;
                    NodeProcess.ImageIndex = 2;
                    NodeProcess.SelectedImageIndex = NodeProcess.ImageIndex;
                    TreeToolingData.Nodes.Add(NodeProcess);
                    _dictProcess.Add(sProcessName, dictTooling);
                    sToolingNo = "";
                }
                _sProcessName = dr["PROCESS_NAME"].ToString();
                NodeProcess = TreeToolingData.Nodes[TreeToolingData.Nodes.Count - 1];
                dictTooling = _dictProcess[sProcessName];


                sToolingNo = "[" + dr["STEP_ITEM_CODE"].ToString() + "]" + dr["STEP_ITEM_NAME"].ToString();
                


                NodeToolingNo.Text = sToolingNo;
                NodeToolingNo.SelectedImageIndex = NodeToolingNo.ImageIndex;

                NodeToolingNo.ImageIndex = 3;
                NodeProcess.Nodes.Add(NodeToolingNo);

                // NodeToolingSN.ImageIndex = 4;
                // NodeToolingSN.SelectedImageIndex = NodeToolingSN.ImageIndex;
                //NodeToolingNo = NodeProcess.Nodes[NodeProcess.Nodes.Count - 1];
                //sToolingSN = dr["STEP_ITEM_CODE"].ToString();
                //NodeToolingSN.Text = sToolingSN;
                //NodeToolingNo.Nodes.Add(NodeToolingSN);
                dictTooling.Add(sToolingNo, sToolingNo);
            }
            TreeToolingData.ExpandAll();
        }

        private void TreeToolingData_DragDrop(object sender, DragEventArgs e)
        {
            //來源Node 
            TreeNode mNode;
            TreeNode SrcNode;
            string sObjectName = (sender as TreeView).Name;
            

            SrcNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode"); //來源Node
            Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            mNode = ((TreeView)sender).GetNodeAt(pt);  //目的Node                                  
            string sDataType = SrcNode.TreeView.Name;
            if (sDataType == sObjectName) //目的與來源相同
                return;

            if (sDataType == "TreeViewProcess")
            {
                if (SrcNode.Level != 1)
                    return;
                Dictionary<string, string> dictTooling = null;
                string sProcessName = SrcNode.Text;
                _dictProcess.TryGetValue(sProcessName, out dictTooling);
                if (dictTooling != null)
                {
                    SajetCommon.Show_Message("Process Duplicate", 0);
                    return;
                }
                else
                {
                    TreeNode NodeProcess = new TreeNode();
                    NodeProcess.Text = sProcessName;
                    NodeProcess.ImageIndex = 2;
                    NodeProcess.SelectedImageIndex = NodeProcess.ImageIndex;
                    TreeToolingData.Nodes.Add(NodeProcess);
                }
            }
            else
            {

                if (mNode == null)
                    mNode = TreeToolingData.Nodes[0];
                    //return;
                int iLevel = mNode.Level;
                
                string sProcessName = "";
                TreeNode NodeProcess = new TreeNode();
                switch (iLevel)
                {
                    case 0: NodeProcess = mNode; break;
                    case 1: NodeProcess = mNode.Parent; ; break;
                    case 2: NodeProcess = mNode.Parent.Parent; break;
                    default: break;
                }
                sProcessName = NodeProcess.Text;
                string sToolingNo = string.Empty, sToolingSN = string.Empty;
                int iQTY = 0;
                if (SrcNode.Level == 1)
                {
                    sToolingNo = SrcNode.Parent.Text;
                    sToolingSN = SrcNode.Text;
                    //檢查重複性
                    Dictionary<string, string> dictTooling = null;

                    _dictProcess.TryGetValue(sProcessName, out dictTooling);
                    bool bToolingSNExist = false;
                    if (dictTooling != null)
                    {
                        string sTemp = string.Empty;
                        dictTooling.TryGetValue(sToolingSN, out sTemp);
                        if (sTemp != null)
                            bToolingSNExist = true;
                    }
                    if (bToolingSNExist)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Tooling SN Duplicate")+" : "+sToolingNo, 0);
                        return;
                    }

                    TreeNode NodeToolingNo = new TreeNode();
                    TreeNode NodeToolingSN = new TreeNode();
                    NodeToolingNo.Text = sToolingNo;
                    NodeToolingSN.Text = sToolingSN;

                    bool bToolingNoExist = false;
                    for (int i = 0; i <= NodeProcess.Nodes.Count - 1; i++)
                    {
                        if (NodeProcess.Nodes[i].Text == sToolingNo)
                        {
                            NodeToolingNo = NodeProcess.Nodes[i];
                            bToolingNoExist = true;
                            break;
                        }
                    }
                    if (!bToolingNoExist)
                    {
                        NodeProcess.Nodes.Add(NodeToolingNo);
                        NodeToolingNo = NodeProcess.Nodes[NodeProcess.Nodes.Count - 1];
                    }

                    NodeToolingNo.ImageIndex = 3;
                    NodeToolingNo.SelectedImageIndex = NodeToolingNo.ImageIndex;
                    NodeToolingSN.ImageIndex = 4;
                    NodeToolingSN.SelectedImageIndex = NodeToolingSN.ImageIndex;
                    NodeToolingNo.Nodes.Add(NodeToolingSN);

                    //加入_dictProcess
                    _dictProcess.TryGetValue(sProcessName, out dictTooling);
                    if (dictTooling == null)
                    {
                        dictTooling = new Dictionary<string, string>();
                        _dictProcess.Add(sProcessName, dictTooling);
                    }
                    dictTooling.Add(sToolingSN, sToolingSN);
                    OnInsertToolingSN(sProcessName, "0", sToolingSN, iQTY);
                    NodeProcess.ExpandAll();
                }
                else if (SrcNode.Nodes.Count == 0)
                {
                    sToolingNo = SrcNode.Text;

                    TreeNode NodeToolingNo = new TreeNode();
                    NodeToolingNo.Text = sToolingNo;
                    for (int i = 0; i <= NodeProcess.Nodes.Count - 1; i++)
                        //if (NodeProcess.Nodes[i].Text.Split('×')[0] == sToolingNo)
                        if (NodeProcess.Nodes[i].Text == sToolingNo)
                        {
                            SajetCommon.Show_Message(SajetCommon.SetLanguage("Step Item Duplicate") + " : " + sToolingNo, 0);
                            //SajetCommon.Show_Message("Step Item Duplicate", 0);
                            return;
                        }
                    /*
                    fQTY f = new fQTY(sToolingNo,0);
                    if (f.ShowDialog() != DialogResult.OK)
                        return;
                    iQTY = Convert.ToInt16(f.txtQTY.Text);
                    f.Dispose();

                    NodeToolingNo.Text += "×" + iQTY;
                     */
                    string iQty = "1";
                    NodeProcess.Nodes.Add(NodeToolingNo);
                    NodeToolingNo = NodeProcess.Nodes[NodeProcess.Nodes.Count - 1];
                    NodeToolingNo.ImageIndex = 3;
                    NodeToolingNo.SelectedImageIndex = NodeToolingNo.ImageIndex;
                    OnInsertToolingSN(sProcessName, sToolingNo, "0", iQTY);
                    NodeProcess.ExpandAll();
                }
            }
        }

        private void TreeToolingData_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void TreeToolingData_DragOver(object sender, DragEventArgs e)
        {
            //當移動到節點上,該節點會Focus並成藍色
            TreeNode DropNode = new TreeNode();
            Point Position = TreeToolingData.PointToClient(new Point(e.X, e.Y));
            DropNode = TreeToolingData.GetNodeAt(Position);
            if (DropNode == null)
                return;


            int iLevel = DropNode.Level;
            TreeToolingData.Focus();
            switch (iLevel)
            {
                case 0: TreeToolingData.SelectedNode = DropNode; break;
                case 1: TreeToolingData.SelectedNode = DropNode.Parent; break;
                case 2: TreeToolingData.SelectedNode = DropNode.Parent.Parent; break;
                default: break;
            }
        }

        private void TreeToolingData_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void collapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeToolingData.CollapseAll();
        }

        private void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeToolingData.ExpandAll();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TreeToolingData.Nodes.Count == 0)
                return;
            if (TreeToolingData.SelectedNode == null)
                return;

            TreeNode treeNode = TreeToolingData.SelectedNode;
            int iLevel = treeNode.Level;
            string sNodeText = treeNode.Text;
            DateTime dtDateTime = ClientUtils.GetSysDate();
            string sProcessName = "", sToolingNO = "", sToolingSN = "";
            Dictionary<string, string> dictTooling = new Dictionary<string, string>();
            switch (iLevel)
            {
                case 0:
                    if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete Process") + " : " + sNodeText + " ?", 2) != DialogResult.Yes)
                        return;
                    sProcessName = sNodeText;
                    OnDeleteProcess(sProcessName);
                    TreeToolingData.Nodes.Remove(treeNode);
                    _dictProcess.TryGetValue(sProcessName, out dictTooling);
                    if (dictTooling != null)
                        _dictProcess.Remove(sProcessName);
                    break;
                case 1:
                    //sNodeText = sNodeText.Split('×')[0];
                    //sNodeText = sNodeText;
                    if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete Step Item") + " : " + sNodeText + " ?", 2) != DialogResult.Yes)
                        return;
                    sProcessName = treeNode.Parent.Text;
                    _dictProcess.TryGetValue(sProcessName, out dictTooling);

                    for (int i = 0; i <= treeNode.Nodes.Count - 1; i++)
                    {
                        sToolingSN = treeNode.Nodes[i].Text;
                        OnDeleteToolingSN(dtDateTime, sProcessName, sNodeText, sToolingSN);
                        if (dictTooling != null)
                            dictTooling.Remove(sToolingSN);
                    }
                    OnDeleteToolingSN(dtDateTime, sProcessName, sNodeText, sToolingSN);
                    TreeToolingData.Nodes.Remove(treeNode);
                    break;
                case 2:
                    if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete Tooling SN") + " : " + sNodeText + " ?", 2) != DialogResult.Yes)
                        return;
                    sProcessName = treeNode.Parent.Parent.Text;
                    sToolingNO = treeNode.Parent.Text;
                    _dictProcess.TryGetValue(sProcessName, out dictTooling);
                    if (dictTooling != null)
                        dictTooling.Remove(sNodeText);
                    OnDeleteToolingSN(dtDateTime, sProcessName, sToolingNO, sNodeText);
                    TreeToolingData.Nodes.Remove(treeNode);
                    break;
                default: break;
            }
            if (TreeToolingData.Nodes.Count == 0)
                AddRoot(_sProcessName);
        }


        private void TreeToolingData_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeToolingData.SelectedNode.SelectedImageIndex = TreeToolingData.SelectedNode.ImageIndex;
        }

        private void uctlProcessTooling_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /*
            if (TreeToolingData.Nodes.Count == 0)
                return;
            if (TreeToolingData.SelectedNode == null)
                return;

            TreeNode treeNode = TreeToolingData.SelectedNode;
            int iLevel = treeNode.Level;
            string sNodeText = treeNode.Text;
            DateTime dtDateTime = ClientUtils.GetSysDate();
            string sProcessName = "", sToolingNO = "";
            Dictionary<string, string> dictTooling = new Dictionary<string, string>();
            switch (iLevel)
            {
                case 1:
                    sToolingNO = sNodeText;//.Split('×')[0];
                    string sQty ="1";// sNodeText.Split('×')[1];
                    int iQty = 0;
                    try
                    {
                        iQty = Convert.ToInt32(sQty);
                    }
                    catch
                    {
                        iQty = 0;
                    }

                    sProcessName = treeNode.Parent.Text;
                    _dictProcess.TryGetValue(sProcessName, out dictTooling);
                    /*
                    fQTY f = new fQTY(sToolingNO,iQty);
                    if (f.ShowDialog() != DialogResult.OK)
                        return;

                    string iQTY = f.txtQTY.Text;
                    f.Dispose();
                     
                    string iQTY = "1";
                    OnUpdateToolingQTY(sProcessName, sToolingNO, iQTY);

                    //sNodeText = sToolingNO + "×" + iQTY;
                    sNodeText = sToolingNO ;//+ "×" + iQTY;

                    ShowTooling();
                    break;
                default:
                    break;
            }
             */ 
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            fDetailData f = new fDetailData();
            try
            {
                f.g_sUpdateType = "APPEND";
                f.g_sformText = btnAppend.Text;
                string sProcessID = SajetCommon.GetID("SAJET.SYS_PROCESS", "PROCESS_ID", "PROCESS_NAME", _sProcessName);
                f.g_sProcessID = sProcessID;
                f.g_sProcessName = _sProcessName;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowTooling();
                    OnAppendStepItem();
                }
            }
            finally
            {
                f.Dispose();
            }
        }
    }
}
