using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.OracleClient;

namespace SajetAuto
{
    public  class SajetAutoComplete
    {
        /// <summary>
        /// 自動完成功能 
        /// 2011/05/27 V1.0 by 宇睿
        /// </summary>
        /// <param name="AutoCompleteKey">自動完成的欄位資料</param>
        /// <param name="DBTable">自動完成的資料庫來源表</param>
        /// <param name="txt">自動完成的文字方塊控制項</param>
        public static void AutoCompleteFunction(string AutoCompleteKeys, string DBTable, TextBox txt)
        {
            txt.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txt.AutoCompleteCustomSource.Clear();
            string strSQL = "SELECT " + AutoCompleteKeys + " FROM " + DBTable;
            DataSet dsAutoCompleteTemp = ClientUtils.ExecuteSQL(strSQL);
            if (dsAutoCompleteTemp.Tables[0].Rows.Count > 0)
                for (int i = 0; i < dsAutoCompleteTemp.Tables[0].Rows.Count; i++)
                    txt.AutoCompleteCustomSource.Add(dsAutoCompleteTemp.Tables[0].Rows[i][0].ToString());

        }
    }
}
