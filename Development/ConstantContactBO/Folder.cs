using System;
using System.Collections.Generic;

using System.Text;

namespace ConstantContactBO
{
    /// <summary>
    /// Contains information's about a Constant Contact Folder
    /// 
    /// </summary>
    [Serializable]
    public class Folder
    {
        #region Fields
        /// <summary>
        /// The Folder Name
        /// </summary>
        private string _name;

        /// <summary>
        /// Folder ID
        /// </summary>
        private string _Id;

        /// <summary>
        /// Folder Link
        /// </summary>
        private string _Link;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with no parameters
        /// </summary>
        public Folder()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the folder name
        /// </summary>
        public string folderName
        {
            get { return _name; }
            set { _name = value; }
        }

        public string folderId
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string folderLink
        {
            get { return _Link; }
            set { _Link = value; }
        }

        #endregion

    }
}
