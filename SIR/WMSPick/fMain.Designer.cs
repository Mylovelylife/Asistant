namespace WMSPick
{
    partial class fMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnToERP = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.lablQty = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtData = new System.Windows.Forms.TextBox();
            this.lab_BoxNo = new System.Windows.Forms.Label();
            this.combRequest = new System.Windows.Forms.ComboBox();
            this.lab_RequestID = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lablMsg = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.TBL1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv_PartNo = new System.Windows.Forms.DataGridView();
            this.col_PartNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Warehouse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_PickQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_UnpickQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gv_location = new System.Windows.Forms.DataGridView();
            this.SELECT = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ReelID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LOCATION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STATUS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATECODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DC_FACTORY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lb_color = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.TBL1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_PartNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_location)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lb_color);
            this.panel1.Controls.Add(this.btnToERP);
            this.panel1.Controls.Add(this.btnFinish);
            this.panel1.Controls.Add(this.lablQty);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.txtData);
            this.panel1.Controls.Add(this.lab_BoxNo);
            this.panel1.Controls.Add(this.combRequest);
            this.panel1.Controls.Add(this.lab_RequestID);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1262, 212);
            this.panel1.TabIndex = 24;
            // 
            // btnToERP
            // 
            this.btnToERP.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.btnToERP.Location = new System.Drawing.Point(584, 80);
            this.btnToERP.Margin = new System.Windows.Forms.Padding(4);
            this.btnToERP.Name = "btnToERP";
            this.btnToERP.Size = new System.Drawing.Size(129, 42);
            this.btnToERP.TabIndex = 88;
            this.btnToERP.Text = "To ERP";
            this.btnToERP.UseVisualStyleBackColor = true;
            this.btnToERP.Click += new System.EventHandler(this.btnToERP_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.btnFinish.Location = new System.Drawing.Point(584, 139);
            this.btnFinish.Margin = new System.Windows.Forms.Padding(4);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(129, 42);
            this.btnFinish.TabIndex = 87;
            this.btnFinish.Text = "Finish";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // lablQty
            // 
            this.lablQty.AutoEllipsis = true;
            this.lablQty.BackColor = System.Drawing.Color.Transparent;
            this.lablQty.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.lablQty.ForeColor = System.Drawing.Color.Maroon;
            this.lablQty.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablQty.Location = new System.Drawing.Point(229, 144);
            this.lablQty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lablQty.Name = "lablQty";
            this.lablQty.Size = new System.Drawing.Size(336, 38);
            this.lablQty.TabIndex = 86;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(41, 141);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 36);
            this.label1.TabIndex = 84;
            this.label1.Text = "QTY";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.btnRefresh.Location = new System.Drawing.Point(584, 16);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(129, 42);
            this.btnRefresh.TabIndex = 83;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtData
            // 
            this.txtData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.txtData.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtData.Location = new System.Drawing.Point(229, 79);
            this.txtData.Margin = new System.Windows.Forms.Padding(4);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(335, 41);
            this.txtData.TabIndex = 4;
            this.txtData.Click += new System.EventHandler(this.txtData_Click);
            this.txtData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtData_KeyPress);
            // 
            // lab_BoxNo
            // 
            this.lab_BoxNo.AutoSize = true;
            this.lab_BoxNo.BackColor = System.Drawing.Color.Transparent;
            this.lab_BoxNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_BoxNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lab_BoxNo.Location = new System.Drawing.Point(41, 79);
            this.lab_BoxNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lab_BoxNo.Name = "lab_BoxNo";
            this.lab_BoxNo.Size = new System.Drawing.Size(67, 36);
            this.lab_BoxNo.TabIndex = 29;
            this.lab_BoxNo.Text = "Box";
            // 
            // combRequest
            // 
            this.combRequest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.combRequest.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.combRequest.FormattingEnabled = true;
            this.combRequest.Location = new System.Drawing.Point(229, 15);
            this.combRequest.Margin = new System.Windows.Forms.Padding(4);
            this.combRequest.Name = "combRequest";
            this.combRequest.Size = new System.Drawing.Size(335, 44);
            this.combRequest.TabIndex = 1;
            this.combRequest.SelectedIndexChanged += new System.EventHandler(this.combRequest_SelectedIndexChanged);
            // 
            // lab_RequestID
            // 
            this.lab_RequestID.AutoSize = true;
            this.lab_RequestID.BackColor = System.Drawing.Color.Transparent;
            this.lab_RequestID.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_RequestID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lab_RequestID.Location = new System.Drawing.Point(41, 21);
            this.lab_RequestID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lab_RequestID.Name = "lab_RequestID";
            this.lab_RequestID.Size = new System.Drawing.Size(163, 36);
            this.lab_RequestID.TabIndex = 23;
            this.lab_RequestID.Text = "Request ID";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lablMsg);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 212);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1262, 106);
            this.panel2.TabIndex = 25;
            // 
            // lablMsg
            // 
            this.lablMsg.AutoEllipsis = true;
            this.lablMsg.BackColor = System.Drawing.Color.White;
            this.lablMsg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lablMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.30189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lablMsg.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablMsg.Location = new System.Drawing.Point(0, 0);
            this.lablMsg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lablMsg.Name = "lablMsg";
            this.lablMsg.Size = new System.Drawing.Size(1262, 106);
            this.lablMsg.TabIndex = 30;
            this.lablMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.TBL1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.panel3.Location = new System.Drawing.Point(0, 318);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1262, 401);
            this.panel3.TabIndex = 26;
            // 
            // TBL1
            // 
            this.TBL1.ColumnCount = 1;
            this.TBL1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TBL1.Controls.Add(this.dgv_PartNo, 0, 0);
            this.TBL1.Controls.Add(this.gv_location, 0, 1);
            this.TBL1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBL1.Location = new System.Drawing.Point(0, 0);
            this.TBL1.Name = "TBL1";
            this.TBL1.RowCount = 2;
            this.TBL1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TBL1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TBL1.Size = new System.Drawing.Size(1262, 401);
            this.TBL1.TabIndex = 2;
            // 
            // dgv_PartNo
            // 
            this.dgv_PartNo.AllowUserToAddRows = false;
            this.dgv_PartNo.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dgv_PartNo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_PartNo.BackgroundColor = System.Drawing.Color.White;
            this.dgv_PartNo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_PartNo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_PartNo,
            this.col_Warehouse,
            this.col_Qty,
            this.col_PickQty,
            this.col_UnpickQty,
            this.col_Desc});
            this.dgv_PartNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_PartNo.Location = new System.Drawing.Point(4, 4);
            this.dgv_PartNo.Margin = new System.Windows.Forms.Padding(4);
            this.dgv_PartNo.Name = "dgv_PartNo";
            this.dgv_PartNo.ReadOnly = true;
            this.dgv_PartNo.RowHeadersWidth = 51;
            this.dgv_PartNo.RowTemplate.Height = 23;
            this.dgv_PartNo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_PartNo.Size = new System.Drawing.Size(1254, 192);
            this.dgv_PartNo.TabIndex = 1;
            this.dgv_PartNo.SelectionChanged += new System.EventHandler(this.dgv_PartNo_SelectionChanged);
            // 
            // col_PartNo
            // 
            this.col_PartNo.HeaderText = "Part No";
            this.col_PartNo.MinimumWidth = 6;
            this.col_PartNo.Name = "col_PartNo";
            this.col_PartNo.ReadOnly = true;
            this.col_PartNo.Width = 200;
            // 
            // col_Warehouse
            // 
            this.col_Warehouse.HeaderText = "Warehouse";
            this.col_Warehouse.MinimumWidth = 6;
            this.col_Warehouse.Name = "col_Warehouse";
            this.col_Warehouse.ReadOnly = true;
            this.col_Warehouse.Width = 125;
            // 
            // col_Qty
            // 
            this.col_Qty.HeaderText = "Qty";
            this.col_Qty.MinimumWidth = 6;
            this.col_Qty.Name = "col_Qty";
            this.col_Qty.ReadOnly = true;
            this.col_Qty.Width = 125;
            // 
            // col_PickQty
            // 
            this.col_PickQty.HeaderText = "Pick Qty";
            this.col_PickQty.MinimumWidth = 6;
            this.col_PickQty.Name = "col_PickQty";
            this.col_PickQty.ReadOnly = true;
            this.col_PickQty.Width = 125;
            // 
            // col_UnpickQty
            // 
            this.col_UnpickQty.HeaderText = "Unpick Qty";
            this.col_UnpickQty.MinimumWidth = 6;
            this.col_UnpickQty.Name = "col_UnpickQty";
            this.col_UnpickQty.ReadOnly = true;
            this.col_UnpickQty.Width = 125;
            // 
            // col_Desc
            // 
            this.col_Desc.HeaderText = "Desc";
            this.col_Desc.MinimumWidth = 6;
            this.col_Desc.Name = "col_Desc";
            this.col_Desc.ReadOnly = true;
            this.col_Desc.Width = 125;
            // 
            // gv_location
            // 
            this.gv_location.AllowUserToAddRows = false;
            this.gv_location.AllowUserToDeleteRows = false;
            this.gv_location.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.gv_location.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.gv_location.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gv_location.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SELECT,
            this.ReelID,
            this.QTY,
            this.LOCATION,
            this.STATUS,
            this.DATECODE,
            this.DC_FACTORY});
            this.gv_location.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gv_location.Location = new System.Drawing.Point(3, 203);
            this.gv_location.Name = "gv_location";
            this.gv_location.RowHeadersWidth = 51;
            this.gv_location.RowTemplate.Height = 27;
            this.gv_location.Size = new System.Drawing.Size(1256, 195);
            this.gv_location.TabIndex = 2;
            this.gv_location.CurrentCellDirtyStateChanged += new System.EventHandler(this.gv_location_CurrentCellDirtyStateChanged);
            // 
            // SELECT
            // 
            this.SELECT.FalseValue = "N";
            this.SELECT.HeaderText = "SELECT";
            this.SELECT.MinimumWidth = 6;
            this.SELECT.Name = "SELECT";
            this.SELECT.TrueValue = "Y";
            this.SELECT.Width = 117;
            // 
            // ReelID
            // 
            this.ReelID.DataPropertyName = "BOX_NO";
            this.ReelID.HeaderText = "ReelID";
            this.ReelID.MinimumWidth = 6;
            this.ReelID.Name = "ReelID";
            this.ReelID.ReadOnly = true;
            this.ReelID.Width = 118;
            // 
            // QTY
            // 
            this.QTY.DataPropertyName = "BOX_QTY";
            this.QTY.HeaderText = "QTY";
            this.QTY.MinimumWidth = 6;
            this.QTY.Name = "QTY";
            this.QTY.ReadOnly = true;
            this.QTY.Width = 92;
            // 
            // LOCATION
            // 
            this.LOCATION.DataPropertyName = "LOCATION_NO";
            this.LOCATION.HeaderText = "LOCATION";
            this.LOCATION.MinimumWidth = 6;
            this.LOCATION.Name = "LOCATION";
            this.LOCATION.ReadOnly = true;
            this.LOCATION.Width = 168;
            // 
            // STATUS
            // 
            this.STATUS.DataPropertyName = "STATUS";
            this.STATUS.HeaderText = "STATUS";
            this.STATUS.MinimumWidth = 6;
            this.STATUS.Name = "STATUS";
            this.STATUS.ReadOnly = true;
            this.STATUS.Width = 141;
            // 
            // DATECODE
            // 
            this.DATECODE.DataPropertyName = "DATECODE";
            this.DATECODE.HeaderText = "D/C";
            this.DATECODE.MinimumWidth = 6;
            this.DATECODE.Name = "DATECODE";
            this.DATECODE.ReadOnly = true;
            this.DATECODE.Width = 85;
            // 
            // DC_FACTORY
            // 
            this.DC_FACTORY.DataPropertyName = "DC_FACTORY";
            this.DC_FACTORY.HeaderText = "DC FACTORY";
            this.DC_FACTORY.MinimumWidth = 6;
            this.DC_FACTORY.Name = "DC_FACTORY";
            this.DC_FACTORY.Width = 185;
            // 
            // lb_color
            // 
            this.lb_color.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_color.AutoSize = true;
            this.lb_color.ForeColor = System.Drawing.Color.Red;
            this.lb_color.Location = new System.Drawing.Point(973, 21);
            this.lb_color.Name = "lb_color";
            this.lb_color.Size = new System.Drawing.Size(123, 15);
            this.lb_color.TabIndex = 89;
            this.lb_color.Text = "紅色：已拋轉ERP";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Green;
            this.label2.Location = new System.Drawing.Point(973, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 15);
            this.label2.TabIndex = 90;
            this.label2.Text = "綠色：已滿足發料量";
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 719);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fMain";
            this.Text = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.TBL1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_PartNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_location)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lab_RequestID;
        private System.Windows.Forms.ComboBox combRequest;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label lab_BoxNo;
        private System.Windows.Forms.Label lablMsg;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgv_PartNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_PartNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Warehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_PickQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_UnpickQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Desc;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Label lablQty;
        private System.Windows.Forms.Button btnToERP;
        private System.Windows.Forms.TableLayoutPanel TBL1;
        private System.Windows.Forms.DataGridView gv_location;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SELECT;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReelID;
        private System.Windows.Forms.DataGridViewTextBoxColumn QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOCATION;
        private System.Windows.Forms.DataGridViewTextBoxColumn STATUS;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATECODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn DC_FACTORY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb_color;
    }
}

