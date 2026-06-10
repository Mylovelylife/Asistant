using SajetClass;
using SajetFilter;
using System;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace PackingDll
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Application.ThreadException += (send, e) =>
            {
                //log.Warn("ThreadException: [" + ActDll.Act_PROGRAM + "][" + ActDll.Act_DLL_FILENAME + "]" + e.Exception.ToString());
                ClientUtils.Show_Error("PackingDll ThreadException", e.Exception);
            };
            AppDomain.CurrentDomain.UnhandledException += (send, e) =>
            {
                //log.Warn("UnhandledException: [" + ActDll.Act_PROGRAM + "][" + ActDll.Act_DLL_FILENAME + "]" + e.ExceptionObject.ToString());
                ClientUtils.Show_Error("PackingDll UnhandledException", (Exception)e.ExceptionObject);
            };


        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.Combine(path, args.Name.Split(',')[0]);
            path = string.Concat(path, ".dll");
            if (File.Exists(path))
                return Assembly.LoadFrom(path);
            return null;
        }
        /*==0:S/N->[Pallet],1:S/N->[Carton],2:C->[Pallet],3:S/N->[Carton]+QC
            4:QC->[Pallet],5:S/N->[Box],6:B->[Carton],7:B->[Pallet],8:B->[Carton]+QC,9:S/N->IB ==*/
        public int g_iPrivilege = 0;
        public int g_iClosePrivilege = 0;
        string g_sProgram;
        public static string g_sExeName;
        public static string g_sUserID;
        public static string g_sUserNo;
        public static string g_sTerminalID;
        public static string g_sProcessID;
        public static string g_sStageID;
        public static string g_sPDLineID;

        public static string g_sPartID;
        public static string g_sPartNo;
        public static string g_sSN = "";
        public static string g_sPallet = "N/A";
        public  string g_sCarton = "N/A";
        public string g_sCarton_New = "";


        public static string g_sInnerBox = "N/A";
        public static string g_sBox = "N/A";
        public static string g_sCSN = "N/A";
        public static string g_sPKSpec = "";
        public static string g_sPKSpecID = "";

        string g_sSNType = "N/A";
        string g_sOldPallet;
        string g_sOldCarton;
        string g_sOldInnerBox;
        string g_sOldBox;
        string g_sInnerQtyField;
        string g_sOtherWOUDI;

        //Option中的設定值===================        
        public struct TOptionSetup
        {
            public string sPKBase; //By Model or WO
            public int iPKAction;  //動作
            public bool bInputEC;  //是否可輸入不良
            public string sRuleFun;
            public bool bRemoveCSN;  //有不良是否清除CSN
        }
        public static TOptionSetup TSetup = new TOptionSetup();

        public struct TOptionData
        {
            //0:CSN,1:Box,2:Carton,3:Pallet,4:InnerBox            
            public bool g_bSysCreate;      //System Create   
            public bool g_bInputRealease;  //Input(Release) 
            public bool g_bPrint;          //是否要列印
            public string g_sPrintMethod;  //列印方式
            public string g_sPrintPort;    //列印Port
            public int g_iPrintQty;        //列印數量            
            public bool g_bNotChange;      //CSN不更改
            public bool g_bSameSN;         //CSN=SN
            public bool g_bCheckSameSN;    //Check CSN=SN
            public bool g_bInput;    //Input
        }
        public static TOptionData[] TOption = new TOptionData[5];
        //SYS_BASE中的預設值==============================
        public bool g_bAutoClose;
        public bool g_bPopUp;
        public bool g_bRefreshQty;
        public bool g_bCycle;
        public Color g_Color;

        string sSQL;
        DataSet dsTemp;

        public DialogResult Show_Message(string sText, int iType)
        {
            int ifreq = 800; int iduration = 200;
            if (g_bPopUp)
            {
                switch (iType)
                {
                    case 0: //Error
                        Console.Beep(ifreq, iduration);
                        return MessageBox.Show(sText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    case 1: //Warning
                        Console.Beep(ifreq, iduration);
                        return MessageBox.Show(sText, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    case 2: //Confirm
                        return MessageBox.Show(sText, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    case 3: //OK
                        TextMsg.Text = sText;
                        TextMsg.ForeColor = Color.Green;
                        TextMsg.BackColor = Color.White;
                        return DialogResult.None;
                    default:
                        return MessageBox.Show(sText, "", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }
            else
            {
                TextMsg.Text = sText;
                switch (iType)
                {
                    case 0: //Error                        
                        TextMsg.ForeColor = Color.Red;
                        TextMsg.BackColor = Color.Silver;
                        Console.Beep(ifreq, iduration);
                        return DialogResult.None;
                    case 1: //Warning                        
                        TextMsg.ForeColor = Color.Blue;
                        TextMsg.BackColor = Color.FromArgb(255, 255, 128);

                        Console.Beep(ifreq, iduration);
                        return DialogResult.None;
                    case 2: //Confirm
                        return MessageBox.Show(sText, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    default:
                        TextMsg.ForeColor = Color.Green;
                        TextMsg.BackColor = Color.White;
                        return DialogResult.None;
                }
            }
        }

        public void check_privilege()
        {
            btnSettings.Enabled = editWO.Enabled = false;
            g_iPrivilege = ClientUtils.GetPrivilege(g_sUserID, ClientUtils.fFunctionName, g_sProgram);

            //g_iPrivilege = ClientUtils.Get_Privilege(g_sUserID, g_sExeName, out g_sProgram);           
            btnSettings.Enabled = editWO.Enabled = (g_iPrivilege >= 1);

            //Close Pallet..
            g_iClosePrivilege = ClientUtils.GetPrivilege(g_sUserID, "Close Pallet(Carton)", g_sProgram);
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            g_Color = editWO.BackColor;
            panelWO.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panelWO.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            SajetCommon.SetLanguageControl(this);
            //ClientUtils.SetLanguage(this, g_sExeName);
            g_sProgram = ClientUtils.fProgramName;
            g_sExeName = ClientUtils.fCurrentProject;

            //Employee
            g_sUserID = ClientUtils.UserPara1;
            g_sUserNo = ClientUtils.fLoginUser;
            ClearData();
            check_privilege();

            panelWO.Enabled = false;
            /*
            gbPallet.Enabled = false;
            gbCarton.Enabled = false;
            gbBox.Enabled = false;
            gbSN.Enabled = false;
            PanelSNInput.Enabled = gbSN.Enabled;
             */

            //讀取本站Terminal
            if (!GetTerminalID())
            {
                return;
            }


            sSQL = $@"SELECT TERMINAL_NAME FROM SAJET.SYS_TERMINAL  WHERE TERMINAL_ID = '{g_sTerminalID}'";

            var ds1 = ClientUtils.ExecuteSQL(sSQL);

            if (ds1.Tables[0].Rows.Count > 0)
            {
                lb_Station.Text = ds1.Tables[0].Rows[0][0].ToString();
            }


            // 讀取Option設定值
            if (!GetOptionData())
            {
                return;
            }

            //讀取SYS_BASE設定值
            string sMsg = "";
            g_bAutoClose = (SajetCommon.GetSysBaseData(g_sProgram, "AUTOCLOSE", ref sMsg) == "Y"); //是否自動關閉棧板與箱號
            g_bPopUp = (SajetCommon.GetSysBaseData(g_sProgram, "MSG_POPUP", ref sMsg) == "Y");     //是否彈出錯誤訊息對話框
            g_bRefreshQty = (SajetCommon.GetSysBaseData(g_sProgram, "Refresh Qty", ref sMsg) == "Y"); //從DB重新計算數量
            g_bCycle = (SajetCommon.GetSysBaseData(g_sProgram, "Pack Spec Cycle", ref sMsg) == "Y");  //多個包裝方式時,是否循環使用            
            if (!string.IsNullOrEmpty(sMsg))
            {
                sMsg = SajetCommon.SetLanguage("Please Setup System Parameter", 1) + " : " + Environment.NewLine + Environment.NewLine + sMsg;
                Show_Message(sMsg, 0);
                return;
            }

            panelWO.Enabled = true;

            //    gbPallet.Enabled = true;
            //    gbCarton.Enabled = true;
            //    gbBox.Enabled = true;
            //   gbSN.Enabled = true;

            //  PanelSNInput.Enabled = gbSN.Enabled;

            SetEditFocus("WO");
            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";

        }

        public bool GetTerminalID()
        {
            g_sTerminalID = ClientUtils.GetValue("Terminal", "Terminal", null);

            if (string.IsNullOrEmpty(g_sTerminalID))
            {
                Show_Message(SajetCommon.SetLanguage("Terminal not be assign", 1), 0);
                return false;
            }

            sSQL = "Select A.TERMINAL_NAME,B.PROCESS_NAME "
                 + "      ,A.PDLINE_ID,A.Stage_ID,A.PROCESS_ID "
                 + " From SAJET.SYS_TERMINAL A "
                 + "     ,SAJET.SYS_PROCESS B "
                 + "Where A.TERMINAL_ID = '" + g_sTerminalID + "' "
                 + "AND A.PROCESS_ID = B.PROCESS_ID ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                Show_Message(SajetCommon.SetLanguage("Terminal data Error", 1), 0);
                return false;
            }
            g_sProcessID = dsTemp.Tables[0].Rows[0]["PROCESS_ID"].ToString();
            g_sStageID = dsTemp.Tables[0].Rows[0]["Stage_ID"].ToString();
            g_sPDLineID = dsTemp.Tables[0].Rows[0]["PDLINE_ID"].ToString();

            this.Text = this.Text + " ("
                      + dsTemp.Tables[0].Rows[0]["PROCESS_NAME"].ToString() + " / "
                      + dsTemp.Tables[0].Rows[0]["TERMINAL_NAME"].ToString() + ")";
            return true;
        }

        public bool GetOptionData()
        {
            sSQL = "SELECT * FROM SAJET.SYS_MODULE_PARAM "
                 + " WHERE MODULE_NAME = 'PACKING' "
                 + " and FUNCTION_NAME = 'Work Station Configuration' "
                 + " and PARAME_NAME = '" + g_sTerminalID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                Show_Message(SajetCommon.SetLanguage("Configuration not Exist", 1), 0);
                return false;
            }

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                string sParamItem = dsTemp.Tables[0].Rows[i]["PARAME_ITEM"].ToString();
                string sParamValue = dsTemp.Tables[0].Rows[i]["PARAME_VALUE"].ToString();
                switch (sParamItem)
                {
                    //Customer SN
                    case "CSN":
                        TOption[0].g_bSysCreate = (sParamValue == "System Create");
                        TOption[0].g_bInputRealease = (sParamValue == "Input (Released)");
                        TOption[0].g_bNotChange = (sParamValue == "Don't Change");
                        TOption[0].g_bSameSN = (sParamValue == "CSN=SN");
                        TOption[0].g_bCheckSameSN = (sParamValue == "CSN=SN (Check)");
                        TOption[0].g_bInput = (sParamValue == "Input");
                        continue;
                    case "Print CSN Label":
                        TOption[0].g_bPrint = (sParamValue == "Y");
                        continue;
                    case "Print CSN Label Method":
                        TOption[0].g_sPrintMethod = sParamValue;
                        continue;
                    case "Print CSN Label Port":
                        TOption[0].g_sPrintPort = sParamValue;
                        continue;
                    case "Print CSN Label Qty":
                        TOption[0].g_iPrintQty = Convert.ToInt32(sParamValue);
                        continue;
                    //InnerBox
                    case "InnerBox":
                        TOption[4].g_bSysCreate = (sParamValue == "System Create");
                        TOption[4].g_bInputRealease = (sParamValue == "Input (Released)");
                        continue;
                    case "Print InnerBox Label":
                        TOption[4].g_bPrint = (sParamValue == "Y");
                        continue;
                    case "Print InnerBox Label Method":
                        TOption[4].g_sPrintMethod = sParamValue;
                        continue;
                    case "Print InnerBox Label Port":
                        TOption[4].g_sPrintPort = sParamValue;
                        continue;
                    case "Print InnerBox Label Qty":
                        TOption[4].g_iPrintQty = Convert.ToInt32(sParamValue);
                        continue;
                    //Box
                    case "Box":
                        TOption[1].g_bSysCreate = (sParamValue == "System Create");
                        TOption[1].g_bInputRealease = (sParamValue == "Input (Released)");
                        continue;
                    case "Print Box Label":
                        TOption[1].g_bPrint = (sParamValue == "Y");
                        continue;
                    case "Print Box Label Method":
                        TOption[1].g_sPrintMethod = sParamValue;
                        continue;
                    case "Print Box Label Port":
                        TOption[1].g_sPrintPort = sParamValue;
                        continue;
                    case "Print Box Label Qty":
                        TOption[1].g_iPrintQty = Convert.ToInt32(sParamValue);
                        continue;

                    //Carton
                    case "Carton":
                        TOption[2].g_bSysCreate = (sParamValue == "System Create");
                        TOption[2].g_bInputRealease = (sParamValue == "Input (Released)");
                        continue;
                    case "Print Carton Label":
                        TOption[2].g_bPrint = (sParamValue == "Y");
                        continue;
                    case "Print Carton Label Method":
                        TOption[2].g_sPrintMethod = sParamValue;
                        continue;
                    case "Print Carton Label Port":
                        TOption[2].g_sPrintPort = sParamValue;
                        continue;
                    case "Print Carton Label Qty":
                        TOption[2].g_iPrintQty = Convert.ToInt32(sParamValue);
                        continue;

                    //Pallet
                    case "Pallet":
                        TOption[3].g_bSysCreate = (sParamValue == "System Create");
                        TOption[3].g_bInputRealease = (sParamValue == "Input (Released)");
                        continue;
                    case "Print Pallet Label":
                        TOption[3].g_bPrint = (sParamValue == "Y");
                        continue;
                    case "Print Pallet Label Method":
                        TOption[3].g_sPrintMethod = sParamValue;
                        continue;
                    case "Print Pallet Label Port":
                        TOption[3].g_sPrintPort = sParamValue;
                        continue;
                    case "Print Pallet Label Qty":
                        TOption[3].g_iPrintQty = Convert.ToInt32(sParamValue);
                        continue;

                    //
                    case "Packing Base":
                        TSetup.sPKBase = sParamValue;
                        LabPKBase.Text = SajetCommon.SetLanguage("By " + sParamValue, 1);
                        continue;
                    case "Packing Action":
                        TSetup.iPKAction = Convert.ToInt32(sParamValue);

                        //Packing Action                                            
                        LabPKAction.Text = "";
                        string sSQL1 = "select param_value from sajet.sys_base "
                                     + "where param_name = 'Packing Action' ";
                        DataSet dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);
                        if (dsTemp1.Tables[0].Rows.Count > 0)
                        {
                            string sValue = dsTemp1.Tables[0].Rows[0]["param_value"].ToString().TrimEnd(new Char[] { ',' });
                            string[] sAction = sValue.Split(new Char[] { ',' });
                            for (int j = 0; j <= sAction.Length - 1; j++)
                            {
                                if (sAction[j].ToString().Substring(0, 1) == sParamValue)
                                {
                                    LabPKAction.Text = sAction[j].ToString().Substring(1) + " ";
                                    break;
                                }
                            }
                        }
                        continue;
                    case "Input Error Code":
                        TSetup.bInputEC = (sParamValue == "Y");
                        LVEC.Enabled = TSetup.bInputEC;
                        // if (!LVEC.Enabled)
                        //     PanelSNInput.Dock = DockStyle.Fill;
                        continue;
                    case "Check Rule by Function":
                        TSetup.sRuleFun = sParamValue;
                        continue;
                    case "Caps Lock":
                        if (sParamValue == "Y")
                        {
                            editWO.CharacterCasing = CharacterCasing.Upper;
                            editPallet.CharacterCasing = CharacterCasing.Upper;
                            editCarton.CharacterCasing = CharacterCasing.Upper;
                            editBox.CharacterCasing = CharacterCasing.Upper;
                            editSN.CharacterCasing = CharacterCasing.Upper;
                            editCSN.CharacterCasing = CharacterCasing.Upper;
                        }
                        continue;
                    case "Remove Customer SN":
                        TSetup.bRemoveCSN = (sParamValue == "Y");
                        continue;
                }
            }
            //根據Packing Action顯示元件
            Show_PKAction();
            if (TSetup.iPKAction == 9)
            {
                gbBox.Text = "Inner Box";
                lablBox.Text = "Inner Box";
                btnCloseBox.Text = "Close Inner Box";
            }

            if (TSetup.iPKAction == 9)
            {
                //找工單INNERBOX的包裝規格
                sSQL = "SELECT SQL_NAME FROM SAJET.SYS_SQL "
                     //+ "WHERE SYSUSE_NAME = 'Inner Box Qty' AND ROWNUM = 1";
                     + " WHERE SYSUSE_NAME = 'Wo Inner Box Qty' AND ROWNUM = 1";

                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("WO Inner Box Qty Field not Found"), 0);
                    return false;
                }
                g_sInnerQtyField = dsTemp.Tables[0].Rows[0]["SQL_NAME"].ToString();
            }
            return true;
        }

        public void Show_PKAction()
        {
            //先全部設為False
            gbPallet.Enabled = false;
            gbCarton.Enabled = false;
            gbBox.Enabled = false;
            gbSN.Enabled = false;
            PanelSNInput.Enabled = false;

            if (TSetup.iPKAction == 2 || TSetup.iPKAction == 4)
            {
                LabCartonTle.Visible = false;
                LabCartonCap.Visible = false;
                LabCartonCapacity.Visible = false;
                LabCartonQty.Visible = false;
                btnCloseCarton.Visible = false;
            }
            //SN.BOX
            if (TSetup.iPKAction == 0 || TSetup.iPKAction == 1 || TSetup.iPKAction == 3 || TSetup.iPKAction == 5 || TSetup.iPKAction == 9)
            {
                gbSN.Enabled = true;
                gbBox.Enabled = true;
                PanelSNInput.Enabled = true;
            }
            //BOX 
            if (TSetup.iPKAction >= 6 && TSetup.iPKAction <= 8)
            {
                gbBox.Enabled = true;
                LabBoxTle.Visible = false;
                LabBoxCap.Visible = false;
                LabBoxCapacity.Visible = false;
                LabBoxQty.Visible = false;
                btnCloseBox.Visible = false;
            }
            if (TSetup.iPKAction != 5 && TSetup.iPKAction != 9)
            {
                gbCarton.Enabled = true;
            }
            if (TSetup.iPKAction == 0 || TSetup.iPKAction == 2 || TSetup.iPKAction == 4 || TSetup.iPKAction == 7)
            {
                gbPallet.Enabled = true;
            }




        }

        public void SetEditFocus(string sKind)
        {
            btnSettings.Enabled = editWO.Enabled = false;
            editWO.BackColor = Color.White;
            editSN.Enabled = false;
            editSN.BackColor = Color.White;
            editCSN.Enabled = false;
            editCSN.BackColor = Color.White;
            editPallet.Enabled = false;
            editPallet.BackColor = Color.White;
            editCarton.Enabled = false;
            editCarton.BackColor = Color.White;
            editBox.Enabled = false;
            editBox.BackColor = Color.White;

            switch (sKind)
            {
                case "WO":
                    btnSettings.Enabled = editWO.Enabled = true;
                    editWO.BackColor = g_Color;
                    editWO.Focus();
                    editWO.SelectAll();
                    break;
                case "SN":
                    editSN.Enabled = true;
                    editSN.BackColor = g_Color;
                    editSN.Focus();
                    editSN.SelectAll();
                    break;
                case "CSN":
                    editCSN.Enabled = true;
                    editCSN.BackColor = g_Color;
                    editCSN.Focus();
                    editCSN.SelectAll();
                    break;
                case "PALLET":
                    editPallet.Enabled = true;
                    editPallet.BackColor = g_Color;
                    editPallet.Focus();
                    editPallet.SelectAll();
                    break;
                case "CARTON":
                    editCarton.Enabled = true;
                    editCarton.BackColor = g_Color;
                    editCarton.Focus();
                    editCarton.SelectAll();
                    break;
                case "BOX":
                    editBox.Enabled = true;
                    editBox.BackColor = g_Color;
                    editBox.Focus();
                    editBox.SelectAll();
                    break;
                default:
                    break;
            }
        }

        public void ClearData()
        {
            LabPart.Text = "";
            LabWo.Text = "";
            editSN.Text = "";
            editCSN.Text = "";
            editWO.Text = "";
            LabWoVersion.Text = "";
            LabPartDesc.Text = "";
            LabTargetQty.Text = "";

            editBox.Text = "";
            LabBoxCapacity.Text = "0";
            LabBoxQty.Text = "0";
            editCarton.Text = "";
            LabCartonCapacity.Text = "0";
            LabCartonQty.Text = "0";
            editPallet.Text = "";
            LabPalletCapacity.Text = "0";
            LabPalletQty.Text = "0";

            LVEC.Items.Clear();
            LVPackSpec.Items.Clear();

            TextMsg.Text = "";
            TextMsg.BackColor = Color.White;
        }

        private void btnSearchWO_Click(object sender, EventArgs e)
        {
            sSQL = "Select WORK_ORDER "
                 + "From SAJET.G_WO_BASE "
                 + "Where WORK_ORDER Like '" + editWO.Text + "%' "
                 + "and WO_STATUS in ('2','3') "
                 + "Order By WORK_ORDER ";
            fFilter f = new fFilter();
            //     f.sServerName = SajetCommon.g_sServerName;
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editWO.Text = f.dgvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editWO_KeyPress(sender, Key);
            }
        }

        private void editWO_KeyPress(object sender, KeyPressEventArgs e)
        {
            lb_process.Text = "";

            TextMsg.Text = "";
            TextMsg.BackColor = Color.White;

            if (e.KeyChar != (char)Keys.Return)
                return;

            //可輸入SN or CSN代替輸入WO
            sSQL = "Select Work_Order "
                 + "from sajet.g_sn_status "
                 + "where serial_number = '" + editWO.Text + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                sSQL = "Select Work_Order "
                     + "from sajet.g_sn_status "
                     + "where customer_sn = '" + editWO.Text + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    editWO.Text = dsTemp.Tables[0].Rows[0]["Work_Order"].ToString();
                }
            }
            else
            {
                editWO.Text = dsTemp.Tables[0].Rows[0]["Work_Order"].ToString();
            }

            //MessageBox.Show(g_sTerminalID); //10000064

            //====SAJET.SJ_CHK_WO_INPUT=====                 
            try
            {
                if (!string.IsNullOrEmpty(g_sTerminalID))
                {
                    


                    sSQL = $@"SELECT * FROM SAJET.SYS_ROUTE_DETAIL WHERE ROUTE_ID =
                    (
                    SELECT ROUTE_ID FROM SAJET.G_WO_BASE WHERE WORK_ORDER = '{editWO.Text}'
                    ) AND PROCESS_ID = (SELECT PROCESS_ID FROM SAJET.SYS_TERMINAL WHERE TERMINAL_ID = {g_sTerminalID})
                    ";

                    var _ds = ClientUtils.ExecuteSQL(sSQL);

                    if (_ds.Tables[0].Rows.Count == 0)
                    {
                       
                        lb_process.Text = "工單途程設定站點不符";

                        return;
                    }
                }
                else 
                {
                    lb_process.Text = "站點未設定";
                    return;
                }
                


                object[][] Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREV", editWO.Text };
                Params[1] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_CHK_WO_INPUT", Params);

                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    Show_Message(SajetCommon.SetLanguage(sRes, 1), 0);
                    editWO.SelectAll();
                    return;
                }
            }
            catch (Exception ex)
            {
                Clipboard.SetText(ex.Message);

                //Show_Message("SAJET.SJ_CHK_WO_INPUT" + Environment.NewLine + ex.Message, 0);

                Show_Message(ex.Message, 0);

                editWO.SelectAll();

                return;
            }
            //================================

            //WO
            sSQL = "SELECT A.*, B.PART_NO, B.SPEC1 "
                 + "FROM SAJET.G_WO_BASE A, SAJET.SYS_PART B "
                 + "WHERE  A.WORK_ORDER = '" + editWO.Text + "' "
                 + "AND A.PART_ID = B.PART_ID(+) and rownum = 1";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            g_sPartID = dsTemp.Tables[0].Rows[0]["PART_ID"].ToString();
            g_sPartNo = dsTemp.Tables[0].Rows[0]["PART_NO"].ToString();
            LabPart.Text = dsTemp.Tables[0].Rows[0]["PART_NO"].ToString();
            LabWo.Text = editWO.Text;
            LabTargetQty.Text = dsTemp.Tables[0].Rows[0]["TARGET_QTY"].ToString();
            LabWoVersion.Text = dsTemp.Tables[0].Rows[0]["VERSION"].ToString();
            LabPartDesc.Text = dsTemp.Tables[0].Rows[0]["SPEC1"].ToString();

            //將資料先紀錄下來,供編碼規則中的Function使用
            listField.Items.Clear();
            listValue.Items.Clear();
            for (int i = 0; i <= dsTemp.Tables[0].Columns.Count - 1; i++)
            {
                string sColumnName = dsTemp.Tables[0].Columns[i].ToString();
                if (listField.Items.IndexOf(sColumnName) == -1)
                {
                    listField.Items.Add(sColumnName);
                    listValue.Items.Add(dsTemp.Tables[0].Rows[0][sColumnName].ToString());
                }
            }

            //檢查各號碼規則-若Rule為No Seq,不可選擇SystemCreate方式
            if (!Check_RuleSeqType())
            {
                return;
            }

            //WO有定義的所有包裝方式
            if (!GetPackSpec())
            {
                return;
            }
            GetLastPackSpec();  //WO在此站最後一次使用的包裝方式

            //最大容器為Box
            if (TSetup.iPKAction == 5 || TSetup.iPKAction == 9)
            {
                Show_Box();
            }
            //最大容器為Carton
            else if (TSetup.iPKAction == 1 || TSetup.iPKAction == 3
                    || TSetup.iPKAction == 6 || TSetup.iPKAction == 8)
            {
                Show_Carton();
            }
            else
            //最大容器為Pallet
            {
                Show_Pallet();
            }

            if (editSN.Text == "")
                Show_Message(SajetCommon.SetLanguage("Work Order OK", 1), 3);
        }

        private bool Check_RuleSeqType()
        {
            //檢查各號碼規則-若Rule為No Seq,不可選擇SystemCreate方式
            sSQL = "select * from sajet.g_wo_param "
                 + "where work_order = '" + editWO.Text + "'"
                 + "and parame_name = 'Sequence Mode' "
                 + "and parame_value = 'Manual' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                string sRuleType = dsTemp.Tables[0].Rows[i]["MODULE_NAME"].ToString();
                switch (sRuleType)
                {
                    case "CUSTOMER SN RULE":
                        if (TOption[0].g_bSysCreate)
                        {
                            string sMsg = SajetCommon.SetLanguage("Can't Use System Create") + Environment.NewLine
                                        + SajetCommon.SetLanguage("Customer SN");
                            Show_Message(sMsg, 0);
                            return false;
                        }
                        break;
                    case "INNERBOX NO RULE":
                        if (TOption[4].g_bSysCreate)
                        {
                            string sMsg = SajetCommon.SetLanguage("Can't Use System Create") + Environment.NewLine
                                        + SajetCommon.SetLanguage("Inner Box");
                            Show_Message(sMsg, 0);
                            return false;
                        }
                        break;
                    case "BOX NO RULE":
                        if (TOption[1].g_bSysCreate)
                        {
                            string sMsg = SajetCommon.SetLanguage("Can't Use System Create") + Environment.NewLine
                                        + SajetCommon.SetLanguage("Box No");
                            Show_Message(sMsg, 0);
                            return false;
                        }
                        break;
                    case "CARTON NO RULE":
                        if (TOption[2].g_bSysCreate)
                        {
                            string sMsg = SajetCommon.SetLanguage("Can't Use System Create") + Environment.NewLine
                                        + SajetCommon.SetLanguage("Carton No");
                            Show_Message(sMsg, 0);
                            return false;
                        }
                        break;
                    case "PALLET NO RULE":
                        if (TOption[3].g_bSysCreate)
                        {
                            string sMsg = SajetCommon.SetLanguage("Can't Use System Create") + Environment.NewLine
                                        + SajetCommon.SetLanguage("Pallet No");
                            Show_Message(sMsg, 0);
                            return false;
                        }
                        break;
                }
            }
            return true;
        }

        public bool GetPackSpec()
        {
            //工單內有定義的包裝方式
            string sInnerQty = "0";
            //sSQL = "SELECT nvl(" + g_sInnerQtyField + ",0) FROM SAJET.SYS_PART "
            //    + "WHERE PART_ID = " + g_sPartID + " AND ROWNUM = 1";
            if (TSetup.iPKAction == 9)
            {
                sSQL = "SELECT nvl(" + g_sInnerQtyField + ",0) FROM SAJET.G_WO_BASE "
                                 + "WHERE WORK_ORDER = '" + editWO.Text + "' AND ROWNUM = 1";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                sInnerQty = dsTemp.Tables[0].Rows[0][0].ToString();
                if (sInnerQty == "0")
                {
                    Show_Message(SajetCommon.SetLanguage("No Packing Spec", 1), 0);
                    return false;
                }
            }
            sSQL = "SELECT a.PKSPEC_ID, a.PALLET_CAPACITY, a.CARTON_CAPACITY, a.BOX_CAPACITY, b.PKSPEC_NAME "
                    + "FROM SAJET.G_PACK_SPEC a "
                    + "    ,SAJET.SYS_PKSPEC b "
                    + "WHERE a.WORK_ORDER = '" + editWO.Text + "' "
                    + "AND a.PKSPEC_ID = b.PKSPEC_ID "
                    + "Order By a.Sequence ";

            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                Show_Message(SajetCommon.SetLanguage("No Packing Spec", 1), 0);
                return false;
            }

            LVPackSpec.Items.Clear();
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                LVPackSpec.Items.Add(dsTemp.Tables[0].Rows[i]["PKSPEC_NAME"].ToString());
                LVPackSpec.Items[LVPackSpec.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["BOX_CAPACITY"].ToString());
                LVPackSpec.Items[LVPackSpec.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["CARTON_CAPACITY"].ToString());
                LVPackSpec.Items[LVPackSpec.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["PALLET_CAPACITY"].ToString());
                LVPackSpec.Items[LVPackSpec.Items.Count - 1].SubItems.Add(sInnerQty);
                LVPackSpec.Items[LVPackSpec.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["PKSPEC_ID"].ToString());
                LVPackSpec.Items[LVPackSpec.Items.Count - 1].ImageIndex = 1;
            }
            return true;
        }

        public void GetLastPackSpec()
        {
            //此站最近一次使用的包裝方式
            sSQL = "SELECT b.PKSPEC_NAME "
                 + "FROM SAJET.G_PACK_SPEC_TERMINAL a "
                 + "    ,SAJET.SYS_PKSPEC b ";
            if (TSetup.sPKBase == "Work Order")
                sSQL = sSQL + " WHERE WORK_ORDER = '" + editWO.Text + "' ";
            else
                sSQL = sSQL + " WHERE PART_ID = '" + g_sPartID + "' ";
            sSQL = sSQL + " AND TERMINAL_ID = '" + g_sTerminalID + "'"
                        + " AND a.PKSPEC_ID = b.PKSPEC_ID "
                        + " and rownum = 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sPkSpec = dsTemp.Tables[0].Rows[0]["PKSPEC_NAME"].ToString();
                GetPack(sPkSpec, "N");
            }
            else
            {
                GetPack("", "N");
            }
            Update_PackTerminal();
        }

        public void GetPack(string sSpecName, string sCloseFlag)
        {
            //找目前應該使用的包裝方式=====
            int iIndex = LVPackSpec.Items.IndexOf(LVPackSpec.FindItemWithText(sSpecName, false, 0));
            //若pallet已滿,此次就用下個包裝方式
            if ((g_bCycle) || (sCloseFlag == "Y"))
                iIndex = iIndex + 1;
            if ((iIndex == -1) || (iIndex > LVPackSpec.Items.Count - 1))
                iIndex = 0;

            //將目前應使用的包裝方式勾起
            for (int i = 0; i <= LVPackSpec.Items.Count - 1; i++)
            {
                LVPackSpec.Items[i].ImageIndex = -1;
            }
            LVPackSpec.Items[iIndex].ImageIndex = 0;

            g_sPKSpec = LVPackSpec.Items[iIndex].Text;
            if (TSetup.iPKAction == 9)
                LabBoxCapacity.Text = LVPackSpec.Items[iIndex].SubItems[4].Text;
            else
                LabBoxCapacity.Text = LVPackSpec.Items[iIndex].SubItems[1].Text;
            LabCartonCapacity.Text = LVPackSpec.Items[iIndex].SubItems[2].Text;
            LabPalletCapacity.Text = LVPackSpec.Items[iIndex].SubItems[3].Text;
            g_sPKSpecID = LVPackSpec.Items[iIndex].SubItems[5].Text;

            //若Box Capacity=0,不顯示Box欄位            
            if (TSetup.iPKAction != 2 && TSetup.iPKAction != 4)
            {
                gbBox.Enabled = (LabBoxCapacity.Text != "0");
                if (TSetup.iPKAction < 6 && TSetup.iPKAction > 8)
                {
                    //Box不是最小單位                         
                    btnCloseBox.Enabled = gbBox.Enabled;
                    LabBoxTle.Enabled = gbBox.Enabled;
                    LabBoxCapacity.Enabled = gbBox.Enabled;
                    LabBoxCap.Enabled = gbBox.Enabled;
                    LabBoxQty.Enabled = gbBox.Enabled;
                    /*
                    btnCloseBox.Enabled = gbBox.Enabled;
                    LabBoxTle.Enabled = gbBox.Enabled;
                    LabBoxCapacity.Enabled = gbBox.Enabled;
                    LabBoxCap.Enabled = gbBox.Enabled;
                    LabBoxQty.Enabled = gbBox.Enabled;
                     */

                }
                else
                {
                    gbBox.Enabled = (LabBoxCapacity.Text != "0");
                }
            }
        }

        //===Pallet==============================================
        public void Show_Pallet()
        {
            //===處理Pallet===        
            //有未滿的棧板
            if (Get_UnfinishPallet())
            {
                Get_PackPalletQty();  //棧板數量
                if (TSetup.iPKAction == 2 || TSetup.iPKAction == 4)
                {
                    SetEditFocus("CARTON");
                }
                else
                {
                    Show_Carton();
                }
            }
            else //無未滿的棧板
            {
                //是否自動產生棧板號
                if (TOption[3].g_bSysCreate)
                {
                    if (Create_NewPallet()) //產生Pallet號碼
                    {
                        if (TSetup.iPKAction == 2 || TSetup.iPKAction == 4)
                        {
                            SetEditFocus("CARTON");
                        }
                        else
                        {
                            //Carton為SystemCreate
                            if (TOption[2].g_bSysCreate)
                            {
                                if (Create_NewCarton())
                                {
                                    //用BOX包Carton
                                    if (TSetup.iPKAction >= 6 && TSetup.iPKAction <= 8)
                                    {
                                        SetEditFocus("BOX");
                                    }
                                    else
                                    {
                                        Show_Box();
                                    }
                                }
                                else
                                {
                                    SetEditFocus("CARTON");
                                }
                            }
                            else
                            {
                                SetEditFocus("CARTON");
                            }
                        }
                    }
                    else
                    {
                        SetEditFocus("PALLET");
                    }
                }
                else
                {
                    SetEditFocus("PALLET");
                }
            }
        }
        public bool Get_UnfinishPallet()
        {
            g_sPallet = "";
            sSQL = "SELECT PALLET_NO FROM SAJET.G_PACK_PALLET ";
            if (TSetup.sPKBase == "Work Order")
                sSQL = sSQL + " Where WORK_ORDER = '" + editWO.Text + "' ";
            else
                sSQL = sSQL + " Where PART_ID = '" + g_sPartID + "' ";
            sSQL = sSQL + " AND TERMINAL_ID = '" + g_sTerminalID + "' "
                        + " AND CLOSE_FLAG = 'N' "
                        + " AND PKSPEC_ID = '" + g_sPKSpecID + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (dsTemp.Tables[0].Rows.Count == 1)
                    g_sPallet = dsTemp.Tables[0].Rows[0]["PALLET_NO"].ToString();
                else
                    g_sPallet = Show_UnFinishForm("Pallet_No"); //有多筆尚未Close的Pallet,跳出Form供User選擇

                if (!string.IsNullOrEmpty(g_sPallet))
                {
                    editPallet.Text = g_sPallet;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public void Get_PackPalletQty()
        {
            sSQL = " SELECT A.CARTON_NO "
                 + " FROM SAJET.G_SN_STATUS A "
                 + "     ,SAJET.G_PACK_CARTON B "
                 + " WHERE A.PALLET_NO = '" + g_sPallet + "' "
                 + " AND A.CARTON_NO = B.CARTON_NO "
                 + " AND B.CLOSE_FLAG = 'Y' "
                 + " GROUP BY A.CARTON_NO ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            LabPalletQty.Text = dsTemp.Tables[0].Rows.Count.ToString();
        }
        public bool Create_NewPallet()
        {
            g_sPallet = "";
            if (!Get_NewNo("Pallet", out g_sPallet))
                return false;

            g_sPallet = Get_NextNewNo("Pallet", g_sPallet);  //檢查Pallet是否已重複,並繼續找下個號碼
            editPallet.Text = g_sPallet;
            LabPalletQty.Text = "0";
            Append_PackNo("Pallet", g_sPallet);
            return true;
        }
        private void editPallet_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editPallet.Text = editPallet.Text.Trim();
            if (editPallet.Text == "")
            {
                string sMsg = SajetCommon.SetLanguage("Data is null") + Environment.NewLine
                            + LabPallet.Text;
                Show_Message(sMsg, 0);
                SetEditFocus("PALLET");
                return;
            }

            if (TOption[3].g_bInputRealease)
            {
                //檢查是否已由BarcodeCenter展開
                if (!Check_ReleaseNo("Pallet No", editPallet.Text))
                {
                    SetEditFocus("PALLET");
                    return;
                }
            }
            else
            {
                //檢查是否符合編碼規則
                if (!Check_Rule("Pallet No", editPallet.Text))
                {
                    SetEditFocus("PALLET");
                    return;
                }
            }

            //檢查是否重複
            if (!Check_Dup("PALLET", editPallet.Text))
            {
                SetEditFocus("PALLET");
                return;
            }
            //檢查UDI           
            if (!Check_UDI("PALLET", editPallet.Text, TOption[3].g_bSysCreate, TOption[2].g_bSysCreate, TOption[1].g_bSysCreate))
            {
                SetEditFocus("PALLET");
                return;
            }
            g_sPallet = editPallet.Text;

            //移至下一個                        
            if (TSetup.iPKAction != 2 && TSetup.iPKAction != 4)
            {
                if (TOption[2].g_bSysCreate)
                {
                    if (Create_NewCarton())
                    {
                        if (TSetup.iPKAction < 6 || TSetup.iPKAction > 8)
                            Show_Box();
                    }
                    else
                    {
                        SetEditFocus("CARTON");
                    }
                }
                else
                {
                    SetEditFocus("CARTON");
                }
            }
            else
                SetEditFocus("CARTON");

            Show_Message(SajetCommon.SetLanguage("Pallet OK", 1), 3);
        }
        private void btnClosePallet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(editPallet.Text))
                return;
            if (Show_Message(SajetCommon.SetLanguage("Close Pallet", 1) + " ?" + Environment.NewLine + editPallet.Text, 2) != DialogResult.Yes)
                return;

            //有權限才可做Close
            if (g_iClosePrivilege < 1)
            {
                if (!Check_Privilege_Close())
                    return;
            }
            Close_Pallet(editPallet.Text, "Y");
        }
        private void Close_Pallet(string sPallet, string sForceClose)
        {
            if (string.IsNullOrEmpty(sPallet))
                return;

            if (g_bRefreshQty)
            {
                if (!Refresh_PalletQty(g_sPallet))
                    return;
            }

            //先Close Carton
            if (editCarton.Text != "")
            {
                if (TSetup.iPKAction != 2 && TSetup.iPKAction != 4)
                {
                    Close_Carton(editCarton.Text, sForceClose);
                }
            }

            //Close Pallet
            sSQL = " Select serial_number FROM SAJET.G_SN_STATUS "
                 + " WHERE PALLET_NO = '" + sPallet + "' "
                 + " and ROWNUM = 1";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                sSQL = "SELECT PALLET_NO FROM SAJET.G_PACK_PALLET "
                     + "WHERE PALLET_NO = '" + sPallet + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count == 0)
                    Show_Message(SajetCommon.SetLanguage("No Pallet", 1) + " [" + sPallet + "] !", 3);
                else
                {
                    //Pallet中無序號,直接刪除此Pallet
                    sSQL = "DELETE SAJET.G_PACK_PALLET "
                         + "WHERE PALLET_NO = '" + sPallet + "' ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    Show_Message(SajetCommon.SetLanguage("Delete Pallet", 1) + " [" + sPallet + "] !", 3);
                }
            }
            else
            {
                sSQL = " UPDATE SAJET.G_PACK_PALLET "
                     + " SET CLOSE_FLAG = 'Y' "
                     + " WHERE PALLET_NO = '" + sPallet + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                //手動強制Close
                if (sForceClose == "Y")
                {
                    sSQL = " INSERT INTO SAJET.G_PACK_FORCECLOSE "
                         + " (PACK_NO, PACK_TYPE, EMP_ID,UPDATE_TIME) "
                         + " VALUES "
                         + " ('" + sPallet + "', 'Pallet', '" + g_sUserID + "',SYSDATE)";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                }
                Show_Message(SajetCommon.SetLanguage("Close Pallet", 1) + " [" + sPallet + "] !", 3);

                //列印Pallet
                if (TOption[3].g_bPrint)
                {
                    Print_Label(3, sPallet);
                }
            }
            g_sPallet = "";
            editPallet.Text = "";
            LabPalletQty.Text = "0";

            if (g_bCycle)
            {
                GetPack(g_sPKSpec, "Y");
                Update_PackTerminal();
            }
            Show_Pallet();
        }

        //===Carton=================================================
        public void Show_Carton()
        {
            //===處理Carton===                        
            //有未滿的Carton
            if (Get_UnfinishCarton())
            {
                Get_PackCartonQty();
                if (gbBox.Enabled)
                {
                    //用BOX包Carton
                    if (TSetup.iPKAction >= 6 && TSetup.iPKAction <= 8)
                    {
                        SetEditFocus("BOX");
                    }
                    else
                    {
                        Show_Box();
                    }
                }
                else
                {
                    SetEditFocus("SN");
                }
            }
            else //無未滿的Carton
            {
                //Carton為SystemCreate
                if (TOption[2].g_bSysCreate)
                {
                    if (Create_NewCarton())
                    {
                        //用BOX包Carton
                        if (TSetup.iPKAction >= 6 && TSetup.iPKAction <= 8)
                        {
                            SetEditFocus("BOX");
                        }
                        else
                        {
                            Show_Box();
                        }
                    }
                    else
                    {
                        SetEditFocus("CARTON");
                    }
                }
                else
                {
                    SetEditFocus("CARTON");
                }
            }
        }
        public bool Get_UnfinishCarton()
        {
            //找之前尚未Close的Box
            g_sCarton = "";
            sSQL = "SELECT CARTON_NO FROM SAJET.G_PACK_CARTON ";
            if (TSetup.sPKBase == "Work Order")
                sSQL = sSQL + " Where WORK_ORDER = '" + editWO.Text + "' ";
            else
                sSQL = sSQL + " Where PART_ID = '" + g_sPartID + "' ";
            sSQL = sSQL + " AND TERMINAL_ID = '" + g_sTerminalID + "' "
                        + " AND CLOSE_FLAG = 'N' "
                        + " AND PKSPEC_ID = '" + g_sPKSpecID + "' "
                        + " order by carton_no ";//add order by carton by jamey 20181025
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (dsTemp.Tables[0].Rows.Count == 1)
                    g_sCarton = dsTemp.Tables[0].Rows[0]["CARTON_NO"].ToString();
                else
                    g_sCarton = Show_UnFinishForm("Carton_No"); //有多筆尚未Close的Carton,跳出Form供User選擇

                if (!string.IsNullOrEmpty(g_sCarton))
                {
                    editCarton.Text = g_sCarton;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public void Get_PackCartonQty()
        {
            //Carton內的數量
            if (gbBox.Enabled)
            {
                sSQL = " SELECT A.BOX_NO "
                     + " FROM SAJET.G_SN_STATUS A "
                     + "     ,SAJET.G_PACK_BOX B "
                     + " WHERE A.CARTON_NO = '" + g_sCarton + "' "
                     + " AND A.BOX_NO = B.BOX_NO "
                     + " AND B.CLOSE_FLAG = 'Y' "
                     + " AND A.WORK_ORDER = '" + editWO.Text.Trim() + "' "
                     + " GROUP BY A.BOX_NO ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                LabCartonQty.Text = dsTemp.Tables[0].Rows.Count.ToString();
            }
            else
            {
                int iIndex = LVPackSpec.Items.IndexOf(LVPackSpec.FindItemWithText(g_sPKSpec, false, 0));
                if (LVPackSpec.Items[iIndex].SubItems[4].Text != "0")
                    sSQL = " SELECT A.INNERBOX_NO "
                         + " FROM SAJET.G_SN_STATUS A "
                         + "     ,SAJET.G_PACK_INNERBOX B "
                         + " WHERE A.CARTON_NO = '" + g_sCarton + "' "
                         + " AND A.INNERBOX_NO = B.INNERBOX_NO "
                         + " AND B.CLOSE_FLAG = 'Y' "
                         + " AND A.WORK_ORDER = '" + editWO.Text.Trim() + "' "
                         + " GROUP BY A.INNERBOX_NO ";
                else
                    sSQL = " SELECT SERIAL_NUMBER "
                        + " FROM SAJET.G_SN_STATUS "
                        + " WHERE CARTON_NO = '" + g_sCarton + "' "
                        + " AND WORK_ORDER = '" + editWO.Text.Trim() + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                LabCartonQty.Text = dsTemp.Tables[0].Rows.Count.ToString();
            }
        }
        public bool Create_NewCarton()
        {
            g_sCarton = "";
            if (!Get_NewNo("Carton", out g_sCarton))
                return false;

            g_sCarton = Get_NextNewNo("Carton", g_sCarton);  //檢查Carton是否已重複,並繼續找下個號碼
            editCarton.Text = g_sCarton;
            LabCartonQty.Text = "0";
            Append_PackNo("Carton", g_sCarton);
            return true;
        }
        private void editCarton_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editCarton.Text = editCarton.Text.Trim();

            g_sCarton_New = editCarton.Text;


            if (editCarton.Text == "")
            {
                string sMsg = SajetCommon.SetLanguage("Data is null") + Environment.NewLine
                            + LabCarton.Text;
                Show_Message(sMsg, 0);
                SetEditFocus("CARTON");
                return;
            }

            //檢查UDI           
            if (!Check_UDI("CARTON", editCarton.Text, TOption[3].g_bSysCreate, TOption[2].g_bSysCreate, TOption[1].g_bSysCreate))
            {
                SetEditFocus("CARTON");
                return;
            }
            g_sCarton = editCarton.Text;
            //Carton為最小單位
            if (TSetup.iPKAction == 2 || TSetup.iPKAction == 4)
            {
                //if (!Check_Carton())
                //{
                //    SetEditFocus("CARTON");
                //    return;
                //}

                if (TSetup.iPKAction == 2)
                {
                    //若Carton原本已有Pallet,直接用原號碼過站(需放回原棧板)
                    if (g_sOldPallet != "N/A" && g_sOldPallet != null)
                    {
                        string sMsg = "Carton: " + editCarton.Text + " OK " + Environment.NewLine
                                    + "Pallet No : " + g_sOldPallet + Environment.NewLine;

                        TextMsg.Text = sMsg;
                        TextMsg.ForeColor = Color.FromArgb(255, 255, 128);
                        TextMsg.BackColor = Color.Maroon;
                        PackGo(g_sOldCarton, "CARTON");
                        SetEditFocus("CARTON");
                        return;
                    }
                }

                if (g_bRefreshQty)
                {
                    if (!Refresh_PalletQty(g_sPallet))
                        return;
                }
                if (LabPalletCapacity.Text != "")
                {
                    if (Convert.ToInt32(LabPalletCapacity.Text) <= Convert.ToInt32(LabPalletQty.Text))
                    {
                        Show_Message(SajetCommon.SetLanguage("Please Close Pallet", 1), 0);
                        SetEditFocus("CARTON");
                        return;
                    }
                }

                //刷Carton就過站
                if (!Input_SN())
                {
                    SetEditFocus("CARTON");
                    return;
                }

                LabPalletQty.Text = Convert.ToString((Convert.ToInt32(LabPalletQty.Text)) + 1);
                if (Convert.ToInt32(LabPalletCapacity.Text) <= Convert.ToInt32(LabPalletQty.Text))
                    Close_Pallet(g_sPallet, "N");
                else
                    SetEditFocus("CARTON");
            }
            else
            {
                if (TOption[2].g_bInputRealease)
                {
                    //檢查是否已由BarcodeCenter展開
                    if (!Check_ReleaseNo("Carton No", editCarton.Text))
                    {
                        SetEditFocus("CARTON");
                        return;
                    }
                }
                else
                {
                    //檢查是否符合編碼規則
                    if (!Check_Rule("Carton No", editCarton.Text))
                    {
                        SetEditFocus("CARTON");
                        return;
                    }
                }
                //檢查是否重複
                if (!Check_Dup("CARTON", editCarton.Text))
                {
                    SetEditFocus("CARTON");
                    return;
                }

                //移至下一個
                if (TSetup.iPKAction < 6 || TSetup.iPKAction > 8)
                    Show_Box();
                else if (gbBox.Enabled)
                    SetEditFocus("BOX");
                else
                    SetEditFocus("SN");
            }

            Show_Message(SajetCommon.SetLanguage("Carton OK", 1), 3);
        }
        private void btnCloseCarton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(editCarton.Text))
                return;
            if (Show_Message(SajetCommon.SetLanguage("Close Carton", 1) + " ?" + Environment.NewLine + editCarton.Text, 2) != DialogResult.Yes)
                return;
            //有權限才可做Close
            if (g_iClosePrivilege < 1)
            {
                if (!Check_Privilege_Close())
                    return;
            }
            Close_Carton(editCarton.Text, "Y");
        }
        private void Close_Carton(string sCarton, string sForceClose)
        {
            if (string.IsNullOrEmpty(sCarton))
                return;

            if (g_bRefreshQty)
            {
                if (!Refresh_CartonQty(sCarton))
                    return;
            }

            //先Close Box
            if (editBox.Text != "" && gbBox.Enabled)
            {
                if (TSetup.iPKAction < 6 && TSetup.iPKAction > 8)
                {
                    Close_Box(editBox.Text, sForceClose);
                }
            }

            //Close Carton
            sSQL = " Select serial_number FROM SAJET.G_SN_STATUS "
                 + " WHERE CARTON_NO = '" + sCarton + "' "
                 + " and Rownum=1";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                sSQL = "SELECT CARTON_NO FROM SAJET.G_PACK_CARTON "
                     + "WHERE CARTON_NO = '" + sCarton + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count == 0)
                    Show_Message(SajetCommon.SetLanguage("No Carton", 1) + " [" + sCarton + "] !", 3);
                else
                {
                    //Carton中無序號,直接刪除此Pallet
                    sSQL = "DELETE SAJET.G_PACK_CARTON "
                         + "WHERE CARTON_NO = '" + sCarton + "' ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    Show_Message(SajetCommon.SetLanguage("Delete Carton", 1) + " [" + sCarton + "] !", 3);
                }
            }
            else
            {
                sSQL = " UPDATE SAJET.G_PACK_CARTON "
                     + " SET CLOSE_FLAG = 'Y' "
                     + " WHERE CARTON_NO = '" + sCarton + "' and rownum = 1";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (TSetup.iPKAction == 3 || TSetup.iPKAction == 8)
                {
                    sSQL = "UPDATE SAJET.G_PACK_Pallet "
                         + "SET CLOSE_FLAG = 'Y' "
                         + "WHERE Pallet_No = '" + sCarton + "' and rownum = 1 ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                }

                //手動強制Close
                if (sForceClose == "Y")
                {
                    sSQL = " INSERT INTO SAJET.G_PACK_FORCECLOSE "
                         + " (PACK_NO, PACK_TYPE, EMP_ID,UPDATE_TIME) "
                         + " VALUES "
                         + " ('" + sCarton + "', 'Carton', '" + g_sUserID + "',SYSDATE)";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                }
                Show_Message(SajetCommon.SetLanguage("Close Carton", 1) + " [" + sCarton + "] !", 3);
                LabPalletQty.Text = Convert.ToString(Convert.ToInt32(LabPalletQty.Text) + 1);

                //列印Carton
                if (TOption[2].g_bPrint)
                {
                    Print_Label(2, sCarton);
                }
            }
            g_sCarton = "";
            editCarton.Text = "";
            LabCartonQty.Text = "0";

            string S = "13689";
            if (S.IndexOf(TSetup.iPKAction.ToString()) == -1 &&
                Convert.ToInt32(LabPalletCapacity.Text) <= Convert.ToInt32(LabPalletQty.Text))
            {
                Close_Pallet(g_sPallet, sForceClose);
            }
            else
            {
                if (g_bCycle)
                {
                    GetPack(g_sPKSpec, "Y");
                    Update_PackTerminal();
                }
                Show_Carton();
            }
        }

        //===Box & Inner Box=================================================
        public void Show_Box()
        {
            //===處理Box=== 
            if (!gbBox.Enabled)
            //if (!gbBox.Enabled)
            {
                SetEditFocus("SN");
                return;
            }
            if (TSetup.iPKAction == 9)
            {
                //有未滿的Inner BOX
                if (Get_UnfinishInnerBox())
                {
                    Get_PackInnerBoxQty();
                    SetEditFocus("SN");
                }
                else //無未滿的Inner BOX
                {
                    //   if (gbBox.Enabled)
                    if (gbBox.Enabled)
                    {
                        //Inner Box為SystemCreate
                        if (TOption[4].g_bSysCreate)
                        {
                            if (Create_NewInnerBox())
                            {
                                SetEditFocus("SN");
                            }
                            else
                            {
                                SetEditFocus("BOX");
                            }
                        }
                        else
                        {
                            SetEditFocus("BOX");
                        }
                    }
                    else
                    {
                        SetEditFocus("SN");
                    }
                }
            }
            else
            {
                //有未滿的BOX
                if (Get_UnfinishBox())
                {
                    Get_PackBoxQty();
                    SetEditFocus("SN");
                }
                else //無未滿的BOX
                {
                    // if (gbBox.Enabled)
                    if (gbBox.Enabled)
                    {
                        //Box為SystemCreate
                        if (TOption[1].g_bSysCreate)
                        {
                            if (Create_NewBox())
                            {
                                SetEditFocus("SN");
                            }
                            else
                            {
                                SetEditFocus("BOX");
                            }
                        }
                        else
                        {
                            SetEditFocus("BOX");
                        }
                    }
                    else
                    {
                        SetEditFocus("SN");
                    }
                }
            }
        }
        public bool Get_UnfinishBox()
        {
            //找之前尚未Close的Box
            g_sBox = "";
            sSQL = "SELECT BOX_NO FROM SAJET.G_PACK_BOX ";
            if (TSetup.sPKBase == "Work Order")
                sSQL = sSQL + " Where WORK_ORDER = '" + editWO.Text + "' ";
            else
                sSQL = sSQL + " Where PART_ID = '" + g_sPartID + "' ";
            sSQL = sSQL + " AND TERMINAL_ID = '" + g_sTerminalID + "' "
                        + " AND CLOSE_FLAG = 'N' "
                        + " AND PKSPEC_ID = '" + g_sPKSpecID + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (dsTemp.Tables[0].Rows.Count == 1)
                    g_sBox = dsTemp.Tables[0].Rows[0]["BOX_NO"].ToString();
                else
                    g_sBox = Show_UnFinishForm("Box_No"); //有多筆尚未Close的Box,跳出Form供User選擇

                if (!string.IsNullOrEmpty(g_sBox))
                {
                    editBox.Text = g_sBox;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool Get_UnfinishInnerBox()
        {
            //找之前尚未Close的InnerBox
            g_sInnerBox = "";
            sSQL = "SELECT INNERBOX_NO FROM SAJET.G_PACK_INNERBOX ";
            if (TSetup.sPKBase == "Work Order")
                sSQL = sSQL + " Where WORK_ORDER = '" + editWO.Text + "' ";
            else
                sSQL = sSQL + " Where PART_ID = '" + g_sPartID + "' ";
            sSQL = sSQL + " AND TERMINAL_ID = '" + g_sTerminalID + "' "
                        + " AND CLOSE_FLAG = 'N' "
                        + " AND PKSPEC_ID = '" + g_sPKSpecID + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (dsTemp.Tables[0].Rows.Count == 1)
                    g_sInnerBox = dsTemp.Tables[0].Rows[0]["INNERBOX_NO"].ToString();
                else
                    g_sInnerBox = Show_UnFinishForm("InnerBox_No"); //有多筆尚未Close的InnerBox,跳出Form供User選擇

                if (!string.IsNullOrEmpty(g_sInnerBox))
                {
                    editBox.Text = g_sInnerBox;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public void Get_PackBoxQty()
        {
            //Box內的數量
            //20250904雃博客製撈委外UDI
            decimal dBoxCount = 0;
            sSQL = "SELECT COUNT(*) CNT "
                 + "FROM SAJET.G_SN_STATUS "
                 + "WHERE BOX_NO = '" + g_sBox + "' "
                 + " AND WORK_ORDER = '" + editWO.Text.Trim() + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            dBoxCount = dBoxCount + decimal.Parse(dsTemp.Tables[0].Rows[0]["CNT"].ToString());
            sSQL = "SELECT COUNT(*) CNT "
                 + "FROM SAJET.G_PACK_BOX_OUT "
                 + "WHERE BOX_NO = '" + g_sBox + "' "
                 + " AND A.WORK_ORDER = '" + editWO.Text.Trim() + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            dBoxCount = dBoxCount + decimal.Parse(dsTemp.Tables[0].Rows[0]["CNT"].ToString());
            LabBoxQty.Text = dBoxCount.ToString();
        }
        public void Get_PackInnerBoxQty()
        {
            //InnerBox內的數量
            sSQL = " SELECT SERIAL_NUMBER "
                 + " FROM SAJET.G_SN_STATUS "
                 + " WHERE INNERBOX_NO = '" + g_sInnerBox + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            LabBoxQty.Text = dsTemp.Tables[0].Rows.Count.ToString();
        }
        public bool Create_NewBox()
        {
            g_sBox = "";
            if (!Get_NewNo("Box", out g_sBox))
                return false;

            g_sBox = Get_NextNewNo("Box", g_sBox);  //檢查Box是否已重複,並繼續找下個號碼
            editBox.Text = g_sBox;
            LabBoxQty.Text = "0";
            Append_PackNo("Box", g_sBox);
            return true;
        }
        public bool Create_NewInnerBox()
        {
            g_sInnerBox = "";
            if (!Get_NewNo("InnerBox", out g_sInnerBox))
                return false;

            g_sInnerBox = Get_NextNewNo("InnerBox", g_sInnerBox);  //檢查InnerBox是否已重複,並繼續找下個號碼
            editBox.Text = g_sInnerBox;
            LabBoxQty.Text = "0";
            Append_PackNo("InnerBox", g_sInnerBox);
            return true;
        }
        private void editBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editBox.Text = editBox.Text.Trim();
            if (editBox.Text == "")
            {
                string sMsg = SajetCommon.SetLanguage("Data is null") + Environment.NewLine
                            + lablBox.Text;
                Show_Message(sMsg, 0);

                SetEditFocus("BOX");
                return;
            }

            //檢查UDI           
            if (!Check_UDI("BOX", editBox.Text, TOption[3].g_bSysCreate, TOption[2].g_bSysCreate, TOption[1].g_bSysCreate))
            {
                SetEditFocus("BOX");
                return;
            }
            if (TSetup.iPKAction == 9)
                g_sInnerBox = editBox.Text;
            else
                g_sBox = editBox.Text;
            //Box為最小單位
            if (TSetup.iPKAction >= 6 && TSetup.iPKAction <= 8)
            {
                //檢查是否重複
                if (!Check_BoxDup(editCarton.Text, editBox.Text))
                {
                    SetEditFocus("BOX");
                    return;
                }
                //if (!Check_Box())
                //{
                //    SetEditFocus("BOX");
                //    return;
                //}

                //若Box原本已有Carton及Pallet,直接用原號碼過站(需放回原箱原棧板)
                if (g_sOldPallet != "N/A" && g_sOldCarton != "N/A" && g_sOldPallet != null && g_sOldCarton != null)
                {
                    TextMsg.Text = "Box: " + editBox.Text + " OK " + Environment.NewLine
                                + "Pallet No : " + g_sOldPallet + Environment.NewLine
                                + "Carton No : " + g_sOldCarton;
                    TextMsg.ForeColor = Color.FromArgb(255, 255, 128);
                    TextMsg.BackColor = Color.Maroon;
                    PackGo(g_sOldBox, "BOX");
                    SetEditFocus("BOX");
                    return;
                }

                if (g_bRefreshQty)
                {
                    if (!Refresh_CartonQty(g_sCarton))
                        return;
                }
                if (LabCartonCapacity.Text != "")
                {
                    if (Convert.ToInt32(LabCartonCapacity.Text) <= Convert.ToInt32(LabCartonQty.Text))
                    {
                        Show_Message(SajetCommon.SetLanguage("Please Close Carton", 1), 0);
                        SetEditFocus("BOX");
                        return;
                    }
                }

                //刷Box就過站
                if (!Input_SN())
                {
                    SetEditFocus("BOX");
                    return;
                }

                LabCartonQty.Text = Convert.ToString((Convert.ToInt32(LabCartonQty.Text)) + 1);
                if (Convert.ToInt32(LabCartonCapacity.Text) <= Convert.ToInt32(LabCartonQty.Text))
                    Close_Carton(g_sCarton, "N");
                else
                    SetEditFocus("BOX");
            }
            else
            {
                if (TSetup.iPKAction == 9)
                {
                    if (TOption[4].g_bInputRealease)
                    {
                        //檢查是否已由BarcodeCenter展開
                        if (!Check_ReleaseNo("InnerBox No", editBox.Text))
                        {
                            SetEditFocus("BOX");
                            return;
                        }
                    }
                    else
                    {
                        //檢查是否符合編碼規則
                        if (!Check_Rule("InnerBox No", editBox.Text))
                        {
                            SetEditFocus("BOX");
                            return;
                        }
                    }
                    //檢查是否重複
                    if (!Check_Dup("INNERBOX", editBox.Text))
                    {
                        SetEditFocus("BOX");
                        return;
                    }
                }
                else
                {
                    if (TOption[1].g_bInputRealease)
                    {
                        //檢查是否已由BarcodeCenter展開
                        if (!Check_ReleaseNo("Box No", editBox.Text))
                        {
                            SetEditFocus("BOX");
                            return;
                        }
                    }
                    else
                    {
                        //檢查是否符合編碼規則
                        if (!Check_Rule("Box No", editBox.Text))
                        {
                            SetEditFocus("BOX");
                            return;
                        }
                    }
                    //檢查是否重複
                    if (!Check_Dup("BOX", editBox.Text))
                    {
                        SetEditFocus("BOX");
                        return;
                    }
                }
                //移至下一個
                SetEditFocus("SN");
            }
            string sText = "";
            if (TSetup.iPKAction == 9)
                sText = "Inner ";
            Show_Message(SajetCommon.SetLanguage(sText + "Box OK"), 3);
        }
        private void btnCloseBox_Click(object sender, EventArgs e)
        {
            if (gbBox.Enabled && string.IsNullOrEmpty(editBox.Text))
                return;
            string sMsg = "";
            if (TSetup.iPKAction == 9)
                sMsg = "Inner ";
            if (Show_Message(SajetCommon.SetLanguage("Close " + sMsg + "Box") + " ?" + Environment.NewLine + editBox.Text, 2) != DialogResult.Yes)
                return;
            //有權限才可做Close
            if (g_iClosePrivilege < 1)
            {
                if (!Check_Privilege_Close())
                    return;
            }
            if (TSetup.iPKAction == 9)
                Close_InnerBox(editBox.Text, "Y");
            else
                Close_Box(editBox.Text, "Y");
        }
        private void Close_Box(string sBox, string sForceClose)
        {
            if (gbBox.Enabled && string.IsNullOrEmpty(editBox.Text))
                return;
            if (g_bRefreshQty)
            {
                if (!Refresh_BoxQty(g_sBox))
                    return;
            }

            //Close Box
            sSQL = " Select serial_number FROM SAJET.G_SN_STATUS "
                 + " WHERE Box_NO = '" + sBox + "' "
                 + " and Rownum=1";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                sSQL = "SELECT BOX_NO FROM SAJET.G_PACK_BOX "
                     + "WHERE BOX_NO = '" + sBox + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count == 0)
                    Show_Message(SajetCommon.SetLanguage("No Box", 1) + " [" + sBox + "] !", 3);
                else
                {
                    //Box中無序號,直接刪除此Box
                    sSQL = "DELETE SAJET.G_PACK_BOX "
                         + "WHERE BOX_NO = '" + sBox + "' ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    Show_Message(SajetCommon.SetLanguage("Delete Box", 1) + " [" + sBox + "] !", 3);
                }
            }
            else
            {
                sSQL = " UPDATE SAJET.G_PACK_BOX "
                     + " SET CLOSE_FLAG = 'Y' "
                     + " WHERE BOX_NO = '" + sBox + "' and rownum = 1";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                //手動強制Close
                if (sForceClose == "Y")
                {
                    sSQL = " INSERT INTO SAJET.G_PACK_FORCECLOSE "
                         + " (PACK_NO, PACK_TYPE, EMP_ID,UPDATE_TIME) "
                         + " VALUES "
                         + " ('" + sBox + "', 'Box', '" + g_sUserID + "',SYSDATE)";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                }
                Show_Message(SajetCommon.SetLanguage("Close Box", 1) + " [" + sBox + "] !", 3);
                LabCartonQty.Text = Convert.ToString(Convert.ToInt32(LabCartonQty.Text) + 1);

                //列印Carton
                if (TOption[1].g_bPrint)
                {
                    Print_Label(1, sBox);
                }
            }

            g_sBox = "";
            editBox.Text = "";
            LabBoxQty.Text = "0";

            if (TSetup.iPKAction != 5 &&
                Convert.ToInt32(LabCartonCapacity.Text) <= Convert.ToInt32(LabCartonQty.Text))
            {
                Close_Carton(g_sCarton, sForceClose);
            }
            else
            {
                if (g_bCycle)
                {
                    GetPack(g_sPKSpec, "Y");
                    Update_PackTerminal();
                }
                Show_Box();
            }
        }
        private void Close_InnerBox(string sBox, string sForceClose)
        {
            if (gbBox.Enabled && string.IsNullOrEmpty(editBox.Text))
                return;
            if (g_bRefreshQty)
            {
                if (!Refresh_InnerBoxQty(g_sInnerBox))
                    return;
            }

            //Close Inner Box
            sSQL = " Select serial_number FROM SAJET.G_SN_STATUS "
                 + " WHERE InnerBox_NO = '" + sBox + "' "
                 + " and Rownum=1";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                sSQL = "SELECT INNERBOX_NO FROM SAJET.G_PACK_INNERBOX "
                     + "WHERE INNERBOX_NO = '" + sBox + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count == 0)
                    Show_Message(SajetCommon.SetLanguage("No Inner Box", 1) + " [" + sBox + "] !", 3);
                else
                {
                    //Inner Box中無序號,直接刪除此Inner Box
                    sSQL = "DELETE SAJET.G_PACK_INNERBOX "
                         + "WHERE INNERBOX_NO = '" + sBox + "' ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    Show_Message(SajetCommon.SetLanguage("Delete Inner Box", 1) + " [" + sBox + "] !", 3);
                }
            }
            else
            {
                sSQL = " UPDATE SAJET.G_PACK_INNERBOX "
                     + " SET CLOSE_FLAG = 'Y' "
                     + " WHERE INNERBOX_NO = '" + sBox + "' and rownum = 1";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                //手動強制Close
                if (sForceClose == "Y")
                {
                    sSQL = " INSERT INTO SAJET.G_PACK_FORCECLOSE "
                         + " (PACK_NO, PACK_TYPE, EMP_ID,UPDATE_TIME) "
                         + " VALUES "
                         + " ('" + sBox + "', 'Inner Box', '" + g_sUserID + "',SYSDATE)";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                }
                Show_Message(SajetCommon.SetLanguage("Close Inner Box", 1) + " [" + sBox + "] !", 3);
                LabCartonQty.Text = Convert.ToString(Convert.ToInt32(LabCartonQty.Text) + 1);

                //列印Carton
                if (TOption[4].g_bPrint)
                {
                    Print_Label(4, sBox);
                }
            }

            g_sInnerBox = "";
            editBox.Text = "";
            LabBoxQty.Text = "0";
            if (g_bCycle)
            {
                GetPack(g_sPKSpec, "Y");
                Update_PackTerminal();
            }
            Show_Box();
        }

        public string Get_NextNewNo(string sType, string sNo)
        {
            //檢查自動產生的號碼是否已存在,並繼續找下個號碼
            string sStart = "";
            string sEnd = "";
            string sNewNo = sNo;
            while (!Check_Exist(sType, sNewNo))
            {
                if (sStart == "")
                    sStart = sNewNo;
                sEnd = sNewNo;
                if (!Get_NewNo(sType, out sNewNo))
                {
                    return sNewNo;
                }
            }

            //顯示重複的區間號碼
            if (sStart != "")
            {
                if (sStart == sEnd)
                    Show_Message(sType + " Duplicate: " + sStart, 3);
                else
                    Show_Message(sType + " Duplicate: " + sStart + " ~ " + sEnd, 3);
            }

            return sNewNo;
        }

        public bool Check_Exist(string sType, string sValue)
        {
            string sTable = "";
            string sField = "";
            switch (sType.ToUpper())
            {
                case "PALLET":
                    sTable = "SAJET.G_PACK_PALLET";
                    sField = "PALLET_NO";
                    break;
                case "CARTON":
                    sTable = "SAJET.G_PACK_CARTON";
                    sField = "CARTON_NO";
                    break;
                case "BOX":
                    sTable = "SAJET.G_PACK_BOX";
                    sField = "BOX_NO";
                    break;
                case "INNERBOX":
                    sTable = "SAJET.G_PACK_INNERBOX";
                    sField = "INNERBOX_NO";
                    break;
                case "CSN":
                    sTable = "SAJET.G_SN_STATUS";
                    sField = "CUSTOMER_SN";
                    break;
            }

            sSQL = " SELECT " + sField + " FROM " + sTable
                 + " WHERE " + sField + " = '" + sValue + "' and rownum = 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return false;
            else
                return true;
        }

        public void Append_PackNo(string sType, string sNo)
        {
            if (sNo == "N/A")
                return;
            string sTable = "";
            string sField = "";
            switch (sType)
            {
                case "Pallet":
                    sTable = "SAJET.G_PACK_PALLET";
                    sField = "PALLET_NO";
                    break;
                case "Carton":
                    sTable = "SAJET.G_PACK_CARTON";
                    sField = "CARTON_NO";
                    break;
                case "Box":
                    sTable = "SAJET.G_PACK_BOX";
                    sField = "BOX_NO";
                    break;
                case "InnerBox":
                    sTable = "SAJET.G_PACK_INNERBOX";
                    sField = "INNERBOX_NO";
                    break;
            }
            sSQL = " SELECT " + sField
                 + " From " + sTable
                 + " Where " + sField + " = '" + sNo + "' and rownum=1";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                sSQL = " INSERT INTO " + sTable
                     + " (" + sField + ",WORK_ORDER,PART_ID,CLOSE_FLAG,TERMINAL_ID,CREATE_EMP_ID,PKSPEC_ID) "
                     + " VALUES "
                     + " ('" + sNo + "','" + editWO.Text + "','" + g_sPartID + "','N','" + g_sTerminalID + "','" + g_sUserID + "','" + g_sPKSpecID + "')";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
        }

        public string Show_UnFinishForm(string sType)
        {
            //有多筆尚未Close,跳出Form供User選擇
            string sFrom = ""; string sFromField = "";
            string sTable = ""; string sField = "";
            switch (sType)
            {
                case "Pallet_No":
                    sField = "Carton_No";
                    sTable = " sajet.g_pack_pallet";
                    break;
                case "Carton_No":
                    if (gbBox.Enabled)
                        sField = "Box_No";
                    else
                        sField = "Serial_Number";
                    sTable = " sajet.g_pack_carton";
                    sFromField = "Pallet_No sFrom, ";
                    if (gbPallet.Enabled)
                        sFrom = editPallet.Text;
                    break;
                case "Box_No":
                    sField = "Serial_Number";
                    sTable = " sajet.g_pack_box";
                    sFromField = "Carton_No sFrom, ";
                    if (gbCarton.Enabled)
                        sFrom = editCarton.Text;
                    break;
                case "InnerBox_No":
                    sField = "Serial_Number";
                    sTable = " sajet.g_pack_Innerbox";
                    sFromField = "Box_No sFrom, ";
                    break;
            }

            fUnfinish f = new fUnfinish();
            f.Text = "Unfinished " + sType;
            f.LVData.Columns[0].Text = sType;
            f.LVData.Columns[1].Text = "Count";

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                string sData = dsTemp.Tables[0].Rows[i][sType].ToString();
                string sSQL1 = "SELECT distinct " + sFromField + sType + " sNo, " + sField + " sCount "
                             + "FROM SAJET.G_SN_STATUS "
                             + "WHERE " + sType + " = '" + sData + "' ";
                DataSet dsData = ClientUtils.ExecuteSQL(sSQL1);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    f.LVData.Items.Add(sData);
                    f.LVData.Items[f.LVData.Items.Count - 1].SubItems.Add("0");
                }
                else if (sFrom != "")
                {
                    //20130522 出現錯誤：位置 3 沒有資料列。
                    //把 Rows[i] 改成 Rows[0]
                    //原本為：sFrom == dsData.Tables[0].Rows[i]["sFrom"].ToString()
                    if (sFrom == dsData.Tables[0].Rows[0]["sFrom"].ToString())
                    {
                        f.LVData.Items.Add(sData);
                        f.LVData.Items[f.LVData.Items.Count - 1].SubItems.Add(dsData.Tables[0].Rows.Count.ToString());
                    }
                }
                else
                {
                    f.LVData.Items.Add(sData);
                    f.LVData.Items[f.LVData.Items.Count - 1].SubItems.Add(dsData.Tables[0].Rows.Count.ToString());
                }

            }
            if (f.ShowDialog() != DialogResult.OK)
                return "";

            string sResult = f.LVData.SelectedItems[0].Text;
            //若無產品則刪除
            for (int i = 0; i <= f.LVData.Items.Count - 1; i++)
            {
                string sNo = f.LVData.Items[i].Text;
                if (sNo != sResult && f.LVData.Items[i].SubItems[1].Text == "0")
                {
                    string sSQL2 = "delete from " + sTable
                                 + " where " + sType + " = '" + sNo + "' ";
                    ClientUtils.ExecuteSQL(sSQL2);
                }
            }
            return sResult;
        }

        public bool Get_NewNo(string sType, out string sNewNo)
        {
            //自動產生號碼 SystemCreate
            string sField = "";
            switch (sType)
            {
                case "Pallet":
                    sField = "Pallet No";
                    break;
                case "Carton":
                    sField = "Carton No";
                    break;
                case "Box":
                    sField = "Box No";
                    break;
                case "InnerBox":
                    sField = "InnerBox No";
                    break;
                case "CSN":
                    sField = "Customer SN";
                    break;
            }

            sSQL = " Select * From SAJET.G_WO_PARAM "
                 + " Where WORK_ORDER = '" + editWO.Text + "' "
                 + " and MODULE_NAME = '" + sField.ToUpper() + " RULE' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            //沒有設定編碼規則
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                //使用預設Function產生號碼
                if (TSetup.sPKBase == "Work Order")
                    sSQL = "select sajet.packing_label('" + sType + "','" + TSetup.sPKBase + "','" + editWO.Text + "') SNID from dual ";
                else
                    sSQL = "select sajet.packing_label('" + sType + "','" + TSetup.sPKBase + "','" + LabPart.Text + "') SNID from dual ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                sNewNo = dsTemp.Tables[0].Rows[0]["SNID"].ToString();
                return true;
            }
            else //有設定編碼規則
            {
                string sRuleName = dsTemp.Tables[0].Rows[0]["FUNCTION_NAME"].ToString();

                //找欲使用的Sequence:sys_label中的seq_name+Rule_Name
                string sSeqFix = "";
                string sSQL1 = "SELECT SEQ_NAME from sajet.sys_label "
                             + "where Upper(label_name) = '" + sField.ToUpper() + "' ";
                DataSet dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);
                if (dsTemp1.Tables[0].Rows.Count > 0)
                    sSeqFix = dsTemp1.Tables[0].Rows[0]["SEQ_NAME"].ToString();
                string sSeqName = sSeqFix + sRuleName;

                //編碼規則
                object[] objData = new object[3];
                string[] sParam = new string[1];
                //(呼叫LabelCheckDll.dll)              
                LabelCheck.Check LabelCheckDll = new LabelCheck.Check();
                //找出編碼規則內容                
                bool bRuleExist = LabelCheckDll.Get_RuleData(sField, editWO.Text, ref sParam, ref objData);

                //找出實際Function值===
                ListView LVFun = new ListView();
                LVFun = (ListView)objData[Array.IndexOf(sParam, "User Function")];
                for (int i = 0; i <= LVFun.Items.Count - 1; i++)
                {
                    string sFun_Field = LVFun.Items[i].SubItems[1].Text;
                    string sFun_Name = LVFun.Items[i].SubItems[2].Text;
                    string sData = "N/A";
                    if (sFun_Field != "N/A")
                        sData = listValue.Items[listField.Items.IndexOf(sFun_Field)].ToString();
                    sSQL = " select " + sFun_Name + "('" + sData + "') fundata from dual ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    string sValue = dsTemp.Tables[0].Rows[0]["fundata"].ToString();
                    LVFun.Items[i].SubItems.Add(sValue);
                }

                //上次Reset日期
                string sResetMark = "";
                sSQL1 = "Select PARAME_VALUE "
                      + "From SAJET.SYS_MODULE_PARAM "
                      + "Where UPPER(MODULE_NAME) = '" + sSeqFix.ToUpper() + "' "
                      + "and FUNCTION_NAME = '" + sRuleName + "' "
                      + "and PARAME_NAME = 'Reset Sequence Mark' ";
                dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);
                if (dsTemp1.Tables[0].Rows.Count > 0)
                    sResetMark = dsTemp1.Tables[0].Rows[0]["PARAME_VALUE"].ToString();

                //產生號碼  
                string sInputNo = "";
                if (bRuleExist)
                {
                    LabelCheckDll.Create_NewNo(out sInputNo, sSeqName, ref sResetMark, sParam, objData);
                }
                sNewNo = sInputNo;

                //紀錄此次Reset日期
                sSQL1 = "Select rowid "
                      + "From SAJET.SYS_MODULE_PARAM "
                      + "Where UPPER(MODULE_NAME) = '" + sSeqFix.ToUpper() + "' "
                      + "and FUNCTION_NAME = '" + sRuleName + "' "
                      + "and PARAME_NAME = 'Reset Sequence Mark' ";
                dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);
                if (dsTemp1.Tables[0].Rows.Count == 0)
                {
                    sSQL1 = " Insert Into SAJET.SYS_MODULE_PARAM "
                          + " (MODULE_NAME,FUNCTION_NAME,PARAME_NAME,PARAME_ITEM,PARAME_VALUE,UPDATE_USERID ) "
                          + " Values "
                          + " ('" + sSeqFix.ToUpper() + "','" + sRuleName + "','Reset Sequence Mark','" + sRuleName + "','" + sResetMark + "','" + g_sUserID + "' )";
                    dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);
                }
                else
                {
                    string sRowid = dsTemp1.Tables[0].Rows[0]["rowid"].ToString();
                    sSQL1 = " update SAJET.SYS_MODULE_PARAM "
                          + " set parame_value = '" + sResetMark + "' "
                          + " where rowid = '" + sRowid + "' ";
                    dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);
                }
                return true;
            }
        }

        public bool Check_Carton()
        {
            sSQL = "SELECT A.WORK_ORDER, A.PART_ID,A.SERIAL_NUMBER "
                 + "     , A.CARTON_NO, B.PART_NO,NVL(A.PALLET_NO,'N/A') PALLET_NO "
                 + "FROM  SAJET.G_SN_STATUS A, SAJET.SYS_PART B "
                 + "WHERE A.CARTON_NO = '" + editCarton.Text + "' "
                 + "AND A.PART_ID = B.PART_ID(+) and rownum = 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                Show_Message(SajetCommon.SetLanguage("No Carton", 1), 0);
                return false;
            }
            //檢查工單或料號是否相同
            string sCartonPart = dsTemp.Tables[0].Rows[0]["PART_NO"].ToString();
            string sCartonWO = dsTemp.Tables[0].Rows[0]["WORK_ORDER"].ToString();
            g_sSN = dsTemp.Tables[0].Rows[0]["SERIAL_NUMBER"].ToString();
            g_sOldPallet = dsTemp.Tables[0].Rows[0]["PALLET_NO"].ToString();
            g_sOldCarton = dsTemp.Tables[0].Rows[0]["CARTON_NO"].ToString();

            if (TSetup.sPKBase == "Work Order")
            {
                if (editWO.Text != sCartonWO)
                {
                    Show_Message(SajetCommon.SetLanguage("Work Order is Different", 1) + Environment.NewLine + sCartonWO, 0);
                    return false;
                }
            }
            else
            {
                if (LabPart.Text != sCartonPart)
                {
                    Show_Message(SajetCommon.SetLanguage("Part No is Different", 1) + Environment.NewLine + sCartonPart, 0);
                    return false;
                }
            }

            //Check Route===================================================             
            try
            {
                object[][] Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TERMINALID", g_sTerminalID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", g_sSN };
                Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_CKRT_ROUTE", Params);

                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    Show_Message(sRes, 0);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Show_Message("SAJET.SJ_CKRT_ROUTE" + Environment.NewLine + ex.Message, 0);
                return false;
            }

            //檢查此Carton是否已Close
            sSQL = "SELECT CLOSE_FLAG from SAJET.G_PACK_CARTON "
                 + "WHERE CARTON_NO = '" + editCarton.Text + "' "
                 + "and rownum = 1";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (dsTemp.Tables[0].Rows[0]["CLOSE_FLAG"].ToString() != "Y")
                {
                    Show_Message(SajetCommon.SetLanguage("This Carton have not Close", 1), 0);
                    return false;
                }
            }

            return true;
        }

        
        public bool Check_Box()
        {
            sSQL = "SELECT A.WORK_ORDER, A.PART_ID,A.SERIAL_NUMBER,A.BOX_NO "
                 + ", B.PART_NO,NVL(A.PALLET_NO,'N/A') PALLET_NO,NVL(A.CARTON_NO,'N/A') CARTON_NO "
                 + "FROM  SAJET.G_SN_STATUS A, SAJET.SYS_PART B "
                 + "WHERE A.BOX_NO = '" + editBox.Text + "' "
                 + "AND A.PART_ID = B.PART_ID(+) and rownum = 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                Show_Message(SajetCommon.SetLanguage("No Box", 1), 0);
                return false;
            }
            //檢查工單或料號是否相同
            string sBoxPart = dsTemp.Tables[0].Rows[0]["PART_NO"].ToString();
            string sBoxWO = dsTemp.Tables[0].Rows[0]["WORK_ORDER"].ToString();
            g_sSN = dsTemp.Tables[0].Rows[0]["SERIAL_NUMBER"].ToString();
            g_sOldPallet = dsTemp.Tables[0].Rows[0]["PALLET_NO"].ToString();
            g_sOldCarton = dsTemp.Tables[0].Rows[0]["CARTON_NO"].ToString();
            g_sOldBox = dsTemp.Tables[0].Rows[0]["BOX_NO"].ToString();
            if (TSetup.sPKBase == "Work Order")
            {
                if (editWO.Text != sBoxWO)
                {
                    Show_Message(SajetCommon.SetLanguage("Work Order is Different", 1) + Environment.NewLine + sBoxWO, 0);
                    return false;
                }
            }
            else
            {
                if (LabPart.Text != sBoxPart)
                {
                    Show_Message(SajetCommon.SetLanguage("Part No is Different", 1) + Environment.NewLine + sBoxPart, 0);
                    return false;
                }
            }

            //Check Route===================================================             
            try
            {
                object[][] Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TERMINALID", g_sTerminalID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", g_sSN };
                Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_CKRT_ROUTE", Params);

                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    Show_Message(sRes, 0);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Show_Message("SAJET.SJ_CKRT_ROUTE" + Environment.NewLine + ex.Message, 0);
                return false;
            }

            //檢查此Box是否已Close
            sSQL = "SELECT CLOSE_FLAG from SAJET.G_PACK_BOX "
                 + "WHERE BOX_NO = '" + editBox.Text + "' "
                 + "and rownum = 1";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (dsTemp.Tables[0].Rows[0]["CLOSE_FLAG"].ToString() != "Y")
                {
                    Show_Message(SajetCommon.SetLanguage("This Box have not Close", 1), 0);
                    return false;
                }
            }

            return true;
        }
        public bool Check_InnerBox()
        {
            sSQL = "SELECT A.WORK_ORDER, A.PART_ID,A.SERIAL_NUMBER,A.INNERBOX_NO,A.BOX_NO "
                 + ", B.PART_NO,NVL(A.PALLET_NO,'N/A') PALLET_NO,NVL(A.CARTON_NO,'N/A') CARTON_NO "
                 + "FROM  SAJET.G_SN_STATUS A, SAJET.SYS_PART B "
                 + "WHERE A.INNERBOX_NO = '" + editBox.Text + "' "
                 + "AND A.PART_ID = B.PART_ID(+) and rownum = 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                Show_Message(SajetCommon.SetLanguage("No Inner Box", 1), 0);
                return false;
            }
            //檢查工單或料號是否相同
            string sBoxPart = dsTemp.Tables[0].Rows[0]["PART_NO"].ToString();
            string sBoxWO = dsTemp.Tables[0].Rows[0]["WORK_ORDER"].ToString();
            g_sSN = dsTemp.Tables[0].Rows[0]["SERIAL_NUMBER"].ToString();
            g_sOldPallet = dsTemp.Tables[0].Rows[0]["PALLET_NO"].ToString();
            g_sOldCarton = dsTemp.Tables[0].Rows[0]["CARTON_NO"].ToString();
            g_sOldBox = dsTemp.Tables[0].Rows[0]["BOX_NO"].ToString();
            g_sOldInnerBox = dsTemp.Tables[0].Rows[0]["INNERBOX_NO"].ToString();
            if (TSetup.sPKBase == "Work Order")
            {
                if (editWO.Text != sBoxWO)
                {
                    Show_Message(SajetCommon.SetLanguage("Work Order is Different", 1) + Environment.NewLine + sBoxWO, 0);
                    return false;
                }
            }
            else
            {
                if (LabPart.Text != sBoxPart)
                {
                    Show_Message(SajetCommon.SetLanguage("Part No is Different", 1) + Environment.NewLine + sBoxPart, 0);
                    return false;
                }
            }

            //Check Route===================================================             
            try
            {
                object[][] Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TERMINALID", g_sTerminalID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", g_sSN };
                Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_CKRT_ROUTE", Params);

                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    Show_Message(sRes, 0);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Show_Message("SAJET.SJ_CKRT_ROUTE" + Environment.NewLine + ex.Message, 0);
                return false;
            }

            //檢查此Box是否已Close
            sSQL = "SELECT CLOSE_FLAG from SAJET.G_PACK_INNERBOX "
                 + "WHERE INNERBOX_NO = '" + editBox.Text + "' "
                 + "and rownum = 1";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (dsTemp.Tables[0].Rows[0]["CLOSE_FLAG"].ToString() != "Y")
                {
                    Show_Message(SajetCommon.SetLanguage("This Inner Box have not Close", 1), 0);
                    return false;
                }
            }

            return true;
        }

        public bool Check_Dup(string sType, string sValue)
        {
            string sTable = "";
            string sField = "";
            switch (sType)
            {
                case "PALLET":
                    sTable = "SAJET.G_PACK_PALLET";
                    sField = "PALLET_NO";
                    break;
                case "CARTON":
                    sTable = "SAJET.G_PACK_CARTON";
                    sField = "CARTON_NO";
                    break;
                case "INNERBOX":
                    sTable = "SAJET.G_PACK_INNERBOX";
                    sField = "INNERBOX_NO";
                    break;
                case "BOX":
                    sTable = "SAJET.G_PACK_BOX";
                    sField = "BOX_NO";
                    break;
            }

            sSQL = "SELECT A.Close_Flag,A." + sField + ",B.PDLINE_NAME "
                 + "  FROM " + sTable + " A "
                 + " LEFT JOIN ( SELECT A.TERMINAL_ID,B.PDLINE_NAME "
                 + "             FROM SAJET.SYS_TERMINAL A,SAJET.SYS_PDLINE B WHERE A.PDLINE_ID = B.PDLINE_ID ) B "
                 + " ON A.TERMINAL_ID = B.TERMINAL_ID "
                 + "WHERE A." + sField + " = '" + sValue
                 + "' AND A." + sField + " <> 'Remainder box'"
                 + " and rownum = 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (dsTemp.Tables[0].Rows[0]["CLOSE_FLAG"].ToString() == "Y")
                    Show_Message(sType + " Had Closed", 0);
                else
                    Show_Message(sType + " Duplicate" + Environment.NewLine
                                + "Line:" + dsTemp.Tables[0].Rows[0]["PDLINE_NAME"].ToString(), 0);
                return false;
            }
            return true;
        }
        public bool Check_BoxDup(string sCarton, string sBox)
        {
            sSQL = @"SELECT
                    SERIAL_NUMBER
                FROM
                    SAJET.G_SN_STATUS
                WHERE
                        BOX_NO = '" + sBox + @"'
                    AND CARTON_NO = '" + sCarton + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                Show_Message("Box Duplicate", 0);
                return false;
            }
            return true;
        }
        public bool Check_UDI(string sType, string sValue, bool bAutoCatronNumber, bool bAutoCartoonCode, bool bAutoBoxCode)
        {
            //MessageBox.Show(sType);
            //MessageBox.Show(g_sCarton_New);
            switch (sType)
            {
                case "PALLET":
                    //增加檢查UDI
                    object[][] Params = new object[3][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TWO", editWO.Text };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TCATRONNUMBER", sValue };
                    Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                    DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_UDI_CHK_CARTON_NUMBER", Params);
                    if (ds.Tables[0].Rows[0]["TRES"].ToString() != "OK")
                    {
                        Show_Message(ds.Tables[0].Rows[0]["TRES"].ToString(), 0);
                        return false;
                    }
                    break;
                case "CARTON":
                    //增加檢查UDI
                    object[][] Params1 = new object[5][];
                    Params1[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TWO", editWO.Text };
                    Params1[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TCATRONNUMBER", g_sPallet };
                    Params1[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TCARTOONCODE", sValue };
                    Params1[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TAUTOCATRONNUMBER", bAutoCatronNumber.ToString() };
                    Params1[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                    DataSet ds1 = ClientUtils.ExecuteProc("SAJET.SJ_UDI_CHK_CARTOON_CODE", Params1);
                    if (ds1.Tables[0].Rows[0]["TRES"].ToString() != "OK")
                    {
                        Show_Message(ds1.Tables[0].Rows[0]["TRES"].ToString(), 0);
                        return false;
                    }
                    break;
                case "BOX":
                    //增加檢查UDI  editCarton.Text
                    object[][] Params2 = new object[7][];
                    Params2[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TWO", editWO.Text };
                    Params2[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TCATRONNUMBER", g_sPallet };
                    Params2[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TCARTOONCODE", g_sCarton_New };

                    Params2[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TBOXCODE", sValue };
                    Params2[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TAUTOCATRONNUMBER", bAutoCatronNumber.ToString() };
                    Params2[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TAUTOCARTOONCODE", bAutoCartoonCode.ToString() };
                    Params2[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                    DataSet ds2 = ClientUtils.ExecuteProc("SAJET.SJ_UDI_CHK_BOX_CODE", Params2);
                    if (ds2.Tables[0].Rows[0]["TRES"].ToString() != "OK")
                    {
                        Show_Message(ds2.Tables[0].Rows[0]["TRES"].ToString(), 0);
                        return false;
                    }
                    break;
                case "SN": 
                    //增加檢查UDI
                    object[][] Params3 = new object[10][];
                    Params3[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TWO", editWO.Text };
                    Params3[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TCATRONNUMBER", g_sPallet };
                    Params3[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TCARTOONCODE", g_sCarton };
                    Params3[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TBOXCODE", g_sBox };
                    Params3[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TXTCODE", sValue };
                    Params3[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TAUTOCATRONNUMBER", bAutoCatronNumber.ToString() };
                    Params3[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TAUTOCARTOONCODE", bAutoCartoonCode.ToString() };
                    Params3[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TAUTOBOXCODE", bAutoBoxCode.ToString() };
                    Params3[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TBOXQTY", LabBoxCapacity.Text.ToString() };
                    Params3[9] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                    DataSet ds3 = ClientUtils.ExecuteProc("SAJET.SJ_UDI_CHK_XT_CODE", Params3);
                    if (ds3.Tables[0].Rows[0]["TRES"].ToString() != "OK")
                    {
                        Show_Message(ds3.Tables[0].Rows[0]["TRES"].ToString(), 0);
                        return false;
                    }
                    break;
            }
            return true;
        }

        public bool Check_Rule(string sLabelType, string sInputNo)
        {
            //Option有設Rule Function時,由Function來檢查
            if (!string.IsNullOrEmpty(TSetup.sRuleFun))
            {
                sSQL = "select " + TSetup.sRuleFun + "('" + sLabelType + "','" + sInputNo + "') result from dual ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    string sResult = dsTemp.Tables[0].Rows[0]["result"].ToString();
                    if (sResult != "OK")
                    {
                        Show_Message(sResult, 0);
                        return false;
                    }
                }
            }
            else
            {
                object[] objData = new object[3];
                string[] sParam = new string[1];

                //(呼叫LabelCheckDll.dll)              
                LabelCheck.Check LabelCheckDll = new LabelCheck.Check();

                //找出編碼規則內容
                //2013/7/15 如果為CSN，Get_RuleData會找不到，sLabelType要改成Customer SN
                string sTempLabelType = "";
                if (sLabelType == "CSN")
                    sTempLabelType = "Customer SN";
                else
                    sTempLabelType = sLabelType;

                bool bRuleExist = LabelCheckDll.Get_RuleData(sTempLabelType, editWO.Text, ref sParam, ref objData);

                if (bRuleExist)
                {
                    //檢查產生的號碼是否符合規則          
                    string sResult = LabelCheckDll.CheckRule_NewNo(sInputNo, sParam, objData, false);
                    if (sResult != "OK")
                    {
                        Show_Message(sResult, 0);
                        return false;
                    }
                }
                else //2013/7/15 手動輸入也要檢查規則
                {
                    //if (TOption[0].g_bInput)
                    //{
                    //    Show_Message(SajetCommon.SetLanguage("No CSN Rule"), 0);
                    //    return false;
                    //}
                }
            }
            return true;
        }

        public bool Check_ReleaseNo(string sLabelType, string sInputNo)
        {
            //檢查是否已在BarcodeCenter中展開
            sSQL = "select * from sajet.sys_label "
                 + "where Label_Name = '" + sLabelType + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                Show_Message(sLabelType + " - Label has not defined", 0);
                return false;
            }
            string sTable = dsTemp.Tables[0].Rows[0]["TABLE_NAME"].ToString();
            string sField = dsTemp.Tables[0].Rows[0]["FIELD_NAME"].ToString();
            sSQL = "select " + sField + " from " + sTable + " "
                 + "where " + sField + "='" + sInputNo + "' "
                 + "and work_order = '" + editWO.Text + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                Show_Message(SajetCommon.SetLanguage(sLabelType) + " " + SajetCommon.SetLanguage("doesn't exist(release)"), 0);
                return false;
            }
            return true;
        }

        public bool Check_SN()
        {
            try
            {
                object[][] Params = new object[14][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPACKTYPE", TSetup.sPKBase };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTERMINALID", g_sTerminalID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREV", editSN.Text };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TWO", editWO.Text };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPARTNO", LabPart.Text };
                Params[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                Params[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TTYPE", "" };
                Params[7] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TSN", "" };
                Params[8] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TCSN", "" };
                Params[9] = new object[] { ParameterDirection.InputOutput, OracleType.VarChar, "TBOX", g_sBox };
                Params[10] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TCARTON", "" };
                Params[11] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TPALLET", "" };
                Params[12] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TINNERBOX", "" };
                Params[13] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TOTHER_WO_UDI", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_PACKING_CHK_SN", Params);
                if (ds.Tables[0].Rows[0]["TRES"].ToString() != "OK")
                {
                    Show_Message(ds.Tables[0].Rows[0]["TRES"].ToString(), 0);
                    return false;
                }
                if (ds.Tables[0].Rows[0]["TTYPE"].ToString() != "OUT")
                {
                    g_sSNType = ds.Tables[0].Rows[0]["TTYPE"].ToString();
                }

                if (ds.Tables[0].Rows[0]["TSN"].ToString() != "OUT")
                    g_sSN = ds.Tables[0].Rows[0]["TSN"].ToString();
                if (ds.Tables[0].Rows[0]["TCSN"].ToString() != "OUT")
                    g_sCSN = ds.Tables[0].Rows[0]["TCSN"].ToString();
                if (ds.Tables[0].Rows[0]["TPALLET"].ToString() != "OUT")
                    g_sOldPallet = ds.Tables[0].Rows[0]["TPALLET"].ToString();
                if (ds.Tables[0].Rows[0]["TCARTON"].ToString() != "OUT")
                    g_sOldCarton = ds.Tables[0].Rows[0]["TCARTON"].ToString();
                if (ds.Tables[0].Rows[0]["TBOX"].ToString() != "OUT")
                    g_sOldBox = ds.Tables[0].Rows[0]["TBOX"].ToString();
                if (ds.Tables[0].Rows[0]["TINNERBOX"].ToString() != "OUT")
                    g_sOldInnerBox = ds.Tables[0].Rows[0]["TINNERBOX"].ToString();
                g_sOtherWOUDI = ds.Tables[0].Rows[0]["TOTHER_WO_UDI"].ToString();
                return true;
            }
            catch (Exception ex)
            {
                Show_Message("SAJET.SJ_PACKING_CHK_SN" + Environment.NewLine + ex.Message, 0);
                return false;
            }
            //Check SN=========================================================
            /*try
            {
                object[][] Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREV", editSN.Text };
                Params[1] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "PSN", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_CKRT_SN_PSN", Params);

                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    Show_Message(sRes, 0);
                    editSN.SelectAll();
                    return false;
                }
                g_sSN = ds.Tables[0].Rows[0]["PSN"].ToString(); //PSN
            }
            catch (Exception ex)
            {
                Show_Message("SAJET.SJ_CKRT_SN_PSN" + Environment.NewLine + ex.Message, 0);
                return false;
            }

            //Check Route===================================================             
            try
            {
                object[][] Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TERMINALID", g_sTerminalID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", g_sSN };
                Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "PSN", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_CKRT_ROUTE", Params);

                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    Show_Message(sRes, 0);
                    editSN.SelectAll();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Show_Message("SAJET.SJ_CKRT_ROUTE" + Environment.NewLine + ex.Message, 0);
                return false;
            }

            //根據檢查工單或料號是否相同
            sSQL = "SELECT A.WORK_ORDER,B.PART_NO "
                 + "      ,A.CUSTOMER_SN,A.SERIAL_NUMBER "
                 + "      ,NVL(A.PALLET_NO,'N/A') PALLET_NO,NVL(A.CARTON_NO,'N/A') CARTON_NO,NVL(A.BOX_NO,'N/A') BOX_NO "                 
                 + "FROM SAJET.G_SN_STATUS A "
                 + "    ,SAJET.SYS_PART B "
                 + "WHERE A.SERIAL_NUMBER = '" + g_sSN + "' "
                 + "AND A.PART_ID = B.PART_ID(+) and rownum = 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            string sSNPart = dsTemp.Tables[0].Rows[0]["PART_NO"].ToString();
            string sSNWO = dsTemp.Tables[0].Rows[0]["WORK_ORDER"].ToString();
            g_sCSN = dsTemp.Tables[0].Rows[0]["CUSTOMER_SN"].ToString();
            g_sOldPallet = dsTemp.Tables[0].Rows[0]["PALLET_NO"].ToString();
            g_sOldCarton = dsTemp.Tables[0].Rows[0]["CARTON_NO"].ToString();
            g_sOldBox = dsTemp.Tables[0].Rows[0]["BOX_NO"].ToString();
            
            if (TSetup.sPKBase == "Work Order")
            {
                if (editWO.Text != sSNWO)
                {
                    Show_Message("Work Order is Different" + Environment.NewLine + sSNWO, 0);
                    return false;
                }
            }
            else
            {
                if (LabPart.Text != sSNPart)
                {
                    Show_Message("Part No is Different" + Environment.NewLine + sSNPart, 0);
                    return false;
                }
            }
            return true;*/
        }

        public bool F_CHECK_DUP_NO()
        {
            if (gbPallet.Enabled)
            {
                sSQL = "SELECT BOX_NO,CARTON_NO,PALLET_NO "
                     + "FROM SAJET.G_SN_STATUS "
                     + " Where CARTON_NO = '" + editCarton.Text + "' "
                     + " AND WORK_ORDER = '" + editWO.Text.Trim() + "' "
                     + " and rownum = 1 ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    if (editPallet.Text != dsTemp.Tables[0].Rows[0]["PALLET_NO"].ToString())
                    {
                        Show_Message(SajetCommon.SetLanguage("Carton in other Pallet", 1) + " (" + dsTemp.Tables[0].Rows[0]["PALLET_NO"].ToString() + ")", 0);
                        return false;
                    }
                }
            }

            if (gbBox.Enabled & gbCarton.Enabled)
            {
                sSQL = "SELECT BOX_NO,CARTON_NO,PALLET_NO "
                     + "FROM SAJET.G_SN_STATUS "
                     + " Where BOX_NO = '" + editBox.Text + "' "
                     + " AND WORK_ORDER = '" + editWO.Text.Trim() + "' "
                     + " and rownum = 1 ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    if (editCarton.Text != dsTemp.Tables[0].Rows[0]["CARTON_NO"].ToString())
                    {
                        Show_Message(SajetCommon.SetLanguage("Box in other Carton", 1) + " (" + dsTemp.Tables[0].Rows[0]["CARTON_NO"].ToString() + ")", 0);
                        return false;
                    }
                }
            }
            return true;
        }

        public bool Check_Defect(string sDefectCode)
        {
            sSQL = " Select DEFECT_CODE,DEFECT_ID,DEFECT_DESC "
                 + " From SAJET.SYS_DEFECT "
                 + " Where DEFECT_CODE = '" + sDefectCode + "' "
                 + " and ENABLED = 'Y'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if ((LVEC.Items.Count == 0) || (LVEC.FindItemWithText(sDefectCode, false, 0) == null))
                {
                    LVEC.Items.Add(dsTemp.Tables[0].Rows[0]["DEFECT_CODE"].ToString());
                    LVEC.Items[LVEC.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["DEFECT_DESC"].ToString());
                    LVEC.Items[LVEC.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["DEFECT_ID"].ToString());
                }
                else
                {
                    Show_Message(SajetCommon.SetLanguage("Defect Code Duplicate", 1), 0);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Input_ErrorSN()
        {
            DateTime dtNow = ClientUtils.GetSysDate();

            //過站====SAJET.SJ_NOGO=====    
            string sRemoveCSN = "N";
            if (TSetup.bRemoveCSN)
                sRemoveCSN = "Y";
            object[][] Params = new object[10][];
            for (int i = 0; i <= LVEC.Items.Count - 1; i++)
            {
                try
                {
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTYPE", g_sSNType };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTERMINALID", g_sTerminalID };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", g_sSN };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREV", editSN.Text };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TDEFECT", LVEC.Items[i].Text };
                    Params[5] = new object[] { ParameterDirection.Input, OracleType.DateTime, "TNOW", dtNow };
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMP", g_sUserNo };
                    Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREMOVECSN", sRemoveCSN };
                    Params[8] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                    Params[9] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TNEXTPROC", "" };
                    DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_PACKING_NOGO", Params);
                    string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                    if (sRes != "OK")
                    {
                        Show_Message(sRes, 0);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Show_Message("SAJET.SJ_PACKING_NOGO" + Environment.NewLine + ex.Message, 0);
                    return false;
                }
            }
            return true;
        }
        private bool Input_CustItem(string sCSN)
        {
            PackingCustItemDll.fMain fData = new PackingCustItemDll.fMain();
            fData.g_sProcessID = g_sProcessID;
            fData.g_sTerminalID = g_sTerminalID;
            fData.g_sWO = LabWo.Text;
            fData.g_sSN = g_sSN;
            fData.g_sProgram = g_sProgram;
            fData.g_sUserNo = g_sUserNo;
            fData.g_sUserID = g_sUserID;

            if (fData.CheckShowOption())
            {
                if (fData.ShowDialog() == DialogResult.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }

        }
        private bool Input_Assembly()
        {
            bool bHasKP = false;
            string sSQL = "SELECT COUNT(*) QTY "
                       + "  FROM SAJET.G_WO_BOM "
                       + " WHERE WORK_ORDER =:WORK_ORDER "
                       + "   AND PROCESS_ID =:PROCESS_ID ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", LabWo.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", g_sProcessID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["QTY"].ToString()) > 0)
                {
                    bHasKP = true;
                }
            }
            if (bHasKP)
            {
                AssemblyDll.fMain fData = new AssemblyDll.fMain();
                fData.g_sProcessID = g_sProcessID;
                fData.g_sTerminalID = g_sTerminalID;
                fData.g_sWO = LabWo.Text;
                fData.G_sDisplayType = "DLL";
                fData.g_sSN = g_sSN;
                fData.g_sUserNo = g_sUserNo;
                fData.g_sUserID = g_sUserID;
                if (fData.ShowDialog() == DialogResult.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public bool Input_SN()
        {
            if (gbPallet.Enabled && string.IsNullOrEmpty(g_sPallet))
            {
                string sMsg = SajetCommon.SetLanguage("Data is null") + Environment.NewLine
                            + SajetCommon.SetLanguage("Pallet No");
                Show_Message(sMsg, 0);
                return false;
            }
            if (gbCarton.Enabled && string.IsNullOrEmpty(g_sCarton))
            {
                string sMsg = SajetCommon.SetLanguage("Data is null") + Environment.NewLine
                            + SajetCommon.SetLanguage("Carton No");
                Show_Message(sMsg, 0);
                return false;
            }
            if (TSetup.iPKAction == 9)
            {
                if (gbBox.Enabled && string.IsNullOrEmpty(g_sInnerBox))
                {
                    string sMsg = SajetCommon.SetLanguage("Data is null") + Environment.NewLine
                                + SajetCommon.SetLanguage("Inner Box");
                    Show_Message(sMsg, 0);
                    return false;
                }
            }
            else
            {
                if (gbBox.Enabled && string.IsNullOrEmpty(g_sBox))
                {
                    string sMsg = SajetCommon.SetLanguage("Data is null") + Environment.NewLine
                                + SajetCommon.SetLanguage("Box No");
                    Show_Message(sMsg, 0);
                    return false;
                }
            }

            sSQL = "Select Sysdate from dual ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            DateTime dtNow = (DateTime)dsTemp.Tables[0].Rows[0]["SYSDATE"];
            //
            string S = "023478";
            if (S.IndexOf(TSetup.iPKAction.ToString()) != -1)
            {
                if (TSetup.iPKAction == 3 || TSetup.iPKAction == 8)
                {
                    g_sPallet = g_sCarton;
                }
                Append_PackNo("Pallet", g_sPallet);
            }
            if (TSetup.iPKAction != 2 && TSetup.iPKAction != 4 && TSetup.iPKAction != 9)
            {
                if (TSetup.iPKAction != 5)
                {
                    Append_PackNo("Carton", g_sCarton);
                }
                S = "678";
                if (S.IndexOf(TSetup.iPKAction.ToString()) == -1)
                {
                    Append_PackNo("Box", g_sBox);
                }
            }
            else if (TSetup.iPKAction == 9)
            {
                Append_PackNo("InnerBox", g_sInnerBox);
            }


            //過站====SAJET.SJ_PACKING_GO=====               
            try
            {
                object[][] Params = new object[12][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTYPE", g_sSNType };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTERMINALID", g_sTerminalID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", g_sSN };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "TNOW", dtNow };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMP", g_sUserNo };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PACKACTION", TSetup.iPKAction.ToString() };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PALLETNO", g_sPallet };
                Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CARTONNO", g_sCarton };
                Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOXNO", g_sBox };
                Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMERSN", g_sCSN };
                Params[10] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                if (g_sSNType == "SN")
                    Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "INNERBOXNO", g_sInnerBox };
                else
                    Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "INNERBOXNO", editSN.Text };

                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_PACKING_GO", Params);
                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    Show_Message(SajetCommon.SetLanguage(sRes), 0);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Show_Message("SAJET.SJ_PACKING_GO" + Environment.NewLine + ex.Message, 0);
                return false;
            }

            //列印CSN
            if (TSetup.iPKAction != 2 && TSetup.iPKAction != 4)
            {
                if (TOption[0].g_bPrint)
                {
                    Print_Label(0, g_sSN);
                }
            }
            return true;
        }

        public bool Input_SN_BOX()
        {
            if (gbPallet.Enabled && string.IsNullOrEmpty(g_sPallet))
            {
                string sMsg = SajetCommon.SetLanguage("Data is null") + Environment.NewLine
                            + SajetCommon.SetLanguage("Pallet No");
                Show_Message(sMsg, 0);
                return false;
            }
            if (gbCarton.Enabled && string.IsNullOrEmpty(g_sCarton))
            {
                string sMsg = SajetCommon.SetLanguage("Data is null") + Environment.NewLine
                            + SajetCommon.SetLanguage("Carton No");
                Show_Message(sMsg, 0);
                return false;
            }
            if (TSetup.iPKAction == 9)
            {
                if (gbBox.Enabled && string.IsNullOrEmpty(g_sInnerBox))
                {
                    string sMsg = SajetCommon.SetLanguage("Data is null") + Environment.NewLine
                                + SajetCommon.SetLanguage("Inner Box");
                    Show_Message(sMsg, 0);
                    return false;
                }
            }
            else
            {
                if (gbBox.Enabled && string.IsNullOrEmpty(g_sBox))
                {
                    string sMsg = SajetCommon.SetLanguage("Data is null") + Environment.NewLine
                                + SajetCommon.SetLanguage("Box No");
                    Show_Message(sMsg, 0);
                    return false;
                }
            }

            sSQL = "Select Sysdate from dual ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            DateTime dtNow = (DateTime)dsTemp.Tables[0].Rows[0]["SYSDATE"];
            //
            string S = "023478";
            if (S.IndexOf(TSetup.iPKAction.ToString()) != -1)
            {
                if (TSetup.iPKAction == 3 || TSetup.iPKAction == 8)
                {
                    g_sPallet = g_sCarton;
                }
                Append_PackNo("Pallet", g_sPallet);
            }
            if (TSetup.iPKAction != 2 && TSetup.iPKAction != 4 && TSetup.iPKAction != 9)
            {
                if (TSetup.iPKAction != 5)
                {
                    Append_PackNo("Carton", g_sCarton);
                }
                S = "678";
                if (S.IndexOf(TSetup.iPKAction.ToString()) == -1)
                {
                    Append_PackNo("Box", g_sBox);
                }
            }
            else if (TSetup.iPKAction == 9)
            {
                Append_PackNo("InnerBox", g_sInnerBox);
            }

            return true;
        }
        private void btnUndo_Click(object sender, EventArgs e)
        {
            ClearData();
            SetEditFocus("WO");
        }

        private void editSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            editSN.Text = editSN.Text.Trim();
            if (editSN.Text.ToUpper() == "UNDO")
            {
                LVEC.Items.Clear();
                editCSN.Text = "";
                Show_Message(SajetCommon.SetLanguage("UNDO OK"), 3);
                SetEditFocus("SN");
                return;
            }
            //可輸入不良代碼,檢查輸入的是否為Defect
            if (TSetup.bInputEC)
            {
                if (Check_Defect(editSN.Text))
                {
                    editCSN.Text = "";
                    SetEditFocus("SN");
                    return;
                }
            }
            //檢查UDI           
            if (!Check_UDI("SN", editSN.Text, TOption[3].g_bSysCreate, TOption[2].g_bSysCreate, TOption[1].g_bSysCreate))
            {
                SetEditFocus("SN");
                return;
            }
            if (!Check_SN())
            {
                SetEditFocus("SN");
                return;
            }
            //檢查UDI           
            if (!Check_UDI("SN", editSN.Text, TOption[3].g_bSysCreate, TOption[2].g_bSysCreate, TOption[1].g_bSysCreate))
            {
                SetEditFocus("SN");
                return;
            }
            //若為不良品直接過站
            if (LVEC.Items.Count > 0)
            {
                if (Input_ErrorSN())
                {
                    LVEC.Items.Clear();
                    Show_Message(SajetCommon.SetLanguage("SN OK"), 3);
                }
                SetEditFocus("SN");
                return;
            }

            //若SN原本已有Pallet and Carton,直接用原號碼過站(需放回原箱原棧板)
            if (g_sOldPallet != "N/A" && g_sOldCarton != "N/A")
            {
                string sMsg = g_sSNType + ": " + editSN.Text + " OK " + Environment.NewLine
                            + "Pallet No : " + g_sOldPallet + Environment.NewLine
                            + "Carton No : " + g_sOldCarton;
                if ((!gbBox.Enabled) || (gbBox.Enabled && g_sOldBox != "N/A"))
                {
                    if (gbBox.Enabled)
                        sMsg = sMsg + Environment.NewLine + "Box No : " + g_sOldBox;

                    TextMsg.Text = sMsg;
                    TextMsg.ForeColor = Color.FromArgb(255, 255, 128);
                    TextMsg.BackColor = Color.Maroon;
                    if (g_sSNType == "SN")
                        PackGo(g_sSN, "SN");
                    else
                        PackGo(g_sOldInnerBox, "INNERBOX");
                    SetEditFocus("SN");
                }
                return;
            }

            //重新Refresh Qty,避免兩台同時作業
            if (g_bRefreshQty)
            {
                if (!Refresh_PalletQty(g_sPallet))
                    return;
                if (!Refresh_CartonQty(g_sCarton))
                    return;
            }

            //檢查各數量是否已滿
            if (LabBoxQty.Text != "" && LabBoxQty.Enabled)
            {
                if (g_bRefreshQty)
                {
                    if (TSetup.iPKAction == 9)
                    {
                        if (!Refresh_InnerBoxQty(g_sInnerBox))
                            return;
                    }
                    else
                    {
                        if (!Refresh_BoxQty(g_sBox))
                            return;
                    }
                }
                if (Convert.ToInt32(LabBoxCapacity.Text) <= Convert.ToInt32(LabBoxQty.Text))
                {
                    Show_Message(SajetCommon.SetLanguage("Please Close Box", 1), 0);
                    return;
                }
            }

            string S = "2459";
            if (S.IndexOf(TSetup.iPKAction.ToString()) == -1)
            {
                if (LabCartonQty.Text != "")
                {
                    if (Convert.ToInt32(LabCartonCapacity.Text) <= Convert.ToInt32(LabCartonQty.Text))
                    {
                        Show_Message(SajetCommon.SetLanguage("Please Close Carton", 1), 0);
                        return;
                    }
                }
            }
            S = "135689";
            if (S.IndexOf(TSetup.iPKAction.ToString()) == -1)
            {
                if (LabPalletQty.Text != "")
                {
                    if (Convert.ToInt32(LabPalletCapacity.Text) <= Convert.ToInt32(LabPalletQty.Text))
                    {
                        Show_Message(SajetCommon.SetLanguage("Please Close Pallet", 1), 0);
                        return;
                    }
                }
            }
            //避免同一個Carton同時被包進兩個不同的Pallet
            if (!F_CHECK_DUP_NO())
            {
                return;
            }

            //組裝配件功能
            bool bReturn = false;
            if (g_sSNType == "SN")
            {
                //CSN=================            
                //System Create
                if (TOption[0].g_bSysCreate)
                {
                    if (!Get_NewNo("CSN", out g_sCSN))
                        return;
                    g_sCSN = Get_NextNewNo("CSN", g_sCSN);  //檢查自動產生的號碼是否已重複
                    editCSN.Text = g_sCSN;
                }
                //NotChangeCSN
                else if (TOption[0].g_bNotChange)
                {
                    editCSN.Text = g_sCSN;
                }
                //CSN=SN
                else if (TOption[0].g_bSameSN)
                {
                    g_sCSN = g_sSN;
                    editCSN.Text = g_sCSN;
                }
                else
                {
                    SetEditFocus("CSN");
                    Show_Message(SajetCommon.SetLanguage("SN OK"), 3);
                    bReturn = true;
                }
            }

            //======================
            //20250522檢查組裝註解
            //if (!Input_Assembly())
            //{
            //    SetEditFocus("SN");
            //    bReturn = true;
            //}

            //if (bReturn)
            //    return;


            if (File.Exists(Application.StartupPath + "\\" + g_sExeName + "\\PackingCustItemDll.dll"))
            {
                if (!Input_CustItem(editCSN.Text))
                {
                    SetEditFocus("SN");
                    return;

                }
            }

            if (g_sOtherWOUDI == "N")
            {
                if (!Input_SN())
                {
                    SetEditFocus("SN");
                    return;
                }
            }
            else
            {
                if (!Input_SN_BOX())
                {
                    SetEditFocus("SN");
                    return;
                }
            }

            if (!LabBoxCapacity.Enabled)
            {
                LabCartonQty.Text = Convert.ToString((Convert.ToInt32(LabCartonQty.Text)) + 1);
                if (Convert.ToInt32(LabCartonCapacity.Text) <= Convert.ToInt32(LabCartonQty.Text))
                    Close_Carton(g_sCarton, "N");
                else
                    SetEditFocus("SN");
            }
            else
            {
                LabBoxQty.Text = Convert.ToString((Convert.ToInt32(LabBoxQty.Text)) + 1);
                if (Convert.ToInt32(LabBoxCapacity.Text) <= Convert.ToInt32(LabBoxQty.Text))
                {
                    if (TSetup.iPKAction == 9)
                        Close_InnerBox(g_sInnerBox, "N");
                    else
                    {
                        if (g_sOtherWOUDI == "Y")
                        {
                            if (!Input_SN())
                            {
                                SetEditFocus("SN");
                                return;
                            }
                        }
                        Close_Box(g_sBox, "N");
                    }
                }
                else
                    SetEditFocus("SN");
            }

            Show_Message(SajetCommon.SetLanguage("SN OK"), 3);
        }

        private void editCSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editCSN.Text = editCSN.Text.Trim();
            if (editCSN.Text == "")
            {
                string sMsg = SajetCommon.SetLanguage("Data is null") + Environment.NewLine
                            + labCustSN.Text;
                Show_Message(sMsg, 0);
                SetEditFocus("CSN");
                return;
            }
            //先檢查是否要與SN相同
            if (TOption[0].g_bCheckSameSN)
            {
                if (editCSN.Text != editSN.Text)
                {
                    Show_Message(SajetCommon.SetLanguage("Customer SN") + " <> " + SajetCommon.SetLanguage("Serial Number", 1), 0);
                    SetEditFocus("CSN");
                    return;
                }
            }
            //20130515 modified by sharon 
            //修改 檢查是否符合編碼規則
            else if (TOption[0].g_bInputRealease)
            {
                //檢查是否已由BarcodeCenter展開
                if (!Check_ReleaseNo("Customer SN", editCSN.Text))
                {
                    SetEditFocus("CSN");
                    return;
                }
            }
            else
            {
                //檢查是否符合編碼規則
                if (!Check_Rule("CSN", editCSN.Text))
                {
                    SetEditFocus("CSN");
                    return;
                }
            }

            //檢查是否重複
            if (!Check_Dup_CSN(editSN.Text))
            {
                SetEditFocus("CSN");
                return;
            }
            g_sCSN = editCSN.Text;

            if (File.Exists(Application.StartupPath + "\\" + g_sExeName + "\\PackingCustItemDll.dll"))
            {
                //畫面上的CSN雖然還沒儲存但也要檢查所以當變數傳進去
                if (!Input_CustItem(g_sCSN))
                {
                    SetEditFocus("CSN");
                    return;

                }
            }


            if (!Input_SN())
            {
                SetEditFocus("SN");
                return;
            }

            Show_Message(SajetCommon.SetLanguage("CSN OK"), 3);


            if (!LabBoxCapacity.Enabled)
            {
                LabCartonQty.Text = Convert.ToString((Convert.ToInt32(LabCartonQty.Text)) + 1);
                if (Convert.ToInt32(LabCartonCapacity.Text) <= Convert.ToInt32(LabCartonQty.Text))
                    Close_Carton(g_sCarton, "N");
                else
                    SetEditFocus("SN");
            }
            else
            {
                LabBoxQty.Text = Convert.ToString((Convert.ToInt32(LabBoxQty.Text)) + 1);
                if (Convert.ToInt32(LabBoxCapacity.Text) <= Convert.ToInt32(LabBoxQty.Text))
                    Close_Box(g_sBox, "N");
                else
                    SetEditFocus("SN");
            }
        }

        public bool Check_Dup_CSN(string sSN)
        {
            sSN = g_sSN;
            g_sCSN = "";
            sSQL = " SELECT SERIAL_NUMBER "
                + " FROM SAJET.G_SN_STATUS "
                + " WHERE CUSTOMER_SN = '" + editCSN.Text + "' and rownum = 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                //Modified by Sharon 2013/7/12
                //如果重覆的CSN就是本身SN的CSN，即可過站
                string sTempSN = dsTemp.Tables[0].Rows[0]["SERIAL_NUMBER"].ToString();
                if (sSN == sTempSN)
                    return true;

                Show_Message(SajetCommon.SetLanguage("Customer SN Duplicate", 1), 0);
                return false;
            }

            sSQL = " SELECT SERIAL_NUMBER "
                 + " FROM SAJET.G_SN_CSN "
                 + " WHERE CUSTOMER_SN = '" + editCSN.Text + "' and rownum = 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                //Modified by Sharon 2013/7/12
                //如果重覆的CSN就是本身SN的CSN，即可過站
                string sTempSN = dsTemp.Tables[0].Rows[0]["SERIAL_NUMBER"].ToString();
                if (sSN == sTempSN)
                    return true;

                Show_Message(SajetCommon.SetLanguage("Customer SN Duplicate", 1), 0);
                return false;
            }

            return true;
        }

        private void LabChangSpec_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (editWO.Text == "")
                return;
            if (editWO.Enabled)
                return;

            fPKSpec fSpec = new fPKSpec();
            try
            {
                fSpec.LabWO.Text = editWO.Text;
                fSpec.LabPartNo.Text = LabPart.Text;
                if (fSpec.ShowDialog() != DialogResult.OK)
                    return;

                Update_PackTerminal();
                KeyPressEventArgs key = new KeyPressEventArgs((char)Keys.Return);
                editWO_KeyPress(sender, key);
            }
            finally
            {
                fSpec.Dispose();
            }
        }

        private bool Check_Privilege_Close()
        {
            //Close Pallet(Carton)權限
            string sEmpID = "0";
            fPasswd fPassword = new fPasswd();
            if (fPassword.ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            sEmpID = fPasswd.g_sEMP_ID;
            fPassword.Dispose();
            int iClose_Privilege = ClientUtils.GetPrivilege(sEmpID, "Close Pallet(Carton)", g_sProgram);
            if (iClose_Privilege >= 1)
                return true;
            else
            {
                Show_Message(SajetCommon.SetLanguage("No Privilege to Close", 1), 0);
                return false;
            }
        }

        private bool Refresh_PalletQty(string sPallet)
        {
            sSQL = "SELECT CLOSE_FLAG "
                 + " FROM SAJET.G_PACK_PALLET "
                 + " WHERE PALLET_NO = '" + sPallet + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (dsTemp.Tables[0].Rows[0]["CLOSE_FLAG"].ToString() == "Y")
                {
                    Show_Message(SajetCommon.SetLanguage("Pallet had been Closed", 1), 0);
                    SetEditFocus("PALLET");
                    return false;
                }
            }

            sSQL = "SELECT A.CARTON_NO "
                 + "FROM SAJET.G_SN_STATUS A,SAJET.G_PACK_CARTON B "
                 + "WHERE A.PALLET_NO = '" + sPallet + "' "
                 + "AND A.CARTON_NO = B.CARTON_NO "
                 + "AND B.CLOSE_FLAG = 'Y' "
                 + "GROUP BY A.CARTON_NO ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            LabPalletQty.Text = dsTemp.Tables[0].Rows.Count.ToString();

            return true;
        }

        private bool Refresh_CartonQty(string sCarton)
        {
            sSQL = "SELECT CLOSE_FLAG "
                 + " FROM SAJET.G_PACK_CARTON "
                 + " WHERE CARTON_NO = '" + sCarton + "' "
                 + " AND WORK_ORDER = '" + editWO.Text.Trim() + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (dsTemp.Tables[0].Rows[0]["CLOSE_FLAG"].ToString() == "Y" && sCarton != "Remainder box")
                {
                    Show_Message(SajetCommon.SetLanguage("Carton had been Closed", 1), 0);
                    SetEditFocus("CARTON");
                    return false;
                }
            }
            if (gbBox.Enabled)
            {
                sSQL = " SELECT A.BOX_NO "
                     + " FROM SAJET.G_SN_STATUS A "
                     + "     ,SAJET.G_PACK_BOX B "
                     + " WHERE CARTON_NO = '" + sCarton + "' "
                     + " AND A.BOX_NO = B.BOX_NO(+) "
                     + " AND B.CLOSE_FLAG = 'Y' "
                     + " AND A.WORK_ORDER = '" + editWO.Text.Trim() + "' "
                     + " GROUP BY A.BOX_NO, B.CLOSE_FLAG ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                LabCartonQty.Text = dsTemp.Tables[0].Rows.Count.ToString();

            }
            else
            {
                sSQL = "SELECT COUNT(*) CNT "
                     + "FROM SAJET.G_SN_STATUS "
                     + "WHERE CARTON_NO = '" + sCarton + "' "
                     + " AND A.WORK_ORDER = '" + editWO.Text.Trim() + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                LabCartonQty.Text = dsTemp.Tables[0].Rows[0]["CNT"].ToString();
            }
            return true;
        }

        private bool Refresh_BoxQty(string sBox)
        {
            sSQL = "SELECT CLOSE_FLAG "
                 + " FROM SAJET.G_PACK_Box "
                 + " WHERE BOX_NO = '" + sBox + "' "
                 + " AND WORK_ORDER = '" + editWO.Text.Trim() + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (dsTemp.Tables[0].Rows[0]["CLOSE_FLAG"].ToString() == "Y" && sBox != "Remainder box")
                {
                    Show_Message(SajetCommon.SetLanguage("Box had been Closed", 1), 0);
                    SetEditFocus("BOX");
                    return false;
                }
            }

            decimal dBoxCount = 0;
            sSQL = "SELECT COUNT(*) CNT "
                 + "FROM SAJET.G_SN_STATUS "
                 + "WHERE BOX_NO = '" + sBox + "' "
                 + " AND WORK_ORDER = '" + editWO.Text.Trim() + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            dBoxCount = dBoxCount + decimal.Parse(dsTemp.Tables[0].Rows[0]["CNT"].ToString());
            sSQL = "SELECT COUNT(*) CNT "
                 + "FROM SAJET.G_PACK_BOX_OUT "
                 + "WHERE BOX_NO = '" + sBox + "' "
                 + " AND WORK_ORDER = '" + editWO.Text.Trim() + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            dBoxCount = dBoxCount + decimal.Parse(dsTemp.Tables[0].Rows[0]["CNT"].ToString());
            LabBoxQty.Text = dBoxCount.ToString();

            return true;
        }
        private bool Refresh_InnerBoxQty(string sBox)
        {
            sSQL = "SELECT CLOSE_FLAG "
                 + " FROM SAJET.G_PACK_INNERBOX "
                 + " WHERE INNERBOX_NO = '" + sBox + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (dsTemp.Tables[0].Rows[0]["CLOSE_FLAG"].ToString() == "Y")
                {
                    Show_Message(SajetCommon.SetLanguage("Inner Box had been Closed", 1), 0);
                    SetEditFocus("BOX");
                    return false;
                }
            }

            sSQL = "SELECT COUNT(*) CNT "
                 + "FROM SAJET.G_SN_STATUS "
                 + "WHERE INNERBOX_NO = '" + sBox + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            LabBoxQty.Text = dsTemp.Tables[0].Rows[0]["CNT"].ToString();

            return true;
        }

        private void Update_PackTerminal()
        {
            //紀錄最後一次使用的包裝方式,供下次開啟程式時使用
            sSQL = " select pkspec_id "
                 + " from sajet.G_PACK_SPEC_TERMINAL "
                 + " Where TERMINAL_ID = '" + g_sTerminalID + "' ";
            if (TSetup.sPKBase == "Work Order")
                sSQL = sSQL + " AND WORK_ORDER = '" + editWO.Text + "' ";
            else
                sSQL = sSQL + " AND PART_ID = '" + g_sPartID + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                sSQL = " insert into sajet.G_PACK_SPEC_TERMINAL "
                     + " (work_order, part_id, terminal_id, create_emp_id, pkspec_id) "
                     + " values "
                     + " ('" + editWO.Text + "', '" + g_sPartID + "', '" + g_sTerminalID + "', '" + g_sUserID + "', '" + g_sPKSpecID + "') ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
            else
            {
                sSQL = " update sajet.G_PACK_SPEC_TERMINAL "
                     + " set pkspec_id = '" + g_sPKSpecID + "' "
                     + "   , create_emp_id = '" + g_sUserID + "' "
                     + " Where terminal_id = '" + g_sTerminalID + "' ";
                if (TSetup.sPKBase == "Work Order")
                    sSQL = sSQL + " AND WORK_ORDER = '" + editWO.Text + "' ";
                else
                    sSQL = sSQL + " AND PART_ID = '" + g_sPartID + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
        }

        public void PackGo(string sValue, string sType)
        {
            sSQL = "Select Sysdate from dual ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            DateTime dtNow = (DateTime)dsTemp.Tables[0].Rows[0]["SYSDATE"];

            //====SAJET.SJ_PACKING_REPACK_GO=====               
            try
            {
                object[][] Params = new object[6][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTERMINALID", g_sTerminalID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "TNOW", dtNow };
                Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMP", g_sUserNo };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PACKACTION", sType };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PACKVALUE", sValue };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_PACKING_REPACK_GO", Params);
                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    Show_Message(sRes, 0);
                    return;
                }
            }
            catch (Exception ex)
            {
                Show_Message("SAJET.SJ_PACKING_REPACK_GO" + Environment.NewLine + ex.Message, 0);
                return;
            }

        }

        public bool Print_Label(int iType, string sNewNo)
        {
            string sFileFix = "";
            string sLabelType = "";
            switch (iType)
            {
                case 0:
                    sFileFix = "S_";
                    sLabelType = "CSN";
                    break;
                case 1:
                    sFileFix = "B_";
                    sLabelType = "BOX";
                    break;
                case 2:
                    sFileFix = "C_";
                    sLabelType = "CARTON";
                    break;
                case 3:
                    sFileFix = "P_";
                    sLabelType = "PALLET";
                    break;
                case 4:
                    sFileFix = "I_";
                    sLabelType = "INNERBOX";
                    break;
            }
            string sPrintMethod = TOption[iType].g_sPrintMethod.ToUpper();
            string sPrintPort = TOption[iType].g_sPrintPort.ToUpper();
            PrintLabel.Setup PrintLabelDll = new PrintLabel.Setup();
            if (TOption[iType].g_sPrintMethod.ToUpper() == "CODESOFT")
                PrintLabelDll.Open(TOption[iType].g_sPrintMethod.ToUpper()); //Link CodeSoft;
            else if (TOption[iType].g_sPrintMethod.ToUpper() == "BARTENDER" && TOption[iType].g_sPrintPort.ToUpper() == "STANDARD")
                PrintLabelDll.Open(TOption[iType].g_sPrintMethod.ToUpper());
            else
                PrintLabelDll.Open(TOption[iType].g_sPrintPort.ToUpper());

            try
            {
                string sMessage = "";
                ListParam.Items.Clear();
                ListData.Items.Clear();
                ListData.Items.Add(sNewNo);
                if (sPrintMethod == "BARTENDER" && sPrintPort == "DATASOURCE")
                {
                    PrintLabelDll.Print_Bartender_DataSource(g_sExeName, "PK_" + sLabelType, sFileFix, "", TOption[iType].g_iPrintQty, sPrintMethod, sPrintPort, ListParam, ListData, out sMessage);
                    if (sMessage != "OK")
                    {
                        SajetCommon.Show_Message(sMessage, 0);
                        return false;
                    }
                    return true;
                }

                //各變數值                        
                PrintLabelDll.GetPrintData("PK_" + sLabelType, ref ListParam, ref ListData);
                //開始列印
                if (TOption[iType].g_sPrintMethod.ToUpper() == "CODESOFT")
                    PrintLabelDll.Print(g_sExeName, "PK_" + sLabelType, sFileFix, "", TOption[iType].g_iPrintQty, TOption[iType].g_sPrintMethod, "6", ListParam, ListData, out sMessage);
                else
                    PrintLabelDll.Print(g_sExeName, "PK_" + sLabelType, sFileFix, "", TOption[iType].g_iPrintQty, TOption[iType].g_sPrintMethod, TOption[iType].g_sPrintPort, ListParam, ListData, out sMessage);
                if (sMessage != "OK")
                {
                    //Show_Message(sMessage, 0);
                    MessageBox.Show(sMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                return true;
            }
            finally
            {
                if (TOption[iType].g_sPrintMethod.ToUpper() == "CODESOFT")
                    PrintLabelDll.Close(TOption[iType].g_sPrintMethod.ToUpper());
                else if (sPrintMethod == "BARTENDER" && sPrintPort == "STANDARD")
                    PrintLabelDll.Close(sPrintMethod);
                else
                    PrintLabelDll.Close(TOption[iType].g_sPrintPort.ToUpper());
            }
        }

        private void editWO_EnabledChanged(object sender, EventArgs e)
        {
            btnSearchWO.Enabled = editWO.Enabled;
        }

        private void fMain_Shown(object sender, EventArgs e)
        {
            editWO.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (editWO.Text == "")
                return;
            if (editWO.Enabled)
                return;

            fPKSpec fSpec = new fPKSpec();
            try
            {
                fSpec.LabWO.Text = editWO.Text;
                fSpec.LabPartNo.Text = LabPart.Text;
                if (fSpec.ShowDialog() != DialogResult.OK)
                    return;

                Update_PackTerminal();
                KeyPressEventArgs key = new KeyPressEventArgs((char)Keys.Return);
                editWO_KeyPress(sender, key);
            }
            finally
            {
                fSpec.Dispose();
            }
        }

        private void gbBox_Enter(object sender, EventArgs e)
        {

        }

        private void btnChangeCarton_Click(object sender, EventArgs e)
        {
            SetEditFocus("CARTON");
            if (TOption[2].g_bSysCreate)
            {
                if (Create_NewCarton())
                {
                    //用BOX包Carton
                    if (TSetup.iPKAction >= 6 && TSetup.iPKAction <= 8)
                    {
                        SetEditFocus("BOX");
                    }
                    else
                    {
                        Show_Box();
                    }
                }
                else
                {
                    SetEditFocus("CARTON");
                }
            }
            else
            {
                SetEditFocus("CARTON");
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            //讀取SYS_BASE設定值
            string sMsg = "";
            string sPrintPort = SajetCommon.GetSysBaseData(g_sProgram, "PACKING PRINT PORT", ref sMsg);
            string sActionValue = SajetCommon.GetSysBaseData(g_sProgram, "Packing Action", ref sMsg);
            if (!string.IsNullOrEmpty(sMsg))
            {
                sMsg = $"{g_sProgram} Please Setup System Parameter:" + Environment.NewLine + Environment.NewLine + sMsg;
                SajetCommon.ShowMessage(sMsg);
                return;
            }

            fSettings frm = new fSettings(g_sProgram, g_sExeName, g_sTerminalID, g_sUserID)
            {
                sPrintPort = sPrintPort,
                sActionValue = sActionValue
            };

            if (frm.ShowDialog() == DialogResult.OK)
            {
                GetOptionData();
                if (!string.IsNullOrWhiteSpace(editWO.Text))
                {
                    KeyPressEventArgs key = new KeyPressEventArgs((char)Keys.Return);
                    editWO_KeyPress(sender, key);
                }
            }
        }

        private void btnClearBox_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(editBox.Text))
            {
                var _SQL = $@"DELETE FROM SAJET.G_PACK_BOX WHERE BOX_NO = '{editBox.Text}'";
                ClientUtils.ExecuteSQL(_SQL);

                _SQL = $@"DELTE FROM SAJET.G_PACK_BOX_OUT WHERE BOX_NO = '{editBox.Text}'";
                ClientUtils.ExecuteSQL(_SQL);

                MessageBox.Show("資料已清除");

                editWO_KeyPress(sender, null);
            }
        }
    }
}

