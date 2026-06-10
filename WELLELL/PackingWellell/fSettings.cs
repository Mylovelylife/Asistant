using SajetClass;
using System;
using System.Data;
using System.Windows.Forms;

namespace PackingDll
{
    public partial class fSettings : Form
    {
        public string sPrintPort;
        public string sActionValue;

        private string program;
        private string project;
        private string terminalId;
        private string userId;
        string[] g_sCSN_Type = new string[] { "Input", "System Create", "Input (Released)", "Don't Change", "CSN=SN", "CSN=SN (Check)" };
        string[] g_sCreate_Type = new string[] { "Input", "System Create", "Input (Released)" };
        string[] g_sPrintMethod = new string[] { "CodeSoft", "Bartender", "DLL" };
        string[] g_sPkBase = new string[] { "Work Order", "Part No" };
        string[] g_sPort;
        string[] g_sBartender = new string[] { "Standard", "DataSource" };

        public fSettings(string program, string project, string terminalId, string userId)
        {
            InitializeComponent();

            this.program = program;
            this.project = project;
            this.terminalId = terminalId;
            this.userId = userId;
        }

        DataSet dsTemp;

        private void fSettings_Load(object sender, EventArgs e)
        {
            //print Port            
            g_sPort = sPrintPort.TrimEnd(new Char[] { ',' }).Split(new Char[] { ',' });
            combCSNPort.Items.Clear();
            combCSNPort.Items.AddRange(g_sPort);
            combInnerPort.Items.Clear();
            combInnerPort.Items.AddRange(g_sPort);
            combBoxPort.Items.Clear();
            combBoxPort.Items.AddRange(g_sPort);
            combCartonPort.Items.Clear();
            combCartonPort.Items.AddRange(g_sPort);
            combPalletPort.Items.Clear();
            combPalletPort.Items.AddRange(g_sPort);

            //Packing Action     
            string[] sAction = sActionValue.TrimEnd(new Char[] { ',' }).Split(new Char[] { ',' });
            combPKAction.Items.Clear();
            combPKActionIndex.Items.Clear();
            for (int i = 0; i <= sAction.Length - 1; i++)
            {
                combPKAction.Items.Add(sAction[i].ToString().Substring(1));
                combPKActionIndex.Items.Add(sAction[i].ToString().Substring(0, 1));
            }
            ShowOption();
            ClientUtils.SetLanguage(this, project);
        }

        private void combCSNMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            combCSNPort.Enabled = true;
            int iIndex = combCSNMethod.SelectedIndex;
            if (iIndex == 0)
            {
                combCSNPort.SelectedIndex = -1;
                combCSNPort.Enabled = false;
            }
            else if (iIndex == 1)
            {
                combCSNPort.Items.Clear();
                combCSNPort.Items.AddRange(g_sBartender);
                combCSNPort.SelectedIndex = 0;
            }
            else if (iIndex == 2)
            {
                combCSNPort.Items.Clear();
                combCSNPort.Items.AddRange(g_sPort);
                combCSNPort.SelectedIndex = 0;
            }
        }

