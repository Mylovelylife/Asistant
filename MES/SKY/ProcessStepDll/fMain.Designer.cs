namespace ProcessStepDll
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPageProcessList = new System.Windows.Forms.TabPage();
            this.gvData = new System.Windows.Forms.DataGridView();
            this.tabPageStepList = new System.Windows.Forms.TabPage();
            this.uctlStepItem1 = new ProcessStepDll.uctlStepItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LabFilter = new System.Windows.Forms.Label();
            this.combFilterField = new System.Windows.Forms.ComboBox();
            this.editFilter = new System.Windows.Forms.TextBox();
            this.combFilter = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.uctlDataGridView1 = new ProcessStepDll.uctlDataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lablFileName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.uctlProgressStatus1 = new ProcessStepDll.uctlProgressStatus();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAppend = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnMaintain = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPageProcessList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            this.tabPageStepList.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "RouteStage.bmp");
            this.imageList1.Images.SetKeyName(1, "RouteProcess.bmp");
            this.imageList1.Images.SetKeyName(2, "RouteRepair.bmp");
            this.imageList1.Images.SetKeyName(3, "RouteUnnecessary.bmp");
            this.imageList1.Images.SetKeyName(4, "RouteDisabled.bmp");
            this.imageList1.Images.SetKeyName(5, "RouteDisabledRepair.bmp");
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl2);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPageProcessList);
            this.tabControl2.Controls.Add(this.tabPageStepList);
            resources.ApplyResources(this.tabControl2, "tabControl2");
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            // 
            // tabPageProcessList
            // 
            this.tabPageProcessList.Controls.Add(this.gvData);
            resources.ApplyResources(this.tabPageProcessList, "tabPageProcessList");
            this.tabPageProcessList.Name = "tabPageProcessList";
            this.tabPageProcessList.UseVisualStyleBackColor = true;
            // 
            // gvData
            // 
            this.gvData.AllowUserToAddRows = false;
            this.gvData.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.gvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gvData.BackgroundColor = System.Drawing.Color.White;
            resources.ApplyResources(this.gvData, "gvData");
            this.gvData.MultiSelect = false;
            this.gvData.Name = "gvData";
            this.gvData.ReadOnly = true;
            this.gvData.RowTemplate.Height = 24;
            this.gvData.VirtualMode = true;
            this.gvData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvData_CellClick);
            this.gvData.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.gvData_CellValueNeeded);
            this.gvData.SelectionChanged += new System.EventHandler(this.gvData_SelectionChanged);
            // 
            // tabPageStepList
            // 
            this.tabPageStepList.Controls.Add(this.uctlStepItem1);
            resources.ApplyResources(this.tabPageStepList, "tabPageStepList");
            this.tabPageStepList.Name = "tabPageStepList";
            this.tabPageStepList.UseVisualStyleBackColor = true;
            // 
            // uctlStepItem1
            // 
            resources.ApplyResources(this.uctlStepItem1, "uctlStepItem1");
            this.uctlStepItem1.Name = "uctlStepItem1";
            this.uctlStepItem1.sFunction = null;
            this.uctlStepItem1.sProgram = null;
            this.uctlStepItem1.sUserID = null;
            this.uctlStepItem1.Load += new System.EventHandler(this.uctlStepItem1_Load);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.LabFilter);
            this.panel1.Controls.Add(this.combFilterField);
            this.panel1.Controls.Add(this.editFilter);
            this.panel1.Controls.Add(this.combFilter);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // LabFilter
            // 
            resources.ApplyResources(this.LabFilter, "LabFilter");
            this.LabFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.LabFilter.Name = "LabFilter";
            // 
            // combFilterField
            // 
            this.combFilterField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combFilterField.FormattingEnabled = true;
            resources.ApplyResources(this.combFilterField, "combFilterField");
            this.combFilterField.Name = "combFilterField";
            // 
            // editFilter
            // 
            resources.ApplyResources(this.editFilter, "editFilter");
            this.editFilter.Name = "editFilter";
            this.editFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editFilter_KeyPress);
            // 
            // combFilter
            // 
            this.combFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.combFilter, "combFilter");
            this.combFilter.FormattingEnabled = true;
            this.combFilter.Name = "combFilter";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.uctlDataGridView1);
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Controls.Add(this.uctlProgressStatus1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // uctlDataGridView1
            // 
            this.uctlDataGridView1.bBindingDataSource = false;
            resources.ApplyResources(this.uctlDataGridView1, "uctlDataGridView1");
            this.uctlDataGridView1.dtSourceTable = null;
            this.uctlDataGridView1.Name = "uctlDataGridView1";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.lablFileName);
            this.panel3.Controls.Add(this.label1);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // lablFileName
            // 
            resources.ApplyResources(this.lablFileName, "lablFileName");
            this.lablFileName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lablFileName.Name = "lablFileName";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // uctlProgressStatus1
            // 
            resources.ApplyResources(this.uctlProgressStatus1, "uctlProgressStatus1");
            this.uctlProgressStatus1.Name = "uctlProgressStatus1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnAppend);
            this.panel2.Controls.Add(this.btnImport);
            this.panel2.Controls.Add(this.btnCopy);
            this.panel2.Controls.Add(this.btnMaintain);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnAppend
            // 
            resources.ApplyResources(this.btnAppend, "btnAppend");
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // btnImport
            // 
            resources.ApplyResources(this.btnImport, "btnImport");
            this.btnImport.Name = "btnImport";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnCopy
            // 
            resources.ApplyResources(this.btnCopy, "btnCopy");
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Tag = "2";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnMaintain_Click);
            // 
            // btnMaintain
            // 
            resources.ApplyResources(this.btnMaintain, "btnMaintain");
            this.btnMaintain.Name = "btnMaintain";
            this.btnMaintain.Tag = "1";
            this.btnMaintain.UseVisualStyleBackColor = true;
            this.btnMaintain.Click += new System.EventHandler(this.btnMaintain_Click);
            // 
            // fMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.splitter1);
            this.Name = "fMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.fMain_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPageProcessList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            this.tabPageStepList.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ComboBox combFilter;
        private System.Windows.Forms.TextBox editFilter;
        private System.Windows.Forms.ComboBox combFilterField;
        private System.Windows.Forms.Label LabFilter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView gvData;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnMaintain;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.TabPage tabPage1;
        private uctlDataGridView uctlDataGridView1;
        private uctlProgressStatus uctlProgressStatus1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lablFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPageProcessList;
        private System.Windows.Forms.TabPage tabPageStepList;
        private uctlStepItem uctlStepItem1;
        private System.Windows.Forms.Button btnAppend;
    }
}