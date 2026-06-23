namespace RepairDll
{
    partial class fAddDefect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fAddDefect));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.editDefect = new System.Windows.Forms.TextBox();
            this.editLocation = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labldefectDesc = new System.Windows.Forms.Label();
            this.btnFilterDefect = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // editDefect
            // 
            resources.ApplyResources(this.editDefect, "editDefect");
            this.editDefect.Name = "editDefect";
            this.editDefect.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editDefect_KeyPress);
            // 
            // editLocation
            // 
            resources.ApplyResources(this.editLocation, "editLocation");
            this.editLocation.Name = "editLocation";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labldefectDesc);
            this.panel1.Controls.Add(this.btnFilterDefect);
            this.panel1.Controls.Add(this.editDefect);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.editLocation);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // labldefectDesc
            // 
            this.labldefectDesc.AutoEllipsis = true;
            this.labldefectDesc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.labldefectDesc, "labldefectDesc");
            this.labldefectDesc.ForeColor = System.Drawing.Color.Maroon;
            this.labldefectDesc.Name = "labldefectDesc";
            // 
            // btnFilterDefect
            // 
            resources.ApplyResources(this.btnFilterDefect, "btnFilterDefect");
            this.btnFilterDefect.Name = "btnFilterDefect";
            this.btnFilterDefect.UseVisualStyleBackColor = true;
            this.btnFilterDefect.Click += new System.EventHandler(this.btnFilterDefect_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.button2);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // fAddDefect
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fAddDefect";
            this.Load += new System.EventHandler(this.fAddDefect_Load);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.fAddDefect_HelpRequested);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.TextBox editDefect;
        public System.Windows.Forms.TextBox editLocation;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnFilterDefect;
        public System.Windows.Forms.Label labldefectDesc;
    }
}