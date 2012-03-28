using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using ConstantContactBO;

namespace ConstantContactUtility.Components
{
    /// <summary>
    /// Parse response Streams into Contacts, creates entries for creating and updating Contacts
    /// 
    /// </summary>
    public class ContactComponent
    {
        #region Fields
        /// <summary>
        /// Association between the ContactStatus enum and real Contact Status values
        /// </summary>
        private static Dictionary<ContactStatus, string> contactStatusNames;

        /// <summary>
        /// Assocition between the ContactOptSource enum and real Contact OptIn/OptOut Source values
        /// </summary>
        private static Dictionary<ContactOptSource, string> optSourceNames;

        /// <summary>
        /// Association between the ContactEmailType enum and real Contact Email Type values
        /// </summary>
        private static Dictionary<ContactEmailType, string> emailTypeNames;
        #endregion

        #region Constants
        /// <summary>
        /// Atom namespace
        /// </summary>
        private const string AtomNamespace = @"http://www.w3.org/2005/Atom";
        /// <summary>
        /// Constant namespace
        /// </summary>
        private const string ConstantNamespace = @"http://ws.constantcontact.com/ns/1.0/";

        /// <summary>
        /// Attribute value used to retrieve the next link of chunk data
        /// </summary>
        private const string ContactXmlAttributeValueNext = "next";

        /// <summary>
        /// Attribute value used to retrieve the id of a Contact
        /// </summary>
        private const string ContactXmlAttributeValueEdit = "edit";

        /// <summary>
        /// Xml node name of Contact Name
        /// </summary>
        private const string ContactXmlElementName = "Name";

        /// <summary>
        /// Xml node name of Contact Email Address
        /// </summary>
        private const string ContactXmlElementEmailAddresss = "EmailAddress";

        /// <summary>
        /// Xml node name of Contact Email Type
        /// </summary>
        private const string ContactXmlElementEmailType = "EmailType";

        /// <summary>
        /// Xml node name of Contact Status
        /// </summary>
        private const string ContactXmlElementStatus = "Status";

        /// <summary>
        /// Xml node name of Contact OptInTime
        /// </summary>
        private const string ContactXmlElementOptInTime = "OptInTime";

        /// <summary>
        /// Xml node name of Contact OptInSource
        /// </summary>
        private const string ContactXmlElementOptInSource = "OptInSource";

        /// <summary>
        /// Xml node name of Contact First Name
        /// </summary>
        private const string ContactXmlElementFirstName = "FirstName";

        /// <summary>
        /// Xml node name of Contact Middle Name
        /// </summary>
        private const string ContactXmlElementMiddleName = "MiddleName";

        /// <summary>
        /// Xml node name of Contact Last Name
        /// </summary>
        private const string ContactXmlElementLastName = "LastName";

        /// <summary>
        /// Xml node name of Contact Job Title
        /// </summary>
        private const string ContactXmlElementJobTitle = "JobTitle";

        /// <summary>
        /// Xml node name of Contact Company Name
        /// </summary>
        private const string ContactXmlElementCompanyName = "CompanyName";

        /// <summary>
        /// Xml node name of Contact Home Phone
        /// </summary>
        private const string ContactXmlElementHomePhone = "HomePhone";

        /// <summary>
        /// Xml node name of Contact Work Phone
        /// </summary>
        private const string ContactXmlElementWorkPhone = "WorkPhone";

        /// <summary>
        /// Xml node name of Contact Address Line 1
        /// </summary>
        private const string ContactXmlElementAddr1 = "Addr1";

        /// <summary>
        /// Xml node name of Contact Address Line 2
        /// </summary>
        private const string ContactXmlElementAddr2 = "Addr2";

        /// <summary>
        /// Xml node name of Contact Address Line 3
        /// </summary>
        private const string ContactXmlElementAddr3 = "Addr3";

        /// <summary>
        /// Xml node name of Contact City
        /// </summary>
        private const string ContactXmlElementCity = "City";

        /// <summary>
        /// Xml node name of Contact State Code
        /// </summary>
        private const string ContactXmlElementStateCode = "StateCode";

        /// <summary>
        /// Xml node name of Contact State Name
        /// </summary>
        private const string ContactXmlElementStateName = "StateName";

        /// <summary>
        /// Xml node name of Contact Country Code
        /// </summary>
        private const string ContactXmlElementCountryCode = "CountryCode";

        /// <summary>
        /// Xml node name of Contact Country Name
        /// </summary>
        private const string ContactXmlElementCountryName = "CountryName";

        /// <summary>
        /// Xml node name of Contact Postal Code
        /// </summary>
        private const string ContactXmlElementPostalCode = "PostalCode";

        /// <summary>
        /// Xml node name of Contact Sub Postal Code
        /// </summary>
        private const string ContactXmlElementSubPostalCode = "SubPostalCode";

        /// <summary>
        /// Xml node name of Contact Note
        /// </summary>
        private const string ContactXmlElementNote = "Note";

        /// <summary>
        /// Xml node name of Contact Custom Field 1
        /// </summary>
        private const string ContactXmlElementCustomField1 = "CustomField1";

        /// <summary>
        /// Xml node name of Contact Custom Field 2
        /// </summary>
        private const string ContactXmlElementCustomField2 = "CustomField2";

        /// <summary>
        /// Xml node name of Contact Custom Field 3
        /// </summary>
        private const string ContactXmlElementCustomField3 = "CustomField3";

        /// <summary>
        /// Xml node name of Contact Custom Field 4
        /// </summary>
        private const string ContactXmlElementCustomField4 = "CustomField4";

        /// <summary>
        /// Xml node name of Contact Custom Field 5
        /// </summary>
        private const string ContactXmlElementCustomField5 = "CustomField5";

        /// <summary>
        /// Xml node name of Contact Custom Field 6
        /// </summary>
        private const string ContactXmlElementCustomField6 = "CustomField6";

