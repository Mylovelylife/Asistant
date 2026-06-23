using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using System.Data;
namespace RepairDll
{

    public class RepairUtility
    {
        static string _sProgram;
        static string _sTerminalID;
        static string _sPreviousProcessID;
        static string _sProcessID;
        static string _sStageID;
        static string _sPDLineID;
        static string _sDefectSN;
        static string _sDefectSNWO;
        static string _sDefectSNPartID;
        static string _sRepairSN;
        static string _sRepairSNWO;
        static string _sRepairSNPartID;
        static string _sRepairType;
        static string _sDefectSNID;
        static string _sDefectRecID;
        static string _sDefectCode;
        static string _sDefectID;
        static List<string> _sDefectIDList;
        static string _sDefectLoc;
        static int _iLocationParams;
        static string _sUserID;
        public RepairUtility()
        {
        }
        public static string sProgram
        {
            get { return _sProgram; }
            set { _sProgram = value; }
        }
        public static string sTerminalID
        {
            get { return _sTerminalID; }
            set { _sTerminalID = value; }
        }
        public static string sPreviousProcessID
        {
            get { return _sPreviousProcessID; }
            set { _sPreviousProcessID = value; }
        }
        public static string sProcessID
        {
            get { return _sProcessID; }
            set { _sProcessID = value; }
        }
        public static string sStageID
        {
            get { return _sStageID; }
            set { _sStageID = value; }
        }
        public static string sPDLineID
        {
            get { return _sPDLineID; }
            set { _sPDLineID = value; }
        }
        public static string sUserID
        {
            get { return _sUserID; }
            set { _sUserID = value; }
        }
        public static string sDefectSN
        {
            get { return _sDefectSN; }
            set { _sDefectSN = value; }
        }
        public static string sDefectSNWO
        {
            get { return _sDefectSNWO; }
            set { _sDefectSNWO = value; }
        }
        public static string sDefectSNPartID
        {
            get { return _sDefectSNPartID; }
            set { _sDefectSNPartID = value; }
        }
        public static string sRepairSN
        {
            get { return _sRepairSN; }
            set { _sRepairSN = value; }
        }
        public static string sRepairSNWO
        {
            get { return _sRepairSNWO; }
            set { _sRepairSNWO = value; }
        }
        public static string sRepairSNPartID
        {
            get { return _sRepairSNPartID; }
            set { _sRepairSNPartID = value; }
        }
        public static string sRepairType
        {
            get { return _sRepairType; }
            set { _sRepairType = value; }
        }
        public static string sDefectSNID
        {
            get { return _sDefectSNID; }
            set { _sDefectSNID = value; }
        }
        public static string sDefectRecID
        {
            get { return _sDefectRecID; }
            set { _sDefectRecID = value; }
        }
        public static string sDefectCode
        {
            get { return _sDefectCode; }
            set { _sDefectCode = value; }
        }
        public static string sDefectID
        {
            get { return _sDefectID; }
            set { _sDefectID = value; }
        }

        public static List<string> sDefectIDList
        {
            get { return _sDefectIDList; }
            set { _sDefectIDList = value; }
        }

