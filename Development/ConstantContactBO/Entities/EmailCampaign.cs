using System;
using System.Collections.Generic;

namespace ConstantContactBO.Entities
{
    /// <summary>
    /// Email Campaign Entity
    /// </summary>
    [Serializable]
    public class EmailCampaign
    {
        #region Members
        private string _id;
        private string _name;
        private DateTime _date;
        private DateTime _lastEditDate;
        private DateTime _lastRunDate;
        private DateTime _nextRunDate;
        private CampaignState _state;
        private int _sent;
        private int _opens;
        private int _clicks;
        private int _bounces;
        private int _forwards;
        private int _optOuts;
        private int _spamReports;
        private string _subject;
        private string _fromName;
        private string _fromEmail;
        private int _fromEmailID;
        private string _replyToEmail;
        private int _replyToEmailID;
        private bool _viewAsWebpage;
        private string _viewAsWebpageLinkText;
        private string _viewAsWebpageText;
        private bool _permissionReminder;
        private string _permissionReminderText;
        private string _greetingSalutation;
        private string _greetingName;
        private string _greetingString;
        private string _organizationName;
        private string _organizationAddress1;
        private string _organizationAddress2;
        private string _organizationAddress3;
        private string _organizationCity;
        private string _organizationState;
        private string _organizationInternationalState;
        private string _organizationPostalCode;
        private string _organizationCountry;
        private bool _includeForwardEmail;
        private string _forwardEmailLinkText;
        private bool _includeSubscribeLink;
        private string _subscribeLinkText;
        private string _content = "";
        private string _xContent = "";
        private string _textContent;
        private string _styleSheet;
        private List<string> _urls;
        private List<ContactList> _contactLists;
        private string _emailContentFormat = "";
        private CampaignType _campaignType;
        #endregion

        #region Properties
        /// <summary>
        /// Name for the Campaign
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        /// <summary>
        /// ID for the Campaign
        /// </summary>
        public string ID
        {
            get { return this._id; }
            set { this._id = value; }
        }

        /// <summary>
        /// Campaign Date
        /// </summary>
        public DateTime Date
        {
            get { return this._date; }
            set { this._date = value; }
        }

        /// <summary>
        /// Last date/time the Campaign was edited
        /// </summary>
        public DateTime LastEditDate
        {
            get { return this._lastEditDate; }
            set { this._lastEditDate = value; }
        }

        /// <summary>
        /// Last date/time the Campaign ran
        /// </summary>
        public DateTime LastRunDate
        {
            get { return this._lastRunDate; }
            set { this._lastRunDate = value; }
        }

        /// <summary>
        /// The next date/time the Campaign will run
        /// </summary>
        public DateTime NextRunDate
        {
            get { return this._nextRunDate; }
            set { this._nextRunDate = value; }
        }

        /// <summary>
        /// Status of the Campaign
        /// </summary>
        public CampaignState State
        {
            get { return this._state; }
            set { this._state = value; }
        }

        /// <summary>
        /// Number of emails sent for the campaign
        /// </summary>
        public int Sent
        {
            get { return this._sent; }
            set { this._sent = value; }
        }

        /// <summary>
        /// Number of unique opens
        /// </summary>
        public int Opens
        {
            get { return this._opens; }
            set { this._opens = value; }
        }

        /// <summary>
        /// Number of unique clicks
        /// </summary>
        public int Clicks
        {
            get { return this._clicks; }
            set { this._clicks = value; }
        }

        /// <summary>
        /// Number of bounced emails
        /// </summary>
        public int Bounces
        {
            get { return this._bounces; }
            set { this._bounces = value; }
        }

        /// <summary>
        /// Number of times the campaign was forwarded (using the Forward link if included)
        /// </summary>
        public int Forwards
        {
            get { return this._forwards; }
            set { this._forwards = value; }
        }

        /// <summary>
        /// Number of opt outs from the campaign
        /// </summary>
        public int OptOuts
        {
            get { return this._optOuts; }
            set { this._optOuts = value; }
        }

