namespace ProcessStepDll
{
    partial class uctlALLTooling
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uctlALLTooling));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.collapseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeViewTooling = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.modifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.collapseToolStripMenuItem,
            this.expandToolStripMenuItem,
            this.toolStripMenuItem1,
            this.modifyToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(126, 92);
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
            // TreeViewTooling
            // 
            this.TreeViewTooling.AllowDrop = true;
            this.TreeViewTooling.BackColor = System.Drawing.Color.White;
            this.TreeViewTooling.ContextMenuStrip = this.contextMenuStrip1;
            this.TreeViewTooling.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeViewTooling.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.TreeViewTooling.ImageIndex = 0;
            this.TreeViewTooling.ImageList = this.imageList1;
            this.TreeViewTooling.ItemHeight = 20;
            this.TreeViewTooling.Location = new System.Drawing.Point(0, 0);
            this.TreeViewTooling.Name = "TreeViewTooling";
            this.TreeViewTooling.SelectedImageIndex = 0;
            this.TreeViewTooling.Size = new System.Drawing.Size(319, 320);
            this.TreeViewTooling.TabIndex = 6;
            this.TreeViewTooling.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeViewProcess_ItemDrag);
            this.TreeViewTooling.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewTooling_AfterSelect);
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
            // modifyToolStripMenuItem
            // 
            this.modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
            this.modifyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.modifyToolStripMenuItem.Text = "Modify";
            this.modifyToolStripMenuItem.Click += new System.EventHandler(this.modifyToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "Append";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // uctlALLTooling
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.TreeViewTooling);
            this.Name = "uctlALLTooling";
            this.Size = new System.Drawing.Size(319, 320);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem collapseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandToolStripMenuItem;
        private System.Windows.Forms.TreeView TreeViewTooling;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem modifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}
