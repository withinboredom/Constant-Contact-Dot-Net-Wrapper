using System;

namespace ConstantContactBO
{
    /// <summary>
    /// Contains information's about a Constant Contact List
    /// 
    /// </summary>
    [Serializable]
    public class ContactList
    {
        #region Fields
        /// <summary>
        /// The list name
        /// </summary>
        private string _name;

        /// <summary>
        /// System-calculated display name for the list for use where space is limited
        /// </summary>
        private string _shortName;

        /// <summary>
        /// If true, Contacts who sign-up or change their settings may add themselves to this list
        /// </summary>
        private bool _optInDefault;

        /// <summary>
        /// Relative position of this Contact List in the full set of Contact Lists. 
        /// <remarks>
        /// This parameter is used when a group of Contact Lists are presented in a UI such as the Site Visitor SignUp. 
        /// The values are arbitrary relative values. A List with SortOrder=25 will be shown after a List with Sort Order 15.
        /// </remarks>
        /// </summary>
        private int? _sortOrder;

        /// <summary>
        /// Id of Contact List
        /// </summary>
        private string _id;

        /// <summary>
        /// Edit link of Contact List
        /// </summary>
        private string _link;

        /// <summary>
        /// Specified the type of Constant Contact system predefined list
        /// </summary>
        private ContactSystemList _systemList;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with no parameters
        /// </summary>
        public ContactList()
        {
        }

        /// <summary>
        /// Constructor with list name and optInDefault
        /// </summary>
        /// <param name="name">Name of Contact List</param>
        /// <param name="optInDefault">OptInDefault value of Contact List</param>
        public ContactList(string name, bool optInDefault)
        {
            _name = name;
            _optInDefault = optInDefault;
        }

        /// <summary>
        /// Constructor with list id
        /// </summary>
        /// <param name="id"></param>
        public ContactList(string id)
        {
            _id = id;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the list name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets the system-calculated display name for the list for use where space is limited
        /// </summary>
        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }

        /// <summary>
        /// If true, Contacts who sign-up or change their settings may add themselves to this list
        /// </summary>
        public bool OptInDefault
        {
            get { return _optInDefault; }
            set { _optInDefault = value; }
        }

        /// <summary>
        /// Gets or sets the relative position of this Contact List in the full set of Contact Lists. 
        /// <remarks>
        /// This parameter is used when a group of Contact Lists are presented in a UI such as the Site Visitor SignUp. 
        /// The values are arbitrary relative values. A List with SortOrder=25 will be shown after a List with Sort Order 15.
        /// </remarks>
        /// </summary>
        public int? SortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

        /// <summary>
        /// Gets or set the Id of Contact List
        /// </summary>
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Gets or sets the Edit link of Contact List
        /// </summary>
        public string Link
        {
            get { return _link; }
            set { _link = value; }
        }

        /// <summary>
        /// True if Contact List is an Constant system predefined list, false otherwise
        /// </summary>        
        public bool IsSystemList
        {
            get
            {
                bool isSystemList = false;

                if (Enum.IsDefined(typeof(ContactSystemList), _systemList))
                    isSystemList = _systemList != ContactSystemList.Undefined;

                return isSystemList;
            }
        }

        /// <summary>
        /// Specified the type of Constant Contact system predefined list
        /// </summary>
        public ContactSystemList SystemList
        {
            get { return _systemList; }
            set { _systemList = value; }
        }

        #endregion
    }

    /// <summary>
    /// Describes what kind of system predefined list is a Contact List
    /// 
    /// </summary>
    public enum ContactSystemList
    {
        /// <summary>
        /// Contact List is not a system predefined List
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Contact List is the Active system predefined List
        /// </summary>
        Active = 1,

        /// <summary>
        /// Contact List is the Removed system predefined List
        /// </summary>
        Removed = 2,

        /// <summary>
        /// Contact List is the DoNotMail system predefined List
        /// </summary>
        DoNotMail = 3
    }
}