        /// <summary>
        /// Number of spam reports from the campaign
        /// </summary>
        public int SpamReports
        {
            get { return this._spamReports; }
            set { this._spamReports = value; }
        }

        /// <summary>
        /// CampaignType of the campaign
        /// </summary>
        public CampaignType CampaignType
        {
            get { return this._campaignType; }
            set { this._campaignType = value; }
        }

        /// <summary>
        /// Subject line for the email
        /// </summary>
        public string Subject
        {
            get { return this._subject; }
            set { this._subject = value; }
        }

        /// <summary>
        /// The name that is displayed in the from box in the recipients email client
        /// </summary>
        public string FromName
        {
            get { return this._fromName; }
            set { this._fromName = value; }
        }

        /// <summary>
        /// Email address entry represents From address shows the address that the email originated from
        /// </summary>
        public string FromEmail
        {
            get { return this._fromEmail; }
            set { this._fromEmail = value; }
        }

        /// <summary>
        /// ID Email address entry represents From address shows the address that the email originated from
        /// </summary>
        public int FromEmailID
        {
            get { return this._fromEmailID; }
            set { this._fromEmailID = value; }
        }

        /// <summary>
        /// Email address entry represents the field used by an email client's reply function
        /// </summary>
        public string ReplyToEmail
        {
            get { return this._replyToEmail; }
            set { this._replyToEmail = value; }
        }

        /// <summary>
        /// ID Email address entry represents the field used by an email client's reply function
        /// </summary>
        public int ReplyToEmailID
        {
            get { return this._replyToEmailID; }
            set { this._replyToEmailID = value; }
        }

        /// <summary>
        /// This allows contacts who cannot view images in their email program to open your email in a browser window.
        /// </summary>
        public bool ViewAsWebpage
        {
            get { return this._viewAsWebpage; }
            set { this._viewAsWebpage = value; }
        }

        /// <summary>
        /// The text for the actual link in the View As Webpage link in the email
        /// </summary>
        public string ViewAsWebpageLinkText
        {
            get { return this._viewAsWebpageLinkText; }
            set { this._viewAsWebpageLinkText = value; }
        }

        /// <summary>
        /// The text displayed together with the LinkText at the top of your email
        /// </summary>
        public string ViewAsWebpageText
        {
            get { return this._viewAsWebpageText; }
            set { this._viewAsWebpageText = value; }
        }

        /// <summary>
        /// Whether to show a permission reminder at the top of the email allowing recipients to confirm their opt in.
        /// </summary>
        public bool PermissionReminder
        {
            get { return this._permissionReminder; }
            set { this._permissionReminder = value; }
        }

        /// <summary>
        /// Permission reminder.
        /// </summary>
        public string PermissionReminderText
        {
            get { return this._permissionReminderText; }
            set { this._permissionReminderText = value; }
        }

        /// <summary>
        /// Describes the chosen salutation to be used at the opening of the email (e.g. Dear)
        /// </summary>
        public string GreetingSalutation
        {
            get { return this._greetingSalutation; }
            set { this._greetingSalutation = value; }
        }

        /// <summary>
        /// Indicates if the email greeting should include just the recipients FirstName, just the LastName, Both, or neither (None)
        /// </summary>
        public string GreetingName
        {
            get { return this._greetingName; }
            set { this._greetingName = value; }
        }

        /// <summary>
        /// Allows you to specify the full greeting string instead of the components of the previous two properties.
        /// </summary>
        public string GreetingString
        {
            get { return this._greetingString; }
            set { this._greetingString = value; }
        }

        /// <summary>
        /// Name of organization for use in the email footer
        /// </summary>
        public string OrganizationName
        {
            get { return this._organizationName; }
            set { this._organizationName = value; }
        }

        /// <summary>
        /// Line 1 of the organization address for use in the email footer
        /// </summary>
        public string OrganizationAddress1
        {
            get { return this._organizationAddress1; }
            set { this._organizationAddress1 = value; }
        }

