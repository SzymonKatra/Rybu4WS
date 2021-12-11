
namespace Rybu4WS.UI
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.txtCode = new System.Windows.Forms.RichTextBox();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCursorPos = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutServers = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.flowLayoutAgents = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttLoadCode = new System.Windows.Forms.Button();
            this.buttSaveDedanModel = new System.Windows.Forms.Button();
            this.lblCodePath = new System.Windows.Forms.Label();
            this.buttReloadCode = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttVerify = new System.Windows.Forms.Button();
            this.buttLoadDedanTrail = new System.Windows.Forms.Button();
            this.lblDedanTrailPath = new System.Windows.Forms.Label();
            this.buttReloadDedanTrail = new System.Windows.Forms.Button();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttDebuggerStep = new System.Windows.Forms.Button();
            this.buttDebuggerReset = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.ctxMenuStripVerify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripItemDeadlock = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripItemTermination = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripItemPossibleTermination = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.ctxMenuStripVerify.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel5);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(947, 405);
            this.splitContainer1.SplitterDistance = 594;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.txtCode, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.flowLayoutPanel4, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(594, 405);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCode.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtCode.Location = new System.Drawing.Point(3, 3);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(588, 369);
            this.txtCode.TabIndex = 0;
            this.txtCode.Text = "Press \'Load\' button to load Rybu4WS code";
            this.txtCode.WordWrap = false;
            this.txtCode.SelectionChanged += new System.EventHandler(this.txtCode_SelectionChanged);
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Controls.Add(this.lblCursorPos);
            this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel4.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(3, 378);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(588, 24);
            this.flowLayoutPanel4.TabIndex = 1;
            // 
            // lblCursorPos
            // 
            this.lblCursorPos.AutoSize = true;
            this.lblCursorPos.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCursorPos.Location = new System.Drawing.Point(537, 0);
            this.lblCursorPos.Name = "lblCursorPos";
            this.lblCursorPos.Size = new System.Drawing.Size(48, 15);
            this.lblCursorPos.TabIndex = 0;
            this.lblCursorPos.Text = "L: 1 C: 1";
            this.lblCursorPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel3);
            this.splitContainer2.Size = new System.Drawing.Size(349, 405);
            this.splitContainer2.SplitterDistance = 174;
            this.splitContainer2.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutServers, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(349, 174);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightBlue;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(343, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "SERVERS";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutServers
            // 
            this.flowLayoutServers.AutoScroll = true;
            this.flowLayoutServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutServers.Location = new System.Drawing.Point(3, 28);
            this.flowLayoutServers.Name = "flowLayoutServers";
            this.flowLayoutServers.Size = new System.Drawing.Size(343, 143);
            this.flowLayoutServers.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutAgents, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(349, 227);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.LightBlue;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(343, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "AGENTS";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutAgents
            // 
            this.flowLayoutAgents.AutoScroll = true;
            this.flowLayoutAgents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutAgents.Location = new System.Drawing.Point(3, 28);
            this.flowLayoutAgents.Name = "flowLayoutAgents";
            this.flowLayoutAgents.Size = new System.Drawing.Size(343, 196);
            this.flowLayoutAgents.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(953, 516);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel2, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel3, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 414);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(947, 99);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 35);
            this.label3.TabIndex = 0;
            this.label3.Text = "Rybu4WS code";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.buttLoadCode);
            this.flowLayoutPanel1.Controls.Add(this.buttSaveDedanModel);
            this.flowLayoutPanel1.Controls.Add(this.lblCodePath);
            this.flowLayoutPanel1.Controls.Add(this.buttReloadCode);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(103, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(821, 29);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // buttLoadCode
            // 
            this.buttLoadCode.Location = new System.Drawing.Point(3, 3);
            this.buttLoadCode.Name = "buttLoadCode";
            this.buttLoadCode.Size = new System.Drawing.Size(75, 23);
            this.buttLoadCode.TabIndex = 0;
            this.buttLoadCode.Text = "Load";
            this.buttLoadCode.UseVisualStyleBackColor = true;
            this.buttLoadCode.Click += new System.EventHandler(this.buttLoadCode_Click);
            // 
            // buttSaveDedanModel
            // 
            this.buttSaveDedanModel.Enabled = false;
            this.buttSaveDedanModel.Location = new System.Drawing.Point(84, 3);
            this.buttSaveDedanModel.Name = "buttSaveDedanModel";
            this.buttSaveDedanModel.Size = new System.Drawing.Size(123, 23);
            this.buttSaveDedanModel.TabIndex = 1;
            this.buttSaveDedanModel.Text = "Save DedAn model";
            this.buttSaveDedanModel.UseVisualStyleBackColor = true;
            this.buttSaveDedanModel.Click += new System.EventHandler(this.buttSaveDedanModel_Click);
            // 
            // lblCodePath
            // 
            this.lblCodePath.AutoSize = true;
            this.lblCodePath.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCodePath.Location = new System.Drawing.Point(213, 0);
            this.lblCodePath.Name = "lblCodePath";
            this.lblCodePath.Size = new System.Drawing.Size(16, 29);
            this.lblCodePath.TabIndex = 2;
            this.lblCodePath.Text = "...";
            this.lblCodePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttReloadCode
            // 
            this.buttReloadCode.Enabled = false;
            this.buttReloadCode.Location = new System.Drawing.Point(235, 3);
            this.buttReloadCode.Name = "buttReloadCode";
            this.buttReloadCode.Size = new System.Drawing.Size(75, 23);
            this.buttReloadCode.TabIndex = 3;
            this.buttReloadCode.Text = "Reload";
            this.buttReloadCode.UseVisualStyleBackColor = true;
            this.buttReloadCode.Click += new System.EventHandler(this.buttReloadCode_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 35);
            this.label4.TabIndex = 2;
            this.label4.Text = "DedAn trail";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.buttVerify);
            this.flowLayoutPanel2.Controls.Add(this.buttLoadDedanTrail);
            this.flowLayoutPanel2.Controls.Add(this.lblDedanTrailPath);
            this.flowLayoutPanel2.Controls.Add(this.buttReloadDedanTrail);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(103, 38);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(821, 29);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // buttVerify
            // 
            this.buttVerify.Enabled = false;
            this.buttVerify.Location = new System.Drawing.Point(3, 3);
            this.buttVerify.Name = "buttVerify";
            this.buttVerify.Size = new System.Drawing.Size(101, 23);
            this.buttVerify.TabIndex = 5;
            this.buttVerify.Text = "Verify in DedAn";
            this.buttVerify.UseVisualStyleBackColor = true;
            this.buttVerify.Click += new System.EventHandler(this.buttVerify_Click);
            // 
            // buttLoadDedanTrail
            // 
            this.buttLoadDedanTrail.Enabled = false;
            this.buttLoadDedanTrail.Location = new System.Drawing.Point(110, 3);
            this.buttLoadDedanTrail.Name = "buttLoadDedanTrail";
            this.buttLoadDedanTrail.Size = new System.Drawing.Size(75, 23);
            this.buttLoadDedanTrail.TabIndex = 0;
            this.buttLoadDedanTrail.Text = "Load";
            this.buttLoadDedanTrail.UseVisualStyleBackColor = true;
            this.buttLoadDedanTrail.Click += new System.EventHandler(this.buttLoadDedanTrail_Click);
            // 
            // lblDedanTrailPath
            // 
            this.lblDedanTrailPath.AutoSize = true;
            this.lblDedanTrailPath.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDedanTrailPath.Location = new System.Drawing.Point(191, 0);
            this.lblDedanTrailPath.Name = "lblDedanTrailPath";
            this.lblDedanTrailPath.Size = new System.Drawing.Size(16, 29);
            this.lblDedanTrailPath.TabIndex = 1;
            this.lblDedanTrailPath.Text = "...";
            this.lblDedanTrailPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttReloadDedanTrail
            // 
            this.buttReloadDedanTrail.Enabled = false;
            this.buttReloadDedanTrail.Location = new System.Drawing.Point(213, 3);
            this.buttReloadDedanTrail.Name = "buttReloadDedanTrail";
            this.buttReloadDedanTrail.Size = new System.Drawing.Size(75, 23);
            this.buttReloadDedanTrail.TabIndex = 2;
            this.buttReloadDedanTrail.Text = "Reload";
            this.buttReloadDedanTrail.UseVisualStyleBackColor = true;
            this.buttReloadDedanTrail.Click += new System.EventHandler(this.buttReloadDedanTrail_Click);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.buttDebuggerStep);
            this.flowLayoutPanel3.Controls.Add(this.buttDebuggerReset);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(103, 73);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(821, 29);
            this.flowLayoutPanel3.TabIndex = 4;
            // 
            // buttDebuggerStep
            // 
            this.buttDebuggerStep.Enabled = false;
            this.buttDebuggerStep.Location = new System.Drawing.Point(3, 3);
            this.buttDebuggerStep.Name = "buttDebuggerStep";
            this.buttDebuggerStep.Size = new System.Drawing.Size(75, 23);
            this.buttDebuggerStep.TabIndex = 0;
            this.buttDebuggerStep.Text = "Step";
            this.buttDebuggerStep.UseVisualStyleBackColor = true;
            this.buttDebuggerStep.Click += new System.EventHandler(this.buttDebuggerStep_Click);
            // 
            // buttDebuggerReset
            // 
            this.buttDebuggerReset.Enabled = false;
            this.buttDebuggerReset.Location = new System.Drawing.Point(84, 3);
            this.buttDebuggerReset.Name = "buttDebuggerReset";
            this.buttDebuggerReset.Size = new System.Drawing.Size(75, 23);
            this.buttDebuggerReset.TabIndex = 1;
            this.buttDebuggerReset.Text = "Reset";
            this.buttDebuggerReset.UseVisualStyleBackColor = true;
            this.buttDebuggerReset.Click += new System.EventHandler(this.buttDebuggerReset_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 35);
            this.label5.TabIndex = 5;
            this.label5.Text = "Debugger";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ctxMenuStripVerify
            // 
            this.ctxMenuStripVerify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripItemDeadlock,
            this.toolStripItemTermination,
            this.toolStripItemPossibleTermination});
            this.ctxMenuStripVerify.Name = "ctxMenuStripVerify";
            this.ctxMenuStripVerify.Size = new System.Drawing.Size(183, 70);
            // 
            // toolStripItemDeadlock
            // 
            this.toolStripItemDeadlock.Name = "toolStripItemDeadlock";
            this.toolStripItemDeadlock.Size = new System.Drawing.Size(182, 22);
            this.toolStripItemDeadlock.Text = "Deadlock";
            this.toolStripItemDeadlock.Click += new System.EventHandler(this.toolStripItemDeadlock_Click);
            // 
            // toolStripItemTermination
            // 
            this.toolStripItemTermination.Name = "toolStripItemTermination";
            this.toolStripItemTermination.Size = new System.Drawing.Size(182, 22);
            this.toolStripItemTermination.Text = "Termination";
            this.toolStripItemTermination.Click += new System.EventHandler(this.toolStripItemTermination_Click);
            // 
            // toolStripItemPossibleTermination
            // 
            this.toolStripItemPossibleTermination.Name = "toolStripItemPossibleTermination";
            this.toolStripItemPossibleTermination.Size = new System.Drawing.Size(182, 22);
            this.toolStripItemPossibleTermination.Text = "Possible termination";
            this.toolStripItemPossibleTermination.Click += new System.EventHandler(this.toolStripItemPossibleTermination_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 516);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormMain";
            this.Text = "Rybu4WS Debugger";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.ctxMenuStripVerify.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox txtCode;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutServers;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutAgents;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button buttLoadCode;
        private System.Windows.Forms.Button buttSaveDedanModel;
        private System.Windows.Forms.Label lblCodePath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button buttLoadDedanTrail;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button buttDebuggerStep;
        private System.Windows.Forms.Button buttDebuggerReset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblDedanTrailPath;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Label lblCursorPos;
        private System.Windows.Forms.Button buttReloadCode;
        private System.Windows.Forms.Button buttReloadDedanTrail;
        private System.Windows.Forms.Button buttVerify;
        private System.Windows.Forms.ContextMenuStrip ctxMenuStripVerify;
        private System.Windows.Forms.ToolStripMenuItem toolStripItemDeadlock;
        private System.Windows.Forms.ToolStripMenuItem toolStripItemTermination;
        private System.Windows.Forms.ToolStripMenuItem toolStripItemPossibleTermination;
    }
}

