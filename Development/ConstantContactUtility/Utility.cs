using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using ConstantContactUtility.Components;
using Activity = ConstantContactBO.Activity;
using Contact = ConstantContactBO.Contact;
using ContactList = ConstantContactBO.ContactList;
using Email = ConstantContactBO.Entities.Email;
using EmailCampaign = ConstantContactBO.Entities.EmailCampaign;
using Folder = ConstantContactBO.Folder;
using Image = ConstantContactBO.Image;
using ListMember = ConstantContactBO.ListMember;

namespace ConstantContactUtility
{
    /// <summary>
    /// Provides utility methods to create a new Contact, update an existing one, etc.
    /// 
    /// </summary>
    public class Utility
    {
        #region Constants
        /// <summary>
        /// Constant Contact server error message for error code 404
        /// </summary>
        private const string WebExceptionCode404Message = "The remote server returned an error: (404) Not Found.";
        /// <summary>
        /// Constant Contact server error message for error code 403
        /// </summary>
        private const string WebExceptionCode403Message = "The remote server returned an error: (403) Forbidden.";
        /// <summary>
        /// Constant Contact server error message for error code 409
        /// </summary>
        private const string WebExceptionCode409Message = "The remote server returned an error: (409) Conflict.";
        #endregion

        #region User authentication
        /// <summary>
        /// Verify user authentication
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <exception cref="ConstantAuthenticationException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server 
        /// or if ApiKey, Username or Password are null or empty</exception>
        public static void IsValidUserAuthentication(AuthenticationData authenticationData)
        {
            ValidateAuthenticationData(authenticationData);

            try
            {
                // try to access the Service Document resource
                // it will throw a WebException if Constant Contact credentials are invalid

                GetResponseStream(new Uri(authenticationData.AccountServiceDocumentUri), authenticationData);
            }
            catch (Exception e)
            {
                throw new ConstantAuthenticationException("Account authentication failed", e,
                                                          authenticationData.Username);
            }
        }
        #endregion



        #region Contact - Retrieve entire collection -
        /// <summary>
        /// Retrieves the first chunk collection of Contacts that the server provides        
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <remarks>Constant Contact server provides paged collections</remarks>
        /// <param name="nextChunkId">Link to the next chunk of data</param>
        /// <exception cref="ConstantException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server 
        /// or if ApiKey, Username or Password are null or empty</exception>
        /// <returns>The collection of Contacts</returns>
        public static IList<Contact> GetContactCollection(AuthenticationData authenticationData, out string nextChunkId)
        {
            return GetContactCollection(authenticationData, null, out nextChunkId);
        }

        /// <summary>
        /// Retrieves the collection of Contacts returned by server at the current chunk Id
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <remarks>Constant Contact server provides paged collections</remarks>
        /// <param name="currentChunkId">Link to the current chunk data</param>
        /// <param name="nextChunkId">Link to the next chunk of data</param>
        /// <exception cref="ConstantException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server 
        /// or if ApiKey, Username or Password are null or empty</exception>
        /// <returns>The collection of Contacts</returns>
        public static IList<Contact> GetContactCollection(AuthenticationData authenticationData, string currentChunkId, out string nextChunkId)
        {
            ValidateAuthenticationData(authenticationData);

            string currentAddress = String.Format(CultureInfo.InvariantCulture, "{0}{1}",
                                                  authenticationData.AccountContactsUri, currentChunkId);

            Stream stream = Stream.Null;
            try
            {
                // get the response stream
                stream = GetResponseStream(new Uri(currentAddress), authenticationData);

                // parse the stream and get a collection of Contacts
                return ContactComponent.GetContactCollection(stream, out nextChunkId);
            }
            catch (Exception e)
            {
                throw new ConstantException(e.Message, e);
            }
            finally
            {
                // close the response stream
                stream.Close();
            }
        }
        #endregion

        #region Contact - Search -
        /// <summary>
        /// Retrieves the first chunk collection of Contacts that match specified Email Addresses
        /// </summary>
        /// <remarks>Constant Contact server provides paged collections</remarks>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="emailAddresses">One or more Email Addresses</param>
        /// <param name="nextChunkId">Link to the next chunk of data</param>
        /// <exception cref="ConstantException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server 
        /// or if ApiKey, Username or Password are null or empty</exception>
        /// <returns>The collection of Contacts</returns>
        public static IList<Contact> SearchContactByEmail(AuthenticationData authenticationData, IEnumerable<string> emailAddresses, out string nextChunkId)
        {
            ValidateAuthenticationData(authenticationData);

            return SearchContactByEmail(authenticationData, emailAddresses, null, out nextChunkId);
        }

        /// <summary>
        /// Retrieves the collection of Contacts that match specified Email Addresses, returned by the server at current chunk Id.
        /// Entire collection of Contacts will be returned if no Email Address is specified
        /// </summary>
        /// <remarks>Constant Contact server provides paged collections</remarks>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="emailAddresses">One or more Email Addresses</param>
        /// <param name="currentChunkId">Link to the current chunk data</param>
        /// <param name="nextChunkId">Link to the next chunk of data</param>
        /// <exception cref="ConstantException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server 
        /// or if ApiKey, Username or Password are null or empty</exception>
        /// <returns>The collection of Contacts</returns>
        public static List<Contact> SearchContactByEmail(AuthenticationData authenticationData, IEnumerable<string> emailAddresses, string currentChunkId, out string nextChunkId)
        {
            if (null == emailAddresses)
            {
                throw new ArgumentNullException("emailAddresses");
            }

            // create the Uri address with the Email address query
            StringBuilder uriAddress = new StringBuilder();
            uriAddress.Append(authenticationData.AccountContactsUri);
            uriAddress.Append("?");
            // loop the Email Address and create the query
            foreach (string email in emailAddresses)
            {
                uriAddress.AppendFormat("email={0}&", HttpUtility.UrlEncode(email.ToLower(CultureInfo.CurrentCulture)));
            }
            // remove the last '&' character from the query
            uriAddress.Remove(uriAddress.Length - 1, 1);

            string currentAddress = String.Format(CultureInfo.InvariantCulture, "{0}{1}",
                                                  uriAddress, currentChunkId);

            Stream stream = Stream.Null;
            try
            {
                // get the response stream
                stream = GetResponseStream(new Uri(currentAddress), authenticationData);

                // parse the stream and get a collection of Contacts
                return ContactComponent.GetContactCollection(stream, out nextChunkId);
            }
            catch (Exception e)
            {
                throw new ConstantException(e.Message, e);
            }
            finally
            {
                // close the response stream
                stream.Close();
            }
        }
        #endregion

        #region Contact - Retrieve details -
        /// <summary>
        /// Retrieve an individual Contact by its Id
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="id">Contact Id</param>        
        /// <exception cref="ConstantException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server or if no Contact with specified Id exists
        /// or if ApiKey, Username or Password are null or empty</exception>
        /// <returns>Contact with specified Id</returns>
        public static Contact GetContactDetailsById(AuthenticationData authenticationData, string id)
        {
            ValidateAuthenticationData(authenticationData);

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Contact Id cannot be null or empty", "id");
            }

            // create the URI for specified Contact Id
            string completeUri = String.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                               authenticationData.AccountContactsUri, id);

            // get the response stream
            Stream stream = Stream.Null;
            try
            {
                stream = GetResponseStream(new Uri(completeUri), authenticationData);

                // parse the stream and obtain a Contact object
                return ContactComponent.GetContactDetails(stream);
            }
            catch (Exception e)
            {
                if (string.Compare(e.Message, WebExceptionCode404Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "Contact with Id '{0}' does not exist.", id));
                }

