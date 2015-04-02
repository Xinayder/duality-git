using System;
using System.Windows.Forms;

using Duality;
using Duality.Editor;
using Duality.Editor.Forms;

using WeifenLuo.WinFormsUI.Docking;

using LibGit2Sharp;
using System.IO;
using System.Collections.Generic;
using AdamsLair.WinForms.ItemModels;
using Duality.Editor.Properties;
using RockyTV.Duality.GitPlugin.Properties;
using System.Xml.Linq;

namespace RockyTV.Duality.GitPlugin
{
    public class GitPlugin : EditorPlugin
    {
        private static GitPlugin instance = null;
        internal static GitPlugin Instance
        {
            get { return instance; }
        }

        private bool isLoading = false;
        private GitSettings gitSettings = null;

        private string gameDirectory = null;

        private string authorName = null;
        private string authorEmail = null;

        private bool isRepoInit = false;

        public override string Id
        {
            get { return "GitPlugin"; }
        }

        public GitPlugin()
        {
            instance = this;
            gameDirectory = Environment.CurrentDirectory;
        }
        protected override IDockContent DeserializeDockContent(Type dockContentType)
        {
            this.isLoading = true;
            IDockContent result;
            if (dockContentType == typeof(GitSettings))
                result = RequestGitSettings();
            else
                result = base.DeserializeDockContent(dockContentType);
            this.isLoading = false;
            return result;

        }
        protected override void InitPlugin(MainForm main)
        {
            base.InitPlugin(main);

            Write("Initializing git repo on {0}", this.gameDirectory);
            Repository.Init(this.gameDirectory);
            this.isRepoInit = true;

            MenuModelItem viewItem = main.MainMenu.RequestItem(GeneralRes.MenuName_Settings);
            viewItem.AddItem(new MenuModelItem
            {
                Name = GitPluginRes.MenuItemName_Git,
                Icon = GitPluginResCache.IconGit,
                ActionHandler = this.menuItemGitSettings_Click
            });
        }
        protected override void LoadUserData(System.Xml.Linq.XElement node)
        {
            this.isLoading = true;
            if (this.gitSettings != null)
            {
                XElement gitElem = node.Element("GitPlugin_0");
                if (gitElem != null)
                {
                    this.gitSettings.LoadUserData(gitElem);
                }
            }
            this.isLoading = false;
        }
        protected override void SaveUserData(System.Xml.Linq.XElement node)
        {
            if (this.isRepoInit)
            {
                using (var repo = new Repository(gameDirectory))
                {
                    foreach (string file in Directory.GetFiles(gameDirectory, "*", SearchOption.AllDirectories))
                    {
                        try
                        {
                            string currPath = Path.GetDirectoryName(file);
                            string currFile = Path.GetFileName(file);

                            if (currPath.Contains(".git")) continue;
                            if (currFile == "EditorUserData.xml") continue;
                            if (currFile == "perflog_editor.txt") continue;
                            if (currFile == "logfile_editor.txt") continue;
                            if (currFile == "logfile.txt") continue;
                            if (currFile == "perflog.txt") continue;
                            if (currFile == "AppData.dat") continue;

                            Write("Staging file '{0}' for commit", currFile);
                            repo.Stage(file);
                        }
                        catch (Exception e) // probably caused because the process is using our file
                        {
                            WriteError("Couldn't stage file '{0}' for commit: {1}", file, e.Message);
                        }
                    }

                    Signature author = null;
                    if (this.authorName != null && this.authorEmail != null)
                    {
                        author = new Signature(this.authorName, this.authorEmail, DateTime.Now);
                    }
                    else
                    {
                        author = new Signature("John Doe", "example@example.com", DateTime.Now);
                    }

                    Commit commit = repo.Commit("Editor reload. Saved repository.", author);

                    Write("Committed changes.");
                }

                if (this.gitSettings != null)
                {
                    XElement gitElem = new XElement("GitPlugin_0");
                    node.Add(gitElem);
                    this.gitSettings.SaveUserData(gitElem);
                }
            }
        }
        public GitSettings RequestGitSettings()
        {
            if (this.gitSettings == null || this.gitSettings.IsDisposed)
            {
                this.gitSettings = new GitSettings();
                this.gitSettings.FormClosed += delegate(object sender, FormClosedEventArgs e) { this.gitSettings = null; };
            }

            if (!this.isLoading)
            {
                this.gitSettings.Show(DualityEditorApp.MainForm.MainDockPanel);
                if (this.gitSettings.Pane != null)
                {
                    this.gitSettings.Pane.Activate();
                    this.gitSettings.Focus();
                }
            }

            return this.gitSettings;
        }
        public void SetGitSettings(string name, string email)
        {
            this.authorEmail = email;
            this.authorName = name;
        }

        private void menuItemGitSettings_Click(object sender, EventArgs e)
        {
            this.RequestGitSettings();
        }

        private static void Write(string msg, params object[] obj)
        {
            string message = string.Format("[GitPlugin] {0}", msg);
            Log.Editor.Write(message, obj);
        }
        private static void WriteWarning(string msg, params object[] obj)
        {
            string message = string.Format("[GitPlugin] {0}", msg);
            Log.Editor.WriteWarning(message, obj);
        }
        private static void WriteError(string msg, params object[] obj)
        {
            string message = string.Format("[GitPlugin] {0}", msg);
            Log.Editor.WriteError(message, obj);
        }
    }
}
