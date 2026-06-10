namespace CWoManagerPcs
{
    partial class fMain
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gvData = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuMACRequest = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuMACRequestYes = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuMACRequestNo = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuViewSN = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuViewBOM = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuViewWoData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.combFactory = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.combWoStatus = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.combFilter = new System.Windows.Forms.ToolStripComboBox();
            this.editFilter = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gbPack = new System.Windows.Forms.GroupBox();
            this.lstPack = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.tsCount = new System.Windows.Forms.ToolStripLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.btnAppend = new System.Windows.Forms.ToolStripButton();
            this.btnModify = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRelease = new System.Windows.Forms.ToolStripButton();
            this.tsReleaseSep = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.tsView = new System.Windows.Forms.ToolStripSeparator();
            this.btnSN = new System.Windows.Forms.ToolStripButton();
            this.btnBom = new System.Windows.Forms.ToolStripButton();
            this.btnPanel = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.btnAssignMAC = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.btnSpec = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbPack.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvData
            // 
            this.gvData.AllowUserToAddRows = false;
            this.gvData.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            this.gvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gvData.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.gvData, "gvData");
            this.gvData.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvData.DefaultCellStyle = dataGridViewCellStyle3;
            this.gvData.MultiSelect = false;
            this.gvData.Name = "gvData";
            this.gvData.ReadOnly = true;
            this.gvData.RowTemplate.Height = 24;
            this.gvData.VirtualMode = true;
            this.gvData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvData_CellClick);
            this.gvData.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.gvData_CellValueNeeded);
            this.gvData.SelectionChanged += new System.EventHandler(this.gvData_SelectionChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuMACRequest,
            this.MenuViewSN,
            this.MenuViewBOM,
            this.viewToolStripMenuItem,
            this.MenuViewWoData,
            this.toolStripSeparator1,
            this.MenuHistory,
            this.MenuStatus});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // MenuMACRequest
            // 
            this.MenuMACRequest.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuMACRequestYes,
            this.MenuMACRequestNo});
            this.MenuMACRequest.Name = "MenuMACRequest";
            resources.ApplyResources(this.MenuMACRequest, "MenuMACRequest");
            this.MenuMACRequest.DropDownOpened += new System.EventHandler(this.MenuMACRequest_DropDownOpened);
            // 
            // MenuMACRequestYes
            // 
            this.MenuMACRequestYes.Name = "MenuMACRequestYes";
            resources.ApplyResources(this.MenuMACRequestYes, "MenuMACRequestYes");
            this.MenuMACRequestYes.Tag = "Y";
            this.MenuMACRequestYes.Click += new System.EventHandler(this.MenuMACRequestYes_Click);
            // 
            // MenuMACRequestNo
            // 
            this.MenuMACRequestNo.Name = "MenuMACRequestNo";
            resources.ApplyResources(this.MenuMACRequestNo, "MenuMACRequestNo");
            this.MenuMACRequestNo.Tag = "N";
            this.MenuMACRequestNo.Click += new System.EventHandler(this.MenuMACRequestYes_Click);
            // 
            // MenuViewSN
            // 
            this.MenuViewSN.Name = "MenuViewSN";
            resources.ApplyResources(this.MenuViewSN, "MenuViewSN");
            this.MenuViewSN.Click += new System.EventHandler(this.MenuViewSN_Click);
            // 
            // MenuViewBOM
            // 
            this.MenuViewBOM.Name = "MenuViewBOM";
            resources.ApplyResources(this.MenuViewBOM, "MenuViewBOM");
            this.MenuViewBOM.Click += new System.EventHandler(this.MenuViewBOM_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
            this.viewToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMenuItem_Click);
            // 
            // MenuViewWoData
            // 
            this.MenuViewWoData.Name = "MenuViewWoData";
            resources.ApplyResources(this.MenuViewWoData, "MenuViewWoData");
            this.MenuViewWoData.Click += new System.EventHandler(this.MenuViewWoData_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // MenuHistory
            // 
            this.MenuHistory.Name = "MenuHistory";
            resources.ApplyResources(this.MenuHistory, "MenuHistory");
            this.MenuHistory.Click += new System.EventHandler(this.MenuHistory_Click);
            // 
            // MenuStatus
            // 
            this.MenuStatus.Name = "MenuStatus";
            resources.ApplyResources(this.MenuStatus, "MenuStatus");
            this.MenuStatus.Click += new System.EventHandler(this.MenuStatus_Click);
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.combFactory,
            this.toolStripLabel2,
            this.combWoStatus,
            this.toolStripLabel3,
            this.combFilter,
            this.editFilter,
            this.toolStripLabel5});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            // 
            // combFactory
            // 
            this.combFactory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combFactory, "combFactory");
            this.combFactory.Name = "combFactory";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            resources.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
            // 
            // combWoStatus
            // 
            this.combWoStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combWoStatus, "combWoStatus");
            this.combWoStatus.Items.AddRange(new object[] {
            resources.GetString("combWoStatus.Items"),
            resources.GetString("combWoStatus.Items1"),
            resources.GetString("combWoStatus.Items2"),
            resources.GetString("combWoStatus.Items3"),
            resources.GetString("combWoStatus.Items4"),
            resources.GetString("combWoStatus.Items5"),
            resources.GetString("combWoStatus.Items6"),
            resources.GetString("combWoStatus.Items7"),
            resources.GetString("combWoStatus.Items8")});
            this.combWoStatus.Name = "combWoStatus";
            this.combWoStatus.SelectedIndexChanged += new System.EventHandler(this.combWoStatus_SelectedIndexChanged);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            resources.ApplyResources(this.toolStripLabel3, "toolStripLabel3");
            // 
            // combFilter
            // 
            this.combFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combFilter, "combFilter");
            this.combFilter.Name = "combFilter";
            // 
            // editFilter
            // 
            resources.ApplyResources(this.editFilter, "editFilter");
            this.editFilter.Name = "editFilter";
            this.editFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editFilter_KeyPress);
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            resources.ApplyResources(this.toolStripLabel5, "toolStripLabel5");
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // gbPack
            // 
            this.gbPack.Controls.Add(this.lstPack);
            resources.ApplyResources(this.gbPack, "gbPack");
            this.gbPack.Name = "gbPack";
            this.gbPack.TabStop = false;
            // 
            // lstPack
            // 
            this.lstPack.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            resources.ApplyResources(this.lstPack, "lstPack");
            this.lstPack.FullRowSelect = true;
            this.lstPack.HideSelection = false;
            this.lstPack.Name = "lstPack";
            this.lstPack.UseCompatibleStateImageBehavior = false;
            this.lstPack.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // columnHeader6
            // 
            resources.ApplyResources(this.columnHeader6, "columnHeader6");
            // 
            // toolStrip2
            // 
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel4,
            this.tsCount});
            this.toolStrip2.Name = "toolStrip2";
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            resources.ApplyResources(this.toolStripLabel4, "toolStripLabel4");
            // 
            // tsCount
            // 
            resources.ApplyResources(this.tsCount, "tsCount");
            this.tsCount.ForeColor = System.Drawing.Color.Maroon;
            this.tsCount.Name = "tsCount";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gvData);
            this.panel2.Controls.Add(this.toolStrip2);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // btnAppend
            // 
            resources.ApplyResources(this.btnAppend, "btnAppend");
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // btnModify
            // 
            resources.ApplyResources(this.btnModify, "btnModify");
            this.btnModify.Name = "btnModify";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            resources.ApplyResources(this.bindingNavigatorSeparator2, "bindingNavigatorSeparator2");
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Name = "btnExport";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // btnRelease
            // 
            resources.ApplyResources(this.btnRelease, "btnRelease");
            this.btnRelease.Name = "btnRelease";
            this.btnRelease.Click += new System.EventHandler(this.btnRelease_Click);
            // 
            // tsReleaseSep
            // 
            this.tsReleaseSep.Name = "tsReleaseSep";
            resources.ApplyResources(this.tsReleaseSep, "tsReleaseSep");
            // 
            // toolStripButton2
            // 
            resources.ApplyResources(this.toolStripButton2, "toolStripButton2");
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.MenuHistory_Click);
            // 
            // toolStripButton3
            // 
            resources.ApplyResources(this.toolStripButton3, "toolStripButton3");
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.MenuStatus_Click);
            // 
            // tsView
            // 
            this.tsView.Name = "tsView";
            resources.ApplyResources(this.tsView, "tsView");
            // 
            // btnSN
            // 
            resources.ApplyResources(this.btnSN, "btnSN");
            this.btnSN.Name = "btnSN";
            this.btnSN.Click += new System.EventHandler(this.MenuViewSN_Click);
            // 
            // btnBom
            // 
            resources.ApplyResources(this.btnBom, "btnBom");
            this.btnBom.Name = "btnBom";
            this.btnBom.Click += new System.EventHandler(this.MenuViewBOM_Click);
            // 
            // btnPanel
            // 
            resources.ApplyResources(this.btnPanel, "btnPanel");
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.Click += new System.EventHandler(this.viewToolStripMenuItem_Click);
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            resources.ApplyResources(this.bindingNavigatorSeparator, "bindingNavigatorSeparator");
            // 
            // btnAssignMAC
            // 
            resources.ApplyResources(this.btnAssignMAC, "btnAssignMAC");
            this.btnAssignMAC.Name = "btnAssignMAC";
            this.btnAssignMAC.Click += new System.EventHandler(this.btnAssignMAC_Click);
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            resources.ApplyResources(this.bindingNavigator1, "bindingNavigator1");
            this.bindingNavigator1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAppend,
            this.btnModify,
            this.bindingNavigatorSeparator2,
            this.btnExport,
            this.toolStripSeparator3,
            this.btnRelease,
            this.tsReleaseSep,
            this.toolStripButton2,
            this.toolStripButton3,
            this.tsView,
            this.btnSN,
            this.btnBom,
            this.btnPanel,
            this.bindingNavigatorSeparator,
            this.btnAssignMAC});
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.ShowItemToolTips = false;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSpec
            // 
            this.btnSpec.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnSpec.Location = new System.Drawing.Point(750, 10);
            this.btnSpec.Name = "btnSpec";
            this.btnSpec.Size = new System.Drawing.Size(100, 25);
            this.btnSpec.TabIndex = 100;
            this.btnSpec.Text = "SPEC RULE";
            this.btnSpec.UseVisualStyleBackColor = true;
            this.btnSpec.Click += new System.EventHandler(this.btnSpec_Click);
            // 
            // fMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSpec);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.gbPack);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.bindingNavigator1);
            this.Name = "fMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.fMain_Load);
            this.Shown += new System.EventHandler(this.fMain_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.gbPack.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gvData;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuViewSN;
        private System.Windows.Forms.ToolStripMenuItem MenuViewBOM;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MenuHistory;
        private System.Windows.Forms.ToolStripMenuItem MenuStatus;
        private System.Windows.Forms.ToolStripMenuItem MenuViewWoData;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuMACRequest;
        private System.Windows.Forms.ToolStripMenuItem MenuMACRequestYes;
        private System.Windows.Forms.ToolStripMenuItem MenuMACRequestNo;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox combFactory;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox combWoStatus;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox combFilter;
        private System.Windows.Forms.ToolStripTextBox editFilter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox gbPack;
        private System.Windows.Forms.ListView lstPack;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripLabel tsCount;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripButton btnAppend;
        private System.Windows.Forms.ToolStripButton btnModify;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton btnExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnRelease;
        private System.Windows.Forms.ToolStripSeparator tsReleaseSep;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator tsView;
        private System.Windows.Forms.ToolStripButton btnSN;
        private System.Windows.Forms.ToolStripButton btnBom;
        private System.Windows.Forms.ToolStripButton btnPanel;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripButton btnAssignMAC;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSpec;
    }
}