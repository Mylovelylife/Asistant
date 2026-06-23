namespace CWoManagerPcs
{
    partial class fSpec
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.GB = new System.Windows.Forms.GroupBox();
            this.panelWorkOrder = new System.Windows.Forms.Panel();
            this.btnQueryWorkOrder = new System.Windows.Forms.Button();
            this.txtWorkOrder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panelSpecCode = new System.Windows.Forms.Panel();
            this.btnGenerateSpec = new System.Windows.Forms.Button();
            this.txtEndNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtStartNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSpecCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelImportExcel = new System.Windows.Forms.Panel();
            this.btnImportExcel = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtExcelPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.GB.SuspendLayout();
            this.panelWorkOrder.SuspendLayout();
            this.panelSpecCode.SuspendLayout();
            this.panelImportExcel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 486);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(818, 50);
            this.panel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(435, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 30);
            this.button1.TabIndex = 6;
            this.button1.Text = "清除";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(333, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(89, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "關閉";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.GB);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(818, 486);
            this.panel2.TabIndex = 1;
            // 
            // GB
            // 
            this.GB.Controls.Add(this.panelWorkOrder);
            this.GB.Controls.Add(this.panelSpecCode);
            this.GB.Controls.Add(this.panelImportExcel);
            this.GB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.GB.Location = new System.Drawing.Point(0, 0);
            this.GB.Name = "GB";
            this.GB.Size = new System.Drawing.Size(818, 486);
            this.GB.TabIndex = 0;
            this.GB.TabStop = false;
            this.GB.Text = "SPEC 維護";
            // 
            // panelWorkOrder
            // 
            this.panelWorkOrder.Controls.Add(this.btnQueryWorkOrder);
            this.panelWorkOrder.Controls.Add(this.txtWorkOrder);
            this.panelWorkOrder.Controls.Add(this.label4);
            this.panelWorkOrder.Location = new System.Drawing.Point(30, 338);
            this.panelWorkOrder.Name = "panelWorkOrder";
            this.panelWorkOrder.Size = new System.Drawing.Size(750, 100);
            this.panelWorkOrder.TabIndex = 5;
            this.panelWorkOrder.Visible = false;
            // 
            // btnQueryWorkOrder
            // 
            this.btnQueryWorkOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnQueryWorkOrder.Location = new System.Drawing.Point(611, 10);
            this.btnQueryWorkOrder.Name = "btnQueryWorkOrder";
            this.btnQueryWorkOrder.Size = new System.Drawing.Size(100, 35);
            this.btnQueryWorkOrder.TabIndex = 2;
            this.btnQueryWorkOrder.Text = "匯入";
            this.btnQueryWorkOrder.UseVisualStyleBackColor = true;
            this.btnQueryWorkOrder.Click += new System.EventHandler(this.btnQueryWorkOrder_Click);
            this.btnQueryWorkOrder.Enter += new System.EventHandler(this.btnQueryWorkOrder_Enter);
            // 
            // txtWorkOrder
            // 
            this.txtWorkOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtWorkOrder.Location = new System.Drawing.Point(156, 19);
            this.txtWorkOrder.Name = "txtWorkOrder";
            this.txtWorkOrder.Size = new System.Drawing.Size(300, 26);
            this.txtWorkOrder.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label4.Location = new System.Drawing.Point(20, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "工單編號：";
            // 
            // panelSpecCode
            // 
            this.panelSpecCode.Controls.Add(this.btnGenerateSpec);
            this.panelSpecCode.Controls.Add(this.txtEndNo);
            this.panelSpecCode.Controls.Add(this.label3);
            this.panelSpecCode.Controls.Add(this.txtStartNo);
            this.panelSpecCode.Controls.Add(this.label2);
            this.panelSpecCode.Controls.Add(this.txtSpecCode);
            this.panelSpecCode.Controls.Add(this.label1);
            this.panelSpecCode.Location = new System.Drawing.Point(30, 153);
            this.panelSpecCode.Name = "panelSpecCode";
            this.panelSpecCode.Size = new System.Drawing.Size(750, 150);
            this.panelSpecCode.TabIndex = 4;
            this.panelSpecCode.Visible = false;
            // 
            // btnGenerateSpec
            // 
            this.btnGenerateSpec.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnGenerateSpec.Location = new System.Drawing.Point(611, 74);
            this.btnGenerateSpec.Name = "btnGenerateSpec";
            this.btnGenerateSpec.Size = new System.Drawing.Size(100, 36);
            this.btnGenerateSpec.TabIndex = 6;
            this.btnGenerateSpec.Text = "產生資料";
            this.btnGenerateSpec.UseVisualStyleBackColor = true;
            this.btnGenerateSpec.Click += new System.EventHandler(this.btnGenerateSpec_Click);
            // 
            // txtEndNo
            // 
            this.txtEndNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtEndNo.Location = new System.Drawing.Point(405, 81);
            this.txtEndNo.Name = "txtEndNo";
            this.txtEndNo.Size = new System.Drawing.Size(150, 26);
            this.txtEndNo.TabIndex = 0;
            this.txtEndNo.Enter += new System.EventHandler(this.txtEndNo_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(315, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "結束序號：";
            // 
            // txtStartNo
            // 
            this.txtStartNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtStartNo.Location = new System.Drawing.Point(156, 84);
            this.txtStartNo.Name = "txtStartNo";
            this.txtStartNo.Size = new System.Drawing.Size(150, 26);
            this.txtStartNo.TabIndex = 2;
            this.txtStartNo.Enter += new System.EventHandler(this.txtStartNo_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(27, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "起始序號：";
            // 
            // txtSpecCode
            // 
            this.txtSpecCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtSpecCode.Location = new System.Drawing.Point(156, 38);
            this.txtSpecCode.Name = "txtSpecCode";
            this.txtSpecCode.Size = new System.Drawing.Size(150, 26);
            this.txtSpecCode.TabIndex = 4;
            this.txtSpecCode.Enter += new System.EventHandler(this.txtSpecCode_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(27, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "特徵碼(前輟)：";
            // 
            // panelImportExcel
            // 
            this.panelImportExcel.Controls.Add(this.btnImportExcel);
            this.panelImportExcel.Controls.Add(this.btnBrowse);
            this.panelImportExcel.Controls.Add(this.txtExcelPath);
            this.panelImportExcel.Controls.Add(this.label5);
            this.panelImportExcel.Location = new System.Drawing.Point(30, 44);
            this.panelImportExcel.Name = "panelImportExcel";
            this.panelImportExcel.Size = new System.Drawing.Size(750, 50);
            this.panelImportExcel.TabIndex = 3;
            // 
            // btnImportExcel
            // 
            this.btnImportExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportExcel.Location = new System.Drawing.Point(611, 10);
            this.btnImportExcel.Name = "btnImportExcel";
            this.btnImportExcel.Size = new System.Drawing.Size(100, 30);
            this.btnImportExcel.TabIndex = 0;
            this.btnImportExcel.Text = "匯入";
            this.btnImportExcel.UseVisualStyleBackColor = true;
            this.btnImportExcel.Click += new System.EventHandler(this.btnImportExcel_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBrowse.Location = new System.Drawing.Point(456, 9);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(50, 30);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtExcelPath
            // 
            this.txtExcelPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtExcelPath.Location = new System.Drawing.Point(156, 9);
            this.txtExcelPath.Name = "txtExcelPath";
            this.txtExcelPath.Size = new System.Drawing.Size(290, 26);
            this.txtExcelPath.TabIndex = 2;
            this.txtExcelPath.Enter += new System.EventHandler(this.txtExcelPath_Enter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label5.Location = new System.Drawing.Point(20, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 20);
            this.label5.TabIndex = 3;
            this.label5.Text = "Excel檔案：";
            // 
            // fSpec
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(818, 536);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fSpec";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SPEC Rule";
            this.Load += new System.EventHandler(this.fSpec_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.GB.ResumeLayout(false);
            this.panelWorkOrder.ResumeLayout(false);
            this.panelWorkOrder.PerformLayout();
            this.panelSpecCode.ResumeLayout(false);
            this.panelSpecCode.PerformLayout();
            this.panelImportExcel.ResumeLayout(false);
            this.panelImportExcel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox GB;
        private System.Windows.Forms.Panel panelImportExcel;
        private System.Windows.Forms.Button btnImportExcel;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtExcelPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panelSpecCode;
        private System.Windows.Forms.Button btnGenerateSpec;
        private System.Windows.Forms.TextBox txtEndNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtStartNo;
        private System.Windows.Forms.TextBox txtSpecCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelWorkOrder;
        private System.Windows.Forms.Button btnQueryWorkOrder;
        private System.Windows.Forms.TextBox txtWorkOrder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}