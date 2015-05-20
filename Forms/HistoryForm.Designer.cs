namespace RockyTV.Duality.GitPlugin
{
    partial class HistoryForm
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
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.clmnShortMsg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmnAuthor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmnHash = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeColumns = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView.ColumnHeadersHeight = 24;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmnShortMsg,
            this.clmnAuthor,
            this.clmnDate,
            this.clmnHash});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(700, 250);
            this.dataGridView.TabIndex = 0;
            // 
            // clmnShortMsg
            // 
            this.clmnShortMsg.Frozen = true;
            this.clmnShortMsg.HeaderText = "Description";
            this.clmnShortMsg.MaxInputLength = 128;
            this.clmnShortMsg.MinimumWidth = 75;
            this.clmnShortMsg.Name = "clmnShortMsg";
            this.clmnShortMsg.ReadOnly = true;
            this.clmnShortMsg.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.clmnShortMsg.Width = 85;
            // 
            // clmnAuthor
            // 
            this.clmnAuthor.Frozen = true;
            this.clmnAuthor.HeaderText = "Author";
            this.clmnAuthor.MaxInputLength = 64;
            this.clmnAuthor.MinimumWidth = 50;
            this.clmnAuthor.Name = "clmnAuthor";
            this.clmnAuthor.ReadOnly = true;
            this.clmnAuthor.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.clmnAuthor.Width = 63;
            // 
            // clmnDate
            // 
            this.clmnDate.Frozen = true;
            this.clmnDate.HeaderText = "Date";
            this.clmnDate.MaxInputLength = 30;
            this.clmnDate.MinimumWidth = 45;
            this.clmnDate.Name = "clmnDate";
            this.clmnDate.ReadOnly = true;
            this.clmnDate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.clmnDate.Width = 55;
            // 
            // clmnHash
            // 
            this.clmnHash.FillWeight = 128F;
            this.clmnHash.Frozen = true;
            this.clmnHash.HeaderText = "Commit";
            this.clmnHash.MaxInputLength = 40;
            this.clmnHash.MinimumWidth = 50;
            this.clmnHash.Name = "clmnHash";
            this.clmnHash.ReadOnly = true;
            this.clmnHash.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.clmnHash.Width = 66;
            // 
            // HistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(700, 250);
            this.Controls.Add(this.dataGridView);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HistoryForm";
            this.Text = "Commit Log";
            this.Enter += new System.EventHandler(this.HistoryForm_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmnShortMsg;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmnAuthor;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmnHash;
    }
}