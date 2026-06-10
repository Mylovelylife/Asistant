
namespace CWoManagerPcs
{
    partial class fMultiSelect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgv_Select = new SajetMES.UI.DataViewer();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Select)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_Select
            // 
            this.dgv_Select.AlwaysShowChecked = true;
            this.dgv_Select.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_Select.AutoScroll = true;
            this.dgv_Select.CheckedColumnName = "CHECKED";
            this.dgv_Select.CheckOnAnyCell = true;
            this.dgv_Select.CurrentCell = null;
            this.dgv_Select.DataSource = null;
            this.dgv_Select.EnableCheckAll = true;
            this.dgv_Select.EnableFilter = false;
            this.dgv_Select.EnableRowCount = false;
            this.dgv_Select.FilterComboboxWidth = 120;
            this.dgv_Select.FilterLabel = "Filter";
            this.dgv_Select.FilterLabelVisible = false;
            this.dgv_Select.FilterText = "";
            this.dgv_Select.FilterTextboxWidth = 180;
            this.dgv_Select.Location = new System.Drawing.Point(14, 0);
            this.dgv_Select.Margin = new System.Windows.Forms.Padding(2);
            this.dgv_Select.MultiSelect = true;
            this.dgv_Select.Name = "dgv_Select";
            this.dgv_Select.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv_Select.Size = new System.Drawing.Size(269, 162);
            this.dgv_Select.TabIndex = 6;
            this.dgv_Select.VisibleColumns = null;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(127, 170);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(208, 169);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // fMultiSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 193);
            this.Controls.Add(this.dgv_Select);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "fMultiSelect";
            this.Text = "fMultiSelect";
            this.Load += new System.EventHandler(this.fMultiSelect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Select)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public SajetMES.UI.DataViewer dgv_Select;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}