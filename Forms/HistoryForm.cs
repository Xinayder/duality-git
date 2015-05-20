using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using LibGit2Sharp;

using Duality;
namespace RockyTV.Duality.GitPlugin
{
    public partial class HistoryForm : DockContent
    {
        private IQueryableCommitLog commitLog = null;

        private string gitDateFormat = "ddd MMM d HH:mm:ss yyyy zzz"; // Wed May 20 17:47:39 2015 -03:00
        private DataGridViewRow gridSelectedRow = null;

        public HistoryForm()
        {
            InitializeComponent();

            this.commitLog = PluginGit.fetch.RequestHistory();
            this._UpdateList();
        }

        private void _UpdateList()
        {
            if (this.dataGridView.CurrentRow != null)
            {
                Log.Editor.Write("CurrentRow is not null, we are saving the current index.");
                gridSelectedRow = this.dataGridView.CurrentRow;
            }

            IEnumerable<DataGridViewRow> rows = PopulateLogList();
            this.dataGridView.Rows.Clear();
            this.dataGridView.Rows.AddRange(rows.ToArray());

            if (gridSelectedRow != null && this.dataGridView.Rows.Count > 0)
            {
                if (this.dataGridView.Rows.Contains(gridSelectedRow)) this.dataGridView.Rows[this.dataGridView.Rows.IndexOf(gridSelectedRow)].Selected = true;
            }
        }

        public IEnumerable<DataGridViewRow> PopulateLogList()
        {
            List<DataGridViewRow> output = new List<DataGridViewRow>();

            if (commitLog != null)
            {
                foreach (Commit commit in commitLog)
                {
                    DataGridViewRow item = new DataGridViewRow { Tag = commit };

                    DataGridViewTextBoxCell hashCell = new DataGridViewTextBoxCell { Value = commit.Sha };
                    DataGridViewTextBoxCell authorCell = new DataGridViewTextBoxCell { Value = string.Format("{0} <{1}>", commit.Author.Name, commit.Author.Email) };
                    DataGridViewTextBoxCell messageCell = new DataGridViewTextBoxCell { Value = commit.MessageShort };
                    DataGridViewTextBoxCell dateCell = new DataGridViewTextBoxCell { Value = commit.Author.When.ToString(gitDateFormat) };

                    item.Cells.AddRange(messageCell, authorCell, dateCell, hashCell);

                    output.Add(item);
                }
            }

            return output;
        }

        private void HistoryForm_Enter(object sender, EventArgs e)
        {
            this._UpdateList();
        }
    }
}