        private void combInnerMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            combInnerPort.Enabled = true;
            int iIndex = combInnerMethod.SelectedIndex;
            if (iIndex == 0)
            {
                combInnerPort.SelectedIndex = -1;
                combInnerPort.Enabled = false;
            }
            else if (iIndex == 1)
            {
                combInnerPort.Items.Clear();
                combInnerPort.Items.AddRange(g_sBartender);
                combInnerPort.SelectedIndex = 0;
            }
            else if (iIndex == 2)
            {
                combInnerPort.Items.Clear();
                combInnerPort.Items.AddRange(g_sPort);
                combInnerPort.SelectedIndex = 0;
            }
        }

        private void combBoxMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            combBoxPort.Enabled = true;
            int iIndex = combBoxMethod.SelectedIndex;
            if (iIndex == 0)
            {
                combBoxPort.SelectedIndex = -1;
                combBoxPort.Enabled = false;
            }
            else if (iIndex == 1)
            {
                combBoxPort.Items.Clear();
                combBoxPort.Items.AddRange(g_sBartender);
                combBoxPort.SelectedIndex = 0;
            }
            else if (iIndex == 2)
            {
                combBoxPort.Items.Clear();
                combBoxPort.Items.AddRange(g_sPort);
                combBoxPort.SelectedIndex = 0;
            }
            /*
            combBoxPort.Enabled = true;
            if (combBoxMethod.SelectedIndex == 0)
            {
                combBoxPort.SelectedIndex = -1;
                combBoxPort.Enabled = false;
            }
            else
            {
                combBoxPort.Enabled = true;
            }
             */
        }

        private void combCartonMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            combCartonPort.Enabled = true;
            int iIndex = combCartonMethod.SelectedIndex;
            if (iIndex == 0)
            {
                combCartonPort.SelectedIndex = -1;
                combCartonPort.Enabled = false;
            }
            else if (iIndex == 1)
            {
                combCartonPort.Items.Clear();
                combCartonPort.Items.AddRange(g_sBartender);
                combCartonPort.SelectedIndex = 0;
            }
            else if (iIndex == 2)
            {
                combCartonPort.Items.Clear();
                combCartonPort.Items.AddRange(g_sPort);
                combCartonPort.SelectedIndex = 0;
            }
            /*
            if (combCartonMethod.SelectedIndex == 0)
            {
                combCartonPort.SelectedIndex = -1;
                combCartonPort.Enabled = false;
            }
            else
            {
                combCartonPort.Enabled = true;
            }
             */
        }

        private void combPalletMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            combPalletPort.Enabled = true;
            int iIndex = combPalletMethod.SelectedIndex;
            if (iIndex == 0)
            {
                combPalletPort.SelectedIndex = -1;
                combPalletPort.Enabled = false;
            }
            else if (iIndex == 1)
            {
                combPalletPort.Items.Clear();
                combPalletPort.Items.AddRange(g_sBartender);
                combPalletPort.SelectedIndex = 0;
            }
            else if (iIndex == 2)
            {
                combPalletPort.Items.Clear();
                combPalletPort.Items.AddRange(g_sPort);
                combPalletPort.SelectedIndex = 0;
            }
            /*
            if (combPalletMethod.SelectedIndex == 0)
            {
                combPalletPort.SelectedIndex = -1;
                combPalletPort.Enabled = false;
            }
            else
            {
                combPalletPort.Enabled = true;
            }
             */
        }

        private void combPKAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            combPKActionIndex.SelectedIndex = combPKAction.SelectedIndex;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (combPKAction.SelectedIndex == -1)
            {
                SajetCommon.ShowMessage("Please Choose Packing Action");
                return;
            }
            if (combPKBase.SelectedIndex == -1)
            {
                SajetCommon.ShowMessage("Please Choose Packing Base");
                return;
            }

            var sSQL = "Delete SAJET.SYS_MODULE_PARAM "
                  + "Where MODULE_NAME = 'PACKING' "
                  + "and FUNCTION_NAME = 'Work Station Configuration'  "
                  + "and PARAME_NAME = '" + terminalId + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            //Customer SN                      
            if (combCSNType.SelectedIndex == 3)
                Save_DB("CSN", "Don''t Change");
            else
                Save_DB("CSN", g_sCSN_Type[combCSNType.SelectedIndex]);

            if (chkbPrintCSN.Checked)
                Save_DB("Print CSN Label", "Y");
            else
                Save_DB("Print CSN Label", "N");
            Save_DB("Print CSN Label Method", g_sPrintMethod[combCSNMethod.SelectedIndex]);
            Save_DB("Print CSN Label Qty", numCSNPrintQty.Value.ToString());
            Save_DB("Print CSN Label Port", combCSNPort.Text);
            //InnerBox
            Save_DB("InnerBox", g_sCreate_Type[combInnerType.SelectedIndex]);
            if (chkbPrintInner.Checked)
                Save_DB("Print InnerBox Label", "Y");
            else
                Save_DB("Print InnerBox Label", "N");
            Save_DB("Print InnerBox Label Method", g_sPrintMethod[combInnerMethod.SelectedIndex]);
            Save_DB("Print InnerBox Label Qty", numInnerPrintQty.Value.ToString());
            Save_DB("Print InnerBox Label Port", combInnerPort.Text);
            //Box
            Save_DB("Box", g_sCreate_Type[combBoxType.SelectedIndex]);
            if (chkbPrintBox.Checked)
                Save_DB("Print Box Label", "Y");
            else
                Save_DB("Print Box Label", "N");
            Save_DB("Print Box Label Method", g_sPrintMethod[combBoxMethod.SelectedIndex]);
            Save_DB("Print Box Label Qty", numBoxPrintQty.Value.ToString());
            Save_DB("Print Box Label Port", combBoxPort.Text);
            //Carton
            Save_DB("Carton", g_sCreate_Type[combCartonType.SelectedIndex]);
            if (chkbPrintCarton.Checked)
                Save_DB("Print Carton Label", "Y");
            else
                Save_DB("Print Carton Label", "N");
            Save_DB("Print Carton Label Method", g_sPrintMethod[combCartonMethod.SelectedIndex]);
            Save_DB("Print Carton Label Qty", numCartonPrintQty.Value.ToString());
            Save_DB("Print Carton Label Port", combCartonPort.Text);
            //Pallet
            Save_DB("Pallet", g_sCreate_Type[combPalletType.SelectedIndex]);
            if (chkbPrintPallet.Checked)
                Save_DB("Print Pallet Label", "Y");
            else
                Save_DB("Print Pallet Label", "N");
            Save_DB("Print Pallet Label Method", g_sPrintMethod[combPalletMethod.SelectedIndex]);
            Save_DB("Print Pallet Label Qty", numPalletPrintQty.Value.ToString());
            Save_DB("Print Pallet Label Port", combPalletPort.Text);

            //=================
            //包裝方式
            Save_DB("Packing Base", g_sPkBase[combPKBase.SelectedIndex]);
            //包裝動作
            Save_DB("Packing Action", combPKActionIndex.Text);
            //是否可輸入不良
            if (chkbInputEC.Checked)
                Save_DB("Input Error Code", "Y");
            else
                Save_DB("Input Error Code", "N");
            //Rule Function
            if (combRuleFun.SelectedIndex > 0)
                Save_DB("Check Rule by Function", combRuleFun.Text);
            //是否轉大寫
            if (chkbCapsLock.Checked)
                Save_DB("Caps Lock", "Y");
            else
                Save_DB("Caps Lock", "N");
            //有不良時是否清除Customer SN
            if (chkbRemoveShipSN.Checked)
                Save_DB("Remove Customer SN", "Y");
            else
                Save_DB("Remove Customer SN", "N");

            SajetCommon.ShowMessage("Settings completed", MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void ShowOption()
        {
            //Rule Function            
            combRuleFun.Items.Clear();
            var sSQL = "select owner || '.' || object_name object_name "
                  + "from ALL_OBJECTS "
                  + "where object_type = 'FUNCTION' "
                  + "and substr(object_name, 1, 3) = 'PK_'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combRuleFun.Items.Add(dsTemp.Tables[0].Rows[i]["object_name"].ToString());
            }

            //此Terminal的設定======================            
            combCSNType.SelectedIndex = 0;
            combCSNMethod.SelectedIndex = 2;
            combCSNPort.SelectedIndex = 0;
            numCSNPrintQty.Value = 1;
            chkbPrintCSN.Checked = false;

            combInnerType.SelectedIndex = 0;
            combInnerMethod.SelectedIndex = 2;
            combInnerPort.SelectedIndex = 0;
            numInnerPrintQty.Value = 1;
            chkbPrintInner.Checked = false;

            combBoxType.SelectedIndex = 0;
            combBoxMethod.SelectedIndex = 2;
            combBoxPort.SelectedIndex = 0;
            numBoxPrintQty.Value = 1;
            chkbPrintBox.Checked = false;

            combCartonType.SelectedIndex = 0;
            combCartonMethod.SelectedIndex = 2;
            combCartonPort.SelectedIndex = 0;
            numCartonPrintQty.Value = 1;
            chkbPrintCarton.Checked = false;

            combPalletType.SelectedIndex = 0;
            combPalletMethod.SelectedIndex = 2;
            combPalletPort.SelectedIndex = 0;
            numPalletPrintQty.Value = 1;
            chkbPrintPallet.Checked = false;

            chkbInputEC.Checked = false;
            chkbCapsLock.Checked = false;

            sSQL = "SELECT * FROM SAJET.SYS_MODULE_PARAM "
                 + " WHERE MODULE_NAME = 'PACKING' "
                 + " and FUNCTION_NAME = 'Work Station Configuration' "
                 + " and PARAME_NAME = '" + terminalId + "'"
                 + " order by PARAME_ITEM "; // add by rita(2010/09/15) 因為配合列印方式顯示列印PORT,所以列印方式必須先被執行
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                string sParamItem = dsTemp.Tables[0].Rows[i]["PARAME_ITEM"].ToString();
                string sParamValue = dsTemp.Tables[0].Rows[i]["PARAME_VALUE"].ToString();
                switch (sParamItem)
                {
                    //Customer SN
                    case "CSN":
                        combCSNType.SelectedIndex = Array.IndexOf(g_sCSN_Type, sParamValue);
                        continue;
                    case "Print CSN Label":
                        chkbPrintCSN.Checked = (sParamValue == "Y");
                        continue;
                    //case "Print CSN Label Method":
                    //    combCSNMethod.SelectedIndex = Array.IndexOf(g_sPrintMethod, sParamValue);
                    //    continue;
                    case "Print CSN Label Port":
                        combCSNPort.SelectedIndex = combCSNPort.Items.IndexOf(sParamValue);
                        continue;
                    case "Print CSN Label Qty":
                        numCSNPrintQty.Value = Convert.ToInt32(sParamValue);
                        continue;

                    //Inner Box
                    case "InnerBox":
                        combInnerType.SelectedIndex = Array.IndexOf(g_sCreate_Type, sParamValue);
                        continue;
                    case "Print InnerBox Label":
                        chkbPrintInner.Checked = (sParamValue == "Y");
                        continue;
                    //case "Print InnerBox Label Method":
                    //    combInnerMethod.SelectedIndex = Array.IndexOf(g_sPrintMethod, sParamValue);
                    //    continue;
                    case "Print InnerBox Label Port":
                        combInnerPort.SelectedIndex = combInnerPort.Items.IndexOf(sParamValue);
                        continue;
                    case "Print InnerBox Label Qty":
                        numInnerPrintQty.Value = Convert.ToInt32(sParamValue);
                        continue;
                    //Box
                    case "Box":
                        combBoxType.SelectedIndex = Array.IndexOf(g_sCreate_Type, sParamValue);
                        continue;
                    case "Print Box Label":
                        chkbPrintBox.Checked = (sParamValue == "Y");
                        continue;
                    //case "Print Box Label Method":
                    //    combBoxMethod.SelectedIndex = Array.IndexOf(g_sPrintMethod, sParamValue);
                    //    continue;
                    case "Print Box Label Port":
                        combBoxPort.SelectedIndex = combBoxPort.Items.IndexOf(sParamValue);
                        continue;
                    case "Print Box Label Qty":
                        numBoxPrintQty.Value = Convert.ToInt32(sParamValue);
                        continue;

                    //Caeton
                    case "Carton":
                        combCartonType.SelectedIndex = Array.IndexOf(g_sCreate_Type, sParamValue);
                        continue;
                    case "Print Carton Label":
                        chkbPrintCarton.Checked = (sParamValue == "Y");
                        continue;
                    //case "Print Carton Label Method":
                    //    combCartonMethod.SelectedIndex = Array.IndexOf(g_sPrintMethod, sParamValue);
                    //    continue;
                    case "Print Carton Label Port":
                        combCartonPort.SelectedIndex = combCartonPort.Items.IndexOf(sParamValue);
                        continue;
                    case "Print Carton Label Qty":
                        numCartonPrintQty.Value = Convert.ToInt32(sParamValue);
                        continue;

                    //Pallet
                    case "Pallet":
                        combPalletType.SelectedIndex = Array.IndexOf(g_sCreate_Type, sParamValue);
                        continue;
                    case "Print Pallet Label":
                        chkbPrintPallet.Checked = (sParamValue == "Y");
                        continue;
                    //case "Print Pallet Label Method":
                    //    combPalletMethod.SelectedIndex = Array.IndexOf(g_sPrintMethod, sParamValue);
                    //    continue;
                    case "Print Pallet Label Port":
                        combPalletPort.SelectedIndex = combPalletPort.Items.IndexOf(sParamValue);
                        continue;
                    case "Print Pallet Label Qty":
                        numPalletPrintQty.Value = Convert.ToInt32(sParamValue);
                        continue;

                    //
                    case "Packing Base":
                        combPKBase.SelectedIndex = Array.IndexOf(g_sPkBase, sParamValue);
                        continue;
                    case "Packing Action":
                        combPKActionIndex.SelectedIndex = combPKActionIndex.Items.IndexOf(sParamValue);
                        combPKAction.SelectedIndex = combPKActionIndex.SelectedIndex;
                        continue;
                    case "Input Error Code":
                        chkbInputEC.Checked = (sParamValue == "Y");
                        continue;
                    case "Check Rule by Function":
                        combRuleFun.SelectedIndex = combRuleFun.Items.IndexOf(sParamValue);
                        continue;
                    case "Caps Lock":
                        chkbCapsLock.Checked = (sParamValue == "Y");
                        continue;
                    case "Remove Customer SN":
                        chkbRemoveShipSN.Checked = (sParamValue == "Y");
                        continue;
                }
            }

            if (combPKAction.SelectedIndex == -1 && combPKAction.Items.Count > 0)
                combPKAction.SelectedIndex = 0;
        }

        private void Save_DB(string sParamItem, string sParamValue)
        {
            var sSQL = "Insert Into SAJET.SYS_MODULE_PARAM "
                  + "(MODULE_NAME,FUNCTION_NAME,PARAME_NAME,PARAME_ITEM,PARAME_VALUE,UPDATE_USERID ) "
                  + "Values "
                  + "('PACKING','Work Station Configuration','" + terminalId + "','" + sParamItem + "','" + sParamValue + "','" + userId + "') ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
        }
    }
}
