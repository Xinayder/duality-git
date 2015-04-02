using System;
using System.Drawing;
using System.Windows.Forms;

using Duality.Editor;
using RockyTV.Duality.GitPlugin.Properties;

using WeifenLuo.WinFormsUI.Docking;
using System.Xml.Linq;
using System.Globalization;

namespace RockyTV.Duality.GitPlugin
{
    public partial class GitSettings : DockContent
    {
        public GitSettings()
        {
            this.InitializeComponent();
        }

        private string tempGitName = null;
        private string tempGitEmail = null;
        private string tempCustomCommitMsg = null;

        public string GitName
        {
            get { return tempGitName; }
        }

        internal void SaveUserData(XElement node)
        {
            node.SetAttributeValue("customCommitMsg", tempCustomCommitMsg);
            node.SetAttributeValue("gitName", tempGitName);
            node.SetAttributeValue("gitEmail", tempGitEmail);
        }

        internal void LoadUserData(XElement node)
        {
            bool tryParseBool;

            if (bool.TryParse((string)node.Attribute("customCommitMsg"), out tryParseBool))
                this.customCommitMsg.Checked = tryParseBool;

            this.tbName.Text = (string)node.Attribute("gitName");
            this.tbEmail.Text = (string)node.Attribute("gitEmail");
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.tempGitName = this.tbName.Text;
            this.tempGitEmail = this.tbEmail.Text;
            this.tempCustomCommitMsg = this.customCommitMsg.Checked.ToString(CultureInfo.InvariantCulture);
            GitPlugin.Instance.SetGitSettings(this.tempGitName, this.tempGitEmail);
        }
    }
}
