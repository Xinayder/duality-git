using System;

using Duality;
using Duality.IO;
using Duality.Editor;
using Duality.Editor.Properties;
using Duality.Editor.Forms;

using AdamsLair.WinForms.ItemModels;
using Duality.Serialization;

namespace RockyTV.GitPlugin.Editor
{
	/// <summary>
	/// Defines a Duality editor plugin.
	/// </summary>
    public class DualityGitEditorPlugin : EditorPlugin
	{
		private bool isLoading = false;
		private string userDataPath = PathOp.Combine(Environment.CurrentDirectory, "GitSettings.dat");

		private PluginUserData userData = null;
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

			this.LoadGitData();

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

		private void LoadGitData()
		{
			this.isLoading = true;
			this.userData = Serializer.TryReadObject<PluginUserData>(this.userDataPath) ?? new PluginUserData();
			this.isLoading = false;
		}

		private void SaveGitData()
		{
			Serializer.WriteObject(this.userData, this.userDataPath, typeof(XmlSerializer));
		}
	}
}
