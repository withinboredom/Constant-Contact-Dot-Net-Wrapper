using System;
namespace ConstantContactBO.Entities
{
    /// <summary>
    /// Contains information about Email
    /// </summary>
    [Serializable]
    public class Email
    {
        #region Fields
        /// <summary>
        /// Email Link
        /// </summary>
        private string _link;

        /// <summary>
        /// Email Link
        /// </summary>
        private string _id;

        /// <summary>
        /// Email Title
        /// </summary>
        private string _title;

        /// <summary>
        /// Email Updated Date
        /// </summary>
        private DateTime? _updated;

        /// <summary>
        /// Email Author Name
        /// </summary>
        private string _authorName;

        /// <summary>
        /// Email Address
        /// </summary>
        private string _emailAddress;

        /// <summary>
        /// Describes the current status of the Email
        /// </summary>
        private EmailStatus _status;

        /// <summary>
        /// Email VerifiedTime
        /// </summary>
        private DateTime? _verifiedTime;
        #endregion

        #region Constructor
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets Email Link
        /// </summary>
        public string Link
        {
            get { return this._link; }
            set { this._link = value; }
        }

        /// <summary>
        /// Gets or sets Email Id
        /// </summary>
        public string Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        /// <summary>
        /// Gets or sets Email Title
        /// </summary>
        public string Title
        {
            get { return this._title; }
            set { this._title = value; }
        }

        /// <summary>
        /// Gets or sets Email Title
        /// </summary>
        public DateTime? Updated
        {
            get { return this._updated; }
            set { this._updated = value; }
        }

        /// <summary>
        /// Gets or sets Email Author Name
        /// </summary>
        public string AuthorName
        {
            get { return this._authorName; }
            set { this._authorName = value; }
        }

        /// <summary>
        /// Gets or sets Email Address
        /// </summary>
        public string EmailAddress
        {
            get { return this._emailAddress; }
            set { this._emailAddress = value; }
        }

        /// <summary>
        /// Gets the current status of the Email
        /// </summary>
        public EmailStatus Status
        {
            get { return this._status; }
            set { this._status = value; }
        }

        /// <summary>
        /// Gets or sets Email VerifiedTime
        /// </summary>
        public DateTime? VerifiedTime
        {
            get { return this._verifiedTime; }
            set { this._verifiedTime = value; }
        }
        #endregion
    }

    /// <summary>
    /// Describes the status of the Email
    /// </summary>
    public enum EmailStatus
    {
        /// <summary>
        /// Email status is undefined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Email status is Verified
        /// </summary>
        Verified = 1,

        /// <summary>
        /// Email status is Pending
        /// </summary>
        Pending = 2
    }
}