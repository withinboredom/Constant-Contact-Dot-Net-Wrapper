using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using ConstantContactBO.Entities;

namespace ConstantContactUtility.Components
{
	/// <summary>
	/// Parse response Streams into Emails
	/// </summary>
	public class EmailComponent
	{
		#region Fields
		/// <summary>
		/// Association between the ContactStatus enum and real Contact Status values
		/// </summary>
		private static Dictionary<EmailStatus, string> emailStatusNames;
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
		/// Constant that describes status of Undefined Email
		/// </summary>
		private const string EmailStatusUndefined = "Undefined";

		/// <summary>
		/// Constant that describes status of Verified Email
		/// </summary>
		private const string EmailStatusVerified = "Verified";

		/// <summary>
		/// Constant that describes status of Pending Email
		/// </summary>
		private const string EmailStatusPending = "Pending";

		/// <summary>
		/// Attribute value used to retrieve the id of a Email
		/// </summary>
		private const string EmailXmlAttributeValueEdit = "edit";

		/// <summary>
		/// Xml node name of Email Title
		/// </summary>
		private const string EmailXmlElementTitle = "title";

		/// <summary>
		/// Xml node name of Email Update
		/// </summary>
		private const string EmailXmlElementUpdated = "updated";

		/// <summary>
		/// Xml node name of Email Author Name
		/// </summary>
		private const string EmailXmlElementAuthorName = "name";

		/// <summary>
		/// Xml node name of Email Address
		/// </summary>
		private const string EmailXmlElementEmailAddress = "EmailAddress";

		/// <summary>
		/// Xml node name of Email Status
		/// </summary>
		private const string EmailXmlElementStatus = "Status";

		/// <summary>
		/// Xml node name of Email VerifiedTime
		/// </summary>
		private const string EmailXmlElementVerifiedTime = "VerifiedTime";

        /// <summary>
        /// Attribute value used to retrieve the next link of chunk data
        /// </summary>
        private const string EmailXmlAttributeValueNext = "next";
		#endregion

		#region Constructor
		/// <summary>
		/// Class constructor
		/// </summary>
		static EmailComponent()
		{
			InitializeEmailStatusNames();
		}
		#endregion

		#region Public static methods
		/// <summary>
		/// Get the collection of Emails from the Http response stream
		/// </summary>
		/// <param name="stream">Response Stream</param>
		/// <returns>Collection of Emails</returns>
		public static List<Email> GetEmailCollection(Stream stream, out string nextChunkId)
		{
			List<Email> list = new List<Email>();
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

				// add Emai object to the collection
				list.Add(GetEmail(node, resolver));
			}

            nextChunkId = GetNextLink(pn, resolver);

			reader.Close();
			xmlreader.Close();

			return list;
		}

		#endregion

		#region Private Methods
		/// <summary>
		/// Initialize ContactStatusName dictionary
		/// </summary>
		private static void InitializeEmailStatusNames()
		{
			emailStatusNames = new Dictionary<EmailStatus, string> {{EmailStatus.Undefined, EmailStatusUndefined}, {EmailStatus.Pending, EmailStatusPending}, {EmailStatus.Verified, EmailStatusVerified}};
		}

		/// <summary>
		/// Get Email object from specified Xml data
		/// </summary>
		/// <param name="node">Xml data cursor model</param>
		/// <param name="resolver">Xml namespace resolver</param>
		/// <returns>Email</returns>
		private static Email GetEmail(XPathNavigator node, IXmlNamespaceResolver resolver)
		{
			Email e = new Email();
			string xpath;
			string xpathSelect;

			e.Link = GetEmailLink(node, resolver);
			xpath = String.Format(CultureInfo.InvariantCulture, @"at:{0}", EmailXmlElementTitle);
			e.Title = GetEmailNodeStringInfo(node, resolver, xpath);
			xpath = String.Format(CultureInfo.InvariantCulture, @"at:{0}", EmailXmlElementUpdated);
			e.Updated = GetEmailNodeDateTimeInfo(node, resolver, xpath);

			xpathSelect = @"at:author";
			XPathNodeIterator emailNodes = node.Select(xpathSelect, resolver);
			
			while (emailNodes.MoveNext())
			{
				XPathNavigator currentEmailNode = emailNodes.Current;
				xpath = String.Format(CultureInfo.InvariantCulture, @"at:{0}", EmailXmlElementAuthorName);
				e.AuthorName = GetEmailNodeStringInfo(currentEmailNode, resolver, xpath); 
			}

			xpathSelect = @"at:content/cc:Email";

			emailNodes = node.Select(xpathSelect, resolver);

			while (emailNodes.MoveNext())
			{
				XPathNavigator currentEmailNode = emailNodes.Current;

				e.Id = GetEmailId(currentEmailNode);

				if (currentEmailNode.HasChildren)
				{
					currentEmailNode.MoveToFirstChild();
					do
					{
						switch (currentEmailNode.Name)
						{
							case EmailXmlElementTitle:
								e.Title = currentEmailNode.Value;
								break;
							case EmailXmlElementEmailAddress:
								e.EmailAddress = currentEmailNode.Value;
								break;
							case EmailXmlElementStatus:
								e.Status = GetEmailStatus(currentEmailNode.Value);
								break;
							case EmailXmlElementVerifiedTime:
								e.VerifiedTime = currentEmailNode.ValueAsDateTime;
								break;
							default:
								break;
						}

					} while (currentEmailNode.MoveToNext());
				}

				break;
			}

			return e;
		}