        /// <summary>
        /// Xml node name of Contact Custom Field 7
        /// </summary>
        private const string ContactXmlElementCustomField7 = "CustomField7";

        /// <summary>
        /// Xml node name of Contact Custom Field 8
        /// </summary>
        private const string ContactXmlElementCustomField8 = "CustomField8";

        /// <summary>
        /// Xml node name of Contact Custom Field 9
        /// </summary>
        private const string ContactXmlElementCustomField9 = "CustomField9";

        /// <summary>
        /// Xml node name of Contact Custom Field 10
        /// </summary>
        private const string ContactXmlElementCustomField10 = "CustomField10";

        /// <summary>
        /// Xml node name of Contact Custom Field 11
        /// </summary>
        private const string ContactXmlElementCustomField11 = "CustomField11";

        /// <summary>
        /// Xml node name of Contact Custom Field 12
        /// </summary>
        private const string ContactXmlElementCustomField12 = "CustomField12";

        /// <summary>
        /// Xml node name of Contact Custom Field 13
        /// </summary>
        private const string ContactXmlElementCustomField13 = "CustomField13";

        /// <summary>
        /// Xml node name of Contact Custom Field 14
        /// </summary>
        private const string ContactXmlElementCustomField14 = "CustomField14";

        /// <summary>
        /// Xml node name of Contact Custom Field 15
        /// </summary>
        private const string ContactXmlElementCustomField15 = "CustomField15";

        /// <summary>
        /// Xml node name of Contact Confirmed
        /// </summary>
        private const string ContactXmlElementConfirmed = "Confirmed";

        /// <summary>
        /// Xml node name of Contact Insert Time
        /// </summary>
        private const string ContactXmlElementInsertTime = "InsertTime";

        /// <summary>
        /// Xml node name of Contact Last Update Time
        /// </summary>
        private const string ContactXmlElementLastUpdateTime = "LastUpdateTime";

        /// <summary>
        /// Xml node name of Contacts Contact Lists
        /// </summary>
        private const string ContactXmlElementContactLists = "ContactLists";
        
        /// <summary>
        /// Constant that describes status of Active Contacts
        /// </summary>
        private const string ContactStatusActive = "Active";

        /// <summary>
        /// Constant that describes status of Unconfirmed Contacts
        /// </summary>
        private const string ContactStatusUnconfirmed = "Unconfirmed";

        /// <summary>
        /// Constant that describes status of Removed Contacts
        /// </summary>
        private const string ContactStatusRemoved = "Removed";

        /// <summary>
        /// Constant that describes status of DoNotMail Contacts
        /// </summary>
        private const string ContactStatusDoNotMail = "Do Not Mail";

        /// <summary>
        /// Constant that describes the customer opt in/out source
        /// </summary>
        private const string OptSourceCustomer = "ACTION_BY_CUSTOMER";

        /// <summary>
        /// Constant that describes the contact opt in/out source
        /// </summary>
        private const string OptSourceContact = "ACTION_BY_CONTACT";

        /// <summary>
        /// Constant that describes HTML email type
        /// </summary>
        private const string ContactEmailTypeHtml = "HTML";

        /// <summary>
        /// Constant that describes Text email type
        /// </summary>
        private const string ContactEmailTypeText = "Text";
        #endregion

        #region Constructor
        /// <summary>
        /// Class constructor
        /// </summary>
        static ContactComponent()
        {
            InitializeContactStatusNames();
            InitializeOptSourceNames();
            InitializeEmailTypeNames();
        }
        #endregion

        #region Public static methods
        /// <summary>
        /// Get the collection of Contacts from the Http response stream
        /// </summary>
        /// <param name="stream">Response Stream</param>
        /// <param name="next">Link to the next chunk of data</param>
        /// <returns>Collection of Contact</returns>
        public static List<Contact> GetContactCollection(Stream stream, out string next)
        {
            List<Contact> list = new List<Contact>();
            //const string xpathSelect = @"//cc:Contact";
            const string xpathSelect = @"//at:entry";

            StreamReader reader = new StreamReader(stream);
            XmlTextReader xmlreader = new XmlTextReader(reader);
            XPathDocument doc = new XPathDocument(xmlreader);

            // initialize navigator
            XPathNavigator pn = doc.CreateNavigator();

            // initialize namespace manager
            XmlNamespaceManager resolver = new XmlNamespaceManager(pn.NameTable);
            resolver.AddNamespace("at", AtomNamespace);
            resolver.AddNamespace("cc", ConstantNamespace);

            XPathExpression expr = pn.Compile(xpathSelect);
            expr.SetContext(resolver);

            XPathNodeIterator nodes = pn.Select(expr);
            while (nodes.MoveNext())
            {
                // save current node
                XPathNavigator node = nodes.Current;

                // add Contact List object to the collection
                list.Add(GetContactDetail(node, resolver));
            }

            next = GetNextLink(pn, resolver);

            reader.Close();
            xmlreader.Close();

            return list;
        }

        /// <summary>
        /// Retrieve an Individual Contact from the Http response stream.        
        /// </summary>
        /// <param name="stream">Response Stream</param>
        /// <returns>Contact parsed from the response Stream</returns>
        public static Contact GetContactDetails(Stream stream)
        {
            Contact contact = null;
            //const string xpathSelect = @"//cc:Contact";
            const string xpathSelect = @"//at:entry";

            StreamReader reader = new StreamReader(stream);
            XmlTextReader xmlreader = new XmlTextReader(reader);
            XPathDocument doc = new XPathDocument(xmlreader);

            // initialize navigator
            XPathNavigator pn = doc.CreateNavigator();

            // initialize namespace manager
            XmlNamespaceManager resolver = new XmlNamespaceManager(pn.NameTable);
            resolver.AddNamespace("at", AtomNamespace);
            resolver.AddNamespace("cc", ConstantNamespace);

            XPathExpression expr = pn.Compile(xpathSelect);
            expr.SetContext(resolver);

            XPathNodeIterator nodes = pn.Select(expr);
            while (nodes.MoveNext())
            {
                // save current node
                XPathNavigator node = nodes.Current;

                // get the Contact object
                contact = GetContactDetail(node, resolver);
            }

            reader.Close();
            xmlreader.Close();

            return contact;
        }

