
namespace Rybu4WS.UI
{
    partial class AgentStateControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblAgentName = new System.Windows.Forms.Label();
            this.listTrace = new System.Windows.Forms.ListView();
            this.colLine = new System.Windows.Forms.ColumnHeader();
            this.colColumn = new System.Windows.Forms.ColumnHeader();
            this.colState = new System.Windows.Forms.ColumnHeader();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblAgentName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.listTrace, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(150, 150);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblAgentName
            // 
            this.lblAgentName.AutoSize = true;
            this.lblAgentName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAgentName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblAgentName.Location = new System.Drawing.Point(3, 0);
            this.lblAgentName.Name = "lblAgentName";
            this.lblAgentName.Size = new System.Drawing.Size(144, 20);
            this.lblAgentName.TabIndex = 0;
            this.lblAgentName.Text = "agent_name";
            this.lblAgentName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listTrace
            // 
            this.listTrace.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colLine,
            this.colColumn,
            this.colState});
            this.listTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTrace.FullRowSelect = true;
            this.listTrace.HideSelection = false;
            this.listTrace.Location = new System.Drawing.Point(3, 23);
            this.listTrace.MultiSelect = false;
            this.listTrace.Name = "listTrace";
            this.listTrace.Size = new System.Drawing.Size(144, 124);
            this.listTrace.TabIndex = 1;
            this.listTrace.UseCompatibleStateImageBehavior = false;
            this.listTrace.View = System.Windows.Forms.View.Details;
            this.listTrace.DoubleClick += new System.EventHandler(this.listTrace_DoubleClick);
            // 
            // colLine
            // 
            this.colLine.Text = "L";
            this.colLine.Width = 30;
            // 
            // colColumn
            // 
            this.colColumn.Text = "C";
            this.colColumn.Width = 30;
            // 
            // colState
            // 
            this.colState.Text = "State";
            this.colState.Width = 80;
            // 
            // AgentStateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AgentStateControl";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblAgentName;
        private System.Windows.Forms.ListView listTrace;
        private System.Windows.Forms.ColumnHeader colLine;
        private System.Windows.Forms.ColumnHeader colColumn;
        private System.Windows.Forms.ColumnHeader colState;
    }
}
