namespace ProcessStepDll
{
    partial class uctlProcessTooling
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uctlProcessTooling));
            this.TreeToolingData = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.collapseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAppend = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeToolingData
            // 
            this.TreeToolingData.AllowDrop = true;
            this.TreeToolingData.BackColor = System.Drawing.Color.White;
            this.TreeToolingData.ContextMenuStrip = this.contextMenuStrip1;
            this.TreeToolingData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeToolingData.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.TreeToolingData.HideSelection = false;
            this.TreeToolingData.ImageIndex = 0;
            this.TreeToolingData.ImageList = this.imageList1;
            this.TreeToolingData.ItemHeight = 20;
            this.TreeToolingData.Location = new System.Drawing.Point(0, 0);
            this.TreeToolingData.Margin = new System.Windows.Forms.Padding(0);
            this.TreeToolingData.Name = "TreeToolingData";
            this.TreeToolingData.SelectedImageIndex = 0;
            this.TreeToolingData.Size = new System.Drawing.Size(297, 360);
            this.TreeToolingData.TabIndex = 2;
            this.TreeToolingData.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeToolingData_ItemDrag);
            this.TreeToolingData.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeToolingData_AfterSelect);
            this.TreeToolingData.DragDrop += new System.Windows.Forms.DragEventHandler(this.TreeToolingData_DragDrop);
            this.TreeToolingData.DragEnter += new System.Windows.Forms.DragEventHandler(this.TreeToolingData_DragEnter);
            this.TreeToolingData.DragOver += new System.Windows.Forms.DragEventHandler(this.TreeToolingData_DragOver);
            this.TreeToolingData.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.uctlProcessTooling_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.collapseToolStripMenuItem,
            this.expandToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(126, 70);
            // 
            // collapseToolStripMenuItem
            // 
            this.collapseToolStripMenuItem.Name = "collapseToolStripMenuItem";
            this.collapseToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.collapseToolStripMenuItem.Text = "Collapse";
            this.collapseToolStripMenuItem.Click += new System.EventHandler(this.collapseToolStripMenuItem_Click);
            // 
            // expandToolStripMenuItem
            // 
            this.expandToolStripMenuItem.Name = "expandToolStripMenuItem";
            this.expandToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.expandToolStripMenuItem.Text = "Expand";
            this.expandToolStripMenuItem.Click += new System.EventHandler(this.expandToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Line.bmp");
            this.imageList1.Images.SetKeyName(1, "Stage.bmp");
            this.imageList1.Images.SetKeyName(2, "Process.bmp");
            this.imageList1.Images.SetKeyName(3, "Terminal.bmp");
            this.imageList1.Images.SetKeyName(4, "TerminalDCT.bmp");
            this.imageList1.Images.SetKeyName(5, "TerminalDisabled.bmp");
            this.imageList1.Images.SetKeyName(6, "TerminalDCTDisable.bmp");
            this.imageList1.Images.SetKeyName(7, "TerminalATE.png.png");
            this.imageList1.Images.SetKeyName(8, "TerminalSMT.png");
            this.imageList1.Images.SetKeyName(9, "TerminalATEDis.png");
            this.imageList1.Images.SetKeyName(10, "TerminalSMTDis.png");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAppend);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 360);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(297, 35);
            this.panel1.TabIndex = 9;
            this.panel1.Visible = false;
            // 
            // btnAppend
            // 
            this.btnAppend.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAppend.Location = new System.Drawing.Point(0, 0);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(96, 35);
            this.btnAppend.TabIndex = 7;
            this.btnAppend.Text = "Append Step";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // uctlProcessTooling
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.TreeToolingData);
            this.Controls.Add(this.panel1);
            this.Name = "uctlProcessTooling";
            this.Size = new System.Drawing.Size(297, 395);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView TreeToolingData;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem collapseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAppend;
    }
}
