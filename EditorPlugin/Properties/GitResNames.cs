using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RockyTV.GitPlugin.Editor.Properties
{
	/// <summary>
	/// This static class contains constant string representations of certain resource names.
	/// </summary>
	public static class GitResNames
	{
		private const string ManifestBaseName = "RockyTV.GitPlugin.Editor.EmbeddedResources.";

		public const string ImageFolder = ManifestBaseName + "Folder.png";
		public const string ImageFile = ManifestBaseName + "Page.png";
		public const string ImageFileDll = ManifestBaseName + "ScriptCode.png";
		public const string ImageFileAdded = ManifestBaseName + "PageAdded.png";
		public const string ImageFileDllAdded = ManifestBaseName + "ScriptAdded.png";
		public const string ImageFileRemoved = ManifestBaseName + "PageRemoved.png";
		public const string ImageFileDllRemoved = ManifestBaseName + "ScriptRemoved.png";
		public const string ImageFileModified = ManifestBaseName + "PageModified.png";
		public const string ImageFileDllModified = ManifestBaseName + "ScriptModified.png";
	}
}