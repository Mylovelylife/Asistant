namespace RepairDll
{
    partial class fReplace
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fReplace));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvKP = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnReplace = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.LVEC = new System.Windows.Forms.ListView();
            this.columnHeaderEC = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderDesc = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnSearchDefect = new System.Windows.Forms.Button();
            this.rdbtnYes = new System.Windows.Forms.RadioButton();
            this.rdbtnNo = new System.Windows.Forms.RadioButton();
            this.editDefect = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gbNewKP = new System.Windows.Forms.GroupBox();
            this.RichTextRemark = new System.Windows.Forms.RichTextBox();
            this.LabNewKPSN = new System.Windows.Forms.Label();
            this.LabRemark = new System.Windows.Forms.Label();
            this.editNewKPSN = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.LabSN = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.WORK_ORDER = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ITEM_PART_SN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ITEM_PART_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SPEC1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ASSY_PROCESS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ASSY_TIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ASSY_EMP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ITEM_PART_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ITEM_GROUP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PROCESS_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKP)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.gbNewKP.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvKP);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // dgvKP
            // 
            this.dgvKP.AllowUserToAddRows = false;
            this.dgvKP.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Lavender;
            this.dgvKP.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvKP.BackgroundColor = System.Drawing.Color.White;
            this.dgvKP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKP.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.WORK_ORDER,
            this.ITEM_PART_SN,
            this.ITEM_PART_NO,
            this.SPEC1,
            this.ASSY_PROCESS,
            this.ASSY_TIME,
            this.ASSY_EMP,
            this.ITEM_PART_ID,
            this.ITEM_GROUP,
            this.PROCESS_ID});
            resources.ApplyResources(this.dgvKP, "dgvKP");
            this.dgvKP.Name = "dgvKP";
            this.dgvKP.ReadOnly = true;
            this.dgvKP.RowTemplate.Height = 24;
            this.dgvKP.TabStop = false;
            this.dgvKP.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKP_CellClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnReplace);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.btnRemove);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnReplace
            // 
            resources.ApplyResources(this.btnReplace, "btnReplace");
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            resources.ApplyResources(this.btnRemove, "btnRemove");
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Controls.Add(this.gbNewKP);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.LVEC);
            this.groupBox2.Controls.Add(this.panel5);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // LVEC
            // 
            this.LVEC.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderEC,
            this.columnHeaderDesc});
            this.LVEC.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.LVEC, "LVEC");
            this.LVEC.FullRowSelect = true;
            this.LVEC.Name = "LVEC";
            this.LVEC.UseCompatibleStateImageBehavior = false;
            this.LVEC.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderEC
            // 
            resources.ApplyResources(this.columnHeaderEC, "columnHeaderEC");
            // 
            // columnHeaderDesc
            // 
            resources.ApplyResources(this.columnHeaderDesc, "columnHeaderDesc");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemDelete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // MenuItemDelete
            // 
            this.MenuItemDelete.Name = "MenuItemDelete";
            resources.ApplyResources(this.MenuItemDelete, "MenuItemDelete");
            this.MenuItemDelete.Click += new System.EventHandler(this.MenuItemDelete_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnSearchDefect);
            this.panel5.Controls.Add(this.rdbtnYes);
            this.panel5.Controls.Add(this.rdbtnNo);
            this.panel5.Controls.Add(this.editDefect);
            this.panel5.Controls.Add(this.label4);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // btnSearchDefect
            // 
            this.btnSearchDefect.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnSearchDefect, "btnSearchDefect");
            this.btnSearchDefect.Name = "btnSearchDefect";
            this.btnSearchDefect.UseVisualStyleBackColor = true;
            this.btnSearchDefect.Click += new System.EventHandler(this.btnSearchDefect_Click);
            // 
            // rdbtnYes
            // 
            resources.ApplyResources(this.rdbtnYes, "rdbtnYes");
            this.rdbtnYes.Name = "rdbtnYes";
            this.rdbtnYes.TabStop = true;
            this.rdbtnYes.UseVisualStyleBackColor = true;
            this.rdbtnYes.Click += new System.EventHandler(this.rdbtnYes_Click);
            // 
            // rdbtnNo
            // 
            resources.ApplyResources(this.rdbtnNo, "rdbtnNo");
            this.rdbtnNo.Name = "rdbtnNo";
            this.rdbtnNo.TabStop = true;
            this.rdbtnNo.UseVisualStyleBackColor = true;
            this.rdbtnNo.Click += new System.EventHandler(this.rdbtnNo_Click);
            // 
            // editDefect
            // 
            resources.ApplyResources(this.editDefect, "editDefect");
            this.editDefect.Name = "editDefect";
            this.editDefect.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editDefect_KeyPress);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // gbNewKP
            // 
            this.gbNewKP.Controls.Add(this.RichTextRemark);
            this.gbNewKP.Controls.Add(this.LabNewKPSN);
            this.gbNewKP.Controls.Add(this.LabRemark);
            this.gbNewKP.Controls.Add(this.editNewKPSN);
            resources.ApplyResources(this.gbNewKP, "gbNewKP");
            this.gbNewKP.Name = "gbNewKP";
            this.gbNewKP.TabStop = false;
            this.gbNewKP.Enter += new System.EventHandler(this.gbNewKP_Enter);
            // 
            // RichTextRemark
            // 
            resources.ApplyResources(this.RichTextRemark, "RichTextRemark");
            this.RichTextRemark.Name = "RichTextRemark";
            // 
            // LabNewKPSN
            // 
            resources.ApplyResources(this.LabNewKPSN, "LabNewKPSN");
            this.LabNewKPSN.Name = "LabNewKPSN";
            // 
            // LabRemark
            // 
            resources.ApplyResources(this.LabRemark, "LabRemark");
            this.LabRemark.Name = "LabRemark";
            // 
            // editNewKPSN
            // 
            resources.ApplyResources(this.editNewKPSN, "editNewKPSN");
            this.editNewKPSN.Name = "editNewKPSN";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightBlue;
            this.panel4.Controls.Add(this.LabSN);
            this.panel4.Controls.Add(this.label1);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // LabSN
            // 
            resources.ApplyResources(this.LabSN, "LabSN");
            this.LabSN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LabSN.ForeColor = System.Drawing.Color.Maroon;
            this.LabSN.Name = "LabSN";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // WORK_ORDER
            // 
            resources.ApplyResources(this.WORK_ORDER, "WORK_ORDER");
            this.WORK_ORDER.Name = "WORK_ORDER";
            this.WORK_ORDER.ReadOnly = true;
            // 
            // ITEM_PART_SN
            // 
            resources.ApplyResources(this.ITEM_PART_SN, "ITEM_PART_SN");
            this.ITEM_PART_SN.Name = "ITEM_PART_SN";
            this.ITEM_PART_SN.ReadOnly = true;
            // 
            // ITEM_PART_NO
            // 
            resources.ApplyResources(this.ITEM_PART_NO, "ITEM_PART_NO");
            this.ITEM_PART_NO.Name = "ITEM_PART_NO";
            this.ITEM_PART_NO.ReadOnly = true;
            // 
            // SPEC1
            // 
            resources.ApplyResources(this.SPEC1, "SPEC1");
            this.SPEC1.Name = "SPEC1";
            this.SPEC1.ReadOnly = true;
            // 
            // ASSY_PROCESS
            // 
            resources.ApplyResources(this.ASSY_PROCESS, "ASSY_PROCESS");
            this.ASSY_PROCESS.Name = "ASSY_PROCESS";
            this.ASSY_PROCESS.ReadOnly = true;
            // 
            // ASSY_TIME
            // 
            resources.ApplyResources(this.ASSY_TIME, "ASSY_TIME");
            this.ASSY_TIME.Name = "ASSY_TIME";
            this.ASSY_TIME.ReadOnly = true;
            // 
            // ASSY_EMP
            // 
            resources.ApplyResources(this.ASSY_EMP, "ASSY_EMP");
            this.ASSY_EMP.Name = "ASSY_EMP";
            this.ASSY_EMP.ReadOnly = true;
            // 
            // ITEM_PART_ID
            // 
            resources.ApplyResources(this.ITEM_PART_ID, "ITEM_PART_ID");
            this.ITEM_PART_ID.Name = "ITEM_PART_ID";
            this.ITEM_PART_ID.ReadOnly = true;
            // 
            // ITEM_GROUP
            // 
            resources.ApplyResources(this.ITEM_GROUP, "ITEM_GROUP");
            this.ITEM_GROUP.Name = "ITEM_GROUP";
            this.ITEM_GROUP.ReadOnly = true;
            // 
            // PROCESS_ID
            // 
            resources.ApplyResources(this.PROCESS_ID, "PROCESS_ID");
            this.PROCESS_ID.Name = "PROCESS_ID";
            this.PROCESS_ID.ReadOnly = true;
            // 
            // fReplace
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Name = "fReplace";
            this.Load += new System.EventHandler(this.fReplace_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKP)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.gbNewKP.ResumeLayout(false);
            this.gbNewKP.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label LabSN;
        private System.Windows.Forms.TextBox editNewKPSN;
        private System.Windows.Forms.Label LabNewKPSN;
        private System.Windows.Forms.RichTextBox RichTextRemark;
        private System.Windows.Forms.Label LabRemark;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdbtnNo;
        private System.Windows.Forms.RadioButton rdbtnYes;
        private System.Windows.Forms.TextBox editDefect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView LVEC;
        private System.Windows.Forms.ColumnHeader columnHeaderEC;
        private System.Windows.Forms.ColumnHeader columnHeaderDesc;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDelete;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.GroupBox gbNewKP;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Button btnSearchDefect;
        private System.Windows.Forms.DataGridView dgvKP;
        private System.Windows.Forms.DataGridViewTextBoxColumn WORK_ORDER;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEM_PART_SN;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEM_PART_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn SPEC1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ASSY_PROCESS;
        private System.Windows.Forms.DataGridViewTextBoxColumn ASSY_TIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn ASSY_EMP;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEM_PART_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEM_GROUP;
        private System.Windows.Forms.DataGridViewTextBoxColumn PROCESS_ID;
    }
}