        public static string sDefectLoc
        {
            get { return _sDefectLoc; }
            set { _sDefectLoc = value; }
        }
        public static int iLocationParams
        {
            get { return _iLocationParams; }
            set { _iLocationParams = value; }
        }
        public static string[] GET_LOCATION_ITEM_PART_FROM_WO(string sSN, string sLoc)
        {
            string[] sParamList = new string[0];
            object[][] Params = new object[1][];
            Dictionary<string, string> dtItemPartNo = new Dictionary<string, string>();
            string sSQL = " select distinct WORK_ORDER,PART_ID from sajet.g_sn_travel "
                        + "  where serial_number=:SN  ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SN", sSN };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                string sPartId = dr["PART_ID"].ToString();
                string sWO = dr["WORK_ORDER"].ToString();
                //(2)找找工單發料清單此插件位置的零件料號
                sSQL = "SELECT WORK_ORDER, PART_ID, ITEM_PART_ID "
                     + " FROM SAJET.G_WO_BOM_LOCATION "
                    + "  WHERE PART_ID =:PART_ID "
                    + "  AND WORK_ORDER = :WORK_ORDER "
                    + "  AND LOCATION=:LOCATION ";

                Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartId };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOCATION", sLoc };
                DataSet dsComponent = ClientUtils.ExecuteSQL(sSQL, Params);
                for (int j = 0; j <= dsComponent.Tables[0].Rows.Count - 1; j++)
                {
                    string sWOComponent = dsComponent.Tables[0].Rows[j]["WORK_ORDER"].ToString();
                    string sPartComponent = dsComponent.Tables[0].Rows[j]["PART_ID"].ToString();
                    string sItemPartID = dsComponent.Tables[0].Rows[j]["ITEM_PART_ID"].ToString();
                    //(3)找零件料號是否有替代料
                    string sSQL1 = "SELECT A.ITEM_GROUP,B.PART_NO "
                                 + "  FROM SAJET.G_WO_BOM A, SAJET.SYS_PART B  "
                                 + " WHERE A.WORK_ORDER=:WORK_ORDER "
                                 + "   AND A.PART_ID = :PART_ID "
                                 + "   AND A.ITEM_PART_ID=:ITEM_PART_ID "
                                 + "   AND A.ITEM_PART_ID = B.PART_ID ";
                    Params = new object[3][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWOComponent };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartComponent };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                    DataSet dsItemGroup = ClientUtils.ExecuteSQL(sSQL1, Params);
                    if (dsItemGroup.Tables[0].Rows.Count == 0)//工單發料單沒有此料號,略過
                        continue;
                    string sItemGroup = dsItemGroup.Tables[0].Rows[0]["ITEM_GROUP"].ToString();
                    if (!dtItemPartNo.ContainsKey(dsItemGroup.Tables[0].Rows[0]["PART_NO"].ToString()))
                    {
                        dtItemPartNo.Add(dsItemGroup.Tables[0].Rows[0]["PART_NO"].ToString(), dsItemGroup.Tables[0].Rows[0]["PART_NO"].ToString());
                    }
                    if (sItemGroup == "0") //沒有替代料,略過下面步驟
                        continue;

