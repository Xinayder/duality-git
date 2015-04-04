using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

using Duality;
using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Properties;

using LibGit2Sharp;
using LibGit2Sharp.Core;

using WeifenLuo.WinFormsUI.Docking;
using AdamsLair.WinForms.ItemModels;

using RockyTV.Duality.GitPlugin.Properties;

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
        private SettingsWindow gitSettings = null;

        private string gameDirectory = null;

        private string authorName = null;
        private string authorEmail = null;

        private bool isRepoInit = false;

        public override string Id
        {
            get { return "RockyTV.GitPlugin"; }
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
            if (dockContentType == typeof(SettingsWindow))
                result = RequestGitSettings();
            else
                result = base.DeserializeDockContent(dockContentType);
            this.isLoading = false;
            return result;

        }
        protected override void InitPlugin(MainForm main)
        {
            base.InitPlugin(main);

            // Try to init a git repository on the current working path.
            // Throws if the directory wasn't found.
            try
            {
                Repository.Init(this.gameDirectory); // Re/initialize a Git repository on the current working directory
                Write("Initialized Git repo on '{0}'.", this.gameDirectory);
                this.isRepoInit = true;
            }
            catch (Exception e)
            {
                WriteError("Failed to initialize Git repository on '{0}': {1}", this.gameDirectory, e.Message);
                this.isRepoInit = false;
            }

            // Generate our .gitignore file
            this.CreateGitIgnore();

            MenuModelItem viewItem = main.MainMenu.RequestItem(GeneralRes.MenuName_Settings);
            viewItem.AddItem(new MenuModelItem
            {
                Name = GitPluginRes.MenuItemName_Git,
                Icon = GitPluginResCache.IconGit,
                ActionHandler = this.menuItemGitSettings_Click
            });
        }
        protected override void LoadUserData(XElement node)
        {
            this.isLoading = true; // Tell Duality that our plugin is loading
            if (this.gitSettings != null) // Null check against our settings window
            {
                XElement gitElem = node.Element("GitPlugin_0");
                if (gitElem != null) // If our settings window is not null
                {
                    this.gitSettings.LoadUserData(gitElem); // Call its load method
                }
            }
            this.isLoading = false; // Tell Duality that our plugin has finished loading
        }
        protected override void SaveUserData(XElement node)
        {
            if (this.isRepoInit) // Check if the repository has been initialized successfully
            {
                using (Repository repo = new Repository(gameDirectory))
                {
                    StringBuilder sb = new StringBuilder();

                    // Loop through each file of each directory under our repository
                    foreach (string file in Directory.GetFiles(this.gameDirectory, "*", SearchOption.AllDirectories))
                    {
                        // Get the status for our file
                        FileStatus status = repo.RetrieveStatus(file);

                        FileStatus[] ignoreFileStatus = new FileStatus[] { 
                            FileStatus.Ignored, FileStatus.Missing, FileStatus.Nonexistent, FileStatus.Unaltered, 
                            FileStatus.Unreadable
                        };

                        // Check if the current file status is not one of the ignored file status
                        foreach (FileStatus ignoreStatus in ignoreFileStatus)
                        {
                            if (status != ignoreStatus) repo.Stage(file);
                        }

                        // Write to our commit message what has been altered
                        switch (status)
                        {
                            case FileStatus.Added:
                                sb.AppendLine(string.Format("Added file '{0}'", file));
                                break;
                            case FileStatus.Removed:
                                sb.AppendLine(string.Format("Removed file '{0}'", file));
                                break;
                            case FileStatus.RenamedInIndex:
                                sb.AppendLine(string.Format("Renamed file '{0}' in index", file));
                                break;
                            case FileStatus.StagedTypeChange:
                                sb.AppendLine(string.Format("Staged type change for file '{0}'", file));
                                break;
                            case FileStatus.Modified:
                                sb.AppendLine(string.Format("Modified file '{0}'", file));
                                break;
                            case FileStatus.TypeChanged:
                                sb.AppendLine(string.Format("Change type for file '{0}'", file));
                                break;
                            case FileStatus.RenamedInWorkDir:
                                sb.AppendLine(string.Format("Renamed file '{0}' in work dir", file));
                                break;
                        }
                    }
                    sb.AppendLine();

                    // Setup the commit author
                    Signature author = null;
                    if (this.authorName != null && this.authorEmail != null)
                    {
                        author = new Signature(this.authorName, this.authorEmail, DateTime.Now);
                    }
                    else
                    {
                        author = new Signature("John Doe", "john.doe@example.com", DateTime.Now);
                    }

                    // Try to commit. If it throws, we log it.
                    try
                    {
                        Write("Committing changes...");
                        Commit commit = repo.Commit(string.Join(@"\r\n", sb.ToString()), author);
                    }
                    catch (EmptyCommitException)
                    {
                        WriteWarning("Nothing changed. Skipping commit.");
                    }
                    catch (Exception e)
                    {
                        WriteError("Error while committing: {0}", e.Message);
                    }
                }

                // Finally, we save our data.
                if (this.gitSettings != null)
                {
                    XElement gitElem = new XElement("GitPlugin_0");
                    node.Add(gitElem);
                    this.gitSettings.SaveUserData(gitElem);
                }
            }
        }
        // This method here requests a new SettingsWindow.
        public SettingsWindow RequestGitSettings()
        {
            if (this.gitSettings == null || this.gitSettings.IsDisposed)
            {
                this.gitSettings = new SettingsWindow();
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
        /// <summary>
        /// Pass our <see cref="SettingsWindow.cs"/> settings to <see cref="GitPlugin.cs"/>.
        /// Defines the author name and email of the commits.
        /// </summary>
        /// <param name="name">The name of the author you want to show on commits.</param>
        /// <param name="email">The email of the author you want to show on commits.</param>
        public void SetGitSettings(string name, string email)
        {
            this.authorEmail = email;
            this.authorName = name;
        }

        private void menuItemGitSettings_Click(object sender, EventArgs e)
        {
            this.RequestGitSettings();
        }

        #region Log
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
        #endregion

        #region GetGitIgnore()
        /// <summary>
        /// Generate a formatted .gitignore file.
        /// </summary>
        /// <returns>A formatted .gitignore file.</returns>
        private string GenerateGitIgnore()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("# .gitignore generated on {0}", DateTime.Now.ToString()));
            sb.AppendLine("# ENCODING: UTF-8");
            sb.AppendLine();
            sb.AppendLine("# Directories");
            sb.AppendLine("#");
            sb.AppendLine(".git");
            sb.AppendLine("Backup");
            sb.AppendLine("Source/Code/**/bin");
            sb.AppendLine("Source/Code/**/obj");
            sb.AppendLine("Source/Packages");
            sb.AppendLine();
            sb.AppendLine("# Files");
            sb.AppendLine("#");
            sb.AppendLine("*.csproj.user");
            sb.AppendLine("*.suo");
            sb.AppendLine("AppData.dat");
            sb.AppendLine("EditorUserData.xml");
            sb.AppendLine("logfile.txt");
            sb.AppendLine("logfile_editor.txt");
            sb.AppendLine("perflog.txt");
            sb.AppendLine("perflog_editor.txt");
            sb.AppendLine();

            return sb.ToString();
        }
        #endregion

        private void CreateGitIgnore()
        {
            string gitIgnorePath = Path.Combine(this.gameDirectory, ".gitignore");
            this.CreateGitIgnore(gitIgnorePath);
        }
        /// <summary>
        /// Create a brand new .gitignore file in the specified path.
        /// </summary>
        /// <param name="file">The file path where you want the file to be created in.</param>
        private void CreateGitIgnore(string file)
        {
            try
            {
                if (!File.Exists(file)) // If the file does not exist, create a new one
                {
                    File.WriteAllText(file, this.GenerateGitIgnore(), Encoding.UTF8);
                    Write("Created .gitignore file.");
                }
                else
                    Write(".gitignore does exist. Skipping creation of a new one.");
            }
            catch (Exception e)
            {
                WriteError("Failed to create .gitignore file: {0}", e.Message);
            }
        }
    }
}
