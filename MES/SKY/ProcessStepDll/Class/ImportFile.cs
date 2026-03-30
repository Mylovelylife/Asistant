using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Data;
using SajetClass;

namespace ProcessStepDll
{
    public class ImportFile
    {
        Dictionary<string, string> dictPart = new Dictionary<string, string>();
        Dictionary<string, string> dictProcess = new Dictionary<string, string>();
        Dictionary<string, string> dictTooling = new Dictionary<string, string>();
        public string sResult;
        public ToolingUtils ToolUtils;
        public DataTable dtImport;
        public string sFileName;
        public delegate void m_OnCompleted();
        public event m_OnCompleted OnCompleted;
        public delegate void m_OnReportStatus(int iCount);
        public event m_OnReportStatus OnReportStatus;
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        private System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource();
        public ImportFile()
        {
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            dtImport = new DataTable();
            dtImport.Columns.Add("PART_NO", typeof(string));
            dtImport.Columns.Add("PROCESS_NAME", typeof(string));
            dtImport.Columns.Add("TOOLING_NO", typeof(string));
            dtImport.Columns.Add("QTY", typeof(string));
            dtImport.Columns.Add("RESULT", typeof(string));

        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnCompleted();
        }

        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnReportStatus(e.ProgressPercentage);
        }


        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            dictPart.Clear();
            dictProcess.Clear();
            dictTooling.Clear();
            ExportExcelAllVer.ExcelEditAll ExcelClass = new ExportExcelAllVer.ExcelEditAll();
            try
            {
                BackgroundWorker worker = (sender) as BackgroundWorker;

                worker.ReportProgress(0);
                try
                {
                    int iRowIndex = 2;
                    ExcelClass.Open(sFileName);
                    object ExcelSheet = ExcelClass.GetSheet(1);
                    while (true)
                    {
                        try
                        {
                            object objPartNo = ExcelClass.GetCellValue1(ExcelSheet, 1, iRowIndex);
                            object objProcess = ExcelClass.GetCellValue1(ExcelSheet, 2, iRowIndex);
                            object objToolingNo = ExcelClass.GetCellValue1(ExcelSheet, 3, iRowIndex);
                            object objQty = ExcelClass.GetCellValue1(ExcelSheet, 4, iRowIndex);
                            if (objPartNo == null)
                                break;
                            string sPartNo = (string)objPartNo;
                            string sProcess = (string)objProcess;
                            string sToolingNo = (string)objToolingNo;

                            string sQty = objQty.ToString();

                            DataRow dr = dtImport.NewRow();
                            dr["PART_NO"] = sPartNo;
                            dr["PROCESS_NAME"] = sProcess;
                            dr["TOOLING_NO"] = sToolingNo;
                            dr["QTY"] = sQty;
                            dr["RESULT"] = "";
                            dtImport.Rows.Add(dr);
                            iRowIndex += 1;
                            ProcessFile(dr);
                            worker.ReportProgress(1);
                            if (backgroundWorker.CancellationPending)
                                break;
                        }
                        catch (Exception ex)
                        {
                            sResult = ex.Message;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    sResult = SajetCommon.SetLanguage("Import Error! Can't find the name 'Sheet1' of sheet") + ":" + Environment.NewLine + Environment.NewLine + ex.Message;
                    return;
                }

            }
            finally
            {
                ExcelClass.Close();
            }
        }
        public void Cancel()
        {
            if (backgroundWorker.IsBusy)
            {

                backgroundWorker.CancelAsync();
            }
        }
        public void Start()
        {
            backgroundWorker.RunWorkerAsync();
        }
        public void Stop()
        {
            if (backgroundWorker.IsBusy)
                backgroundWorker.CancelAsync();
        }
        private void ProcessFile(DataRow dr)
        {
            string sResult = "";
            string sPartNo = dr["PART_NO"].ToString();
            string sProcessName = dr["PROCESS_NAME"].ToString();
            string sToolingNo = dr["TOOLING_NO"].ToString();
            string sQty = dr["QTY"].ToString();
            string sPartID;
            string sProcessID;
            string sToolingID;
            dictPart.TryGetValue(sPartNo, out sPartID);
            if (sPartID == null)
            {
                if (ToolUtils.sFunType == "MODEL")
                    sPartID = SajetCommon.GetID("SAJET.SYS_MODEL", "MODEL_ID", "MODEL_NAME", sPartNo);
                else
                    sPartID = SajetCommon.GetID("SAJET.SYS_PART", "PART_ID", "PART_NO", sPartNo);
                if (sPartID == "0")
                    sResult = SajetCommon.SetLanguage("Part/Model Error");
                else
                    dictPart.Add(sPartNo, sPartID);
            }
            dictProcess.TryGetValue(sProcessName, out sProcessID);
            if (sProcessID == null)
            {
                sProcessID = SajetCommon.GetID("SAJET.SYS_PROCESS", "PROCESS_ID", "PROCESS_NAME", sProcessName);
                if (sProcessID == "0")
                    sResult = SajetCommon.SetLanguage("Process Error");
                else
                    dictProcess.Add(sProcessName, sProcessID);
            }
            dictTooling.TryGetValue(sToolingNo, out sToolingID);
            if (sToolingID == null)
            {
                sToolingID = SajetCommon.GetID("SAJET.SYS_TOOLING", "TOOLING_ID", "TOOLING_NO", sToolingNo);
                if (sToolingID == "0")
                    sResult = SajetCommon.SetLanguage("Tooling Error");
                else
                    dictTooling.Add(sToolingNo, sToolingID);
            }
            int iQty = 0;
            try
            {
                iQty = Convert.ToInt32(sQty);
            }
            catch
            {
                sResult = SajetCommon.SetLanguage("Qty Error");
            }

            if (sPartID != "0" && sProcessID != "0" && sToolingID != "0" && string.IsNullOrEmpty(sResult))
            {
                ToolUtils.sPKFieldIDValue = sPartID;
                try
                {
                    if (ToolUtils.ToolingExist(sProcessID, sToolingID))
                    {
                        ToolUtils.UpdateToolingQTY(sProcessName, sToolingNo, sQty);
                        sResult = "OK,UPDATE";
                    }
                    else
                    {
                        ToolUtils.InsertToolingSN(sProcessName, sToolingNo, "0", iQty);
                        sResult = "OK,INSERT";
                    }
                }
                catch (Exception EX)
                {
                    sResult = EX.Message;
                }
            }
            dr["RESULT"] = sResult;

        }
    }
}
