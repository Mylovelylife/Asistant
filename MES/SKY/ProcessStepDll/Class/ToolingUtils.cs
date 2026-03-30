using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using SajetClass;
namespace ProcessStepDll
{
    public class ToolingUtils
    {
        private string _sFunType;
        private string _sPKFieldIDValue;
        private string _sTableName;
        private string _sPKFieldID;
        private string _sHTTableName;
        private string _sEmpID;
        private DateTime _dtDateTime;
        private string _sDestKeyValue;



        public DateTime dtDateTime
        {
            set
            {
                _dtDateTime = value;
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
            get { return _sFunType; }
        }
        public string sPKFieldIDValue
        {
            set
            { _sPKFieldIDValue = value; }
            get
            { return _sPKFieldIDValue; }
        }
        public string sPKFieldID
        {
            set
            { _sPKFieldID = value; }
            get
            { return _sPKFieldID; }
        }
        public string sPKFieldName;
        public string sDestKeyValue
        {
            set
            { _sDestKeyValue = value; }
            get
            { return _sDestKeyValue; }
        }
        public string sEmpID
        {
            set
            { _sEmpID = value; }
        }



        public void Copy()
        {
            UpdateTableByModel();
            InsertHTableByModel(_sDestKeyValue);
            DeleteTableByModel(_sDestKeyValue);
            CopyFromModel();
            InsertHTableByModel(_sDestKeyValue);
        }
        public void InsertToolingSN(string sProcessName, string sToolingNO, string sToolingSN, int iQTY)
        {
            //
            string sStepItemCode = sToolingNO;
            int iIndex = sToolingNO.IndexOf("]");
            if (iIndex >= 0)
                sStepItemCode = sToolingNO.Substring(1,iIndex - 1); ;
            string sSQL = "INSERT INTO SAJET.SYS_PROCESS_STEP "
                 + " (PROCESS_ID,STEP_ITEM_ID,UPDATE_USERID) "
                 + "VALUES(:PROCESS_ID,:STEP_ITEM_ID,:UPDATE_USERID) ";


            string sToolingNOID = SajetCommon.GetID("SAJET.SYS_STEP_ITEM", "STEP_ITEM_ID", "STEP_ITEM_CODE", sStepItemCode);
            string sProcessID = SajetCommon.GetID("SAJET.SYS_PROCESS", "PROCESS_ID", "PROCESS_NAME", sProcessName);
          
            object[][] Params = new object[3][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", sProcessID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.Number, "STEP_ITEM_ID", sToolingNOID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", _sEmpID };
            ClientUtils.ExecuteSQL(sSQL, Params);
            InsertHTableByTooling(sProcessID, sToolingNOID);
        }
        public void DeleteProcess(string sProcessName)
        {
            string sProcessID = SajetCommon.GetID("SAJET.SYS_PROCESS", "PROCESS_ID", "PROCESS_NAME", sProcessName);
            string sSQL = "UPDATE SAJET.SYS_PROCESS_STEP "
                   + "  SET ENABLED='Drop' "
                   + "     ,UPDATE_TIME =:UPDATE_TIME "
                   + "     ,UPDATE_USERID =:UPDATE_USERID "
                   + " WHERE PROCESS_ID =:PROCESS_ID ";

            object[][] Params = new object[3][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", _dtDateTime };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", _sEmpID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            InsertHTableByProcess(sProcessID);
            DeleteTableByProcess(sProcessID);
        }
        
        public void DeleteToolingSN(DateTime dtTime, string sProcessName, string sToolingNO)
        {
            string sStepItemCode =sToolingNO;
            int iIndex = sToolingNO.IndexOf("]");
            if (iIndex>=0)
                sStepItemCode = sToolingNO.Substring(1,iIndex - 1); 
            string sToolingNOID = SajetCommon.GetID("SAJET.SYS_STEP_ITEM", "STEP_ITEM_ID", "STEP_ITEM_CODE", sStepItemCode);
            string sProcessID = SajetCommon.GetID("SAJET.SYS_PROCESS", "PROCESS_ID", "PROCESS_NAME", sProcessName);
            string sSQL = "UPDATE  SAJET.SYS_PROCESS_STEP  "
                   + "  SET ENABLED='Drop' "
                   + "     ,UPDATE_TIME =:UPDATE_TIME "
                   + "     ,UPDATE_USERID =:UPDATE_USERID "
                   + " WHERE PROCESS_ID =:PROCESS_ID "
                   + "   AND STEP_ITEM_ID =:STEP_ITEM_ID ";
            object[][] Params = new object[4][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtTime };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", _sEmpID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STEP_ITEM_ID", sToolingNOID };
            ClientUtils.ExecuteSQL(sSQL, Params);
            InsertHTableByTooling(sProcessID, sToolingNOID);
            DeltetTableByTooling(sProcessID, sToolingNOID);
        }
         
        /*
        public void DeleteToolingSN(DateTime dtTime, string sProcessName, string sToolingSN)
        {
            string sToolingSNID = SajetCommon.GetID("SAJET.SYS_TOOLING_SN", "TOOLING_SN_ID", "TOOLING_SN", sToolingSN);
            string sProcessID = SajetCommon.GetID("SAJET.SYS_PROCESS", "PROCESS_ID", "PROCESS_NAME", sProcessName);
            string sSQL = "UPDATE  " + _sTableName
                   + "  SET ENABLED='Drop' "
                   + "     ,UPDATE_TIME =:UPDATE_TIME "
                   + "     ,UPDATE_USERID =:UPDATE_USERID "
                   + " WHERE " + _sPKFieldID + " =:MODEL_ID "
                   + "   AND PROCESS_ID =:PROCESS_ID "
                   + "   AND TOOLING_SN_ID =:TOOLING_SN_ID ";
            object[][] Params = new object[5][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtTime };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", _sEmpID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODEL_ID", sPKFieldIDValue };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_SN_ID", sToolingSNID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            InsertHTableByTooling(sProcessID, sToolingSNID);
            DeltetTableByTooling(sProcessID, sToolingSNID);
        }
         */ 
        
        
        private void UpdateTableByModel()
        {
            string sSQL = "UPDATE  SAJET.SYS_PROCESS_STEP "
                  + "  SET ENABLED='Drop' "
                  + "     ,UPDATE_TIME =:UPDATE_TIME "
                  + "     ,UPDATE_USERID =:UPDATE_USERID "
                  + " WHERE " + _sPKFieldID + " =:MODEL_ID ";
            object[][] Params = new object[3][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", _dtDateTime };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", _sEmpID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODEL_ID", _sDestKeyValue };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
        }
        private void InsertHTableByModel(string sFieldID)
        {
            string sSQL = "INSERT INTO SAJET.SYS_HT_PROCESS_STEP "
                      + " SELECT * FROM SAJET.SYS_PROCESS_STEP "
                      + " WHERE " + _sPKFieldID + " =:MODEL_ID ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODEL_ID", sFieldID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
        }
        private void InsertHTableByProcess(string sProcessID)
        {
            string sSQL = "INSERT INTO  SAJET.SYS_HT_PROCESS_STEP "
                      + " SELECT * FROM  SAJET.SYS_PROCESS_STEP "
                      + " WHERE PROCESS_ID =:PROCESS_ID ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
        }
        private void InsertHTableByTooling(string sProcessID, string sToolingNOID)
        {
            string sSQL = "INSERT INTO SAJET.SYS_HT_PROCESS_STEP "
                      + " SELECT * FROM SAJET.SYS_PROCESS_STEP "
                      + " WHERE PROCESS_ID =:PROCESS_ID "
                      + "   AND STEP_ITEM_ID =:STEP_ITEM_ID ";

            object[][] Params = new object[2][];
            
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STEP_ITEM_ID", sToolingNOID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
        }

        private void DeleteTableByModel(string sFieldID)
        {
            string sSQL = "DELETE  SAJET.SYS_PROCESS_STEP "
                        + " WHERE " + _sPKFieldID + " =:MODEL_ID ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODEL_ID", sFieldID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
        }
        private void DeltetTableByTooling(string sProcessID,string sToolingID)
        {
            string sSQL = "DELETE  SAJET.SYS_PROCESS_STEP "
                    + " WHERE PROCESS_ID =:PROCESS_ID "
                    + "   AND STEP_ITEM_ID =:STEP_ITEM_ID ";

            object[][] Params = new object[2][];

            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STEP_ITEM_ID", sToolingID };
            
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
        }
        private void DeleteTableByProcess(string sProcessID)
        {
            string sSQL = "DELETE  SAJET.SYS_PROCESS_STEP "
                       + " WHERE PROCESS_ID =:PROCESS_ID ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
        }
        private void CopyFromModel()
        {
            string sSQL = "INSERT INTO SAJET.SYS_PROCESS_STEP "
                 + " (" + _sPKFieldID + ",STEP_ITEM_ID,UPDATE_USERID,UPDATE_TIME ) "
                 + " SELECT " + _sDestKeyValue + ",STEP_ITEM_ID,:UPDATE_USERID,:UPDATE_TIME "
                 + "  FROM SAJET.SYS_PROCESS_STEP "
                 + " WHERE " + _sPKFieldID + "=:MODEL_ID AND ENABLED='Y' ";

            object[][] Params = new object[3][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", _sEmpID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", _dtDateTime };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODEL_ID", sPKFieldIDValue };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

        }

        public void UpdateToolingQTY(string sProcessName, string sToolingNO, string iQTY)
        {
            string sToolingNOID = SajetCommon.GetID("SAJET.SYS_TOOLING", "TOOLING_ID", "TOOLING_NO", sToolingNO);
            string sProcessID = SajetCommon.GetID("SAJET.SYS_PROCESS", "PROCESS_ID", "PROCESS_NAME", sProcessName);

            string sSQL = "update " + _sTableName
                        + @"  set update_userid = :update_userid, update_time = sysdate, tooling_qty = :tooling_qty
                                    where " + _sPKFieldID + @" = :model_id
                                      and process_id = :process_id
                                      and tooling_id = :tooling_id";
            object[][] Params = new object[5][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.Number, "MODEL_ID", _sPKFieldIDValue };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", sProcessID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.Number, "TOOLING_ID", sToolingNOID };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.Number, "TOOLING_QTY", iQTY };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", _sEmpID };
            ClientUtils.ExecuteSQL(sSQL, Params);

            InsertHTableByTooling(sProcessID, sToolingNOID);
        }
        public bool ToolingExist(string sProcessID, string sToolingID)
        {
            string sSQL = "SELECT " + _sPKFieldID + "  FROM " + _sTableName
                 + " WHERE " + _sPKFieldID + " =:MODEL_ID "
                 + " AND PROCESS_ID =:PROCESS_ID "
                 + " AND TOOLING_ID =:TOOLING_ID ";
            object[][] Params = new object[3][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODEL_ID", sPKFieldIDValue };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_ID", sToolingID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            return dsTemp.Tables[0].Rows.Count > 0;
        }
    }
}
