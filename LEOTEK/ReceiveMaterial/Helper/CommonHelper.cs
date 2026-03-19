//using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using ReceiveMaterial.Model;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;




namespace ReceiveMaterial.Helper
{
	public class CommonHelper
	{
		public string g_SQL { get; set; }
		public string g_FileName { get; set; }
		public string _connectionString { get; set; }

		public int isDisplay;

		public DataSet ds;

		public CommonHelper() 
		{
			isDisplay = 1;

			var intranet = false;

			foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
			{
				var gatewayAddresses = netInterface.GetIPProperties().GatewayAddresses;

				if (netInterface.Name == "internal")
				{
					if (gatewayAddresses.Any())
					{
						foreach (var gateway in gatewayAddresses)
						{
							if (gateway.Address.ToString().IndexOf("10.10") >= 0)
							{
								intranet = true;
							}							
						}
					}
				}				
			}



			if (intranet)
			{
				_connectionString = "User Id=sajet;Password=tech;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.1.126.220)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=leotek)));";
			}
			else 
			{
				_connectionString = "User Id=sajet;Password=tech;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=219.87.155.100)(PORT=37679)))(CONNECT_DATA=(SERVICE_NAME=leotek)));";
			}						
		}
		public void GetDropDowndata(ComboBox cb, ComboBox sender = null)
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("TBText", typeof(string));
			dt.Columns.Add("TBValue", typeof(string));

			cb.DataSource = null;
			cb.Items.Clear();
			dt.Rows.Add("Please Choose", "0");

			switch (cb.Name.ToString())
			{
				case "cb_FileChoice":

					//g_SQL = $@"SELECT DISTINCT FILENAME,NO FROM SAJET.G_RECEIVE_FILE WHERE 1 = 1 AND STATUS = {isDisplay}";
					g_SQL = $@"SELECT FILENAME,NO FROM SAJET.G_RECEIVE_FILE WHERE 1 = 1 ORDER BY CREATEDATE DESC";

					//var dt2 = new DataTable();


					//using (OracleConnection conn = new OracleConnection(_connectionString))
					//{
					//	conn.Open();

					//	using (OracleDataAdapter adt = new OracleDataAdapter(g_SQL, conn))
					//	{
					//		adt.Fill(dt2);
					//	}
					//}

					//if (dt2.Rows.Count > 0)
					//{
					//	foreach (DataRow row in dt2.Rows)
					//	{
					//		dt.Rows.Add(row[0].ToString(), row[1].ToString());
					//	}
					//}

					var ds = ClientUtils.ExecuteSQL(g_SQL);

					if (ds.Tables[0].Rows.Count > 0)
					{
						foreach (DataRow row in ds.Tables[0].Rows)
						{
							dt.Rows.Add(row[0].ToString(), row[1].ToString());
						}
					}

					break;
				case "cb_BarCode_Type":
					dt.Rows.Add("棧板標籤", "1");
					dt.Rows.Add("外箱標籤", "2");
					break;
				default:
					break;
			}

			cb.DataSource = dt;
			cb.DisplayMember = "TBText";
			cb.ValueMember = "TBValue";
			cb.DropDownStyle = ComboBoxStyle.DropDownList;

			//switch (cb.Name.ToString())
			//{
			//	case "cb_FileChoice":
			//		if (cb.Items.Count > 1)
			//		{
			//			cb.SelectedIndex = 1;
			//		}
			//		break;	
			//}
			cb.SelectedIndex = 0;
		}
		public void GetFile(string _GUID)
		{

			//using (OracleConnection conn = new OracleConnection(_connectionString))
			//{
			//	conn.Open();

			//	using (OracleDataAdapter adt = new OracleDataAdapter(g_SQL, conn))
			//	{
			//		adt.Fill(dt);
			//	}
			//}


			//if (dt.Rows.Count > 0)
			//{
			//	return dt.Rows[0][0].ToString();
			//}
			//else
			//{
			//	g_SQL = $@"INSERT INTO SAJET.G_RECEIVE_FILE (NO,FILENAME) VALUES ('{_GUID}','{g_FileName}')";

			//	using (OracleConnection conn = new OracleConnection(_connectionString))
			//	{
			//		conn.Open();

			//		using (OracleCommand cmd = new OracleCommand(g_SQL, conn))
			//		{
			//			cmd.ExecuteNonQuery();
			//		}
			//	}

			//	return _GUID;
			//}

			g_SQL = $@"SELECT NO FROM SAJET.G_RECEIVE_FILE WHERE FILENAME = '{g_FileName}'";
			ds = ClientUtils.ExecuteSQL(g_SQL);
			if (ds.Tables[0].Rows.Count > 0)
			{
				//return ds.Tables[0].Rows[0][0].ToString();
			}
			else
			{
				g_SQL = $@"INSERT INTO SAJET.G_RECEIVE_FILE (NO,FILENAME) VALUES ('{_GUID}','{g_FileName}')";
				ClientUtils.ExecuteSQL(g_SQL);				
			}
		}
		public DataTable Query()
		{
			var dt = new DataTable();

			try
			{
				ds = ClientUtils.ExecuteSQL(g_SQL);

				dt = ds.Tables[0];
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}");
			}


