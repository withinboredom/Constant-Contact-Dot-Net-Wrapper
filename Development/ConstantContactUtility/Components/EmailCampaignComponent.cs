using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using ConstantContactBO;
using ConstantContactBO.Entities;

namespace ConstantContactUtility.Components
{
    /// <summary>
    /// Parse response Streams into Contacts, creates entries for creating and updating Contacts
    /// 
    /// </summary>
    public class EmailCampaignComponent
    {
        #region Fields
        /// <summary>
        /// Association between the EmailCampaignStatus enum and real EmailCampaign Status values
        /// </summary>
        private static Dictionary<CampaignState, string> emailCampaignStatusNames;
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
        /// Attribute value used to edit an EmailCampaign
        /// </summary>
        private const string EmailCampaignXmlAttributeValueEdit = "edit";

        /// <summary>
        /// Name Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeName = "Name";

        /// <summary>
        /// Status Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeStatus = "Status";

        /// <summary>
        /// Date Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeDate = "Date";

        /// <summary>
        /// ContactLists parent Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeContactLists = "ContactLists";

        /// <summary>
        /// ContactList Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeContactList = "ContactList";

        /// <summary>
        /// LastEditDate Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeLastEditDate = "LastEditDate";

        /// <summary>
        /// NextRunDate Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeNextRunDate = "NextRunDate";

        /// <summary>
        /// Sent Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeSent = "Sent";

        /// <summary>
        /// Opens Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeOpens = "Opens";

        /// <summary>
        /// Clicks Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeClicks = "Clicks";

        /// <summary>
        /// Bounces Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeBounces = "Bounces";

        /// <summary>
        /// Forwards Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeForwards = "Forwards";

        /// <summary>
        /// SpamReports Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeSpamReports = "SpamReports";

        /// <summary>
        /// Subject Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeSubject = "Subject";

        /// <summary>
        /// FromName Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeFromName = "FromName";

        /// <summary>
        /// CampaignType Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeCampaignType = "CampaignType";

        /// <summary>
        /// ViewAsWebpage Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeViewAsWebpage = "ViewAsWebpage";

        /// <summary>
        /// ViewAsWebpageLinkText Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeViewAsWebpageLinkText = "ViewAsWebpageLinkText";

        /// <summary>
        /// ViewAsWebpageText Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeViewAsWebpageText = "ViewAsWebpageText";

        /// <summary>
        /// PermissionReminder Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodePermissionReminder = "PermissionReminder";

        /// <summary>
        /// PermissionReminderText Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodePermissionReminderText = "PermissionReminderText";

        /// <summary>
        /// GreetingSalutation Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeGreetingSalutation = "GreetingSalutation";

        /// <summary>
        /// GreetingString Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeGreetingString = "GreetingString";

        /// <summary>
        /// OrganizationName Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeOrganizationName = "OrganizationName";

        /// <summary>
        /// OrganizationAddress1 Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeOrganizationAddress1 = "OrganizationAddress1";

        /// <summary>
        /// OrganizationAddress2 Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeOrganizationAddress2 = "OrganizationAddress2";

        /// <summary>
        /// OrganizationAddress3 Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeOrganizationAddress3 = "OrganizationAddress3";

        /// <summary>
        /// OrganizationCity Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeOrganizationCity = "OrganizationCity";

        /// <summary>
        /// OrganizationState Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeOrganizationState = "OrganizationState";

        /// <summary>
        /// OrganizationInternationalState Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeOrganizationInternationalState = "OrganizationInternationalState";

        /// <summary>
        /// OrganizationCountry Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeOrganizationCountry = "OrganizationCountry";

        /// <summary>
        /// OrganizationPostalCode Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeOrganizationPostalCode = "OrganizationPostalCode";

        /// <summary>
        /// IncludeForwardEmail Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeIncludeForwardEmail = "IncludeForwardEmail";

        /// <summary>
        /// ForwardEmailLinkText Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeForwardEmailLinkText = "ForwardEmailLinkText";