        /// <summary>
        /// Line 2 of the organization address for use in the email footer
        /// </summary>
        public string OrganizationAddress2
        {
            get { return this._organizationAddress2; }
            set { this._organizationAddress2 = value; }
        }

        /// <summary>
        /// Line 3 of the organization address for use in the email footer
        /// </summary>
        public string OrganizationAddress3
        {
            get { return this._organizationAddress3; }
            set { this._organizationAddress3 = value; }
        }

        /// <summary>
        /// City of the organization for use in the email footer
        /// </summary>
        public string OrganizationCity
        {
            get { return this._organizationCity; }
            set { this._organizationCity = value; }
        }

        /// <summary>
        /// State of the organization for use in the email footer
        /// </summary>
        public string OrganizationState
        {
            get { return this._organizationState; }
            set { this._organizationState = value; }
        }

        /// <summary>
        /// International "State" if outside the US for use in the email footer
        /// </summary>
        public string OrganizationInternationalState
        {
            get { return this._organizationInternationalState; }
            set { this._organizationInternationalState = value; }
        }

        /// <summary>
        /// PostalCode/Zipcode of the organization for use in the email footer
        /// </summary>
        public string OrganizationPostalCode
        {
            get { return this._organizationPostalCode; }
            set { this._organizationPostalCode = value; }
        }

        /// <summary>
        /// Country of the organization for use in the email footer
        /// </summary>
        public string OrganizationCountry
        {
            get { return this._organizationCountry; }
            set { this._organizationCountry = value; }
        }

        /// <summary>
        /// Boolean type property that indicates if the email should include a Forward This Email link
        /// </summary>
        public bool IncludeForwardEmail
        {
            get { return this._includeForwardEmail; }
            set { this._includeForwardEmail = value; }
        }

        /// <summary>
        /// If IncludeForwardEmail property is set to YES, then this property should be specified and will appear in the email as the text of the link
        /// </summary>
        public string ForwardEmailLinkText
        {
            get { return this._forwardEmailLinkText; }
            set { this._forwardEmailLinkText = value; }
        }

        /// <summary>
        /// Boolean type property that indicates if the email should include a Subscribe link
        /// </summary>
        public bool IncludeSubscribeLink
        {
            get { return this._includeSubscribeLink; }
            set { this._includeSubscribeLink = value; }
        }

        /// <summary>
        /// If IncludeSubscribeLink property is set to YES, then this property should be specified and will appear in the email as the text of the link
        /// </summary>
        public string SubscribeLinkText
        {
            get { return this._subscribeLinkText; }
            set { this._subscribeLinkText = value; }
        }

        /// <summary>
        /// The full HTML content of the email
        /// </summary>
        public string Content
        {
            get { return this._content; }
            set { this._content = value; }
        }

        /// <summary>
        /// The full XHTML content of the email
        /// </summary>
        public string XContent
        {
            get { return this._xContent; }
            set { this._xContent = value; }
        }

        /// <summary>
        /// The text version of the email content. Will be used for email clients that do not have HTML email capability or have it disabled.
        /// </summary>
        public string TextContent
        {
            get { return this._textContent; }
            set { this._textContent = value; }
        }

        /// <summary>
        /// Collection of URL entries that were included in this email for click tracking purposes
        /// </summary>
        public List<string> URLS
        {
            get { return this._urls; }
            set { this._urls = value; }
        }

        /// <summary>
        /// Collection of ContactList entries that are associated with this email Campaign
        /// </summary>
        public List<ContactList> ContactLists
        {
            get { return this._contactLists; }
            set { this._contactLists = value; }
        }

        /// <summary>
        /// Describes whether email content is based on HTML or XHTML.
        /// </summary>
        public string EmailContentFormat
        {
            get { return this._emailContentFormat; }
            set { this._emailContentFormat = value; }
        }

        /// <summary>
        /// The Style Sheet for the Email
        /// </summary>
        public string StyleSheet
        {
            get { return this._styleSheet; }
            set { this._styleSheet = value; }
        }

        #endregion
    }
}