			return dt;

			//using (var connection = new OracleConnection(_connectionString))
			//{
			//	try
			//	{
			//		// 打開連接
			//		connection.Open();

			//		using (var adt = new OracleDataAdapter(g_SQL, connection))
			//		{
			//			adt.Fill(dt);
			//		}
			//	}
			//	catch (Exception ex)
			//	{
			//		// 處理連接或查詢時的異常					
			//		MessageBox.Show($"Error: {ex.Message}");
			//	}

			//	return dt;
			//}
		}
		public void ImportShipping()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = "Excel Files|*.xls;*.xlsx;*.xlsm",
				Title = "Select an Excel File"
			};

			var invalid = true;

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string _FilePath = openFileDialog.FileName;

				g_FileName = System.IO.Path.GetFileName(_FilePath);

				//g_SQL = $@"SELECT * FROM SAJET.G_RECEIVE_FILE WHERE FILENAME = '{g_FileName}'";
				//ds = ClientUtils.ExecuteSQL(g_SQL);

				//if (ds.Tables[0].Rows.Count > 0)
				//{
				//	MessageBox.Show("該檔案重複匯入");
				//	return;
				//}

				try
				{
					FileStream stream = File.Open(_FilePath, FileMode.Open, FileAccess.Read);
					IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);

					var _ds = reader.AsDataSet();

					var DataType = string.Empty;

					var NO_FILE = Guid.NewGuid().ToString();
					
					foreach (DataTable dt in _ds.Tables)
					{
						switch (dt.TableName)
						{
							case "0010-原材":
								DataType = "001";
								break;
							case "0020-成品":
								DataType = "002";
								break;
							case "0030-半品":
								DataType = "003";
								break;
							default:
								break;
						}

						if (dt.Rows[0][7].ToString() != "ENCRYPTION")
						{
							MessageBox.Show("Please use the file which is from Taiwan !!!");

							invalid = false;
							return;
						}

						if (invalid)
						{
							CopyData(dt, DataType, NO_FILE);
						}						
					}

					if (invalid)
					{
						GetFile(NO_FILE);

						MessageBox.Show("Data Import Success !!!");
					}					
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}						
		}
		public void CopyData(DataTable dt,string DataType,string NO_FILE)
		{
			var NewDt = dt.AsEnumerable().Skip(1);
			
			foreach (DataRow dr in NewDt)
			{
				var WO = string.IsNullOrEmpty(dr["Column6"].ToString().Trim()) ? "NA" : dr["Column6"].ToString();

				g_SQL = 
				$@"INSERT INTO SAJET.G_RECEIVE_HEADER 
				(
					NO         
					,PART_NO    
					,QTY        
					,UOM        					
					,REFERENCE  					
					,BATCH      
					,BATCH_TO   										
					,DATATYPE   										
					,NO_FILE    
					,WO       
				) VALUES 
				(
					'{Guid.NewGuid().ToString()}',                                       
					'{dr["Column0"].ToString()}',
					'{dr["Column1"].ToString()}',
					'{dr["Column2"].ToString()}',
					'{dr["Column3"].ToString()}',
					'{dr["Column4"].ToString()}',
					'{dr["Column5"].ToString()}',
					'{DataType}',
					'{NO_FILE}',
					'{WO}'
				)";

				ClientUtils.ExecuteSQL(g_SQL);
			}


			//using (OracleConnection conn = new OracleConnection(_connectionString))
			//{
			//	conn.Open();

			//	using (OracleBulkCopy bulkCopy = new OracleBulkCopy(conn))
			//	{
			//		// 设置目标表名称
			//		bulkCopy.DestinationTableName = "G_RECEIVE_HEADER";					
			//		bulkCopy.ColumnMappings.Add("MATERIAL", "PART_NO");
			//		bulkCopy.ColumnMappings.Add("QTY", "QTY");
			//		bulkCopy.ColumnMappings.Add("RECEIVE_QTY", "RECEIVE_QTY");
			//		bulkCopy.ColumnMappings.Add("UOM", "UOM");
			//		bulkCopy.ColumnMappings.Add("REFERENCE", "REFERENCE");
			//		bulkCopy.ColumnMappings.Add("SN_FROM", "BATCH");
			//		bulkCopy.ColumnMappings.Add("SN_TO", "BATCH_TO");
			//		bulkCopy.ColumnMappings.Add("DATATYPE", "DATATYPE");
			//		bulkCopy.ColumnMappings.Add("NO", "NO");
			//		bulkCopy.ColumnMappings.Add("NO_FILE", "NO_FILE");
			//		bulkCopy.ColumnMappings.Add("STATUS", "STATUS");
			//		bulkCopy.ColumnMappings.Add("CREATEDATE", "CREATEDATE");
			//		bulkCopy.ColumnMappings.Add("WO", "WO");

			//		// 批量复制数据
			//		bulkCopy.WriteToServer(dt);
			//	}
			//}

		}
		public void ConnectionTest()
		{
			try
			{
				using (OracleConnection conn = new OracleConnection(_connectionString))
				{
					conn.Open();
					MessageBox.Show("S");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Connect Fail");
			}

		}
		public void Execute()
		{
			try
			{
				ClientUtils.ExecuteSQL(g_SQL);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			//using (var connection = new OracleConnection(_connectionString))
			//{
			//	try
			//	{
			//		connection.Open();

			//		using (var cmd = new OracleCommand(g_SQL, connection))
			//		{
			//			// 假设您已经设置了正确的参数
			//			//cmd.Parameters.Add(new OracleParameter("PALLET", Data.PALLET));
			//			//cmd.Parameters.Add(new OracleParameter("DN", Data.DN));
			//			//cmd.Parameters.Add(new OracleParameter("DNITEM", Data.DNITEM));
			//			//cmd.Parameters.Add(new OracleParameter("PART", Data.PART));
			//			//cmd.Parameters.Add(new OracleParameter("CARTON", Data.CARTON));
			//			//cmd.Parameters.Add(new OracleParameter("QTY", Data.QTY));
			//			//cmd.Parameters.Add(new OracleParameter("WO", Data.WO));
			//			cmd.ExecuteNonQuery();
			//		}
			//	}
			//	catch (Exception ex)
			//	{
			//		// 弹出异常消息
			//		MessageBox.Show(ex.Message);
			//	}
			//}
		}
		public void ExecuteProc(string SP_NAME, OracleParameter[] Params,out string result)
		{
			using (OracleConnection conn = new OracleConnection(_connectionString))
			{
				try
				{
					// 打开连接
					conn.Open();

					// 创建命令对象
					using (OracleCommand cmd = new OracleCommand(SP_NAME, conn))
					{
						// 指定命令类型为存储过程
						cmd.CommandType = System.Data.CommandType.StoredProcedure;
						cmd.Parameters.AddRange(Params);

						// 执行存储过程
						cmd.ExecuteNonQuery();

						// 获取输出参数值
						result = cmd.Parameters["sRESULT"].Value.ToString();

					}
				}
				catch (Exception ex)
				{
					// 捕获并处理其他异常							
					MessageBox.Show(ex.Message);

					result = ex.Message;
				}
			}
		}

		public string Patch(string Data) 
		{
			// 解析 JSON 字串
			JObject jsonObj = JObject.Parse(Data);

			// 提取 SN 欄位並合併
			string sn = jsonObj["SN"].ToString();
			string[] snValues = sn.Split('-'); // 分割 SN 值
			if (snValues.Length == 2)
			{
				string newSN = snValues[0] + "-" + snValues[1];  // 拼接兩個 SN 值
				jsonObj["SN"] = newSN; // 更新 SN 欄位
			}

			// 輸出處理後的 JSON
			//Console.WriteLine(jsonObj.ToString());

			return jsonObj.ToString();
		}

		#region 共用函式
		public void Set_GridTemplate(DataGridView gv)
		{
			try
			{
				gv.AllowDrop = false;
				gv.AllowUserToAddRows = false;
				gv.AllowUserToDeleteRows = false;
				gv.BackgroundColor = System.Drawing.Color.DarkGray;
				//gv.BorderStyle = BorderStyle.FixedSingle;
				gv.ReadOnly = true;
				if (gv.Rows.Count > 0)
				{
					//foreach (DataGridViewColumn _column in gv.Columns)
					//{
					//	if (_column.Index != 6)
					//	{
					//		_column.ReadOnly = (_column.GetType().ToString().ToUpper().IndexOf("CHECKBOX") >= 0) ? false : true;
					//	}

					//}

					DataGridViewRow lastRow = gv.Rows.Cast<DataGridViewRow>().Last();

					// 設置最後一行的背景色為黃色
					//lastRow.DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
				}


				gv.AllowUserToOrderColumns = true;
				gv.MultiSelect = false;
				gv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
				gv.AllowUserToResizeColumns = false;
				gv.AllowUserToResizeRows = false;
				gv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				gv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightYellow;
				//gv.DefaultCellStyle.SelectionForeColor = Color.Black;
				//gv.DefaultCellStyle.SelectionBackColor = gv.DefaultCellStyle.BackColor;
				gv.DefaultCellStyle.SelectionForeColor = gv.DefaultCellStyle.ForeColor;

				//DataGridViewCellStyle style = gv.ColumnHeadersDefaultCellStyle;
				//style.BackColor = Color.Black;
				//style.ForeColor = Color.White;			
				gv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Black;
				gv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
				gv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(gv.Font, FontStyle.Bold);
				gv.EnableHeadersVisualStyles = false;

				//gv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;            
				//gv.RowHeadersWidthSizeMode =
				//DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool isNumber(string s)
		{
			int Flag = 0;
			char[] str = s.ToCharArray();
			for (int i = 0; i < str.Length; i++)
			{
				if (Char.IsNumber(str[i]))
				{
					Flag++;
				}
				else
				{
					Flag = -1;
					break;
				}
			}

			if (Flag > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public int Calculate(string input)
		{
			// 定義正則表達式來提取字串中的數字部分
			string pattern = @"(\d+)-";
			var match = System.Text.RegularExpressions.Regex.Match(input.Split('-')[0], pattern);

			var split1 = input.Split('-')[0].ToString();
			var split2 = input.Split('-')[1].ToString();

			int num1 = int.Parse(split1.Substring(split1.Length - split2.Length));
			int num2 = int.Parse(split2);


			return (num2 - num1 + 1);
		}
		public bool IsValidJsonObject(string jsonString)
		{			
			try
			{
				// 尝试解析 JSON 字符串并检查是否为 JSON 对象
				var jObject = JObject.Parse(jsonString);
				return true;
			}
			catch (Exception ex)
			{				
				return false;
			}
		}
		public void SettingData(System.Windows.Forms.Control.ControlCollection cc)
		{
			foreach (var crl in cc.OfType<TableLayoutPanel>())
			{
				crl.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble;
				SettingData(crl.Controls);
			}

			foreach (var crl in cc.OfType<Panel>())
			{
				SettingData(crl.Controls);
			}

			foreach (var crl in cc.OfType<TextBox>())
			{
				crl.Dock = DockStyle.Fill;
				//crl.ImeMode = System.Windows.Forms.ImeMode.Disable;
			}


			foreach (var crl in cc.OfType<ComboBox>())
			{
				crl.Dock = DockStyle.Fill;
				if (crl.Name.ToString() != "cb_Version")
				{
					GetDropDowndata(crl);
				}
			}

			foreach (var crl in cc.OfType<DataGridView>())
			{
				Set_GridTemplate(crl);
			}
		}
		public static string GetLocalIPAddress()
		{
			foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
			{
				foreach (var ip in netInterface.GetIPProperties().UnicastAddresses)
				{
					if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
					{
						return ip.Address.ToString();
					}
				}
			}
			return "Not Available";
		}
		public static bool IsInternalIP(string ipAddress)
		{
			var ip = IPAddress.Parse(ipAddress);

			// 內網 IP 範圍
			var privateRanges = new[]
			{
				new IPAddressRange(IPAddress.Parse("10.10.0.0"), IPAddress.Parse("10.10.255.255")),			
			};

			foreach (var range in privateRanges)
			{
				if (range.Contains(ip))
				{
					return true;
				}
			}

			return false;
		}
		#endregion




		

	}
}
