using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "PROCESS_ID";
        public static String gsDef_Table = "SAJET.SYS_PROCESS";
        public static String gsDef_HTTable = "SAJET.SYS_HT_PROCESS";
        public static String gsDef_OrderField = "PROCESS_CODE"; //預設排序欄位
        public static String gsDef_KeyData = "PROCESS_NAME";    //用於Disable時的訊息顯示

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        //Detail
        public static String gsDef_DtlKeyField = "STEP_ITEM_ID";
        public static String gsDef_DtlTable = "SAJET.SYS_STEP_ITEM";
        public static String gsDef_DtlHTTable = "SAJET.SYS_HT_STEP_ITEM";
        public static String gsDef_DtlOrderField = "STEP_ITEM_CODE"; //預設排序欄位
        public static String gsDef_DtlKeyData = "STEP_ITEM_NAME";    //用於Disable時的訊息顯示
        public struct TGridDetail_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGridDetail_Field[] tGridDetailField;


        public static void Initial_Table()
        {
            //要在Grid顯示出來的欄位及順序
            //Master ========================
            Array.Resize(ref tGridField, 4);
            tGridField[0].sFieldName = "PROCESS_NAME";
            tGridField[0].sCaption = "Process Name";
            tGridField[1].sFieldName = "PROCESS_DESC";
            tGridField[1].sCaption = "Description";
            tGridField[2].sFieldName = "PROCESS_DESC2";
            tGridField[2].sCaption = "Description2";
            tGridField[3].sFieldName = "PROCESS_CODE";
            tGridField[3].sCaption = "Process Code";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }

            //Detail===============
            Array.Resize(ref tGridDetailField, 5);
            tGridDetailField[0].sFieldName = "STEP_ITEM_CODE";
            tGridDetailField[0].sCaption = "Step Item Code";
            tGridDetailField[1].sFieldName = "STEP_ITEM_NAME";
            tGridDetailField[1].sCaption = "Step Item Name";
            tGridDetailField[2].sFieldName = "ATE_TEST";
            tGridDetailField[2].sCaption = "Accuracy Test";
            tGridDetailField[3].sFieldName = "ATE_VERIFY";
            tGridDetailField[3].sCaption = "Accuracy Verify";
            tGridDetailField[4].sFieldName = "NEED_CSN";
            tGridDetailField[4].sCaption = "Need CSN";
           
            //欄位多國語言
            for (int i = 0; i <= tGridDetailField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridDetailField[i].sCaption, 1);
                tGridDetailField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.Stage_Name,a.Stage_Desc,a.Stage_Code "
                     + "       ,a.ENABLED,b.emp_name,a.UPDATE_TIME "
                     + " from " + TableDefine.gsDef_HTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + " Where a." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
        public static string DetailHistory_SQL(string sID)
        {
           
            string s = string.Format(@"SELECT A.STEP_ITEM_CODE,A.STEP_ITEM_NAME,A.ENABLED
                                             ,B.EMP_NAME,A.UPDATE_TIME
                                       FROM SAJET.SYS_STEP_ITEM_HT A,SAJET.SYS_EMP B
                                       WHERE A.UPDATE_USERID=B.EMP_ID AND A.STEP_ITEM_ID={0} Order By a.Update_Time", sID);
            return s;
        }
    }
}
