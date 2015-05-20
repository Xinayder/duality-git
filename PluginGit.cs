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
using System.Linq;

using WeifenLuo.WinFormsUI.Docking;
using AdamsLair.WinForms.ItemModels;

using RockyTV.Duality.GitPlugin.Properties;
using Duality.Serialization;

namespace RockyTV.Duality.GitPlugin
{
    public class PluginGit : EditorPlugin
    {
        private static PluginGit singleton;
        public static PluginGit fetch 
        {
            get { return singleton; }
        }

        private bool isFirstTime = true;

        private DateTime lastCommitTime = DateTime.Now;

        private bool isLoading = false;
        private HistoryForm formCommitLog = null;

        private Repository gitRepo = null;

        private string gameDirectory = null;

        private string userDataPath = null;

        private GitUserData userData = null;
        public GitUserData UserData
        {
            get { return this.userData; }
            set { this.userData = value ?? new GitUserData(); }
        }

        private bool isRepoInit = false;

        public override string Id
        {
            get { return "RockyTV.GitPlugin"; }
        }

        public PluginGit()
        {
            singleton = this;
            gameDirectory = Environment.CurrentDirectory;
        }
        protected override IDockContent DeserializeDockContent(Type dockContentType)
        {
            this.isLoading = true;
            IDockContent result;
            if (dockContentType == typeof(HistoryForm))
                result = RequestLogForm();
            else
                result = base.DeserializeDockContent(dockContentType);
            this.isLoading = false;
            return result;
        }
        protected override void InitPlugin(MainForm main)
        {
            base.InitPlugin(main);

            this.userDataPath = Path.Combine(gameDirectory, "GitSettings.dat");

            // Try to init a git repository on the current working path.
            // Throws if the directory wasn't found.
            try
            {
                this.isFirstTime = !Repository.IsValid(this.gameDirectory);
                this.gitRepo = new Repository(Repository.Init(this.gameDirectory)); // Re/initialize a Git repository on the current working directory
                Logger.Debug("Initialized Git repo on '{0}'.", this.gameDirectory);
                this.isRepoInit = true;
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to initialize Git repository on '{0}': {1}", this.gameDirectory, e.Message);
                this.isRepoInit = false;
            }

            // Subscribe to events.
            this.SubscribeToEvents();

            // Load our user data.
            this.LoadGitData();

            // Generate our .gitignore file
            this.CreateGitIgnore();

            MenuModelItem viewItem = main.MainMenu.RequestItem(GeneralRes.MenuName_Settings);
            viewItem.AddItem(new MenuModelItem
            {
                Name = GitPluginRes.MenuItemName_Git,
                Icon = GitPluginResCache.IconGit,
                ActionHandler = this.menuItemGitSettings_Click
            });

            MenuModelItem gitItem = main.MainMenu.RequestItem("Git");
            gitItem.AddItem(new MenuModelItem
            {
                Name = "Commit Log",
                Icon = null,
                ActionHandler = this.menuItemGitLog_Click
            });

            if (this.isRepoInit)
            {
                if (this.isFirstTime)
                {
                    gitRepo.Stage("*");

                    Signature commitAuthor = new Signature("Duality Git Plugin", "rockytv@gmail.com", DateTime.Now);
                    gitRepo.Commit("Initial commit", commitAuthor);
                }
            }
        }
        public HistoryForm RequestLogForm()
        {
            if (this.formCommitLog == null || this.formCommitLog.IsDisposed)
            {
                this.formCommitLog = new HistoryForm();
                this.formCommitLog.FormClosed += delegate(object sender, FormClosedEventArgs e) { this.formCommitLog = null; };
            }

            if (!this.isLoading)
            {
                this.formCommitLog.Show(DualityEditorApp.MainForm.MainDockPanel);
                if (this.formCommitLog.Pane != null)
                {
                    this.formCommitLog.Pane.Activate();
                    this.formCommitLog.Focus();
                }
            }

            return this.formCommitLog;
        }
        private void menuItemGitSettings_Click(object sender, EventArgs e)
        {
            //this.RequestGitSettings();
            DualityEditorApp.Select(this, new ObjectSelection(new[] { this.UserData }));
        }
        private void menuItemGitLog_Click(object sender, EventArgs args)
        {
            this.RequestLogForm();
        }
        protected override void SaveUserData(XElement node)
        {
            if (this.userData.CommitSettings == CommitTrigger.EditorReload)
            {
                this.SaveGitData();
            }
        }
        private void SubscribeToEvents() // Subscribe to events we need.
        {
            DualityEditorApp.EditorIdling += Plugin_EditorIdling; // Called when the editor is idling.
            DualityEditorApp.SaveAllTriggered += Plugin_SaveAllTriggered; // Called when save all is triggered.
            DualityEditorApp.Terminating += Plugin_Terminating; // Called when the editor is being terminated.
        }
        private void LoadGitData() // Load our git user data into a local variable.
        {
            this.isLoading = true;
            this.UserData = Formatter.TryReadObject<GitUserData>(this.userDataPath) ?? new GitUserData();
            this.isLoading = false;
        }
        private void SaveGitData() // Save our git user data into a file.
        {
            Formatter.WriteObject(this.userData, this.userDataPath, FormattingMethod.Xml);
            if (this.userData.CommitSettings == CommitTrigger.EditorSaveAll) CommitChanges();
        }
        // Commit the changes made in the working directory.
        private void CommitChanges()
        {
            if (this.isRepoInit) // Check if the repository has been initialized successfully
            {

                // Stage all files
                gitRepo.Stage("*");

                // Setup the commit author
                Signature author = new Signature(this.userData.AuthorName, this.userData.AuthorEmail, DateTime.Now);
                Commit currCommit = null;
                try
                {
                    currCommit = gitRepo.Commit("Temporary commit message", author);
                }
                catch (EmptyCommitException e) { } // Commit is null, do nothing instead

                if (currCommit != null)
                {
                    // Write our commit message
                    StringBuilder sb = new StringBuilder();

                    Tree commitTree = gitRepo.Head.Tip.Tree;
                    Tree parentCommitTree = gitRepo.Head.Tip.Parents.Single().Tree;

                    TreeChanges changes = gitRepo.Diff.Compare<TreeChanges>(parentCommitTree, commitTree);
                    if (changes.Count() > 0)
                    {
                        string pluralFile = "file";
                        string pluralInsertion = "insertion";
                        string pluralDeletion = "deletion";
                        if (changes.Count() != 1) pluralFile = "files";
                        if (changes.Added.Count() != 1) pluralInsertion = "insertions";
                        if (changes.Deleted.Count() != 1) pluralDeletion = "deletions";

                        sb.AppendLine(string.Format("{0} {1} changed, {2} {3}(+), {4} {5}(-)",
                            changes.Count(), pluralFile, changes.Added.Count(), pluralInsertion, changes.Deleted.Count(), pluralDeletion));

                        CommitOptions commitOptions = new CommitOptions()
                        {
                            AmendPreviousCommit = true,
                            AllowEmptyCommit = false,
                            PrettifyMessage = true,
                            CommentaryChar = '#'
                        };

                        // Try to commit. If it throws, we log it.
                        try
                        {
                            Commit ammendedCommit = gitRepo.Commit(sb.ToString(), author, commitOptions);
                            Logger.Debug("Committed changes, id: " + ammendedCommit.Sha);
                        }
                        catch (EmptyCommitException)
                        {
                            Logger.Debug("Nothing changed. Skipping commit.");
                        }
                        catch (Exception e)
                        {
                            Logger.LogError("Error while committing: {0}", e.Message);
                        }
                    }

                }
            }
        }
        private void Plugin_EditorIdling(object sender, EventArgs e)
        {
            if (this.userData.CommitSettings == CommitTrigger.Automatically)
            {
                TimeSpan timeSinceLastCommit = DateTime.Now - this.lastCommitTime;
                CommitFrequency frequency = this.userData.CommitFrequency;
                if (frequency == CommitFrequency.FiveMinutes && timeSinceLastCommit.TotalMinutes > 5 ||
                    frequency == CommitFrequency.FifteenMinutes && timeSinceLastCommit.TotalMinutes > 15 ||
                    frequency == CommitFrequency.ThirtyMinutes && timeSinceLastCommit.TotalMinutes > 30 ||
                    frequency == CommitFrequency.OneHour && timeSinceLastCommit.TotalMinutes > 60)
                {
                    this.CommitChanges();
                    this.lastCommitTime = DateTime.Now;
                }
            }
        }
        private void Plugin_Terminating(object sender, EventArgs e)
        {
            this.SaveGitData();

            if (this.userData.CommitSettings == CommitTrigger.EditorLeaving) this.CommitChanges();

            gitRepo.Dispose();

            DualityEditorApp.EditorIdling -= Plugin_EditorIdling;
            DualityEditorApp.SaveAllTriggered -= Plugin_SaveAllTriggered;
            DualityEditorApp.Terminating -= Plugin_Terminating;
        }

