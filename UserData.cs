using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RockyTV.Duality.GitPlugin
{
    /// Defines a persistent class used for saving user data.
    [Serializable]
    public class GitUserData
    {
        private string authorName = "John Doe";
        private string authorEmail = "john.doe@example.com";
        private CommitTrigger commitSettings = CommitTrigger.EditorReload;
        private CommitFrequency commitFrequency = CommitFrequency.ThirtyMinutes;

        /// <summary>
        /// Author information.
        /// Specify the name of the author that is shown in the commit history.
        /// </summary>
        public string AuthorName
        {
            get { return this.authorName; }
            set { this.authorName = value; }
        }

        /// <summary>
        /// Author information.
        /// Specify the email of the author that is shown in the commit history. 
        /// </summary>
        public string AuthorEmail
        {
            get { return this.authorEmail; }
            set { this.authorEmail = value; }
        }

        /// <summary>
        /// Specify whether commits should be committed when the Duality Editor is closed/reloaded, when the Duality Editor is closed, 
        /// after a few minutes defined by <seealso cref="CommitFrequency"/> have passed, or if commits should be committed manually, when the user wants to commit.
        /// </summary>
        public CommitTrigger CommitSettings
        {
            get { return this.commitSettings; }
            set { this.commitSettings = value; }
        }

        public CommitFrequency CommitFrequency
        {
            get { return this.commitFrequency; }
            set { this.commitFrequency = value; }
        }
    }

    /// <summary>
    /// Specifies when changes should be committed to the local git repository.
    /// </summary>
    public enum CommitTrigger
    {
        /// <summary>
        /// Commit when the editor is being reloaded.
        /// </summary>
        EditorReload,
        /// <summary>
        /// Commit when the editor is closing.
        /// </summary>
        EditorLeaving,
        /// <summary>
        /// Commit automatically
        /// </summary>
        Automatically,
        /// <summary>
        /// Commit whenever you want to commit.
        /// </summary>
        Manual
    }

    /// <summary>
    /// Specifies the frequency of the commits.
    /// Will only work if <see cref="CommitTrigger"/> is set to Automatically.
    /// </summary>
    public enum CommitFrequency
    {
        /// <summary>
        /// Every five minutes.
        /// </summary>
        FiveMinutes,
        /// <summary>
        /// Every fifteen minutes.
        /// </summary>
        FifteenMinutes,
        /// <summary>
        /// Every thirty minutes.
        /// </summary>
        ThirtyMinutes,
        /// <summary>
        /// Every hour.
        /// </summary>
        OneHour
    }
}
