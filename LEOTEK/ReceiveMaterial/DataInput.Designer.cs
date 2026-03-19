
namespace ReceiveMaterial
{
	partial class DataInput
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.bt_confirm = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.lb_Item = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lb_Part_No = new System.Windows.Forms.Label();
			this.lb_WO = new System.Windows.Forms.Label();
			this.lb_PO = new System.Windows.Forms.Label();
			this.txt_QTY = new System.Windows.Forms.TextBox();
			this.txt_Carton = new System.Windows.Forms.TextBox();
			this.txt_PALLET = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.lb_QTY = new System.Windows.Forms.Label();
			this.lb_Carton = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.cb_BarCode_Type = new System.Windows.Forms.ComboBox();
			this.lb_Barcode_Type = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetDouble;
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.bt_confirm, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.label5, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.lb_Item, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lb_Part_No, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lb_WO, 3, 3);
			this.tableLayoutPanel1.Controls.Add(this.lb_PO, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.txt_QTY, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.txt_Carton, 3, 2);
			this.tableLayoutPanel1.Controls.Add(this.txt_PALLET, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.label4, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.lb_QTY, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lb_Carton, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label1, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.cb_BarCode_Type, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lb_Barcode_Type, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(951, 236);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// bt_confirm
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.bt_confirm, 4);
			this.bt_confirm.Dock = System.Windows.Forms.DockStyle.Fill;
			this.bt_confirm.Location = new System.Drawing.Point(6, 190);
			this.bt_confirm.Name = "bt_confirm";
			this.bt_confirm.Size = new System.Drawing.Size(939, 40);
			this.bt_confirm.TabIndex = 1;
			this.bt_confirm.Text = "確認";
			this.bt_confirm.UseVisualStyleBackColor = true;
			this.bt_confirm.Click += new System.EventHandler(this.bt_confirm_Click);
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(568, 60);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(52, 21);
			this.label5.TabIndex = 5;
			this.label5.Text = "項次";
			// 
			// lb_Item
			// 
			this.lb_Item.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lb_Item.AutoSize = true;
			this.lb_Item.Location = new System.Drawing.Point(801, 60);
			this.lb_Item.Name = "lb_Item";
			this.lb_Item.Size = new System.Drawing.Size(60, 21);
			this.lb_Item.TabIndex = 4;
			this.lb_Item.Text = "          ";
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(94, 106);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(52, 21);
			this.label3.TabIndex = 5;
			this.label3.Text = "料號";
			// 
			// lb_Part_No
			// 
			this.lb_Part_No.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lb_Part_No.AutoSize = true;
			this.lb_Part_No.Location = new System.Drawing.Point(337, 106);
			this.lb_Part_No.Name = "lb_Part_No";
			this.lb_Part_No.Size = new System.Drawing.Size(40, 21);
			this.lb_Part_No.TabIndex = 4;
			this.lb_Part_No.Text = "      ";
			// 
			// lb_WO
			// 
			this.lb_WO.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lb_WO.AutoSize = true;
			this.lb_WO.Location = new System.Drawing.Point(798, 152);
			this.lb_WO.Name = "lb_WO";
			this.lb_WO.Size = new System.Drawing.Size(65, 21);
			this.lb_WO.TabIndex = 7;
			this.lb_WO.Text = "           ";
			// 
			// lb_PO
			// 
			this.lb_PO.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lb_PO.AutoSize = true;
			this.lb_PO.Location = new System.Drawing.Point(337, 60);
			this.lb_PO.Name = "lb_PO";
			this.lb_PO.Size = new System.Drawing.Size(40, 21);
			this.lb_PO.TabIndex = 3;
			this.lb_PO.Text = "      ";
			// 
			// txt_QTY
			// 
			this.txt_QTY.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.txt_QTY.Location = new System.Drawing.Point(266, 146);
			this.txt_QTY.Name = "txt_QTY";
			this.txt_QTY.Size = new System.Drawing.Size(182, 33);
			this.txt_QTY.TabIndex = 3;
			this.txt_QTY.TextChanged += new System.EventHandler(this.txt_QTY_TextChanged);
			// 
			// txt_Carton
			// 
			this.txt_Carton.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.txt_Carton.Location = new System.Drawing.Point(740, 100);
			this.txt_Carton.Name = "txt_Carton";
			this.txt_Carton.Size = new System.Drawing.Size(182, 33);
			this.txt_Carton.TabIndex = 2;
			// 
			// txt_PALLET
			// 
			this.txt_PALLET.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.txt_PALLET.Location = new System.Drawing.Point(740, 8);
			this.txt_PALLET.Name = "txt_PALLET";
			this.txt_PALLET.Size = new System.Drawing.Size(182, 33);
			this.txt_PALLET.TabIndex = 1;
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(572, 152);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(43, 21);
			this.label4.TabIndex = 6;
			this.label4.Text = "WO";
			// 
			// lb_QTY
			// 
			this.lb_QTY.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lb_QTY.AutoSize = true;
			this.lb_QTY.Location = new System.Drawing.Point(94, 152);
			this.lb_QTY.Name = "lb_QTY";
			this.lb_QTY.Size = new System.Drawing.Size(52, 21);
			this.lb_QTY.TabIndex = 5;
			this.lb_QTY.Text = "數量";
			// 
			// lb_Carton
			// 
			this.lb_Carton.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lb_Carton.AutoSize = true;
			this.lb_Carton.Location = new System.Drawing.Point(562, 106);
			this.lb_Carton.Name = "lb_Carton";
			this.lb_Carton.Size = new System.Drawing.Size(64, 21);
			this.lb_Carton.TabIndex = 4;
			this.lb_Carton.Text = "Carton";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(59, 60);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(122, 21);
			this.label2.TabIndex = 2;
			this.label2.Text = "REFERENCE";
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(547, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(94, 21);
			this.label1.TabIndex = 1;
			this.label1.Text = "棧板編號";
			// 
			// cb_BarCode_Type
			// 
			this.cb_BarCode_Type.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.cb_BarCode_Type.FormattingEnabled = true;
			this.cb_BarCode_Type.Location = new System.Drawing.Point(272, 10);
			this.cb_BarCode_Type.Name = "cb_BarCode_Type";
			this.cb_BarCode_Type.Size = new System.Drawing.Size(169, 29);
			this.cb_BarCode_Type.TabIndex = 1;
			// 
			// lb_Barcode_Type
			// 
			this.lb_Barcode_Type.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lb_Barcode_Type.AutoSize = true;
			this.lb_Barcode_Type.Location = new System.Drawing.Point(73, 14);
			this.lb_Barcode_Type.Name = "lb_Barcode_Type";
			this.lb_Barcode_Type.Size = new System.Drawing.Size(94, 21);
			this.lb_Barcode_Type.TabIndex = 0;
			this.lb_Barcode_Type.Text = "標籤類型";
			// 
			// DataInput
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(951, 236);
			this.Controls.Add(this.tableLayoutPanel1);
			this.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.Name = "DataInput";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "fOutBarcode";
			this.Load += new System.EventHandler(this.DataInput_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cb_BarCode_Type;
		private System.Windows.Forms.Label lb_Barcode_Type;
		private System.Windows.Forms.Label lb_Carton;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lb_QTY;
		private System.Windows.Forms.TextBox txt_QTY;
		private System.Windows.Forms.TextBox txt_Carton;
		private System.Windows.Forms.TextBox txt_PALLET;
		private System.Windows.Forms.Label lb_PO;
		private System.Windows.Forms.Label lb_WO;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lb_Part_No;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lb_Item;
		private System.Windows.Forms.Button bt_confirm;
	}
}