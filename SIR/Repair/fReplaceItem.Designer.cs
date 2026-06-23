namespace RepairDll
{
    partial class fReplaceItem
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.LabSN = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.editLC = new System.Windows.Forms.TextBox();
            this.LabLC = new System.Windows.Forms.Label();
            this.editItemNo = new System.Windows.Forms.TextBox();
            this.LabItem = new System.Windows.Forms.Label();
            this.editLotNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.editVendorCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.editDateCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSearchKP = new System.Windows.Forms.Button();
            this.btnFilterVendor = new System.Windows.Forms.Button();
            this.lablVendorName = new System.Windows.Forms.Label();
            this.lablItemSpec = new System.Windows.Forms.Label();
            this.chkbBGA = new System.Windows.Forms.CheckBox();
            this.combBGAType = new System.Windows.Forms.ComboBox();
            this.editReelNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkbDate = new System.Windows.Forms.CheckBox();
            this.dtMaterialDate = new System.Windows.Forms.DateTimePicker();
            this.lablReelPartSpec = new System.Windows.Forms.Label();
            this.lablReelPartNo = new System.Windows.Forms.Label();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightBlue;
            this.panel4.Controls.Add(this.LabSN);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(388, 34);
            this.panel4.TabIndex = 4;
            // 
            // LabSN
            // 
            this.LabSN.AutoSize = true;
            this.LabSN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LabSN.Font = new System.Drawing.Font("新細明體", 12F);
            this.LabSN.ForeColor = System.Drawing.Color.Maroon;
            this.LabSN.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabSN.Location = new System.Drawing.Point(127, 9);
            this.LabSN.Name = "LabSN";
            this.LabSN.Size = new System.Drawing.Size(53, 18);
            this.LabSN.TabIndex = 1;
            this.LabSN.Text = "LabSN";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Serial Number";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Controls.Add(this.btnOK);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 450);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(388, 45);
            this.panel3.TabIndex = 23;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(219, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 36;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(91, 7);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 35;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // editLC
            // 
            this.editLC.Font = new System.Drawing.Font("新細明體", 12F);
            this.editLC.Location = new System.Drawing.Point(127, 40);
            this.editLC.Name = "editLC";
            this.editLC.Size = new System.Drawing.Size(184, 27);
            this.editLC.TabIndex = 24;
            this.editLC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editLC_KeyPress);
            // 
            // LabLC
            // 
            this.LabLC.AutoSize = true;
            this.LabLC.Font = new System.Drawing.Font("新細明體", 12F);
            this.LabLC.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabLC.Location = new System.Drawing.Point(12, 43);
            this.LabLC.Name = "LabLC";
            this.LabLC.Size = new System.Drawing.Size(63, 16);
            this.LabLC.TabIndex = 25;
            this.LabLC.Text = "Location";
            // 
            // editItemNo
            // 
            this.editItemNo.BackColor = System.Drawing.Color.White;
            this.editItemNo.Font = new System.Drawing.Font("新細明體", 12F);
            this.editItemNo.Location = new System.Drawing.Point(127, 70);
            this.editItemNo.Name = "editItemNo";
            this.editItemNo.Size = new System.Drawing.Size(184, 27);
            this.editItemNo.TabIndex = 26;
            this.editItemNo.TextChanged += new System.EventHandler(this.editItemNo_TextChanged);
            this.editItemNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editItemNo_KeyPress);
            // 
            // LabItem
            // 
            this.LabItem.AutoSize = true;
            this.LabItem.Font = new System.Drawing.Font("新細明體", 12F);
            this.LabItem.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabItem.Location = new System.Drawing.Point(12, 70);
            this.LabItem.Name = "LabItem";
            this.LabItem.Size = new System.Drawing.Size(59, 16);
            this.LabItem.TabIndex = 27;
            this.LabItem.Text = "Item No";
            // 
            // editLotNo
            // 
            this.editLotNo.Font = new System.Drawing.Font("新細明體", 12F);
            this.editLotNo.Location = new System.Drawing.Point(128, 302);
            this.editLotNo.Name = "editLotNo";
            this.editLotNo.Size = new System.Drawing.Size(184, 27);
            this.editLotNo.TabIndex = 42;
            this.editLotNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editLotNo_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 12F);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(13, 302);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 16);
            this.label3.TabIndex = 31;
            this.label3.Text = "Lot No";
            // 
            // editVendorCode
            // 
            this.editVendorCode.Font = new System.Drawing.Font("新細明體", 12F);
            this.editVendorCode.Location = new System.Drawing.Point(127, 362);
            this.editVendorCode.Name = "editVendorCode";
            this.editVendorCode.Size = new System.Drawing.Size(184, 27);
            this.editVendorCode.TabIndex = 44;
            this.editVendorCode.TextChanged += new System.EventHandler(this.editVendorCode_TextChanged);
            this.editVendorCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editVendorCode_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 12F);
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(12, 365);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 16);
            this.label4.TabIndex = 29;
            this.label4.Text = "Vendor Code";
            // 
            // editDateCode
            // 
            this.editDateCode.Font = new System.Drawing.Font("新細明體", 12F);
            this.editDateCode.Location = new System.Drawing.Point(128, 272);
            this.editDateCode.Name = "editDateCode";
            this.editDateCode.Size = new System.Drawing.Size(184, 27);
            this.editDateCode.TabIndex = 41;
            this.editDateCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editDateCode_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 12F);
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(13, 272);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 16);
            this.label5.TabIndex = 33;
            this.label5.Text = "DateCode";
            // 
            // btnSearchKP
            // 
            this.btnSearchKP.Font = new System.Drawing.Font("新細明體", 9F);
            this.btnSearchKP.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSearchKP.Location = new System.Drawing.Point(317, 70);
            this.btnSearchKP.Name = "btnSearchKP";
            this.btnSearchKP.Size = new System.Drawing.Size(25, 24);
            this.btnSearchKP.TabIndex = 34;
            this.btnSearchKP.TabStop = false;
            this.btnSearchKP.Text = "...";
            this.btnSearchKP.UseVisualStyleBackColor = true;
            this.btnSearchKP.Click += new System.EventHandler(this.btnSearchKP_Click);
            // 
            // btnFilterVendor
            // 
            this.btnFilterVendor.Font = new System.Drawing.Font("新細明體", 9F);
            this.btnFilterVendor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFilterVendor.Location = new System.Drawing.Point(317, 362);
            this.btnFilterVendor.Name = "btnFilterVendor";
            this.btnFilterVendor.Size = new System.Drawing.Size(25, 24);
            this.btnFilterVendor.TabIndex = 35;
            this.btnFilterVendor.TabStop = false;
            this.btnFilterVendor.Text = "...";
            this.btnFilterVendor.UseVisualStyleBackColor = true;
            this.btnFilterVendor.Click += new System.EventHandler(this.btnFilterVendor_Click);
            // 
            // lablVendorName
            // 
            this.lablVendorName.AutoEllipsis = true;
            this.lablVendorName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablVendorName.Font = new System.Drawing.Font("新細明體", 12F);
            this.lablVendorName.ForeColor = System.Drawing.Color.Maroon;
            this.lablVendorName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablVendorName.Location = new System.Drawing.Point(127, 392);
            this.lablVendorName.Name = "lablVendorName";
            this.lablVendorName.Size = new System.Drawing.Size(185, 50);
            this.lablVendorName.TabIndex = 36;
            // 
            // lablItemSpec
            // 
            this.lablItemSpec.AutoEllipsis = true;
            this.lablItemSpec.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablItemSpec.Font = new System.Drawing.Font("新細明體", 12F);
            this.lablItemSpec.ForeColor = System.Drawing.Color.Maroon;
            this.lablItemSpec.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablItemSpec.Location = new System.Drawing.Point(127, 100);
            this.lablItemSpec.Name = "lablItemSpec";
            this.lablItemSpec.Size = new System.Drawing.Size(185, 50);
            this.lablItemSpec.TabIndex = 37;
            // 
            // chkbBGA
            // 
            this.chkbBGA.AutoSize = true;
            this.chkbBGA.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.chkbBGA.Location = new System.Drawing.Point(13, 152);
            this.chkbBGA.Name = "chkbBGA";
            this.chkbBGA.Size = new System.Drawing.Size(91, 20);
            this.chkbBGA.TabIndex = 38;
            this.chkbBGA.TabStop = false;
            this.chkbBGA.Text = "BGA Item";
            this.chkbBGA.UseVisualStyleBackColor = true;
            this.chkbBGA.Click += new System.EventHandler(this.chkbBGA_Click);
            // 
            // combBGAType
            // 
            this.combBGAType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combBGAType.Font = new System.Drawing.Font("新細明體", 12F);
            this.combBGAType.FormattingEnabled = true;
            this.combBGAType.Items.AddRange(new object[] {
            "",
            "Change",
            "Take In",
            "Take Out",
            "Heating"});
            this.combBGAType.Location = new System.Drawing.Point(127, 152);
            this.combBGAType.Name = "combBGAType";
            this.combBGAType.Size = new System.Drawing.Size(184, 24);
            this.combBGAType.TabIndex = 39;
            // 
            // editReelNo
            // 
            this.editReelNo.Font = new System.Drawing.Font("新細明體", 12F);
            this.editReelNo.Location = new System.Drawing.Point(127, 180);
            this.editReelNo.Name = "editReelNo";
            this.editReelNo.Size = new System.Drawing.Size(184, 27);
            this.editReelNo.TabIndex = 40;
            this.editReelNo.TextChanged += new System.EventHandler(this.editReelNo_TextChanged);
            this.editReelNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editReelNo_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F);
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(13, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 16);
            this.label2.TabIndex = 41;
            this.label2.Text = "Reel No";
            // 
            // chkbDate
            // 
            this.chkbDate.AutoSize = true;
            this.chkbDate.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.chkbDate.Location = new System.Drawing.Point(16, 332);
            this.chkbDate.Name = "chkbDate";
            this.chkbDate.Size = new System.Drawing.Size(111, 20);
            this.chkbDate.TabIndex = 44;
            this.chkbDate.TabStop = false;
            this.chkbDate.Text = "Material Date";
            this.chkbDate.UseVisualStyleBackColor = true;
            // 
            // dtMaterialDate
            // 
            this.dtMaterialDate.Font = new System.Drawing.Font("新細明體", 12F);
            this.dtMaterialDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtMaterialDate.Location = new System.Drawing.Point(128, 332);
            this.dtMaterialDate.Name = "dtMaterialDate";
            this.dtMaterialDate.Size = new System.Drawing.Size(184, 27);
            this.dtMaterialDate.TabIndex = 43;
            this.dtMaterialDate.Value = new System.DateTime(2009, 8, 4, 0, 0, 0, 0);
            // 
            // lablReelPartSpec
            // 
            this.lablReelPartSpec.AutoEllipsis = true;
            this.lablReelPartSpec.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablReelPartSpec.Font = new System.Drawing.Font("新細明體", 12F);
            this.lablReelPartSpec.ForeColor = System.Drawing.Color.Maroon;
            this.lablReelPartSpec.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablReelPartSpec.Location = new System.Drawing.Point(128, 238);
            this.lablReelPartSpec.Name = "lablReelPartSpec";
            this.lablReelPartSpec.Size = new System.Drawing.Size(184, 31);
            this.lablReelPartSpec.TabIndex = 45;
            // 
            // lablReelPartNo
            // 
            this.lablReelPartNo.AutoEllipsis = true;
            this.lablReelPartNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablReelPartNo.Font = new System.Drawing.Font("新細明體", 12F);
            this.lablReelPartNo.ForeColor = System.Drawing.Color.Maroon;
            this.lablReelPartNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablReelPartNo.Location = new System.Drawing.Point(128, 210);
            this.lablReelPartNo.Name = "lablReelPartNo";
            this.lablReelPartNo.Size = new System.Drawing.Size(184, 25);
            this.lablReelPartNo.TabIndex = 46;
            // 
            // fReplaceItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 495);
            this.Controls.Add(this.lablReelPartNo);
            this.Controls.Add(this.lablReelPartSpec);
            this.Controls.Add(this.dtMaterialDate);
            this.Controls.Add(this.chkbDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.editReelNo);
            this.Controls.Add(this.combBGAType);
            this.Controls.Add(this.chkbBGA);
            this.Controls.Add(this.lablItemSpec);
            this.Controls.Add(this.lablVendorName);
            this.Controls.Add(this.btnFilterVendor);
            this.Controls.Add(this.btnSearchKP);
            this.Controls.Add(this.editDateCode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.editLotNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.editVendorCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.editItemNo);
            this.Controls.Add(this.LabItem);
            this.Controls.Add(this.editLC);
            this.Controls.Add(this.LabLC);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Name = "fReplaceItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Replace Item";
            this.Load += new System.EventHandler(this.fReplaceItem_Load);
            this.Shown += new System.EventHandler(this.fReplaceItem_Shown);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        public System.Windows.Forms.Label LabSN;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox editLC;
        private System.Windows.Forms.Label LabLC;
        private System.Windows.Forms.TextBox editItemNo;
        private System.Windows.Forms.Label LabItem;
        private System.Windows.Forms.TextBox editLotNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox editVendorCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox editDateCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSearchKP;
        private System.Windows.Forms.Button btnFilterVendor;
        public System.Windows.Forms.Label lablVendorName;
        public System.Windows.Forms.Label lablItemSpec;
        private System.Windows.Forms.CheckBox chkbBGA;
        private System.Windows.Forms.ComboBox combBGAType;
        private System.Windows.Forms.TextBox editReelNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkbDate;
        private System.Windows.Forms.DateTimePicker dtMaterialDate;
        public System.Windows.Forms.Label lablReelPartSpec;
        public System.Windows.Forms.Label lablReelPartNo;
    }
}