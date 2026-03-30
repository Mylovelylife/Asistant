using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Reflection;

namespace ProcessStepDll
{
    public partial class uctlProgressStatus : UserControl
    {
        private int _iTotalCount;
        private int _iProcessCount;       
        public int  iTotalCount
        {
            set
            {               
                _iTotalCount = value;
                _iProcessCount = 0;
            }
        }
      
        public uctlProgressStatus()
        {
            InitializeComponent();
            PropertyInfo info = this.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            info.SetValue(tableLayoutPanel1, true, null); 
        }
        public void AddCount(int iCount)
        {
            _iProcessCount = _iProcessCount + iCount;
            lablProcessCount.Text = _iProcessCount.ToString();
        }
        public void Initial()
        {
            SajetCommon.SetLanguageControl(this);
        }
        public void Clear()
        {
            _iProcessCount = 0;
            lablProcessCount.Text = _iProcessCount.ToString();
           
        }
        public void ClearALL()
        {
            _iTotalCount = 0;           
            Clear();
        }
        public void SetFont()
        {
            lablProcessCount.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular);           
        }
    }
}
