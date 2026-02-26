using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SajetClass;
using SajetFilter;
using System.Data.OracleClient;
using System.Reflection;
using System.Data.OleDb;
using System.Collections.Specialized;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using CBOM.Helper;
using static System.Windows.Forms.AxHost;

namespace CBOM
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        public string g_sBOMID;
        public string g_SUPER_BOM;

        private CommonHelper myHelper;

        public string g_sUserID;
        public String g_sProgram, g_sFunction;
        public string g_PartID;

        string sSQL;
        DataSet dsTemp;
        DataSet objDS;
        int ExcelRowcount;
        StringCollection g_tsErrorData;
        DateTime g_dtDateTime;
        public void check_privilege()
        {
            string sPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram).ToString();

            TreeBomData.AllowDrop = SajetCommon.CheckEnabled("INSERT", sPrivilege);
            ModifyToolStripMenuItem.Enabled = SajetCommon.CheckEnabled("UPDATE", sPrivilege);
            LVPart.AllowDrop = SajetCommon.CheckEnabled("INSERT", sPrivilege);
            deleteToolStripMenuItem.Enabled = SajetCommon.CheckEnabled("DELETE", sPrivilege);
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;

            myHelper = new CommonHelper();
            this.Text = this.Text + "(" + SajetCommon.g_sFileVersion + ")";
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;

            btnExpand.BackColor = Color.SteelBlue;
            btnExpand.ForeColor = Color.White;

            btnImport.BackColor = Color.SeaGreen;
            btnImport.ForeColor = Color.White;

            check_privilege();

            for (int i = 0; i < LV1.Columns.Count; i++)
            {
                myHelper.AdjustListViewColumnWidth(LV1, i);
            }
        }

        private void collapseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeBom.CollapseAll();
        }

        private void expandToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeBom.ExpandAll();
        }

        public void Get_Bom_Ver(string sPart)
        {
            //��Ƹ����Ҧ�BOM����
            combVer.Items.Clear();
            string sSQL = "Select PART_ID, VERSION "
                       + "From SAJET.SYS_PART "
                       + "Where PART_NO = '" + sPart + "' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            if (DS.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Part No Error", 0);
                editPartNo.Focus();
                editPartNo.SelectAll();
                return;
            }
            string sPartID = DS.Tables[0].Rows[0]["PART_ID"].ToString();
            string sVer = DS.Tables[0].Rows[0]["VERSION"].ToString();

            sSQL = "Select VERSION From SAJET.SYS_BOM_INFO "
                 + "Where PART_ID = '" + sPartID + "' and enabled = 'Y' "
                 + "order by update_time desc ";
            DS = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
                combVer.Items.Add(DS.Tables[0].Rows[i]["VERSION"].ToString());

            if (combVer.FindString(sVer) == -1)
                combVer.Items.Insert(0, sVer);

            combVer.SelectedIndex = 0;
        }

        private void ShowBomDetail(string sPartNo, string sVer)
        {
            //��ܮڸ`�I
            TreeBom.Nodes.Clear();
            TreeBom.Nodes.Add(editPartNo.Text);
            TreeBom.Nodes[0].Tag = combVer.Text;
            TreeBom.Nodes[0].ImageIndex = 0;
            TreeBom.Nodes[0].SelectedImageIndex = TreeBom.Nodes[0].ImageIndex;
            TreeNode tNode = TreeBom.Nodes[0];

            //��ܩҦ��l���Ƹ�(TreeBom)
            string sSQL = "Select PART_ID "
                        + "From SAJET.SYS_PART "
                        + "Where PART_NO = '" + sPartNo + "' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            string sPartID = DS.Tables[0].Rows[0]["PART_ID"].ToString();

            sSQL = "select AA.ALEVEL,BB.Part_NO,CC.PART_NO ITEM_PART_NO from "
                 + "( "
                 + "   select a.*,level ALEVEL from "
                 + "    ( "
                 + "     select b.part_id,a.item_part_id,a.bom_id,a.process_id "
                 + "     from sajet.sys_bom a,sajet.sys_bom_info b "
                 + "     where a.bom_id = b.bom_id "
                 + "     ) a "
                 + "   start with part_id = '" + sPartID + "' "
                 + "   connect by prior item_part_id = part_id "
                 + ") AA "
                 + ",sajet.sys_part BB,sajet.sys_part CC "
                 + "where AA.part_id = BB.part_id "
                 + "and AA.item_part_id = CC.part_id "
                 + "group by AA.ALEVEL,BB.Part_NO,CC.PART_NO "
                 + "order by ALEVEL,BB.part_no,CC.part_no ";
            DS = ClientUtils.ExecuteSQL(sSQL);

            TreeNode tParentNode = null;
            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
            {
                string sMainPart = DS.Tables[0].Rows[i]["Part_NO"].ToString();
                string sItemPart = DS.Tables[0].Rows[i]["ITEM_PART_NO"].ToString();
                int iLevel = Convert.ToInt32(DS.Tables[0].Rows[i]["ALEVEL"].ToString());

                TreeNode NewNode = new TreeNode();
                NewNode.Text = sItemPart;
                NewNode.ImageIndex = 2;
                NewNode.SelectedImageIndex = NewNode.ImageIndex;
                NewNode.Name = sItemPart;

                tParentNode = tNode; //�Ĥ@�h 
                TreeNode tParentNode1 = tNode; //�Ĥ@�h 
                if (iLevel == 1)
                {
                    tParentNode.Nodes.Add(NewNode);
                }
                else
                {
                    for (int j = 2; j <= iLevel - 1; j++)
                    {
                        tParentNode = tParentNode.LastNode;
                    }
                    TreeNode[] tFindNodes = tParentNode.Nodes.Find(sMainPart, true);
                    if (tFindNodes.Length > 0)
                    {
                        tParentNode = tFindNodes[0];
                        tParentNode.Nodes.Add(NewNode);
                    }
                }
            }
            tNode.Expand();
        }

        private void ShowPartDetail(string sPartNo, string sVer)
        {
            LVData.Items.Clear();
            g_sBOMID = "";

            sPartNo = sPartNo.Trim();

            //��ܤl���ԲӸ�T(�k��TreeBomData&LVData)
            string sSQL = "Select PART_ID "
                       + "From SAJET.SYS_PART "
                       + "Where PART_NO = '" + sPartNo + "' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            string sPartID = g_PartID = DS.Tables[0].Rows[0]["PART_ID"].ToString();
            //�ڸ`�I===
            TreeBomData.Nodes.Clear();
            TreeBomData.Nodes.Add(sPartNo);
            TreeBomData.Nodes[0].ImageIndex = 0;
            TreeBomData.Nodes[0].SelectedImageIndex = 0;
            TreeBomData.Nodes[0].Tag = sVer;

            sSQL = " Select BOM_ID,NVL(SPEC2,' ') SUPER_BOM "
                 + " From SAJET.SYS_BOM_INFO TB1,SAJET.SYS_PART TB2  "
                 + " Where TB1.PART_ID = TB2.PART_ID AND TB1.PART_ID = '" + sPartID + "' and TB1.VERSION = '" + sVer + "' ";
            DS = ClientUtils.ExecuteSQL(sSQL);
            if (DS.Tables[0].Rows.Count == 0)
            {
                return;
            }
            g_sBOMID = DS.Tables[0].Rows[0]["BOM_ID"].ToString();
            g_SUPER_BOM = DS.Tables[0].Rows[0]["SUPER_BOM"].ToString();

            AddNode(sPartID, sVer, DS);


            TreeBomData.ExpandAll();
        }

        private void AddNode(string sPartID,string sVer,DataSet DS) 
        {

            //�l�`�I===            
            //string sPreProcess = "";
            //string sProcess = "";
            string sPreStepCode = "";
            string sStepName = "";
            string sPreRelation = "";
            sSQL = $@"Select  I.STEP_ITEM_CODE,F.PROCESS_CODE,D.PART_NO ITEM_PART_NO,B.ITEM_PART_ID, A.BOM_ID 
                    ,F.PROCESS_NAME,B.ITEM_COUNT,B.VERSION,B.ITEM_GROUP,B.PRIMARY_FLAG 
                    ,D.PART_TYPE, D.SPEC1
                    ,G.MODEL_NAME
                    , b.rowid, NVL(b.PROCESS_ID, 0) PROCESS_ID
                    ,I.STEP_ITEM_NAME, B.STEP_ITEM_ID, B.KEY_COMPONENT,D.UOM
                    From SAJET.SYS_BOM_INFO A
                    , SAJET.SYS_BOM B
                    , SAJET.SYS_PART D
                    , SAJET.SYS_PROCESS F
                    , SAJET.SYS_MODEL G
                    , SAJET.SYS_PROCESS_STEP H
                    , SAJET.SYS_STEP_ITEM I
                    Where A.PART_ID = '{sPartID}' and A.VERSION = '{sVer}'
                    and A.BOM_ID = B.BOM_ID
                    and B.ITEM_PART_ID = D.PART_ID(+)
                    and B.PROCESS_ID = F.PROCESS_ID(+)
                    AND D.MODEL_ID = G.MODEL_ID(+)
                    AND B.PROCESS_ID = H.PROCESS_ID(+)
                    AND B.STEP_ITEM_ID = H.STEP_ITEM_ID(+)
                    AND H.STEP_ITEM_ID = I.STEP_ITEM_ID(+)
                    Order By STEP_ITEM_NAME, B.ITEM_GROUP, 
                    B.PRIMARY_FLAG DESC,
                    ITEM_PART_NO, VERSION ";
            DS = ClientUtils.ExecuteSQL(sSQL);

            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
            {
                string sPROCESS_CODE = DS.Tables[0].Rows[i]["PROCESS_CODE"].ToString();
                string sProcess = DS.Tables[0].Rows[i]["PROCESS_NAME"].ToString();
                string sItemPartNo = DS.Tables[0].Rows[i]["ITEM_PART_NO"].ToString();
                string sItemCount = DS.Tables[0].Rows[i]["ITEM_COUNT"].ToString();
                string sItemGroup = DS.Tables[0].Rows[i]["ITEM_GROUP"].ToString();
                string sSubVersion = DS.Tables[0].Rows[i]["VERSION"].ToString();
                string sPartType = DS.Tables[0].Rows[i]["PART_TYPE"].ToString();
                string sSpec1 = DS.Tables[0].Rows[i]["SPEC1"].ToString();

                string sUOM = DS.Tables[0].Rows[i]["UOM"].ToString();


                string sRowID = DS.Tables[0].Rows[i]["ROWID"].ToString();
                string sProcessID = DS.Tables[0].Rows[i]["PROCESS_ID"].ToString();
                string sItemPartID = DS.Tables[0].Rows[i]["ITEM_PART_ID"].ToString();
                string sPrimaryFlag = DS.Tables[0].Rows[i]["PRIMARY_FLAG"].ToString();
                string sModelName = DS.Tables[0].Rows[i]["MODEL_NAME"].ToString();
                sStepName = DS.Tables[0].Rows[i]["STEP_ITEM_NAME"].ToString();

                string sStepCode = DS.Tables[0].Rows[i]["STEP_ITEM_CODE"].ToString();
                string sKey = DS.Tables[0].Rows[i]["KEY_COMPONENT"].ToString();
                string sStepID = DS.Tables[0].Rows[i]["STEP_ITEM_ID"].ToString();

                //if (string.IsNullOrEmpty(sProcess))
                //    sProcess = "N/A";

                if (string.IsNullOrEmpty(sStepName))
                    sStepName = "N/A";

                LVData.Items.Add(sItemPartNo);             //Item0-Part
                LVData.Items[i].SubItems.Add(sProcess);    //Item1-Process
                LVData.Items[i].SubItems.Add(sItemCount);  //Item2-Qty
                LVData.Items[i].SubItems.Add(sItemGroup);  //Item3-Relation
                LVData.Items[i].SubItems.Add(sSubVersion); //Item4-Version
                LVData.Items[i].SubItems.Add(sPartType);   //Item5-Part_Type
                LVData.Items[i].SubItems.Add(sSpec1);      //Item6-Spec



                //Location ====               
                string sLocation = "";
                string sSQL1 = " Select Location "
                             + " From SAJET.SYS_BOM_LOCATION "
                             + " Where BOM_ID = '" + g_sBOMID + "' "
                             + " And Item_Part_ID = '" + sItemPartID + "' "
                             + " ORDER BY LOCATION ";
                DataSet DS1 = ClientUtils.ExecuteSQL(sSQL1);
                for (int j = 0; j <= DS1.Tables[0].Rows.Count - 1; j++)
                {
                    sLocation = sLocation + DS1.Tables[0].Rows[j]["Location"].ToString() + ',';
                }
                String delim = ",";
                sLocation = sLocation.TrimEnd(delim.ToCharArray());
                LVData.Items[i].SubItems.Add(sLocation); //Item7 -Location
                LVData.Items[i].SubItems.Add(sRowID); //Item8 -Rowid
                LVData.Items[i].SubItems.Add(sProcessID); //Item9 -Process_ID
                LVData.Items[i].SubItems.Add(sItemPartID); //Item10 -Item_Part_ID
                LVData.Items[i].SubItems.Add(""); //Item11 -UpdateFlag
                LVData.Items[i].SubItems.Add(sPrimaryFlag); //Item 12
                LVData.Items[i].SubItems.Add(sModelName);//Item 13
                LVData.Items[i].SubItems.Add(sStepName);   //Item14-STEP_ITEM_NAME
                LVData.Items[i].SubItems.Add(sStepCode);     //Item15-STEP_ITEM_CODE
                LVData.Items[i].SubItems.Add(sKey);     //Item16-KEY_COMPONENT
                LVData.Items[i].SubItems.Add(sUOM);      //Item17-Spec
                LVData.Items[i].SubItems.Add(sPROCESS_CODE);   //Item18 PROCESS_CODE
                LVData.Items[i].SubItems.Add(sStepID);   //Item19 STEP_ITEM_ID


                LVData.Items[i].ImageIndex = 2;

                //�eTreeView==================
                //Tree-Process
                //if (sPreProcess != sProcess)
                //{
                //    TreeBomData.Nodes[0].Nodes.Add(sProcess);
                //    TreeBomData.Nodes[0].LastNode.ImageIndex = 1;
                //    TreeBomData.Nodes[0].LastNode.SelectedImageIndex = 1;
                //    TreeBomData.Nodes[0].LastNode.Name = sProcess; //���FFind�ϥ�
                //    sPreRelation = "";
                //}

                if (sPreStepCode != sStepName)
                {
                    TreeBomData.Nodes[0].Nodes.Add(sStepName);
                    TreeBomData.Nodes[0].LastNode.ImageIndex = 1;
                    TreeBomData.Nodes[0].LastNode.SelectedImageIndex = 1;
                    TreeBomData.Nodes[0].LastNode.Name = sStepName; //���FFind�ϥ�
                    sPreRelation = "";
                }
                //Tree-Part
                TreeNode tNode = new TreeNode();
                tNode.Text = sItemPartNo;
                tNode.Tag = i.ToString();  //���F�PLVData����(Tag�ȬOLVData��Row)
                //tNode.Tag = sItemPartID;

                if (sItemGroup == "0" || sPreRelation != sItemGroup)
                {
                    tNode.ImageIndex = 2;
                    tNode.SelectedImageIndex = tNode.ImageIndex;
                    TreeBomData.Nodes[0].LastNode.Nodes.Add(tNode);
                }
                else  //Tree-���N��
                {
                    tNode.ImageIndex = 3;
                    tNode.SelectedImageIndex = tNode.ImageIndex;
                    TreeBomData.Nodes[0].LastNode.LastNode.Nodes.Add(tNode);
                }
                //sPreProcess = sProcess;
                sPreStepCode = sStepName;
                sPreRelation = sItemGroup;
            }

        }

        private void combVer_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowPartDetail(editPartNo.Text, combVer.Text); //�Ƹ��MProcess         
            //ShowBomDetail(editPartNo.Text, combVer.Text);  //��ܩҦ��l���Ƹ�

            editPartFilter.Focus();
        }

        private void editPartFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != 13)
                return;

            string sPartNo = editPartFilter.Text = editPartFilter.Text.Trim();
            if (string.IsNullOrEmpty(sPartNo))
            {
                SajetCommon.Show_Message("Please Input Part No Prefix", 0);
                editPartFilter.Focus();
                return;
            }
            sPartNo = sPartNo + "%";
            string sSQL = @"Select Part_NO,Spec1,spec2 from Sajet.SYS_Part 
                        Where Part_No Like  :PART_NO 
                        and enabled = 'Y' 
                        order by part_no ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", sPartNo };
            DataSet DS = ClientUtils.ExecuteSQL(sSQL, Params);
            LVPart.Items.Clear();
            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
            {
                LVPart.Items.Add(DS.Tables[0].Rows[i]["Part_NO"].ToString());
                LVPart.Items[i].SubItems.Add(DS.Tables[0].Rows[i]["Spec1"].ToString());
                LVPart.Items[i].SubItems.Add(DS.Tables[0].Rows[i]["Spec2"].ToString());
                LVPart.Items[i].ImageIndex = 2;
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
            //�ӷ�Node           
            TreeNode SrcNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
            //�ت�Node   
            Point pt = ((System.Windows.Forms.TreeView)sender).PointToClient(new Point(e.X, e.Y));
            TreeNode mNode = ((System.Windows.Forms.TreeView)sender).GetNodeAt(pt);
            if (SrcNode == mNode)
                return;
            if (mNode == null)
                mNode = TreeBomData.TopNode;
            TreeBomData.Select();
            TreeBomData.Focus();

            if (SrcNode == null)  //�ӷ��OLVPart,�[�J�s���l���Ƹ�
            {
                string sPart = TreeBomData.Nodes[0].Text;
                string sVer = TreeBomData.Nodes[0].Tag.ToString();
                string sAddPart = LVPart.SelectedItems[0].Text; //���[�J���l��
                //�D��P�l��ۦP                
                if (sPart == sAddPart)
                {
                    SajetCommon.Show_Message("Sub Part No = Main Part No", 0);
                    return;
                }

                //�ˬd�O�_�|�y���L�a�j��
                if (Check_SubPartRecu(sPart, sAddPart))
                {
                    return;
                }

                if (F_AppandBomData(sPart, sVer, sAddPart, mNode))
                {
                    //�LRowid�����ܷs�W,��Insert
                    for (int i = 0; i <= LVData.Items.Count - 1; i++)
                    {
                        if (LVData.Items[i].SubItems[11].Text == "Y")
                            Update_BOM(i);
                    }
                    TreeBomData.ExpandAll();
                    //ShowBomDetail(editPartNo.Text, combVer.Text); 
                }
            }
            //else  //���ʭ쥻�w�����Ƹ�
            //{
            //    if (SrcNode.Level <= 1)
            //        return;
            //    if (mNode.Level == 0 | mNode.Level > 2)
            //        return;

            //    //���N�ƻP�D�Ƥ@�P����
            //    if (SrcNode.Level == 3)
            //        SrcNode = SrcNode.Parent;

            //    //�����N�ƪ��D�ƥu�ಾ�ʻs�{
            //    if (SrcNode.Level == 2 && SrcNode.Nodes.Count != 0)
            //        switch (mNode.Level)
            //        {
            //            case 1:
            //            default:
            //                break;
            //            case 2:
            //                mNode = mNode.Parent;
            //                break;
            //        }

            //    if (MoveBomData(SrcNode, mNode))
            //    {
            //        //�LRowid�����ܷs�W,��Insert
            //        for (int i = 0; i <= LVData.Items.Count - 1; i++)
            //        {
            //            if (LVData.Items[i].SubItems[11].Text == "Y")
            //                Update_BOM(i);
            //        }
            //    }
            //}
        }

        private bool MoveBomData(TreeNode tSrcNode, TreeNode tTargetNode)
        {
            string sTProcess = ""; //�ؼ�Process
            string sProcess = "";
            string sTProcessID = ""; //�ؼ�Process ID
            string sPrimaryFlag = "";

            int iSrcInx = System.Convert.ToInt32(tSrcNode.Tag);
            //�[�J�����N��========================================================
            if (tTargetNode.Level == 2)
            {
                int iTargetInx = System.Convert.ToInt32(tTargetNode.Tag);
                sTProcess = tTargetNode.Parent.Text;
                sTProcessID = getProcessID(sTProcess);
                if (tSrcNode.Level == 2)
                    sProcess = tSrcNode.Parent.Text;
                else
                    sProcess = tSrcNode.Parent.Parent.Text;

                //Process���P
                if (sTProcess != sProcess)
                {
                    //��Process�U�w������
                    //if (!CheckDup(sTProcessID, tSrcNode.Text, 0))
                    //{
                    //    string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                    //    SajetCommon.Show_Message(sMsg + Environment.NewLine + tSrcNode.Text, 0);
                    //    return false;
                    //}

                    if (tSrcNode.Text != tTargetNode.Text)
                    {
                        bool bResult = false;
                        for (int i = 0; i <= tTargetNode.Nodes.Count - 1; i++)
                            if (tSrcNode.Text == tTargetNode.Nodes[i].Text)
                            {
                                bResult = true;
                                break;
                            }

                        //if (!bResult && !CheckDup(sTProcessID, tSrcNode.Text, 0))
                        //{
                        //    string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                        //    SajetCommon.Show_Message(sMsg + Environment.NewLine + tSrcNode.Text, 0);
                        //    return false;
                        //}
                    }
                }
                else   //Process�ۦP
                {
                    if (tSrcNode.Level == 3)
                        for (int i = 0; i <= tTargetNode.Nodes.Count - 1; i++)
                            if (tSrcNode.Text == tTargetNode.Nodes[i].Text)
                            {
                                string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                                SajetCommon.Show_Message(sMsg + Environment.NewLine + tSrcNode.Text, 0);
                                return false;
                            }
                }

                //�ؼЭ쥻�L���N��,�ݧ��ܦ��s��Relation���X
                string sTargetRelation = LVData.Items[iTargetInx].SubItems[3].Text;
                if (sTargetRelation == "0")
                {
                    sTargetRelation = F_GETMAXGROUP(g_sBOMID);
                    LVData.Items[iTargetInx].SubItems[3].Text = sTargetRelation;
                    LVData.Items[iTargetInx].SubItems[11].Text = "Y";
                    LVData.Items[iTargetInx].SubItems[12].Text = "Y";
                }
                LVData.Items[iSrcInx].SubItems[1].Text = sTProcess;
                LVData.Items[iSrcInx].SubItems[3].Text = sTargetRelation;
                LVData.Items[iSrcInx].SubItems[9].Text = sTProcessID;
                LVData.Items[iSrcInx].SubItems[11].Text = "Y";
                LVData.Items[iSrcInx].SubItems[12].Text = "N";
            }
            else
            //�[�J���D��============================================================
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
                    //if (!CheckDup(sTProcessID, tSrcNode.Text, 0))
                    //{
                    //    string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                    //    SajetCommon.Show_Message(sMsg + Environment.NewLine + tSrcNode.Text, 0);
                    //    return false;
                    //}
                }
                else
                {
                    //if (!CheckDup(sTProcessID, tSrcNode.Text, 1))
                    //{
                    //    string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                    //    SajetCommon.Show_Message(sMsg + Environment.NewLine + tSrcNode.Text, 0);
                    //    return false;
                    //}
                }
                LVData.Items[iSrcInx].SubItems[1].Text = sTProcess;
                LVData.Items[iSrcInx].SubItems[9].Text = sTProcessID;
                LVData.Items[iSrcInx].SubItems[11].Text = "Y";
                LVData.Items[iSrcInx].SubItems[12].Text = "Y";
            }

            if (tSrcNode.Level == 2 && tSrcNode.Nodes.Count != 0)  //�쥻���Ƭ��D�ƥB�����N��,�h��������
            {
                foreach (TreeNode item in tSrcNode.Nodes)
                {
                    int iSubSrcInx = System.Convert.ToInt32(item.Tag);
                    LVData.Items[iSubSrcInx].SubItems[1].Text = sTProcess;
                    LVData.Items[iSubSrcInx].SubItems[9].Text = sTProcessID;
                    LVData.Items[iSubSrcInx].SubItems[11].Text = "Y";
                }

                tSrcNode.Remove();
                tTargetNode.Nodes.Add(tSrcNode);
            }
            else
            {
                TreeNode tNewNode = new TreeNode
                {
                    Text = tSrcNode.Text,
                    Tag = tSrcNode.Tag,
                    ImageIndex = tTargetNode.Level + 1
                };
                tNewNode.SelectedImageIndex = tNewNode.ImageIndex;
                tTargetNode.Nodes.Add(tNewNode);
                tSrcNode.Remove();
            }

            tTargetNode.Expand();
            return true;
        }

        private bool Check_SubPartRecu(string sPartNo, string sSubPartNo)
        {
            string sPartID = GET_FIELD_ID("SAJET.SYS_PART", "PART_NO", "PART_ID", sPartNo);
            string sSubPartID = GET_FIELD_ID("SAJET.SYS_PART", "PART_NO", "PART_ID", sSubPartNo);

            //�ˬd�O�_�|�y�����j
            //Ex:A->B->C->D , D�U���i�A�[�JA,B,C,�_�h�|�y���L�a�j��  
            sSQL = "select AA.LV,BB.Part_NO,CC.PART_NO ITEMPART from "
                 + "( "
                 + "   select Level LV,part_id,item_part_id from "
                 + "   ( "
                 + "     select b.part_id,a.item_part_id "
                 + "     from sajet.sys_bom a,sajet.sys_bom_info b "
                 + "     where a.bom_id = b.bom_id and b.part_id = '" + sSubPartID + "' "
                 + "   ) "
                 + "   start with item_part_id = '" + sPartID + "' "
                 + "   connect by prior part_id = item_part_id " //�ѤU�V�W��
                 + " ) AA "
                 + "  ,sajet.sys_part BB,sajet.sys_part CC "
                 + "where AA.part_id = '" + sSubPartID + "' "
                 + "and AA.part_id = BB.part_id "
                 + "and AA.item_part_id = CC.part_id "
                 + "and rownum = 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sMsg1 = SajetCommon.SetLanguage("Sub Part No Recursive", 1);
                string sMsg2 = SajetCommon.SetLanguage("Bottom Up Level", 1);
                string sMsg3 = SajetCommon.SetLanguage("Part No", 1);
                string sMsg4 = SajetCommon.SetLanguage("Sub Part No", 1);
                SajetCommon.Show_Message(sMsg1 + " !!" + Environment.NewLine
                          + sMsg2 + " : " + dsTemp.Tables[0].Rows[0]["LV"].ToString() + Environment.NewLine
                          + sMsg3 + " : " + dsTemp.Tables[0].Rows[0]["PART_NO"].ToString() + Environment.NewLine
                          + sMsg4 + " : " + dsTemp.Tables[0].Rows[0]["ITEMPART"].ToString(), 0);
                return true;
            }
            return false;
        }

        private string F_GETMAXGROUP(string sBOMID)
        {
            string sItemGroup;
            int j = 0;
            DataSet DS = ClientUtils.ExecuteSQL($@"WITH T AS
 (SELECT ITEM_GROUP FROM SAJET.SYS_BOM WHERE BOM_ID = '{sBOMID}')
SELECT TO_CHAR(MIN_A - 1 + LEVEL)
  FROM (SELECT 1 MIN_A, MAX(ITEM_GROUP) MAX_A FROM T)
CONNECT BY LEVEL <= MAX_A - MIN_A + 1
MINUS
SELECT ITEM_GROUP FROM T");
            if (DS.Tables[0].Rows.Count != 0)
            {
                sItemGroup = DS.Tables[0].Rows[0][0].ToString();
                int.TryParse(sItemGroup, out j);
            }
            else
            {
                sSQL = " Select distinct item_group from sajet.sys_bom "
                     + " where BOM_ID = '" + sBOMID + "' "
                     + " order by item_group desc ";
                DS = ClientUtils.ExecuteSQL(sSQL);
                //�]��Item Group���i�঳�D�Ʀr��,�ҥH�u��X�̤j���Ʀr+1
                for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
                {
                    sItemGroup = DS.Tables[0].Rows[i]["item_group"].ToString();
                    if (int.TryParse(sItemGroup, out j))
                        break;
                }
                sItemGroup = Convert.ToString(j + 1);
            }
            return sItemGroup;
        }

        private string getProcessID(string sProcessName)
        {
            sSQL = "Select Process_ID from sajet.sys_process "
                 + "where process_name = '" + sProcessName + "' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            if (DS.Tables[0].Rows.Count > 0)
                return DS.Tables[0].Rows[0]["Process_ID"].ToString();
            else
                return "0";
        }

        private string getProcess(string sStep)
        {
            sSQL = " Select B.PROCESS_NAME from SAJET.SYS_PROCESS_STEP A,"
                 + " SAJET.SYS_PROCESS B,"
                 + " SAJET.SYS_STEP_ITEM D"
                 + " Where A.PROCESS_ID = B.PROCESS_ID "
                 + " AND A.STEP_ITEM_ID = D.STEP_ITEM_ID "
                 + " AND D.STEP_ITEM_NAME = '" + sStep + "' "
                 + "AND ROWNUM = 1 ";
            
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            if (DS.Tables[0].Rows.Count > 0)
                return DS.Tables[0].Rows[0]["PROCESS_NAME"].ToString();
            else
                return "0";
        }

        private bool checkStep(string sPart, string sStep, string sProcess)
        {
            //sSQL = " Select B.PROCESS_NAME from SAJET.SYS_PART_PROCESS_STEP A,"
            //     + " SAJET.SYS_PROCESS B,"
            //     + " SAJET.SYS_PART C,"
            //     + " SAJET.SYS_STEP_ITEM D"
            //     + " Where A.PROCESS_ID = B.PROCESS_ID "
            //     + " AND A.PART_ID = C.PART_ID "
            //     + " AND A.STEP_ITEM_ID = D.STEP_ITEM_ID "
            //     + " AND C.PART_NO = '" + sPart + "' "
            //     + " AND D.STEP_ITEM_NAME = '" + sStep + "' "
            //     + " AND B.PROCESS_NAME = '" + sProcess + "' "
            //     + "AND ROWNUM = 1 ";


            //sProcess  ��ڷN�q�w�g�אּSTEP_ITEM_CODE
            sSQL = " Select B.PROCESS_NAME from SAJET.SYS_PART_PROCESS_STEP A,"
                 + " SAJET.SYS_PROCESS B,"
                 + " SAJET.SYS_PART C,"
                 + " SAJET.SYS_STEP_ITEM D"
                 + " Where A.PROCESS_ID = B.PROCESS_ID "
                 + " AND A.PART_ID = C.PART_ID "
                 + " AND A.STEP_ITEM_ID = D.STEP_ITEM_ID "
                 + " AND C.PART_NO = '" + sPart + "' "
                 + " AND D.STEP_ITEM_CODE = '" + sProcess + "' "
                 + "AND ROWNUM = 1 ";

            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            if (DS.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }

        private bool CheckDup(string sProcessID, string sItemPart, int iCount, string sStep)
        {
            string sSQL = " Select count(*) sCount "
                        + " from sajet.sys_bom a, sajet.sys_part b,SAJET.SYS_STEP_ITEM C "
                        + " where a.BOM_ID = '" + g_sBOMID + "' "
                        + " and NVL(a.Process_ID,'0') = '" + sProcessID + "' "
                        + " and a.Item_Part_ID = b.part_id "
                        + " and a.STEP_ITEM_ID = C.STEP_ITEM_ID "
                        + " and b.Part_No= '" + sItemPart + "' "
                        + " and C.STEP_ITEM_NAME= '" + sStep + "' ";
            DataSet DS = ClientUtils.ExecuteSQL(sSQL);
            if (iCount.ToString() == DS.Tables[0].Rows[0]["sCount"].ToString())
                return true;
            else
                return false;
        }

        private bool F_AppandBomData(string sPart, string sVer, string sAddPart, TreeNode tNode)
        {
            string sProcess = "";
            string sCount = "";
            string sRelation = "";
            string sPartVersion = "";
            string sLocation = "";
            string sPrimaryFlag = "";
            bool bChangeGroup = false;
            int iNodeLevel = tNode.Level;
            string sStep = "";
            string sKey = "";

            if (iNodeLevel == 0) //�s��process
            {
                sProcess = "";
                sCount = "1";
                sRelation = "0";
                sPartVersion = "";
                sLocation = "";
                sPrimaryFlag = "Y";
                sStep = "";
                sKey = "";
                sStep = "";
                sKey = "N";
            }
            else if (iNodeLevel == 1) //�b�P�Ӫ�process�U�[�D��//�אּ�u��STEP
            {
                sStep = tNode.Text;
                sProcess = getProcess(sStep);
                if (sProcess == "0")
                {
                    string sMsg = SajetCommon.SetLanguage("Sub Part No Step", 1);
                    SajetCommon.Show_Message(sMsg + Environment.NewLine + sAddPart, 0);
                    return false;
                }
                sCount = "1";
                sRelation = "0";
                sPartVersion = "";
                sLocation = "";
                sPrimaryFlag = "Y";
                sKey = "N";

                //�Ƹ�
                string sProcessID = getProcessID(sProcess);
                if (!CheckDup(sProcessID, sAddPart, 0,sStep))
                {
                    string sMsg = SajetCommon.SetLanguage("Sub Part No Duplicate", 1);
                    SajetCommon.Show_Message(sMsg + Environment.NewLine + sAddPart, 0);
                    return false;
                }
            }
            else if (iNodeLevel == 2) //�bPart���W�[�@���N��
            {
                //sProcess = tNode.Parent.Text;

                if (sAddPart == tNode.Text)
                    return false;
                else
                {
                    
                    sProcess = getProcess(sStep);
                    if (sProcess == "0")
                    {
                        string sMsg = SajetCommon.SetLanguage("Sub Part No Step", 1);
                        SajetCommon.Show_Message(sMsg + Environment.NewLine + sAddPart, 0);
                        return false;
                    }
                    string sProcessID = getProcessID(sProcess);
                    if (!CheckDup(sProcessID, sAddPart, 0, sStep))
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
                sPrimaryFlag = "N";

                //�Y�쥻�L���N��,�ݱNgroup�אּ�D0���� 
                if (sRelation == "0")
                {
                    sRelation = F_GETMAXGROUP(g_sBOMID);
                    bChangeGroup = true;
                }
            }
            else if (iNodeLevel == 3) //�b���N�Ƥ��W�[�@���N��
            {
                //Part����
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
                sProcess = getProcess(sStep);
                if (sProcess == "0")
                {
                    string sMsg = SajetCommon.SetLanguage("Sub Part No Step", 1);
                    SajetCommon.Show_Message(sMsg + Environment.NewLine + sAddPart, 0);
                    return false;
                }
                string sProcessID = getProcessID(sProcess);
                //sProcess = tNode.Parent.Parent.Text;
                sCount = LVData.Items[iIndex].SubItems[2].Text;
                sRelation = LVData.Items[iIndex].SubItems[3].Text;
                sPartVersion = LVData.Items[iIndex].SubItems[4].Text;
                sLocation = LVData.Items[iIndex].SubItems[7].Text;
                sPrimaryFlag = "N";
            }
            else
            {
                return false;
            }

            fData fData = new fData();
            fData.g_sUpdateType = "Append";
            fData.g_sPartNo = sPart;
            fData.g_sVer = sVer;
            fData.g_sProcess = sProcess;
            fData.editSubPartNo.Text = sAddPart;
            fData.editQty.Text = sCount;
            fData.editSubPartVer.Text = "";//sPartVersion;
            fData.editGroup.Text = sRelation;
            fData.g_sChangeGroup = bChangeGroup;
            fData.g_sBOM_ID = g_sBOMID;
            string[] split = sLocation.Split(new Char[] { ',' });
            fData.editLocation.Lines = split;
            fData.g_sStep = sStep;
            fData.g_sKey = sKey;

            if (iNodeLevel >= 2)
            {
                fData.editSubPartNo.Enabled = false;
                fData.combProcess.Enabled = false;
                fData.editQty.Enabled = false;
                fData.editGroup.Enabled = bChangeGroup;
            }
            else if (iNodeLevel == 1)
            {
                fData.editSubPartNo.Enabled = false;
                fData.combProcess.Enabled = false;
                fData.editGroup.Enabled = false;
            }
            else if (iNodeLevel == 0)
            {
                fData.editGroup.Enabled = false;
            }

            // =======Show Form==========================================================
            if (fData.ShowDialog() != DialogResult.OK)
            {
                return false;
            }

            g_sBOMID = fData.g_sBOM_ID;
            sAddPart = fData.editSubPartNo.Text;
            //�Y�ܧ󬰦����N�����Y,�ݦP�ɧ��GROUP
            if (bChangeGroup)
            {
                int iTag = System.Convert.ToInt32(tNode.Tag.ToString());
                LVData.Items[iTag].SubItems[3].Text = fData.editGroup.Text;
                LVData.Items[iTag].SubItems[11].Text = "Y"; //Y1���ܦ�����Ʀ����,��Update DB
                                                            //LVData.Items[iTag].SubItems[12].Text = sPrimaryFlag;
            }

            sProcess = fData.combProcess.Text.Trim();
            sStep = fData.combStep.Text.Trim();
            sKey = fData.combKey.Text.Trim();
            //�[�J�s�`�I===================================================
            if (iNodeLevel == 0) //�ݥ��إ�step���`�I
            {
                //��O�_�w����step��node  
                TreeNode[] tFindStepNodes = TreeBomData.Nodes[0].Nodes.Find(sStep, true);

                if (tFindStepNodes.Length == 0)
                {
                    TreeNode tStepNode = new TreeNode();
                    tStepNode.Text = sStep;
                    tStepNode.ImageIndex = iNodeLevel + 1;
                    tStepNode.Name = sStep;
                    tStepNode.SelectedImageIndex = tStepNode.ImageIndex;
                    tNode.Nodes.Add(tStepNode);
                    tNode = tNode.LastNode;
                }
                else
                {
                    tNode = tFindStepNodes[0];
                }
                iNodeLevel = 1;
            }

            if (iNodeLevel == 3) //�Y�O�즲����N�Ƹ`�I�W,Tree Node�ئb�P�@�h
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
            LVData.Items[iRwoCount].SubItems.Add(fData.editQty.Text);        //Item2-Qty
            LVData.Items[iRwoCount].SubItems.Add(fData.editGroup.Text);      //Item3-Relation
            LVData.Items[iRwoCount].SubItems.Add(fData.editSubPartVer.Text); //Item4-Version
            LVData.Items[iRwoCount].SubItems.Add(fData.g_sItemPartType);     //Item5-Part_Type 
            LVData.Items[iRwoCount].SubItems.Add(fData.g_sItemSpec1);        //Item6-Spec
                                                                             //Location==
            sLocation = "";
            for (int j = 0; j <= fData.editLocation.Lines.Length - 1; j++)
            {
                sLocation = sLocation + fData.editLocation.Lines[j].ToString() + ',';
            }
            String delim = ",";
            sLocation = sLocation.TrimEnd(delim.ToCharArray());
            LVData.Items[iRwoCount].SubItems.Add(sLocation);  //Item7 -Location
                                                              //==            
            LVData.Items[iRwoCount].SubItems.Add("");  //Item8 -Rowid
            LVData.Items[iRwoCount].SubItems.Add(fData.g_sProcessID);  //Item9 -Process_ID
            LVData.Items[iRwoCount].SubItems.Add(fData.g_sItemPartID);  //Item10 -Item_Part_ID
            LVData.Items[iRwoCount].SubItems.Add("Y"); //Item11 -Update Flag
            LVData.Items[iRwoCount].SubItems.Add(sPrimaryFlag);
            LVData.Items[iRwoCount].SubItems.Add(fData.g_sModelName);
            LVData.Items[iRwoCount].SubItems.Add(sStep);   //Item14-STEP_ITEM_NAME
            LVData.Items[iRwoCount].SubItems.Add(fData.g_sStepID);     //Item15-STEP_ITEM_ID
            LVData.Items[iRwoCount].SubItems.Add(sKey);     //Item16-KEY_COMPONENT
            LVData.Items[iRwoCount].ImageIndex = 2;
            LVData.Items[iRwoCount].StateImageIndex = LVData.Items[iRwoCount].ImageIndex;
            fData.Dispose();
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
            string sPrimaryFLAG = LVData.Items[iRow].SubItems[12].Text;
            string sStepID = LVData.Items[iRow].SubItems[15].Text;
            string sKey = LVData.Items[iRow].SubItems[16].Text;

            if (sVERSION == "")
                sVERSION = "N/A";
            if (sRowID == "")
            {
                if (g_SUPER_BOM.ToUpper().IndexOf("SUPERBOM") >= 0)
                {
                    sSQL =
                    $@"
                    INSERT INTO SAJET.SYS_BOM
                      (BOM_ID, ITEM_PART_ID, ITEM_GROUP, ITEM_COUNT,
                       PROCESS_ID, VERSION, UPDATE_USERID, PRIMARY_FLAG,
                       STEP_ITEM_ID, KEY_COMPONENT)
                    SELECT BOM_ID,
                           '{sITEM_PART_ID}',
                           '{sITEM_GROUP}',
                           '{sITEM_COUNT}',
                           '{sPROCESS_ID}',
                           '{sVERSION}',
                           '{g_sUserID}',
                           '{sPrimaryFLAG}',
                           '{sStepID}',
                           '{sKey}'
                    FROM 
                    (
                          SELECT DISTINCT TB2.BOM_ID FROM SAJET.SYS_BOM TB1,SAJET.SYS_BOM_INFO TB2 
                          WHERE TB1.BOM_ID = TB2.BOM_ID  AND TB2.PART_ID IN (SELECT PART_ID FROM SAJET.SYS_PART WHERE UPPER(SPEC2) = 'SUPERBOM')
                          AND STEP_ITEM_ID = '{sStepID}'
                    )

                    ";
                }
                else 
                {
                    sSQL = " Insert Into SAJET.SYS_BOM "
                      + " (BOM_ID,ITEM_PART_ID,ITEM_GROUP,ITEM_COUNT "
                      + "  ,PROCESS_ID,VERSION,UPDATE_USERID,PRIMARY_FLAG,STEP_ITEM_ID,KEY_COMPONENT) "
                      + " Values "
                      + " ('" + g_sBOMID + "','" + sITEM_PART_ID + "','" + sITEM_GROUP + "','" + sITEM_COUNT + "' "
                      + " ,'" + sPROCESS_ID + "','" + sVERSION + "','" + g_sUserID + "','" + sPrimaryFLAG + "','" + sStepID + "','" + sKey + "' ) ";
                }
                
                ClientUtils.ExecuteSQL(sSQL);

                //Insert Bom Location====
                sSQL = " Delete SAJET.SYS_BOM_LOCATION "
                     + " Where BOM_ID = '" + g_sBOMID + "' "
                     + " and Item_Part_Id = '" + sITEM_PART_ID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
                if (!string.IsNullOrEmpty(sLocation))
                {
                    string[] split = sLocation.Split(new Char[] { ',' });
                    for (int i = 0; i <= split.Length - 1; i++)
                    {
                        sSQL = " Insert Into SAJET.SYS_BOM_LOCATION "
                             + " (BOM_ID,ITEM_PART_ID,ITEM_GROUP,LOCATION,UPDATE_USERID) "
                             + " Values "
                             + " ('" + g_sBOMID + "','" + sITEM_PART_ID + "','" + sITEM_GROUP + "','" + split.GetValue(i).ToString() + "' "
                             + " ,'" + g_sUserID + "') ";
                        ClientUtils.ExecuteSQL(sSQL);
                    }
                }
                //�䦹��RowID
                sSQL = " Select Rowid from SAJET.SYS_BOM "
                     + " Where BOM_ID = '" + g_sBOMID + "' "
                     + " and Item_Part_Id = '" + sITEM_PART_ID + "' "
                     + " and NVL(Process_ID,0) = '" + sPROCESS_ID + "' ";
                DataSet DS = ClientUtils.ExecuteSQL(sSQL);
                LVData.Items[iRow].SubItems[8].Text = DS.Tables[0].Rows[0]["RowID"].ToString();
                sRowID = DS.Tables[0].Rows[0]["RowID"].ToString();
            }
            else
            {
                sSQL = " Update SAJET.SYS_BOM "
                     + " Set ITEM_GROUP = '" + sITEM_GROUP + "' "
                     + "   ,ITEM_COUNT = '" + sITEM_COUNT + "' "
                     + "   ,PROCESS_ID = '" + sPROCESS_ID + "' "
                     + "   ,VERSION = '" + sVERSION + "' "
                     + "   ,UPDATE_USERID = '" + g_sUserID + "' "
                     + "   ,UPDATE_TIME = SYSDATE "
                     + "   ,PRIMARY_FLAG = '" + sPrimaryFLAG + "' "
                     + "   ,STEP_ITEM_ID = '" + sStepID + "' "
                     + "   ,KEY_COMPONENT = '" + sKey + "' "
                     + " Where Rowid = '" + sRowID + "'";
                ClientUtils.ExecuteSQL(sSQL);
            }
            LVData.Items[iRow].SubItems[11].Text = "";
            CopyToHistory(sRowID);
        }
        private void CopyToHistory(string sRowid)
        {
            sSQL = "Insert Into SAJET.SYS_HT_BOM "
                 + "Select * from SAJET.SYS_BOM "
                 + "Where ROWID = '" + sRowid + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeBomData.CollapseAll();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            TreeBomData.ExpandAll();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TreeBomData.Nodes.Count == 0)
                return;
            if (TreeBomData.SelectedNode == null)
                return;

            string sSQL = "";
            int iNodeLevel = TreeBomData.SelectedNode.Level;
            if (iNodeLevel == 0) //�R�����BOM
            {
                string sMsg = SajetCommon.SetLanguage("Delete this BOM", 1);
                if (SajetCommon.Show_Message(sMsg + " ?", 2) != DialogResult.Yes)
                    return;
                // Copy To History ==
                sSQL = " UPDATE SAJET.SYS_BOM "
                     + " SET ENABLED = 'Drop' "
                     + "    ,UPDATE_USERID = '" + g_sUserID + "' "
                     + "    ,UPDATE_TIME = SYSDATE "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
                sSQL = " Insert Into SAJET.SYS_HT_BOM "
                     + " Select * from SAJET.SYS_BOM "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
                //==================

                sSQL = " DELETE SAJET.SYS_BOM "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' ";
                ClientUtils.ExecuteSQL(sSQL);

                sSQL = " DELETE SAJET.SYS_BOM_LOCATION "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' ";
                ClientUtils.ExecuteSQL(sSQL);

                sSQL = " DELETE SAJET.SYS_BOM_INFO "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' ";
                ClientUtils.ExecuteSQL(sSQL);

            }
            else if (iNodeLevel == 1) //�R����process�U�Ҧ���
            {
                string sMsg = SajetCommon.SetLanguage("Delete all Part of this Process", 1);
                if (SajetCommon.Show_Message(sMsg + " ?", 2) != DialogResult.Yes)
                    return;

                int iRow = System.Convert.ToInt32(TreeBomData.SelectedNode.Nodes[0].Tag.ToString());
                string sProcessID = LVData.Items[iRow].SubItems[9].Text;
                string sStepID = LVData.Items[iRow].SubItems[15].Text;

                for (int i = 0; i <= TreeBomData.SelectedNode.Nodes.Count - 1; i++)
                {
                    int iIndex = System.Convert.ToInt32(TreeBomData.SelectedNode.Nodes[i].Tag.ToString());
                    string sItemPartID = LVData.Items[iIndex].SubItems[10].Text;
                    string sRowID = LVData.Items[iIndex].SubItems[8].Text;
                    //��BOM���w�g�S���ۦP��Part��,�R��Location
                    sSQL = " SELECT ITEM_PART_ID FROM SAJET.SYS_BOM "
                         + " WHERE BOM_ID = '" + g_sBOMID + "' "
                         + " AND ITEM_PART_ID = '" + sItemPartID + "' "
                         + " AND ROWID <> '" + sRowID + "' "
                         + " AND ROWNUM=1 ";
                    DataSet DS = ClientUtils.ExecuteSQL(sSQL);
                    if (DS.Tables[0].Rows.Count == 0)
                    {
                        sSQL = " DELETE SAJET.SYS_BOM_LOCATION "
                             + " WHERE BOM_ID = '" + g_sBOMID + "' "
                             + " AND ITEM_PART_ID = '" + sItemPartID + "' ";
                        ClientUtils.ExecuteSQL(sSQL);
                    }
                }

                // Copy To History ==
                sSQL = " UPDATE SAJET.SYS_BOM "
                     + " SET ENABLED = 'Drop' "
                     + "    ,UPDATE_USERID = '" + g_sUserID + "' "
                     + "    ,UPDATE_TIME = SYSDATE "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' "
                     + " AND NVL(PROCESS_ID,'0') = '" + sProcessID + "' "
                     + " AND NVL(STEP_ITEM_ID,'0') = '" + sStepID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
                sSQL = " Insert Into SAJET.SYS_HT_BOM "
                     + " Select * from SAJET.SYS_BOM "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' "
                     + " AND NVL(PROCESS_ID,'0') = '" + sProcessID + "' "
                     + " AND NVL(STEP_ITEM_ID,'0') = '" + sStepID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
                //==================

                sSQL = " DELETE SAJET.SYS_BOM "
                     + " WHERE BOM_ID = '" + g_sBOMID + "' "
                     + " AND NVL(PROCESS_ID,'0') = '" + sProcessID + "' "
                     + " AND NVL(STEP_ITEM_ID,'0') = '" + sStepID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
            }
            else  //�R���Y�@�Ӯ�
            {
                string sMsg = SajetCommon.SetLanguage("Delete this Part", 1) + " ?";

                int iRow = System.Convert.ToInt32(TreeBomData.SelectedNode.Tag.ToString());
                string sProcessID = LVData.Items[iRow].SubItems[9].Text;
                string sItemPartID = LVData.Items[iRow].SubItems[10].Text;
                string sItemGroup = LVData.Items[iRow].SubItems[3].Text;
                string sStepCode = LVData.Items[iRow].SubItems[15].Text;
                string sStepID = LVData.Items[iRow].SubItems[19].Text;

                if (iNodeLevel == 2 && sItemGroup != "0")
                    sMsg += Environment.NewLine + SajetCommon.SetLanguage("Substitutes are also removed", 1);
                if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                    return;

                //修正錯誤  ~~ by Jim 20260209
                string sFilterSQL = $@" WHERE BOM_ID = '{g_sBOMID}' AND PROCESS_ID = '{sProcessID }' AND STEP_ITEM_ID =  '{sStepID}' ";
                
                if (iNodeLevel == 2)
                    sFilterSQL += $" AND ITEM_PART_ID = '{sItemPartID} ' ";
                else
                    sFilterSQL += $" AND ITEM_GROUP = '{sItemGroup}' ";


                // Copy To History ==
                ClientUtils.ExecuteSQL($@"UPDATE SAJET.SYS_BOM
                SET ENABLED = 'Drop', UPDATE_USERID = '{g_sUserID}', UPDATE_TIME = SYSDATE" + sFilterSQL);


                ClientUtils.ExecuteSQL($@"INSERT INTO SAJET.SYS_HT_BOM
                SELECT * FROM SAJET.SYS_BOM" + sFilterSQL);
                //==================
                //ClientUtils.ExecuteSQL("DELETE SAJET.SYS_BOM" + sFilterSQL);

                //新增Super Bom功能 ~~ by Jim 20260209
                object[][] Params = new object[5][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "i_BOM_ID", g_sBOMID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "i_STEP_ITEM_ID", sStepID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "i_ITEM_PART_ID", sItemPartID };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OP_TYPE", "D" };
                Params[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };

                DataSet dsTemp = ClientUtils.ExecuteProc("SAJET.SP_SUPER_BOM", Params);





                //�R����L���N��,�NITEM GROUP�אּ0
                if (sItemGroup != "0")
                {
                    sSQL = " SELECT COUNT(*) CNT FROM SAJET.SYS_BOM "
                         + " WHERE BOM_ID = '" + g_sBOMID + "' "
                         + " AND ITEM_GROUP = '" + sItemGroup + "' ";
                    DataSet DS = ClientUtils.ExecuteSQL(sSQL);
                    if (DS.Tables[0].Rows[0]["CNT"].ToString() == "1")
                    {
                        sSQL = " UPDATE SAJET.SYS_BOM "
                             + " SET ITEM_GROUP = '0' "
                             + (sFilterSQL = " WHERE BOM_ID = '" + g_sBOMID + "' "
                             + " AND ITEM_GROUP = '" + sItemGroup + "' ");
                        ClientUtils.ExecuteSQL(sSQL);
                        ClientUtils.ExecuteSQL($@"INSERT INTO SAJET.SYS_HT_BOM
  SELECT * FROM SAJET.SYS_BOM" + sFilterSQL);
                    }
                }

                //��BOM���S��Part��,�R��Location
                ClientUtils.ExecuteSQL($@"DELETE SAJET.SYS_BOM_LOCATION A
                 WHERE A.BOM_ID = '{g_sBOMID}'
                   AND NOT EXISTS (SELECT 1
                          FROM SAJET.SYS_BOM B
                         WHERE A.BOM_ID = B.BOM_ID
                           AND A.ITEM_PART_ID = B.ITEM_PART_ID)");
                            }
            ShowPartDetail(editPartNo.Text, combVer.Text);
            //ShowBomDetail(editPartNo.Text, combVer.Text); 
        }

        private void TreeBomData_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var _CurrentNode = TreeBomData.SelectedNode.Text.Trim();
            var _Version = TreeBomData.Nodes[0].Tag;


            var BaseSQL = $@"SELECT
                            I.STEP_ITEM_CODE,
                            I.STEP_ITEM_NAME,
                            F.PROCESS_CODE,
                            F.PROCESS_NAME,
                            D.PART_NO ITEM_PART_NO,
                            D.SPEC1,
                            B.ITEM_COUNT,
                            D.UOM
                        FROM SAJET.SYS_BOM_INFO A,
                             SAJET.SYS_BOM B,
                             SAJET.SYS_PART D,
                             SAJET.SYS_PROCESS F,
                             SAJET.SYS_MODEL G,
                             SAJET.SYS_PROCESS_STEP H,
                             SAJET.SYS_STEP_ITEM I
                        WHERE A.PART_ID = '{g_PartID}'
                          AND A.VERSION = '{_Version}'
                          AND A.BOM_ID = B.BOM_ID
                          AND B.ITEM_PART_ID = D.PART_ID(+)
                          AND B.PROCESS_ID = F.PROCESS_ID(+)
                          AND D.MODEL_ID = G.MODEL_ID(+)
                          AND B.PROCESS_ID = H.PROCESS_ID(+)
                          AND B.STEP_ITEM_ID = H.STEP_ITEM_ID(+)
                          AND H.STEP_ITEM_ID = I.STEP_ITEM_ID(+) ";




            //gv_bom
            //��ܿ�ܪ��Ƹ��Ӷ����
            LV1.Items.Clear();
            if (TreeBomData.SelectedNode.Level > 1)
            {

                int iIndex = Convert.ToInt32(TreeBomData.SelectedNode.Tag.ToString());
                LV1.Items.Add(LVData.Items[iIndex].SubItems[15].Text);
                LV1.Items[0].SubItems.Add(LVData.Items[iIndex].SubItems[14].Text);
                LV1.Items[0].SubItems.Add(LVData.Items[iIndex].SubItems[0].Text);
                LV1.Items[0].SubItems.Add(LVData.Items[iIndex].SubItems[6].Text);
                LV1.Items[0].SubItems.Add(LVData.Items[iIndex].SubItems[2].Text);
                LV1.Items[0].SubItems.Add(LVData.Items[iIndex].SubItems[17].Text);
                LV1.Items[0].ImageIndex = 2;
                LV1.Items[0].StateImageIndex = LV1.Items[0].ImageIndex;
            }
            else 
            {

                sSQL = 
                $@"SELECT * 
                FROM 
                (
                    {BaseSQL}
                ) TB1
                WHERE STEP_ITEM_NAME = '{_CurrentNode}'";
                

                var ds = ClientUtils.ExecuteSQL(sSQL);

                foreach (DataRow row in ds.Tables[0].Rows) 
                {
                    ListViewItem item = new ListViewItem(row["STEP_ITEM_CODE"].ToString());
                    item.SubItems.Add(row["STEP_ITEM_NAME"].ToString());
                    item.SubItems.Add(row["ITEM_PART_NO"].ToString());
                    item.SubItems.Add(row["SPEC1"].ToString());
                    item.SubItems.Add(row["ITEM_COUNT"].ToString());
                    item.SubItems.Add(row["UOM"].ToString());
                    item.ImageIndex = 2;
                    LV1.Items.Add(item);


                    //ListViewItem item = new ListViewItem(row["PROCESS_CODE"].ToString());
                    //item.SubItems.Add(row["PROCESS_NAME"].ToString());
                    //item.SubItems.Add(row["ITEM_PART_NO"].ToString());
                    //item.SubItems.Add(row["SPEC1"].ToString());
                    //item.SubItems.Add(row["ITEM_COUNT"].ToString());
                    //item.SubItems.Add(row["UOM"].ToString());
                    //item.ImageIndex = 2;
                    //LV1.Items.Add(item);
                }
            }

            //LV1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            for (int i = 0; i < LV1.Columns.Count; i++)
            {
                myHelper.AdjustListViewColumnWidth(LV1, i);
            }
        }

        private void TreeBomData_DragOver(object sender, DragEventArgs e)
        {
            //�����ʨ�`�I�W,�Ӹ`�I�|Focus�æ��Ŧ�
            TreeNode DropNode = new TreeNode();
            Point Position = TreeBomData.PointToClient(new Point(e.X, e.Y));
            DropNode = TreeBomData.GetNodeAt(Position);
            if (DropNode != null)
            {
                TreeBomData.Focus();
                TreeBomData.SelectedNode = DropNode;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            string sPartNo = editPartNo.Text = editPartNo.Text.Trim();
            if (string.IsNullOrEmpty(sPartNo))
            {
                SajetCommon.Show_Message("Please Input Part No Prefix", 0);
                editPartNo.Focus();
                return;
            }
            sPartNo = sPartNo + "%";
            string sSQL = @" select part_no,spec1,spec2 
                  from sajet.sys_part 
                  where enabled = 'Y' 
                  and part_no Like :PART_NO 
                  Order By part_no ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", sPartNo };
            //DataSet DS = ClientUtils.ExecuteSQL(sSQL, Params);
            fFilter f = new fFilter(Params);

            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editPartNo.Text = f.dgvData.CurrentRow.Cells["part_no"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editPartNo_KeyPress(sender, Key);
            }
        }

        private void editPartNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            TreeBomData.Nodes.Clear();
            LVData.Items.Clear();
            LV1.Items.Clear();
            TreeBom.Nodes.Clear();
            if (e.KeyChar == (char)Keys.Return)
            {
                Get_Bom_Ver(editPartNo.Text);
            }
        }

        public static string GET_FIELD_ID(string sTable, string sFieldName, string sFieldID, string sFieldValue)
        {
            string sSQL = " Select " + sFieldID + " FIELD_ID from " + sTable
                        + " Where " + sFieldName + " = '" + sFieldValue + "' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return dsTemp.Tables[0].Rows[0]["FIELD_ID"].ToString();
            else
                return "0";
        }

        private void ModifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TreeBomData.Nodes.Count == 0)
                return;
            if (TreeBomData.SelectedNode == null)
                return;
            if (TreeBomData.SelectedNode.Level < 2)
                return;

            int iSelectRow = Convert.ToInt32(TreeBomData.SelectedNode.Tag.ToString());

            string sPart = TreeBomData.Nodes[0].Text;
            string sVer = TreeBomData.Nodes[0].Tag.ToString();

            string sProcess = LVData.Items[iSelectRow].SubItems[1].Text;
            string sAddPart = LVData.Items[iSelectRow].SubItems[0].Text;
            string sCount = LVData.Items[iSelectRow].SubItems[2].Text;
            string sPartVersion = LVData.Items[iSelectRow].SubItems[4].Text;
            string sGroup = LVData.Items[iSelectRow].SubItems[3].Text;
            string sLocation = LVData.Items[iSelectRow].SubItems[7].Text;
            string sRowID = LVData.Items[iSelectRow].SubItems[8].Text;
            string sStep = LVData.Items[iSelectRow].SubItems[14].Text;
            string sKey = LVData.Items[iSelectRow].SubItems[16].Text;



            fData fData = new fData();
            fData.g_sPartNo = sPart;
            fData.g_sVer = sVer;
            fData.g_sProcess = sProcess;
            fData.editSubPartNo.Text = sAddPart;
            fData.editQty.Text = sCount;
            fData.editSubPartVer.Text = sPartVersion;
            fData.editGroup.Text = sGroup;
            fData.g_sChangeGroup = false;
            fData.g_sBOM_ID = g_sBOMID;
            string[] split = sLocation.Split(new Char[] { ',' });
            fData.editLocation.Lines = split;

            fData.editSubPartNo.Enabled = false;

            fData.g_sUpdateType = "Modify";
            fData.g_sRowid = sRowID;
            fData.g_sStep = sStep;
            fData.g_sKey = sKey;
            // =======Show Form==========================
            if (fData.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                string sITEM_COUNT = fData.editQty.Text;
                string sITEM_GROUP = fData.editGroup.Text;
                string sVERSION = fData.editSubPartVer.Text;
                string sPROCESS_ID = fData.g_sProcessID;
                string sITEM_PART_ID = fData.g_sItemPartID;
                string sStepID = fData.g_sStepID;
                string[] sLocations = fData.editLocation.Lines;
                sKey = fData.combKey.Text;
                var sPRIMARY_FLAG = sITEM_GROUP == "0" ? "Y" : "";

                if (g_SUPER_BOM.ToUpper().IndexOf("SUPERBOM") >= 0 )
                {
                    sSQL =
                    $@"
                    UPDATE SAJET.SYS_BOM TB_MAIN  SET ITEM_COUNT = '{sITEM_COUNT}' WHERE BOM_ID IN
                    (
                        SELECT DISTINCT TB2.BOM_ID FROM SAJET.SYS_BOM TB1,SAJET.SYS_BOM_INFO TB2 
                        WHERE TB1.BOM_ID = TB2.BOM_ID AND TB2.PART_ID IN (SELECT PART_ID FROM SAJET.SYS_PART WHERE UPPER(SPEC2) = 'SUPERBOM')
                        AND STEP_ITEM_ID = {sStepID} 
                    ) AND TB_MAIN.STEP_ITEM_ID = {sStepID} AND ITEM_PART_ID = {sITEM_PART_ID}
                    ";
                }
                else 
                {
                    sSQL = $@" Update SAJET.SYS_BOM 
                        Set ITEM_GROUP = '{sITEM_GROUP}' 
                           ,ITEM_COUNT = '{sITEM_COUNT}' 
                           ,PROCESS_ID = '{sPROCESS_ID}' 
                           ,VERSION =   '{sVERSION}' 
                           ,STEP_ITEM_ID = '{sStepID}' 
                           ,KEY_COMPONENT = '{sKey}'
                           ,UPDATE_USERID = '{g_sUserID}'
                           ,UPDATE_TIME = SYSDATE 
                           ,PRIMARY_FLAG = '{sPRIMARY_FLAG}'
                         Where Rowid = '{sRowID}' ";
                }

                ClientUtils.ExecuteSQL(sSQL);
                CopyToHistory(sRowID);

                //ITEM GROUP�ק�ᤣ��0 �B (�ק諸�������N�ƪ��D�� �� �ק諸�����N��) �h �ܰʸs�դ��ƥ�ITEM GROUP�BPROCESS
                if (sITEM_GROUP != "0" && ((TreeBomData.SelectedNode.Level == 2 && TreeBomData.SelectedNode.Nodes.Count != 0) || TreeBomData.SelectedNode.Level == 3))
                {
                    TreeNode treenode = TreeBomData.SelectedNode.Level == 3 ? TreeBomData.SelectedNode.Parent : TreeBomData.SelectedNode;
                    int iSubSelectRow = Convert.ToInt32(treenode.Tag.ToString());
                    string sSubRowID = LVData.Items[iSubSelectRow].SubItems[8].Text;
                    if (sRowID != sSubRowID)
                    {
                        ClientUtils.ExecuteSQL($@"UPDATE SAJET.SYS_BOM
   SET ITEM_GROUP = '{sITEM_GROUP}', PROCESS_ID = '{sPROCESS_ID}', UPDATE_USERID = '{g_sUserID}',
       UPDATE_TIME = SYSDATE
 WHERE ROWID = '{sSubRowID}'");
                        CopyToHistory(sSubRowID);
                    }

                    foreach (TreeNode item in treenode.Nodes)
                    {
                        iSubSelectRow = Convert.ToInt32(item.Tag.ToString());
                        sSubRowID = LVData.Items[iSubSelectRow].SubItems[8].Text;
                        if (sRowID == sSubRowID)
                            continue;

                        ClientUtils.ExecuteSQL($@"UPDATE SAJET.SYS_BOM
   SET ITEM_GROUP = '{sITEM_GROUP}', PROCESS_ID = '{sPROCESS_ID}', UPDATE_USERID = '{g_sUserID}',
       UPDATE_TIME = SYSDATE
 WHERE ROWID = '{sSubRowID}'");
                        CopyToHistory(sSubRowID);
                    }
                }

                if (sGroup != "0")
                {
                    string sFilterSQL = "WHERE BOM_ID = '" + g_sBOMID + "' AND ITEM_GROUP = '" + sGroup + "'";
                    DataSet DS = ClientUtils.ExecuteSQL("SELECT COUNT(*) CNT FROM SAJET.SYS_BOM " + sFilterSQL);
                    if (DS.Tables[0].Rows[0]["CNT"].ToString() == "1")
                    {
                        ClientUtils.ExecuteSQL("UPDATE SAJET.SYS_BOM SET ITEM_GROUP = '0', PRIMARY_FLAG = 'Y'" + sFilterSQL);
                        ClientUtils.ExecuteSQL("INSERT INTO SAJET.SYS_HT_BOM SELECT * FROM SAJET.SYS_BOM " + sFilterSQL);
                    }
                }


                //Insert Bom Location====
                sSQL = " Delete SAJET.SYS_BOM_LOCATION "
                     + " Where BOM_ID = '" + g_sBOMID + "' "
                     + " and Item_Part_Id = '" + sITEM_PART_ID + "' ";
                ClientUtils.ExecuteSQL(sSQL);
                for (int i = 0; i <= sLocations.Length - 1; i++)
                {
                    sSQL = " Insert Into SAJET.SYS_BOM_LOCATION "
                         + " (BOM_ID,ITEM_PART_ID,ITEM_GROUP,LOCATION,UPDATE_USERID) "
                         + " Values "
                         + " ('" + g_sBOMID + "','" + sITEM_PART_ID + "','" + sITEM_GROUP + "','" + sLocations[i].ToString() + "' "
                         + " ,'" + g_sUserID + "') ";
                    ClientUtils.ExecuteSQL(sSQL);
                }

                ShowPartDetail(editPartNo.Text, combVer.Text);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception - " + ex.Message, 0);
            }
        }

        private void fMain_Shown(object sender, EventArgs e)
        {
            editPartNo.Focus();
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(editPartNo.Text))// || TreeBomData.Nodes.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Part No", 2), 0);
                return;
            }

            if (string.IsNullOrEmpty(combVer.Text))
                combVer.Text = "N/A";

            Assembly assembly = null;
            object obj = null;
            Type type = null;
            string strApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!File.Exists(strApplicationPath + "\\CBOMLevel.dll"))//�b���a�ݵo�{DLL�ɮ׫h���t�~�զr��A�_�h�q��Ʈw���j�M�{���b��!
            {
                strApplicationPath = Path.GetDirectoryName(Application.ExecutablePath);
                string strSQL = @"  SELECT A.EXE_FILENAME
                                                FROM SAJET.SYS_PROGRAM_NAME A ,
                                                SAJET.SYS_PROGRAM_FUN_NAME B
                                                WHERE A.PROGRAM = B.PROGRAM
                                                AND B.DLL_FILENAME = 'CBOMLevel.dll'";
                DataSet dsEXE_FILENAME = ClientUtils.ExecuteSQL(strSQL);
                if (dsEXE_FILENAME.Tables[0].Rows.Count > 0)
                {
                    strApplicationPath += "\\" + dsEXE_FILENAME.Tables[0].Rows[0][0].ToString();
                }
                //�����M��½Ķ�ɮ�
                if (File.Exists(strApplicationPath + "\\CBOMLevel.xml"))
                {
                    File.Copy(strApplicationPath + "\\CBOMLevel.xml", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\CBOM.xml", true);
                }
            }
            try
            {
                //�ո˸�T              
                assembly = Assembly.LoadFrom(strApplicationPath + "\\CBOMLevel.dll");
                type = assembly.GetType(("CBOMLevel.fMain"));
                obj = assembly.CreateInstance(type.FullName, true, BindingFlags.CreateInstance, null, new object[] { editPartNo.Text, combVer.Text }, null, null);
                //�վ�j�p
                //                ((Form)obj).Size = new Size(((Form)obj).Width * 2, ((Form)obj).Height);
                //((Form)obj).ShowDialog();

                /*obj = assembly.CreateInstance(type.FullName, true);*/
                ((Form)obj).StartPosition = FormStartPosition.CenterScreen;
                ((Form)obj).WindowState = FormWindowState.Normal;
                ((Form)obj).ShowDialog();

            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Load Function Error" + Environment.NewLine + ex.Message, 0);
            }

            //            string sSQL = "";
            //            DataSet dsTemp;

            //            object[][] Params = new object[1][];
            //            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", g_sBOMID };

            //            try
            //            {
            //                //���⤧�e�i���������R��
            //                sSQL = "DELETE FROM SAJET.SYS_BOM WHERE BOM_ID = :BOM_ID AND JOB_EXTEND = '5'";
            //                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            //                sSQL = "DELETE FROM SAJET.SYS_BOM_LOCATION WHERE BOM_ID = :BOM_ID AND JOB_EXTEND = '5'";
            //                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);



            //                //�إ�SAJET.SYS_BOM������
            //                sSQL = @"INSERT INTO SAJET.SYS_BOM
            //                        (BOM_ID, ITEM_PART_ID, ITEM_GROUP, ITEM_COUNT, PROCESS_ID, VERSION, 
            //                        UPDATE_USERID, UPDATE_TIME, ENABLED, LOCATION, UNIT, ITEM_SEQ ,JOB_EXTEND)
            //                        SELECT A.BOM_ID, B.ITEM_PART_ID, decode(B.ITEM_GROUP,0,0,B.BOM_ID||B.ITEM_GROUP) ITEM_GROUP, 
            //                        B.ITEM_COUNT, B.PROCESS_ID, B.VERSION, 
            //                        B.UPDATE_USERID, B.UPDATE_TIME, B.ENABLED, B.LOCATION, B.UNIT, B.ITEM_SEQ, '5' JOB_EXTEND
            //                        FROM SAJET.SYS_BOM A, SAJET.SYS_BOM B, SAJET.SYS_BOM_INFO C
            //                        WHERE A.ITEM_PART_ID = C.PART_ID AND C.BOM_ID = B.BOM_ID
            //                        AND A.BOM_ID = :BOM_ID AND A.JOB_EXTEND = '3'";
            //                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            //                //�إ�SAJET.SYS_BOM_LOCATION������
            //                sSQL = @"INSERT INTO SAJET.SYS_BOM_LOCATION
            //                        (BOM_ID, ITEM_PART_ID, ITEM_GROUP, VERSION, LOCATION,
            //                        UPDATE_USERID, UPDATE_TIME, ENABLED, ITEM_SEQ ,JOB_EXTEND)
            //                        SELECT A.BOM_ID, B.ITEM_PART_ID, decode(B.ITEM_GROUP,0,0,B.BOM_ID||B.ITEM_GROUP) ITEM_GROUP, 
            //                        B.VERSION, B.LOCATION, B.UPDATE_USERID, B.UPDATE_TIME, B.ENABLED, B.ITEM_SEQ, '5' JOB_EXTEND
            //                        FROM SAJET.SYS_BOM A, SAJET.SYS_BOM_LOCATION B, SAJET.SYS_BOM_INFO C
            //                        WHERE A.ITEM_PART_ID = C.PART_ID AND C.BOM_ID = B.BOM_ID
            //                        AND A.BOM_ID = :BOM_ID AND A.JOB_EXTEND = '3'";
            //                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);


            //                //��sSAJET.SYS_BOM_INFO
            //                Params = new object[2][];
            //                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", g_sBOMID };
            //                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DOWNLOAD_USERID", g_sUserID };

            //                sSQL = @"UPDATE SAJET.SYS_BOM_INFO 
            //                         SET DOWNLOAD_BOM = 'Y', DOWNLOAD_USERID = :DOWNLOAD_USERID, DOWNLOAD_TIME = SYSDATE
            //                         WHERE BOM_ID = :BOM_ID";
            //                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            //                ShowPartDetail(editPartNo.Text, combVer.Text);

            //            }
            //            catch (Exception ex)
            //            {
            //                SajetCommon.Show_Message(ex.Message, 0);
            //                return;
            //            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            string path;
            string sSheet;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();
            path = openFile.FileName;
            if (path.Trim() == "")
                return;
            /*
            objDS = new DataSet();

            string sType = Path.GetExtension(path);
            string strCon = "";
            string strCom = "";
            //Office 2007
            if (sType == ".xlsx")
            {
                strCon = " Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1'";
            }
            else
            {
                strCon = " Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;'";

            }
            //strCon = " Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1'";
            OleDbConnection objConn = new OleDbConnection(strCon);
            try
            {
                objConn.Open();
                DataTable dataTable = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                sSheet = dataTable.Rows[0]["TABLE_NAME"].ToString();
            }
            catch (Exception ex)
            {
                objConn.Close();
                string reason = "�}��Excel���~:" + ex.Message;
                SajetCommon.Show_Message(reason, 0);
                return;
            }
            strCom = " SELECT * FROM [" + sSheet + "] ";

            OleDbDataAdapter objCmd = new OleDbDataAdapter(strCom, objConn);
            try
            {
                objCmd.Fill(objDS);
                objCmd.Dispose();
                objConn.Close();
            }
            catch (Exception ex)
            {
                objCmd.Dispose();
                objConn.Close();
                string reason = "���J���~!!SHEET�W���~:" + ex.Message;
                SajetCommon.Show_Message(reason, 0);
                return;
            }
            ExcelRowcount = CaculateCount();
            string sFileName = System.IO.Path.GetFileName(path);
            if (ExcelRowcount == 0)
            {
                string reason = sFileName + " " + "�s�Ʈ榡���~ !";
                SajetCommon.Show_Message(reason, 0);
                return;
            }
             */
            ExportExcelAllVer.ExcelEditAll ExcelClass = new ExportExcelAllVer.ExcelEditAll();
            try
            {
                ExcelClass.Open(path);
                object ExcelSheet = ExcelClass.GetSheet(1);
                if (ExcelClass.GetCellValue1(ExcelSheet, 1, 1).ToString() != "Part No")
                {
                    SajetCommon.Show_Message("File Format Error", 0);
                    return;
                }
                int iColCount = 6;
                int iRowIndex = 2;
                int iOKCount = 0;
                int iNGCount = 0;

                g_tsErrorData = new StringCollection();
                g_dtDateTime = ClientUtils.GetSysDate();
                while (true)
                {
                    try
                    {
                        string[] sDataList = new string[iColCount];
                        Application.DoEvents();
                        try
                        {
                            object oFieldName;
                            oFieldName = ExcelClass.GetCellValue1(ExcelSheet, 1, iRowIndex);
                            if (oFieldName == null)
                                break;
                        }
                        catch { break; }



                        for (int iCol = 1; iCol <= iColCount; iCol++)
                        {
                            object oFieldName;
                            oFieldName = ExcelClass.GetCellValue1(ExcelSheet, iCol, iRowIndex);
                            if (oFieldName == null)
                                sDataList[iCol - 1] = "";
                            else
                                sDataList[iCol - 1] = Convert.ToString(oFieldName);
                        }
                        string sPartNo = sDataList[0].Trim();
                        string sItemPartNo = sDataList[1].Trim();                        
                        string sProcessName = sDataList[2].Trim();
                        //excel�W�����אּSYS_STEP_ITEM.STEP_ITEM_CODE
                        string sStepName = sDataList[3].Trim();
                        string sKey = sDataList[4].Trim();
                        string sQty = sDataList[5].Trim();
                        string sMsg = "";
                        if (!FileTran(sPartNo, sItemPartNo, sProcessName, sStepName, sKey, sQty, ref sMsg))
                        {
                            iNGCount += 1;
                            g_tsErrorData.Add(sMsg);
                        }
                        else
                            iOKCount += 1;
                        iRowIndex += 1;
                    }
                    catch (Exception ex)
                    {
                        SajetCommon.Show_Message(ex.Message, 0);
                        return;
                    }
                }


                /*
                  for (int i = 0; i < ExcelRowcount; i++)
                  {
                      string sPartNo = objDS.Tables[0].Rows[i][0].ToString().Trim();
                      string sItemPartNo = objDS.Tables[0].Rows[i][1].ToString().Trim();
                      string sProcessName = objDS.Tables[0].Rows[i][2].ToString().Trim();
                      string sMsg = "";
                      if (!FileTran(sPartNo, sItemPartNo, sProcessName, ref sMsg))
                      {
                          g_tsErrorData.Add(sMsg);
                          continue;
                      }
                  }
                 */
                if (g_tsErrorData.Count > 0)
                {
                    fErrorData f = new fErrorData();
                    for (int a = 0; a <= g_tsErrorData.Count - 1; a++)
                    {
                        string[] sValue = g_tsErrorData[a].ToString().Split('@');
                        f.dgvMsg.Rows.Add();
                        f.dgvMsg.Rows[f.dgvMsg.Rows.Count - 1].Cells["PART_NO"].Value = sValue[0];
                        f.dgvMsg.Rows[f.dgvMsg.Rows.Count - 1].Cells["ITEM_PART"].Value = sValue[1];
                        f.dgvMsg.Rows[f.dgvMsg.Rows.Count - 1].Cells["PROCESS_NAME"].Value = sValue[2];
                        f.dgvMsg.Rows[f.dgvMsg.Rows.Count - 1].Cells["STEP_ITEM_NAME"].Value = sValue[3];
                        f.dgvMsg.Rows[f.dgvMsg.Rows.Count - 1].Cells["KEY_COMPONENT"].Value = sValue[4];
                        f.dgvMsg.Rows[f.dgvMsg.Rows.Count - 1].Cells["QTY"].Value = sValue[5];
                        f.dgvMsg.Rows[f.dgvMsg.Rows.Count - 1].Cells["ERR_MESSAGE"].Value = sValue[6];
                    }
                    f.ShowDialog();
                }
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Import OK") + Environment.NewLine
                                        + SajetCommon.SetLanguage("OK Count") + " : " + iOKCount.ToString() + Environment.NewLine
                                        + SajetCommon.SetLanguage("NG Count") + " : " + iNGCount.ToString(), -1);
            }
            finally
            {
                ExcelClass.Close();
            }
        }

        private bool FileTran(string sPartNo, string sItemPartNo, string sProcessName, string sStepName,string sKey,string sQty, ref string sError)
        {
            string sPartID = GET_FIELD_ID("SAJET.SYS_PART", "PART_NO", "PART_ID", sPartNo);
            string sSubPartID = GET_FIELD_ID("SAJET.SYS_PART", "PART_NO", "PART_ID", sItemPartNo);
            string sProcessID = GET_FIELD_ID("SAJET.SYS_PROCESS", "PROCESS_NAME", "PROCESS_ID", sProcessName);

            string sStepID = GET_FIELD_ID("SAJET.SYS_STEP_ITEM", "STEP_ITEM_CODE", "STEP_ITEM_ID", sStepName);
            string sMsg = "";
            if (sPartID == "0")
            {
                sMsg = SajetCommon.SetLanguage("Part No Error", 1);
                sError = sPartNo + "@" + sItemPartNo + "@" + sProcessName + "@" + sStepName + "@" + sKey + "@" + sQty + "@" + sMsg;
                return false;
            }
            if (sSubPartID == "0")
            {
                sMsg = SajetCommon.SetLanguage("Item Part No Error", 1);
                sError = sPartNo + "@" + sItemPartNo + "@" + sProcessName + "@" + sStepName + "@" + sKey + "@" + sQty + "@" + sMsg;
                return false;
            }
            if (sProcessID == "0")
            {
                sMsg = SajetCommon.SetLanguage("Process Error", 1);
                sError = sPartNo + "@" + sItemPartNo + "@" + sProcessName + "@" + sStepName + "@" + sKey + "@" + sQty + "@" + sMsg;
                return false;
            }
            if (sProcessID == "0")
            {
                sMsg = SajetCommon.SetLanguage("Step Error", 1);
                sError = sPartNo + "@" + sItemPartNo + "@" + sProcessName + "@" + sStepName + "@" + sKey + "@" + sQty + "@" + sMsg;
                return false;
            }
            if (string.IsNullOrEmpty(sQty))
            {
                sMsg = SajetCommon.SetLanguage("Qty Error", 1);
                sError = sPartNo + "@" + sItemPartNo + "@" + sProcessName + "@" + sStepName + "@" + sKey + "@" + sQty + "@" + sMsg;
                return false;
            }
            if (sKey == "Y" || sKey == "N")
            {
            }
            else
            {
                sMsg = SajetCommon.SetLanguage("Key Error", 1);
                sError = sPartNo + "@" + sItemPartNo + "@" + sProcessName + "@" + sStepName + "@" + sKey + "@" + sQty + "@" + sMsg;
                return false;
            }

            if (!checkStep(sPartNo, sStepName, sProcessName))
            {
                sMsg = SajetCommon.SetLanguage("Part Step Error", 1);
                sError = sPartNo + "@" + sItemPartNo + "@" + sProcessName + "@" + sStepName + "@" + sKey + "@" + sQty + "@" + sMsg;
                return false;
            }

            string sBomID = GET_FIELD_ID("SAJET.SYS_BOM_INFO", "PART_ID", "BOM_ID", sPartID);
            if (sBomID == "0")
            {
                /*
                sMsg = SajetCommon.SetLanguage("No Bom", 1);
                sError = sPartNo + "@" + sItemPartNo + "@" + sProcessName + "@" + sMsg;
                return false;            
                 */
                sBomID = GetMaxID("SAJET.SYS_BOM_INFO", "BOM_ID", 8, ref sMsg);
                if (sBomID == "0")
                {
                    sMsg = "Get MaxID Error," + sMsg;
                    sError = sPartNo + "@" + sItemPartNo + "@" + sProcessName + "@" + sStepName + "@" + sKey + "@" + sQty + "@" + sMsg;
                    return false;
                }
                string sVersion = "N/A";
                if (!InsertBOMHeader(sBomID, sPartID, sVersion, ref sMsg))
                {
                    sError = sPartNo + "@" + sItemPartNo + "@" + sProcessName + "@" + sStepName + "@" + sKey + "@" + sQty + "@" + sMsg;
                    return false;
                }
            }
            //�ˬdBom���O�_����Item
            sSQL = @"SELECT ITEM_PART_ID,ITEM_GROUP,ROWID
                    FROM SAJET.SYS_BOM
                    WHERE BOM_ID = :BOM_ID AND ITEM_PART_ID = :ITEM_PART_ID
                      AND STEP_ITEM_ID = :STEP_ITEM_ID AND PROCESS_ID = :PROCESS_ID";
            object[][] Params = new object[4][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBomID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sSubPartID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STEP_ITEM_ID", sStepID };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count <= 0)
            {
                sSQL = "INSERT INTO SAJET.SYS_BOM (BOM_ID,ITEM_PART_ID,ITEM_GROUP,ITEM_COUNT,PROCESS_ID,VERSION,UPDATE_USERID,UPDATE_TIME,STEP_ITEM_ID,KEY_COMPONENT  "
                    + " ,PRIMARY_FLAG ) "
                    + " VALUES "
                    + " (:BOM_ID,:ITEM_PART_ID,:ITEM_GROUP,:ITEM_COUNT,:PROCESS_ID,'N/A',:UPDATE_USERID,:UPDATE_TIME,:STEP_ITEM_ID,:KEY_COMPONENT "
                    + " ,'Y' )  ";

                Params = new object[9][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBomID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sSubPartID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_GROUP", "0" };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_COUNT", sQty };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtDateTime };
                Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STEP_ITEM_ID", sStepID };
                Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "KEY_COMPONENT", sKey };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                sSQL = "Insert Into SAJET.SYS_HT_BOM "
                     + "Select * from SAJET.SYS_BOM "
                     + " WHERE BOM_ID =:BOM_ID "
                     + "   AND ITEM_PART_ID =:ITEM_PART_ID ";
                Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBomID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sSubPartID };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                return true;
            }
            string sGroup = dsTemp.Tables[0].Rows[0]["ITEM_GROUP"].ToString();
            string sRowID = dsTemp.Tables[0].Rows[0]["ROWID"].ToString();
            //���ʸ�Ʈw
            if (sGroup == "0")
            {
                sSQL = " Update SAJET.SYS_BOM "
                        + " Set PROCESS_ID = :PROCESS_ID "
                        + "    ,STEP_ITEM_ID =:STEP_ITEM_ID "
                        + "    ,KEY_COMPONENT =:KEY_COMPONENT "
                        + "    ,UPDATE_USERID =:UPDATE_USERID "
                        + "    ,UPDATE_TIME = :UPDATE_TIME  "
                        + "    ,ITEM_COUNT = :ITEM_COUNT  "
                        + " Where Rowid = :DATA_ROWID ";
                Params = new object[7][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtDateTime };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATA_ROWID", sRowID };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STEP_ITEM_ID", sStepID };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "KEY_COMPONENT", sKey };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_COUNT", sQty };
                ClientUtils.ExecuteSQL(sSQL, Params);
                CopyToHistory(sRowID);
            }
            else
            {
                sSQL = @"SELECT ROWID
                        FROM SAJET.SYS_BOM
                        WHERE BOM_ID = '" + sBomID + "' "
                        + " AND ITEM_GROUP = '" + sGroup + "' ";
                DataSet ds = ClientUtils.ExecuteSQL(sSQL);
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    sRowID = ds.Tables[0].Rows[i]["ROWID"].ToString();
                    sSQL = " Update SAJET.SYS_BOM "
                        + " Set PROCESS_ID = :PROCESS_ID "
                        + "    ,STEP_ITEM_ID =:STEP_ITEM_ID "
                        + "    ,KEY_COMPONENT =:KEY_COMPONENT "
                        + "    ,UPDATE_USERID = :UPDATE_USERID "
                        + "    ,UPDATE_TIME = :UPDATE_TIME "
                        + "    ,ITEM_COUNT = :ITEM_COUNT  "
                        + " Where Rowid = :DATA_ROWID ";
                    Params = new object[7][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtDateTime };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATA_ROWID", sRowID };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STEP_ITEM_ID", sStepID };
                    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "KEY_COMPONENT", sKey };
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_COUNT", sQty };
                    ClientUtils.ExecuteSQL(sSQL, Params);
                    CopyToHistory(sRowID);
                }
            }

            return true;
        }
        private bool InsertBOMHeader(string sBOMID, string sPartID, string sVersion, ref string sMessage)
        {
            try
            {
                object[][] Params = new object[5][];
                string sSQL = " INSERT INTO SAJET.SYS_BOM_INFO "
                     + " (BOM_ID, PART_ID, VERSION, UPDATE_USERID, UPDATE_TIME) "
                     + " VALUES "
                     + " (:BOM_ID, :PART_ID, :VERSION, :UPDATE_USERID, :UPDATE_TIME) ";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBOMID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sVersion };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtDateTime };
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                sMessage = "OK";
                return true;
            }
            catch (Exception ex)
            {
                sMessage = ex.Message;
                return false;
            }
        }
        private string GetMaxID(string sTable, string sField, int iIDLength, ref string sMessage)
        {
            try
            {
                object[][] Params = new object[5][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TFIELD", sField };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTABLE", sTable };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNUM", iIDLength.ToString() };
                Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                Params[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "T_MAXID", "" };
                DataSet dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_GET_MAXID", Params);

                string sRes = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    sMessage = sRes;
                    return "0";
                }

                return dsTemp.Tables[0].Rows[0]["T_MAXID"].ToString();
            }
            catch (Exception ex)
            {
                sMessage = "SAJET.SJ_GET_MAXID," + sTable + "," + ex.Message;
                return "0";
            }
        }
        private int CaculateCount()
        {
            int co;
            for (int i = objDS.Tables[0].Rows.Count - 1; i >= 0; i--)
            {
                if (objDS.Tables[0].Rows[i][0].ToString().Trim() == "")
                {
                    try
                    {
                        objDS.Tables[0].Rows.RemoveAt(i);
                        continue;
                    }
                    catch
                    { }
                }
                else
                {
                    continue;
                }
            }
            return co = objDS.Tables[0].Rows.Count;
        }



    }
}