        /// <summary>
        /// Get the Atom entry for newly Contact to be send to Constant server
        /// </summary>
        /// <param name="contact">Contact to be created</param>
        /// <param name="accountContactListUri">Uri address of Account Owner Contact resource</param>
        /// <returns>Atom entry for creating specified Contact</returns>        
        public static StringBuilder CreateNewContact(Contact contact, string accountContactListUri)
        {
            return CreateAtomEntry(contact, accountContactListUri);
        }

        /// <summary>
        /// Get the Atom entry for the Contact to be updated
        /// </summary>
        /// <param name="contact">Contact to be updated</param>
        /// <param name="apiUri">Uri of the API</param>
        /// <param name="accountContactListUri">Uri address of Account Owner Contact List resource</param>        
        /// <param name="fullUpdate">True if all Contact fields will be update; False otherwise (only the following fields 
        /// will be updated: EmailAddress, FirstName, LastName, MiddleName, HomePhone, Addr1, Addr2, Addr3,
        /// City, StateCode, StateName, CountryCode, CountryName, PostalCode, SubPostalCode)</param>
        /// <returns>Atom entry for updating specified Contact</returns>
        public static StringBuilder UpdateContact(Contact contact, string apiUri,
            string accountContactListUri, bool fullUpdate)
        {
            return fullUpdate
                       ? CreateFullUpdateAtomEntry(contact, apiUri, accountContactListUri)
                       : CreateSmallUpdateAtomEntry(contact, apiUri, accountContactListUri);
        }

