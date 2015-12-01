using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LibGit2Sharp;
using LibGit2Sharp.Core;

using System.IO;

using Duality;

using RockyTV.GitPlugin.Editor.Properties;
using RockyTV.GitPlugin.Editor.Extensions;

using EditorPlugin = RockyTV.GitPlugin.Editor.DualityGitEditorPlugin;

namespace RockyTV.GitPlugin.Editor.Forms
{
	public partial class CommitDialog : Form
	{
		public CommitDialog()
		{
			InitializeComponent();
			Focus();
			PopulateTreeView();
		}

		private Dictionary<string, string> objectsList = new Dictionary<string, string>();
		public List<string> StagedFilesList = new List<string>();
		public string CommitMessage = string.Empty;
		public CommitOptions CommitOptions = new CommitOptions();

		private void ListDirectoryFiles(TreeView treeView, string path)
		{
			ImageList treeImageList = new ImageList();
			treeImageList.Images.Add("Folder", GitRes.folder);
			treeImageList.Images.Add("File", GitRes.page_white);
			treeImageList.TransparentColor = Color.Transparent;
			treeImageList.ColorDepth = ColorDepth.Depth32Bit;
			treeView.ImageList = treeImageList;

			treeView.Nodes.Clear();

			DirectoryInfo rootDirectoryInfo = new DirectoryInfo(path);

			treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo, true));
		}

		private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo, bool expanded = false)
		{
			TreeNode directoryNode = new TreeNode(directoryInfo.Name);
			directoryNode.ImageKey = "Folder";
			directoryNode.SelectedImageKey = "Folder";
			directoryNode.Name = directoryInfo.Name;
			if (expanded)
				directoryNode.Expand();

			foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
			{
				if (!(directory.Name == ".git" || directory.FullName == Path.Combine(Environment.CurrentDirectory, "Source", "Packages") ||
					directory.FullName == Path.Combine(Environment.CurrentDirectory, "Source", "Code", ".vs") || directory.FullName == Path.Combine(Environment.CurrentDirectory, "Source", "Code", "EditorPlugin", "obj") ||
					directory.FullName == Path.Combine(Environment.CurrentDirectory, "Source", "Code", "EditorPlugin", "bin") || directory.FullName == Path.Combine(Environment.CurrentDirectory, "Source", "Code", "CorePlugin", "obj") ||
					directory.FullName == Path.Combine(Environment.CurrentDirectory, "Source", "Code", "CorePlugin", "bin") || directory.FullName == Path.Combine(Environment.CurrentDirectory, "Backup")))
						directoryNode.Nodes.Add(CreateDirectoryNode(directory));
			}

			foreach (FileInfo file in directoryInfo.GetFiles())
			{
				if (!(file.Extension == ".suo" || file.Extension == ".csproj.user" || file.Name == "AppData.dat" || file.Name == "logfile.txt" ||
					file.Name == "logfile_editor.txt" || file.Name == "perflog.txt" || file.Name == "perflog_editor.txt" || file.Name == "DualityEditor.exe" ||
					file.Name == "EditorUserData.xml"))
				{
					TreeNode fileNode = new TreeNode(file.Name);
					fileNode.Name = file.Name;
					fileNode.ImageKey = "File";
					fileNode.SelectedImageKey = "File";

					directoryNode.Nodes.Add(fileNode);
				}
			}

			return directoryNode;
		}

		private void AddFilesToDictionary(Dictionary<string, string> dictionary, string path)
		{
			dictionary.Clear();

			DirectoryInfo rootDirectoryInfo = new DirectoryInfo(path);

			dictionary.AddRange(AddDirectoryToDictionary(rootDirectoryInfo));
		}

		private static List<KeyValuePair<string, string>> AddDirectoryToDictionary(DirectoryInfo directoryInfo)
		{
			List<KeyValuePair<string, string>> directoryPair = new List<KeyValuePair<string, string>>();
			directoryPair.Add(new KeyValuePair<string, string>(directoryInfo.FullName, directoryInfo.Name));

			foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
			{
				string[] invalidNames = { ".git", Path.Combine(Environment.CurrentDirectory, "Source", "Packages"), Path.Combine(Environment.CurrentDirectory, "Source", "Code", ".vs"),
					Path.Combine(Environment.CurrentDirectory, "Source", "Code", "EditorPlugin", "obj"), Path.Combine(Environment.CurrentDirectory, "Source", "Code", "EditorPlugin", "bin"),
					Path.Combine(Environment.CurrentDirectory, "Source", "Code", "CorePlugin", "obj"), Path.Combine(Environment.CurrentDirectory, "Source", "Code", "CorePlugin", "bin"),
					Path.Combine(Environment.CurrentDirectory, "Backup")
				};

				// Check if the directory does not contain any of the invalid paths above
				if (!invalidNames.Any(name => directory.FullName.Contains(name)))
					// Check if the directory has any files, if it has, we display it
					if (directory.GetFiles("*", SearchOption.AllDirectories).Count() > 0)
						directoryPair.AddRange(AddDirectoryToDictionary(directory));
			}

			foreach (FileInfo file in directoryInfo.GetFiles())
			{
				if (!(file.Extension == ".suo" || file.Extension == ".csproj.user" || file.Name == "AppData.dat" || file.Name == "logfile.txt" ||
					file.Name == "logfile_editor.txt" || file.Name == "perflog.txt" || file.Name == "perflog_editor.txt" || file.Name == "DualityEditor.exe" ||
					file.Name == "EditorUserData.xml"))
				{
					directoryPair.Add(new KeyValuePair<string, string>(file.FullName, file.Name));
				}
			}

			return directoryPair;
		}

		private void PopulateTreeViewFromDictionary(Dictionary<string, string> dictionary, TreeView treeView)
		{
			ListDirectoryFiles(treeView, dictionary.ElementAt(0).Key);
		}

		private void PopulateTreeView()
		{
			AddFilesToDictionary(objectsList, Environment.CurrentDirectory);
			PopulateTreeViewFromDictionary(objectsList, fileTreeView);
		}

		private void buttonCommit_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(textBoxMessage.Text) || string.IsNullOrWhiteSpace(textBoxMessage.Text))
				textBoxMessage.Focus();
			else
			{
				CommitOptions commitOptions = new CommitOptions();
				commitOptions.AllowEmptyCommit = false;
				commitOptions.AmendPreviousCommit = false;
				commitOptions.PrettifyMessage = true;

				CommitOptions = commitOptions;
				CommitMessage = textBoxMessage.Text;

				AddCheckedNodesToList(fileTreeView.TopNode);

				DialogResult = DialogResult.OK;
				Close();
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void AddCheckedNodesToList(TreeNode treeNode)
		{
			if (objectsList.Count > 0)
			{
				if (treeNode.Checked)
				{
					Log.Editor.WriteWarning("Checked node: {0} ({1})", treeNode.Name, treeNode.FullPath);
					foreach (KeyValuePair<string, string> kvp in objectsList)
					{
						if (treeNode.Name == kvp.Value)
						{
							if (treeNode == fileTreeView.TopNode)
								continue;

							string currentDir = Environment.CurrentDirectory;

							string fullFilePath = Path.Combine(
								currentDir.Substring(0, currentDir.IndexOf(fileTreeView.TopNode.Name)),
								treeNode.FullPath
								);

							if (fullFilePath == kvp.Key)
							{
								if (!StagedFilesList.Contains(kvp.Key))
								{
									StagedFilesList.Add(kvp.Key);
									Log.Editor.Write("Added '{0}' to list", kvp.Key);
								}
							}
						}
					}
				}

				foreach (TreeNode node in treeNode.Nodes)
				{
					AddCheckedNodesToList(node);
				}
			}
		}

		private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
		{
			foreach (TreeNode node in treeNode.Nodes)
			{
				node.Checked = nodeChecked;
				if (node.Nodes.Count > 0)
					CheckAllChildNodes(node, nodeChecked);
			}
		}

		private void stagedFileList_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (e.Action != TreeViewAction.Unknown)
			{
				if (e.Node.Nodes.Count > 0)
				{
					CheckAllChildNodes(e.Node, e.Node.Checked);
				}
			}
		}
	}
}