        /// <summary>
        /// Get Email Node Info
        /// </summary>
        /// <param name="node">current node</param>
        /// <param name="resolver">node resolver</param>
        /// <param name="xpath">xpath</param>
        /// <returns></returns>
		private static string GetEmailNodeStringInfo(XPathNavigator node, IXmlNamespaceResolver resolver, string xpath)
		{
			XPathNodeIterator contentNode = node.Select(xpath, resolver);
			string fieldContent = string.Empty;

			while (contentNode.MoveNext())
			{
				fieldContent = contentNode.Current.Value;

				break;
			}

			return fieldContent;
		}

        /// <summary>
        /// Get Email Node Date time Info
        /// </summary>
        /// <param name="node">current node</param>
        /// <param name="resolver">node resolver</param>
        /// <param name="xpath">xpath</param>
        /// <returns></returns>
		private static DateTime? GetEmailNodeDateTimeInfo(XPathNavigator node, IXmlNamespaceResolver resolver, string xpath)
		{
			XPathNodeIterator contentNode = node.Select(xpath, resolver);
			DateTime? fieldContent = null;

			while (contentNode.MoveNext())
			{
				fieldContent = contentNode.Current.ValueAsDateTime;

				break;
			}

			return fieldContent;
		}

		/// <summary>
		/// Gets the edit Link of Email from Xml data
		/// </summary>
		/// <param name="node">Xml data cursor model</param>
		/// <param name="resolver">Xml namespace resolver</param>
		/// <returns>Edit Link of Email</returns>
		private static string GetEmailLink(XPathNavigator node, IXmlNamespaceResolver resolver)
		{
			string xpathId = String.Format(CultureInfo.InvariantCulture, @"at:link[@rel='{0}']", EmailXmlAttributeValueEdit);

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
		/// Gets the Id of Email from Xml data
		/// </summary>
		/// <param name="node">Xml data cursor model</param>
		/// <returns>Id of Email</returns>
		private static string GetEmailId(XPathNavigator node)
		{
			Regex r = new Regex(@"/([a-z]|[A-Z]|-|\d)+$");

			return r.Match(node.GetAttribute("id", string.Empty)).Value.Substring(1);
		}

		/// <summary>
		/// Get the EmailStatus enum
		/// </summary>
		/// <param name="emailStatus">Email Status real value</param>
		/// <returns>EmailStatus enum value</returns>
		private static EmailStatus GetEmailStatus(string emailStatus)
		{
			EmailStatus status = EmailStatus.Undefined;

			if (!emailStatusNames.ContainsValue(emailStatus)) return status;

			foreach (KeyValuePair<EmailStatus, string> kvp in emailStatusNames)
			{
				if (0 != string.Compare(kvp.Value, emailStatus, StringComparison.OrdinalIgnoreCase)) continue;
					
				status = kvp.Key;
				break;
			}

			return status;
		}

        /// <summary>
        /// Gets the link to next chunk of data
        /// </summary>
        /// <param name="navigator">Xml data cursor model</param>
        /// <param name="resolver">Xml namespace resolver</param>
        /// <returns>Link to next chunk of data</returns>
        private static string GetNextLink(XPathNavigator navigator, IXmlNamespaceResolver resolver)
        {
            return GetLink(navigator, EmailXmlAttributeValueNext, resolver);
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
		#endregion
	}
}