        /// <summary>
        /// IncludeSubscribeLink Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeIncludeSubscribeLink = "IncludeSubscribeLink";

        /// <summary>
        /// SubscribeLinkText Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeSubscribeLinkText = "SubscribeLinkText";

        /// <summary>
        /// EmailContentFormat Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeEmailContentFormat = "EmailContentFormat";

        /// <summary>
        /// EmailContent Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeEmailContent = "EmailContent";

        /// <summary>
        /// EmailTextContent Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeEmailTextContent = "EmailTextContent";

        /// <summary>
        /// OptOuts Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeOptOuts = "OptOuts";

        /// <summary>
        /// GreetingName Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeGreetingName = "GreetingName";

        /// <summary>
        /// FromEmail Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeFromEmail = "FromEmail";

        /// <summary>
        /// ReplyToEmail Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeReplyToEmail = "ReplyToEmail";

        /// <summary>
        /// ReplyToEmail Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeEmail = "Email";

        /// <summary>
        /// ReplyToEmail Node Of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeEmailAddress = "EmailAddress";

        /// <summary>
        /// StyleSheet Node of Email Campaign
        /// </summary>
        private const string EmailCampaignXmlNodeStyleSheet = "StyleSheet";

        #endregion

        #region Constructor
        /// <summary>
        /// Class constructor
        /// </summary>
        static EmailCampaignComponent()
        {
            InitializeEmailCampaignStatusNames();
        }
        #endregion

        #region Public static methods
        /// <summary>
        /// Get the Atom entry for newly Email Campaign to be send to Constant server
        /// </summary>
        /// <param name="emailCampaign">EmailCampaign to be created</param>
        /// <param name="authenticationData">Account Owner EmailCampaign resource</param>
        /// <returns>Atom entry for creating specified EmailCampaign</returns>
        public static StringBuilder CreateNewEmailCampaign(EmailCampaign emailCampaign, AuthenticationData authenticationData)
        {
            return CreateAtomEntry(emailCampaign, authenticationData, null);
        }

        /// <summary>
        /// Get the Atom entry for update Email Campaign to be send to Constant server
        /// </summary>
        /// <param name="emailCampaign">EmailCampaign to be updated</param>
        /// <param name="authenticationData">Account Owner EmailCampaign resource</param>
        /// <param name="id">Email Campaign Id for update</param>
        /// <returns>Atom entry for the specified EmailCampaign</returns>
        public static StringBuilder UpdateEmailCampaign(EmailCampaign emailCampaign, AuthenticationData authenticationData, string id)
        {
            return CreateAtomEntry(emailCampaign, authenticationData, id);
        }

        /// <summary>
        /// Get Email Campaign collection from the Http response stream.
        /// </summary>
        /// <param name="stream">Response stream</param>
        /// <returns>Collection of Email Campaigns</returns>
        public static List<EmailCampaign> GetEmailCampaignCollection(Stream stream)
        {
            List<EmailCampaign> list = new List<EmailCampaign>();
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
                list.Add(GetEmailCampaign(node, resolver));
            }

            reader.Close();
            xmlreader.Close();

            return list;
        }

        /// <summary>
        /// Get Email Campaign from the Http response stream.
        /// </summary>
        /// <param name="stream">Response stream</param>
        /// <returns>Email Campaign</returns>
        public static EmailCampaign GetEmailCampaign(Stream stream)
        {
            EmailCampaign campaign = new EmailCampaign();
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
                campaign = GetEmailCampaign(node, resolver);
                break;
            }

            reader.Close();
            xmlreader.Close();

