namespace CBOM
{
    partial class fErrorData
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvMsg = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.PART_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ITEM_PART = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PROCESS_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STEP_ITEM_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KEY_COMPONENT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ERR_MESSAGE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMsg)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvMsg
            // 
            this.dgvMsg.AllowUserToAddRows = false;
            this.dgvMsg.AllowUserToDeleteRows = false;
            this.dgvMsg.BackgroundColor = System.Drawing.Color.White;
            this.dgvMsg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMsg.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMsg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMsg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PART_NO,
            this.ITEM_PART,
            this.PROCESS_NAME,
            this.STEP_ITEM_NAME,
            this.KEY_COMPONENT,
            this.Qty,
            this.ERR_MESSAGE});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMsg.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMsg.Location = new System.Drawing.Point(0, 0);
            this.dgvMsg.Name = "dgvMsg";
            this.dgvMsg.ReadOnly = true;
            this.dgvMsg.RowHeadersWidth = 10;
            this.dgvMsg.RowTemplate.Height = 24;
            this.dgvMsg.Size = new System.Drawing.Size(864, 353);
            this.dgvMsg.TabIndex = 32;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 353);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(864, 40);
            this.panel1.TabIndex = 31;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(554, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // PART_NO
            // 
            this.PART_NO.HeaderText = "Part No";
            this.PART_NO.Name = "PART_NO";
            this.PART_NO.ReadOnly = true;
            this.PART_NO.Width = 150;
            // 
            // ITEM_PART
            // 
            this.ITEM_PART.HeaderText = "Item Part";
            this.ITEM_PART.Name = "ITEM_PART";
            this.ITEM_PART.ReadOnly = true;
            this.ITEM_PART.Width = 150;
            // 
            // PROCESS_NAME
            // 
            this.PROCESS_NAME.HeaderText = "Process Name";
            this.PROCESS_NAME.Name = "PROCESS_NAME";
            this.PROCESS_NAME.ReadOnly = true;
            this.PROCESS_NAME.Width = 150;
            // 
            // STEP_ITEM_NAME
            // 
            this.STEP_ITEM_NAME.HeaderText = "Step Name";
            this.STEP_ITEM_NAME.Name = "STEP_ITEM_NAME";
            this.STEP_ITEM_NAME.ReadOnly = true;
            // 
            // KEY_COMPONENT
            // 
            this.KEY_COMPONENT.HeaderText = "Key";
            this.KEY_COMPONENT.Name = "KEY_COMPONENT";
            this.KEY_COMPONENT.ReadOnly = true;
            // 
            // Qty
            // 
            this.Qty.HeaderText = "Qty";
            this.Qty.Name = "Qty";
            this.Qty.ReadOnly = true;
            // 
            // ERR_MESSAGE
            // 
            this.ERR_MESSAGE.HeaderText = "Error Message";
            this.ERR_MESSAGE.Name = "ERR_MESSAGE";
            this.ERR_MESSAGE.ReadOnly = true;
            this.ERR_MESSAGE.Width = 200;
            // 
            // fErrorData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 393);
            this.Controls.Add(this.dgvMsg);
            this.Controls.Add(this.panel1);
            this.Name = "fErrorData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Error Message";
            this.Load += new System.EventHandler(this.fErrorData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMsg)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgvMsg;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DataGridViewTextBoxColumn PART_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEM_PART;
        private System.Windows.Forms.DataGridViewTextBoxColumn PROCESS_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn STEP_ITEM_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn KEY_COMPONENT;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ERR_MESSAGE;
    }
}