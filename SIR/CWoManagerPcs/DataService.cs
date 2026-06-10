using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;

namespace CWoManager
{
    public class DataService
    {
        public static List<WOProperty> GetWOProperty(string workOrder, bool isDefault = false)
        {
            string sql = @"SELECT WP.PROPERTY_VALUE, P.PROPERTY_ID, P.PROPERTY_NAME, P.VALUE_DEFAULT, P.VALUE_TYPE, P.INPUT_TYPE, P.VALUE_LIST, P.NECESSARY, P.CONVERT_TYPE, P.SQL_SYNTAX, P.PROPERTY_DESC, P.ISREADONLY,P.IS_MULTI
                            FROM SAJET.SYS_PROPERTY P, (SELECT PROPERTY_ID, PROPERTY_VALUE FROM SAJET.G_WO_PROPERTY WHERE WORK_ORDER=:WORK_ORDER) WP 
                            WHERE P.ENABLED='Y' AND P.PROPERTY_TYPE='2' AND P.PROPERTY_ID=WP.PROPERTY_ID(+)
                            ORDER BY P.PROPERTY_NAME";

            using (DataTable dtTemp = ClientUtils.ExecuteSQL(sql, new object[][] {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", workOrder ?? string.Empty }}).Tables[0])
            {
                return dtTemp.AsEnumerable().Select(row => new WOProperty(row, isDefault)).ToList();
            }
        }

        public static void UpdateWOProperty(string workOrder, List<WOProperty> workOrderProps)
        {
            //先確認是否已有資料
            string sql = @"SELECT * FROM SAJET.G_WO_PROPERTY
                                  WHERE WORK_ORDER=:WORK_ORDER";
            DataSet dsWOProperty = ClientUtils.ExecuteSQL(sql, new object[][] { new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", workOrder } });

            //先記錄歷史資訊
            if (dsWOProperty.Tables[0].Rows.Count > 0)
            {
                //已有資料TYPE為M修改
                sql = @"INSERT INTO SAJET.G_HT_WO_PROPERTY 
                           SELECT WORK_ORDER,
                                  PROPERTY_ID,
                                  PROPERTY_VALUE,
                                  UPDATE_USERID,
                                  UPDATE_TIME,
                                  'M' TYPE
                             FROM SAJET.G_WO_PROPERTY
                            WHERE WORK_ORDER=:WORK_ORDER";
                ClientUtils.ExecuteSQL(sql, new object[][] { new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", workOrder } });
            }
            sql = "DELETE SAJET.G_WO_PROPERTY WHERE WORK_ORDER=:WORK_ORDER";
            ClientUtils.ExecuteSQL(sql, new object[][] { new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", workOrder } });

            DateTime dateTime = DateTime.Now;
            sql = @"INSERT INTO SAJET.G_WO_PROPERTY(WORK_ORDER, PROPERTY_ID, PROPERTY_VALUE, UPDATE_USERID, UPDATE_TIME)
                    VALUES(:WORK_ORDER, :PROPERTY_ID, :PROPERTY_VALUE, :UPDATE_USERID, :UPDATE_TIME)";
            foreach (WOProperty WOProp in workOrderProps.Where(p => !string.IsNullOrWhiteSpace(p.PropertyValue)))
            {
                ClientUtils.ExecuteSQL(sql, new object[][] {
                    new object[]{ ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", workOrder },
                    new object[]{ ParameterDirection.Input, OracleType.Number, "PROPERTY_ID", WOProp.PROPERTY_ID },
                    new object[]{ ParameterDirection.Input, OracleType.VarChar, "PROPERTY_VALUE", WOProp.PropertyValue },
                    new object[]{ ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", ClientUtils.UserPara1 },
                    new object[]{ ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dateTime }});
            }
            if (dsWOProperty.Tables[0].Rows.Count == 0)
            {
                //無資料首度新增 TYPE為A新增
                sql = @"INSERT INTO SAJET.G_HT_WO_PROPERTY 
                           SELECT WORK_ORDER,
                                  PROPERTY_ID,
                                  PROPERTY_VALUE,
                                  UPDATE_USERID,
                                  UPDATE_TIME,
                                  'A' TYPE
                             FROM SAJET.G_WO_PROPERTY
                            WHERE WORK_ORDER=:WORK_ORDER";
                ClientUtils.ExecuteSQL(sql, new object[][] { new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", workOrder } });
            }
        }

        public static bool ExistWorkOrder(string workOrder)
        {
            string sql = "SELECT * FROM SAJET.G_WO_BASE WHERE UPPER(WORK_ORDER)=UPPER(:WORK_ORDER)";
            DataSet dataSet = ClientUtils.ExecuteSQL(sql, new object[][] {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", workOrder.Trim() } });

            return dataSet.Tables[0].Rows.Count > 0;
        }
    }
}
