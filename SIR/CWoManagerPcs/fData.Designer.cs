namespace CWoManagerPcs
{
    partial class fData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fData));
            this.dtDueDate = new System.Windows.Forms.DateTimePicker();
            this.combLine = new System.Windows.Forms.ComboBox();
            this.editRemark = new System.Windows.Forms.TextBox();
            this.LabRemark = new System.Windows.Forms.Label();
            this.LabLine = new System.Windows.Forms.Label();
            this.LabDueDate = new System.Windows.Forms.Label();
            this.LabScheduleDate = new System.Windows.Forms.Label();
            this.LabFactory = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBOM = new System.Windows.Forms.Button();
            this.combVersion = new System.Windows.Forms.ComboBox();
            this.btnSearchPart = new System.Windows.Forms.Button();
            this.combWoType = new System.Windows.Forms.ComboBox();
            this.LabWoStatus = new System.Windows.Forms.Label();
            this.combOutProcess = new System.Windows.Forms.ComboBox();
            this.combInProcess = new System.Windows.Forms.ComboBox();
            this.combRoute = new System.Windows.Forms.ComboBox();
            this.combWoRule = new System.Windows.Forms.ComboBox();
            this.LabOutProcess = new System.Windows.Forms.Label();
            this.LabInProcess = new System.Windows.Forms.Label();
            this.LabRoute = new System.Windows.Forms.Label();
            this.LabWoType = new System.Windows.Forms.Label();
            this.LabWoRule = new System.Windows.Forms.Label();
            this.LabVersion = new System.Windows.Forms.Label();
            this.editPart = new System.Windows.Forms.TextBox();
            this.editWO = new System.Windows.Forms.TextBox();
            this.LabStatus = new System.Windows.Forms.Label();
            this.LabWO = new System.Windows.Forms.Label();
            this.LabPart = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuAppend = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSearchRoute = new System.Windows.Forms.Button();
            this.btnSearchRule = new System.Windows.Forms.Button();
            this.editTargetQty = new System.Windows.Forms.TextBox();
            this.dtScheduleDate = new System.Windows.Forms.DateTimePicker();
            this.LabTargetQty = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBindingSEQ = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.columnPKSpec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpPackingSpec = new System.Windows.Forms.TabPage();
            this.LVPkSPec = new System.Windows.Forms.ListView();
            this.PKSPEC_NAME = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BOX_QTY = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CARTON_QTY = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PALLET_QTY = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpProperty = new System.Windows.Forms.TabPage();
            this.dgvProperty = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpPackingSpec.SuspendLayout();
            this.tpProperty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProperty)).BeginInit();
            this.SuspendLayout();
            // 
            // dtDueDate
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.dtDueDate, 2);
            resources.ApplyResources(this.dtDueDate, "dtDueDate");
            this.dtDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDueDate.Name = "dtDueDate";
            // 
            // combLine
            // 
            resources.ApplyResources(this.combLine, "combLine");
            this.combLine.Name = "combLine";
            // 
            // editRemark
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.editRemark, 2);
            resources.ApplyResources(this.editRemark, "editRemark");
            this.editRemark.Name = "editRemark";
            // 
            // LabRemark
            // 
            resources.ApplyResources(this.LabRemark, "LabRemark");
            this.LabRemark.BackColor = System.Drawing.Color.Transparent;
            this.LabRemark.Name = "LabRemark";
            // 
            // LabLine
            // 
            resources.ApplyResources(this.LabLine, "LabLine");
            this.LabLine.BackColor = System.Drawing.Color.Transparent;
            this.LabLine.Name = "LabLine";
            // 
            // LabDueDate
            // 
            resources.ApplyResources(this.LabDueDate, "LabDueDate");
            this.LabDueDate.BackColor = System.Drawing.Color.Transparent;
            this.LabDueDate.Name = "LabDueDate";
            // 
            // LabScheduleDate
            // 
            resources.ApplyResources(this.LabScheduleDate, "LabScheduleDate");
            this.LabScheduleDate.BackColor = System.Drawing.Color.Transparent;
            this.LabScheduleDate.Name = "LabScheduleDate";
            // 
            // LabFactory
            // 
            resources.ApplyResources(this.LabFactory, "LabFactory");
            this.LabFactory.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.LabFactory, 2);
            this.LabFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.LabFactory.Name = "LabFactory";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // btnBOM
            // 
            resources.ApplyResources(this.btnBOM, "btnBOM");
            this.btnBOM.ForeColor = System.Drawing.Color.Maroon;
            this.btnBOM.Name = "btnBOM";
            this.btnBOM.UseVisualStyleBackColor = true;
            this.btnBOM.Click += new System.EventHandler(this.btnBOM_Click);
            // 
            // combVersion
            // 
            resources.ApplyResources(this.combVersion, "combVersion");
            this.combVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combVersion.FormattingEnabled = true;
            this.combVersion.Name = "combVersion";
            // 
            // btnSearchPart
            // 
            resources.ApplyResources(this.btnSearchPart, "btnSearchPart");
            this.btnSearchPart.BackColor = System.Drawing.SystemColors.Control;
            this.btnSearchPart.Name = "btnSearchPart";
            this.btnSearchPart.UseVisualStyleBackColor = false;
            this.btnSearchPart.Click += new System.EventHandler(this.btnSearchPart_Click);
            // 
            // combWoType
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.combWoType, 2);
            resources.ApplyResources(this.combWoType, "combWoType");
            this.combWoType.FormattingEnabled = true;
            this.combWoType.Name = "combWoType";
            // 
            // LabWoStatus
            // 
            resources.ApplyResources(this.LabWoStatus, "LabWoStatus");
            this.LabWoStatus.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.LabWoStatus, 2);
            this.LabWoStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.LabWoStatus.Name = "LabWoStatus";
            // 
            // combOutProcess
            // 
            this.combOutProcess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tableLayoutPanel1.SetColumnSpan(this.combOutProcess, 2);
            resources.ApplyResources(this.combOutProcess, "combOutProcess");
            this.combOutProcess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combOutProcess.FormattingEnabled = true;
            this.combOutProcess.Name = "combOutProcess";
            // 
            // combInProcess
            // 
            this.combInProcess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tableLayoutPanel1.SetColumnSpan(this.combInProcess, 2);
            resources.ApplyResources(this.combInProcess, "combInProcess");
            this.combInProcess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combInProcess.FormattingEnabled = true;
            this.combInProcess.Name = "combInProcess";
            // 
            // combRoute
            // 
            this.combRoute.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            resources.ApplyResources(this.combRoute, "combRoute");
            this.combRoute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combRoute.FormattingEnabled = true;
            this.combRoute.Name = "combRoute";
            this.combRoute.SelectedIndexChanged += new System.EventHandler(this.combRoute_SelectedIndexChanged);
            // 
            // combWoRule
            // 
            this.combWoRule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            resources.ApplyResources(this.combWoRule, "combWoRule");
            this.combWoRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combWoRule.FormattingEnabled = true;
            this.combWoRule.Name = "combWoRule";
            this.combWoRule.Sorted = true;
            this.combWoRule.SelectedIndexChanged += new System.EventHandler(this.combWoRule_SelectedIndexChanged);
            // 
            // LabOutProcess
            // 
            resources.ApplyResources(this.LabOutProcess, "LabOutProcess");
            this.LabOutProcess.BackColor = System.Drawing.Color.Transparent;
            this.LabOutProcess.Name = "LabOutProcess";
            // 
            // LabInProcess
            // 
            resources.ApplyResources(this.LabInProcess, "LabInProcess");
            this.LabInProcess.BackColor = System.Drawing.Color.Transparent;
            this.LabInProcess.Name = "LabInProcess";
            // 
            // LabRoute
            // 
            resources.ApplyResources(this.LabRoute, "LabRoute");
            this.LabRoute.BackColor = System.Drawing.Color.Transparent;
            this.LabRoute.Name = "LabRoute";
            // 
            // LabWoType
            // 
            resources.ApplyResources(this.LabWoType, "LabWoType");
            this.LabWoType.BackColor = System.Drawing.Color.Transparent;
            this.LabWoType.Name = "LabWoType";
            // 
            // LabWoRule
            // 
            resources.ApplyResources(this.LabWoRule, "LabWoRule");
            this.LabWoRule.BackColor = System.Drawing.Color.Transparent;
            this.LabWoRule.Name = "LabWoRule";
            // 
            // LabVersion
            // 
            resources.ApplyResources(this.LabVersion, "LabVersion");
            this.LabVersion.BackColor = System.Drawing.Color.Transparent;
            this.LabVersion.Name = "LabVersion";
            // 
            // editPart
            // 
            this.editPart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            resources.ApplyResources(this.editPart, "editPart");
            this.editPart.Name = "editPart";
            this.editPart.EnabledChanged += new System.EventHandler(this.editPart_EnabledChanged);
            this.editPart.TextChanged += new System.EventHandler(this.editPart_TextChanged);
            this.editPart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editPart_KeyPress);
            // 
            // editWO
            // 
            this.editWO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tableLayoutPanel1.SetColumnSpan(this.editWO, 2);
            resources.ApplyResources(this.editWO, "editWO");
            this.editWO.Name = "editWO";
            // 
            // LabStatus
            // 
            resources.ApplyResources(this.LabStatus, "LabStatus");
            this.LabStatus.BackColor = System.Drawing.Color.Transparent;
            this.LabStatus.Name = "LabStatus";
            // 
            // LabWO
            // 
            resources.ApplyResources(this.LabWO, "LabWO");
            this.LabWO.BackColor = System.Drawing.Color.Transparent;
            this.LabWO.Name = "LabWO";
            // 
            // LabPart
            // 
            resources.ApplyResources(this.LabPart, "LabPart");
            this.LabPart.BackColor = System.Drawing.Color.Transparent;
            this.LabPart.Name = "LabPart";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuAppend,
            this.MenuRemove});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // MenuAppend
            // 
            this.MenuAppend.Name = "MenuAppend";
            resources.ApplyResources(this.MenuAppend, "MenuAppend");
            this.MenuAppend.Click += new System.EventHandler(this.MenuAppend_Click);
            // 
            // MenuRemove
            // 
            this.MenuRemove.Name = "MenuRemove";
            resources.ApplyResources(this.MenuRemove, "MenuRemove");
            this.MenuRemove.Click += new System.EventHandler(this.MenuRemove_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Name = "panel2";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabStop = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.TabStop = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.TextDirection = System.Windows.Forms.ToolStripTextDirection.Vertical90;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.MenuAppend_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton2, "toolStripButton2");
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.MenuRemove_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.btnSearchRoute, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSearchRule, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.editTargetQty, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnBOM, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.editRemark, 4, 7);
            this.tableLayoutPanel1.Controls.Add(this.LabRemark, 3, 7);
            this.tableLayoutPanel1.Controls.Add(this.dtScheduleDate, 4, 5);
            this.tableLayoutPanel1.Controls.Add(this.dtDueDate, 4, 6);
            this.tableLayoutPanel1.Controls.Add(this.LabFactory, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSearchPart, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.LabWO, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.editWO, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.LabOutProcess, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.combOutProcess, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.combInProcess, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.combRoute, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.LabInProcess, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.LabRoute, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.LabStatus, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.LabLine, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.LabWoStatus, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.LabPart, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.editPart, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.LabVersion, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.combVersion, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.LabDueDate, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.LabWoRule, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.LabScheduleDate, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.combWoRule, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.LabWoType, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.combWoType, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.LabTargetQty, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 4, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // btnSearchRoute
            // 
            resources.ApplyResources(this.btnSearchRoute, "btnSearchRoute");
            this.btnSearchRoute.BackColor = System.Drawing.SystemColors.Control;
            this.btnSearchRoute.Name = "btnSearchRoute";
            this.btnSearchRoute.UseVisualStyleBackColor = false;
            this.btnSearchRoute.Click += new System.EventHandler(this.btnSearchRoute_Click);
            // 
            // btnSearchRule
            // 
            resources.ApplyResources(this.btnSearchRule, "btnSearchRule");
            this.btnSearchRule.BackColor = System.Drawing.SystemColors.Control;
            this.btnSearchRule.Name = "btnSearchRule";
            this.btnSearchRule.UseVisualStyleBackColor = false;
            this.btnSearchRule.Click += new System.EventHandler(this.btnSearchRule_Click);
            // 
            // editTargetQty
            // 
            this.editTargetQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            resources.ApplyResources(this.editTargetQty, "editTargetQty");
            this.editTargetQty.Name = "editTargetQty";
            this.editTargetQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editTargetQty_KeyPress);
            // 
            // dtScheduleDate
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.dtScheduleDate, 2);
            resources.ApplyResources(this.dtScheduleDate, "dtScheduleDate");
            this.dtScheduleDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtScheduleDate.Name = "dtScheduleDate";
            this.dtScheduleDate.Value = new System.DateTime(2009, 4, 7, 0, 0, 0, 0);
            // 
            // LabTargetQty
            // 
            resources.ApplyResources(this.LabTargetQty, "LabTargetQty");
            this.LabTargetQty.Name = "LabTargetQty";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.btnBindingSEQ, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.combLine, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // btnBindingSEQ
            // 
            resources.ApplyResources(this.btnBindingSEQ, "btnBindingSEQ");
            this.btnBindingSEQ.ForeColor = System.Drawing.Color.Maroon;
            this.btnBindingSEQ.Name = "btnBindingSEQ";
            this.btnBindingSEQ.UseVisualStyleBackColor = true;
            this.btnBindingSEQ.Click += new System.EventHandler(this.btnBindingSEQ_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // columnPKSpec
            // 
            resources.ApplyResources(this.columnPKSpec, "columnPKSpec");
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpPackingSpec);
            this.tabControl1.Controls.Add(this.tpProperty);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tpPackingSpec
            // 
            this.tpPackingSpec.Controls.Add(this.toolStrip1);
            this.tpPackingSpec.Controls.Add(this.LVPkSPec);
            resources.ApplyResources(this.tpPackingSpec, "tpPackingSpec");
            this.tpPackingSpec.Name = "tpPackingSpec";
            this.tpPackingSpec.UseVisualStyleBackColor = true;
            // 
            // LVPkSPec
            // 
            resources.ApplyResources(this.LVPkSPec, "LVPkSPec");
            this.LVPkSPec.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PKSPEC_NAME,
            this.BOX_QTY,
            this.CARTON_QTY,
            this.PALLET_QTY});
            this.LVPkSPec.ContextMenuStrip = this.contextMenuStrip1;
            this.LVPkSPec.FullRowSelect = true;
            this.LVPkSPec.GridLines = true;
            this.LVPkSPec.HideSelection = false;
            this.LVPkSPec.Name = "LVPkSPec";
            this.LVPkSPec.TabStop = false;
            this.LVPkSPec.UseCompatibleStateImageBehavior = false;
            this.LVPkSPec.View = System.Windows.Forms.View.Details;
            // 
            // PKSPEC_NAME
            // 
            resources.ApplyResources(this.PKSPEC_NAME, "PKSPEC_NAME");
            // 
            // BOX_QTY
            // 
            resources.ApplyResources(this.BOX_QTY, "BOX_QTY");
            // 
            // CARTON_QTY
            // 
            resources.ApplyResources(this.CARTON_QTY, "CARTON_QTY");
            // 
            // PALLET_QTY
            // 
            resources.ApplyResources(this.PALLET_QTY, "PALLET_QTY");
            // 
            // tpProperty
            // 
            this.tpProperty.Controls.Add(this.dgvProperty);
            resources.ApplyResources(this.tpProperty, "tpProperty");
            this.tpProperty.Name = "tpProperty";
            this.tpProperty.UseVisualStyleBackColor = true;
            // 
            // dgvProperty
            // 
            this.dgvProperty.AllowUserToAddRows = false;
            this.dgvProperty.AllowUserToDeleteRows = false;
            this.dgvProperty.AllowUserToResizeRows = false;
            this.dgvProperty.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvProperty.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvProperty.BackgroundColor = System.Drawing.Color.White;
            this.dgvProperty.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvProperty, "dgvProperty");
            this.dgvProperty.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvProperty.Name = "dgvProperty";
            this.dgvProperty.RowHeadersVisible = false;
            this.dgvProperty.RowTemplate.Height = 24;
            this.dgvProperty.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProperty.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvProperty_CellFormatting);
            this.dgvProperty.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProperty_CellValueChanged);
            this.dgvProperty.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvProperty_DataBindingComplete);
            // 
            // fData
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fData";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.fData_Load);
            this.Shown += new System.EventHandler(this.fData_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpPackingSpec.ResumeLayout(false);
            this.tpPackingSpec.PerformLayout();
            this.tpProperty.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProperty)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox editWO;
        private System.Windows.Forms.Label LabStatus;
        private System.Windows.Forms.Label LabWO;
        private System.Windows.Forms.TextBox editPart;
        private System.Windows.Forms.Label LabPart;
        private System.Windows.Forms.Label LabWoType;
        private System.Windows.Forms.Label LabWoRule;
        private System.Windows.Forms.Label LabVersion;
        private System.Windows.Forms.Label LabScheduleDate;
        private System.Windows.Forms.TextBox editRemark;
        private System.Windows.Forms.Label LabRemark;
        private System.Windows.Forms.Label LabOutProcess;
        private System.Windows.Forms.Label LabInProcess;
        private System.Windows.Forms.Label LabRoute;
        private System.Windows.Forms.Label LabLine;
        private System.Windows.Forms.Label LabDueDate;
        private System.Windows.Forms.ComboBox combWoRule;
        private System.Windows.Forms.ComboBox combOutProcess;
        private System.Windows.Forms.ComboBox combInProcess;
        private System.Windows.Forms.ComboBox combRoute;
        private System.Windows.Forms.ComboBox combLine;
        private System.Windows.Forms.Label LabWoStatus;
        private System.Windows.Forms.DateTimePicker dtDueDate;
        private System.Windows.Forms.ComboBox combWoType;
        private System.Windows.Forms.Button btnSearchPart;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuAppend;
        private System.Windows.Forms.ToolStripMenuItem MenuRemove;
        private System.Windows.Forms.Label LabFactory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ColumnHeader columnPKSpec;
        private System.Windows.Forms.Label LabTargetQty;
        private System.Windows.Forms.TextBox editTargetQty;
        public System.Windows.Forms.Button btnBOM;
        public System.Windows.Forms.ComboBox combVersion;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DateTimePicker dtScheduleDate;
        private System.Windows.Forms.Button btnSearchRule;
        private System.Windows.Forms.Button btnSearchRoute;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpPackingSpec;
        private System.Windows.Forms.TabPage tpProperty;
        public System.Windows.Forms.DataGridView dgvProperty;
        private System.Windows.Forms.ListView LVPkSPec;
        private System.Windows.Forms.ColumnHeader PKSPEC_NAME;
        private System.Windows.Forms.ColumnHeader BOX_QTY;
        private System.Windows.Forms.ColumnHeader CARTON_QTY;
        private System.Windows.Forms.ColumnHeader PALLET_QTY;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.Windows.Forms.Button btnBindingSEQ;
    }
}