        private void Plugin_SaveAllTriggered(object sender, EventArgs e)
        {
            Logger.Debug("SaveAll was triggered, saving our data.");
            this.SaveGitData();
        }

        public IQueryableCommitLog RequestHistory()
        {
            if (this.isRepoInit)
            {
                return gitRepo.Commits;
            }
            return null;
        }
        #region Log
        private static class Logger
        {
            public static void Debug(string msg, params object[] obj)
            {
                string message = string.Format("[DEBUG][GitPlugin] {0}", msg);
                Log.Editor.WriteWarning(message, obj);
            }
            public static void LogNormal(string msg, params object[] obj)
            {
                string message = string.Format("[GitPlugin] {0}", msg);
                Log.Editor.Write(message, obj);
            }
            public static void LogWarning(string msg, params object[] obj)
            {
                string message = string.Format("[GitPlugin] {0}", msg);
                Log.Editor.WriteWarning(message, obj);
            }
            public static void LogError(string msg, params object[] obj)
            {
                string message = string.Format("[GitPlugin] {0}", msg);
                Log.Editor.WriteError(message, obj);
            }
        }
        #endregion
        #region .gitignore
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
            sb.AppendLine("DualityEditor.exe");
            sb.AppendLine("EditorUserData.xml");
            sb.AppendLine("logfile.txt");
            sb.AppendLine("logfile_editor.txt");
            sb.AppendLine("perflog.txt");
            sb.AppendLine("perflog_editor.txt");
            sb.AppendLine();

            return sb.ToString();
        }

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
                    Logger.Debug("Created .gitignore file.");
                }
                else
                {
                    Logger.Debug(".gitignore does exist. Skipping creation of a new one.");
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to create .gitignore file: {0}", e.Message);
            }
        }
        #endregion
    }
}
