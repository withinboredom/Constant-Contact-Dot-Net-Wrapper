using System;
using System.Collections.Generic;

namespace ConstantContactBO
{
    /// <summary>
    /// Contains information's about a Constant Contact
    /// 
    /// </summary>
    [Serializable]
    public class Contact
    {
        #region Fields
        /// <summary>
        /// Id of current Contact
        /// </summary>
        private string _id;

        /// <summary>
        /// Edit link of current Contact
        /// </summary>
        private string _link;

        /// <summary>
        /// Describes the current status of the Contact
        /// </summary>
        private ContactStatus _status;

        /// <summary>
        /// Email Address of Contact
        /// </summary>
        private string _emailAddress;

        /// <summary>
        /// The preferred type of email
        /// </summary>
        private ContactEmailType _emailType;

        /// <summary>
        /// Concatenation of First Name and Last Name
        /// </summary>
        private string _name;

        /// <summary>
        /// First name
        /// </summary>
        private string _firstName;

        /// <summary>
        /// Middle name
        /// </summary>
        private string _middleName;

        /// <summary>
        /// Last name
        /// </summary>
        private string _lastName;

        /// <summary>
        /// Job title
        /// </summary>
        private string _jobTitle;

        /// <summary>
        /// Company name
        /// </summary>
        private string _companyName;

        /// <summary>
        /// Work phone
        /// </summary>
        private string _workPhone;

        /// <summary>
        /// Home phone
        /// </summary>
        private string _homePhone;

        /// <summary>
        /// Address line 1
        /// </summary>
        private string _addressLine1;

        /// <summary>
        /// Address line 2
        /// </summary>
        private string _addressLine2;

        /// <summary>
        /// Address line 3
        /// </summary>
        private string _addressLine3;

        /// <summary>
        /// City
        /// </summary>
        private string _city;

        /// <summary>
        /// State code. Must be a valid US/Canada
        /// </summary>
        private string _stateCode;

        /// <summary>
        /// State name
        /// </summary>
        private string _stateName;

        /// <summary>
        /// Country code
        /// </summary>
        private string _countryCode;

        /// <summary>
        /// Country name
        /// </summary>
        private string _countryName;

        /// <summary>
        /// Postal code
        /// </summary>
        private string _postalCode;

        /// <summary>
        /// Sub postal code
        /// </summary>
        private string _subPostalCode;

        /// <summary>
        /// Customer notes field. This field should only be visible to the Customer 
        /// but not to the Contact
        /// </summary>
        private string _note;

        /// <summary>
        /// Custom field 1
        /// </summary>
        private string _customField1;

        /// <summary>
        /// Custom field 2
        /// </summary>
        private string _customField2;

        /// <summary>
        /// Custom field 3
        /// </summary>
        private string _customField3;

        /// <summary>
        /// Custom field 4
        /// </summary>
        private string _customField4;

        /// <summary>
        /// Custom field 5
        /// </summary>
        private string _customField5;

        /// <summary>
        /// Custom field 6
        /// </summary>
        private string _customField6;

        /// <summary>
        /// Custom field 7
        /// </summary>
        private string _customField7;

        /// <summary>
        /// Custom field 8
        /// </summary>
        private string _customField8;

        /// <summary>
        /// Custom field 9
        /// </summary>
        private string _customField9;

        /// <summary>
        /// Custom field 10
        /// </summary>
        private string _customField10;

        /// <summary>
        /// Custom field 11
        /// </summary>
        private string _customField11;

        /// <summary>
        /// Custom field 12
        /// </summary>
        private string _customField12;

        /// <summary>
        /// Custom field 13
        /// </summary>
        private string _customField13;

        /// <summary>
        /// Custom field 14
        /// </summary>
        private string _customField14;

        /// <summary>
        /// Custom field 15
        /// </summary>
        private string _customField15;

        /// <summary>
        /// Indicates which lists the Contact belongs to
        /// </summary>
        private readonly IList<ContactOptInList> _contactLists;

        /// <summary>
        /// The times the Contact opted-in
        /// </summary>
        private DateTime? _optInTime;

        /// <summary>
        /// The source of the opt-in
        /// </summary>
        private ContactOptSource _optInSource;

        /// <summary>
        /// The time the Contact opted-out
        /// </summary>
        private DateTime? _optOutTime;

        /// <summary>
        /// Source of the opt-out
        /// </summary>
        private ContactOptSource _optOutSource;

        /// <summary>
        /// The reason the Contact opted-out, if provided
        /// </summary>
        private string _optOutReason;

        /// <summary>
        /// The confirmed state of Contact
        /// </summary>
        private bool _confirmed;

        /// <summary>
        /// The insert time
        /// </summary>
        private DateTime? _insertTime;

        /// <summary>
        /// The last update time
        /// </summary>
        private DateTime? _lastUpdateTime;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor with no parameters
        /// </summary>
        public Contact()
        {
            _contactLists = new List<ContactOptInList>();
        }