                throw new ConstantException(e.Message, e);
            }
            finally
            {
                // close the response stream
                stream.Close();
            }
        }
        #endregion

        #region Contact - Create new -
        /// <summary>
        /// Create a New Contact
        /// </summary>        
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="contact">Contact to be created</param>        
        /// <remarks>The POST data presents only values for EmailAddress, FirstName, LastName, OptInSource and ContactLists elements</remarks>        
        /// <exception cref="ArgumentNullException">Thrown if specified Contact is null</exception>
        /// <exception cref="ArgumentException">Thrown if E-mail Address of specified Contact is null or empty</exception>
        /// <exception cref="ConstantException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server or if specified Contact does not belongs to any list
        /// or if ApiKey, Username or Password are null or empty</exception>
        /// <returns>Newly created Contact</returns>
        public static Contact CreateNewContact(AuthenticationData authenticationData, Contact contact)
        {
            ValidateAuthenticationData(authenticationData);

            if (null == contact)
            {
                throw new ArgumentNullException("contact");
            }

            if (string.IsNullOrEmpty(contact.EmailAddress))
            {
                throw new ArgumentException("Contact E-mail Address cannot be null or empty", "contact");
            }

            if (null == contact.ContactLists
                || contact.ContactLists.Count == 0)
            {
                throw new ConstantException("Contact does not belongs to any contact list");
            }

            // get the Atom entry for specified Contact
            StringBuilder data = ContactComponent.CreateNewContact(contact, authenticationData.AccountContactListsUri);

            Stream stream = Stream.Null;
            try
            {
                // post the Atom entry at specified Uri and save the response stream
                stream = PostInformation(authenticationData, new Uri(authenticationData.AccountContactsUri),
                                         data.ToString());

                // return newly created Contact
                return ContactComponent.GetContactDetails(stream);
            }
            catch (Exception e)
            {
                throw new ConstantException(e.Message, e);
            }
            finally
            {
                // close the response stream
                stream.Close();
            }
        }

        /// <summary>
        /// POST the data at the specified Uri address and returns the response Stream
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="address">Uri address</param>
        /// <param name="data">Data to be send at specified Uri address</param>
        /// <returns>Response Stream</returns>
        private static Stream PostInformation(AuthenticationData authenticationData, Uri address, string data)
        {
            // set the Http request content type
            const string contentType = @"application/atom+xml";

            // send a Http POST request and return the response Stream
            return GetResponseStream(authenticationData, address, WebRequestMethods.Http.Post, contentType, data);
        }
        #endregion

        #region Contact - Update -
        /// <summary>
        /// Update a Contact using the full form. All Contact fields will be updated
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="contact">Contact to be updated</param>
        /// <exception cref="ArgumentNullException">Thrown if specified Contact is null</exception>
        /// <exception cref="ArgumentException">Thrown if Id or Email Address of specified Contact is null or empty</exception>        
        /// <exception cref="ConstantException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server, if no Contact with specified Id exists 
        /// or if Contact cannot be updated (it belongs to the Do-Not-Mail list)
        /// or if ApiKey, Username or Password are null or empty</exception>        
        public static void UpdateContactFullForm(AuthenticationData authenticationData, Contact contact)
        {
            UpdateContact(authenticationData, contact);
        }

        /// <summary>
        /// Update a Contact
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="contact">Contact to be updated</param>
        /// <exception cref="ArgumentNullException">Thrown if specified Contact is null</exception>
        /// <exception cref="ArgumentException">Thrown if Id or Email Address of specified Contact is null or empty</exception>        
        /// <exception cref="ConstantException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server, if no Contact with specified Id exists 
        /// or if Contact cannot be updated (it belongs to the Do-Not-Mail list)
        /// or if ApiKey, Username or Password are null or empty</exception>        
        private static void UpdateContact(AuthenticationData authenticationData, Contact contact)
        {
            bool fullUpdate = true;
            ValidateAuthenticationData(authenticationData);

            if (null == contact)
            {
                throw new ArgumentNullException("contact");
            }

            if (string.IsNullOrEmpty(contact.Id))
            {
                throw new ArgumentException("Contact Id cannot be null or empty", "contact");
            }

            if (string.IsNullOrEmpty(contact.EmailAddress))
            {
                throw new ArgumentException("Contact Email Address cannot be null or empty", "contact");
            }

            // create the URI for specified Contact Id
            string completeUri = String.Format(CultureInfo.InvariantCulture, "{0}{1}",
                                               AuthenticationData.HostAddress, contact.Link);

            // get the Atom entry for specified Contact
            StringBuilder data = ContactComponent.UpdateContact(contact, authenticationData.ApiRootUri,
                                                                authenticationData.AccountContactListsUri, fullUpdate);

            try
            {
                // put the Atom entry at specified Uri
                PutInformation(authenticationData, new Uri(completeUri), data.ToString());
            }
            catch (Exception e)
            {
                if (string.Compare(e.Message, WebExceptionCode404Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "Contact with Id '{0}' does not exist.", contact.Id));
                }
                if (string.Compare(e.Message, WebExceptionCode403Message) == 0)
                {
                    throw new ConstantException("Contact cannot be updated. It belongs to the Do-Not-Mail list.");
                }

                throw new ConstantException(e.Message, e);
            }
        }
        /// <summary>
        /// PUT the data at the specified Uri address. 
        /// Constant Contact server will not send any response
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="address">Uri address</param>
        /// <param name="data">Data to be send at specified Uri address</param>        
        private static void PutInformation(AuthenticationData authenticationData, Uri address, string data)
        {
            // set the Http request content type
            const string contentType = @"application/atom+xml";

            // send a Http PUT request and return the response Stream
            GetResponseStream(authenticationData, address, WebRequestMethods.Http.Put, contentType, data);
        }
        #endregion

        #region Contact - Unsubscribe -
        /// <summary>
        /// Opting-out ("Unsubscribe") a Contact
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="contactId">Contact Id</param>
        /// <exception cref="ArgumentException">Thrown if Id of specified Contact is null or empty</exception>        
        /// <exception cref="ConstantException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server or if no Contact with specified Id exists
        /// or if ApiKey, Username or Password are null or empty</exception>
        /// <remarks>Opted-out Contacts become members of the Do-Not-Mail special list</remarks>
        public static void UnsubscribeContact(AuthenticationData authenticationData, string contactId)
        {
            ValidateAuthenticationData(authenticationData);

            if (string.IsNullOrEmpty(contactId))
            {
                throw new ArgumentException("Contact Id cannot be null or empty", "contactId");
            }

            // create the URI for specified Contact List Id
            string completeUri = String.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                               authenticationData.AccountContactsUri, contactId);

            try
            {
                // issue a Http DELETE and specified Uri            
                DeleteInformation(authenticationData, new Uri(completeUri));
            }
            catch (Exception e)
            {
                // possible that the Contact does not exist any more
                if (string.Compare(e.Message, WebExceptionCode404Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "Contact with Id '{0}' does not exist.", contactId));
                }

                throw new ConstantException(e.Message, e);
            }
        }

        /// <summary>
        /// Sends a Http DELETE request at the specified Uri address
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="address">Uri address</param>
        private static void DeleteInformation(AuthenticationData authenticationData, Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
            request.Credentials = CreateCredentialCache(address, authenticationData);

            request.Method = "DELETE";

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                Console.WriteLine(String.Format(CultureInfo.InvariantCulture,
                                                "Method {0}, response description: {1}", request.Method,
                                                response.StatusDescription));
            }
            finally
            {
                if (response != null)
                {
                    // close the response
                    response.Close();
                }
            }
        }
        #endregion

        #region Contact - Remove from all lists -
        /// <summary>
        /// Remove Contact from all Contact Lists
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="contactId">Contact Id</param>
        /// <exception cref="ArgumentException">Thrown if Id of specified Contact is null or empty</exception>        
        /// <exception cref="ConstantException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server or if no Contact with specified Id exists
        /// or if ApiKey, Username or Password are null or empty</exception>
        public static void RemoveContactFromAllLists(AuthenticationData authenticationData, string contactId)
        {
            ValidateAuthenticationData(authenticationData);

            if (string.IsNullOrEmpty(contactId))
            {
                throw new ArgumentException("Contact Id cannot be null or empty", "contactId");
            }

            // create the URI for specified Contact Id
            string completeUri = String.Format(CultureInfo.InvariantCulture, "{0}/{1}",
                                               authenticationData.AccountContactsUri, contactId);

            // get Contact by Id
            Contact contact = GetContactDetailsById(authenticationData, contactId);

            // consider that Contact does not needs to be updated
            bool needUpdate = false;

            if (contact.ContactLists.Count != 0)
            {
                // remove Contact from all Contact Lists
                contact.ContactLists.Clear();

                // Contact must be updated
                needUpdate = true;
            }

            if (!needUpdate)
            {
                // no need to update Contact
                return;
            }

            // get the Atom entry for specified Contact
            StringBuilder data = ContactComponent.RemoveContactFromAllLists(contact,
                                                                            authenticationData.AccountContactsUri);

            try
            {
                // put the Atom entry at specified Uri
                PutInformation(authenticationData, new Uri(completeUri), data.ToString());
            }
            catch (Exception e)
            {
                // possible that the Contact does not exist any more; not sure if this could happened
                if (string.Compare(e.Message, WebExceptionCode404Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "Contact with Id '{0}' does not exist.", contactId));
                }

                throw new ConstantException(e.Message, e);
            }
        }
        #endregion


        #region Contact List - Retrieve entire collection -
        /// <summary>
        /// Retrieves the first chunk collection of user Contact Lists that the server provides 
        /// for current Contact Account Owner.
        /// The collection is sorted by the Sort Order and it will not include the system 
        /// predefined lists ("Active", "Removed", "DoNotEmail")
        /// </summary>
        /// <remarks>Constant Contact server provides paged collections</remarks>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="nextChunkId">Link to the next chunk data</param>
        /// <exception cref="ConstantException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server 
        /// or if ApiKey, Username or Password are null or empty</exception>        
        /// <returns>The collection of user Contact Lists</returns>
        public static IList<ContactList> GetUserContactListCollection(AuthenticationData authenticationData, out string nextChunkId)
        {
            ValidateAuthenticationData(authenticationData);

            return GetUserContactListCollection(authenticationData, null, out nextChunkId);
        }

        /// <summary>
        /// Retrieves the collection of user Contact Lists returned by the server at current chunk Id.        
        /// The collection is sorted by the Sort Order and it will not include the system 
        /// predefined lists ("Active", "Removed", "DoNotEmail")
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="currentChunkId">Link to the current chunk data</param>
        /// <param name="nextChunkId">Link to the next chunk of data</param>
        /// <exception cref="ConstantException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server
        /// or if ApiKey, Username or Password are null or empty</exception>
        /// <returns>The collection of user Contact Lists</returns>
        public static IList<ContactList> GetUserContactListCollection(AuthenticationData authenticationData, string currentChunkId, out string nextChunkId)
        {
            // get the collection of Contact Lists
            IList<ContactList> list = GetContactListCollection(authenticationData, currentChunkId, out nextChunkId);

            IList<ContactList> nonSystemList = new List<ContactList>();

            foreach (ContactList contactList in list)
            {
                if (!contactList.IsSystemList)
                {
                    nonSystemList.Add(contactList);
                }
            }

            return nonSystemList;
        }

        /// <summary>
        /// Retrieves the collection of Contact Lists returned by the server at current chunk Id.
        /// The collection is sorted by the Sort Order and it will include the system 
        /// predefined lists ("Active", "Removed", "DoNotEmail")
        /// </summary>
        /// <remarks>Constant Contact server provides paged collections</remarks>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="currentChunkId">Link to the current chunk data</param>
        /// <param name="nextChunkId">Link to the next chunk of data</param>
        /// <exception cref="ConstantException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server 
        /// or if ApiKey, Username or Password are null or empty</exception>
        /// <returns>The collection of Contact Lists</returns>
        private static IList<ContactList> GetContactListCollection(AuthenticationData authenticationData, string currentChunkId, out string nextChunkId)
        {
            string currentAddress = String.Format(CultureInfo.InvariantCulture, "{0}{1}",
                                                  authenticationData.AccountContactListsUri, currentChunkId);

            Stream stream = Stream.Null;
            try
            {
                // get the response stream
                stream = GetResponseStream(new Uri(currentAddress), authenticationData);

                // parse the stream and get a collection of Contact Lists
                return ContactListComponent.GetContactListsCollection(stream, out nextChunkId);
            }
            catch (Exception e)
            {
                throw new ConstantException(e.Message, e);
            }
            finally
            {
                // close the response stream
                stream.Close();
            }
        }
        #endregion

        #region Contact List - Retrieve List Members -
        /// <summary>
        /// Retrieves all list members of selected node.
        /// </summary>
        /// <remarks>Constant Contact server provides paged collections</remarks>
        /// <param name="link">Link to target list</param>   
        /// <param name="Authdata">Authentication data (username, password and API Key)</param>
        /// <param name="nextChunk">Link to the next chunk data</param>       
        /// <returns>The collection of Contact List Members</returns>
        public static IList<Contact> getListMembers(string link, AuthenticationData Authdata, out string nextChunk)
        {
            if (string.IsNullOrEmpty(link))
            {
                throw new ArgumentException("list link cannot be null or empty", "link");
            }
            //Create new xmldocument, create memberslist uri, GET, load XML.
            XmlDocument xDoc = new XmlDocument();
            //If initial chunk, append /members to URI, if next link, no need to append /members, use link as is.
            string membersURI;
            if (link.Contains("next") == true)
            {
                membersURI = link;
            }
            else
            {
                membersURI = "https://api.constantcontact.com" + link + "/members";
            }
            try
            {
                xDoc.LoadXml(Utility.httpGet(Authdata, membersURI));
                //Define namespaces
                XmlNamespaceManager xnsmgr = new XmlNamespaceManager(xDoc.NameTable);
                xnsmgr.AddNamespace("ns1", "http://www.w3.org/2005/Atom");
                xnsmgr.AddNamespace("ns2", "http://ws.constantcontact.com/ns/1.0/");
                //Check for link to next chunk of data. If no next chunk, return empty string.
                nextChunk = "";
                XmlNodeList xnlLinks = xDoc.SelectNodes("//ns1:link", xnsmgr);
                foreach (XmlNode xnLink in xnlLinks)
                {
                    if (xnLink.Attributes["rel"] != null)
                    {
                        if (xnLink.Attributes["rel"].Value == "next")
                        {
                            nextChunk = xnLink.Attributes["href"].Value;
                        }
                    }
                }
                //Select nodes 
                XmlNodeList xnlListMembers = xDoc.SelectNodes("//ns2:ContactListMember", xnsmgr);
                //parse XML for ID, EMail address, Name info
                IList<Contact> listmembers = new List<Contact>();
                foreach (XmlNode xnMember in xnlListMembers)
                {
                    XmlNode xnLink = xnMember.SelectSingleNode("../../ns1:link", xnsmgr);
                    string sLink = xnLink.Attributes["href"].Value;

                    XmlNode xnID = xnMember.SelectSingleNode("../../ns1:id", xnsmgr);
                    string sID = xnID.InnerText;

                    XmlNode xnEmailAddress = xnMember.SelectSingleNode("ns2:EmailAddress", xnsmgr);
                    string sEmailAddress = xnEmailAddress.InnerText;

                    XmlNode xnName = xnMember.SelectSingleNode("ns2:Name", xnsmgr);
                    string sName = xnName.InnerText;

                    Contact listmember = new Contact();
                    listmember.Link = sLink;
                    listmember.Id = sID;
                    listmember.Name = sName;
                    listmember.EmailAddress = sEmailAddress;
                    listmembers.Add(listmember);
                }
                //return list of listmembers
                return listmembers;
            }
            catch (Exception e)
            {
                if (string.Compare(e.Message, WebExceptionCode404Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "Link to '{0}' does not exist.", link));
                }

                throw new ConstantException(e.Message, e);
            }
        }
        #endregion

        #region Contact List - Delete List -
        /// <summary>
        /// Delete a Contact List specified by list ID
        /// </summary>
        /// <param name="authdata">Authentication data (username, password and API Key)</param>
        /// <param name="listID">ID of target list to delete</param> 
        public static void deleteList(AuthenticationData authdata, string listID)
        {
            if (string.IsNullOrEmpty(listID))
            {
                throw new ArgumentException("List Id cannot be null or empty", "id");
            }
            try
            {
                httpDelete(authdata, authdata.AccountContactListsUri + "/" + listID);
            }
            catch (Exception e)
            {
                if (string.Compare(e.Message, WebExceptionCode404Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "List with ID '{0}' does not exist.", listID));
                }

                throw new ConstantException(e.Message, e);
            }
        }
        #endregion

        #region Contact List - Create New List -
        /// <summary>
        /// Create a new Contact List
        /// </summary>
        /// <param name="authdata">Authentication data (username, password and API Key)</param>
        /// <param name="name">Name of list to be created</param> 
        /// <param name="optindefault">determines if list is the default opt in list</param> 
        public static void addList(string name, bool optindefault, AuthenticationData authdata)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("List name cannot be null or empty", "name");
            }
            StringBuilder XMLData = new StringBuilder();
            XMLData.Append("<entry xmlns=\"http://www.w3.org/2005/Atom\">");
            XMLData.Append("<id>data:,</id>");
            XMLData.Append("<title />");
            XMLData.Append("<author />");
            XMLData.Append("<updated>2008-04-16</updated>");
            XMLData.Append("<content type=\"application/vnd.ctct+xml\">");
            XMLData.Append("<ContactList xmlns=\"http://ws.constantcontact.com/ns/1.0/\">");
            XMLData.Append("<OptInDefault>" + optindefault.ToString() + "</OptInDefault>");
            XMLData.Append("<Name>" + name + "</Name>");
            XMLData.Append("<SortOrder>99</SortOrder>");
            XMLData.Append("</ContactList>");
            XMLData.Append("</content>");
            XMLData.Append("</entry>");
            try
            {
                httpPost(authdata, authdata.AccountContactListsUri, XMLData.ToString());
            }
            catch (Exception e)
            {
                if (string.Compare(e.Message, WebExceptionCode409Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "List with name '{0}' already exists.", name));
                }

                throw new ConstantException(e.Message, e);
            }
        }

        /// <summary>
        /// Create a new Contact List
        /// </summary>
        /// <param name="authdata">Authentication data (username, password and API Key)</param>
        /// <param name="name">Name of list to be created</param> 
        /// <param name="sortOrder">Order that the list will show in the Constant Contact UI</param>
        /// <param name="optindefault">determines if list is the default opt in list</param> 
        public static void addList(string name, bool optindefault, int sortOrder, AuthenticationData authdata)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("List name cannot be null or empty", "name");
            }
            StringBuilder XMLData = new StringBuilder();
            XMLData.Append("<entry xmlns=\"http://www.w3.org/2005/Atom\">");
            XMLData.Append("<id>data:,</id>");
            XMLData.Append("<title />");
            XMLData.Append("<author />");
            XMLData.Append("<updated>2008-04-16</updated>");
            XMLData.Append("<content type=\"application/vnd.ctct+xml\">");
            XMLData.Append("<ContactList xmlns=\"http://ws.constantcontact.com/ns/1.0/\">");
            XMLData.Append("<OptInDefault>" + optindefault.ToString() + "</OptInDefault>");
            XMLData.Append("<Name>" + name + "</Name>");
            XMLData.Append("<SortOrder>" + sortOrder.ToString() + "</SortOrder>");
            XMLData.Append("</ContactList>");
            XMLData.Append("</content>");
            XMLData.Append("</entry>");
            try
            {
                httpPost(authdata, authdata.AccountContactListsUri, XMLData.ToString());
            }
            catch (Exception e)
            {
                if (string.Compare(e.Message, WebExceptionCode409Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "List with name '{0}' already exists.", name));
                }

                throw new ConstantException(e.Message, e);
            }
        }
        #endregion

        #region Contact List - Update Existing List
        /// <summary>
        /// Update an existing ContactList
        /// </summary>
        /// <param name="list">ContactList object to be updated</param>
        /// <param name="authdata">Authentication Data for the Constant Contact account</param>
        public static void UpdateList(ContactList list, AuthenticationData authdata)
        {
            if (list == null)
            {
                throw new ArgumentException("List cannot be null or empty", "name");
            }
            StringBuilder XMLData = new StringBuilder();
            XMLData.Append("<entry xmlns=\"http://www.w3.org/2005/Atom\">");
            XMLData.Append("<id>" + "http://api.constantcontact.com" + list.Link + "</id>");
            XMLData.Append("<title />");
            XMLData.Append("<author />");
            XMLData.Append("<updated>2008-04-16</updated>");
            XMLData.Append("<content type=\"application/vnd.ctct+xml\">");
            XMLData.Append("<ContactList xmlns=\"http://ws.constantcontact.com/ns/1.0/\">");
            XMLData.Append("<OptInDefault>" + list.OptInDefault.ToString() + "</OptInDefault>");
            XMLData.Append("<Name>" + list.Name + "</Name>");
            XMLData.Append("<SortOrder>" + list.SortOrder.ToString() + "</SortOrder>");
            XMLData.Append("</ContactList>");
            XMLData.Append("</content>");
            XMLData.Append("</entry>");

            Uri contactUri = new Uri(authdata.ApiRootUri + list.Link);
            try
            {
                PutInformation(authdata, contactUri, XMLData.ToString());
            }
            catch (Exception e)
            {
                if (string.Compare(e.Message, WebExceptionCode409Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "List with name '{0}' already exists.", list.Name));
                }

                throw new ConstantException(e.Message, e);
            }
        }
        #endregion

        #region EmailCampaign - Retrieve entire collection
        /// <summary>
        /// Retrieves the collection of Emails returned by server
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <exception cref="ConstantException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server 
        /// or if ApiKey, Username or Password are null or empty</exception>
        /// <returns>The collection of Emails</returns>
        public static IList<Email> GetEmailCollection(AuthenticationData authenticationData, string currentChunkId, out string nextChunkId)
        {
            ValidateAuthenticationData(authenticationData);

            string currentAddress = String.Format(CultureInfo.InvariantCulture, "{0}{1}",
                                                  authenticationData.AccountEmailsListUri, currentChunkId);

            Stream stream = Stream.Null;
            try
            {
                // get the response stream
                stream = GetResponseStream(new Uri(currentAddress), authenticationData);

                // parse the stream and get a collection of Emails
                return EmailComponent.GetEmailCollection(stream, out nextChunkId);
            }
            catch (Exception e)
            {
                throw new ConstantException(e.Message, e);
            }
            finally
            {
                // close the response stream
                stream.Close();
            }
        }

        #endregion

        #region EmailCampaign - Create new -
        /// <summary>
        /// Create a New EmailCampaign
        /// </summary>        
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="campaign">Email Campaign to be created</param>        
        /// <exception cref="ArgumentNullException">Thrown if specified EmailCampaign is null</exception>
        /// <exception cref="ConstantException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server or if ApiKey, Username or Password are null or empty</exception>
        /// <returns>Newly created EmailCampaign</returns>
        public static EmailCampaign CreateNewEmailCampaign(AuthenticationData authenticationData, EmailCampaign campaign)
        {
            ValidateAuthenticationData(authenticationData);

            if (null == campaign)
            {
                throw new ArgumentNullException("campaign");
            }

            //if (null == campaign.ContactLists
            //    || campaign.ContactLists.Count == 0)
            //{
            //    throw new ConstantException("Contact does not belongs to any contact list");
            //}

            // get the Atom entry for specified Contact
            StringBuilder data = EmailCampaignComponent.CreateNewEmailCampaign(campaign, authenticationData);

            //TextReader tr = new StreamReader("d:\\add.xml");
            //TextReader tr = new StreamReader("d:\\add_campaign.xml");
            //string test = tr.ReadToEnd();
            //tr.Close();

            Stream stream = Stream.Null;
            try
            {
                // post the Atom entry at specified Uri and save the response stream
                stream = PostInformation(authenticationData, new Uri(authenticationData.AccountEmailCampaignsListUri),
                                         data.ToString());

                // return newly created EmailCampaign
                //return EmailCampaignComponent.GetContactDetails(stream);
                return null;
            }
            catch (Exception e)
            {
                throw new ConstantException(e.Message, e);
            }
            finally
            {
                // close the response stream
                stream.Close();
            }
        }
        #endregion

        #region EmailCampaign - Search By Status -
        /// <summary>
        /// Gets a list of email campaigns filtered by status
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="status">campaign status</param>
        /// <returns>filtered email campaigns</returns>
        public static List<EmailCampaign> GetEmailCampaignCollection(AuthenticationData authenticationData, string status)
        {
            string currentAddress = string.Format("{0}?status={1}", authenticationData.AccountEmailCampaignsListUri, status);

            Stream stream = Stream.Null;
            try
            {
                // get the response stream
                stream = GetResponseStream(new Uri(currentAddress), authenticationData);

                // parse the stream and get a collection of Contact Lists
                return EmailCampaignComponent.GetEmailCampaignCollection(stream);
            }
            catch (Exception e)
            {
                throw new ConstantException(e.Message, e);
            }
            finally
            {
                // close the response stream
                stream.Close();
            }
        }

        /// <summary>
        /// Gets a list of email campaigns
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <returns>email campaigns list</returns>
        public static List<EmailCampaign> GetEmailCampaignCollection(AuthenticationData authenticationData)
        {
            string currentAddress = authenticationData.AccountEmailCampaignsListUri;

            Stream stream = Stream.Null;
            try
            {
                // get the response stream
                stream = GetResponseStream(new Uri(currentAddress), authenticationData);

                // parse the stream and get a collection of Email Campaigns
                return EmailCampaignComponent.GetEmailCampaignCollection(stream);
            }
            catch (Exception e)
            {
                throw new ConstantException(e.Message, e);
            }
            finally
            {
                // close the response stream
                stream.Close();
            }
        }
        #endregion

        #region EmailCampaign - Remove entity -
        /// <summary>
        /// Sends a Http DELETE request at the specified Uri address to delete an Email Campaign
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="id">campaign id</param>
        public static void DeleteEmailCampaign(AuthenticationData authenticationData, string id)
        {
            ValidateAuthenticationData(authenticationData);

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Email Campaign Id cannot be null or empty", "id");
            }

            // create the URI for specified Email Campaign Id
            string completeUri = String.Format(CultureInfo.InvariantCulture, "{0}/{1}", authenticationData.AccountEmailCampaignsListUri, id);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(completeUri);
            request.Credentials = CreateCredentialCache(new Uri(completeUri), authenticationData);

            request.Method = "DELETE";

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                Console.WriteLine(String.Format(CultureInfo.InvariantCulture,
                                                "Method {0}, response description: {1}", request.Method,
                                                response.StatusDescription));
            }
            catch (Exception e)
            {
                // possible that the Contact does not exist any more
                if (string.Compare(e.Message, WebExceptionCode404Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "Contact with Id '{0}' does not exist.", id));
                }

                throw new ConstantException(e.Message, e);
            }
            finally
            {
                if (response != null)
                {
                    // close the response
                    response.Close();
                }
            }
        }
        #endregion

        #region Email Campaign - Update entity -
        /// <summary>
        /// Get EmailCampaign By Id
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="id">campaign id</param>
        /// <returns>Email campaign with the specified id</returns>
        public static EmailCampaign GetEmailCampaignById(AuthenticationData authenticationData, string id)
        {
            string currentAddress = string.Format("{0}/{1}", authenticationData.AccountEmailCampaignsListUri, id);

            Stream stream = Stream.Null;
            try
            {
                // get the response stream
                stream = GetResponseStream(new Uri(currentAddress), authenticationData);

                // parse the stream and get an Email Campaign
                return EmailCampaignComponent.GetEmailCampaign(stream);
            }
            catch (Exception e)
            {
                throw new ConstantException(e.Message, e);
            }
            finally
            {
                // close the response stream
                stream.Close();
            }
        }

        /// <summary>
        /// Update a New EmailCampaign
        /// </summary>        
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="campaign">Email Campaign to be updated</param>        
        /// <exception cref="ArgumentNullException">Thrown if specified EmailCampaign is null</exception>
        /// <exception cref="ConstantException">Thrown if communication error with Constant server occur 
        /// or other related with the response from server or if ApiKey, Username or Password are null or empty</exception>
        /// <returns>Updated EmailCampaign</returns>
        public static EmailCampaign UpdateEmailCampaign(AuthenticationData authenticationData, EmailCampaign campaign)
        {
            ValidateAuthenticationData(authenticationData);

            if (null == campaign)
            {
                throw new ArgumentNullException("campaign");
            }

            //if (null == campaign.ContactLists
            //    || campaign.ContactLists.Count == 0)
            //{
            //    throw new ConstantException("Contact does not belongs to any contact list");
            //}

            // get the Atom entry for specified Contact
            StringBuilder data = EmailCampaignComponent.UpdateEmailCampaign(campaign, authenticationData, campaign.ID);

            try
            {
                // Put the Atom entry at specified Uri and save the response stream
                PutEmailCampaignInformation(authenticationData, new Uri(authenticationData.AccountEmailCampaignsListUri + "/" + campaign.ID),
                                         data.ToString());

                return null;
            }
            catch (Exception e)
            {
                throw new ConstantException(e.Message, e);
            }
        }

        /// <summary>
        /// PUT the data at the specified Uri address. 
        /// Constant Contact server will not send any response
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="address">Uri address</param>
        /// <param name="data">Data to be send at specified Uri address</param>        
        private static void PutEmailCampaignInformation(AuthenticationData authenticationData, Uri address, string data)
        {
            // set the Http request content type
            const string contentType = @"application/atom+xml";

            // send a Http PUT request and return the response Stream
            GetResponseStream(authenticationData, address, WebRequestMethods.Http.Put, contentType, data);
        }
        #endregion


        #region Activities - Get all activities
        /// <summary>
        /// Gets first chunk of all activities
        /// </summary>
        /// <param name="Authdata">Authentication Data</param>
        /// <param name="nextChunk">Out Link to next chunk of data if</param>
        /// <returns>returns list up to 50 first activities. if more than 50, out nextChunk link to next chunk of data</returns>
        public static IList<Activity> getActivities(AuthenticationData Authdata, out string nextChunk)
        {
            return getActivities(Authdata, "", out nextChunk);
        }

        /// <summary>
        /// Gets chunk of activites at specified link
        /// </summary>
        /// <param name="Authdata">Authentication Data</param>
        /// <param name="link">Link to target chunk of data</param>
        /// <param name="nextChunk">out link to next chunk of data</param>
        /// <returns>returns list up to 50 activities of specified chunk. if more than 50, out nextChunk link to next chunk of data</returns>
        public static IList<Activity> getActivities(AuthenticationData Authdata, string link, out string nextChunk)
        {
            //Create new xmldocument, create activitieslist uri, GET, load XML.
            XmlDocument xDoc = new XmlDocument();
            string URI = "";
            if (link == "")
            {
                URI = Authdata.accountActivitiesUri;
            }
            else
            {
                URI = Authdata.ApiRootUri + link;
            }
            try
            {
                xDoc.LoadXml(Utility.httpGet(Authdata, URI));
                //Define namespaces
                XmlNamespaceManager xnsmgr = new XmlNamespaceManager(xDoc.NameTable);
                xnsmgr.AddNamespace("ns1", "http://www.w3.org/2005/Atom");
                xnsmgr.AddNamespace("ns2", "http://ws.constantcontact.com/ns/1.0/");
                //Check for link to next chunk of data. If no next chunk, return empty string.
                nextChunk = "";
                XmlNodeList xnlLinks = xDoc.SelectNodes("//ns1:link", xnsmgr);
                foreach (XmlNode xnLink in xnlLinks)
                {
                    if (xnLink.Attributes["rel"] != null)
                    {
                        if (xnLink.Attributes["rel"].Value == "next")
                        {
                            nextChunk = xnLink.Attributes["href"].Value;
                        }
                    }
                }
                //Select nodes
                XmlNodeList xnlActivities = xDoc.SelectNodes("//ns2:Activity", xnsmgr);
                //parse XML
                IList<Activity> Activities = new List<Activity>();
                foreach (XmlNode xnMember in xnlActivities)
                {
                    Activity activity = new Activity();

                    XmlNode xnName = xnMember.SelectSingleNode("../../ns1:title", xnsmgr);
                    activity.activityName = xnName.InnerText;

                    XmlNode xnID = xnMember.SelectSingleNode("../../ns1:id", xnsmgr);
                    activity.activityId = xnID.InnerText;

                    XmlNode xnUpdated = xnMember.SelectSingleNode("../../ns1:updated", xnsmgr);
                    activity.updated = xnUpdated.InnerText;

                    XmlNode xnLink = xnMember.SelectSingleNode("../../ns1:link", xnsmgr);
                    activity.activityLink = xnLink.Attributes["href"].Value;

                    XmlNode xnType = xnMember.SelectSingleNode("ns2:Type", xnsmgr);
                    activity.Type = xnType.InnerText;

                    XmlNode xnStatus = xnMember.SelectSingleNode("ns2:Status", xnsmgr);
                    activity.Status = xnStatus.InnerText;

                    XmlNode xnTransactions = xnMember.SelectSingleNode("ns2:TransactionCount", xnsmgr);
                    activity.transactionCount = xnTransactions.InnerText;

                    XmlNode xnErrors = xnMember.SelectSingleNode("ns2:Errors", xnsmgr);
                    activity.Errors = xnErrors.InnerText;

                    XmlNode xnRunStart = xnMember.SelectSingleNode("ns2:RunStartTime", xnsmgr);
                    activity.runStartTime = xnRunStart.InnerText;

                    XmlNode xnRunFinish = xnMember.SelectSingleNode("ns2:RunFinishTime", xnsmgr);
                    activity.runFinishTime = xnRunFinish.InnerText;

                    XmlNode xnInsert = xnMember.SelectSingleNode("ns2:InsertTime", xnsmgr);
                    activity.insertTime = xnInsert.InnerText;

                    Activities.Add(activity);
                }
                return Activities;
            }
            catch (Exception e)
            {
                if (string.Compare(e.Message, WebExceptionCode404Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "Activity at link '{0}' does not exist.", link));
                }

                throw new ConstantException(e.Message, e);

            }
        }
        #endregion

        #region Activities - Get Activity Details
        /// <summary>
        /// Get details of activity with specified ID
        /// </summary>
        /// <param name="Authdata">Authentication Data</param>
        /// <param name="id">ID of target activity</param>
        /// <returns>Returns details of activity of specified id</returns>
        public static Activity getActivityDetails(AuthenticationData Authdata, string id)
        {
            //verify id was specified
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Activity Id cannot be null or empty", "id");
            }
            string blank = "";
            string fullcall = Authdata.accountActivitiesUri + "/" + id;
            //return Activity
            return getActivities(Authdata, id, out blank)[0];
        }
        #endregion

        #region Activities - Export Contacts
        /// <summary>
        /// Creates export all contacts activity for targeded list.
        /// </summary>
        /// <param name="authdata">Authenticatoin Data</param>
        /// <param name="listId">ID of target list</param>
        /// <param name="fileType">Export File Type (TXT or CSV)</param>
        /// <param name="exportOptDate">if add/remove date is included in file</param>
        /// <param name="exportOptSource">if add/removed by (contact or site owner) is included</param>
        /// <param name="exportListName">if name of list is included in file</param>
        /// <param name="sortBy">sort by Email Address in Ascending Order (EMAIL_ADDRESS) or Date in Descending Order (DATE_DESC)</param>
        /// <param name="columns">List of what columns to include in exported file</param>
        /// <returns>Calls urlEncodedPost, which returns the response from server after HTTP POST</returns>
        public static string exportContacts(AuthenticationData authdata, int listId, string fileType, bool exportOptDate, bool exportOptSource, bool exportListName, string sortBy, IList<string> columns)
        {
            fileType = fileType.ToUpper();
            sortBy = sortBy.ToUpper();
            if (fileType != "TXT")
            {
                if (fileType != "CSV")
                {
                    throw new ArgumentException("File Type Must be CSV or TXT", "fileType");
                }
            }
            if (sortBy != "EMAIL_ADDRESS")
            {
                if (fileType != "DATE_DESC")
                {
                    throw new ArgumentException("Invalid Sort By Type Specified (must be DATE_DESC or EMAIL_ADDRESS)", "sortBy");
                }
            }
            string uri = authdata.accountActivitiesUri;
            string options = "activityType=EXPORT_CONTACTS&fileType=" + fileType + "&exportOptDate=" + exportOptDate.ToString() + "&exportOptSource="
                + exportOptSource.ToString() + "&exportListName=" + exportListName.ToString() + "&sortBy=" + sortBy;
            string calldata = "";
            int i;
            for (i = 0; i < columns.Count; i++)
            {
                string thisline = columns[i];
                thisline = HttpUtility.UrlEncode(thisline);
                calldata = calldata + "&columns=" + thisline;
            }
            string targetlist = authdata.AccountContactListsUri + "/" + listId.ToString();
            targetlist = HttpUtility.UrlEncode(targetlist);
            string fullrequest = options + calldata + "&listId=" + targetlist;
            try
            {
                return Utility.urlEncodedPost(authdata, uri, fullrequest);
            }
            catch (Exception e)
            {
                if (string.Compare(e.Message, WebExceptionCode404Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "List with ID of '{0}' does not exist.", listId));
                }

                throw new ConstantException(e.Message, e);

            }
        }
        #endregion

        #region Activities - Bulk Upload from file (URLEncoded / CSV ONLY)
        /// <summary>
        /// Uploads text/csv file dumped to string to specified lists
        /// </summary>
        /// <param name="authdata">Authentication Data</param>
        /// <param name="data">CSV or text, dumped to string</param>
        /// <param name="listIds">ID(s) of target lists to upload to</param>
        /// <returns>Calls urlEncodedPost, which then returns response from server (string)</returns>
        public static string bulkUrlEncoded(AuthenticationData authdata, string data, IList<string> listIds)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("No data to be uploaded specified.", "data");
            }
            if (listIds.Count == 0)
            {
                throw new ArgumentException("No target list ID(s) Specified.", "listIds");
            }
            string encodeddata = "&data=" + HttpUtility.UrlEncode(data);
            //SV_ADD is add contact, REMOVE_CONTACTS_FROM_LISTS self explanatory. CLEAR_CONTACTS_FROM_LISTS 
            //sent with no data will clear the list.
            int i = 0;
            string JoinedURIs = "";
            for (i = 0; i < listIds.Count; i++)
            {
                JoinedURIs = JoinedURIs + "&lists=" + authdata.AccountContactListsUri + "/" + listIds[i];
            }
            string fullrequest = "activityType=SV_ADD" + encodeddata + JoinedURIs;
            try
            {
                return Utility.urlEncodedPost(authdata, authdata.accountActivitiesUri, fullrequest);
            }
            catch (Exception e)
            {
                throw new ConstantException(e.Message, e);
            }
        }
        #endregion


        #region Library - Get All Folders
        /// <summary>
        /// Retrieves list of first 50 folders
        /// </summary>
        /// <param name="Authdata">Authentication Data</param>
        /// <param name="nextChunk">Out; if more than 50 folders, will out link to next chunk of data</param>
        /// <returns>Returns list of Folders</returns>
        public static IList<Folder> listFolders(AuthenticationData Authdata, out string nextChunk)
        {
            return listFolders(Authdata, "", out nextChunk);
        }

        /// <summary>
        /// Retrieves list of all folders in chunks of 50
        /// </summary>
        /// <param name="Authdata">Authentication Data</param>
        /// <param name="link">link to chunk of data desired</param>
        /// <param name="nextChunk">link to next chunk of 50 if needed, otherwise returns blank string</param>
        /// <returns>list of up to 50 folders</returns>
        public static IList<Folder> listFolders(AuthenticationData Authdata, string link, out string nextChunk)
        {
            //Create new xmldocument, create activitieslist uri, GET, load XML.
            XmlDocument xDoc = new XmlDocument();
            string URI = "";
            if (link == "")
            {
                URI = Authdata.accountFoldersUri;
            }
            else
            {
                URI = Authdata.ApiRootUri + link;
            }
            try
            {
                xDoc.LoadXml(Utility.httpGet(Authdata, URI));
                //Define namespaces
                XmlNamespaceManager xnsmgr = new XmlNamespaceManager(xDoc.NameTable);
                xnsmgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");
                //Check for link to next chunk of data. If no next chunk, return empty string.
                nextChunk = "";
                XmlNodeList xnlLinks = xDoc.SelectNodes("//ns1:link", xnsmgr);
                foreach (XmlNode xnLink in xnlLinks)
                {
                    if (xnLink.Attributes["rel"] != null)
                    {
                        if (xnLink.Attributes["rel"].Value == "next")
                        {
                            nextChunk = xnLink.Attributes["href"].Value;
                        }
                    }
                }
                //Select nodes
                XmlNodeList xnlFolders = xDoc.SelectNodes("//atom:entry", xnsmgr);
                //parse XML
                IList<Folder> Folders = new List<Folder>();
                foreach (XmlNode xnMember in xnlFolders)
                {
                    Folder folder = new Folder();

                    XmlNode xnName = xnMember.SelectSingleNode("atom:title", xnsmgr);
                    folder.folderName = xnName.InnerText;
                    XmlNode xnId = xnMember.SelectSingleNode("atom:id", xnsmgr);
                    folder.folderId = xnId.InnerText;
                    XmlNode xnfolderLink = xnMember.SelectSingleNode("atom:link", xnsmgr);
                    folder.folderLink = xnfolderLink.Attributes["href"].Value;

                    Folders.Add(folder);
                }
                //return list of activities
                return Folders;
            }
            catch (Exception e)
            {
                if (string.Compare(e.Message, WebExceptionCode404Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "Folders at link '{0}' does not exist.", link));
                }

                throw new ConstantException(e.Message, e);

            }
        }
        #endregion

        #region Library - Add a new folder
        /// <summary>
        /// Creates a new folder
        /// </summary>
        /// <param name="authdata">Authentication Data</param>
        /// <param name="folderName">Name of folder to be created</param>
        /// <returns>Calls httpPost, which returns response from server</returns>
        public static string addFolder(AuthenticationData authdata, string folderName)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentException("folderName Cannot Be Null Or Empty.", "folderName");
            }
            StringBuilder XMLData = new StringBuilder();
            XMLData.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            XMLData.Append("<atom:entry xmlns:atom=\"http://www.w3.org/2005/Atom\">");
            XMLData.Append("<atom:content>");
            XMLData.Append("<Folder>");
            XMLData.Append("<Name>" + folderName + "</Name>");
            XMLData.Append("</Folder>");
            XMLData.Append("</atom:content>");
            XMLData.Append("</atom:entry>");
            try
            {
                return httpPost(authdata, authdata.accountFoldersUri, XMLData.ToString());
            }
            catch (Exception e)
            {
                if (string.Compare(e.Message, WebExceptionCode409Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "Folder with name '{0}' already exists.", folderName));
                }

                throw new ConstantException(e.Message, e);

            }
        }
        #endregion


        #region Images - List All Images
        /// <summary>
        /// Get chunk of first 50 images in specified folder
        /// </summary>
        /// <param name="folderId">ID of target folder</param>
        /// <param name="Authdata">Authentication Data</param>
        /// <param name="nextChunk">out link to next chunk of data</param>
        /// <returns></returns>
        public static IList<Image> listImages(string folderId, AuthenticationData Authdata, out string nextChunk)
        {
            if (string.IsNullOrEmpty(folderId))
            {
                throw new ArgumentException("folderId Cannot Be Null Or Empty.", "folderId");
            }
            string fullURI = Authdata.accountFoldersUri + "/" + folderId + "/images";
            try
            {
                return listImages(Authdata, fullURI, out nextChunk);
            }
            catch (Exception e)
            {
                if (string.Compare(e.Message, WebExceptionCode404Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "Folders with ID '{0}' does not exist.", folderId));
                }

                throw new ConstantException(e.Message, e);

            }
        }

        /// <summary>
        /// Get chunk of 50 images at specified link
        /// </summary>
        /// <param name="Authdata">Authentication Data</param>
        /// <param name="link">Link to desired chunk of data</param>
        /// <param name="nextChunk">out link to next chunk of data</param>
        /// <returns></returns>
        public static IList<Image> listImages(AuthenticationData Authdata, string link, out string nextChunk)
        {
            if (string.IsNullOrEmpty(link))
            {
                throw new ArgumentException("Link Cannot Be Null Or Empty.", "link");
            }
            //Create new xmldocument, create activitieslist uri, GET, load XML.
            XmlDocument xDoc = new XmlDocument();
            string URI = Authdata.ApiRootUri + link;

            try
            {
                xDoc.LoadXml(Utility.httpGet(Authdata, URI));
                //Define namespaces
                XmlNamespaceManager xnsmgr = new XmlNamespaceManager(xDoc.NameTable);
                xnsmgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");
                //Check for link to next chunk of data. If no next chunk, return empty string.
                nextChunk = "";
                XmlNodeList xnlLinks = xDoc.SelectNodes("//ns1:link", xnsmgr);
                foreach (XmlNode xnLink in xnlLinks)
                {
                    if (xnLink.Attributes["rel"] != null)
                    {
                        if (xnLink.Attributes["rel"].Value == "next")
                        {
                            nextChunk = xnLink.Attributes["href"].Value;
                        }
                    }
                }
                //Select nodes
                XmlNodeList xnlImages = xDoc.SelectNodes("//atom:entry", xnsmgr);
                //parse XML
                IList<Image> Images = new List<Image>();
                foreach (XmlNode xnMember in xnlImages)
                {
                    Image image = new Image();

                    XmlNode xnName = xnMember.SelectSingleNode("atom:content/Image/FileName", xnsmgr);
                    image.fileName = xnName.InnerText;
                    XmlNode xnURL = xnMember.SelectSingleNode("atom:content/Image/ImageURL", xnsmgr);
                    image.imageUrl = xnURL.InnerText;
                    XmlNode xnHeight = xnMember.SelectSingleNode("atom:content/Image/Height", xnsmgr);
                    image.height = xnHeight.InnerText;
                    XmlNode xnWidth = xnMember.SelectSingleNode("atom:content/Image/Width", xnsmgr);
                    image.width = xnWidth.InnerText;
                    XmlNode xnDescription = xnMember.SelectSingleNode("atom:content/Image/Description", xnsmgr);
                    image.description = xnDescription.InnerText;
                    XmlNode xnFileSize = xnMember.SelectSingleNode("atom:content/Image/FileSize", xnsmgr);
                    image.fileSize = xnFileSize.InnerText;
                    XmlNode xnUpdated = xnMember.SelectSingleNode("atom:content/Image/LastUpdated", xnsmgr);
                    image.updated = xnUpdated.InnerText;
                    XmlNode xnMd5hash = xnMember.SelectSingleNode("atom:content/Image/MD5Hash", xnsmgr);
                    image.md5Hash = xnMd5hash.InnerText;
                    XmlNode xnLink = xnMember.SelectSingleNode("atom:link", xnsmgr);
                    image.link = xnLink.Attributes["href"].Value;
                    Images.Add(image);
                }
                //return list of activities
                return Images;
            }
            catch (Exception e)
            {
                if (string.Compare(e.Message, WebExceptionCode404Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "Link '{0}' does not exist. (404)", link));
                }

                throw new ConstantException(e.Message, e);

            }
        }
        #endregion

        #region Images - Get Image Details
        /// <summary>
        /// Gets Details of a single image
        /// </summary>
        /// <param name="link">link to target image</param>
        /// <param name="Authdata">Authentication Data</param>
        /// <returns>Details of target image</returns>
        public static Image getImageDetails(string link, AuthenticationData Authdata)
        {
            if (string.IsNullOrEmpty(link))
            {
                throw new ArgumentException("link Cannot Be Null Or Empty.", "link");
            }
            string blank = "";
            try
            {
                //return Image
                return listImages(Authdata, link, out blank)[0];
            }
            catch (Exception e)
            {
                if (string.Compare(e.Message, WebExceptionCode404Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "Image with link '{0}' does not exist.", link));
                }

                throw new ConstantException(e.Message, e);

            }
        }
        #endregion

        #region Images - Delete Single Image
        /// <summary>
        /// Deletes target image
        /// </summary>
        /// <param name="authdata">Authentication Data</param>
        /// <param name="link">Link to desired image to delete (EX: /ws/customers/{username}/library/folders/{folderID}/images/{imageID})</param>
        public static void deleteImage(AuthenticationData authdata, string link)
        {
            if (string.IsNullOrEmpty(link))
            {
                throw new ArgumentException("link Cannot Be Null Or Empty.", "link");
            }
            string fullURI = authdata.ApiRootUri + link;
            try
            {
                Utility.httpDelete(authdata, link);
            }
            catch (Exception e)
            {
                if (string.Compare(e.Message, WebExceptionCode404Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "Image with link '{0}' does not exist.", link));
                }

                throw new ConstantException(e.Message, e);

            }
        }
        #endregion

        #region Images - Delete All Images In Specified Folder
        /// <summary>
        /// Deletes all images in target folder
        /// </summary>
        /// <param name="authdata">Authentication Data</param>
        /// <param name="folderId">ID of desired folder to empty</param>
        public static void clearFolder(AuthenticationData authdata, int folderId)
        {
            string fullURI = authdata.accountFoldersUri + "/" + folderId.ToString();
            try
            {
                Utility.httpDelete(authdata, fullURI);
            }
            catch (Exception e)
            {
                if (string.Compare(e.Message, WebExceptionCode404Message) == 0)
                {
                    throw new ConstantException(String.Format(CultureInfo.InvariantCulture,
                                                              "Folder with ID '{0}' does not exist.", folderId));
                }

                throw new ConstantException(e.Message, e);

            }
        }
        #endregion


        #region Common - Read response -
        /// <summary>
        /// Sends a Http request on specified Uri address and returns the response Stream
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="address">Uri address</param>
        /// <param name="requestMethod">Type of Http request</param>
        /// <param name="contentType">Content type of the Http request</param>
        /// <param name="data">Data to be send at specified Uri address</param>
        /// <returns>Response Stream</returns>
        private static Stream GetResponseStream(AuthenticationData authenticationData,
            Uri address, string requestMethod, string contentType, string data)
        {
            // get data bytes
            byte[] dataByte = Encoding.ASCII.GetBytes(data);

            // send the request and return the response Stream
            return GetResponseStream(authenticationData, address, requestMethod, contentType, dataByte);
        }

        /// <summary>
        /// Sends a Http request on specified Uri address and returns the response Stream
        /// </summary>
        /// <param name="authenticationData">Authentication data (username, password and API Key)</param>
        /// <param name="address">Uri address</param>
        /// <param name="requestMethod">Type of Http request</param>
        /// <param name="contentType">Content type of the Http request</param>
        /// <param name="data">Data to be send at specified Uri address</param>
        /// <returns>Response Stream</returns>
        private static Stream GetResponseStream(AuthenticationData authenticationData,
            Uri address, string requestMethod, string contentType, byte[] data)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
            request.Credentials = CreateCredentialCache(address, authenticationData);

            request.Method = requestMethod;
            // set the content type of the data being posted
            request.ContentType = contentType;
            // request MUST include a WWW-Authenticate
            request.PreAuthenticate = true;

            // set the content length of the data being posted
            request.ContentLength = data.Length;

            // write data
            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);

            Stream responseStream = Stream.Null;
            try
            {
                // get the response Stream
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Console.WriteLine(String.Format(CultureInfo.InvariantCulture,
                                                "Method {0}, response description: {1}", request.Method,
                                                response.StatusDescription));

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // server don't send any response to us
                    return Stream.Null;
                }

                // get the response Stream
                responseStream = response.GetResponseStream();

                // read the response stream and save it into a memory stream
                MemoryStream memoryStream = ReadResponseStream(responseStream);

                return memoryStream;
            }
            finally
            {
                // close the Stream object
                stream.Close();

                // close response stream; it also closes the web response
                responseStream.Close();
            }
        }

        /// <summary>
        /// Converts the specified Stream into a Memory Stream
        /// </summary>
        /// <param name="stream">Stream to be converted</param>
        /// <exception cref="IOException">Thrown if any of the underlying IO calls fail</exception>
        /// <returns>Result of conversion</returns>
        private static MemoryStream ReadResponseStream(Stream stream)
        {
            MemoryStream memoryStream = new MemoryStream();

            // read the stream
            byte[] responseBytes = ReadFully(stream, 0);

            // write all the bytes into the memory stream
            memoryStream.Write(responseBytes, 0, responseBytes.Length);

            // set current position to 0
            memoryStream.Position = 0;

            return memoryStream;
        }

        /// <summary>
        /// Sends a Http GET request and returns the response Stream from the specified Uri address
        /// </summary>
        /// <param name="address">Uri address</param>     
        /// <param name="authenticationData">Authentication data</param>
        /// <returns>Response Stream</returns>
        private static Stream GetResponseStream(Uri address, AuthenticationData authenticationData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
            request.Credentials = CreateCredentialCache(address, authenticationData);
            request.Method = WebRequestMethods.Http.Get;

            // request MUST include a WWW-Authenticate
            request.PreAuthenticate = true;

            Stream stream = Stream.Null;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // get the response Stream
                stream = response.GetResponseStream();

                // read the response stream and save it into a memory stream
                return ReadResponseStream(stream);
            }
            catch (WebException e)
            {
                if (null != e.Response)
                {
                    Console.Out.WriteLine("WebException Response Headers =");
                    Console.Out.WriteLine(e.Response.Headers);
                }
                throw;
            }
            finally
            {
                if (stream != Stream.Null)
                {
                    // close response stream; it also closes the web response
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. 
        /// The data is returned as a byte array 
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        /// <exception cref="IOException">Thrown if any of the underlying IO calls fail</exception>
        /// <returns>Stream bytes</returns>
        private static byte[] ReadFully(Stream stream, int initialLength)
        {
            // if we've been passed an unhelpful initial length, 
            // just use 32K
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // if we've reached the end of our buffer, check to see if there's
                // any more information
                if (read != buffer.Length) continue;
                int nextByte = stream.ReadByte();

                // end of stream? if so, we're done
                if (nextByte == -1)
                {
                    return buffer;
                }

                // resize the buffer, put in the byte we've just
                // read, and continue
                byte[] newBuffer = new byte[buffer.Length * 2];
                Array.Copy(buffer, newBuffer, buffer.Length);
                newBuffer[read] = (byte)nextByte;
                buffer = newBuffer;
                read++;
            }

            // buffer is now too big. shrink it
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }
        #endregion

        #region Common - Create network credentials -
        /// <summary>
        /// Create credentials for network transport
        /// </summary>
        /// <param name="address">Uri address</param>
        /// <param name="authenticationData">Authentication data</param>
        /// <returns>The Credentials for specified Uri address</returns>
        private static ICredentials CreateCredentialCache(Uri address, AuthenticationData authenticationData)
        {
            NetworkCredential networkCred = new NetworkCredential(authenticationData.AccountUserName, authenticationData.Password);
            CredentialCache cacheCred = new CredentialCache();
            cacheCred.Add(address, "Basic", networkCred);

            return cacheCred;
        }
        #endregion

        #region Validation
        /// <summary>
        /// Check if API Key, Username and Password are not null or empty        
        /// </summary>
        /// <param name="authenticationData">Authentication data to be validated</param>
        /// <exception cref="ConstantException">Thrown if API Key, Username or Password are null or empty</exception>
        private static void ValidateAuthenticationData(AuthenticationData authenticationData)
        {
            if (string.IsNullOrEmpty(authenticationData.Username))
            {
                throw new ConstantException("Username cannot be null or empty");
            }

            if (string.IsNullOrEmpty(authenticationData.Password))
            {
                throw new ConstantException("Password cannot be null or empty");
            }

            if (string.IsNullOrEmpty(authenticationData.ApiKey))
            {
                throw new ConstantException("API Key cannot be null or empty");
            }
        }

        /// <summary>
        /// Check if e-mail is valid
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            const string strRegex = @"^([a-zA-Z0-9_\-\.~#$%]+)@((\[[0-9]{1,3}" +
                                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            Regex re = new Regex(strRegex);
            return re.IsMatch(email);
        }
        #endregion

        #region Sorting rules
        /// <summary>
        /// Defines the compare criteria for two Contact List instances
        /// </summary>
        /// <param name="x">Contact List to be compared</param>
        /// <param name="y">Contact List to be compared</param>
        /// <returns></returns>
        public static int CompareContactListsBySortOrder(ContactList x, ContactList y)
        {
            if (x.SortOrder.HasValue && y.SortOrder.HasValue)
            {
                return x.SortOrder.Value.CompareTo(y.SortOrder.Value);
            }

            return 0;
        }
        #endregion

        #region http utilites, to be merged.
        /// <summary>
        /// Performs HTTP GET to specified URI
        /// </summary>
        /// <param name="Authdata">Authentication Data</param>
        /// <param name="URI">Target URI to send request to</param>
        /// <returns>String containing XML response from server</returns>
        public static string httpGet(AuthenticationData Authdata, string URI)
        {
            // Create a request for the URL. 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URI);
            // Add Request Header
            request.Accept = "application/atom+xml";
            // set the credentials.
            request.Credentials = new NetworkCredential(Authdata.AccountUserName, Authdata.Password);
            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            //cleanup.
            reader.Close();
            response.Close();
            // Return server response.
            return responseFromServer;
        }

        /// <summary>
        /// HTTP POST using URLEncoded Content Type
        /// </summary>
        /// <param name="Authdata">Authentication Data</param>
        /// <param name="URI">URI to POST URLEncoded data to</param>
        /// <param name="content">URLencoded data to POST</param>
        /// <returns>string containing XML response from server</returns>
        public static string urlEncodedPost(AuthenticationData Authdata, string URI, string content)
        {
            // Create a request
            WebRequest request = WebRequest.Create(URI);
            // Set API+UN/PWD Credentials
            request.Credentials = new NetworkCredential(Authdata.AccountUserName, Authdata.Password);
            // Set Method type
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            string postData = content;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType of Request
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            return (responseFromServer);
        }

        /// <summary>
        /// Sends HTTP Delete to specified URI
        /// </summary>
        /// <param name="Authdata">Authentication Data</param>
        /// <param name="URI">Target URI to send DELETE to</param>
        /// <returns>string containing XML response from server</returns>
        public static string httpDelete(AuthenticationData Authdata, string URI)
        {
            // Create a request for the URL. 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URI);
            // Add Request Header
            request.Method = "DELETE";
            // set the credentials.
            request.Credentials = new NetworkCredential(Authdata.AccountUserName, Authdata.Password);
            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            //cleanup.
            reader.Close();
            response.Close();
            // Return server response.
            return responseFromServer;
        }

        /// <summary>
        /// Posts XML data to specified URI
        /// </summary>
        /// <param name="Authdata">Authentication Data</param>
        /// <param name="URI">Target URI to POST XML data to</param>
        /// <param name="data">XML Data</param>
        /// <returns>string containing XML response from server</returns>
        public static string httpPost(AuthenticationData Authdata, string URI, string data)
        {
            // Create a request for the URL. 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URI);
            // Add Request Header
            request.Accept = "application/atom+xml";
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // set the credentials.
            request.Credentials = new NetworkCredential(Authdata.AccountUserName, Authdata.Password);
            // Create POST data and convert it to a byte array.
            string postData = data;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/atom+xml";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            Console.Write(responseFromServer);
            return responseFromServer;
        }
        #endregion
    }

    /// <summary>
    /// General exception type. Could be used when communication errors
    /// with the Constant Contact Server occur or other errors
    /// 
    /// </summary>
    [Serializable]
    public class ConstantException : Exception
    {
        #region Constructor
        /// <summary>
        /// Default empty constructor
        /// </summary>
        public ConstantException()
        {
        }

        /// <summary>
        /// Constructor with message parameter
        /// </summary>
        /// <param name="message">Exception message</param>
        public ConstantException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor with message and inner exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public ConstantException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        #endregion
    }

    /// <summary>
    /// Exception class for Constant Contact Account authentication
    /// 
    /// </summary>
    [Serializable]
    public class ConstantAuthenticationException : Exception
    {
        #region Fields
        /// <summary>
        /// Constant Contact Account username
        /// </summary>
        private string _username;

        /// <summary>
        /// Constant Contact Account password
        /// </summary>
        private string _password;
        #endregion

        #region Constructor
        /// <summary>
        /// Default empty constructor
        /// </summary>
        public ConstantAuthenticationException()
        {
        }

        /// <summary>
        /// Constructor with message parameter
        /// </summary>
        /// <param name="message">Exception mesage</param>
        public ConstantAuthenticationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor with message parameter and username
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="username">Constant username</param>
        public ConstantAuthenticationException(string message, string username)
            : base(message)
        {
            Username = username;
        }

        /// <summary>
        /// Constructor with message parameter, username and password
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="username">Constant username</param>
        /// <param name="password">Constant password</param>
        public ConstantAuthenticationException(string message, string username, string password)
            : base(message)
        {
            Username = username;
            Password = password;
        }

        /// <summary>
        /// Constructor with message and inner exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public ConstantAuthenticationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor with message, inner exception and username
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        /// <param name="username">Constant username</param>        
        public ConstantAuthenticationException(string message, Exception innerException, string username)
            : base(message, innerException)
        {
            Username = username;
        }

        /// <summary>
        /// Constructor with message, inner exception, username and password
        /// </summary>
        /// <param name="message">Excetion message</param>
        /// <param name="innerException">Inner exception</param>
        /// <param name="username">Constant username</param>
        /// <param name="password">Constant password</param>
        public ConstantAuthenticationException(string message, Exception innerException, string username, string password)
            : base(message, innerException)
        {
            Username = username;
            Password = password;
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Serialization constructor
        /// </summary>
        /// <param name="serializationInfo">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected ConstantAuthenticationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// GetObjectData override
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        [System.Security.Permissions.SecurityPermission
            (System.Security.Permissions.SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Username", _username);
            info.AddValue("Password", _password);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the Constant Contact Account username that cannot access Constant resources
        /// </summary>
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>
        /// Gets or sets the Constant Contact Account password that cannot access Constant resources
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        #endregion
    }
}