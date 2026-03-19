using ReceiveMaterial.Helper;
using ReceiveMaterial.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using Oracle.ManagedDataAccess.Client;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using ZXing;
using System.Data.OracleClient;
using ExcelDataReader.Log;

namespace ReceiveMaterial
{
	public partial class fMain : Form
	{
		private CommonHelper myHelper;

		private DataSet ds;

		private DataTable dt;
		public fMain()
		{
			InitializeComponent();
			
			myHelper = new CommonHelper();

			txt_Barcode.Focus();			
		}
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x02000000;
				return cp;
			}
		}
		private void fMain_Load(object sender, EventArgs e)
		{
			myHelper.SettingData(this.Controls);

			CenterControl(lb_Message, pn_Message);

			txt_Barcode.Focus();
		}
		private void pic_import_Click(object sender, EventArgs e)
		{			
			myHelper.ImportShipping();
			myHelper.GetDropDowndata(cb_FileChoice);
		}
		private void cb_FileChoice_DropDownClosed(object sender, EventArgs e)
		{
			txt_Barcode.Enabled = (cb_FileChoice.SelectedIndex == 0) ? false : true;

			txt_Barcode.Focus();

			ShowData();			
		}
		
		//private void cb_FileChoice_SelectedIndexChanged(object sender, EventArgs e)
		//{
		//	ShowData();
		//}
		public void ShowData() 
		{
			myHelper.g_SQL = $@"
			SELECT PART_NO,QTY,RECEIVE_QTY,TRANSFER_QTY,REMAIN_QTY,UOM,REFERENCE,BATCH AS SN,STATUS,DATATYPE,NO,NVL(WO,'NA') WO 
			,CREATEDATE,
			(SELECT EMP_NO FROM SAJET.SYS_EMP WHERE EMP_ID = CREATOR) EMP_NO
			FROM 
			(
				SELECT PART_NO,QTY,RECEIVE_QTY,(SELECT NVL(SUM(QTY),0) FROM SAJET.G_RECEIVE_DETAIL WHERE STATUS != -1 AND NO_HEADER = TB1.NO AND STATUS = 1) TRANSFER_QTY,
				(QTY - RECEIVE_QTY) REMAIN_QTY,UOM,REFERENCE,BATCH,STATUS, 
				CASE DATATYPE WHEN '001' THEN 'RM (0010)' WHEN '002' THEN 'FG (0020)'  ELSE 'SFG (0030)' END  DATATYPE,NO,WO,
				(    
					SELECT CREATEDATE FROM SAJET.G_RECEIVE_DETAIL WHERE NO_HEADER = TB1.NO AND ROWNUM = 1    
				) CREATEDATE,
				(    
					SELECT CREATOR FROM SAJET.G_RECEIVE_DETAIL WHERE NO_HEADER = TB1.NO AND ROWNUM = 1    
				) CREATOR
				FROM SAJET.G_RECEIVE_HEADER TB1 WHERE TB1.NO_FILE = '{cb_FileChoice.SelectedValue}'  ORDER BY DATATYPE
			) TB1";

			ds = ClientUtils.ExecuteSQL(myHelper.g_SQL);

			var dt = ds.Tables[0];

			//var dt = myHelper.Query();

			gv_Header.DataSource = dt;
			myHelper.Set_GridTemplate(gv_Header);
			gv_Header.Columns["STATUS"].Visible = false;
			//gv_Header.Columns["NO"].Visible = false;


			//var SelectRow = gv_Header.Rows
			//.OfType<DataGridViewRow>()
			//.Where(row => row.Cells["STATUS"].Value.ToString() == "1")
			//.ToList();

			//foreach (var row in SelectRow)
			//{
			//	foreach (DataGridViewCell cell in row.Cells)
			//	{
			//		cell.Style.Font = new Font(gv_Header.DefaultCellStyle.Font, FontStyle.Bold);
			//	}
			//}

			//var TranferRow = gv_Header.Rows
			//.OfType<DataGridViewRow>()
			//.Where(row => row.Cells["RECEIVE_QTY"].Value.ToString() != row.Cells["TRANSFER_QTY"].Value.ToString())
			//.ToList();
			//foreach (var row in TranferRow)
			//{
			//	foreach (DataGridViewCell cell in row.Cells)
			//	{
			//		cell.Style.ForeColor = Color.Red;
			//	}
			//}
		}
		private void bt_export_Click(object sender, EventArgs e)
		{
			using (fExport SubForm = new fExport())
			{
				// 確認使用者點選的是 checkboxColumn								
				if (SubForm.ShowDialog() == DialogResult.OK)
				{

					myHelper.g_SQL =
					$@"
					SELECT * FROM 
					(
					SELECT NO,PART_NO,(SELECT NVL(SUM(QTY),0) FROM SAJET.G_RECEIVE_DETAIL WHERE NO = TB1.NO AND PART_NO = TB1.PART_NO AND STATUS = 0) QTY,UOM,WO 
					FROM SAJET.G_RECEIVE_HEADER TB1 WHERE TB1.DATATYPE = '{SubForm.StoreType}' AND RECEIVE_QTY != 0 AND NO_FILE ='{cb_FileChoice.SelectedValue}' 
					) TB1 WHERE TB1.QTY != 0
					";

					ds = ClientUtils.ExecuteSQL(myHelper.g_SQL);
					dt = ds.Tables[0];
					//dt = myHelper.Query();

					myHelper.g_SQL =
					$@"
					UPDATE SAJET.G_RECEIVE_DETAIL TB1 SET STATUS = 1 WHERE NO_HEADER IN
					(SELECT NO FROM SAJET.G_RECEIVE_HEADER 
					WHERE NO_FILE = '{cb_FileChoice.SelectedValue}' AND DATATYPE = '{SubForm.StoreType}' AND RECEIVE_QTY != 0 )  
					AND STATUS = 0
					";

					Clipboard.SetText(myHelper.g_SQL);
					ClientUtils.ExecuteSQL(myHelper.g_SQL);


					using (SaveFileDialog saveFileDialog = new SaveFileDialog())
					{
						saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
						saveFileDialog.Title = "Save a Text File";
						saveFileDialog.FileName = "output.txt"; // 默認文件名

						// 顯示對話框並檢查是否選擇了文件
						if (saveFileDialog.ShowDialog() == DialogResult.OK)
						{
							// 獲取選擇的文件路徑
							string filePath = saveFileDialog.FileName;

							try
							{
								using (StreamWriter writer = new StreamWriter(filePath))
								{
									// 写入每一行的数据
									foreach (DataRow dr in dt.Rows)
									{
										var cell1 = new[]
										{
											dr["PART_NO"].ToString() ?? string.Empty,
											dr["QTY"].ToString() ?? string.Empty,
											dr["UOM"].ToString() ?? string.Empty,
											dr["WO"].ToString() ?? string.Empty,
										};

										//var cells = dr.Cells
										//	.OfType<DataGridViewCell>()
										//	.Select(cell => cell.Value?.ToString() ?? string.Empty);

										string line = string.Join("\t", cell1); // 使用制表符分隔各列
										writer.WriteLine(line);

										myHelper.g_SQL = $@"UPDATE SAJET.G_RECEIVE_DETAIL SET STATUS = 1 WHERE STATUS = 0 AND  NO_HEADER = '{dr["NO"].ToString()}'";

										ClientUtils.ExecuteSQL(myHelper.g_SQL);

										//myHelper.Execute();

										myHelper.g_SQL = $@"UPDATE SAJET.G_RECEIVE_FILE RF
												    SET STATUS = 0
												    WHERE NO = '{cb_FileChoice.SelectedValue}'
												    AND NOT EXISTS 
													(
												    		SELECT 1
												    		FROM SAJET.G_RECEIVE_HEADER TB1
												    		JOIN SAJET.G_RECEIVE_DETAIL TB2 ON TB1.NO = TB2.NO_HEADER
												    		WHERE TB1.NO_FILE = '{dr["NO"].ToString()}' AND TB2.STATUS = 0
												    )";
										ClientUtils.ExecuteSQL(myHelper.g_SQL);
										//myHelper.Execute();
									}
								}

								lb_Message.Text = $"Data has been exported to {filePath}";
							}
							catch (Exception ex)
							{
								lb_Message.Text = ex.Message.ToString();
							}
							// 寫入數據到選擇的文件路徑
							//WriteToFile(filePath);
						}
					}
				}
			}

			ShowData();
		}
		private void CenterControl(Control control, Panel panel)
		{
			control.Left = (panel.ClientSize.Width - control.Width) / 2;			
		}
		private void gv_Header_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				gv_Header.ContextMenuStrip = MS;
				DataGridViewRow gv_row = gv_Header.CurrentRow;

				var hitTestInfo = gv_Header.HitTest(e.X, e.Y);

				if (hitTestInfo.RowIndex >= 0)
				{
					gv_Header.ClearSelection();
					gv_Header.Rows[hitTestInfo.RowIndex].Selected = true;
					gv_Header.CurrentCell = gv_Header.Rows[hitTestInfo.RowIndex].Cells[0];
					MS.Show(gv_Header, new Point(e.X, e.Y));
				}
			}
		}
		private void Print_Click(object sender, EventArgs e)
		{
			DataGridViewRow CurrentRow = gv_Header.CurrentRow;

			ReceiveModel _ReceiveModel = new ReceiveModel();

			using (DataInput SubForm = new DataInput("請輸入補印資料", _ReceiveModel))
			{
				_ReceiveModel.PALLET = "";
				_ReceiveModel.DN = CurrentRow.Cells["REFERENCE"].Value.ToString();
				_ReceiveModel.DNITEM = 1;
				_ReceiveModel.PART = CurrentRow.Cells["PART_NO"].Value.ToString();
				_ReceiveModel.CARTON = 0;
				_ReceiveModel.QTY = int.Parse(CurrentRow.Cells["REMAIN_QTY"].Value.ToString());
				_ReceiveModel.WO = CurrentRow.Cells["WO"].Value.ToString();

				if (SubForm.ShowDialog() == DialogResult.OK)
				{					
					// 創建 QR Code 編碼器
					//BarcodeWriter barcodeWriter = new BarcodeWriter();
					//barcodeWriter.Format = BarcodeFormat.QR_CODE;
					//barcodeWriter.Options = new ZXing.QrCode.QrCodeEncodingOptions
					//{
					//	Width = pic_Barcode.Width,
					//	Height = pic_Barcode.Height,
					//	Margin = 1
					//};

					//// 生成 QR Code 圖像
					//Bitmap qrCodeBitmap = barcodeWriter.Write(SubForm.g_Barcode);

					//// 顯示在 PictureBox 中
					//pic_Barcode.Image = qrCodeBitmap;

					txt_Barcode.Text = SubForm.g_Barcode;
				}
			}
		}		
		private void ck_Display_CheckedChanged(object sender, EventArgs e)
		{

			if (ck_Display.Checked)
			{
				myHelper.g_SQL = "DELETE FROM SAJET.G_RECEIVE_DETAIL";
				ClientUtils.ExecuteSQL(myHelper.g_SQL);


				myHelper.g_SQL = "UPDATE SAJET.G_RECEIVE_HEADER SET STATUS = 0,RECEIVE_QTY = 0";
				ClientUtils.ExecuteSQL(myHelper.g_SQL);

				myHelper.g_SQL = "UPDATE SAJET.G_RECEIVE_FILE SET STATUS = 0";
				ClientUtils.ExecuteSQL(myHelper.g_SQL);				
			}

			ShowData();
			//myHelper.isDisplay = (ck_Display.Checked) ? 0 : 1;

			//myHelper.GetDropDowndata(cb_FileChoice);			
		}
		private void txt_Barcode_KeyDown(object sender, KeyEventArgs e)
		{			
			if (e.KeyCode == Keys.Enter)
			{
				#region 變數區
				var result = String.Empty;

				var LOT = "";

				//var input = txt_Barcode.Text.Trim();
				//為了修正SN 沒有的情況
				var input = txt_Barcode.Text.Trim().Replace(" ", "").Replace("\"\"-\"\"", "\"-\"");				
				input = input.Replace(" ","");

				if (input.IndexOf("\"-\"A") > 0 )
				{
					input = input.Replace("\"-\"", "");
				}
				//修正不符合JSON格式問題 ex. "SN":"A2410013423"-"A2410013552"
				//MessageBox.Show(input);

				var BarCodeType = 0;
				#endregion

				ShowData();
				

				if (!string.IsNullOrEmpty(input))
				{
					bool isValid = myHelper.IsValidJsonObject(input);

					BarCodeType = isValid ? 1 : 0; //1：外標 0:內標

					//SN的格式造成判斷Json格式錯誤
					//{"PALLET":"51","DN":"4301685764","DNITEM":1,"PART":"T12R-LX6-4128A","BOXCOUNT":26,"QTY":130,"WO":"X1T24A0094","SN":"A2410013423"-"A2410013552"}
					//{"PALLET":"L1","DN":"4301759092","DNITEM":1,"PART":"8DA51GY16001","BOXCOUNT":16,"QTY":16,"WO":"X1G2570150","SN":"-"}
					//MessageBox.Show(isValid.ToString());


					ReceiveModel Data;

					#region 標籤資料處理 (內外標)
					if (isValid) //外標
					{
						Data = JsonConvert.DeserializeObject<ReceiveModel>(input);

						if (string.IsNullOrEmpty(Data.DN.Trim()))
						{
							MessageSetting("DN is empty");
							return;
						}
					}
					else //內標
					{
						var arr_input = input.Split(';');
						var PART = arr_input[0].ToString();

						var QTY = 0;//int.Parse(arr_input[1].ToString());
						var DN = ""; //arr_input[2].Split('-')[0].ToString();
						var WO = "";
						LOT = arr_input[2];


						

						//判斷是否為料號型態
						myHelper.g_SQL = $@"SELECT CASE WHEN PART_TYPE LIKE 'XF%' THEN '002' ELSE (CASE WHEN PART_TYPE LIKE 'XW%' THEN '003' ELSE '001' END) END PART_TYPE  
										FROM SAJET.SYS_PART WHERE PART_NO = '{PART}'   ";

						ds = ClientUtils.ExecuteSQL(myHelper.g_SQL);

						//dt = myHelper.Query();

						if (ds.Tables[0].Rows.Count > 0)
						{
							switch (ds.Tables[0].Rows[0][0].ToString())
							{
								case "001":
									DN = arr_input[2].Split('-')[0].ToString();
									QTY = int.Parse(arr_input[1].ToString());
									WO = "NA";
									break;
								case "002": //成品
									DN = "NA";
									WO = arr_input[2].Split('-')[0].ToString();
									if (arr_input[1].ToString().IndexOf("-") >= 0)
									{
										QTY = myHelper.Calculate(arr_input[1].ToString());
									}
									break;
								case "003":
									DN = "NA";
									WO = arr_input[2].Split('-')[0].ToString();
									QTY = int.Parse(arr_input[1].ToString());
									break;
								default:
									break;
							}
						}
						else 
						{
							MessageSetting("Cann't find this part on list.");
							return;
						}

						if (QTY == 0)
						{
							MessageSetting("Barcode information is wrong.");
							return;
						}
												
						////內標根本沒DN，只能從現有資料上取						

						var MyRow = gv_Header.Rows.OfType<DataGridViewRow>()
						.FirstOrDefault(row =>
						{
							var partNoValue = row.Cells["PART_NO"].Value?.ToString().Trim();  // 使用 null 合併運算子
							var woValue = row.Cells["WO"].Value?.ToString().Trim();  // 使用 null 合併運算子

							return partNoValue == PART.Trim() && woValue == WO.Trim();
						});

						if (MyRow != null)
						{
							DN = MyRow.Cells["REFERENCE"].Value.ToString().Trim();

							//內標無PALLET、CARTON，預設帶0
							var jsonObject = new { PALLET = "0", DN = DN, DNITEM = 1, PART = PART, CARTON = 1, QTY = QTY, WO = WO, SN = "" };
							string jsonString = JsonConvert.SerializeObject(jsonObject);							
							Data = JsonConvert.DeserializeObject<ReceiveModel>(jsonString);
						}
						else
						{
							MessageBox.Show($@"Cann't find the Part {PART.Trim()} in this list");
							return;
						}						
					}
					#endregion


					if ((!string.IsNullOrEmpty(Data.PART)))
					{
						Data.WO = (string.IsNullOrEmpty(Data.WO)) ? "NA" : Data.WO;

						//探查到匯入資料裡有空白，使用Trim去除  ~~ By Jim 20241203
						var MyRow = gv_Header.Rows.OfType<DataGridViewRow>().Where
						(
							row => row.Cells["PART_NO"].Value.ToString().Trim() == Data.PART.Trim()
							&& row.Cells["WO"].Value.ToString().Trim() == Data.WO.Trim()
						);

						var iCheck = MyRow.ToList().Count;
						
						//判斷料號 & 資料是否有符合
						//var MatchRow = MyRow.FirstOrDefault();						
						if (iCheck == 0)
						{
							MessageSetting("Cann't find this part on list.");
							return;
						}
												
						//判斷是否已滿足數量
						var DataCheck = MyRow.Where
						(
							row => row.Cells["STATUS"].Value.ToString() == "1" 
							&& row.Cells["REFERENCE"].Value.ToString() == Data.DN
						).ToList();

						if (DataCheck.Count > 0)
						{							
							MessageSetting("This part receipt quantity had reached");
							return;
						}
						
						//判斷是否大於需求數量
						bool isMatching = MyRow.Any
						(
							row => row.Cells["STATUS"].Value.ToString() != "1" 															
							&& int.Parse(row.Cells["REMAIN_QTY"].Value.ToString())  >= Data.QTY					
						);

						if (!isMatching)
						{
							MessageSetting("Input amount is greater then current data,please check Barcode Information");
							return;
						}

						//判斷是否已掃過
						myHelper.g_SQL = $@"SELECT * FROM SAJET.G_RECEIVE_DETAIL WHERE BARCODE = '{input}'";
						ds = ClientUtils.ExecuteSQL(myHelper.g_SQL);
						
						if (ds.Tables[0].Rows.Count > 0)
						{
							MessageSetting("QRCode had scan already !");
							return;
						}


						

						object[][] Params = new object[14][];
						Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "P_PALLET", Data.PALLET };
						Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "P_DN", Data.DN };
						Params[2] = new object[] { ParameterDirection.Input, OracleType.Number, "I_DNITEM", Data.DNITEM };
						Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "P_PART", Data.PART };
						Params[4] = new object[] { ParameterDirection.Input, OracleType.Number, "I_CARTON", Data.CARTON };
						Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_QTY", Data.QTY };
						Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "P_WO", Data.WO };
						Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "P_LOT", LOT };
						Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "P_NO_FILE", cb_FileChoice.SelectedValue };
						Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "P_USERID", ClientUtils.UserPara1.ToString() };
						Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "P_BARCODETYPE", BarCodeType };
						Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "P_BARCODE", input };
						Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "P_SN", Data.SN };
						Params[13] = new object[] { ParameterDirection.Output, OracleType.VarChar, "sRESULT", "" };


						ds = ClientUtils.ExecuteProc("SAJET.SP_DATARECEIVE_NEW", Params);

						lb_Message.Text = ds.Tables[0].Rows[0][0].ToString();

						txt_Barcode.Text = "";

						ShowData();
						
						var MatchRow1 = gv_Header.Rows.OfType<DataGridViewRow>().Where
						(
							row => row.Cells["PART_NO"].Value.ToString().Trim() == Data.PART.Trim()
							&& row.Cells["WO"].Value.ToString().Trim() == Data.WO.Trim()
						).FirstOrDefault();

						if (MatchRow1 != null)
						{							
							MatchRow1.Selected = true;

							MatchRow1.DefaultCellStyle.SelectionForeColor = Color.Red;
						}

					}
					else
					{
						MessageBox.Show("標籤資訊錯誤");
						txt_Barcode.Text = "";
					}
				}

			}
		}
		private void MessageSetting(string message) 
		{
			lb_Message.Text = message;
			txt_Barcode.Text = "";
			txt_Barcode.Focus();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			//myHelper.g_SQL = @"SELECT * FROM sajet.SYS_PROGRAM_FUN_AUTHORITY T WHERE program = 'Assembly'";
			//myHelper.g_SQL = $@"
			//SELECT * FROM SAJET.G_RECEIVE_HEADER TB1 WHERE TB1.No_File = '{cb_FileChoice.SelectedValue.ToString()}' AND TB1.PART_NO = '8A4M32420001' AND STATUS = 0 ";

			myHelper.g_SQL = @"UPDATE SAJET.G_RECEIVE_DETAIL SET PART_NO = TRIM(PART_NO),WO = TRIM(WO),DN = TRIM(DN)";
			var ds = ClientUtils.ExecuteSQL(myHelper.g_SQL);
			MessageBox.Show("1");

			myHelper.g_SQL = @"UPDATE SAJET.G_RECEIVE_HEADER SET PART_NO = TRIM(PART_NO),WO = TRIM(WO),REFERENCE = TRIM(REFERENCE)";
			var ds2 = ClientUtils.ExecuteSQL(myHelper.g_SQL);
			MessageBox.Show("2");


			//gv_Header.DataSource = ds.Tables[0];
		}

		private void button2_Click(object sender, EventArgs e)
		{
			myHelper.g_SQL = $@"SELECT * FROM SAJET.G_RECEIVE_HEADER WHERE PART_NO = 'NYLTIE-YJ102' AND NO_FILE = '{ cb_FileChoice.SelectedValue}' ";
			var ds = ClientUtils.ExecuteSQL(myHelper.g_SQL);
			gv_Header.DataSource = ds.Tables[0];


			//myHelper.g_SQL = @"insert into sajet.SYS_PROGRAM_FUN_AUTHORITY(PROGRAM, FUNCTION, AUTH_SEQ, AUTHORITYS)
			//values('Assembly', 'DataReceive', 3, 'Full Control')";
			//ClientUtils.ExecuteSQL(myHelper.g_SQL);

			//myHelper.g_SQL = @"insert into sajet.SYS_PROGRAM_FUN_AUTHORITY(PROGRAM, FUNCTION, AUTH_SEQ, AUTHORITYS)
			//values('Assembly', 'WOMaterial', 3, 'Full Control')";
			//ClientUtils.ExecuteSQL(myHelper.g_SQL);

			//myHelper.g_SQL = @"insert into sajet.SYS_PROGRAM_FUN_AUTHORITY(PROGRAM, FUNCTION, AUTH_SEQ, AUTHORITYS)
			//values('Assembly', 'PreMachining', 3, 'Full Control')";
			//ClientUtils.ExecuteSQL(myHelper.g_SQL);

			//myHelper.g_SQL = @"insert into sajet.SYS_PROGRAM_FUN_AUTHORITY(PROGRAM, FUNCTION, AUTH_SEQ, AUTHORITYS)
			//values('Assembly', 'RCInput_SN_NEW', 3, 'Full Control')";
			//ClientUtils.ExecuteSQL(myHelper.g_SQL);
		}

		private void gv_Header_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
			{
				DataGridViewRow row = gv_Header.Rows[e.RowIndex];
				DataGridViewCell cell = row.Cells["STATUS"]; // 假設 "Status" 是你要檢查的欄位名稱
				DataGridViewCell cell_RECEIVE_QTY = row.Cells["RECEIVE_QTY"];
				DataGridViewCell cell_TRANSFER_QTY = row.Cells["TRANSFER_QTY"];

				if (cell.Value.ToString() == "1")
				{
					e.CellStyle.Font = new Font(gv_Header.Font, FontStyle.Bold);
				}

				if (cell_RECEIVE_QTY.Value.ToString() != cell_TRANSFER_QTY.Value.ToString())
				{
					row.DefaultCellStyle.ForeColor = Color.Red;
				}
			}
		}
	}
}