        /// <summary>
        /// Constructor with e-mail address, first name and last name
        /// </summary>
        /// <param name="emailAddress">Email Address of Contact</param>
        /// <param name="firstName">Contact First Name</param>
        /// <param name="lastName">Contact Last Name</param>
        public Contact(string emailAddress, string firstName, string lastName)
        {
            _contactLists = new List<ContactOptInList>();

            _emailAddress = emailAddress;
            _firstName = firstName;
            _lastName = lastName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Id of current Contact
        /// </summary>
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Edit link of current Contact
        /// </summary>
        public string Link
        {
            get { return _link; }
            set { _link = value; }
        }

        /// <summary>
        /// Gets the current status of the Contact
        /// </summary>
        public ContactStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// Gets or sets the e-mail address of the contact
        /// </summary>
        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                _emailAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets the preferred type of email
        /// </summary>
        public ContactEmailType EmailType
        {
            get { return _emailType; }
            set { _emailType = value; }
        }

        /// <summary>
        /// Gets the concatenation of First Name and Last Name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the first name of the contact
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
            }
        }

        /// <summary>
        /// Gets or sets the middle name of the contact
        /// </summary>
        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                _middleName = value;
            }
        }

        /// <summary>
        /// Gets or sets the last name of the contact
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
            }
        }

        /// <summary>
        /// Gets or sets the job title of the contact
        /// </summary>
        public string JobTitle
        {
            get { return _jobTitle; }
            set
            {
                _jobTitle = value;
            }
        }

        /// <summary>
        /// Gets or sets the company name of the contact
        /// </summary>
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                _companyName = value;
            }
        }

        /// <summary>
        /// Gets or sets the work phone of the contact
        /// </summary>
        public string WorkPhone
        {
            get { return _workPhone; }
            set
            {
                _workPhone = value;
            }
        }

        /// <summary>
        /// Gets or sets the home phone of the contact
        /// </summary>
        public string HomePhone
        {
            get { return _homePhone; }
            set
            {
                _homePhone = value;
            }
        }

        /// <summary>
        /// Gets or sets the address line 1 of the contact
        /// </summary>
        public string AddressLine1
        {
            get { return _addressLine1; }
            set
            {
                _addressLine1 = value;
            }
        }

        /// <summary>
        /// Gets or sets the address line 2 of the contact
        /// </summary>
        public string AddressLine2
        {
            get { return _addressLine2; }
            set
            {
                _addressLine2 = value;
            }
        }

        /// <summary>
        /// Gets or sets the address line 3 of the contact
        /// </summary>
        public string AddressLine3
        {
            get { return _addressLine3; }
            set
            {
                _addressLine3 = value;
            }
        }

        /// <summary>
        /// Gets or sets the city of the contact
        /// </summary>
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
            }
        }

        /// <summary>
        /// Gets or sets the state of the contact
        /// </summary>
        public string StateCode
        {
            get { return _stateCode; }
            set
            {
                _stateCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the StateName
        /// </summary>
        public string StateName
        {
            get { return _stateName; }
            set
            {
                _stateName = value;
            }
        }

        /// <summary>
        /// Gets or sets the country of the contact
        /// </summary>
        public string CountryCode
        {
            get { return _countryCode; }
            set
            {
                _countryCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the CountryName
        /// </summary>
        public string CountryName
        {
            get { return _countryName; }
            set
            {
                _countryName = value;
            }
        }

        /// <summary>
        /// Gets or sets the postal code of the contact
        /// </summary>
        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                _postalCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the sub postal code of the contact
        /// </summary>
        public string SubPostalCode
        {
            get { return _subPostalCode; }
            set
            {
                _subPostalCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the customer notes field
        /// </summary>
        public string Note
        {
            get { return _note; }
            set
            {
                _note = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom field 1 of the contact
        /// </summary>
        public string CustomField1
        {
            get { return _customField1; }
            set
            {
                _customField1 = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom field 2 of the contact
        /// </summary>
        public string CustomField2
        {
            get { return _customField2; }
            set
            {
                _customField2 = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom field 3 of the contact
        /// </summary>
        public string CustomField3
        {
            get { return _customField3; }
            set
            {
                _customField3 = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom field 4 of the contact
        /// </summary>
        public string CustomField4
        {
            get { return _customField4; }
            set
            {
                _customField4 = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom field 5 of the contact
        /// </summary>
        public string CustomField5
        {
            get { return _customField5; }
            set
            {
                _customField5 = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom field 6 of the contact
        /// </summary>
        public string CustomField6
        {
            get { return _customField6; }
            set
            {
                _customField6 = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom field 7 of the contact
        /// </summary>
        public string CustomField7
        {
            get { return _customField7; }
            set
            {
                _customField7 = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom field 8 of the contact
        /// </summary>
        public string CustomField8
        {
            get { return _customField8; }
            set
            {
                _customField8 = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom field 9 of the contact
        /// </summary>
        public string CustomField9
        {
            get { return _customField9; }
            set
            {
                _customField9 = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom field 10 of the contact
        /// </summary>
        public string CustomField10
        {
            get { return _customField10; }
            set
            {
                _customField10 = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom field 11 of the contact
        /// </summary>
        public string CustomField11
        {
            get { return _customField11; }
            set
            {
                _customField11 = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom field 12 of the contact
        /// </summary>
        public string CustomField12
        {
            get { return _customField12; }
            set
            {
                _customField12 = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom field 13 of the contact
        /// </summary>
        public string CustomField13
        {
            get { return _customField13; }
            set
            {
                _customField13 = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom field 14 of the contact
        /// </summary>
        public string CustomField14
        {
            get { return _customField14; }
            set
            {
                _customField14 = value;
            }
        }

        /// <summary>
        /// Gets or sets the custom field 15 of the contact
        /// </summary>
        public string CustomField15
        {
            get { return _customField15; }
            set
            {
                _customField15 = value;
            }
        }

        /// <summary>
        /// Get the lists the Contact belongs to
        /// </summary>
        public IList<ContactOptInList> ContactLists
        {
            get { return _contactLists; }
        }

        /// <summary>
        /// Gets the time the Contact opted-in
        /// </summary>
        public DateTime? OptInTime
        {
            get { return _optInTime; }
            set { _optInTime = value; }
        }

        /// <summary>
        /// Gets or sets the source of the opt-in
        /// </summary>
        public ContactOptSource OptInSource
        {
            get { return _optInSource; }
            set { _optInSource = value; }
        }

        /// <summary>
        /// Gets the time Contact opted-out
        /// </summary>
        public DateTime? OptOutTime
        {
            get { return _optOutTime; }
            set { _optOutTime = value; }
        }

        /// <summary>
        /// Gets or sets the source of the opt-out
        /// </summary>
        public ContactOptSource OptOutSource
        {
            get { return _optOutSource; }
            set { _optOutSource = value; }
        }

        /// <summary>
        /// Gets the reason the Contact opted-out
        /// </summary>
        public string OptOutReason
        {
            get { return _optOutReason; }
            set { _optOutReason = value; }
        }

        /// <summary>
        /// Gets the confirmed state of Contact
        /// </summary>
        public bool Confirmed
        {
            get { return _confirmed; }
            set { _confirmed = value; }
        }

        /// <summary>
        /// Gets the Contact insert time
        /// </summary>
        public DateTime? InsertTime
        {
            get { return _insertTime; }
            set { _insertTime = value; }
        }

        /// <summary>
        /// Gets the Contact last update time
        /// </summary>
        public DateTime? LastUpdateTime
        {
            get { return _lastUpdateTime; }
            set { _lastUpdateTime = value; }
        }

        #endregion
    }

    /// <summary>
    /// Links a Contact to a Constant Contact List
    /// 
    /// </summary>
    [Serializable]
    public class ContactOptInList
    {
        #region Fields
        /// <summary>
        /// Contact List the Contact opted-in
        /// </summary>
        private ContactList _contactList;

        /// <summary>
        /// The time the Contact opted-in to this list
        /// </summary>
        private DateTime? _optInTime;

        /// <summary>
        /// The source of the opt-in to this list
        /// </summary>
        private ContactOptSource _optInSource;
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public ContactOptInList()
        {
            _optInSource = ContactOptSource.Undefined;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the Contact List Contact opted-in
        /// </summary>
        public ContactList ContactList
        {
            get { return _contactList; }
            set { _contactList = value; }
        }

        /// <summary>
        /// Gets the time the Contact opted-in to this list 
        /// </summary>
        public DateTime? OptInTime
        {
            get { return _optInTime; }
            set { _optInTime = value; }
        }

        /// <summary>
        /// Gets or sets the source of the opt-in to this list
        /// </summary>
        public ContactOptSource OptInSource
        {
            get { return _optInSource; }
            set { _optInSource = value; }
        }

        #endregion
    }

    /// <summary>
    /// Contact preferred type of email
    /// 
    /// </summary>
    public enum ContactEmailType
    {
        /// <summary>
        /// Preferred email type of Contact is undefined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Preffered email type of Contact is HTML
        /// </summary>
        Html = 1,

        /// <summary>
        /// Preferred email type of Contact is TEXT
        /// </summary>
        Text = 2
    }

    /// <summary>
    /// Describes the status of the Contact
    /// 
    /// </summary>
    public enum ContactStatus
    {
        /// <summary>
        /// Constat status is undefined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Contact status is Active
        /// </summary>
        Active = 1,

        /// <summary>
        /// Contact status is Unconfirmed
        /// </summary>
        Unconfirmed = 2,

        /// <summary>
        /// Contact status is Removed
        /// </summary>
        Removed = 3,

        /// <summary>
        /// Contact status is DoNotMail
        /// </summary>
        DoNotMail = 4
    }

    /// <summary>
    /// Type of the Contact opt-in / opt-out source
    /// 
    /// </summary>
    public enum ContactOptSource
    {
        /// <summary>
        /// The opt-in / opt-out source is undefined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The opt-in / opt-out source is ACTION_BY_CUSTOMER
        /// </summary>
        ActionByCustomer = 1,

        /// <summary>
        /// The opt-in / opt-out source is ACTION_BY_CONTACT
        /// </summary>
        ActionByContact = 2
    }
}
