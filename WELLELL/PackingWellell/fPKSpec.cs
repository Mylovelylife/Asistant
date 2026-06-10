using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
namespace PackingDll
{
    public partial class fPKSpec : Form
    {
        public fPKSpec()
        {
            InitializeComponent();
        }

        string sSQL;
        DataSet dsTemp;
        fMain fM = new fMain();

        private void fPKSpec_Load(object sender, EventArgs e)
        {
            panel4.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel4.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;

            LVAll.Items.Clear();
            //此工單有定義之包裝方式
            sSQL = "SELECT a.PKSPEC_ID,c.PKSPEC_NAME, c.PALLET_QTY, c.CARTON_QTY, c.BOX_QTY "
                 + "FROM SAJET.G_PACK_SPEC a, SAJET.SYS_PKSPEC c "
                 + "WHERE a.WORK_ORDER = '" + LabWO.Text + "' "
                 + "AND a.PKSPEC_ID = C.PKSPEC_ID "
                 + "Order By a.sequence ";

            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                LVAll.Items.Add(dsTemp.Tables[0].Rows[i]["PKSPEC_NAME"].ToString());
                LVAll.Items[LVAll.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["BOX_QTY"].ToString());
                LVAll.Items[LVAll.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["CARTON_QTY"].ToString());
                LVAll.Items[LVAll.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["PALLET_QTY"].ToString());
                LVAll.Items[LVAll.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["PKSPEC_ID"].ToString());
            }

            //此料有定義的包裝方式
            sSQL = "SELECT B.PKSPEC_ID,C.PKSPEC_NAME, c.PALLET_QTY, c.CARTON_QTY, c.BOX_QTY "
                 + "FROM SAJET.SYS_PART A "
                 + "   , SAJET.SYS_PART_PKSPEC B "
                 + "   , SAJET.SYS_PKSPEC C "
                 + "WHERE  A.PART_NO = '" + LabPartNo.Text + "' "
                 + "AND    A.PART_ID = B.PART_ID "
                 + "AND    B.PKSPEC_ID = C.PKSPEC_ID "
                 + "AND    C.ENABLED='Y' "
                 + "Order BY C.PKSPEC_NAME ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                string s = dsTemp.Tables[0].Rows[i]["PKSPEC_NAME"].ToString();
                if (LVAll.FindItemWithText(s, false, 0) == null)
                {
                    LVAll.Items.Add(dsTemp.Tables[0].Rows[i]["PKSPEC_NAME"].ToString());
                    LVAll.Items[LVAll.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["BOX_QTY"].ToString());
                    LVAll.Items[LVAll.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["CARTON_QTY"].ToString());
                    LVAll.Items[LVAll.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["PALLET_QTY"].ToString());
                    LVAll.Items[LVAll.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["PKSPEC_ID"].ToString());
                }
            }
            ClientUtils.SetLanguage(this, fMain.g_sExeName);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (LVChoose.Items.Count == 0)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            //更改此工單可用的包裝方式
            sSQL = "delete from sajet.g_pack_spec "
                 + "where work_order = '" + LabWO.Text + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            for (int i = 0; i <= LVChoose.Items.Count - 1; i++)
            {
                string sPKID = LVChoose.Items[i].SubItems[4].Text;
                string sPalletQty = LVChoose.Items[i].SubItems[3].Text;
                string sCartonQty = LVChoose.Items[i].SubItems[2].Text;
                string sBoxQty = LVChoose.Items[i].SubItems[1].Text;

                sSQL = " Insert Into SAJET.G_PACK_SPEC "
                     + " (WORK_ORDER,PART_ID,PKSPEC_ID,PALLET_CAPACITY,CARTON_CAPACITY,BOX_CAPACITY,Sequence,UPDATE_USERID) "
                     + " Values "
                     + " ('" + LabWO.Text + "','" + fMain.g_sPartID + "','" + sPKID + "','" + sPalletQty + "','" + sCartonQty + "','" + sBoxQty + "','" + (i + 1).ToString() + "','" + fMain.g_sUserID + "')";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }

            DialogResult = DialogResult.OK;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (LVAll.SelectedItems.Count == 0)
                return;
            for (int j = 0; j <= LVAll.SelectedItems.Count - 1; j++)
            {
                LVChoose.Items.Add(LVAll.SelectedItems[j].Text);
                for (int i = 1; i <= 4; i++)
                {
                    string sData = LVAll.SelectedItems[j].SubItems[i].Text;
                    LVChoose.Items[LVChoose.Items.Count - 1].SubItems.Add(sData);
                }
            }
            for (int j = LVAll.SelectedItems.Count - 1; j >= 0; j--)
            {
                LVAll.SelectedItems[j].Remove();
            }
        }

        private void btnUnSelect_Click(object sender, EventArgs e)
        {
            if (LVChoose.SelectedItems.Count == 0)
                return;
            for (int j = 0; j <= LVChoose.SelectedItems.Count - 1; j++)
            {
                LVAll.Items.Add(LVChoose.SelectedItems[j].Text);
                for (int i = 1; i <= 4; i++)
                {
                    string sData = LVChoose.SelectedItems[j].SubItems[i].Text;
                    LVAll.Items[LVAll.Items.Count - 1].SubItems.Add(sData);
                }
            }
            for (int j = LVChoose.SelectedItems.Count - 1; j >= 0; j--)
            {
                LVChoose.SelectedItems[j].Remove();
            }
        }

        private void btnSortDown_Click(object sender, EventArgs e)
        {
            if (LVChoose.SelectedItems.Count == 0)
                return;
            if (LVChoose.SelectedItems[0].Index == LVChoose.Items.Count - 1)
                return;
            int iSelectIndex = LVChoose.SelectedItems[0].Index;
            ListViewItem item = new ListViewItem();
            item = LVChoose.SelectedItems[0];
            LVChoose.Items[iSelectIndex].Remove();
            LVChoose.Items.Insert(iSelectIndex + 1, item);
        }

        private void btnSortUp_Click(object sender, EventArgs e)
        {
            if (LVChoose.SelectedItems.Count == 0)
                return;
            if (LVChoose.SelectedItems[0].Index == 0)
                return;
            int iSelectIndex = LVChoose.SelectedItems[0].Index;
            ListViewItem item = new ListViewItem();
            item = LVChoose.SelectedItems[0];
            LVChoose.Items[iSelectIndex].Remove();
            LVChoose.Items.Insert(iSelectIndex - 1, item);
        }

        private void fPKSpec_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
        }

        private void LabPartNo_Click(object sender, EventArgs e)
        {

        }

        private void LabWO_Click(object sender, EventArgs e)
        {

        }
    }
}