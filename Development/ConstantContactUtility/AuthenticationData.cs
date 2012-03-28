using System;
using System.Globalization;

namespace ConstantContactUtility
{
    /// <summary>
    /// Class used to store API Key, username and password for the Constant Contact REST API
    /// 
    /// </summary>
    public class AuthenticationData
    {
        #region Constants
        /// <summary>
        /// Constant Contact base URI
        /// </summary>
        private const string BaseUri = @"https://api.constantcontact.com/ws/customers";
        /// <summary>
        /// Constant Contact Contact Lists resource name
        /// </summary>
        private const string ContactListsUri = "lists";
        /// <summary>
        /// Constant Contact Contacts resource name
        /// </summary>
        private const string ContactsUri = "contacts";
        /// <summary>
        /// Constant Contact Campaigns resource name
        /// </summary>
        private const string CampaignsUri = "campaigns";
        /// <summary>
        /// Constant Contact Settings resource name
        /// </summary>
        private const string SettingsUri = "settings";
        /// <summary>
        /// Constant Contact EmailAddresses resource name
        /// </summary>
        private const string EmailAddressesUri = "emailaddresses";
        /// <summary>
        /// Constant Contact Activities resource name
        /// </summary>
        private const string ActivitiesUri = "activities";
        /// <summary>
        /// Constant Contact Folders resource name
        /// </summary>
        private const string FoldersUri = "library/folders";
        /// <summary>
        /// Constant Contact Host Address
        /// </summary>
        public const string HostAddress = @"https://api.constantcontact.com";
        #endregion

        #region Fields
        /// <summary>
        /// API Application Key and is used to identify the application making an API request
        /// </summary>
        private string _apiKey;
        /// <summary>
        /// Constant Contact Customer's user name
        /// </summary>
        private string _username;
        /// <summary>
        /// Constant Contact Customer's password
        /// </summary>
        private string _password;
        /// <summary>
        /// Account base URI.
        /// {BaseUri}/{UserName}/, where {UserName} is the account owner's Constant Contact user name
        /// </summary>
        private string _accountBaseUri;
        /// <summary>
        /// Account Contact Lists URI
        /// </summary>
        private string _accountContactListsUri;
        /// <summary>
        /// Account Contacts URI
        /// </summary>
        private string _accountContactsUri;
        /// <summary>
        /// Account Service document URI
        /// </summary>
        private string _accountServiceDocumentUri;
        /// <summary>
        /// Account user name used to build the network credentials.
        /// Combination of API and Username: {API Key}%{Username}
        /// </summary>
        private string _accountUsername;
        /// <summary>
        /// Account Emails List URI
        /// </summary>
        private string _accountEmailsListUri;
        /// <summary>
        /// Get the WS URI for current customer
        /// </summary>
        private string _accountEmailCampaignWSCustomerUri;
        /// <summary>
        /// Get the EmailCampaign URI for current customer
        /// </summary>
        private string _accountEmailCampaignsListUri;
        /// <summary>
        /// Get the Activities URI for current customer
        /// </summary>
        private string _accountActivitiesUri;
        /// <summary>
        /// Get the Folders URI for current customer
        /// </summary>
        private string _accountFoldersUri;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the account username
        /// </summary>
        public string Username
        {
            get { return _username; }
            set
            {
                if (string.Compare(_username, value, StringComparison.OrdinalIgnoreCase) == 0) return;
                _username = value;
                UpdateAccountUrIs();
                UpdateAccountUserName();
            }
        }

        /// <summary>
        /// Gets or sets the account password
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// Gets or sets the account API Key
        /// </summary>
        public string ApiKey
        {
            get { return _apiKey; }
            set
            {
                if (string.Compare(_apiKey, value, StringComparison.OrdinalIgnoreCase) == 0) return;
                _apiKey = value;
                UpdateAccountUserName();
            }
        }

        /// <summary>
        /// Gets the account Service document URI
        /// </summary>
        public string AccountServiceDocumentUri
        {
            get { return _accountServiceDocumentUri; }
        }

