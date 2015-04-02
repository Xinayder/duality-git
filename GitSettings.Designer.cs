namespace RockyTV.Duality.GitPlugin
{
    partial class GitSettings
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
            this.tbName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.customCommitMsg = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(53, 12);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(169, 20);
            this.tbName.TabIndex = 0;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(12, 15);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "Name";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(12, 41);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(32, 13);
            this.labelEmail.TabIndex = 3;
            this.labelEmail.Text = "Email";
            // 
            // tbEmail
            // 
            this.tbEmail.Location = new System.Drawing.Point(53, 38);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(169, 20);
            this.tbEmail.TabIndex = 2;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(12, 82);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(210, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = global::RockyTV.Duality.GitPlugin.Properties.GitPluginRes.Button_GitSave;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // customCommitMsg
            // 
            this.customCommitMsg.AutoSize = true;
            this.customCommitMsg.Location = new System.Drawing.Point(15, 64);
            this.customCommitMsg.Name = "customCommitMsg";
            this.customCommitMsg.Size = new System.Drawing.Size(168, 17);
            this.customCommitMsg.TabIndex = 5;
            this.customCommitMsg.Text = global::RockyTV.Duality.GitPlugin.Properties.GitPluginRes.Checkbox_CustomMessages;
            this.customCommitMsg.UseVisualStyleBackColor = true;
            // 
            // GitSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
            this.ClientSize = new System.Drawing.Size(235, 110);
            this.Controls.Add(this.customCommitMsg);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.tbName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = global::RockyTV.Duality.GitPlugin.Properties.GitPluginRes.IconGitSettings;
            this.Name = "GitSettings";
            this.Text = "Git Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.CheckBox customCommitMsg;
    }
}