            return campaign;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initialize Email Campaign Status Names
        /// </summary>
        private static void InitializeEmailCampaignStatusNames()
        {
            emailCampaignStatusNames = new Dictionary<CampaignState, string>();
            emailCampaignStatusNames.Add(CampaignState.Archived, "Archived");
            emailCampaignStatusNames.Add(CampaignState.ArchivePending, "ArchivePending");
            emailCampaignStatusNames.Add(CampaignState.Closed, "Closed");
            emailCampaignStatusNames.Add(CampaignState.ClosePending, "ClosePending");
            emailCampaignStatusNames.Add(CampaignState.Draft, "Draft");
            emailCampaignStatusNames.Add(CampaignState.Running, "Running");
            emailCampaignStatusNames.Add(CampaignState.Scheduled, "Scheduled");
            emailCampaignStatusNames.Add(CampaignState.Sent, "Sent");
        }

        /// <summary>
        /// Create an Atom entry used to create a new EmailCampaign
        /// </summary>
        /// <param name="emailCampaign">EmailCampaign to be created</param>
        /// <param name="authenticationData">Account Owner EmailCampaign resource</param>
        /// <param name="id">Account Owner EmailCampaign id - if its already created</param>
        /// <returns>Atom entry used to create new EmailCampaign
        /// </returns>
        private static StringBuilder CreateAtomEntry(EmailCampaign emailCampaign, AuthenticationData authenticationData, string id)
        {
            string atomId = string.IsNullOrEmpty(id) ? "0000000000000" : id;
            StringBuilder data = new StringBuilder();

            data.Append("<?xml version='1.0' encoding='UTF-8'?>");
            data.AppendFormat("<entry xmlns=\"{0}\">", AtomNamespace);

            if (atomId.Equals("0000000000000"))
            {
                data.AppendFormat("<link href=\"/ws/customers/{0}/{1}\" rel=\"{2}\" />", authenticationData.Username,
                                  authenticationData.CampaignsURI, EmailCampaignXmlAttributeValueEdit);
                data.AppendFormat("<id>http://api.constantcontact.com/ws/customers/{0}/{1}</id>", authenticationData.Username, authenticationData.CampaignsURI);
            }
            else
            {
                data.AppendFormat("<link href=\"/ws/customers/{0}/{1}/{3}\" rel=\"{2}\" />", authenticationData.Username,
                                  authenticationData.CampaignsURI, EmailCampaignXmlAttributeValueEdit, atomId);
                data.AppendFormat("<id>http://api.constantcontact.com/ws/customers/{0}/{1}/{2}</id>",
                                  authenticationData.Username, authenticationData.CampaignsURI, atomId);
            }

            data.AppendFormat("<title type=\"text\">{0}</title>", emailCampaign.Name);
            data.AppendFormat("<updated>{0}</updated>", DateTime.Now.ToString("o"));//yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'
            data.AppendFormat("<author><name>{0}</name></author>", "Constant Contact");
            data.Append("<content type=\"application/vnd.ctct+xml\">");
            data.AppendFormat("<Campaign xmlns=\"{0}\" id=\"http://api.constantcontact.com/ws/customers/{1}/{2}/{3}\" >", ConstantNamespace, authenticationData.Username, authenticationData.CampaignsURI, atomId);
            data.AppendFormat("<Name>{0}</Name>", emailCampaign.Name);
            data.Append("<Status>Draft</Status>");
            data.AppendFormat("<Date>{0}</Date>", emailCampaign.Date.ToString("o"));//yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'

            if (!atomId.Equals("0000000000000"))
            {
                data.AppendFormat("<LastEditDate>{0}</LastEditDate>", emailCampaign.LastEditDate.ToString("o"));
                //data.AppendFormat("<NextRunDate>{0}</NextRunDate>", emailCampaign.NextRunDate.ToString("o"));
                data.AppendFormat("<Sent>{0}</Sent>", emailCampaign.Sent);
                data.AppendFormat("<Opens>{0}</Opens>", emailCampaign.Opens);
                data.AppendFormat("<Clicks>{0}</Clicks>", emailCampaign.Clicks);
                data.AppendFormat("<Bounces>{0}</Bounces>", emailCampaign.Bounces);
                data.AppendFormat("<Forwards>{0}</Forwards>", emailCampaign.Forwards);
                data.AppendFormat("<OptOuts>{0}</OptOuts>", emailCampaign.OptOuts);
                data.AppendFormat("<SpamReports>{0}</SpamReports>", emailCampaign.SpamReports);
                data.AppendFormat("<CampaignType>{0}</CampaignType>", emailCampaign.CampaignType);
            }

            data.AppendFormat("<Subject>{0}</Subject>", emailCampaign.Subject);
            data.AppendFormat("<FromName>{0}</FromName>", emailCampaign.FromName);
            data.AppendFormat("<ViewAsWebpage>{0}</ViewAsWebpage>", emailCampaign.ViewAsWebpage ? "YES" : "NO");
            data.AppendFormat("<ViewAsWebpageLinkText>{0}</ViewAsWebpageLinkText>", emailCampaign.ViewAsWebpageLinkText);
            data.AppendFormat("<ViewAsWebpageText>{0}</ViewAsWebpageText>", emailCampaign.ViewAsWebpageText);
            data.AppendFormat("<PermissionReminder>{0}</PermissionReminder>", emailCampaign.PermissionReminder ? "YES" : "NO");
            data.AppendFormat("<PermissionReminderText>{0}</PermissionReminderText>", emailCampaign.PermissionReminderText);
            data.AppendFormat("<GreetingSalutation>{0}</GreetingSalutation>", emailCampaign.GreetingSalutation);
            data.AppendFormat("<GreetingString>{0}</GreetingString>", emailCampaign.GreetingString);
            data.AppendFormat("<OrganizationName>{0}</OrganizationName>", emailCampaign.OrganizationName);
            data.AppendFormat("<OrganizationAddress1>{0}</OrganizationAddress1>", emailCampaign.OrganizationAddress1);
            data.AppendFormat("<OrganizationAddress2>{0}</OrganizationAddress2>", emailCampaign.OrganizationAddress2);
            data.AppendFormat("<OrganizationAddress3>{0}</OrganizationAddress3>", emailCampaign.OrganizationAddress3);
            data.AppendFormat("<OrganizationCity>{0}</OrganizationCity>", emailCampaign.OrganizationCity);
            data.AppendFormat("<OrganizationState>{0}</OrganizationState>", emailCampaign.OrganizationState);
            data.AppendFormat("<OrganizationInternationalState>{0}</OrganizationInternationalState>", emailCampaign.OrganizationInternationalState);
            data.AppendFormat("<OrganizationCountry>{0}</OrganizationCountry>", emailCampaign.OrganizationCountry);
            data.AppendFormat("<OrganizationPostalCode>{0}</OrganizationPostalCode>", emailCampaign.OrganizationPostalCode);
            data.AppendFormat("<IncludeForwardEmail>{0}</IncludeForwardEmail>", emailCampaign.IncludeForwardEmail ? "YES" : "NO");
            data.AppendFormat("<ForwardEmailLinkText>{0}</ForwardEmailLinkText>", emailCampaign.ForwardEmailLinkText);
            data.AppendFormat("<IncludeSubscribeLink>{0}</IncludeSubscribeLink>", emailCampaign.IncludeSubscribeLink ? "YES" : "NO");
            data.AppendFormat("<SubscribeLinkText>{0}</SubscribeLinkText>", emailCampaign.SubscribeLinkText);

            // These nodes are currently only supported in CUSTOM campaign types
            if (emailCampaign.CampaignType == CampaignType.CUSTOM)
            {
                data.AppendFormat("<GreetingName>{0}</GreetingName>", emailCampaign.GreetingName);
                data.AppendFormat("<EmailContentFormat>{0}</EmailContentFormat>", emailCampaign.EmailContentFormat);
                data.AppendFormat("<EmailContent>{0}</EmailContent>", emailCampaign.EmailContentFormat.Equals("HTML") ? emailCampaign.Content : emailCampaign.XContent);
                data.AppendFormat("<EmailTextContent>{0}</EmailTextContent>", emailCampaign.TextContent);
                data.AppendFormat("<StyleSheet>{0}</StyleSheet>", emailCampaign.StyleSheet);
            }
            data.Append("<ContactLists>");
            if (emailCampaign.ContactLists.Count > 0)
            {
                foreach (var contact in emailCampaign.ContactLists)
                {
                    data.AppendFormat("<ContactList id=\"http://api.constantcontact.com/ws/customers/{0}/lists/{1}\">",
                                      authenticationData.Username, contact.Id);
                    data.AppendFormat("<link xmlns=\"{0}\" href=\"/ws/customers/{1}/lists/{2}\" rel=\"self\" />",
                                      AtomNamespace, authenticationData.Username, contact.Id);
                    data.Append("</ContactList>");
                }
            }
            data.Append("</ContactLists>");

            data.Append("<FromEmail>");
            data.AppendFormat("<Email id=\"http://api.constantcontact.com/ws/customers/{0}/settings/emailaddresses/{1}\">", authenticationData.Username, emailCampaign.FromEmailID);
            data.AppendFormat("<link xmlns=\"{0}\" href=\"/ws/customers/{1}/settings/emailaddresses/{2}\" rel=\"self\" />", AtomNamespace, authenticationData.Username, emailCampaign.FromEmailID);
            data.Append("</Email>");
            data.AppendFormat("<EmailAddress>{0}</EmailAddress>", emailCampaign.FromEmail);
            data.Append("</FromEmail>");

            data.Append("<ReplyToEmail>");
            data.AppendFormat("<Email id=\"http://api.constantcontact.com/ws/customers/{0}/settings/emailaddresses/{1}\">", authenticationData.Username, emailCampaign.ReplyToEmailID);
            data.AppendFormat("<link xmlns=\"{0}\" href=\"/ws/customers/{1}/settings/emailaddresses/{2}\" rel=\"self\" />", AtomNamespace, authenticationData.Username, emailCampaign.ReplyToEmailID);
            data.Append("</Email>");
            data.AppendFormat("<EmailAddress>{0}</EmailAddress>", emailCampaign.ReplyToEmail);
            data.Append("</ReplyToEmail>");

            data.Append("</Campaign></content><source>");
            data.AppendFormat("<id>http://api.constantcontact.com/ws/customers/{0}/campaigns</id>", authenticationData.Username);
            data.AppendFormat("<title type=\"text\">Campaigns for customer: {0}</title>", authenticationData.Username);
            data.AppendFormat("<link href=\"campaigns\" /><link href=\"campaigns\" rel=\"self\" /><author><name>{0}</name></author><updated>{1}</updated></source></entry>", authenticationData.Username, DateTime.Now.ToString("o"));//yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'

            return data;
        }

