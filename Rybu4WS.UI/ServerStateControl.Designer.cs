
namespace Rybu4WS.UI
{
    partial class ServerStateControl
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
            this.lblServerName = new System.Windows.Forms.Label();
            this.listVariables = new System.Windows.Forms.ListView();
            this.colVariable = new System.Windows.Forms.ColumnHeader();
            this.colValue = new System.Windows.Forms.ColumnHeader();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblServerName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.listVariables, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(160, 150);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblServerName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblServerName.Location = new System.Drawing.Point(3, 0);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(154, 20);
            this.lblServerName.TabIndex = 0;
            this.lblServerName.Text = "server_name";
            this.lblServerName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listVariables
            // 
            this.listVariables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colVariable,
            this.colValue});
            this.listVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listVariables.HideSelection = false;
            this.listVariables.Location = new System.Drawing.Point(3, 23);
            this.listVariables.Name = "listVariables";
            this.listVariables.Size = new System.Drawing.Size(154, 124);
            this.listVariables.TabIndex = 1;
            this.listVariables.UseCompatibleStateImageBehavior = false;
            this.listVariables.View = System.Windows.Forms.View.Details;
            // 
            // colVariable
            // 
            this.colVariable.Text = "Variable";
            this.colVariable.Width = 75;
            // 
            // colValue
            // 
            this.colValue.Text = "Value";
            this.colValue.Width = 75;
            // 
            // ServerStateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ServerStateControl";
            this.Size = new System.Drawing.Size(160, 150);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.ListView listVariables;
        private System.Windows.Forms.ColumnHeader colVariable;
        private System.Windows.Forms.ColumnHeader colValue;
    }
}