                    //找出替代料的料號
                    string sSQL2 = "SELECT A.ITEM_GROUP,B.PART_NO "
                      + "  FROM SAJET.G_WO_BOM A,SAJET.SYS_PART B   "
                      + " WHERE A.WORK_ORDER=:WORK_ORDER "
                      + "   AND A.PART_ID =:PART_ID "
                      + "   AND A.ITEM_GROUP =:ITEM_GROUP "
                      + "   AND A.ITEM_PART_ID = B.PART_ID ";
                    Params = new object[3][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWOComponent };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartComponent };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_GROUP", sItemGroup };
                    DataSet dsAlternative = ClientUtils.ExecuteSQL(sSQL2, Params);
                    for (int k = 0; k <= dsAlternative.Tables[0].Rows.Count - 1; k++)
                    {
                        if (!dtItemPartNo.ContainsKey(dsAlternative.Tables[0].Rows[k]["PART_NO"].ToString()))
                        {
                            dtItemPartNo.Add(dsAlternative.Tables[0].Rows[k]["PART_NO"].ToString(), dsAlternative.Tables[0].Rows[k]["PART_NO"].ToString());
                        }
                    }
                }
            }
            sParamList = new string[dtItemPartNo.Count];
            int iIndex = 0;
            foreach (KeyValuePair<string, string> Item in dtItemPartNo)
            {
                sParamList[iIndex] = Item.Key;
                iIndex += 1;
            }
            return sParamList;
        }
        public static string[] GET_LOCATION_ITEM_PART(string sSN, string sLoc)
        {
            string[] sParamList = new string[0];
            object[][] Params = new object[1][];
            string sSQL = " select distinct PART_ID,VERSION from sajet.g_sn_travel "
                        + "  where serial_number=:SN  ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SN", sSN };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                sSQL = " select PART_ID,VERSION from sajet.g_sn_status "
                       + "  where serial_number=:SN   AND ROWNUM = 1 ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                sSQL = " select ITEM_PART_ID PART_ID ,VERSION FROM SAJET.G_SN_KEYPARTS "
                      + "  WHERE  ITEM_PART_SN=:SN  AND ROWNUM = 1 ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }
            Dictionary<string, string> dtItemPartNo = new Dictionary<string, string>();
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                string sPartID = dr["PART_ID"].ToString();
                string sVersion = dr["VERSION"].ToString();
                if (string.IsNullOrEmpty(sVersion))
                    sVersion = "N/A";
                Params = new object[3][];
                //(2)找單一BOM插件位置對應的料號
                sSQL = "SELECT B.BOM_ID,B.ITEM_PART_ID "
                     + " FROM SAJET.SYS_BOM_INFO A,SAJET.SYS_BOM_LOCATION B    "
                    + "  where A.PART_ID =:PART_ID "
                    + "    and A.VERSION = :VERSION "
                    + "    AND A.BOM_ID = B.BOM_ID "
                    + "    AND B.LOCATION=:LOCATION "
                    + "  GROUP BY B.BOM_ID,B.ITEM_PART_ID ";
                Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sVersion };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOCATION", sLoc };
                DataSet dsComponent = ClientUtils.ExecuteSQL(sSQL, Params);
                for (int j = 0; j <= dsComponent.Tables[0].Rows.Count - 1; j++)
                {
                    string sBOMID = dsComponent.Tables[0].Rows[j]["BOM_ID"].ToString();
                    string sItemPartID = dsComponent.Tables[0].Rows[j]["ITEM_PART_ID"].ToString();
                    //(3)找零件料號是否有替代料
                    string sSQL1 = "SELECT A.ITEM_GROUP,B.PART_NO "
                                 + "  FROM SAJET.SYS_BOM A,SAJET.SYS_PART B   "
                                 + " WHERE A.BOM_ID=:BOM_ID "
                                 + "   AND A.ITEM_PART_ID=:ITEM_PART_ID "
                                 + "   AND A.ITEM_PART_ID = B.PART_ID ";
                    Params = new object[2][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBOMID };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                    DataSet dsItemGroup = ClientUtils.ExecuteSQL(sSQL1, Params);
                    if (dsItemGroup.Tables[0].Rows.Count == 0)
                        continue;
                    string sItemGroup = dsItemGroup.Tables[0].Rows[0]["ITEM_GROUP"].ToString();
                    if (!dtItemPartNo.ContainsKey(dsItemGroup.Tables[0].Rows[0]["PART_NO"].ToString()))
                    {
                        dtItemPartNo.Add(dsItemGroup.Tables[0].Rows[0]["PART_NO"].ToString(), dsItemGroup.Tables[0].Rows[0]["PART_NO"].ToString());
                    }

                    if (sItemGroup == "0")
                        continue;

                    //找出替代料的料號
                    string sSQL2 = "SELECT A.ITEM_GROUP,B.PART_NO "
                      + "  FROM SAJET.SYS_BOM A,SAJET.SYS_PART B   "
                      + " WHERE A.BOM_ID=:BOM_ID "
                      + "   AND A.ITEM_GROUP =:ITEM_GROUP "
                      + "   AND A.ITEM_PART_ID = B.PART_ID ";
                    Params = new object[2][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBOMID };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_GROUP", sItemGroup };
                    DataSet dsAlternative = ClientUtils.ExecuteSQL(sSQL2, Params);
                    for (int k = 0; k <= dsAlternative.Tables[0].Rows.Count - 1; k++)
                    {

                        if (!dtItemPartNo.ContainsKey(dsAlternative.Tables[0].Rows[k]["PART_NO"].ToString()))
                        {
                            dtItemPartNo.Add(dsAlternative.Tables[0].Rows[k]["PART_NO"].ToString(), dsAlternative.Tables[0].Rows[k]["PART_NO"].ToString());
                        }
                    }
                }

            }
            sParamList = new string[dtItemPartNo.Count];
            int iIndex = 0;
            foreach (KeyValuePair<string, string> Item in dtItemPartNo)
            {
                sParamList[iIndex] = Item.Key;
                iIndex += 1;
            }
            return sParamList;
        }


    }
}
