using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CBOM.Helper
{
    public class CommonHelper
    {
        public void AdjustListViewColumnWidth(System.Windows.Forms.ListView listView, int columnIndex)
        {
            if (listView.Columns.Count <= columnIndex) return;

            ColumnHeader col = listView.Columns[columnIndex];

            using (Graphics g = listView.CreateGraphics())
            {
                // 1️⃣ 測量 Column Header (標題) 的寬度
                int headerWidth = (int)g.MeasureString(col.Text, listView.Font).Width + 10;

                // 2️⃣ 測量內容最大寬度
                int maxContentWidth = 0;
                foreach (ListViewItem item in listView.Items)
                {
                    string text = item.SubItems[columnIndex].Text;
                    int width = (int)g.MeasureString(text, listView.Font).Width + 10;

                    if (width > maxContentWidth)
                        maxContentWidth = width;
                }

                // 3️⃣ 使用最大寬度設定欄位
                col.Width = Math.Max(headerWidth, maxContentWidth);
            }
        }


        public void OracleBulkInsert(DataTable dt, string tableName, int batchSize = 200)
        {
            if (dt == null || dt.Rows.Count == 0)
                return;

            int count = 0;
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("INSERT ALL");

            foreach (DataRow row in dt.Rows)
            {
                sb.Append($" INTO {tableName} (");

                // 欄位名稱
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append(dt.Columns[i].ColumnName);

                    if (i < dt.Columns.Count - 1)
                        sb.Append(",");
                }

                sb.Append(") VALUES (");

                // 欄位值
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    object value = row[i];

                    if (value == DBNull.Value)
                    {
                        sb.Append("NULL");
                    }
                    else if (value is string)
                    {
                        sb.Append($"'{value.ToString().Replace("'", "''")}'");
                    }
                    else if (value is DateTime)
                    {
                        DateTime dtValue = (DateTime)value;
                        sb.Append($"TO_DATE('{dtValue:yyyy-MM-dd HH:mm:ss}','YYYY-MM-DD HH24:MI:SS')");
                    }
                    else
                    {
                        sb.Append(value.ToString());
                    }

                    if (i < dt.Columns.Count - 1)
                        sb.Append(",");
                }

                sb.AppendLine(")");

                count++;

                // 批次送出
                if (count % batchSize == 0)
                {
                    sb.AppendLine("SELECT * FROM dual");

                    ClientUtils.ExecuteSQL(sb.ToString());

                    sb.Clear();
                    sb.AppendLine("INSERT ALL");
                }
            }

            // 最後剩餘資料
            if (count % batchSize != 0)
            {
                sb.AppendLine("SELECT * FROM dual");
                ClientUtils.ExecuteSQL(sb.ToString());
            }
        }
    }
}
