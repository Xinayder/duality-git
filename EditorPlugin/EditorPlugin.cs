using System;

using Duality;
using Duality.IO;
using Duality.Editor;
using Duality.Editor.Properties;
using Duality.Editor.Forms;

using AdamsLair.WinForms.ItemModels;
using Duality.Serialization;

using LibGit2Sharp;
using LibGit2Sharp.Core;
using System.Xml.Linq;

namespace RockyTV.GitPlugin.Editor
{
	/// <summary>
	/// Defines a Duality editor plugin.
	/// </summary>
    public class DualityGitEditorPlugin : EditorPlugin
	{
		private bool isLoading = false;

		private PluginUserData userData = new PluginUserData();
		public PluginUserData UserData
		{
			get { return userData; }
			set { userData = value ?? new PluginUserData(); }
		}

		public override string Id
		{
			get { return "RockyTV.GitPlugin"; }
		}

		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);

			// Auto retrieve git user info from global .gitconfig file
			if (userData.AutoFetchConfig && string.IsNullOrEmpty(userData.AuthorName) && string.IsNullOrEmpty(userData.AuthorEmail))
			{
				using (Configuration gitConfig = new Configuration(null, null, null))
				{
					userData.AuthorName = gitConfig.Get<string>("user.name").Value;
					userData.AuthorEmail = gitConfig.Get<string>("user.email").Value;
				}
			}

			// Request menu
			MenuModelItem viewItem = main.MainMenu.RequestItem(GeneralRes.MenuName_Settings);
			viewItem.AddItem(new MenuModelItem
			{
				Name = "Git User Data",
				ActionHandler = this.menuItemGitSettings_Click
			});
		}

		private void menuItemGitSettings_Click(object sender, EventArgs e)
		{
			DualityEditorApp.Select(this, new ObjectSelection(new[] { this.UserData }));
		}

		protected override void LoadUserData(XElement node)
		{
			isLoading = true;

			bool tryParseBool;
			if (node.GetElementValue("autoFetchConfig", out tryParseBool))
				userData.AutoFetchConfig = tryParseBool;

			XElement gitElem = node.Element("user");
			if (gitElem != null)
			{
				userData.AuthorName = gitElem.GetElementValue("name");
				userData.AuthorEmail = gitElem.GetElementValue("email");
			}

			isLoading = false;
		}

		protected override void SaveUserData(XElement node)
		{
			node.SetElementValue("autoFetchConfig", userData.AutoFetchConfig);

			XElement gitElem = new XElement("user");
			gitElem.SetElementValue("name", userData.AuthorName);
			gitElem.SetElementValue("email", userData.AuthorEmail);
			node.Add(gitElem);
		}
	}
}
