using System;
using System.Xml;
using System.Linq;
using System.Drawing;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Globalization;

using Duality;
using Duality.Editor;

using WeifenLuo.WinFormsUI.Docking;

namespace RockyTV.Duality.GitPlugin
{
    public partial class SettingsWindow : DockContent
    {
        public SettingsWindow()
        {
            this.InitializeComponent();
        }

        private string authorName = null;
        private string authorEmail = null;

        internal void SaveUserData(XElement node)
        {
            node.SetAttributeValue("authorName", this.authorName);
            node.SetAttributeValue("authorEmail", this.authorEmail);
        }

        internal void LoadUserData(XElement node)
        {
            this.boxAuthorName.Text = node.GetAttributeValue("authorName");
            this.boxAuthorEmail.Text = node.GetAttributeValue("authorEmail");

            this.authorName = this.boxAuthorName.Text;
            this.authorEmail = this.boxAuthorEmail.Text;

            GitPlugin.Instance.SetGitSettings(this.authorName, this.authorEmail);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.authorName = this.boxAuthorName.Text;
            this.authorEmail = this.boxAuthorEmail.Text;
            GitPlugin.Instance.SetGitSettings(this.authorName, this.authorEmail);

            this.Focus();
        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            this.toolTip.SetToolTip(this.labelAuthor, global::RockyTV.Duality.GitPlugin.Properties.GitPluginRes.ToolTip_AuthorName);
            this.toolTip.SetToolTip(this.boxAuthorName, global::RockyTV.Duality.GitPlugin.Properties.GitPluginRes.ToolTip_AuthorName);
            this.toolTip.SetToolTip(this.labelEmail, global::RockyTV.Duality.GitPlugin.Properties.GitPluginRes.ToolTip_AuthorEmail);
            this.toolTip.SetToolTip(this.boxAuthorEmail, global::RockyTV.Duality.GitPlugin.Properties.GitPluginRes.ToolTip_AuthorEmail);
        }
    }
}