        /// <summary>
        /// Get EmailCampaign object from specified Xml data
        /// </summary>
        /// <param name="node">Xml data cursor model</param>
        /// <param name="resolver">Xml namespace resolver</param>
        /// <returns>EmailCampaign</returns>
        private static EmailCampaign GetEmailCampaign(XPathNavigator node, IXmlNamespaceResolver resolver)
        {
            EmailCampaign emailCampaign = new EmailCampaign();

            const string xpathSelect = @"at:content/cc:Campaign";

            XPathNodeIterator emailCampaignContentNodes = node.Select(xpathSelect, resolver);

            while (emailCampaignContentNodes.MoveNext())
            {
                XPathNavigator currentNode = emailCampaignContentNodes.Current;

                emailCampaign.ID = GetEmailCampaignId(currentNode);

                if (currentNode.HasChildren)
                {
                    currentNode.MoveToFirstChild();
                    do
                    {
                        switch (currentNode.Name)
                        {
                            case EmailCampaignXmlNodeName:
                                emailCampaign.Name = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeStatus:
                                emailCampaign.State = (CampaignState)Enum.Parse(typeof(CampaignState), currentNode.Value);
                                break;

                            case EmailCampaignXmlNodeDate:
                                emailCampaign.Date = currentNode.ValueAsDateTime;
                                break;

                            case EmailCampaignXmlNodeLastEditDate:
                                emailCampaign.LastEditDate = currentNode.ValueAsDateTime;
                                break;

                            case EmailCampaignXmlNodeNextRunDate:
                                emailCampaign.NextRunDate = currentNode.ValueAsDateTime;
                                break;

                            case EmailCampaignXmlNodeSent:
                                emailCampaign.Sent = currentNode.ValueAsInt;
                                break;

                            case EmailCampaignXmlNodeOpens:
                                emailCampaign.Opens = currentNode.ValueAsInt;
                                break;

                            case EmailCampaignXmlNodeClicks:
                                emailCampaign.Clicks = currentNode.ValueAsInt;
                                break;

                            case EmailCampaignXmlNodeBounces:
                                emailCampaign.Bounces = currentNode.ValueAsInt;
                                break;

                            case EmailCampaignXmlNodeForwards:
                                emailCampaign.Forwards = currentNode.ValueAsInt;
                                break;

                            case EmailCampaignXmlNodeSpamReports:
                                emailCampaign.SpamReports = currentNode.ValueAsInt;
                                break;

                            case EmailCampaignXmlNodeOptOuts:
                                emailCampaign.OptOuts = currentNode.ValueAsInt;
                                break;

                            case EmailCampaignXmlNodeSubject:
                                emailCampaign.Subject = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeFromName:
                                emailCampaign.FromName = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeCampaignType:
                                emailCampaign.CampaignType = (CampaignType)Enum.Parse(typeof(CampaignType), currentNode.Value);
                                break;

                            case EmailCampaignXmlNodeViewAsWebpage:
                                emailCampaign.ViewAsWebpage = currentNode.Value.Equals("YES", StringComparison.CurrentCultureIgnoreCase) ? true : false;
                                break;

                            case EmailCampaignXmlNodeViewAsWebpageLinkText:
                                emailCampaign.ViewAsWebpageLinkText = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeViewAsWebpageText:
                                emailCampaign.ViewAsWebpageText = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodePermissionReminder:
                                emailCampaign.PermissionReminder = currentNode.Value.Equals("YES", StringComparison.CurrentCultureIgnoreCase) ? true : false;
                                break;

                            case EmailCampaignXmlNodePermissionReminderText:
                                emailCampaign.PermissionReminderText = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeGreetingName:
                                emailCampaign.GreetingName = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeGreetingSalutation:
                                emailCampaign.GreetingSalutation = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeGreetingString:
                                emailCampaign.GreetingString = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeOrganizationName:
                                emailCampaign.OrganizationName = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeOrganizationAddress1:
                                emailCampaign.OrganizationAddress1 = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeOrganizationAddress2:
                                emailCampaign.OrganizationAddress2 = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeOrganizationAddress3:
                                emailCampaign.OrganizationAddress3 = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeOrganizationCity:
                                emailCampaign.OrganizationCity = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeOrganizationState:
                                emailCampaign.OrganizationState = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeOrganizationInternationalState:
                                emailCampaign.OrganizationInternationalState = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeOrganizationCountry:
                                emailCampaign.OrganizationCountry = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeOrganizationPostalCode:
                                emailCampaign.OrganizationPostalCode = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeIncludeForwardEmail:
                                emailCampaign.IncludeForwardEmail = currentNode.Value.Equals("YES", StringComparison.CurrentCultureIgnoreCase) ? true : false;
                                break;

                            case EmailCampaignXmlNodeForwardEmailLinkText:
                                emailCampaign.ForwardEmailLinkText = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeIncludeSubscribeLink:
                                emailCampaign.IncludeSubscribeLink = currentNode.Value.Equals("YES", StringComparison.CurrentCultureIgnoreCase) ? true : false;
                                break;

                            case EmailCampaignXmlNodeSubscribeLinkText:
                                emailCampaign.SubscribeLinkText = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeEmailContentFormat:
                                emailCampaign.EmailContentFormat = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeEmailContent:
                                if (emailCampaign.EmailContentFormat.Equals("HTML"))
                                    emailCampaign.Content = currentNode.Value;
                                else
                                    emailCampaign.XContent = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeEmailTextContent:
                                emailCampaign.TextContent = currentNode.Value;
                                break;

                            case EmailCampaignXmlNodeContactLists:
                                emailCampaign.ContactLists = GetContactListFromCampaignResponse(currentNode);
                                break;

                            case EmailCampaignXmlNodeFromEmail:
                                ConstantContactEmail fromEmail = GetEmailFromCampaignResponse(currentNode);
                                emailCampaign.FromEmailID = fromEmail.EmailId;
                                emailCampaign.FromEmail = fromEmail.EmailAddress;
                                break;
                            case EmailCampaignXmlNodeReplyToEmail:
                                ConstantContactEmail replyToEmail = GetEmailFromCampaignResponse(currentNode);
                                emailCampaign.ReplyToEmailID = replyToEmail.EmailId;
                                emailCampaign.ReplyToEmail = replyToEmail.EmailAddress;
                                break;

                            case EmailCampaignXmlNodeStyleSheet:
                                emailCampaign.StyleSheet = currentNode.Value;
                                break;
                        }

                    } while (currentNode.MoveToNext());
                }

                break;
            }

            return emailCampaign;
        }

