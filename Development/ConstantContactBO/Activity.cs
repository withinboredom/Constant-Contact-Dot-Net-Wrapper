using System;
using System.Collections.Generic;
using System.Text;

namespace ConstantContactBO
{
    /// <summary>
    /// Contains information's about a Constant Contact Activity
    /// 
    /// </summary>
    [Serializable]
    public class Activity
    {
        #region Fields
        /// <summary>
        /// The Activity Name
        /// </summary>
        private string _name;

        /// <summary>
        /// Activity ID
        /// </summary>
        private string _Id;

        /// <summary>
        /// DateTime Activity Was Updated
        /// </summary>
        private string _updated;

        /// <summary>
        /// Activity Link
        /// </summary>
        private string _Link;

        /// <summary>
        /// Type of Activity
        /// </summary>
        private string _Type;

        /// <summary>
        /// Status of Activity
        /// </summary>
        private string _Status;

        /// <summary>
        /// Transaction Count of Activity
        /// </summary>
        private string _transaction;

        /// <summary>
        /// Activity Errors
        /// </summary>
        private string _Errors;

        /// <summary>
        /// Activity Run Start Time
        /// </summary>
        private string _RunStart;

        /// <summary>
        /// Activity Run Finish Time
        /// </summary>
        private string _RunFinish;

        /// <summary>
        /// Activity Insert Time
        /// </summary>
        private string _InsertTime;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with no parameters
        /// </summary>
        public Activity()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the list name
        /// </summary>
        public string activityName
        {
            get { return _name; }
            set { _name = value; }
        }

        public string activityId
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string updated
        {
            get { return _updated; }
            set { _updated = value; }
        }

        public string activityLink
        {
            get { return _Link; }
            set { _Link = value; }
        }

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public string transactionCount
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        public string Errors
        {
            get { return _Errors; }
            set { _Errors = value; }
        }

        public string runStartTime
        {
            get { return _RunStart; }
            set { _RunStart = value; }
        }

        public string runFinishTime
        {
            get { return _RunFinish; }
            set { _RunFinish = value; }
        }

        public string insertTime
        {
            get { return _InsertTime; }
            set { _InsertTime = value; }
        }
        #endregion

    }
}
