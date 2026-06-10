using SajetClass;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace CWoManagerPcs
{
    public partial class fWoBom : Form
    {
        public fWoBom()
        {
            InitializeComponent();
        }

        public string g_sPartID;
        public string g_sBomID;
        public string g_sRouteID;

        private void editPartFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != 13)
                return;

            string sSQL = "Select Part_NO,Spec1 from Sajet.SYS_Part "
                        + "Where Part_No Like '" + editPartFilter.Text + "%'";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            LVPart.Items.Clear();
            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
            {
                LVPart.Items.Add(DS.Tables[0].Rows[i]["Part_NO"].ToString());
                LVPart.Items[i].SubItems.Add(DS.Tables[0].Rows[i]["Spec1"].ToString());
                LVPart.Items[i].ImageIndex = 2;
            }
        }

        public void ShowBom(string sPartID, string sVer, string sTag = "All")
        {
            DataSet DS;
            string sSQL;

            LVData.Items.Clear();
            //跦?==================================================
            TreeBomData.Nodes.Clear();
            TreeBomData.Nodes.Add(LabPartNo.Text);
            TreeBomData.Nodes[0].ImageIndex = 0;
            TreeBomData.Nodes[0].SelectedImageIndex = 0;
            TreeBomData.Nodes[0].Tag = sVer;

            //赽?===================================================

            //珂梑岆瘁衄WO_BOM
            string sType = "(WO_BOM)";
            string sUpdateFlag = "";
            sSQL = $@"SELECT D.PART_NO ITEM_PART_NO,
       B.ITEM_PART_ID,
       F.PROCESS_NAME,
       B.ITEM_COUNT,
       NVL(B.VERSION, 'N/A') VERSION,
       B.ITEM_GROUP,
       D.PART_TYPE,
       D.SPEC1,
       B.ROWID BOMROWID,
       NVL(B.PROCESS_ID, 0) PROCESS_ID,
       B.BOM_OPTION1,
       B.BOM_OPTION2,
       B.BOM_OPTION3,
       B.BOM_OPTION4,
       CASE B.BOM_OPTION4
         WHEN 0 THEN
          'Assy Part'
         WHEN 1 THEN
          'Key Parts'
         WHEN 2 THEN
          'Lot'
         ELSE
          'Key Parts'
       END BOM_TYPE
  FROM SAJET.G_WO_BOM B, SAJET.SYS_PART D, SAJET.SYS_PROCESS F
 WHERE B.WORK_ORDER = '{LabWO.Text}'
   AND B.PART_ID = '{sPartID}'{(sTag == "All" ? "" : $@"
   AND NVL(B.BOM_OPTION4, 1) = '{sTag}'")}
   AND B.ITEM_PART_ID = D.PART_ID(+)
   AND B.PROCESS_ID = F.PROCESS_ID(+)
 ORDER BY F.PROCESS_NAME, B.ITEM_GROUP, ITEM_PART_NO";
            DS = ClientUtils.ExecuteSQL(sSQL);
            if (DS.Tables[0].Rows.Count == 0)
            {
                sType = "(Default)";
                sUpdateFlag = "Y";
                sSQL = $@"SELECT D.PART_NO ITEM_PART_NO,
       B.ITEM_PART_ID,
       A.BOM_ID,
       F.PROCESS_NAME,
       B.ITEM_COUNT,
       NVL(B.VERSION, 'N/A') VERSION,
       B.ITEM_GROUP,
       D.PART_TYPE,
       D.SPEC1,
       '' BOMROWID,
       NVL(B.PROCESS_ID, 0) PROCESS_ID,
       B.BOM_OPTION1,
       B.BOM_OPTION2,
       B.BOM_OPTION3,
       B.BOM_OPTION4,
       CASE B.BOM_OPTION4
         WHEN 0 THEN
          'Assy Part'
         WHEN 1 THEN
          'Key Parts'
         WHEN 2 THEN
          'Lot'
         ELSE
          'Key Parts'
       END BOM_TYPE,
       A.BOM_ID
  FROM SAJET.SYS_BOM_INFO A,
       SAJET.SYS_BOM      B,
       SAJET.SYS_PART     D,
       SAJET.SYS_PROCESS  F
 WHERE A.PART_ID = '{sPartID}'
   AND A.VERSION = '{sVer}'{(sTag == "All" ? "" : $@"
   AND NVL(B.BOM_OPTION4, 1) = '{sTag}'")}
   AND A.BOM_ID = B.BOM_ID
   AND B.ITEM_PART_ID = D.PART_ID(+)
   AND B.PROCESS_ID = F.PROCESS_ID(+)
 ORDER BY F.PROCESS_NAME, B.ITEM_GROUP, ITEM_PART_NO";
                DS = ClientUtils.ExecuteSQL(sSQL);
                if (DS.Tables[0].Rows.Count > 0)
                {
                    //WO_BOM,鞞尨SYS_BOM,楊党蜊
                    //?珂Save馱BOM Copy善WO BOM摽,符褫党蜊,瘁婓壺麼痄衄?觳
                    TreeBomData.AllowDrop = false;
                    LVPart.AllowDrop = false;
                    MenuItemDelete.Visible = false;
                    MenuItemModify.Visible = false;
                    PopMenu2.Opening -= PopMenu2_Opening;

                }
            }
            LabBomType.Text = SajetCommon.SetLanguage(sType, 1);
            string sPreProcess = "";
            string sProcess = "";
            string sPreRelation = "";

            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
            {
                sProcess = DS.Tables[0].Rows[i]["PROCESS_NAME"].ToString();
                string sItemPartNo = DS.Tables[0].Rows[i]["ITEM_PART_NO"].ToString();
                string sItemCount = DS.Tables[0].Rows[i]["ITEM_COUNT"].ToString();
                string sItemGroup = DS.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                string sSubVersion = DS.Tables[0].Rows[i]["VERSION"].ToString();
                string sPartType = DS.Tables[0].Rows[i]["PART_TYPE"].ToString();
                string sSpec1 = DS.Tables[0].Rows[i]["SPEC1"].ToString();
                string sRowID = DS.Tables[0].Rows[i]["bomrowid"].ToString();
                string sProcessID = DS.Tables[0].Rows[i]["PROCESS_ID"].ToString();
                string sItemPartID = DS.Tables[0].Rows[i]["ITEM_PART_ID"].ToString();
                string sOption1 = DS.Tables[0].Rows[i]["BOM_OPTION1"].ToString();
                string sOption2 = DS.Tables[0].Rows[i]["BOM_OPTION2"].ToString();
                string sOption3 = DS.Tables[0].Rows[i]["BOM_OPTION3"].ToString();
                string sOption4 = DS.Tables[0].Rows[i]["BOM_OPTION4"].ToString();
                string sBomType = SajetCommon.SetLanguage(DS.Tables[0].Rows[i]["BOM_TYPE"].ToString());

                if (string.IsNullOrEmpty(sProcess))
                    sProcess = "N/A";
                LVData.Items.Add(sItemPartNo);             //Item0-Part
                LVData.Items[i].SubItems.Add(sProcess);    //Item1-Process
                LVData.Items[i].SubItems.Add(sItemCount);  //Item2-Qty
                LVData.Items[i].SubItems.Add(sItemGroup);  //Item3-Relation
                LVData.Items[i].SubItems.Add(sSubVersion); //Item4-Version
                LVData.Items[i].SubItems.Add(sPartType);   //Item5-Part_Type
                LVData.Items[i].SubItems.Add(sSpec1);      //Item6-Spec
                //Location ==============================                               
                string sLocation = "";
                DataSet DSLoc;
                if (sType == "(Default)")
                {
                    g_sBomID = DS.Tables[0].Rows[i]["BOM_ID"].ToString();
                    string sSQL1 = " Select Location "
                                 + " From SAJET.SYS_BOM_LOCATION "
                                 + " Where BOM_ID = '" + g_sBomID + "' "
                                 + " And Item_Part_ID = '" + sItemPartID + "' "
                                 + " ORDER BY LOCATION ";
                    DSLoc = ClientUtils.ExecuteSQL(sSQL1);
                }
                else
                {
                    sSQL = $@"SELECT BOM_ID FROM SAJET.SYS_BOM_INFO WHERE PART_ID = '{sPartID}' AND VERSION = '{sVer}'";
                    DSLoc = ClientUtils.ExecuteSQL(sSQL);
                    if (DSLoc.Tables[0].Rows.Count > 0)
                    {
                        g_sBomID = DSLoc.Tables[0].Rows[0][0].ToString();
                    }
                    string sSQL1 = " Select Location "
                                 + " From SAJET.G_WO_BOM_LOCATION "
                                 + " Where WORK_ORDER = '" + LabWO.Text + "' "
                                 + " And Item_Part_ID = '" + sItemPartID + "' "
                                 + " ORDER BY LOCATION ";
                    DSLoc = ClientUtils.ExecuteSQL(sSQL1);
                }
                for (int j = 0; j <= DSLoc.Tables[0].Rows.Count - 1; j++)
                    sLocation = sLocation + DSLoc.Tables[0].Rows[j]["Location"].ToString() + ',';
                String delim = ",";
                sLocation = sLocation.TrimEnd(delim.ToCharArray());
                LVData.Items[i].SubItems.Add(sLocation); //Item7 -Location
                //===========================================

                LVData.Items[i].SubItems.Add(sRowID);      //Item8 -Rowid
                LVData.Items[i].SubItems.Add(sProcessID);  //Item9 -Process_ID
                LVData.Items[i].SubItems.Add(sItemPartID); //Item10 -Item_Part_ID           
                LVData.Items[i].SubItems.Add(sUpdateFlag); //Item11 -UpdateFlag
                LVData.Items[i].SubItems.Add(sOption1); //Item12-BOM_OPTION1
                LVData.Items[i].SubItems.Add(sOption2); //Item13-BOM_OPTION2
                LVData.Items[i].SubItems.Add(sOption3); //Item14-BOM_OPTION3
                LVData.Items[i].SubItems.Add("N"); //Item15-COPY TO SYS_BOM
                LVData.Items[i].SubItems.Add(sOption4); //Item16-BOM_OPTION4
                LVData.Items[i].SubItems.Add(sBomType); //Item17-BomType
                LVData.Items[i].ImageIndex = 2;

                //?TreeView================================================
                //Tree-Process
                if (sPreProcess != sProcess)
                {
                    TreeBomData.Nodes[0].Nodes.Add(sProcess);
                    TreeBomData.Nodes[0].LastNode.ImageIndex = 1;
                    TreeBomData.Nodes[0].LastNode.SelectedImageIndex = 1;
                    TreeBomData.Nodes[0].LastNode.Name = sProcess;
                    sPreRelation = "";
                }
                //Tree-Part
                TreeNode tNode = new TreeNode();
                tNode.Text = sItemPartNo;
                tNode.Tag = i.ToString();  //賸鷂LVData(Tag硉岆LVData腔Row)

                if (sItemGroup == "0" || sPreRelation != sItemGroup)
                {
                    tNode.ImageIndex = 2;
                    tNode.SelectedImageIndex = tNode.ImageIndex;
                    TreeBomData.Nodes[0].LastNode.Nodes.Add(tNode);
                }
                else  //Tree-杸測蹋
                {
                    tNode.ImageIndex = 3;
                    tNode.SelectedImageIndex = tNode.ImageIndex;
                    TreeBomData.Nodes[0].LastNode.LastNode.Nodes.Add(tNode);
                }
                sPreProcess = sProcess;
                sPreRelation = sItemGroup;
            }
            TreeBomData.ExpandAll();
        }

        private void TreeBomData_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //鞞尨腢腔蹋???揃蹋
            LV1.Items.Clear();
            if (TreeBomData.SelectedNode.Level > 1)
            {
                int iIndex = Convert.ToInt32(TreeBomData.SelectedNode.Tag.ToString());
                LV1.Items.Add(LVData.Items[iIndex].Text);
                for (int i = 1; i <= 17; i++)
                {
                    if ((i > 7 && i < 12) || i == 15 || i == 16)
                    {
                        continue;
                    }
                    LV1.Items[0].SubItems.Add(LVData.Items[iIndex].SubItems[i].Text);
                }
                LV1.Items[0].ImageIndex = 2;
                LV1.Items[0].StateImageIndex = LV1.Items[0].ImageIndex;
            }
        }

        private void TreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void TreeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void TreeBomData_DragDrop(object sender, DragEventArgs e)
        {
            //埭Node           
            TreeNode SrcNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
            //醴腔Node   
            TreeNode mNode;
            Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            mNode = ((TreeView)sender).GetNodeAt(pt);
            if (mNode == null)
                mNode = TreeBomData.TopNode;
            TreeBomData.Select();
            TreeBomData.Focus();

            if (SrcNode == null)  //埭岆LVPart,樓陔腔赽蕆蹋?
            {
                string sPart = TreeBomData.Nodes[0].Text;
                string sVer = TreeBomData.Nodes[0].Tag.ToString();
                string sAddPart = LVPart.SelectedItems[0].Text; //郗樓腔赽蹋

                if (F_AppandBomData(sPart, sVer, sAddPart, mNode))
                {
                    //Rowid腔桶尨陔崝,剒Insert
                    for (int i = 0; i <= LVData.Items.Count - 1; i++)
                    {
                        if (LVData.Items[i].SubItems[11].Text == "Y")
                            Update_BOM(i);
                    }
                    TreeBomData.ExpandAll();
                }
            }
            else  //痄埻掛眒衄腔蹋?
            {
                if (SrcNode.Level <= 1)
                    return;
                if (mNode.Level == 0 | mNode.Level > 2)
                    return;
                if (MoveBomData(SrcNode, mNode))
                {
                    //Rowid腔桶尨陔崝,剒Insert
                    for (int i = 0; i <= LVData.Items.Count - 1; i++)
                    {
                        if (LVData.Items[i].SubItems[11].Text == "Y")
                            Update_BOM(i);
                    }
                }
            }
        }

        private bool F_AppandBomData(string sPart, string sVer, string sAddPart, TreeNode tNode)
        {
            string sProcess = "";
            string sCount = "";
            string sRelation = "";
            string sPartVersion = "";
            string sLocation = "";
            string sBomType = "";
            bool bChangeGroup = false;
            int iNodeLevel = tNode.Level;

            if (iNodeLevel == 0) //陔腔process
            {
                sProcess = "";
                sCount = "1";
                sRelation = "0";
                sPartVersion = "";
                sLocation = "";
                sBomType = "1";

            }
            else if (iNodeLevel == 1) //婓肮?腔process狟樓翋蹋
            {
                sProcess = tNode.Text;
                sCount = "1";
                sRelation = "0";
                sPartVersion = "";
                sLocation = "";
                sBomType = "1";
                //蹋?
                string sProcessID = getProcessID(sProcess);
                if (!CheckDup(sProcessID, sAddPart, 0))
                {
                    string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                    SajetCommon.Show_Message(sMsg + Environment.NewLine + sAddPart, 0);
                    return false;
                }
            }
            else if (iNodeLevel == 2) //婓Part笢崝樓珨杸測蹋
            {
                sProcess = tNode.Parent.Text;

                if (sAddPart == tNode.Text)
                    return false;
                else
                {
                    string sProcessID = getProcessID(sProcess);
                    if (!CheckDup(sProcessID, sAddPart, 0))
                    {
                        string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                        SajetCommon.Show_Message(sMsg + Environment.NewLine + sAddPart, 0);
                        return false;
                    }
                }

                int iIndex = System.Convert.ToInt32(tNode.Tag.ToString());
                sCount = LVData.Items[iIndex].SubItems[2].Text;
                sRelation = LVData.Items[iIndex].SubItems[3].Text;
                sPartVersion = LVData.Items[iIndex].SubItems[4].Text;
                sLocation = LVData.Items[iIndex].SubItems[7].Text;
                sBomType = LVData.Items[iIndex].SubItems[16].Text;

                //埻掛杸測蹋,剒group蜊準0腔硉 
                if (sRelation == "0")
                {
                    int iRelation = 0;
                    foreach (TreeNode item in tNode.Parent.Nodes)
                    {
                        string _sRelation = LVData.Items[Convert.ToInt32(item.Tag.ToString())].SubItems[3].Text;

                        if (int.TryParse(_sRelation, out int _iRelation))
                        {
                            if (iRelation < _iRelation)
                            {
                                iRelation = _iRelation;
                            }
                        }

                    }
                    sRelation = (iRelation + 1).ToString();
                    bChangeGroup = true;
                }
            }
            else if (iNodeLevel == 3) //婓杸測蹋笢崝樓珨杸測蹋
            {
                //Part笭恚
                for (int i = 0; i <= tNode.Parent.Nodes.Count - 1; i++)
                {
                    string sData = tNode.Parent.Nodes[i].Text;
                    if (sData == sAddPart)
                    {
                        string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                        SajetCommon.Show_Message(sMsg + Environment.NewLine + sAddPart, 0);
                        return false;
                    }
                }

                int iIndex = System.Convert.ToInt32(tNode.Tag.ToString());
                sProcess = tNode.Parent.Parent.Text;
                sCount = LVData.Items[iIndex].SubItems[2].Text;
                sRelation = LVData.Items[iIndex].SubItems[3].Text;
                sPartVersion = LVData.Items[iIndex].SubItems[4].Text;
                sLocation = LVData.Items[iIndex].SubItems[7].Text;
                sBomType = LVData.Items[iIndex].SubItems[16].Text;
            }
            else
            {
                return false;
            }

            fBomData_New f = new fBomData_New();

            f.LabWorkOrder.Text = LabWO.Text;
            f.LabPart.Text = sPart;
            f.LabVer.Text = sVer;
            f.g_sSelectProcess = sProcess;
            f.editSubPart.Text = sAddPart;
            f.editQty.Text = sCount;
            f.editSubPartVer.Text = "";//sPartVersion;
            f.editGroup.Text = sRelation;
            f.g_sChangeGroup = bChangeGroup;
            f.g_sBomType = sBomType;
            f.g_sRouteID = g_sRouteID;
            string[] split = sLocation.Split(new Char[] { ',' });
            f.editLocation.Lines = split;

            if (iNodeLevel >= 2)
            {
                f.editSubPart.Enabled = false;
                f.combProcess.Enabled = false;
                f.editQty.Enabled = false;
                f.editGroup.Enabled = bChangeGroup;
            }
            else if (iNodeLevel == 1)
            {
                f.editSubPart.Enabled = false;
                f.combProcess.Enabled = false;
                f.editGroup.Enabled = false;
            }
            else if (iNodeLevel == 0)
            {
                f.editGroup.Enabled = false;
            }

            // =======Show Form==========================================================
            if (f.ShowDialog() != DialogResult.OK)
            {
                return false;
            }

            sAddPart = f.editSubPart.Text;
            //?載衄杸測蹋燊,剒肮載蜊GROUP
            if (bChangeGroup)
            {
                int iTag = System.Convert.ToInt32(tNode.Tag.ToString());
                LVData.Items[iTag].SubItems[3].Text = f.editGroup.Text;
                LVData.Items[iTag].SubItems[11].Text = "Y"; //Y1桶尨森遁揃蹋衄載,剒Update DB
            }

            sProcess = f.combProcess.Text.Trim();
            if (sProcess == "")
                sProcess = "N/A";
            //樓陔?===================================================
            if (iNodeLevel == 0) //剒珂膘蕾Process腔?
            {
                //梑岆瘁眒衄森process腔node  
                TreeNode[] tFindProcessNodes = TreeBomData.Nodes[0].Nodes.Find(sProcess, false);

                if (tFindProcessNodes.Length == 0)
                {
                    TreeNode tProcessNode = new TreeNode();
                    tProcessNode.Text = sProcess;
                    tProcessNode.ImageIndex = iNodeLevel + 1;
                    tProcessNode.SelectedImageIndex = tProcessNode.ImageIndex;
                    tProcessNode.Name = sProcess;
                    tNode.Nodes.Add(tProcessNode);
                    tNode = tNode.LastNode;
                }
                else
                {
                    tNode = tFindProcessNodes[0];
                }
                iNodeLevel = 1;
            }

            if (iNodeLevel == 3) //岆迍珝善杸測蹋?奻,Tree Node膘婓肮珨
            {
                iNodeLevel = iNodeLevel - 1;
                tNode = tNode.Parent;
            }
            int iRwoCount = LVData.Items.Count;
            TreeNode t1 = new TreeNode();
            t1.Text = sAddPart;
            t1.Tag = iRwoCount.ToString();
            t1.ImageIndex = iNodeLevel + 1;
            t1.SelectedImageIndex = t1.ImageIndex;
            tNode.Nodes.Add(t1);

            LVData.Items.Add(sAddPart);  //Item0-Part
            LVData.Items[iRwoCount].SubItems.Add(sProcess);    //Item1-Process
            LVData.Items[iRwoCount].SubItems.Add(f.editQty.Text);        //Item2-Qty
            LVData.Items[iRwoCount].SubItems.Add(f.editGroup.Text);      //Item3-Relation
            LVData.Items[iRwoCount].SubItems.Add(f.editSubPartVer.Text); //Item4-Version
            LVData.Items[iRwoCount].SubItems.Add(f.g_sItemPartType);     //Item5-Part_Type 
            LVData.Items[iRwoCount].SubItems.Add(f.g_sItemSpec1);        //Item6-Spec

            //Location==
            sLocation = "";
            for (int j = 0; j <= f.editLocation.Lines.Length - 1; j++)
            {
                sLocation = sLocation + f.editLocation.Lines[j].ToString() + ',';
            }
            String delim = ",";
            sLocation = sLocation.TrimEnd(delim.ToCharArray());
            LVData.Items[iRwoCount].SubItems.Add(sLocation);  //Item7 -Location
            //==            
            LVData.Items[iRwoCount].SubItems.Add("");  //Item8 -Rowid
            LVData.Items[iRwoCount].SubItems.Add(f.g_sProcessID);  //Item9 -Process_ID
            LVData.Items[iRwoCount].SubItems.Add(f.g_sItemPartID);  //Item10 -Item_Part_ID
            LVData.Items[iRwoCount].SubItems.Add("Y"); //Item11 -Update Flag
            LVData.Items[iRwoCount].SubItems.Add(f.tbKPSNLen.Text);        //Item12-BOM_OPTION1
            LVData.Items[iRwoCount].SubItems.Add(f.tbChkString.Text);        //Item13-BOM_OPTION2
            LVData.Items[iRwoCount].SubItems.Add(f.tbChkIndex.Text);        //Item14-BOM_OPTION3
            LVData.Items[iRwoCount].SubItems.Add(f.cbCopyBom.Checked ? "Y" : "N");        //Item15 Copy To SYS_BOM
            LVData.Items[iRwoCount].SubItems.Add(f.g_sBomType);        //Item16-BomType
            LVData.Items[iRwoCount].SubItems.Add(f.g_sBomTypeText);        //Item17 -BOM_OPTION4
            LVData.Items[iRwoCount].ImageIndex = 2;
            LVData.Items[iRwoCount].StateImageIndex = LVData.Items[iRwoCount].ImageIndex;


            f.Dispose();
            return true;
        }

        private void Update_BOM(int iRow)
        {
            string sSQL = "";
            string sITEM_COUNT = LVData.Items[iRow].SubItems[2].Text;
            string sITEM_GROUP = LVData.Items[iRow].SubItems[3].Text;
            string sVERSION = LVData.Items[iRow].SubItems[4].Text;
            string sRowID = LVData.Items[iRow].SubItems[8].Text;
            string sPROCESS_ID = LVData.Items[iRow].SubItems[9].Text;
            string sITEM_PART_ID = LVData.Items[iRow].SubItems[10].Text;
            string sLocation = LVData.Items[iRow].SubItems[7].Text;
            string sOption1 = LVData.Items[iRow].SubItems[12].Text;
            string sOption2 = LVData.Items[iRow].SubItems[13].Text;
            string sOption3 = LVData.Items[iRow].SubItems[14].Text;
            string sOption4 = LVData.Items[iRow].SubItems[16].Text;
            if (sVERSION == "")
                sVERSION = "N/A";
            if (sRowID == "")
            {
                sSQL = " Insert Into SAJET.G_WO_BOM "
                     + " (WORK_ORDER,PART_ID,ITEM_PART_ID,ITEM_GROUP,ITEM_COUNT "
                     + "  ,PROCESS_ID,VERSION,UPDATE_USERID,BOM_OPTION1,BOM_OPTION2,BOM_OPTION3,BOM_OPTION4) "
                     + " Values "
                     + " ('" + LabWO.Text + "','" + g_sPartID + "','" + sITEM_PART_ID + "','" + sITEM_GROUP + "','" + sITEM_COUNT + "' "
                     + " ,'" + sPROCESS_ID + "','" + sVERSION + "','" + ClientUtils.UserPara1 + "','" + sOption1 + "','" + sOption2 + "','" + sOption3 + "','" + sOption4 + "') ";
                ClientUtils.ExecuteSQL(sSQL);

                //Insert Bom Location====
                sSQL = " Delete SAJET.G_WO_BOM_LOCATION "
                     + " Where WORK_ORDER = '" + LabWO.Text + "' "
                     + " and Item_Part_Id = '" + sITEM_PART_ID + "' ";
                ClientUtils.ExecuteSQL(sSQL);

                string[] split = sLocation.Split(new Char[] { ',' });
                for (int i = 0; i <= split.Length - 1; i++)
                {
                    sSQL = " Insert Into SAJET.G_WO_BOM_LOCATION "
                         + " (WORK_ORDER,PART_ID,ITEM_PART_ID,ITEM_GROUP,LOCATION,UPDATE_USERID) "
                         + " Values "
                         + " ('" + LabWO.Text + "','" + g_sPartID + "','" + sITEM_PART_ID + "','" + sITEM_GROUP + "','" + split.GetValue(i).ToString() + "' "
                         + " ,'" + ClientUtils.UserPara1 + "') ";
                    ClientUtils.ExecuteSQL(sSQL);
                }

                //梑森遁RowID
                sSQL = " Select Rowid from SAJET.G_WO_BOM "
                     + " Where WORK_ORDER = '" + LabWO.Text + "' "
                     + " and Item_Part_Id = '" + sITEM_PART_ID + "' "
                     + " and NVL(Process_ID,0) = '" + sPROCESS_ID + "' ";
                DataSet DS = ClientUtils.ExecuteSQL(sSQL);
                LVData.Items[iRow].SubItems[8].Text = DS.Tables[0].Rows[0]["RowID"].ToString();
            }
            else
            {
                sSQL = " Update SAJET.G_WO_BOM "
                     + " Set ITEM_GROUP = '" + sITEM_GROUP + "' "
                     + "   ,ITEM_COUNT = '" + sITEM_COUNT + "' "
                     + "   ,PROCESS_ID = '" + sPROCESS_ID + "' "
                     + "   ,VERSION = '" + sVERSION + "' "
                     + "   ,BOM_OPTION1 = '" + sOption1 + "' "
                     + "   ,BOM_OPTION2 = '" + sOption2 + "' "
                     + "   ,BOM_OPTION3 = '" + sOption3 + "' "
                     + "   ,BOM_OPTION4 = '" + sOption4 + "' "
                     + "   ,UPDATE_USERID = '" + ClientUtils.UserPara1 + "' "
                     + "   ,UPDATE_TIME = SYSDATE "
                     + " Where Rowid = '" + sRowID + "'";
                ClientUtils.ExecuteSQL(sSQL);
            }

            //COPY TO SYS_BOM
            if (LVData.Items[iRow].SubItems[15].Text == "Y")
            {

                if (string.IsNullOrEmpty(g_sBomID))
                {
                    string sBomID = SajetCommon.GetMaxID("SAJET.SYS_BOM_INFO", "BOM_ID", 8);
                    sSQL = $@"BEGIN
  INSERT INTO SAJET.SYS_BOM_INFO
    (BOM_ID, PART_ID, VERSION, UPDATE_USERID)
    SELECT '{sBomID}',
           '{g_sPartID}',
           '{LabVer.Text}',
           '{ClientUtils.UserPara1}'
      FROM DUAL;
--  INSERT INTO SAJET.SYS_HT_BOM_INFO
--    (BOM_ID, CHECK_DFIFILE, DFIFILE_USERID, DFIFILE_TIME, UPDATE_TYPE)
--    SELECT T.BOM_ID, T.CHECK_DFIFILE, T.DFIFILE_USERID, T.DFIFILE_TIME, '3'
--      FROM SAJET.SYS_BOM_INFO T
--     WHERE T.BOM_ID = '{sBomID}';
END;";
                    ClientUtils.ExecuteSQL(sSQL);

                    g_sBomID = sBomID;
                }

                sSQL = $@"SELECT ROWID
  FROM SAJET.SYS_BOM
 WHERE BOM_ID = '{g_sBomID}'
   AND ITEM_PART_ID = '{sITEM_PART_ID}'
   AND NVL(PROCESS_ID, 0) = '{sPROCESS_ID}'";
                DataTable dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];
                string rowid = string.Empty;
                if (dtTemp.Rows.Count > 0)
                {
                    rowid = dtTemp.Rows[0][0].ToString();
                }
                //更新SYS_BOM
                sSQL = $@"BEGIN
  MERGE INTO SAJET.SYS_BOM T1
  USING (SELECT '{g_sBomID}' BOM_ID,
                '{sITEM_PART_ID}' ITEM_PART_ID,
                '{sVERSION}' VERSION,
                '{sPROCESS_ID}' PROCESS_ID
           FROM DUAL) T2
  ON (T1.ROWID = '{rowid}' AND T1.BOM_ID = T2.BOM_ID)
  WHEN MATCHED THEN
    UPDATE
       SET T1.PROCESS_ID    = T2.PROCESS_ID,
           T1.UPDATE_USERID = '{ClientUtils.UserPara1}',
           T1.UPDATE_TIME   = SYSDATE,
           T1.ITEM_GROUP    = '{sITEM_GROUP}',
           T1.ITEM_COUNT    = '{sITEM_COUNT}',
           T1.VERSION       = T2.VERSION,
           T1.LOCATION      = '{sLocation}',
           T1.BOM_OPTION1   = '{sOption1}',
           T1.BOM_OPTION2   = '{sOption2}',
           T1.BOM_OPTION3   = '{sOption3}',
           T1.BOM_OPTION4   = '{sOption4}'
  WHEN NOT MATCHED THEN
    INSERT
      (T1.BOM_ID,
       T1.ITEM_PART_ID,
       T1.ITEM_GROUP,
       T1.ITEM_COUNT,
       T1.PROCESS_ID,
       T1.VERSION,
       T1.UPDATE_USERID,
       T1.LOCATION,
       T1.BOM_OPTION1,
       T1.BOM_OPTION2,
       T1.BOM_OPTION3,
       T1.BOM_OPTION4)
    VALUES
      (T2.BOM_ID,
       T2.ITEM_PART_ID,
       '{sITEM_GROUP}',
       '{sITEM_COUNT}',
       T2.PROCESS_ID,
       T2.VERSION,
       '{ClientUtils.UserPara1}',
       '{sLocation}',
       '{sOption1}',
       '{sOption2}',
       '{sOption3}',
       '{sOption4}');

  INSERT INTO SAJET.SYS_HT_BOM
    (BOM_ID,
     ITEM_PART_ID,
     ITEM_GROUP,
     ITEM_COUNT,
     PROCESS_ID,
     VERSION,
     UPDATE_USERID,
     UPDATE_TIME,
     ENABLED,
     LOCATION,
     UNIT,
     ITEM_SEQ,
     IS_MATERIAL,
     PURCHASE,
     PRIMARY_FLAG,
     BOM_OPTION1,
     BOM_OPTION2,
     BOM_OPTION3,
     BOM_OPTION4)
    SELECT T.BOM_ID,
           T.ITEM_PART_ID,
           T.ITEM_GROUP,
           T.ITEM_COUNT,
           T.PROCESS_ID,
           T.VERSION,
           T.UPDATE_USERID,
           T.UPDATE_TIME,
           T.ENABLED,
           T.LOCATION,
           T.UNIT,
           T.ITEM_SEQ,
           T.IS_MATERIAL,
           T.PURCHASE,
           T.PRIMARY_FLAG,
           T.BOM_OPTION1,
           T.BOM_OPTION2,
           T.BOM_OPTION3,
           T.BOM_OPTION4
      FROM SAJET.SYS_BOM T
     WHERE T.BOM_ID = '{g_sBomID}'
       AND T.ITEM_PART_ID = '{sITEM_PART_ID}'
       AND NVL(T.VERSION, 'N/A') = '{sVERSION}'
       AND T.PROCESS_ID = '{sPROCESS_ID}';
END;";
                ClientUtils.ExecuteSQL(sSQL);

            }

            LVData.Items[iRow].SubItems[11].Text = ""; ;
        }

        private bool MoveBomData(TreeNode tSrcNode, TreeNode tTargetNode)
        {
            string sTProcess = ""; //醴Process
            string sProcess = "";
            string sTProcessID = ""; //醴Process ID

            int iSrcInx = System.Convert.ToInt32(tSrcNode.Tag);
            //樓傖杸測蹋========================================================
            if (tTargetNode.Level == 2)
            {
                int iTargetInx = System.Convert.ToInt32(tTargetNode.Tag);
                sTProcess = tTargetNode.Parent.Text;
                sTProcessID = getProcessID(sTProcess);
                if (tSrcNode.Level == 2)
                    sProcess = tSrcNode.Parent.Text;
                else
                    sProcess = tSrcNode.Parent.Parent.Text;

                //Process祥肮
                if (sTProcess != sProcess)
                {
                    //森Process狟眒衄森蹋
                    if (!CheckDup(sTProcessID, tSrcNode.Text, 0))
                    {
                        string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                        SajetCommon.Show_Message(sMsg + Environment.NewLine + tSrcNode.Text, 0);
                        return false;
                    }
                    //
                    if (tSrcNode.Text != tTargetNode.Text)
                    {
                        bool bResult = false;
                        for (int i = 0; i <= tTargetNode.Nodes.Count - 1; i++)
                        {
                            if (tSrcNode.Text == tTargetNode.Nodes[i].Text)
                            {
                                bResult = true;
                                break;
                            }
                        }
                        if (!bResult)
                        {
                            if (!CheckDup(sTProcessID, tSrcNode.Text, 0))
                            {
                                string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                                SajetCommon.Show_Message(sMsg + Environment.NewLine + tSrcNode.Text, 0);
                                return false;
                            }
                        }
                    }
                }
                else   //Process眈肮
                {
                    if (tSrcNode.Level == 3)
                    {
                        for (int i = 0; i <= tTargetNode.Nodes.Count - 1; i++)
                        {
                            if (tSrcNode.Text == tTargetNode.Nodes[i].Text)
                            {
                                string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                                SajetCommon.Show_Message(sMsg + Environment.NewLine + tSrcNode.Text, 0);
                                return false;
                            }
                        }
                    }
                }

                //醴埻掛杸測蹋,剒蜊?傖陔腔Relation?徨
                string sTargetRelation = LVData.Items[iTargetInx].SubItems[3].Text;
                if (sTargetRelation == "0")
                {
                    sTargetRelation = F_GETMAXGROUP(LabWO.Text);
                    LVData.Items[iTargetInx].SubItems[3].Text = sTargetRelation;
                    LVData.Items[iTargetInx].SubItems[11].Text = "Y";
                }
                LVData.Items[iSrcInx].SubItems[3].Text = sTargetRelation;
                LVData.Items[iSrcInx].SubItems[11].Text = "Y";
                LVData.Items[iSrcInx].SubItems[1].Text = sTProcess;
                LVData.Items[iSrcInx].SubItems[9].Text = sTProcessID;
            }
            else
            //樓傖翋蹋============================================================
            {
                sTProcess = tTargetNode.Text;
                sTProcessID = getProcessID(sTProcess);
                if (tSrcNode.Level == 2)
                {
                    sProcess = tSrcNode.Parent.Text;
                    if (sProcess == sTProcess)
                        return false;
                }
                else
                    sProcess = tSrcNode.Parent.Parent.Text;
                if (sProcess != sTProcess)
                {
                    if (!CheckDup(sTProcessID, tSrcNode.Text, 0))
                    {
                        string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                        SajetCommon.Show_Message(sMsg + Environment.NewLine + tSrcNode.Text, 0);
                        return false;
                    }
                }
                else
                {
                    if (!CheckDup(sTProcessID, tSrcNode.Text, 1))
                    {
                        string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                        SajetCommon.Show_Message(sMsg + Environment.NewLine + tSrcNode.Text, 0);
                        return false;
                    }
                }
                LVData.Items[iSrcInx].SubItems[3].Text = "0";
                LVData.Items[iSrcInx].SubItems[11].Text = "Y";
                LVData.Items[iSrcInx].SubItems[1].Text = sTProcess;
                LVData.Items[iSrcInx].SubItems[9].Text = sTProcessID;
            }

            //埻掛腔蹋杸測蹋,ITEM GROUP剒蜊傖0
            if (tSrcNode.Level == 3)
            {
                if (tSrcNode.Parent.Nodes.Count == 1)
                {
                    int iParantInx = System.Convert.ToInt32(tSrcNode.Parent.Tag);
                    LVData.Items[iParantInx].SubItems[3].Text = "0";
                    LVData.Items[iParantInx].SubItems[11].Text = "Y";
                }
            }

            TreeNode tNewNode = new TreeNode();
            tNewNode.Text = tSrcNode.Text;
            tNewNode.Tag = tSrcNode.Tag;
            tNewNode.ImageIndex = tTargetNode.Level + 1;
            tNewNode.SelectedImageIndex = tNewNode.ImageIndex;
            tTargetNode.Nodes.Add(tNewNode);
            tSrcNode.Remove();
            tTargetNode.Expand();
            return true;
        }

        private string getProcessID(string sProcessName)
        {
            string sSQL = "Select Process_ID from sajet.sys_process "
                        + "where process_name = '" + sProcessName + "' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            if (DS.Tables[0].Rows.Count > 0)
                return DS.Tables[0].Rows[0]["Process_ID"].ToString();
            else
                return "0";
        }

        private bool CheckDup(string sProcessID, string sItemPart, int iCount)
        {
            string sSQL = " Select count(*) sCount "
                        + " from sajet.g_wo_bom a, sajet.sys_part b "
                        + " where a.work_order = '" + LabWO.Text + "' "
                        + " and NVL(a.Process_ID,'0') = '" + sProcessID + "' "
                        + " and a.Item_Part_ID = b.part_id "
                        + " and b.Part_No= '" + sItemPart + "' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            if (iCount.ToString() == DS.Tables[0].Rows[0]["sCount"].ToString())
                return true;
            else
                return false;
        }

        private string F_GETMAXGROUP(string sWO)
        {
            string sSQL = "Select MAX(ITEM_GROUP)+1 ITEM_GROUP from sajet.g_wo_bom "
                        + "where work_order = '" + sWO + "' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            string sItemGroup = DS.Tables[0].Rows[0]["ITEM_GROUP"].ToString();
            return sItemGroup;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeBomData.CollapseAll();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            TreeBomData.ExpandAll();
        }

        private void MenuItemDelete_Click(object sender, EventArgs e)
        {
            string sSQL = "";
            int iNodeLevel = TreeBomData.SelectedNode.Level;
            if (iNodeLevel == 0) //壺淕?BOM
            {
                string sMsg = SajetCommon.SetLanguage("Delete this WO BOM", 1);
                if (SajetCommon.Show_Message(sMsg + " ?", 2) != DialogResult.Yes)
                    return;

                sSQL = " DELETE SAJET.G_WO_BOM "
                     + " WHERE WORK_ORDER = '" + LabWO.Text + "' ";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                sSQL = " DELETE SAJET.G_WO_BOM_LOCATION "
                     + " WHERE WORK_ORDER = '" + LabWO.Text + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
            else if (iNodeLevel == 1) //壺森process狟垀衄蹋
            {
                string sMsg = SajetCommon.SetLanguage("Delete all Part of this Process", 1);
                if (SajetCommon.Show_Message(sMsg + " ?", 2) != DialogResult.Yes)
                    return;

                int iRow = System.Convert.ToInt32(TreeBomData.SelectedNode.Nodes[0].Tag.ToString());
                string sProcessID = LVData.Items[iRow].SubItems[9].Text;

                for (int i = 0; i <= TreeBomData.SelectedNode.Nodes.Count - 1; i++)
                {
                    int iIndex = System.Convert.ToInt32(TreeBomData.SelectedNode.Nodes[i].Tag.ToString());
                    string sItemPartID = LVData.Items[iIndex].SubItems[10].Text;
                    string sRowID = LVData.Items[iIndex].SubItems[8].Text;
                    //森BOM笢眒?衄眈肮腔Part,壺Location
                    sSQL = " SELECT ITEM_PART_ID FROM SAJET.G_WO_BOM "
                         + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                         + " AND ITEM_PART_ID = '" + sItemPartID + "' "
                         + " AND ROWID <> '" + sRowID + "' "
                         + " AND ROWNUM=1 ";
                    DataSet DS = ClientUtils.ExecuteSQL(sSQL);
                    if (DS.Tables[0].Rows.Count == 0)
                    {
                        sSQL = " DELETE SAJET.G_WO_BOM_LOCATION "
                             + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                             + " AND ITEM_PART_ID = '" + sItemPartID + "' ";
                        ClientUtils.ExecuteSQL(sSQL);
                    }
                }

                sSQL = " DELETE SAJET.G_WO_BOM "
                     + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                     + " AND NVL(PROCESS_ID,'0') = '" + sProcessID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
            }
            else  //壺議珨?蹋
            {
                string sMsg = SajetCommon.SetLanguage("Delete this Part", 1);
                if (SajetCommon.Show_Message(sMsg + " ?", 2) != DialogResult.Yes)
                    return;

                int iRow = System.Convert.ToInt32(TreeBomData.SelectedNode.Tag.ToString());
                string sProcessID = LVData.Items[iRow].SubItems[9].Text;
                string sItemPartID = LVData.Items[iRow].SubItems[10].Text;
                string sItemGroup = LVData.Items[iRow].SubItems[3].Text;

                sSQL = " DELETE SAJET.G_WO_BOM "
                     + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                     + " AND ITEM_PART_ID = '" + sItemPartID + "' "
                     + " AND PROCESS_ID = '" + sProcessID + "' ";
                ClientUtils.ExecuteSQL(sSQL);

                //壺摽杸測蹋,ITEM GROUP蜊0
                if (sItemGroup != "0")
                {
                    sSQL = " SELECT COUNT(*) CNT FROM SAJET.G_WO_BOM "
                         + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                         + " AND ITEM_GROUP = '" + sItemGroup + "' ";
                    DataSet DS = ClientUtils.ExecuteSQL(sSQL);
                    if (DS.Tables[0].Rows[0]["CNT"].ToString() == "1")
                    {
                        sSQL = " UPDATE SAJET.G_WO_BOM "
                             + " SET ITEM_GROUP = '0' "
                             + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                             + " AND ITEM_GROUP = '" + sItemGroup + "' ";
                        ClientUtils.ExecuteSQL(sSQL);
                    }
                }

                //森BOM笢眒?衄眈肮腔Part,壺Location
                sSQL = " SELECT ITEM_PART_ID FROM SAJET.G_WO_BOM "
                     + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                     + " AND ITEM_PART_ID = '" + sItemPartID + "' "
                     + " AND ROWNUM=1 ";
                DataSet DS1 = ClientUtils.ExecuteSQL(sSQL);
                if (DS1.Tables[0].Rows.Count == 0)
                {
                    sSQL = " DELETE SAJET.G_WO_BOM_LOCATION "
                         + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                         + " AND ITEM_PART_ID = '" + sItemPartID + "' ";
                    ClientUtils.ExecuteSQL(sSQL);
                }
            }
            string sTag = "";
            if (rbAssyPart.Checked)
            {
                sTag = "0";
            }
            else if (rbKeyParts.Checked)
            {
                sTag = "1";
            }
            ShowBom(g_sPartID, LabVer.Text, sTag);
        }

        private void TreeBomData_DragOver(object sender, DragEventArgs e)
        {
            TreeNode DropNode = new TreeNode();
            Point Position = TreeBomData.PointToClient(new Point(e.X, e.Y));
            DropNode = TreeBomData.GetNodeAt(Position);
            if (DropNode != null)
            {
                TreeBomData.Focus();
                TreeBomData.SelectedNode = DropNode;
            }
        }

        private void fWoBom_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
        }

        private void MenuItemModify_Click(object sender, EventArgs e)
        {
            var tNode = TreeBomData.SelectedNode;
            int iNodeLevel = tNode.Level;

            int iIndex = System.Convert.ToInt32(tNode.Tag.ToString());
            string sProcess = string.Empty;
            if (tNode.Level == 3)
            {
                sProcess = tNode.Parent.Parent.Text;
            }
            else
            {
                sProcess = tNode.Parent.Text;
            }
            string sCount = LVData.Items[iIndex].SubItems[2].Text;
            string sRelation = LVData.Items[iIndex].SubItems[3].Text;
            string sPartVersion = LVData.Items[iIndex].SubItems[4].Text;
            string sLocation = LVData.Items[iIndex].SubItems[7].Text;
            bool bChangeGroup = false;
            //if (sRelation == "0")
            //{
            //    sRelation = "";
            //    bChangeGroup = true;
            //}
            fBomData_New f = new fBomData_New();
            f.LabWorkOrder.Text = LabWO.Text;
            f.LabPart.Text = LabPartNo.Text;
            f.LabVer.Text = LabVer.Text;
            f.g_sSelectProcess = sProcess;
            f.g_sOldProcess = sProcess;
            f.editSubPart.Text = tNode.Text;
            f.editQty.Text = sCount;
            f.tbKPSNLen.Text = LVData.Items[iIndex].SubItems[12].Text;
            f.tbChkString.Text = LVData.Items[iIndex].SubItems[13].Text;
            f.tbChkIndex.Text = LVData.Items[iIndex].SubItems[14].Text;
            f.editSubPartVer.Text = "";
            f.editGroup.Text = sRelation;
            f.g_sChangeGroup = bChangeGroup;
            string[] split = sLocation.Split(new Char[] { ',' });
            f.editLocation.Lines = split;
            f.g_sFunc = "MODIFY";

            f.editSubPart.Enabled = false;
            f.combProcess.Enabled = true;
            f.editQty.Enabled = true;
            f.editGroup.Enabled = bChangeGroup;
            f.g_sRouteID = g_sRouteID;

            f.g_sBomType = LVData.Items[iIndex].SubItems[16].Text;
            // =======Show Form==========================================================
            if (f.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string sAddPart = f.editSubPart.Text;
            //?載衄杸測蹋燊,剒肮載蜊GROUP
            if (bChangeGroup)
            {
                int iTag = System.Convert.ToInt32(tNode.Tag.ToString());
                LVData.Items[iTag].SubItems[3].Text = f.editGroup.Text;
                LVData.Items[iTag].SubItems[11].Text = "Y"; //Y1桶尨森遁揃蹋衄載,剒Update DB
            }

            sProcess = f.combProcess.Text.Trim();
            if (sProcess == "")
                sProcess = "N/A";
            //樓陔?===================================================

            LVData.Items[iIndex].SubItems[1].Text = sProcess;    //Item1-Process
            LVData.Items[iIndex].SubItems[2].Text = f.editQty.Text;        //Item2-Qty
            LVData.Items[iIndex].SubItems[3].Text = f.editGroup.Text;      //Item3-Relation
            LVData.Items[iIndex].SubItems[4].Text = f.editSubPartVer.Text; //Item4-Version
            LVData.Items[iIndex].SubItems[5].Text = f.g_sItemPartType;     //Item5-Part_Type 
            LVData.Items[iIndex].SubItems[6].Text = f.g_sItemSpec1;        //Item6-Spec

            //Location==
            sLocation = "";
            for (int j = 0; j <= f.editLocation.Lines.Length - 1; j++)
            {
                sLocation = sLocation + f.editLocation.Lines[j].ToString() + ',';
            }
            String delim = ",";
            sLocation = sLocation.TrimEnd(delim.ToCharArray());
            LVData.Items[iIndex].SubItems[7].Text = sLocation;  //Item7 -Location
            //==            
            LVData.Items[iIndex].SubItems[9].Text = f.g_sProcessID;  //Item9 -Process_ID
            LVData.Items[iIndex].SubItems[10].Text = f.g_sItemPartID;  //Item10 -Item_Part_ID
            LVData.Items[iIndex].SubItems[11].Text = "Y"; //Item11 -Update Flag
            LVData.Items[iIndex].SubItems[12].Text = f.tbKPSNLen.Text;        //Item12-BOM_OPTION1
            LVData.Items[iIndex].SubItems[13].Text = f.tbChkString.Text;        //Item13-BOM_OPTION2
            LVData.Items[iIndex].SubItems[14].Text = f.tbChkIndex.Text;        //Item14-BOM_OPTION3
            LVData.Items[iIndex].SubItems[15].Text = f.cbCopyBom.Checked ? "Y" : "N";        //Item15 Copy To SYS_BOM
            LVData.Items[iIndex].SubItems[16].Text = f.g_sBomType;        //Item16-BomType

            f.Dispose();

            Update_BOM(iIndex);
            string sTag = "All";
            if (rbAssyPart.Checked)
            {
                sTag = "0";
            }
            else if (rbKeyParts.Checked)
            {
                sTag = "1";
            }
            ShowBom(g_sPartID, LabVer.Text, sTag);

            return;
        }

        public void PopMenu2_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int iNodeLevel = TreeBomData.SelectedNode.Level;
            if (iNodeLevel == 0) //壺淕?BOM
            {
                MenuItemModify.Visible = false;
            }
            else if (iNodeLevel == 1)
            {
                MenuItemModify.Visible = false;
            }
            else
            {
                MenuItemModify.Visible = true;
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton r = (RadioButton)sender;
            if (r.Checked)
            {
                ShowBom(g_sPartID, LabVer.Text, r.Tag.ToString());
            }

        }

    }
}