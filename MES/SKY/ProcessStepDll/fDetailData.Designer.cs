namespace ProcessStepDll
{
    partial class fDetailData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fDetailData));
            this.panelControl = new System.Windows.Forms.Panel();
            this.combCSN = new System.Windows.Forms.ComboBox();
            this.LabMultiRC = new System.Windows.Forms.Label();
            this.LabProcess = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.editStepName = new System.Windows.Forms.TextBox();
            this.editStepCode = new System.Windows.Forms.TextBox();
            this.LabCode = new System.Windows.Forms.Label();
            this.LabName = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.combATETest = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.combATEVerify = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panelControl.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            resources.ApplyResources(this.panelControl, "panelControl");
            this.panelControl.BackColor = System.Drawing.Color.Transparent;
            this.panelControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelControl.Controls.Add(this.combATEVerify);
            this.panelControl.Controls.Add(this.label3);
            this.panelControl.Controls.Add(this.combATETest);
            this.panelControl.Controls.Add(this.label2);
            this.panelControl.Controls.Add(this.combCSN);
            this.panelControl.Controls.Add(this.LabMultiRC);
            this.panelControl.Controls.Add(this.LabProcess);
            this.panelControl.Controls.Add(this.label1);
            this.panelControl.Controls.Add(this.editStepName);
            this.panelControl.Controls.Add(this.editStepCode);
            this.panelControl.Controls.Add(this.LabCode);
            this.panelControl.Controls.Add(this.LabName);
            this.panelControl.Name = "panelControl";
            // 
            // combCSN
            // 
            this.combCSN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.combCSN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combCSN.FormattingEnabled = true;
            this.combCSN.Items.AddRange(new object[] {
            resources.GetString("combCSN.Items"),
            resources.GetString("combCSN.Items1")});
            resources.ApplyResources(this.combCSN, "combCSN");
            this.combCSN.Name = "combCSN";
            // 
            // LabMultiRC
            // 
            resources.ApplyResources(this.LabMultiRC, "LabMultiRC");
            this.LabMultiRC.BackColor = System.Drawing.Color.Transparent;
            this.LabMultiRC.Name = "LabMultiRC";
            // 
            // LabProcess
            // 
            resources.ApplyResources(this.LabProcess, "LabProcess");
            this.LabProcess.BackColor = System.Drawing.Color.Transparent;
            this.LabProcess.ForeColor = System.Drawing.Color.Maroon;
            this.LabProcess.Name = "LabProcess";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // editStepName
            // 
            this.editStepName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.editStepName, "editStepName");
            this.editStepName.Name = "editStepName";
            this.editStepName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editStepName_KeyPress);
            // 
            // editStepCode
            // 
            this.editStepCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.editStepCode, "editStepCode");
            this.editStepCode.Name = "editStepCode";
            this.editStepCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editStepCode_KeyPress);
            // 
            // LabCode
            // 
            resources.ApplyResources(this.LabCode, "LabCode");
            this.LabCode.BackColor = System.Drawing.Color.Transparent;
            this.LabCode.Name = "LabCode";
            // 
            // LabName
            // 
            resources.ApplyResources(this.LabName, "LabName");
            this.LabName.BackColor = System.Drawing.Color.Transparent;
            this.LabName.Name = "LabName";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // combATETest
            // 
            this.combATETest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.combATETest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combATETest.FormattingEnabled = true;
            this.combATETest.Items.AddRange(new object[] {
            resources.GetString("combATETest.Items"),
            resources.GetString("combATETest.Items1")});
            resources.ApplyResources(this.combATETest, "combATETest");
            this.combATETest.Name = "combATETest";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // combATEVerify
            // 
            this.combATEVerify.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.combATEVerify.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combATEVerify.FormattingEnabled = true;
            this.combATEVerify.Items.AddRange(new object[] {
            resources.GetString("combATEVerify.Items"),
            resources.GetString("combATEVerify.Items1")});
            resources.ApplyResources(this.combATEVerify, "combATEVerify");
            this.combATEVerify.Name = "combATEVerify";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Name = "label3";
            // 
            // fDetailData
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.panel2);
            this.Name = "fDetailData";
            this.Load += new System.EventHandler(this.fData_Load);
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox editStepCode;
        private System.Windows.Forms.Label LabCode;
        private System.Windows.Forms.TextBox editStepName;
        private System.Windows.Forms.Label LabName;
        private System.Windows.Forms.Label LabProcess;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox combCSN;
        private System.Windows.Forms.Label LabMultiRC;
        private System.Windows.Forms.ComboBox combATEVerify;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox combATETest;
        private System.Windows.Forms.Label label2;
    }
}