        /// <summary>
        /// Gets the account Contacts URI
        /// </summary>
        public string AccountContactsUri
        {
            get { return _accountContactsUri; }
        }

        /// <summary>
        /// Gets the account Contact Lists URI
        /// </summary>
        public string AccountContactListsUri
        {
            get { return _accountContactListsUri; }
        }

        /// <summary>
        /// Gets the account user name used to build the network credentials.
        /// (combination of API and Username: {API Key}%{Username})
        /// </summary>
        public string AccountUserName
        {
            get { return _accountUsername; }
        }

        /// <summary>
        /// Gets the root URI for the API connect
        /// </summary>
        public string ApiRootUri
        {
            get { return HostAddress; }
        }

        /// <summary>
        /// Gets the account Emails Lists URI
        /// </summary>
        public string AccountEmailsListUri
        {
            get { return _accountEmailsListUri; }
        }

        /// <summary>
        /// Get the WS URI for current customer
        /// </summary>
        public string AccountEmailCampaignWSCustomerUri
        {
            get { return _accountEmailCampaignWSCustomerUri; }
        }

        /// <summary>
        /// Get the EmailCampaign URI for current customer
        /// </summary>
        public string AccountEmailCampaignsListUri
        {
            get { return _accountEmailCampaignsListUri; }
        }

        /// <summary>
        /// Get the Activities URI for current customer
        /// </summary>
        public string accountActivitiesUri
        {
            get { return _accountActivitiesUri; }
        }

        /// <summary>
        /// Get the Folders URI for current customer
        /// </summary>
        public string accountFoldersUri
        {
            get { return _accountFoldersUri; }
        }

        /// <summary>
        /// Get the CampaignsURI
        /// </summary>
        public string CampaignsURI
        {
            get { return CampaignsUri; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public AuthenticationData()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="APIKey">API Application Key and is used to identify the application making an API request</param>
        /// <param name="username">Constant Contact Customer's user name</param>
        /// <param name="password">Constant Contact Customer's password</param>
        public AuthenticationData(string APIKey, string username, string password)
        {
            _apiKey = APIKey;
            _username = username;
            _password = password;

            UpdateAccountUrIs();
            UpdateAccountUserName();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Update the account Base URI
        /// </summary>
        private void UpdateAccountUrIs()
        {
            _accountBaseUri = String.Format(CultureInfo.InvariantCulture, "{0}/{1}", BaseUri, _username);
            _accountContactListsUri = String.Format(CultureInfo.InvariantCulture, "{0}/{1}", _accountBaseUri, ContactListsUri);
            _accountContactsUri = String.Format(CultureInfo.InvariantCulture, "{0}/{1}", _accountBaseUri, ContactsUri);
            _accountServiceDocumentUri = String.Format(CultureInfo.InvariantCulture, "{0}/", _accountBaseUri);
            _accountEmailsListUri = String.Format(CultureInfo.InvariantCulture, "{0}/{1}/{2}", _accountBaseUri, SettingsUri, EmailAddressesUri);
            _accountEmailCampaignWSCustomerUri = String.Format(CultureInfo.InvariantCulture, "/ws/customers/{0}", _username);
            _accountEmailCampaignsListUri = String.Format(CultureInfo.InvariantCulture, "{0}/{1}", _accountBaseUri, CampaignsUri);
            _accountActivitiesUri = String.Format(CultureInfo.InvariantCulture, "{0}/{1}", _accountBaseUri, ActivitiesUri);
            _accountFoldersUri = String.Format(CultureInfo.InvariantCulture, "{0}/{1}", _accountBaseUri, FoldersUri);
        }

        /// <summary>
        /// Update the account user name used to build the network credentials.
        /// Combination of API and Username: {API Key}%{Username}
        /// </summary>
        private void UpdateAccountUserName()
        {
            _accountUsername = String.Format(CultureInfo.InvariantCulture, "{0}%{1}", _apiKey, _username);
        }
        #endregion
    }
}
