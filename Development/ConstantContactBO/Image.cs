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
    public class Image
    {
        #region Fields
        /// <summary>
        /// The File Name
        /// </summary>
        private string _fileName;

        /// <summary>
        /// The File Name
        /// </summary>
        private string _fileType;

        /// <summary>
        /// The imgae url
        /// </summary>
        private string _imageUrl;

        /// <summary>
        /// The image height
        /// </summary>
        private string _height;

        /// <summary>
        /// The image width
        /// </summary>
        private string _width;

        /// <summary>
        /// The image description
        /// </summary>
        private string _description;

        /// <summary>
        /// The image file size
        /// </summary>
        private string _fileSize;

        /// <summary>
        /// The image updated time/date
        /// </summary>
        private string _updated;

        /// <summary>
        /// The image md5 hash
        /// </summary>
        private string _md5hash;

        /// <summary>
        /// The image imageUsage
        /// </summary>
        private string _imageUsage;

        /// <summary>
        /// The image link
        /// </summary>
        private string _link;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with no parameters
        /// </summary>
        public Image()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the folder name
        /// </summary>
        public string fileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public string fileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }

        public string imageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        public string height
        {
            get { return _height; }
            set { _height = value; }
        }

        public string width
        {
            get { return _width; }
            set { _width = value; }
        }

        public string description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string fileSize
        {
            get { return _fileSize; }
            set { _fileSize = value; }
        }

        public string updated
        {
            get { return _updated; }
            set { _updated = value; }
        }

        public string md5Hash
        {
            get { return _md5hash; }
            set { _md5hash = value; }
        }

        public string imageUsage
        {
            get { return _imageUsage; }
            set { _imageUsage = value; }
        }

        public string link
        {
            get { return _link; }
            set { _link = value; }
        }
        #endregion

    }
}

