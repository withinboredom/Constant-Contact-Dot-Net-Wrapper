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
    /// Parse response Streams into Contact Lists, creates entries for creating and updating Contact Lists
    /// 
    /// </summary>
    public class ContactListComponent
    {
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
        /// Id of Active system predefined Contact List
        /// </summary>
        private const string ActiveSystemListId = "active";

        /// <summary>
        /// Id of Removed system predefined Contact List
        /// </summary>
        private const string RemovedSystemListId = "removed";

        /// <summary>
        /// Id of Do-Not-Mail system predefined Contact List
        /// </summary>
        private const string DoNotMailSystemListId = "do-not-mail";

        /// <summary>
        /// Attribute value used to retrieve the next link of chunk data
        /// </summary>
        private const string ContactListXmlAttributeValueNext = "next";

        /// <summary>
        /// Attribute value used to retrieve the id of a Contact List
        /// </summary>
        private const string ContactListXmlAttributeValueEdit = "edit";

        /// <summary>
        /// Xml node name of Contact List Name
        /// </summary>
        private const string ContactListXmlElementName = "Name";

        /// <summary>
        /// Xml node name of Contact List Shortname
        /// </summary>
        private const string ContactListXmlElementShortName = "ShortName";

        /// <summary>
        /// Xml node name of Contact List OptInDefault
        /// </summary>
        private const string ContactListXmlElementOptInDefault = "OptInDefault";

        /// <summary>
        /// Xml node name of Contact List SortOrder
        /// </summary>
        private const string ContactListXmlElementSortOrder = "SortOrder";
        #endregion

        #region Public static methods
        /// <summary>
        /// Check if specified Contact List name is a Constant Contact system predefined list
        /// </summary>
        /// <param name="listName">Name of Contact List</param>
        /// <returns>Type of system predefined list. 
        /// Will return ContactSystemList.Undefined if is not a system List
        /// </returns>
        public static ContactSystemList IsPredefinedSystemList(string listName)
        {
            ContactSystemList systemType = ContactSystemList.Undefined;

            if (string.Compare(ActiveSystemListId, listName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Constant Contact Active system predefined list
                systemType = ContactSystemList.Active;
            }
            else if (string.Compare(RemovedSystemListId, listName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Constant Contact Removed system predefined list
                systemType = ContactSystemList.Removed;
            }
            else if (string.Compare(DoNotMailSystemListId, listName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Constant Contact Do-Not-Mail system predefined list
                systemType = ContactSystemList.DoNotMail;
            }

            return systemType;
        }

        /// <summary>
        /// Get Contact List collection from the Http response stream.
        /// The collection is sorted by the Sort Order and it will include the system predefined lists ("Active", "Removed", "DoNotEmail")
        /// </summary>
        /// <param name="stream">Response stream</param>
        /// <param name="next">Link to the next chunk of data</param>
        /// <returns>Collection of Contact List</returns>
        public static List<ContactList> GetContactListsCollection(Stream stream, out string next)
        {
            List<ContactList> list = new List<ContactList>();
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
                list.Add(GetContactList(node, resolver));
            }

            // sort the collection by SortOrder field
            list.Sort(Utility.CompareContactListsBySortOrder);

            next = GetNextLink(pn, resolver);

            reader.Close();
            xmlreader.Close();

            return list;
        }

        /// <summary>
        /// Retrieve an Individual Contact List from the Http response stream
        /// </summary>
        /// <param name="stream">Reponse Stream</param>
        /// <returns>Contact List parsed from the reponse Stream</returns>
        public static ContactList GetContactListDetails(Stream stream)
        {
            ContactList cList = null;
            const string xpathSelect = @"//cc:ContactList";

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

                // get the Contact List object
                cList = GetContactList(node, resolver);
            }

            reader.Close();
            xmlreader.Close();

            return cList;
        }

        /// <summary>
        /// Get the Atom entry for newly Contact List to be send to Constant server
        /// </summary>
        /// <param name="list">Contact List to be created</param>
        /// <returns>Atom entry used to create new Contact List</returns>
        public static StringBuilder CreateNewContactList(ContactList list)
        {
            return CreateAtomEntry(list);
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Gets the link to next chunk of data
        /// </summary>
        /// <param name="navigator">Xml data cursor model</param>
        /// <param name="resolver">Xml namespace resolver</param>
        /// <returns>Link to the next chunk of data</returns>
        private static string GetNextLink(XPathNavigator navigator, IXmlNamespaceResolver resolver)
        {
            return GetLink(navigator, ContactListXmlAttributeValueNext, resolver);
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

                break;
            }

            return linkHref;
        }

        /// <summary>
        /// Gets the edit Link of Contact List from Xml data
        /// </summary>
        /// <param name="node">Xml data cursor model</param>
        /// <param name="resolver">Xml namespace resolver</param>
        /// <returns>Edit link of Contact List</returns>
        private static string GetContactListLink(XPathNavigator node, IXmlNamespaceResolver resolver)
        {
            string xpathId = String.Format(CultureInfo.InvariantCulture,
                                           @"at:link[@rel='{0}']", ContactListXmlAttributeValueEdit);

            XPathNodeIterator idNode = node.Select(xpathId, resolver);
            string link = string.Empty;

            while (idNode.MoveNext())
            {
                link = idNode.Current.GetAttribute("href", string.Empty);

                break;
            }

            return link;
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
        /// Get ContactList object from specified Xml data
        /// </summary>
        /// <param name="node">Xml data cursor model</param>
        /// <param name="resolver">Xml namespace resolver</param>
        /// <returns>Contact List</returns>
        private static ContactList GetContactList(XPathNavigator node, IXmlNamespaceResolver resolver)
        {
            ContactList cList = new ContactList();
            cList.Link = GetContactListLink(node, resolver);

            const string xpathSelect = @"at:content/cc:ContactList";

            XPathNodeIterator contactListNodes = node.Select(xpathSelect, resolver);

            while (contactListNodes.MoveNext())
            {
                XPathNavigator currentContactListNode = contactListNodes.Current;

                cList.Id = GetContactListId(currentContactListNode);
                cList.SystemList = IsPredefinedSystemList(cList.Id);

                if (currentContactListNode.HasChildren)
                {
                    currentContactListNode.MoveToFirstChild();
                    do
                    {
                        switch (currentContactListNode.Name)
                        {
                            case ContactListXmlElementName:
                                cList.Name = currentContactListNode.Value;
                                break;
                            case ContactListXmlElementShortName:
                                cList.ShortName = currentContactListNode.Value;
                                break;
                            case ContactListXmlElementOptInDefault:
                                cList.OptInDefault = currentContactListNode.ValueAsBoolean;
                                break;
                            case ContactListXmlElementSortOrder:
                                cList.SortOrder = currentContactListNode.ValueAsInt;
                                break;
                        }

                    } while (currentContactListNode.MoveToNext());
                }

                break;
            }

            return cList;
        }

        /// <summary>
        /// Create an Atom entry used to create a new Contact List
        /// </summary>
        /// <param name="list">Contact List to be created</param>
        /// <returns>Atom entry used to create new Contact List
        /// <example>
        ///     <entry xmlns="http://www.w3.org/2005/Atom">
        ///         <id>data:,</id>
        ///         <title/>
        ///         <author/>
        ///         <updated>2008-04-16</updated>
        ///         <content type="application/vnd.ctct+xml">
        ///             <ContactList xmlns="http://ws.constantcontact.com/ns/1.0/">
        ///                 <OptInDefault>false</OptInDefault>
        ///                 <Name>A New List</Name>
        ///                 <SortOrder>99</SortOrder>
        ///             </ContactList>
        ///         </content>
        ///     </entry>
        /// </example>
        /// </returns>
        private static StringBuilder CreateAtomEntry(ContactList list)
        {
            StringBuilder data = new StringBuilder();

            data.AppendFormat("<entry xmlns=\"{0}\">", AtomNamespace);
            data.Append("<id>data:,</id>");
            data.Append("<title/>");
            data.Append("<author/>");
            data.AppendFormat("<updated>{0}</updated>", DateTime.Now.ToShortDateString());
            data.Append("<content type=\"application/vnd.ctct+xml\">");
            data.AppendFormat("<ContactList xmlns=\"{0}\">", ConstantNamespace);
            data.AppendFormat("<OptInDefault>{0}</OptInDefault>", list.OptInDefault);
            data.AppendFormat("<Name>{0}</Name>", list.Name);
            data.AppendFormat("<SortOrder>{0}</SortOrder>", list.SortOrder);
            data.Append("</ContactList>");
            data.Append("</content>");
            data.Append("</entry>");

            return data;
        }
        #endregion
    }
}