        /// <summary>
        /// Get the Atom entry for the Contact to be removed from all Contact Lists
        /// </summary>
        /// <param name="contact">Contact to be updated</param>
        /// <param name="accountContactUri">Uri address of Account Owner Contact resource</param>
        /// <returns>Atom entry for updating specified Contact</returns>
        public static StringBuilder RemoveContactFromAllLists(Contact contact, string accountContactUri)
        {
            return CreateRemoveFromAllListsAtomEntry(contact, accountContactUri);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initialize ContactStatusName dictionary
        /// </summary>
        private static void InitializeContactStatusNames()
        {
            contactStatusNames = new Dictionary<ContactStatus, string>();
            contactStatusNames.Add(ContactStatus.Active, ContactStatusActive);
            contactStatusNames.Add(ContactStatus.DoNotMail, ContactStatusDoNotMail);
            contactStatusNames.Add(ContactStatus.Removed, ContactStatusRemoved);
            contactStatusNames.Add(ContactStatus.Unconfirmed, ContactStatusUnconfirmed);
        }

        /// <summary>
        /// Get the ContactStatus enum
        /// </summary>
        /// <param name="contactStatus">Contact Status real value</param>
        /// <returns>ContactStatus enum value</returns>
        private static ContactStatus GetContactStatus(string contactStatus)
        {
            ContactStatus status = ContactStatus.Undefined;

            if (contactStatusNames.ContainsValue(contactStatus))
            {
                foreach (KeyValuePair<ContactStatus, string> kvp in contactStatusNames)
                {
                    if (0 == string.Compare(kvp.Value, contactStatus, StringComparison.OrdinalIgnoreCase))
                    {
                        status = kvp.Key;
                        break;
                    }
                }
            }

            return status;
        }

        /// <summary>
        /// Initialize Opt Source Name dictionary
        /// </summary>
        private static void InitializeOptSourceNames()
        {
            optSourceNames = new Dictionary<ContactOptSource, string>();
            optSourceNames.Add(ContactOptSource.ActionByContact, OptSourceContact);
            optSourceNames.Add(ContactOptSource.ActionByCustomer, OptSourceCustomer);
        }

        /// <summary>
        /// Initialize Email Type Name dictionary
        /// </summary>
        private static void InitializeEmailTypeNames()
        {
            emailTypeNames = new Dictionary<ContactEmailType, string>();
            emailTypeNames.Add(ContactEmailType.Text, ContactEmailTypeText);
            emailTypeNames.Add(ContactEmailType.Html, ContactEmailTypeHtml);
        }

        /// <summary>
        /// Get the ContactOptSource enum
        /// </summary>
        /// <param name="optSource">OptIn/OptOur Source real value</param>
        /// <returns>ContactOptSource enum value</returns>
        private static ContactOptSource GetOptSource(string optSource)
        {
            ContactOptSource source = ContactOptSource.Undefined;

            if (optSourceNames.ContainsValue(optSource))
            {
                foreach (KeyValuePair<ContactOptSource, string> kvp in optSourceNames)
                {
                    if (0 == string.Compare(kvp.Value, optSource, StringComparison.OrdinalIgnoreCase))
                    {
                        source = kvp.Key;
                        break;
                    }
                }
            }

            return source;
        }

        /// <summary>
        /// Get the ContactEmailType enum
        /// </summary>
        /// <param name="emailType">Email Type real value</param>
        /// <returns>ContactEmailType enum value</returns>
        private static ContactEmailType GetEmailType(string emailType)
        {
            ContactEmailType type = ContactEmailType.Undefined;

            try
            {
                type = (ContactEmailType)Enum.Parse(typeof(ContactEmailType), emailType, true);
            }
            catch (Exception)
            {
                // "swallow it"
            }

            return type;
        }

        /// <summary>
        /// Gets the link to next chunk of data
        /// </summary>
        /// <param name="navigator">Xml data cursor model</param>
        /// <param name="resolver">Xml namespace resolver</param>
        /// <returns>Link to next chunk of data</returns>
        private static string GetNextLink(XPathNavigator navigator, IXmlNamespaceResolver resolver)
        {
            return GetLink(navigator, ContactXmlAttributeValueNext, resolver);
        }

        /// <summary>
        /// Gets the link with specified rel attribute value
        /// </summary>
        /// <param name="navigator">Xml data cursor model</param>
        /// <param name="attributeValue">Value of "rel" attribute</param>
        /// <param name="resolver">Xml namespace resolver</param>
        /// <returns>Link to the chunk data</returns>
        private static string GetLink(XPathNavigator navigator, string attributeValue, IXmlNamespaceResolver resolver)
        {
            string xpathNext = String.Format(CultureInfo.InvariantCulture,
                                             @"//at:link[@rel='{0}']", attributeValue);

            XPathNodeIterator link = navigator.Select(xpathNext, resolver);
            string linkHref = string.Empty;

            while (link.MoveNext())
            {
                Regex r = new Regex(@"\?next\=([a-z]|[A-Z]|\d|-)+$");

                linkHref = r.Match(link.Current.GetAttribute("href", string.Empty)).Value;
            }

            return linkHref;
        }

        /// <summary>
        /// Gets the edit Link of Contact from Xml data
        /// </summary>
        /// <param name="node">Xml data cursor model</param>
        /// <param name="resolver">Xml namespace resolver</param>
        /// <returns>Edit Link of Contact</returns>
        private static string GetContactLink(XPathNavigator node, IXmlNamespaceResolver resolver)
        {
            string xpathId = String.Format(CultureInfo.InvariantCulture,
                                           @"at:link[@rel='{0}']", ContactXmlAttributeValueEdit);

            XPathNodeIterator idNode = node.Select(xpathId, resolver);
            string id = string.Empty;

            while (idNode.MoveNext())
            {
                id = idNode.Current.GetAttribute("href", string.Empty);
                
                break;
            }

            return id;
        }

        /// <summary>
        /// Gets the Id of Contact from Xml data
        /// </summary>
        /// <param name="node">Xml data cursor model</param>
        /// <returns>Id of Contact</returns>
        private static string GetContactId(XPathNavigator node)
        {
            Regex r = new Regex(@"/([a-z]|[A-Z]|-|\d)+$");

            return r.Match(node.GetAttribute("id", string.Empty)).Value.Substring(1);
        }

        /// <summary>
        /// Gets the Id of Contact List from Xml data
        /// </summary>
        /// <param name="node">Xml data cursor model</param>
        /// <returns>Id of Contact List</returns>
        private static string GetContactListId(XPathNavigator node)
        {
            Regex r = new Regex(@"/([a-z]|[A-Z]|-|\d)+$");

            return r.Match(node.GetAttribute("id", string.Empty)).Value.Substring(1);
        }

        /// <summary>
        /// Get Contact object with details from specified Xml data
        /// </summary>
        /// <param name="node">Xml data cursor model</param>
        /// <param name="resolver">Xml namespace resolver</param>
        /// <returns>Contact with details</returns>
        private static Contact GetContactDetail(XPathNavigator node, IXmlNamespaceResolver resolver)
        {
            Contact c = new Contact();
            c.Link = GetContactLink(node, resolver);

            const string xpathSelect = @"at:content/cc:Contact";

            XPathNodeIterator contactNodes = node.Select(xpathSelect, resolver);

            while (contactNodes.MoveNext())
            {
                XPathNavigator currentContactNode = contactNodes.Current;

                c.Id = GetContactId(currentContactNode);

                if (currentContactNode.HasChildren)
                {
                    currentContactNode.MoveToFirstChild();
                    do
                    {
                        switch (currentContactNode.Name)
                        {
                            case ContactXmlElementAddr1:
                                c.AddressLine1 = currentContactNode.Value;
                                break;
                            case ContactXmlElementAddr2:
                                c.AddressLine2 = currentContactNode.Value;
                                break;
                            case ContactXmlElementAddr3:
                                c.AddressLine3 = currentContactNode.Value;
                                break;
                            case ContactXmlElementCity:
                                c.City = currentContactNode.Value;
                                break;
                            case ContactXmlElementCompanyName:
                                c.CompanyName = currentContactNode.Value;
                                break;
                            case ContactXmlElementConfirmed:
                                c.Confirmed = currentContactNode.ValueAsBoolean;
                                break;
                            case ContactXmlElementCountryCode:
                                c.CountryCode = currentContactNode.Value;
                                break;
                            case ContactXmlElementCountryName:
                                c.CountryName = currentContactNode.Value;
                                break;
                            case ContactXmlElementCustomField1:
                                c.CustomField1 = currentContactNode.Value;
                                break;
                            case ContactXmlElementCustomField2:
                                c.CustomField2 = currentContactNode.Value;
                                break;
                            case ContactXmlElementCustomField3:
                                c.CustomField3 = currentContactNode.Value;
                                break;
                            case ContactXmlElementCustomField4:
                                c.CustomField4 = currentContactNode.Value;
                                break;
                            case ContactXmlElementCustomField5:
                                c.CustomField5 = currentContactNode.Value;
                                break;
                            case ContactXmlElementCustomField6:
                                c.CustomField6 = currentContactNode.Value;
                                break;
                            case ContactXmlElementCustomField7:
                                c.CustomField7 = currentContactNode.Value;
                                break;
                            case ContactXmlElementCustomField8:
                                c.CustomField8 = currentContactNode.Value;
                                break;
                            case ContactXmlElementCustomField9:
                                c.CustomField9 = currentContactNode.Value;
                                break;
                            case ContactXmlElementCustomField10:
                                c.CustomField10 = currentContactNode.Value;
                                break;
                            case ContactXmlElementCustomField11:
                                c.CustomField11 = currentContactNode.Value;
                                break;
                            case ContactXmlElementCustomField12:
                                c.CustomField12 = currentContactNode.Value;
                                break;
                            case ContactXmlElementCustomField13:
                                c.CustomField13 = currentContactNode.Value;
                                break;
                            case ContactXmlElementCustomField14:
                                c.CustomField14 = currentContactNode.Value;
                                break;
                            case ContactXmlElementCustomField15:
                                c.CustomField15 = currentContactNode.Value;
                                break;
                            case ContactXmlElementEmailAddresss:
                                c.EmailAddress = currentContactNode.Value;
                                break;
                            case ContactXmlElementEmailType:
                                c.EmailType = GetEmailType(currentContactNode.Value);
                                break;
                            case ContactXmlElementFirstName:
                                c.FirstName = currentContactNode.Value;
                                break;
                            case ContactXmlElementHomePhone:
                                c.HomePhone = currentContactNode.Value;
                                break;
                            case ContactXmlElementInsertTime:
                                c.InsertTime = currentContactNode.ValueAsDateTime;
                                break;
                            case ContactXmlElementJobTitle:
                                c.JobTitle = currentContactNode.Value;
                                break;
                            case ContactXmlElementLastName:
                                c.LastName = currentContactNode.Value;
                                break;
                            case ContactXmlElementLastUpdateTime:
                                c.LastUpdateTime = currentContactNode.ValueAsDateTime;
                                break;
                            case ContactXmlElementMiddleName:
                                c.MiddleName = currentContactNode.Value;
                                break;
                            case ContactXmlElementName:
                                c.Name = currentContactNode.Value;
                                break;
                            case ContactXmlElementNote:
                                c.Note = currentContactNode.Value;
                                break;
                            case ContactXmlElementPostalCode:
                                c.PostalCode = currentContactNode.Value;
                                break;
                            case ContactXmlElementStateCode:
                                c.StateCode = currentContactNode.Value;
                                break;
                            case ContactXmlElementStateName:
                                c.StateName = currentContactNode.Value;
                                break;
                            case ContactXmlElementStatus:
                                c.Status = GetContactStatus(currentContactNode.Value);
                                break;
                            case ContactXmlElementSubPostalCode:
                                c.SubPostalCode = currentContactNode.Value;
                                break;
                            case ContactXmlElementWorkPhone:
                                c.WorkPhone = currentContactNode.Value;
                                break;
                            case ContactXmlElementContactLists:
                                if (currentContactNode.HasChildren)
                                {
                                    // loop through all the Contact List
                                    XPathNavigator contactListsNode = currentContactNode.Clone();
                                    XPathNodeIterator lists = contactListsNode.Select("//cc:ContactList", resolver);
                                    while (lists.MoveNext())
                                    {
                                        // get current Contact List
                                        XPathNavigator list = lists.Current;

                                        ContactList contactList = new ContactList();
                                        // get the id of Contact List
                                        contactList.Id = GetContactListId(list);

                                        ContactOptInList optInList = new ContactOptInList();
                                        optInList.ContactList = contactList;

                                        if (list.HasChildren)
                                        {
                                            list.MoveToFirstChild();
                                            do
                                            {
                                                switch (list.Name)
                                                {
                                                    case ContactXmlElementOptInSource:
                                                        optInList.OptInSource = GetOptSource(list.Value);
                                                        break;
                                                    case ContactXmlElementOptInTime:
                                                        optInList.OptInTime = list.ValueAsDateTime;
                                                        break;
                                                }
                                            } while (list.MoveToNext());
                                        }

                                        // add the Contact List to current Contact
                                        c.ContactLists.Add(optInList);
                                    }
                                }
                                break;
                        }

                    } while (currentContactNode.MoveToNext());
                }

                break;
            }

            return c;
        }

        /// <summary>
        /// Create an Atom entry used to create a new Contact
        /// </summary>
        /// <param name="contact">Contact to be created</param>
        /// <param name="accountContactListUri">Uri address of Account Owner Contact List resource</param>
        /// <returns>Atom entry used to create new Contact
        /// <example>
        ///     <entry xmlns="http://www.w3.org/2005/Atom">
        ///         <title type="text"> </title>
        ///         <updated>2008-07-23T14:21:06.407Z</updated>
        ///         <author></author>
        ///         <id>data:,none</id>
        ///         <summary type="text">Contact</summary>
        ///         <content type="application/vnd.ctct+xml">
        ///             <Contact xmlns="http://ws.constantcontact.com/ns/1.0/">
        ///                 <EmailAddress>test101@example.com</EmailAddress>
        ///                 <FirstName>First</FirstName>
        ///                 <LastName>Last</LastName>
        ///                 <OptInSource>ACTION_BY_CONTACT</OptInSource>
        ///                 <ContactLists>
        ///                     <ContactList id="http://api.constantcontact.com/ws/customers/joesflowers/lists/1" />
        ///                 </ContactLists>
        ///             </Contact>
        ///         </content>
        ///     </entry>
        /// </example></returns>
        private static StringBuilder CreateAtomEntry(Contact contact, string accountContactListUri)
        {
            StringBuilder data = new StringBuilder();

            data.AppendFormat("<entry xmlns=\"{0}\">", AtomNamespace);
            data.Append("<title type=\"text\"></title>");
            data.Append("<updated>2008-07-23T14:21:06.407Z</updated>");
            data.Append("<author></author>");
            data.Append("<id>data:,none</id>");
            data.Append("<summary type=\"text\">Contact</summary>");
            data.Append("<content type=\"application/vnd.ctct+xml\">");
            data.AppendFormat("<Contact xmlns=\"{0}\">", ConstantNamespace);
            data.AppendFormat("<EmailAddress>{0}</EmailAddress>", contact.EmailAddress);
            data.AppendFormat("<FirstName>{0}</FirstName>", contact.FirstName);
            data.AppendFormat("<MiddleName>{0}</MiddleName>", contact.MiddleName);
            data.AppendFormat("<LastName>{0}</LastName>", contact.LastName);
            data.AppendFormat("<HomePhone>{0}</HomePhone>", contact.HomePhone);
            data.AppendFormat("<Addr1>{0}</Addr1>", contact.AddressLine1);
            data.AppendFormat("<Addr2>{0}</Addr2>", contact.AddressLine2);
            data.AppendFormat("<Addr3>{0}</Addr3>", contact.AddressLine3);
            data.AppendFormat("<City>{0}</City>", contact.City);
            data.AppendFormat("<StateCode>{0}</StateCode>", contact.StateCode);
            data.AppendFormat("<StateName>{0}</StateName>", contact.StateName);
            data.AppendFormat("<PostalCode>{0}</PostalCode>", contact.PostalCode);
            data.AppendFormat("<SubPostalCode>{0}</SubPostalCode>", contact.SubPostalCode);
            data.AppendFormat("<CountryCode>{0}</CountryCode>", contact.CountryCode);
            data.AppendFormat("<CompanyName>{0}</CompanyName>", contact.CompanyName);
            data.AppendFormat("<JobTitle>{0}</JobTitle>", contact.JobTitle);
            data.AppendFormat("<WorkPhone>{0}</WorkPhone>", contact.WorkPhone);
            data.AppendFormat("<EmailType>{0}</EmailType>",
                              contact.EmailType == ContactEmailType.Undefined
                                  ? ContactEmailTypeHtml
                                  : emailTypeNames[contact.EmailType]);
            data.AppendFormat("<OptInSource>{0}</OptInSource>", optSourceNames[contact.OptInSource]);
            data.AppendFormat("<Note>{0}</Note>", contact.Note);
            data.AppendFormat("<CustomField1>{0}</CustomField1>", contact.CustomField1);
            data.AppendFormat("<CustomField2>{0}</CustomField2>", contact.CustomField2);
            data.AppendFormat("<CustomField3>{0}</CustomField3>", contact.CustomField3);
            data.AppendFormat("<CustomField4>{0}</CustomField4>", contact.CustomField4);
            data.AppendFormat("<CustomField5>{0}</CustomField5>", contact.CustomField5);
            data.AppendFormat("<CustomField6>{0}</CustomField6>", contact.CustomField6);
            data.AppendFormat("<CustomField7>{0}</CustomField7>", contact.CustomField7);
            data.AppendFormat("<CustomField8>{0}</CustomField8>", contact.CustomField8);
            data.AppendFormat("<CustomField9>{0}</CustomField9>", contact.CustomField9);
            data.AppendFormat("<CustomField10>{0}</CustomField10>", contact.CustomField10);
            data.AppendFormat("<CustomField11>{0}</CustomField11>", contact.CustomField11);
            data.AppendFormat("<CustomField12>{0}</CustomField12>", contact.CustomField12);
            data.AppendFormat("<CustomField13>{0}</CustomField13>", contact.CustomField13);
            data.AppendFormat("<CustomField14>{0}</CustomField14>", contact.CustomField14);
            data.AppendFormat("<CustomField15>{0}</CustomField15>", contact.CustomField15);

            data.Append("<ContactLists>");
            if (contact.ContactLists != null)
            {
                foreach (ContactOptInList cList in contact.ContactLists)
                {
                    data.AppendFormat("<ContactList id=\"{0}/{1}\"/>",
                                      Regex.Replace(accountContactListUri, "https://", "http://"), cList.ContactList.Id);
                }
            }
            data.Append("</ContactLists>");

            data.Append("</Contact>");
            data.Append("</content>");
            data.Append("</entry>");

            return data;
        }

        /// <summary>
        /// Create an Atom entry used to update a Contact
        /// </summary>
        /// <param name="contact">Contact to be updated</param>
        /// <param name="accountContactUri">Uri address of Account Owner Contact resource</param>
        /// <returns>Atom entry used to update the Contact
        /// <example>
        ///     <entry xmlns="http://www.w3.org/2005/Atom">
        ///         <title type="text"> </title>
        ///         <updated>2008-07-23T14:21:06.407Z</updated>
        ///         <author></author>
        ///         <id>http://api.constantcontact.com/ws/customers/joesflowers/contacts/1454</id>
        ///         <summary type="text">Contact</summary>
        ///         <content type="application/vnd.ctct+xml">
        ///             <Contact xmlns="http://ws.constantcontact.com/ns/1.0/">
        ///                 <OptInSource>ACTION_BY_CUSTOMER</OptInSource>
        ///                 <ContactLists>
        ///                 </ContactLists>
        ///             </Contact>
        ///         </content>
        ///     </entry>
        /// </example>
        /// </returns>
        private static StringBuilder CreateRemoveFromAllListsAtomEntry(Contact contact, string accountContactUri)
        {
            string contactId = Regex.Replace(String.Format(CultureInfo.InvariantCulture,
                                                           "{0}/{1}", accountContactUri, contact.Id),
                                             "https://", "http://");

            StringBuilder data = new StringBuilder();

            data.AppendFormat("<entry xmlns=\"{0}\">", AtomNamespace);
            data.Append("<title type=\"text\"></title>");
            data.Append("<updated>2008-07-23T14:21:06.407Z</updated>");
            data.Append("<author><name>Constant Contact</name></author>");
            data.AppendFormat("<id>{0}</id>", contactId);
            data.Append("<summary type=\"text\">Contact</summary>");
            data.Append("<content type=\"application/vnd.ctct+xml\">");
            data.AppendFormat("<Contact xmlns=\"{0}\" id=\"{1}\">", ConstantNamespace, contactId);
            data.AppendFormat("<EmailAddress>{0}</EmailAddress>", contact.EmailAddress);

            // set the opt in source
            data.AppendFormat("<OptInSource>{0}</OptInSource>", optSourceNames[ContactOptSource.ActionByCustomer]);

            data.Append("<ContactLists>");
            data.Append("</ContactLists>");

            data.Append("</Contact>");
            data.Append("</content>");
            data.Append("</entry>");

            return data;
        }

        /// <summary>
        /// Create an Atom entry used to update a Contact.
        /// Only the following fields will be updated: EmailAddress, FirstName, LastName, MiddleName, HomePhone, Addr1, Addr2, Addr3,
        /// City, StateCode, StateName, CountryCode, CountryName, PostalCode, SubPostalCode
        /// </summary>
        /// <param name="contact">Contact to be updated</param>
        /// <param name="apiUri">Uri address of Account Owner Contact resource</param>
        /// <param name="accountContactListUri">Uri address of Account Owner Contact List resource</param>
        /// <returns>Atom entry used to update the Contact
        /// <example>
        ///     <entry xmlns="http://www.w3.org/2005/Atom">
        ///         <title type="text"> </title>
        ///         <updated>2008-07-23T14:21:06.407Z</updated>
        ///         <author></author>
        ///         <id>http://api.constantcontact.com/ws/customers/joesflowers/contacts/1454</id>
        ///         <summary type="text">Contact</summary>
        ///         <content type="application/vnd.ctct+xml">
        ///             <Contact xmlns="http://ws.constantcontact.com/ns/1.0/">
        ///                 <EmailAddress>test101@example.com</EmailAddress>
        ///                 <FirstName>First</FirstName>
        ///                 <LastName>Last</LastName>
        ///                 <OptInSource>ACTION_BY_CUSTOMER</OptInSource>
        ///                 <ContactLists>
        ///                     <ContactList id="http://api.constantcontact.com/ws/customers/joesflowers/lists/1" />
        ///                 </ContactLists>
        ///             </Contact>
        ///         </content>
        ///     </entry>
        /// </example>
        /// </returns>
        private static StringBuilder CreateSmallUpdateAtomEntry(Contact contact, string apiUri, 
            string accountContactListUri)
        {
            // TBD!! - Change to use the contact.Link field
            string contactId = Regex.Replace(String.Format(CultureInfo.InvariantCulture,
                                                           "{0}{1}", apiUri, contact.Link),
                                             "https://", "http://");

            StringBuilder data = new StringBuilder();

            data.AppendFormat("<entry xmlns=\"{0}\">", AtomNamespace);
            data.Append("<title type=\"text\"></title>");
            data.Append("<updated>2008-07-23T14:21:06.407Z</updated>");
            data.Append("<author><name>Constant Contact</name></author>");
            data.AppendFormat("<id>{0}</id>", contactId);
            data.Append("<summary type=\"text\">Contact</summary>");
            data.Append("<content type=\"application/vnd.ctct+xml\">");
            data.AppendFormat("<Contact xmlns=\"{0}\" id=\"{1}\">", ConstantNamespace, contactId);
            data.AppendFormat("<EmailAddress>{0}</EmailAddress>", contact.EmailAddress);
            data.AppendFormat("<FirstName>{0}</FirstName>", contact.FirstName);
            data.AppendFormat("<LastName>{0}</LastName>", contact.LastName);
            data.AppendFormat("<MiddleName>{0}</MiddleName>", contact.MiddleName);

            // set the opt in source
            ContactOptSource optInSource = ContactOptSource.ActionByCustomer;
            if (contact.Status == ContactStatus.DoNotMail)
            {
                optInSource = ContactOptSource.ActionByContact;
            }
            data.AppendFormat("<OptInSource>{0}</OptInSource>", optSourceNames[optInSource]);
            data.AppendFormat("<HomePhone>{0}</HomePhone>", contact.HomePhone);
            data.AppendFormat("<Addr1>{0}</Addr1>", contact.AddressLine1);
            data.AppendFormat("<Addr2>{0}</Addr2>", contact.AddressLine2);
            data.AppendFormat("<Addr3>{0}</Addr3>", contact.AddressLine3);
            data.AppendFormat("<City>{0}</City>", contact.City);
            data.AppendFormat("<StateCode>{0}</StateCode>", contact.StateCode);
            data.AppendFormat("<StateName>{0}</StateName>", contact.StateName);
            data.AppendFormat("<CountryCode>{0}</CountryCode>", contact.CountryCode);
            data.AppendFormat("<CountryName>{0}</CountryName>", contact.CountryName);
            data.AppendFormat("<PostalCode>{0}</PostalCode>", contact.PostalCode);
            data.AppendFormat("<SubPostalCode>{0}</SubPostalCode>", contact.SubPostalCode);

            data.Append("<ContactLists>");
            if (contact.ContactLists != null)
            {
                foreach (ContactOptInList cList in contact.ContactLists)
                {
                    data.AppendFormat("<ContactList id=\"{0}/{1}\"/>",
                                      Regex.Replace(accountContactListUri, "https://", "http://"), cList.ContactList.Id);
                }
            }
            data.Append("</ContactLists>");

            data.Append("</Contact>");
            data.Append("</content>");
            data.Append("</entry>");

            return data;
        }

        /// <summary>
        /// Create an Atom entry used to update a Contact
        /// </summary>
        /// <param name="contact">Contact to be updated</param>
        /// <param name="accountContactUri">Uri address of Account Owner Contact resource</param>
        /// <param name="accountContactListUri">Uri address of Account Owner Contact List resource</param>
        /// <returns>Atom entry used to update the Contact
        /// <example>
        ///     <entry xmlns="http://www.w3.org/2005/Atom">
        ///         <title type="text"> </title>
        ///         <updated>2008-07-23T14:21:06.407Z</updated>
        ///         <author></author>
        ///         <id>http://api.constantcontact.com/ws/customers/joesflowers/contacts/1454</id>
        ///         <summary type="text">Contact</summary>
        ///         <content type="application/vnd.ctct+xml">
        ///             <Contact xmlns="http://ws.constantcontact.com/ns/1.0/">
        ///                 <EmailAddress>test101@example.com</EmailAddress>
        ///                 <FirstName>First</FirstName>
        ///                 <LastName>Last</LastName>
        ///                 <OptInSource>ACTION_BY_CUSTOMER</OptInSource>
        ///                 <ContactLists>
        ///                     <ContactList id="http://api.constantcontact.com/ws/customers/joesflowers/lists/1" />
        ///                 </ContactLists>
        ///             </Contact>
        ///         </content>
        ///     </entry>
        /// </example>
        /// </returns>
        private static StringBuilder CreateFullUpdateAtomEntry(Contact contact, string accountContactUri,
            string accountContactListUri)
        {
            string contactId = Regex.Replace(String.Format(CultureInfo.InvariantCulture,
                                                           "{0}{1}", accountContactUri, contact.Link),
                                             "https://", "http://");

            StringBuilder data = new StringBuilder();

            data.AppendFormat("<entry xmlns=\"{0}\">", AtomNamespace);
            data.Append("<title type=\"text\"></title>");
            data.Append("<updated>2008-07-23T14:21:06.407Z</updated>");
            data.Append("<author><name>Constant Contact</name></author>");
            data.AppendFormat("<id>{0}</id>", contactId);
            data.Append("<summary type=\"text\">Contact</summary>");
            data.Append("<content type=\"application/vnd.ctct+xml\">");
            data.AppendFormat("<Contact xmlns=\"{0}\" id=\"{1}\">", ConstantNamespace, contactId);
            data.AppendFormat("<EmailAddress>{0}</EmailAddress>", contact.EmailAddress);
            data.AppendFormat("<FirstName>{0}</FirstName>", contact.FirstName);
            data.AppendFormat("<LastName>{0}</LastName>", contact.LastName);
            data.AppendFormat("<MiddleName>{0}</MiddleName>", contact.MiddleName);
            data.AppendFormat("<OptInSource>{0}</OptInSource>", optSourceNames[contact.OptInSource]);
            data.AppendFormat("<HomePhone>{0}</HomePhone>", contact.HomePhone);
            data.AppendFormat("<Addr1>{0}</Addr1>", contact.AddressLine1);
            data.AppendFormat("<Addr2>{0}</Addr2>", contact.AddressLine2);
            data.AppendFormat("<Addr3>{0}</Addr3>", contact.AddressLine3);
            data.AppendFormat("<City>{0}</City>", contact.City);
            data.AppendFormat("<StateCode>{0}</StateCode>", contact.StateCode);
            data.AppendFormat("<StateName>{0}</StateName>", contact.StateName);
            data.AppendFormat("<CountryCode>{0}</CountryCode>", contact.CountryCode);
            data.AppendFormat("<CountryName>{0}</CountryName>", contact.CountryName);
            data.AppendFormat("<PostalCode>{0}</PostalCode>", contact.PostalCode);
            data.AppendFormat("<SubPostalCode>{0}</SubPostalCode>", contact.SubPostalCode);

            data.AppendFormat("<EmailType>{0}</EmailType>",
                              contact.EmailType == ContactEmailType.Undefined
                                  ? ContactEmailTypeHtml
                                  : emailTypeNames[contact.EmailType]);
            data.AppendFormat("<WorkPhone>{0}</WorkPhone>", contact.WorkPhone);
            data.AppendFormat("<JobTitle>{0}</JobTitle>", contact.JobTitle);
            data.AppendFormat("<CompanyName>{0}</CompanyName>", contact.CompanyName);
            data.AppendFormat("<Note>{0}</Note>", contact.Note);
            data.AppendFormat("<CustomField1>{0}</CustomField1>", contact.CustomField1);
            data.AppendFormat("<CustomField2>{0}</CustomField2>", contact.CustomField2);
            data.AppendFormat("<CustomField3>{0}</CustomField3>", contact.CustomField3);
            data.AppendFormat("<CustomField4>{0}</CustomField4>", contact.CustomField4);
            data.AppendFormat("<CustomField5>{0}</CustomField5>", contact.CustomField5);
            data.AppendFormat("<CustomField6>{0}</CustomField6>", contact.CustomField6);
            data.AppendFormat("<CustomField7>{0}</CustomField7>", contact.CustomField7);
            data.AppendFormat("<CustomField8>{0}</CustomField8>", contact.CustomField8);
            data.AppendFormat("<CustomField9>{0}</CustomField9>", contact.CustomField9);
            data.AppendFormat("<CustomField10>{0}</CustomField10>", contact.CustomField10);
            data.AppendFormat("<CustomField11>{0}</CustomField11>", contact.CustomField11);
            data.AppendFormat("<CustomField12>{0}</CustomField12>", contact.CustomField12);
            data.AppendFormat("<CustomField13>{0}</CustomField13>", contact.CustomField13);
            data.AppendFormat("<CustomField14>{0}</CustomField14>", contact.CustomField14);
            data.AppendFormat("<CustomField15>{0}</CustomField15>", contact.CustomField15);

            data.Append("<ContactLists>");
            if (contact.ContactLists != null)
            {
                foreach (ContactOptInList cList in contact.ContactLists)
                {
                    data.AppendFormat("<ContactList id=\"{0}/{1}\"/>",
                                      Regex.Replace(accountContactListUri, "https://", "http://"), cList.ContactList.Id);

                }
            }
            data.Append("</ContactLists>");

            data.Append("</Contact>");
            data.Append("</content>");
            data.Append("</entry>");

            return data;
        }
        #endregion
    }
}