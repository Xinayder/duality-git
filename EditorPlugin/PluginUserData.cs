using System;

namespace RockyTV.GitPlugin.Editor
{
	/// Defines a persistent class used for saving user data.
	public class PluginUserData
	{
		private string authorName = "John Doe";
		/// <summary>
		/// Author information.
		/// Specify the name of the author that is shown in the commit history.
		/// </summary>
		public string AuthorName
		{
			get { return authorName; }
			set { authorName = value; }
		}

		private string authorEmail = "john.doe@example.com";
		/// <summary>
		/// Author information.
		/// Specify the email of the author that is shown in the commit history. 
		/// </summary>
		public string AuthorEmail
		{
			get { return authorEmail; }
			set { authorEmail = value; }
		}
	}
}
