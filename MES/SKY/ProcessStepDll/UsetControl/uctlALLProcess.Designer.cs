namespace ProcessStepDll
{
    partial class uctlALLProcess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uctlALLProcess));
            this.TreeViewProcess = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.collapseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeViewProcess
            // 
            this.TreeViewProcess.AllowDrop = true;
            this.TreeViewProcess.BackColor = System.Drawing.Color.White;
            this.TreeViewProcess.ContextMenuStrip = this.contextMenuStrip1;
            this.TreeViewProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeViewProcess.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.TreeViewProcess.ImageIndex = 0;
            this.TreeViewProcess.ImageList = this.imageList1;
            this.TreeViewProcess.ItemHeight = 18;
            this.TreeViewProcess.Location = new System.Drawing.Point(0, 0);
            this.TreeViewProcess.Name = "TreeViewProcess";
            this.TreeViewProcess.SelectedImageIndex = 0;
            this.TreeViewProcess.Size = new System.Drawing.Size(404, 290);
            this.TreeViewProcess.TabIndex = 5;
            this.TreeViewProcess.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeViewProcess_ItemDrag);
            this.TreeViewProcess.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewProcess_AfterSelect);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.collapseToolStripMenuItem,
            this.expandToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(126, 48);
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
            // uctlALLProcess
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.TreeViewProcess);
            this.Name = "uctlALLProcess";
            this.Size = new System.Drawing.Size(404, 290);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView TreeViewProcess;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem collapseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
    }
}
