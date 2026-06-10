using System;
using System.Data;
using System.Linq;
using SajetClass;

namespace CWoManagerPcs
{
    public class WOProperty
    {
        public decimal PROPERTY_ID;
        /// <summary>
        /// 屬性名稱
        /// </summary>
        public string PROPERTY_NAME;
        /// <summary>
        /// 屬性預設值
        /// </summary>
        public string VALUE_DEFAULT;
        /// <summary>
        /// 數值類型(V:Varchar2,N:Number)
        /// </summary>
        public ValueType VALUE_TYPE;
        /// <summary>
        /// 輸入方式(K:KeyIn,S:SelectList,R:Range)
        /// </summary>
        public InputType INPUT_TYPE;
        /// <summary>
        /// 項目清單
        /// </summary>
        public string VALUE_LIST;
        /// <summary>
        /// 是否必須(Y:必要,N:非必要)
        /// </summary>
        public bool NECESSARY;
        /// <summary>
        /// 輸入值轉換(N:None,U:Uppercase,L:Lowercase)
        /// </summary>
        public ConvertType CONVERT_TYPE;

        public string SQL_SYNTAX;

        public string PROPERTY_VALUE;


        public string PROPERTY_DESC;
        /// <summary>
        /// 唯獨鎖定(Y:是,N:否)
        /// </summary>
        public string ISREADONLY;
        /// <summary>
        /// 唯獨鎖定(Y:是,N:否)
        /// </summary>
        public string IS_MULTI;

        public WOProperty(DataRow dataRow, bool isDefault = false)
        {
            PROPERTY_ID = Convert.ToDecimal(dataRow[nameof(PROPERTY_ID)]);
            PROPERTY_NAME = dataRow[nameof(PROPERTY_NAME)].ToString();
            VALUE_DEFAULT = dataRow[nameof(VALUE_DEFAULT)].ToString();
            VALUE_TYPE = ToEnum<ValueType>(dataRow[nameof(VALUE_TYPE)].ToString());
            INPUT_TYPE = ToEnum<InputType>(dataRow[nameof(INPUT_TYPE)].ToString());
            VALUE_LIST = dataRow[nameof(VALUE_LIST)].ToString();
            NECESSARY = dataRow[nameof(NECESSARY)].ToString() == "Y";
            CONVERT_TYPE = ToEnum<ConvertType>(dataRow[nameof(CONVERT_TYPE)].ToString());

            PROPERTY_VALUE = dataRow[nameof(PROPERTY_VALUE)].ToString();
            if (isDefault && string.IsNullOrEmpty(PROPERTY_VALUE))
                PROPERTY_VALUE = VALUE_DEFAULT;

            SQL_SYNTAX = dataRow[nameof(SQL_SYNTAX)].ToString();
            PROPERTY_DESC = dataRow[nameof(PROPERTY_DESC)].ToString();
            ISREADONLY=dataRow[nameof(ISREADONLY)].ToString();
            IS_MULTI = dataRow[nameof(IS_MULTI)].ToString();
        }

        private T ToEnum<T>(string value)
        {
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                // 回傳開頭字元符合傳入字串之 Enum.Item
                if (item.ToString().ToUpper().StartsWith(value.ToUpper()))
                    return item;
            }

            return (T)Enum.Parse(typeof(T), value, true);
        }

        public string[] ValueList()
        {
            return VALUE_LIST.Split(',').Where(v => !string.IsNullOrEmpty(v)).ToArray();
        }

        public string [] ListByQuery()
        {
            try
            {
                using (DataTable dt = ClientUtils.ExecuteSQL(SQL_SYNTAX).Tables[0])
                {
                    return dt.AsEnumerable().Select(c => Convert.ToString(c.Field<string>(0))).ToArray();
                }
            }
            catch
            {
                return new string[] { };
            }
            //return new string[] { };
        }

        public ErrorType Validate(string value = null)
        {
            if (value == null) value = PropertyValue;

            ErrorType error = ErrorType.None;

            // 必要輸入欄位
            if (NECESSARY && string.IsNullOrWhiteSpace(value))
                error = ErrorType.Necessary;
            else if (!string.IsNullOrWhiteSpace(value))
            {
                decimal numValue = 0;
                // 數值類型: 數字
                if (VALUE_TYPE == ValueType.Number && !decimal.TryParse(value, out numValue))
                    error = ErrorType.ValueType;
                // 輸入方式: 數值列表、範圍
                else if (INPUT_TYPE == InputType.Range)
                {
                    string[] valueRange = VALUE_LIST.Split(',');
                    decimal minValue = Convert.ToDecimal(valueRange.First());
                    decimal maxValue = Convert.ToDecimal(valueRange.Last());

                    if (numValue < minValue || numValue > maxValue)
                        error = ErrorType.InputType;
                }
                else if (INPUT_TYPE == InputType.SelectList && !ValueList().Contains(value))
                {
                    error = ErrorType.InputType;
                }
                else if (INPUT_TYPE == InputType.Query && !ListByQuery().Contains(value))
                {
                    error = ErrorType.InputType;
                }
            }

            return error;
        }

        public string PropertyName { get => PROPERTY_NAME; }

        public string DataType { get => SajetCommon.SetLanguage(VALUE_TYPE.ToString()); }

        public string PropertyValue
        {
            get { return PROPERTY_VALUE; }
            set
            {
                // 輸入值替換
                if (value != null)
                {
                    switch (CONVERT_TYPE)
                    {
                        case ConvertType.Uppercase:
                            value = value.ToUpper();
                            break;
                        case ConvertType.Lowercase:
                            value = value.ToLower();
                            break;
                    }
                }
                PROPERTY_VALUE = value;
            }
        }

        public string Specification
        { get => SajetCommon.SetLanguage(PROPERTY_DESC.ToString()); }

    }

    public enum ErrorType
    {
        None,
        Necessary,
        ValueType,
        InputType,
    }

    public enum ValueType
    {
        Varchar,
        Number
    }

    public enum InputType
    {
        KeyIn,
        SelectList,
        Range,
        Query  //SQL
    }

    public enum ConvertType
    {
        None,
        Uppercase,
        Lowercase
    }

    public enum WorkOrderStatus
    {
        Prepare
    }
}
