namespace RockyTV.GitPlugin.Editor.Forms
{
	partial class CommitDialog
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
			this.fileTreeView = new System.Windows.Forms.TreeView();
			this.textBoxMessage = new System.Windows.Forms.TextBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonCommit = new System.Windows.Forms.Button();
			this.labelDesc = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// fileTreeView
			// 
			this.fileTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.fileTreeView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.fileTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.fileTreeView.CheckBoxes = true;
			this.fileTreeView.FullRowSelect = true;
			this.fileTreeView.HideSelection = false;
			this.fileTreeView.Indent = 15;
			this.fileTreeView.Location = new System.Drawing.Point(15, 25);
			this.fileTreeView.Name = "fileTreeView";
			this.fileTreeView.Size = new System.Drawing.Size(386, 257);
			this.fileTreeView.TabIndex = 0;
			this.fileTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.stagedFileList_AfterCheck);
			// 
			// textBoxMessage
			// 
			this.textBoxMessage.AcceptsReturn = true;
			this.textBoxMessage.AcceptsTab = true;
			this.textBoxMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.textBoxMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxMessage.ForeColor = System.Drawing.SystemColors.ControlText;
			this.textBoxMessage.Location = new System.Drawing.Point(15, 290);
			this.textBoxMessage.Multiline = true;
			this.textBoxMessage.Name = "textBoxMessage";
			this.textBoxMessage.Size = new System.Drawing.Size(386, 67);
			this.textBoxMessage.TabIndex = 1;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(15, 362);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonCommit
			// 
			this.buttonCommit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCommit.Location = new System.Drawing.Point(326, 362);
			this.buttonCommit.Name = "buttonCommit";
			this.buttonCommit.Size = new System.Drawing.Size(75, 23);
			this.buttonCommit.TabIndex = 3;
			this.buttonCommit.Text = "Commit";
			this.buttonCommit.UseVisualStyleBackColor = true;
			this.buttonCommit.Click += new System.EventHandler(this.buttonCommit_Click);
			// 
			// labelDesc
			// 
			this.labelDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelDesc.Location = new System.Drawing.Point(12, 0);
			this.labelDesc.Name = "labelDesc";
			this.labelDesc.Size = new System.Drawing.Size(235, 20);
			this.labelDesc.TabIndex = 4;
			this.labelDesc.Text = "Choose which files to stage for commit";
			this.labelDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// CommitDialog
			// 
			this.AcceptButton = this.buttonCommit;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(414, 391);
			this.Controls.Add(this.labelDesc);
			this.Controls.Add(this.buttonCommit);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.textBoxMessage);
			this.Controls.Add(this.fileTreeView);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(430, 430);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(430, 430);
			this.Name = "CommitDialog";
			this.ShowIcon = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Select files to stage for commit";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TreeView fileTreeView;
		private System.Windows.Forms.TextBox textBoxMessage;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonCommit;
		private System.Windows.Forms.Label labelDesc;
	}
}