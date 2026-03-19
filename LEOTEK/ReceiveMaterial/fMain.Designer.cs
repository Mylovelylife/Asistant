
namespace ReceiveMaterial
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
		/// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
		/// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
		/// 這個方法的內容。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.ck_Display = new System.Windows.Forms.CheckBox();
			this.txt_Barcode = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.pn_Message = new System.Windows.Forms.Panel();
			this.lb_Message = new System.Windows.Forms.Label();
			this.gv_Header = new System.Windows.Forms.DataGridView();
			this.bt_export = new System.Windows.Forms.Button();
			this.cb_FileChoice = new System.Windows.Forms.ComboBox();
			this.lb_FileChoice = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pic_import = new System.Windows.Forms.PictureBox();
			this.lb_FileImport = new System.Windows.Forms.Label();
			this.MS = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.Print = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.pn_Message.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gv_Header)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pic_import)).BeginInit();
			this.MS.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.tableLayoutPanel1.Controls.Add(this.ck_Display, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.txt_Barcode, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.label2, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.pn_Message, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.gv_Header, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.bt_export, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.cb_FileChoice, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.lb_FileChoice, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.pic_import, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lb_FileImport, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(748, 584);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// ck_Display
			// 
			this.ck_Display.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ck_Display.AutoSize = true;
			this.ck_Display.Enabled = false;
			this.ck_Display.Location = new System.Drawing.Point(604, 46);
			this.ck_Display.Margin = new System.Windows.Forms.Padding(2);
			this.ck_Display.Name = "ck_Display";
			this.ck_Display.Size = new System.Drawing.Size(59, 19);
			this.ck_Display.TabIndex = 1;
			this.ck_Display.Text = "Clear";
			this.ck_Display.UseVisualStyleBackColor = true;
			this.ck_Display.CheckedChanged += new System.EventHandler(this.ck_Display_CheckedChanged);
			// 
			// txt_Barcode
			// 
			this.txt_Barcode.Location = new System.Drawing.Point(154, 41);
			this.txt_Barcode.Margin = new System.Windows.Forms.Padding(2);
			this.txt_Barcode.Name = "txt_Barcode";
			this.txt_Barcode.Size = new System.Drawing.Size(215, 25);
			this.txt_Barcode.TabIndex = 1;
			this.txt_Barcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Barcode_KeyDown);
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(376, 48);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(67, 15);
			this.label2.TabIndex = 18;
			this.label2.Text = "Clear Data";
			// 
			// pn_Message
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.pn_Message, 4);
			this.pn_Message.Controls.Add(this.lb_Message);
			this.pn_Message.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pn_Message.Location = new System.Drawing.Point(5, 77);
			this.pn_Message.Margin = new System.Windows.Forms.Padding(2);
			this.pn_Message.Name = "pn_Message";
			this.pn_Message.Size = new System.Drawing.Size(738, 29);
			this.pn_Message.TabIndex = 2;
			// 
			// lb_Message
			// 
			this.lb_Message.AutoSize = true;
			this.lb_Message.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lb_Message.ForeColor = System.Drawing.Color.Red;
			this.lb_Message.Location = new System.Drawing.Point(0, 0);
			this.lb_Message.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lb_Message.Name = "lb_Message";
			this.lb_Message.Size = new System.Drawing.Size(439, 15);
			this.lb_Message.TabIndex = 1;
			this.lb_Message.Text = "       .......                                                                   " +
    "                           ";
			// 
			// gv_Header
			// 
			this.gv_Header.AllowUserToAddRows = false;
			this.gv_Header.AllowUserToDeleteRows = false;
			this.gv_Header.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.gv_Header.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gv_Header.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.gv_Header.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.tableLayoutPanel1.SetColumnSpan(this.gv_Header, 4);
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Transparent;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.gv_Header.DefaultCellStyle = dataGridViewCellStyle2;
			this.gv_Header.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gv_Header.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.gv_Header.Location = new System.Drawing.Point(5, 113);
			this.gv_Header.Margin = new System.Windows.Forms.Padding(2);
			this.gv_Header.Name = "gv_Header";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gv_Header.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.gv_Header.RowHeadersWidth = 72;
			this.gv_Header.RowTemplate.Height = 36;
			this.gv_Header.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gv_Header.Size = new System.Drawing.Size(738, 414);
			this.gv_Header.TabIndex = 12;
			this.gv_Header.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gv_Header_CellFormatting);
			this.gv_Header.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gv_Header_MouseDown);
			// 
			// bt_export
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.bt_export, 4);
			this.bt_export.Dock = System.Windows.Forms.DockStyle.Fill;
			this.bt_export.Location = new System.Drawing.Point(5, 534);
			this.bt_export.Margin = new System.Windows.Forms.Padding(2);
			this.bt_export.Name = "bt_export";
			this.bt_export.Size = new System.Drawing.Size(738, 45);
			this.bt_export.TabIndex = 2;
			this.bt_export.Text = "SAP TXT EXPORT";
			this.bt_export.UseVisualStyleBackColor = true;
			this.bt_export.Click += new System.EventHandler(this.bt_export_Click);
			// 
			// cb_FileChoice
			// 
			this.cb_FileChoice.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.cb_FileChoice.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cb_FileChoice.FormattingEnabled = true;
			this.cb_FileChoice.Location = new System.Drawing.Point(525, 8);
			this.cb_FileChoice.Margin = new System.Windows.Forms.Padding(2);
			this.cb_FileChoice.Name = "cb_FileChoice";
			this.cb_FileChoice.Size = new System.Drawing.Size(218, 23);
			this.cb_FileChoice.TabIndex = 15;
			this.cb_FileChoice.DropDownClosed += new System.EventHandler(this.cb_FileChoice_DropDownClosed);
			// 
			// lb_FileChoice
			// 
			this.lb_FileChoice.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lb_FileChoice.AutoSize = true;
			this.lb_FileChoice.Location = new System.Drawing.Point(376, 12);
			this.lb_FileChoice.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lb_FileChoice.Name = "lb_FileChoice";
			this.lb_FileChoice.Size = new System.Drawing.Size(74, 15);
			this.lb_FileChoice.TabIndex = 17;
			this.lb_FileChoice.Text = "Choose File";
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(5, 48);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 15);
			this.label1.TabIndex = 3;
			this.label1.Text = "Barcode Scan";
			// 
			// pic_import
			// 
			this.pic_import.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.pic_import.Image = ((System.Drawing.Image)(resources.GetObject("pic_import.Image")));
			this.pic_import.Location = new System.Drawing.Point(154, 11);
			this.pic_import.Margin = new System.Windows.Forms.Padding(2);
			this.pic_import.Name = "pic_import";
			this.pic_import.Size = new System.Drawing.Size(22, 17);
			this.pic_import.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pic_import.TabIndex = 3;
			this.pic_import.TabStop = false;
			this.pic_import.Click += new System.EventHandler(this.pic_import_Click);
			// 
			// lb_FileImport
			// 
			this.lb_FileImport.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lb_FileImport.AutoSize = true;
			this.lb_FileImport.Location = new System.Drawing.Point(5, 12);
			this.lb_FileImport.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lb_FileImport.Name = "lb_FileImport";
			this.lb_FileImport.Size = new System.Drawing.Size(101, 15);
			this.lb_FileImport.TabIndex = 1;
			this.lb_FileImport.Text = "Shipping Import";
			// 
			// MS
			// 
			this.MS.ImageScalingSize = new System.Drawing.Size(28, 28);
			this.MS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Print});
			this.MS.Name = "MS";
			this.MS.Size = new System.Drawing.Size(139, 28);
			// 
			// Print
			// 
			this.Print.Name = "Print";
			this.Print.Size = new System.Drawing.Size(138, 24);
			this.Print.Text = "標籤補印";
			this.Print.Click += new System.EventHandler(this.Print_Click);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
			// 
			// button1
			// 
			this.button1.Enabled = false;
			this.button1.Location = new System.Drawing.Point(268, 282);
			this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(67, 21);
			this.button1.TabIndex = 2;
			this.button1.Text = "搜尋";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Visible = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(340, 282);
			this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(67, 19);
			this.button2.TabIndex = 3;
			this.button2.Text = "button2";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Visible = false;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// contextMenuStrip2
			// 
			this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.contextMenuStrip2.Name = "contextMenuStrip2";
			this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
			// 
			// fMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(748, 584);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.tableLayoutPanel1);
			this.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "fMain";
			this.Text = "收料";
			this.Load += new System.EventHandler(this.fMain_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.pn_Message.ResumeLayout(false);
			this.pn_Message.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gv_Header)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pic_import)).EndInit();
			this.MS.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lb_FileImport;
		private System.Windows.Forms.PictureBox pic_import;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lb_FileChoice;
		private System.Windows.Forms.ComboBox cb_FileChoice;
		private System.Windows.Forms.Button bt_export;
		private System.Windows.Forms.DataGridView gv_Header;
		private System.Windows.Forms.Panel pn_Message;
		private System.Windows.Forms.Label lb_Message;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox ck_Display;
		private System.Windows.Forms.ContextMenuStrip MS;
		private System.Windows.Forms.ToolStripMenuItem Print;
		private System.Windows.Forms.TextBox txt_Barcode;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
	}
}

