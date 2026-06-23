namespace CWoManagerPcs
{
    partial class fSnSpecStatus
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.dgvSpecStatus = new System.Windows.Forms.DataGridView();
            this.colWorkOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSerialNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSerialStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpdateUserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSpecStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFilter
            // 
            this.txtFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtFilter.Location = new System.Drawing.Point(80, 15);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(300, 29);
            this.txtFilter.TabIndex = 0;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblFilter.Location = new System.Drawing.Point(15, 20);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(59, 25);
            this.lblFilter.TabIndex = 1;
            this.lblFilter.Text = "Filter:";
            // 
            // dgvSpecStatus
            // 
            this.dgvSpecStatus.AllowUserToAddRows = false;
            this.dgvSpecStatus.AllowUserToDeleteRows = false;
            this.dgvSpecStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSpecStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWorkOrder,
            this.colSerialNumber,
            this.colSerialStatus,
            this.colUpdateUserID,
            this.colCreateTime});
            this.dgvSpecStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvSpecStatus.Location = new System.Drawing.Point(0, 60);
            this.dgvSpecStatus.Name = "dgvSpecStatus";
            this.dgvSpecStatus.ReadOnly = true;
            this.dgvSpecStatus.RowTemplate.Height = 27;
            this.dgvSpecStatus.Size = new System.Drawing.Size(800, 340);
            this.dgvSpecStatus.TabIndex = 2;
            this.dgvSpecStatus.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSpecStatus_KeyDown);
            // 
            // colWorkOrder
            // 
            this.colWorkOrder.DataPropertyName = "WORK_ORDER";
            this.colWorkOrder.HeaderText = "Work Order";
            this.colWorkOrder.Name = "colWorkOrder";
            this.colWorkOrder.ReadOnly = true;
            // 
            // colSerialNumber
            // 
            this.colSerialNumber.DataPropertyName = "SERIAL_NUMBER";
            this.colSerialNumber.HeaderText = "Serial Number";
            this.colSerialNumber.Name = "colSerialNumber";
            this.colSerialNumber.ReadOnly = true;
            // 
            // colSerialStatus
            // 
            this.colSerialStatus.DataPropertyName = "SERIAL_STATUS";
            this.colSerialStatus.HeaderText = "Serial Status";
            this.colSerialStatus.Name = "colSerialStatus";
            this.colSerialStatus.ReadOnly = true;
            // 
            // colUpdateUserID
            // 
            this.colUpdateUserID.DataPropertyName = "UPDATE_USERID";
            this.colUpdateUserID.HeaderText = "Update User ID";
            this.colUpdateUserID.Name = "colUpdateUserID";
            this.colUpdateUserID.ReadOnly = true;
            // 
            // colCreateTime
            // 
            this.colCreateTime.DataPropertyName = "CREATE_TIME";
            this.colCreateTime.HeaderText = "Create Time";
            this.colCreateTime.Name = "colCreateTime";
            this.colCreateTime.ReadOnly = true;
            // 
            // fSnSpecStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 400);
            this.Controls.Add(this.dgvSpecStatus);
            this.Controls.Add(this.lblFilter);
            this.Controls.Add(this.txtFilter);
            this.Name = "fSnSpecStatus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "G_SN_SPEC_STATUS";
            this.Load += new System.EventHandler(this.fSnSpecStatus_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSpecStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.DataGridView dgvSpecStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWorkOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSerialNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSerialStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUpdateUserID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateTime;
    }
}