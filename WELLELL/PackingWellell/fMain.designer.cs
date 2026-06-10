using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
namespace PackingDll
{
    partial class fMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.split2 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gbBox = new System.Windows.Forms.GroupBox();
            this.btnClearBox = new System.Windows.Forms.Button();
            this.LabBoxCap = new System.Windows.Forms.Label();
            this.LabBoxCapacity = new System.Windows.Forms.Label();
            this.btnCloseBox = new System.Windows.Forms.Button();
            this.editBox = new System.Windows.Forms.TextBox();
            this.LabBoxQty = new System.Windows.Forms.Label();
            this.lablBox = new System.Windows.Forms.Label();
            this.LabBoxTle = new System.Windows.Forms.Label();
            this.gbCarton = new System.Windows.Forms.GroupBox();
            this.btnChangeCarton = new System.Windows.Forms.Button();
            this.LabCartonCap = new System.Windows.Forms.Label();
            this.LabCartonCapacity = new System.Windows.Forms.Label();
            this.btnCloseCarton = new System.Windows.Forms.Button();
            this.editCarton = new System.Windows.Forms.TextBox();
            this.LabCartonQty = new System.Windows.Forms.Label();
            this.LabCarton = new System.Windows.Forms.Label();
            this.LabCartonTle = new System.Windows.Forms.Label();
            this.gbPallet = new System.Windows.Forms.GroupBox();
            this.LabPalletCap = new System.Windows.Forms.Label();
            this.btnClosePallet = new System.Windows.Forms.Button();
            this.editPallet = new System.Windows.Forms.TextBox();
            this.LabPalletCapacity = new System.Windows.Forms.Label();
            this.LabPallet = new System.Windows.Forms.Label();
            this.LabPalletTle = new System.Windows.Forms.Label();
            this.LabPalletQty = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.gbPkspec = new System.Windows.Forms.GroupBox();
            this.LVPackSpec = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBox = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCarton = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPallet = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderInner = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.listValue = new System.Windows.Forms.ListBox();
            this.listField = new System.Windows.Forms.ListBox();
            this.ListData = new System.Windows.Forms.ListBox();
            this.ListParam = new System.Windows.Forms.ListBox();
            this.LabChangSpec = new System.Windows.Forms.LinkLabel();
            this.gbSN = new System.Windows.Forms.GroupBox();
            this.LVEC = new System.Windows.Forms.ListView();
            this.columnHeaderDefect = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDefDesc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PanelSNInput = new System.Windows.Forms.Panel();
            this.editSN = new System.Windows.Forms.TextBox();
            this.labCustSN = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.editCSN = new System.Windows.Forms.TextBox();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.panelWO = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnChangeWo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearchWO = new System.Windows.Forms.Button();
            this.editWO = new System.Windows.Forms.TextBox();
            this.LabPart = new System.Windows.Forms.Label();
            this.LabPKBase = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.LabWo = new System.Windows.Forms.Label();
            this.LabPKAction = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lb_Station = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LabWoVersion = new System.Windows.Forms.Label();
            this.lb_process = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.LabPartDesc = new System.Windows.Forms.Label();
            this.LabTargetQty = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.split1 = new System.Windows.Forms.SplitContainer();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.TextMsg = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.split2)).BeginInit();
            this.split2.Panel1.SuspendLayout();
            this.split2.Panel2.SuspendLayout();
            this.split2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbBox.SuspendLayout();
            this.gbCarton.SuspendLayout();
            this.gbPallet.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.gbPkspec.SuspendLayout();
            this.panel3.SuspendLayout();
            this.gbSN.SuspendLayout();
            this.PanelSNInput.SuspendLayout();
            this.panelWO.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split1)).BeginInit();
            this.split1.Panel1.SuspendLayout();
            this.split1.Panel2.SuspendLayout();
            this.split1.SuspendLayout();
            this.SuspendLayout();
            // 
            // split2
            // 
            resources.ApplyResources(this.split2, "split2");
            this.split2.Name = "split2";
            // 
            // split2.Panel1
            // 
            resources.ApplyResources(this.split2.Panel1, "split2.Panel1");
            this.split2.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // split2.Panel2
            // 
            resources.ApplyResources(this.split2.Panel2, "split2.Panel2");
            this.split2.Panel2.Controls.Add(this.tableLayoutPanel2);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.gbBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.gbCarton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gbPallet, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // gbBox
            // 
            this.gbBox.Controls.Add(this.btnClearBox);
            this.gbBox.Controls.Add(this.LabBoxCap);
            this.gbBox.Controls.Add(this.LabBoxCapacity);
            this.gbBox.Controls.Add(this.btnCloseBox);
            this.gbBox.Controls.Add(this.editBox);
            this.gbBox.Controls.Add(this.LabBoxQty);
            this.gbBox.Controls.Add(this.lablBox);
            this.gbBox.Controls.Add(this.LabBoxTle);
            resources.ApplyResources(this.gbBox, "gbBox");
            this.gbBox.Name = "gbBox";
            this.gbBox.TabStop = false;
            this.gbBox.Enter += new System.EventHandler(this.gbBox_Enter);
            // 
            // btnClearBox
            // 
            resources.ApplyResources(this.btnClearBox, "btnClearBox");
            this.btnClearBox.Name = "btnClearBox";
            this.btnClearBox.UseVisualStyleBackColor = true;
            this.btnClearBox.Click += new System.EventHandler(this.btnClearBox_Click);
            // 
            // LabBoxCap
            // 
            resources.ApplyResources(this.LabBoxCap, "LabBoxCap");
            this.LabBoxCap.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LabBoxCap.Name = "LabBoxCap";
            // 
            // LabBoxCapacity
            // 
            this.LabBoxCapacity.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.LabBoxCapacity, "LabBoxCapacity");
            this.LabBoxCapacity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.LabBoxCapacity.Name = "LabBoxCapacity";
            // 
            // btnCloseBox
            // 
            resources.ApplyResources(this.btnCloseBox, "btnCloseBox");
            this.btnCloseBox.ForeColor = System.Drawing.Color.Black;
            this.btnCloseBox.Name = "btnCloseBox";
            this.btnCloseBox.UseVisualStyleBackColor = true;
            this.btnCloseBox.Click += new System.EventHandler(this.btnCloseBox_Click);
            // 
            // editBox
            // 
            resources.ApplyResources(this.editBox, "editBox");
            this.editBox.Name = "editBox";
            this.editBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editBox_KeyPress);
            // 
            // LabBoxQty
            // 
            this.LabBoxQty.BackColor = System.Drawing.SystemColors.Control;
            this.LabBoxQty.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.LabBoxQty, "LabBoxQty");
            this.LabBoxQty.ForeColor = System.Drawing.Color.Red;
            this.LabBoxQty.Name = "LabBoxQty";
            // 
            // lablBox
            // 
            resources.ApplyResources(this.lablBox, "lablBox");
            this.lablBox.ForeColor = System.Drawing.Color.Blue;
            this.lablBox.Name = "lablBox";
            // 
            // LabBoxTle
            // 
            resources.ApplyResources(this.LabBoxTle, "LabBoxTle");
            this.LabBoxTle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LabBoxTle.Name = "LabBoxTle";
            // 
            // gbCarton
            // 
            this.gbCarton.Controls.Add(this.btnChangeCarton);
            this.gbCarton.Controls.Add(this.LabCartonCap);
            this.gbCarton.Controls.Add(this.LabCartonCapacity);
            this.gbCarton.Controls.Add(this.btnCloseCarton);
            this.gbCarton.Controls.Add(this.editCarton);
            this.gbCarton.Controls.Add(this.LabCartonQty);
            this.gbCarton.Controls.Add(this.LabCarton);
            this.gbCarton.Controls.Add(this.LabCartonTle);
            resources.ApplyResources(this.gbCarton, "gbCarton");
            this.gbCarton.Name = "gbCarton";
            this.gbCarton.TabStop = false;
            // 
            // btnChangeCarton
            // 
            resources.ApplyResources(this.btnChangeCarton, "btnChangeCarton");
            this.btnChangeCarton.Name = "btnChangeCarton";
            this.btnChangeCarton.UseVisualStyleBackColor = true;
            this.btnChangeCarton.Click += new System.EventHandler(this.btnChangeCarton_Click);
            // 
            // LabCartonCap
            // 
            resources.ApplyResources(this.LabCartonCap, "LabCartonCap");
            this.LabCartonCap.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LabCartonCap.Name = "LabCartonCap";
            // 
            // LabCartonCapacity
            // 
            this.LabCartonCapacity.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.LabCartonCapacity, "LabCartonCapacity");
            this.LabCartonCapacity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.LabCartonCapacity.Name = "LabCartonCapacity";
            // 
            // btnCloseCarton
            // 
            resources.ApplyResources(this.btnCloseCarton, "btnCloseCarton");
            this.btnCloseCarton.ForeColor = System.Drawing.Color.Black;
            this.btnCloseCarton.Name = "btnCloseCarton";
            this.btnCloseCarton.UseVisualStyleBackColor = true;
            this.btnCloseCarton.Click += new System.EventHandler(this.btnCloseCarton_Click);
            // 
            // editCarton
            // 
            resources.ApplyResources(this.editCarton, "editCarton");
            this.editCarton.Name = "editCarton";
            this.editCarton.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editCarton_KeyPress);
            // 
            // LabCartonQty
            // 
            this.LabCartonQty.BackColor = System.Drawing.SystemColors.Control;
            this.LabCartonQty.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.LabCartonQty, "LabCartonQty");
            this.LabCartonQty.ForeColor = System.Drawing.Color.Red;
            this.LabCartonQty.Name = "LabCartonQty";
            // 
            // LabCarton
            // 
            resources.ApplyResources(this.LabCarton, "LabCarton");
            this.LabCarton.ForeColor = System.Drawing.Color.Blue;
            this.LabCarton.Name = "LabCarton";
            // 
            // LabCartonTle
            // 
            resources.ApplyResources(this.LabCartonTle, "LabCartonTle");
            this.LabCartonTle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LabCartonTle.Name = "LabCartonTle";
            // 
            // gbPallet
            // 
            this.gbPallet.Controls.Add(this.LabPalletCap);
            this.gbPallet.Controls.Add(this.btnClosePallet);
            this.gbPallet.Controls.Add(this.editPallet);
            this.gbPallet.Controls.Add(this.LabPalletCapacity);
            this.gbPallet.Controls.Add(this.LabPallet);
            this.gbPallet.Controls.Add(this.LabPalletTle);
            this.gbPallet.Controls.Add(this.LabPalletQty);
            resources.ApplyResources(this.gbPallet, "gbPallet");
            this.gbPallet.Name = "gbPallet";
            this.gbPallet.TabStop = false;
            // 
            // LabPalletCap
            // 
            resources.ApplyResources(this.LabPalletCap, "LabPalletCap");
            this.LabPalletCap.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LabPalletCap.Name = "LabPalletCap";
            // 
            // btnClosePallet
            // 
            resources.ApplyResources(this.btnClosePallet, "btnClosePallet");
            this.btnClosePallet.ForeColor = System.Drawing.Color.Black;
            this.btnClosePallet.Name = "btnClosePallet";
            this.btnClosePallet.UseVisualStyleBackColor = true;
            this.btnClosePallet.Click += new System.EventHandler(this.btnClosePallet_Click);
            // 
            // editPallet
            // 
            resources.ApplyResources(this.editPallet, "editPallet");
            this.editPallet.Name = "editPallet";
            this.editPallet.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editPallet_KeyPress);
            // 
            // LabPalletCapacity
            // 
            this.LabPalletCapacity.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.LabPalletCapacity, "LabPalletCapacity");
            this.LabPalletCapacity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.LabPalletCapacity.Name = "LabPalletCapacity";
            // 
            // LabPallet
            // 
            resources.ApplyResources(this.LabPallet, "LabPallet");
            this.LabPallet.ForeColor = System.Drawing.Color.Blue;
            this.LabPallet.Name = "LabPallet";
            // 
            // LabPalletTle
            // 
            resources.ApplyResources(this.LabPalletTle, "LabPalletTle");
            this.LabPalletTle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LabPalletTle.Name = "LabPalletTle";
            // 
            // LabPalletQty
            // 
            this.LabPalletQty.BackColor = System.Drawing.SystemColors.Control;
            this.LabPalletQty.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.LabPalletQty, "LabPalletQty");
            this.LabPalletQty.ForeColor = System.Drawing.Color.Red;
            this.LabPalletQty.Name = "LabPalletQty";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.gbPkspec, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.gbSN, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // gbPkspec
            // 
            this.gbPkspec.Controls.Add(this.LVPackSpec);
            this.gbPkspec.Controls.Add(this.panel3);
            this.gbPkspec.Controls.Add(this.LabChangSpec);
            resources.ApplyResources(this.gbPkspec, "gbPkspec");
            this.gbPkspec.Name = "gbPkspec";
            this.gbPkspec.TabStop = false;
            // 
            // LVPackSpec
            // 
            this.LVPackSpec.BackColor = System.Drawing.Color.White;
            this.LVPackSpec.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderBox,
            this.columnHeaderCarton,
            this.columnHeaderPallet,
            this.columnHeaderInner});
            resources.ApplyResources(this.LVPackSpec, "LVPackSpec");
            this.LVPackSpec.FullRowSelect = true;
            this.LVPackSpec.GridLines = true;
            this.LVPackSpec.HideSelection = false;
            this.LVPackSpec.Name = "LVPackSpec";
            this.LVPackSpec.SmallImageList = this.imageList1;
            this.LVPackSpec.UseCompatibleStateImageBehavior = false;
            this.LVPackSpec.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderName
            // 
            resources.ApplyResources(this.columnHeaderName, "columnHeaderName");
            // 
            // columnHeaderBox
            // 
            resources.ApplyResources(this.columnHeaderBox, "columnHeaderBox");
            // 
            // columnHeaderCarton
            // 
            resources.ApplyResources(this.columnHeaderCarton, "columnHeaderCarton");
            // 
            // columnHeaderPallet
            // 
            resources.ApplyResources(this.columnHeaderPallet, "columnHeaderPallet");
            // 
            // columnHeaderInner
            // 
            resources.ApplyResources(this.columnHeaderInner, "columnHeaderInner");
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ok.bmp");
            this.imageList1.Images.SetKeyName(1, "cancel.bmp");
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.listValue);
            this.panel3.Controls.Add(this.listField);
            this.panel3.Controls.Add(this.ListData);
            this.panel3.Controls.Add(this.ListParam);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listValue
            // 
            resources.ApplyResources(this.listValue, "listValue");
            this.listValue.FormattingEnabled = true;
            this.listValue.Name = "listValue";
            // 
            // listField
            // 
            resources.ApplyResources(this.listField, "listField");
            this.listField.FormattingEnabled = true;
            this.listField.Name = "listField";
            // 
            // ListData
            // 
            resources.ApplyResources(this.ListData, "ListData");
            this.ListData.FormattingEnabled = true;
            this.ListData.Name = "ListData";
            // 
            // ListParam
            // 
            resources.ApplyResources(this.ListParam, "ListParam");
            this.ListParam.FormattingEnabled = true;
            this.ListParam.Name = "ListParam";
            // 
            // LabChangSpec
            // 
            this.LabChangSpec.ActiveLinkColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.LabChangSpec, "LabChangSpec");
            this.LabChangSpec.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.LabChangSpec.LinkColor = System.Drawing.Color.Black;
            this.LabChangSpec.Name = "LabChangSpec";
            this.LabChangSpec.TabStop = true;
            this.LabChangSpec.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LabChangSpec_LinkClicked);
            // 
            // gbSN
            // 
            this.gbSN.Controls.Add(this.LVEC);
            this.gbSN.Controls.Add(this.PanelSNInput);
            resources.ApplyResources(this.gbSN, "gbSN");
            this.gbSN.Name = "gbSN";
            this.gbSN.TabStop = false;
            // 
            // LVEC
            // 
            this.LVEC.BackColor = System.Drawing.Color.White;
            this.LVEC.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderDefect,
            this.columnHeaderDefDesc});
            resources.ApplyResources(this.LVEC, "LVEC");
            this.LVEC.FullRowSelect = true;
            this.LVEC.GridLines = true;
            this.LVEC.HideSelection = false;
            this.LVEC.Name = "LVEC";
            this.LVEC.UseCompatibleStateImageBehavior = false;
            this.LVEC.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderDefect
            // 
            resources.ApplyResources(this.columnHeaderDefect, "columnHeaderDefect");
            // 
            // columnHeaderDefDesc
            // 
            resources.ApplyResources(this.columnHeaderDefDesc, "columnHeaderDefDesc");
            // 
            // PanelSNInput
            // 
            this.PanelSNInput.BackColor = System.Drawing.Color.Transparent;
            this.PanelSNInput.Controls.Add(this.editSN);
            this.PanelSNInput.Controls.Add(this.labCustSN);
            this.PanelSNInput.Controls.Add(this.label5);
            this.PanelSNInput.Controls.Add(this.editCSN);
            resources.ApplyResources(this.PanelSNInput, "PanelSNInput");
            this.PanelSNInput.Name = "PanelSNInput";
            // 
            // editSN
            // 
            resources.ApplyResources(this.editSN, "editSN");
            this.editSN.Name = "editSN";
            this.editSN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editSN_KeyPress);
            // 
            // labCustSN
            // 
            resources.ApplyResources(this.labCustSN, "labCustSN");
            this.labCustSN.ForeColor = System.Drawing.Color.Black;
            this.labCustSN.Name = "labCustSN";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Name = "label5";
            // 
            // editCSN
            // 
            resources.ApplyResources(this.editCSN, "editCSN");
            this.editCSN.Name = "editCSN";
            this.editCSN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editCSN_KeyPress);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // panelWO
            // 
            this.panelWO.BackColor = System.Drawing.Color.LightBlue;
            this.panelWO.Controls.Add(this.btnSettings);
            this.panelWO.Controls.Add(this.btnChangeWo);
            this.panelWO.Controls.Add(this.label1);
            this.panelWO.Controls.Add(this.btnSearchWO);
            this.panelWO.Controls.Add(this.editWO);
            resources.ApplyResources(this.panelWO, "panelWO");
            this.panelWO.Name = "panelWO";
            // 
            // btnSettings
            // 
            resources.ApplyResources(this.btnSettings, "btnSettings");
            this.btnSettings.BackColor = System.Drawing.Color.Transparent;
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnChangeWo
            // 
            this.btnChangeWo.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnChangeWo, "btnChangeWo");
            this.btnChangeWo.Name = "btnChangeWo";
            this.btnChangeWo.UseVisualStyleBackColor = false;
            this.btnChangeWo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // btnSearchWO
            // 
            this.btnSearchWO.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnSearchWO, "btnSearchWO");
            this.btnSearchWO.Name = "btnSearchWO";
            this.btnSearchWO.UseVisualStyleBackColor = false;
            this.btnSearchWO.Click += new System.EventHandler(this.btnSearchWO_Click);
            // 
            // editWO
            // 
            this.editWO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            resources.ApplyResources(this.editWO, "editWO");
            this.editWO.Name = "editWO";
            this.editWO.EnabledChanged += new System.EventHandler(this.editWO_EnabledChanged);
            this.editWO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editWO_KeyPress);
            // 
            // LabPart
            // 
            this.LabPart.AutoEllipsis = true;
            this.LabPart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.LabPart, "LabPart");
            this.LabPart.ForeColor = System.Drawing.Color.Maroon;
            this.LabPart.Name = "LabPart";
            // 
            // LabPKBase
            // 
            this.LabPKBase.AutoEllipsis = true;
            this.LabPKBase.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.LabPKBase, "LabPKBase");
            this.LabPKBase.ForeColor = System.Drawing.Color.Maroon;
            this.LabPKBase.Name = "LabPKBase";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Name = "label9";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Name = "label6";
            // 
            // LabWo
            // 
            this.LabWo.AutoEllipsis = true;
            this.LabWo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.LabWo, "LabWo");
            this.LabWo.ForeColor = System.Drawing.Color.Maroon;
            this.LabWo.Name = "LabWo";
            // 
            // LabPKAction
            // 
            this.LabPKAction.AutoEllipsis = true;
            this.LabPKAction.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.LabPKAction, "LabPKAction");
            this.LabPKAction.ForeColor = System.Drawing.Color.Maroon;
            this.LabPKAction.Name = "LabPKAction";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Name = "label10";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.tableLayoutPanel3);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.lb_Station, 5, 1);
            this.tableLayoutPanel3.Controls.Add(this.label3, 4, 1);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.LabWoVersion, 5, 0);
            this.tableLayoutPanel3.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.lb_process, 5, 2);
            this.tableLayoutPanel3.Controls.Add(this.label13, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.LabWo, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label8, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.LabPartDesc, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.LabTargetQty, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.LabPart, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.LabPKAction, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.label11, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.LabPKBase, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.label10, 2, 2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // lb_Station
            // 
            resources.ApplyResources(this.lb_Station, "lb_Station");
            this.lb_Station.ForeColor = System.Drawing.Color.Red;
            this.lb_Station.Name = "lb_Station";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // LabWoVersion
            // 
            this.LabWoVersion.AutoEllipsis = true;
            this.LabWoVersion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.LabWoVersion, "LabWoVersion");
            this.LabWoVersion.ForeColor = System.Drawing.Color.Maroon;
            this.LabWoVersion.Name = "LabWoVersion";
            // 
            // lb_process
            // 
            resources.ApplyResources(this.lb_process, "lb_process");
            this.lb_process.ForeColor = System.Drawing.Color.Red;
            this.lb_process.Name = "lb_process";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Name = "label13";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Name = "label8";
            // 
            // LabPartDesc
            // 
            this.LabPartDesc.AutoEllipsis = true;
            this.LabPartDesc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.LabPartDesc, "LabPartDesc");
            this.LabPartDesc.ForeColor = System.Drawing.Color.Maroon;
            this.LabPartDesc.Name = "LabPartDesc";
            // 
            // LabTargetQty
            // 
            this.LabTargetQty.AutoEllipsis = true;
            this.LabTargetQty.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.LabTargetQty, "LabTargetQty");
            this.LabTargetQty.ForeColor = System.Drawing.Color.Maroon;
            this.LabTargetQty.Name = "LabTargetQty";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Name = "label11";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.split1);
            this.panel1.Name = "panel1";
            // 
            // split1
            // 
            resources.ApplyResources(this.split1, "split1");
            this.split1.Name = "split1";
            // 
            // split1.Panel1
            // 
            this.split1.Panel1.Controls.Add(this.splitter1);
            this.split1.Panel1.Controls.Add(this.split2);
            // 
            // split1.Panel2
            // 
            this.split1.Panel2.Controls.Add(this.TextMsg);
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // TextMsg
            // 
            this.TextMsg.AutoEllipsis = true;
            this.TextMsg.BackColor = System.Drawing.Color.White;
            this.TextMsg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.TextMsg, "TextMsg");
            this.TextMsg.Name = "TextMsg";
            // 
            // fMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelWO);
            this.Name = "fMain";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.Shown += new System.EventHandler(this.fMain_Shown);
            this.split2.Panel1.ResumeLayout(false);
            this.split2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split2)).EndInit();
            this.split2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.gbBox.ResumeLayout(false);
            this.gbBox.PerformLayout();
            this.gbCarton.ResumeLayout(false);
            this.gbCarton.PerformLayout();
            this.gbPallet.ResumeLayout(false);
            this.gbPallet.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.gbPkspec.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.gbSN.ResumeLayout(false);
            this.PanelSNInput.ResumeLayout(false);
            this.PanelSNInput.PerformLayout();
            this.panelWO.ResumeLayout(false);
            this.panelWO.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.split1.Panel1.ResumeLayout(false);
            this.split1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split1)).EndInit();
            this.split1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Panel panelWO;
        private System.Windows.Forms.TextBox editWO;
        private System.Windows.Forms.Button btnSearchWO;
        private System.Windows.Forms.Label LabPart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label LabPalletCapacity;
        private System.Windows.Forms.Label LabPalletQty;
        private System.Windows.Forms.Label LabPalletTle;
        private System.Windows.Forms.TextBox editPallet;
        private System.Windows.Forms.Label LabPallet;
        private System.Windows.Forms.Button btnClosePallet;
        private System.Windows.Forms.Button btnCloseCarton;
        private System.Windows.Forms.Label LabCartonCapacity;
        private System.Windows.Forms.Label LabCartonQty;
        private System.Windows.Forms.Label LabCartonTle;
        private System.Windows.Forms.TextBox editCarton;
        private System.Windows.Forms.Label LabCarton;
        private System.Windows.Forms.Button btnCloseBox;
        private System.Windows.Forms.Label LabBoxCapacity;
        private System.Windows.Forms.Label LabBoxQty;
        private System.Windows.Forms.Label LabBoxTle;
        private System.Windows.Forms.TextBox editBox;
        private System.Windows.Forms.Label lablBox;
        private System.Windows.Forms.TextBox editSN;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox editCSN;
        private System.Windows.Forms.Label labCustSN;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel LabChangSpec;
        private System.Windows.Forms.ListView LVPackSpec;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderBox;
        private System.Windows.Forms.ColumnHeader columnHeaderCarton;
        private System.Windows.Forms.ColumnHeader columnHeaderPallet;
        private System.Windows.Forms.ListView LVEC;
        private System.Windows.Forms.ColumnHeader columnHeaderDefect;
        private System.Windows.Forms.ColumnHeader columnHeaderDefDesc;
        private System.Windows.Forms.Button btnChangeWo;
        private System.Windows.Forms.Panel PanelSNInput;
        private System.Windows.Forms.Label LabWo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label LabPKBase;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListBox listField;
        private System.Windows.Forms.ListBox listValue;
        private System.Windows.Forms.ListBox ListParam;
        private System.Windows.Forms.ListBox ListData;
        private System.Windows.Forms.Label LabPKAction;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox gbPallet;
        private System.Windows.Forms.GroupBox gbCarton;
        private System.Windows.Forms.GroupBox gbBox;
        private System.Windows.Forms.GroupBox gbSN;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gbPkspec;
        private System.Windows.Forms.Label TextMsg;
        private System.Windows.Forms.SplitContainer split2;
        private System.Windows.Forms.Label LabPalletCap;
        private System.Windows.Forms.Label LabCartonCap;
        private System.Windows.Forms.Label LabBoxCap;
        private System.Windows.Forms.SplitContainer split1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ColumnHeader columnHeaderInner;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Label LabTargetQty;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label LabPartDesc;
        private System.Windows.Forms.Label LabWoVersion;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnChangeCarton;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Label lb_process;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnClearBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lb_Station;
    }
}
