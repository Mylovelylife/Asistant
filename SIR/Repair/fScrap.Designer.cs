namespace RepairDll
{
    partial class fScrap
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.LabSN = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.editMemo = new System.Windows.Forms.TextBox();
            this.rbtnScrap = new System.Windows.Forms.RadioButton();
            this.rbtnReturn = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.LabSN);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(476, 40);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Controls.Add(this.btnOK);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 233);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(476, 39);
            this.panel3.TabIndex = 23;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(250, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(129, 7);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbtnReturn);
            this.panel2.Controls.Add(this.rbtnScrap);
            this.panel2.Controls.Add(this.editMemo);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(476, 193);
            this.panel2.TabIndex = 24;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("新細明體", 12F);
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(3, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 16);
            this.label9.TabIndex = 16;
            this.label9.Text = "Serial Number";
            // 
            // LabSN
            // 
            this.LabSN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LabSN.Font = new System.Drawing.Font("新細明體", 12F);
            this.LabSN.ForeColor = System.Drawing.Color.Maroon;
            this.LabSN.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LabSN.Location = new System.Drawing.Point(109, 9);
            this.LabSN.Name = "LabSN";
            this.LabSN.Size = new System.Drawing.Size(198, 25);
            this.LabSN.TabIndex = 17;
            this.LabSN.Text = "SN";
            this.LabSN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 16);
            this.label1.TabIndex = 17;
            this.label1.Text = "Memo";
            // 
            // editMemo
            // 
            this.editMemo.AcceptsReturn = true;
            this.editMemo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.editMemo.Location = new System.Drawing.Point(109, 10);
            this.editMemo.Multiline = true;
            this.editMemo.Name = "editMemo";
            this.editMemo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.editMemo.Size = new System.Drawing.Size(343, 142);
            this.editMemo.TabIndex = 0;
            // 
            // rbtnScrap
            // 
            this.rbtnScrap.AutoSize = true;
            this.rbtnScrap.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbtnScrap.Location = new System.Drawing.Point(111, 160);
            this.rbtnScrap.Name = "rbtnScrap";
            this.rbtnScrap.Size = new System.Drawing.Size(57, 19);
            this.rbtnScrap.TabIndex = 19;
            this.rbtnScrap.TabStop = true;
            this.rbtnScrap.Text = "Scrap";
            this.rbtnScrap.UseVisualStyleBackColor = true;
            // 
            // rbtnReturn
            // 
            this.rbtnReturn.AutoSize = true;
            this.rbtnReturn.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbtnReturn.Location = new System.Drawing.Point(204, 160);
            this.rbtnReturn.Name = "rbtnReturn";
            this.rbtnReturn.Size = new System.Drawing.Size(115, 19);
            this.rbtnReturn.TabIndex = 20;
            this.rbtnReturn.TabStop = true;
            this.rbtnReturn.Text = "Return Material";
            this.rbtnReturn.UseVisualStyleBackColor = true;
            // 
            // fScrap
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(476, 272);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Name = "fScrap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scrap SN";
            this.Load += new System.EventHandler(this.fScrap_Load);
            this.Activated += new System.EventHandler(this.fScrap_Activated);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.Label LabSN;
        private System.Windows.Forms.RadioButton rbtnReturn;
        private System.Windows.Forms.RadioButton rbtnScrap;
        private System.Windows.Forms.TextBox editMemo;
        private System.Windows.Forms.Label label1;
    }
}