        /// <summary>
        /// Gets the Id of EmailCampaign from Xml data
        /// </summary>
        /// <param name="node">Xml data cursor model</param>
        /// <returns>Id of Contact List</returns>
        private static string GetEmailCampaignId(XPathNavigator node)
        {
            Regex r = new Regex(@"/([a-z]|[A-Z]|-|\d)+$");

            return r.Match(node.GetAttribute("id", string.Empty)).Value.Substring(1);
        }

        /// <summary>
        /// Returns a contact lists from the xml data
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static List<ContactList> GetContactListFromCampaignResponse(XPathNavigator node)
        {
            List<ContactList> result = new List<ContactList>();

            if (node.HasChildren)
            {
                node.MoveToFirstChild();
                {
                    do
                    {
                        switch (node.Name)
                        {
                            case EmailCampaignXmlNodeContactList:
                                ContactList contactList = new ContactList();
                                contactList.Id = GetContactListId(node);
                                result.Add(contactList);
                                break;
                        }
                    } while (node.MoveToNext());
                }
                node.MoveToParent();
            }
            return result;
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
        /// Gets the Id of System Email from Xml data
        /// </summary>
        /// <param name="node">Xml data cursor model</param>
        /// <returns>Id of System Email</returns>
        private static ConstantContactEmail GetEmailFromCampaignResponse(XPathNavigator node)
        {
            int result = 0;
            ConstantContactEmail email = new ConstantContactEmail();
            if (node.HasChildren)
            {
                node.MoveToFirstChild();
                {
                    do
                    {
                        switch (node.Name)
                        {
                            case EmailCampaignXmlNodeEmail:
                                Regex r = new Regex(@"/([a-z]|[A-Z]|-|\d)+$");
                                email.EmailId = Convert.ToInt16(r.Match(node.GetAttribute("id", string.Empty)).Value.Substring(1));
                                break;
                            case EmailCampaignXmlNodeEmailAddress:
                                email.EmailAddress = node.Value;
                                break;

                        }
                    } while (node.MoveToNext());
                }
                node.MoveToParent();
            }
            return email;
        }
        #endregion

        #region Private Internal Classes
        private class ConstantContactEmail
        {
            private String _emailAddress;
            private int _emailId;

            public String EmailAddress
            {
                set { _emailAddress = value; }
                get { return _emailAddress; }
            }

            public int EmailId
            {
                set { _emailId = value; }
                get { return _emailId; }
            }
        }
        #endregion
    }
}