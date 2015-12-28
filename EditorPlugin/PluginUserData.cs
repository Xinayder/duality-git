using System;

namespace RockyTV.GitPlugin.Editor
{
	/// Defines a persistent class used for saving user data.
	public class PluginUserData
	{
		private bool autoFetchConfig = true;
		/// <summary>
		/// Automatically retrieve author name and email from global git settings.
		/// </summary>
		public bool AutoFetchConfig
		{
			get { return autoFetchConfig; }
			set { autoFetchConfig = value; }
		}

		private string authorName = string.Empty;
		/// <summary>
		/// Author information.
		/// Specify the name of the author that is shown in the commit history.
		/// </summary>
		public string AuthorName
		{
			get { return authorName; }
			set { authorName = value; }
		}

		private string authorEmail = string.Empty;
		/// <summary>
		/// Author information.
		/// Specify the email of the author that is shown in the commit history. 
		/// </summary>
		public string AuthorEmail
		{
			get { return authorEmail; }
			set { authorEmail = value; }
		}

		private bool usefulLogMessages = true;
		/// <summary>
		/// Specify whether to print messages like "nothing to commit" to output.
		/// This does not apply to exceptions.
		/// </summary>
		public bool UsefulLogMessages
		{
			get { return usefulLogMessages; }
			set { usefulLogMessages = value; }
		}
	}
}
