namespace RepairDll
{
    partial class fCheck
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該公開 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labEmp = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.editEmp = new System.Windows.Forms.TextBox();
            this.labSN = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lbSN = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labEmp);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.editEmp);
            this.panel3.Location = new System.Drawing.Point(0, 250);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(414, 70);
            this.panel3.TabIndex = 7;
            // 
            // labEmp
            // 
            this.labEmp.AutoSize = true;
            this.labEmp.Font = new System.Drawing.Font("新細明體", 12F);
            this.labEmp.ForeColor = System.Drawing.Color.Maroon;
            this.labEmp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labEmp.Location = new System.Drawing.Point(293, 29);
            this.labEmp.Name = "labEmp";
            this.labEmp.Size = new System.Drawing.Size(34, 16);
            this.labEmp.TabIndex = 2;
            this.labEmp.Text = "N/A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F);
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(25, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Comfirm EmpID";
            // 
            // editEmp
            // 
            this.editEmp.Font = new System.Drawing.Font("新細明體", 12F);
            this.editEmp.Location = new System.Drawing.Point(141, 26);
            this.editEmp.Name = "editEmp";
            this.editEmp.Size = new System.Drawing.Size(146, 27);
            this.editEmp.TabIndex = 0;
            this.editEmp.TextChanged += new System.EventHandler(this.editEmp_TextChanged);
            this.editEmp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editEmp_KeyPress);
            // 
            // labSN
            // 
            this.labSN.AutoSize = true;
            this.labSN.Font = new System.Drawing.Font("新細明體", 12F);
            this.labSN.ForeColor = System.Drawing.Color.Maroon;
            this.labSN.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labSN.Location = new System.Drawing.Point(168, 28);
            this.labSN.Name = "labSN";
            this.labSN.Size = new System.Drawing.Size(34, 16);
            this.labSN.TabIndex = 1;
            this.labSN.Text = "N/A";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.labSN);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(414, 78);
            this.panel2.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lbSN);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(414, 78);
            this.panel4.TabIndex = 5;
            // 
            // lbSN
            // 
            this.lbSN.AutoSize = true;
            this.lbSN.Font = new System.Drawing.Font("新細明體", 12F);
            this.lbSN.ForeColor = System.Drawing.Color.Maroon;
            this.lbSN.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbSN.Location = new System.Drawing.Point(143, 28);
            this.lbSN.Name = "lbSN";
            this.lbSN.Size = new System.Drawing.Size(34, 16);
            this.lbSN.TabIndex = 1;
            this.lbSN.Text = "N/A";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 12F);
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(25, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Serial Number";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(25, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Serial Number";
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dgvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvData.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvData.DefaultCellStyle = dataGridViewCellStyle15;
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.Location = new System.Drawing.Point(0, 0);
            this.dgvData.MultiSelect = false;
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.RowHeadersWidth = 25;
            this.dgvData.RowTemplate.Height = 24;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvData.Size = new System.Drawing.Size(414, 172);
            this.dgvData.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 309);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(414, 57);
            this.panel1.TabIndex = 6;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(125, 16);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("新細明體", 11.25F);
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(227, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dgvData);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 78);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(414, 172);
            this.panel5.TabIndex = 8;
            // 
            // fCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 366);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fCheck";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Check";
            this.Load += new System.EventHandler(this.fCheck_Load);
            this.Shown += new System.EventHandler(this.fCheck_Shown);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labEmp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox editEmp;
        private System.Windows.Forms.Label labSN;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lbSN;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel5;

    }
}