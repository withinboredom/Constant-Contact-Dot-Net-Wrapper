using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;
using ConstantContactUtility;
using UploadContactForm.App_Code;
using ContactList=ConstantContactBO.ContactList;
using EmailCampaign = ConstantContactBO.Entities.EmailCampaign;
using Email = ConstantContactBO.Entities.Email;

namespace UploadContactForm.EmailCampaignPages
{
    public partial class AddEmailCampaign : Page
    {
        #region Properties
        /// <summary>
        /// Authentication data
        /// </summary>
        private AuthenticationData AuthenticationData
        {
            get
            {
                if (Session["AuthenticationData"] == null)
                {
                    Session.Add("AuthenticationData", ConstantContact.AuthenticationData);
                }
                return (AuthenticationData)Session["AuthenticationData"];
            }
        }

        /// <summary>
        /// Current chunk of data
        /// </summary>
        private string CurrentChunk
        {
            get { return (string)ViewState["currentChunk"]; }
// ReSharper disable UnusedPrivateMember
            set { ViewState["currentChunk"] = value; }
// ReSharper restore UnusedPrivateMember
        }

        /// <summary>
        /// Next chunk of data
        /// </summary>
        private string NextChunk
        {
// ReSharper disable UnusedPrivateMember
            get { return (string)ViewState["nextChunk"]; }
// ReSharper restore UnusedPrivateMember
            set { ViewState["nextChunk"] = value; }
        }

        private string ECID
        {
            get { return (string)ViewState["ecid"]; }
            set { ViewState["ecid"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.lblAddInfo.Text = "Add new Email Campaign";
                this.btnAdd.Text = "Add Email Campaign";

                this.ddlGreetingName.DataSource = Utils.GreetingName();
                this.ddlGreetingName.DataBind();

                try
                {
                    string nextChunk;
                    IList<ContactList> contactsList = string.IsNullOrEmpty(this.CurrentChunk)
                                                          ? Utility.GetUserContactListCollection(
                                                                this.AuthenticationData, out nextChunk)
                                                          : Utility.GetUserContactListCollection(
                                                                this.AuthenticationData, this.CurrentChunk, out nextChunk);
                    this.NextChunk = nextChunk;

                    this.txtPermReminder.Text =
                        string.Format("You're receiving this email because of your relationship with {0}. Please confirm your continued interest in receiving email from us.",this.AuthenticationData.Username);
                    this.txtContent.Text =
                        "<html lang=\"en\" xml:lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:cctd=\"http://www.constantcontact.com/cctd\"><body><CopyRight>Copyright (c) 1996-2009 Constant Contact. All rights reserved.  Except as permitted under a separate written agreement with Constant Contact, neither the Constant Contact software, nor any content that appears on any Constant Contact site, including but not limited to, web pages, newsletters, or templates may be reproduced, republished, repurposed, or distributed without the prior written permission of Constant Contact.  For inquiries regarding reproduction or distribution of any Constant Contact material, please contact legal@constantcontact.com.</CopyRight><OpenTracking/><!--  Do NOT delete previous line if you want to get statistics on the number of opened emails --><CustomBlock name=\"letter.intro\" title=\"Personalization\"><Greeting/></CustomBlock></body></html>";
                    this.txtTextContent.Text =
                        "<Text>Copyright (c) 1996-2009 Constant Contact. All rights reserved. Except as permitted under a separate written agreement with Constant Contact, neither the Constant Contact software, nor any content that appears on any Constant Contact site, including but not limited to, web pages, newsletters, or templates may be reproduced, republished, repurposed, or distributed without the prior written permission of Constant Contact. For inquiries regarding reproduction or distribution of any Constant Contact material, please contact legal@constantcontact.com.</Text>";

                    this.chkListContactLists.DataSource = contactsList;
                    this.chkListContactLists.DataBind();

                    this.txtCampaignName.Text = DateTime.Now.ToString("MMM dd yyyy");
                    this.txtCampaignSubject.Text = "News from";
                    this.txtViewAsWebageText.Text = "Having trouble viewing this email?";
                    this.txtViewAsWebpageLinkText.Text = "Click here";

                    this.ddlCountry.DataSource = Utils.GetAllCountries();
                    this.ddlCountry.DataTextField = "Name";
                    this.ddlCountry.DataValueField = "Code";
                    this.ddlCountry.DataBind();

                    this.ddlUSStates.DataSource = Utils.GetAllUSStates();
                    this.ddlUSStates.DataTextField = "Name";
                    this.ddlUSStates.DataValueField = "Code";
                    this.ddlUSStates.DataBind();

                    IList<Email> emailsList = Utility.GetEmailCollection(this.AuthenticationData);
                    this.ddlFromEmail.DataSource = emailsList;
                    this.ddlFromEmail.DataValueField = "ID";
                    this.ddlFromEmail.DataTextField = "Title";
                    this.ddlFromEmail.DataBind();

                    this.ddlReplyToEmail.DataSource = emailsList;
                    this.ddlReplyToEmail.DataValueField = "ID";
                    this.ddlReplyToEmail.DataTextField = "Title";
                    this.ddlReplyToEmail.DataBind();
                }
                catch (ConstantException ce)
                {
                    #region display alert message

                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(@"<script language='javascript'>");
                    stringBuilder.AppendFormat(@"alert('{0}')", ce.Message);
                    stringBuilder.Append(@"</script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "AlertMessage", stringBuilder.ToString());

                    #endregion
                }

                if (this.Session["_ecid_"] != null)
                {
                    string id = this.Session["_ecid_"].ToString();
                    this.ECID = id;
                    this.Session.Remove("_ecid_");
                    this.lblAddInfo.Text = string.Concat("Edit Email Campaign ", this.ECID);
                    this.btnAdd.Text = "Update Email Campaign";
                    
                    EmailCampaign eCampaign = Utility.GetEmailCampaignById(this.AuthenticationData, id);

                    if (eCampaign != null)
                    {
                        this.txtCampaignName.Text = eCampaign.Name;
                        this.txtCampaignSubject.Text = eCampaign.Subject;
                        this.txtFromName.Text = eCampaign.FromName;
                        this.ddlFromEmail.SelectedValue = eCampaign.FromEmailID.ToString();
                        this.ddlReplyToEmail.SelectedValue = eCampaign.ReplyToEmailID.ToString();
                        this.rbViewWebpageY.Checked = eCampaign.ViewAsWebpage;
                        this.rbViewWebpageN.Checked = this.rbViewWebpageY.Checked;
                        this.txtViewAsWebpageLinkText.Text = eCampaign.ViewAsWebpageLinkText;
                        this.txtViewAsWebageText.Text = eCampaign.ViewAsWebpageText;
                        this.rbPermReminderY.Checked = eCampaign.PermissionReminder;
                        this.txtGreetingSalutation.Text = eCampaign.GreetingSalutation;

                        //this.ddlGreetingName.SelectedValue = eCampaign.GreetingName.Replace("FirstAndLastName", "First & Last Name").Replace("FirstName", "First Name").Replace("LastName","Last Name").Replace("NONE", "None");
                        if (!string.IsNullOrEmpty(eCampaign.GreetingName))
                            this.ddlGreetingName.SelectedValue = eCampaign.GreetingName.Replace("FirstAndLastName", "First & Last Name").Replace("FirstName", "First Name").Replace("LastName", "Last Name").Replace("NONE", "None");
                        
                        this.txtGreetingString.Text = eCampaign.GreetingString;
                        this.txtOrganizationName.Text = eCampaign.OrganizationName;
                        this.txtOrganizationAddress1.Text = eCampaign.OrganizationAddress1;
                        this.txtOrganizationAddress2.Text = eCampaign.OrganizationAddress2;
                        this.txtOrganizationAddress3.Text = eCampaign.OrganizationAddress3;
                        this.txtOrganizationCity.Text = eCampaign.OrganizationCity;
                        this.rbViewWebpageY.Checked = eCampaign.ViewAsWebpage;
                        this.rbViewWebpageN.Checked = !this.rbViewWebpageY.Checked;
                        this.ddlUSStates.SelectedValue = eCampaign.OrganizationState;
                        this.txtOrganizationInternationalState.Text = eCampaign.OrganizationInternationalState;
                        this.txtOrganizationPostalCode.Text = eCampaign.OrganizationPostalCode;
                        this.ddlCountry.SelectedValue = eCampaign.OrganizationCountry;
                        this.rbtnIncludeForwardEmail.SelectedValue = eCampaign.IncludeForwardEmail.ToString().ToLower();
                        this.txtForwardEmailLinkText.Text = eCampaign.ForwardEmailLinkText;
                        this.rbtnIncludeSubscribeLink.SelectedValue = eCampaign.IncludeSubscribeLink.ToString().ToLower();
                        this.txtSubscribeLinkText.Text = eCampaign.SubscribeLinkText;
                        this.txtContent.Text = eCampaign.Content;
                        this.txtXContent.Text = eCampaign.XContent;
                        this.txtTextContent.Text = eCampaign.TextContent;
                        this.rbEmailContentFormat.SelectedValue = string.IsNullOrEmpty(eCampaign.Content) ? "XHTML" : "HTML";
                        this.rbPermReminderY.Checked = string.IsNullOrEmpty(this.txtPermReminder.Text) ? false : true;

                        #region contact lists
                        // loop throught all the contact lists
                        foreach (ListItem item in this.chkListContactLists.Items)
                        {
                            if (eCampaign.ContactLists == null || eCampaign.ContactLists.Count <= 0) continue;

                            ListItem item1 = item;
                            ContactList c = eCampaign.ContactLists.FirstOrDefault(i => i.Id.Equals(item1.Value));

                            if (c != null)
                            {
                                item.Selected = true;
                            }
                        }

                        //chkListContactLists.Enabled = false;
                        #endregion
                    }
                }
            }
        }

        //protected void ckUnsubscr_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (ckUnsubscr.Checked)
        //    {
        //        chkListContactLists.Enabled = false;
        //        foreach (ListItem item in chkListContactLists.Items)
        //        {
        //            item.Selected = false;
        //        }
        //    }
        //    else
        //    {
        //        chkListContactLists.Enabled = true;
        //    }
        //}

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            this.Page.Validate();

            if (!Page.IsValid)
            {
                return;
            }

            if(string.IsNullOrEmpty(this.ECID))
            {
                List<EmailCampaign> list = Utility.GetEmailCampaignCollection(this.AuthenticationData);

                if (list != null && list.Count > 0)
                {
                    EmailCampaign ec = list.FirstOrDefault(i => i.Name.Equals(this.txtCampaignName.Text.Trim()));

                    if (ec != null)
                    {
                        #region display alert message

                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(@"<script language='javascript'>");
                        stringBuilder.AppendFormat(@"alert('{0}')",
                                                   "The name of this Email has already been used. Please specify a unique name.\\nNote: Names of Emails that have been removed may not be re-used.");
                        stringBuilder.Append(@"</script>");
                        ClientScript.RegisterStartupScript(typeof (Page), "AlertMessage", stringBuilder.ToString());

                        #endregion

                        return;
                    }
                }
            }

            if (this.ddlFromEmail.Items.Count == 0 || this.ddlReplyToEmail.Items.Count == 0)
            {
                #region display alert message

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"<script language='javascript'>");
                stringBuilder.AppendFormat(@"alert('{0}')", "No FromEmail or ReplyToEmail specified!");
                stringBuilder.Append(@"</script>");
                ClientScript.RegisterStartupScript(typeof(Page), "AlertMessage", stringBuilder.ToString());

                #endregion

                return;
            }

            if (this.ddlCountry.SelectedValue.Equals("0"))
            {
                #region display alert message

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"<script language='javascript'>");
                stringBuilder.AppendFormat(@"alert('{0}')", "Please select a country!");
                stringBuilder.Append(@"</script>");
                ClientScript.RegisterStartupScript(typeof(Page), "AlertMessage", stringBuilder.ToString());

                #endregion

                return;
            }

            if (this.ddlCountry.SelectedValue.Equals("us"))
            {
                if (!string.IsNullOrEmpty(Server.HtmlEncode(this.txtOrganizationInternationalState.Text.Trim())))
                {
                    #region display alert message

                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(@"<script language='javascript'>");
                    stringBuilder.AppendFormat(@"alert('{0}')",
                                               "You can not specify an International state if you selected United States!");
                    stringBuilder.Append(@"</script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "AlertMessage", stringBuilder.ToString());

                    #endregion
                    
                    return;
                }
            }

            if (!this.ddlCountry.SelectedValue.Equals("0"))
            {
                if (!this.ddlUSStates.SelectedValue.Equals("0") &&
                    !this.ddlCountry.SelectedValue.Equals("us"))
                {
                    #region display alert message

                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(@"<script language='javascript'>");
                    stringBuilder.AppendFormat(@"alert('{0}')",
                                               "You can not specify an U.S. state if you didnt selected United States as country!");
                    stringBuilder.Append(@"</script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "AlertMessage", stringBuilder.ToString());

                    #endregion

                    return;
                }
            }

            if (!this.ddlUSStates.SelectedValue.Equals("0") &&
                !string.IsNullOrEmpty(Server.HtmlEncode(this.txtOrganizationInternationalState.Text.Trim())))
            {
                #region display alert message

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"<script language='javascript'>");
                stringBuilder.AppendFormat(@"alert('{0}')",
                                           "Please specify an US state or an International state! Not both.");
                stringBuilder.Append(@"</script>");
                ClientScript.RegisterStartupScript(typeof (Page), "AlertMessage", stringBuilder.ToString());

                #endregion

                return;
            }

            if (this.rbEmailContentFormat.SelectedValue.Equals("HTML") && string.IsNullOrEmpty(this.txtContent.Text.Trim()))
            {
                #region display alert message

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"<script language='javascript'>");
                stringBuilder.AppendFormat(@"alert('{0}')",
                                           "Content cannot be empty.");
                stringBuilder.Append(@"</script>");
                ClientScript.RegisterStartupScript(typeof(Page), "AlertMessage", stringBuilder.ToString());

                #endregion

                return;
            }

            if (this.rbEmailContentFormat.SelectedValue.Equals("XHTML") && string.IsNullOrEmpty(this.txtXContent.Text.Trim()))
            {
                #region display alert message

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"<script language='javascript'>");
                stringBuilder.AppendFormat(@"alert('{0}')",
                                           "XContent cannot be empty.");
                stringBuilder.Append(@"</script>");
                ClientScript.RegisterStartupScript(typeof(Page), "AlertMessage", stringBuilder.ToString());

                #endregion

                return;
            }

            if (string.IsNullOrEmpty(this.ECID))
            {
                bool hasSelectedItems = false;
                foreach (ListItem item in this.chkListContactLists.Items)
                {
                    if (!item.Selected) continue;
                    hasSelectedItems = true;
                    break;
                }

                if (!hasSelectedItems)
                {
                    #region display alert message

                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(@"<script language='javascript'>");
                    stringBuilder.AppendFormat(@"alert('{0}')", "Please select at least on contact list!");
                    stringBuilder.Append(@"</script>");
                    ClientScript.RegisterStartupScript(typeof (Page), "AlertMessage", stringBuilder.ToString());

                    #endregion

                    return;
                }
            }

            try
            {
                // create new Email Campaign
                EmailCampaign campaign = GetEmailCampaignInformation();
                if (campaign != null)
                {
                    if (string.IsNullOrEmpty(campaign.ID))
                        Utility.CreateNewEmailCampaign(this.AuthenticationData, campaign);
                    else
                        Utility.UpdateEmailCampaign(this.AuthenticationData, campaign);
                    if (string.IsNullOrEmpty(this.ECID))
                        this.ECID = "0";
                    Response.Redirect(string.Format("~/EmailCampaignPages/AddCampaignConfirmation.aspx?ecid={0}", this.ECID));
                }
                else
                {
                    #region display alert message
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(@"<script language='javascript'>");
                    stringBuilder.AppendFormat(@"alert('{0}')", "There was a problem reading fields!");
                    stringBuilder.Append(@"</script>");
                    ClientScript.RegisterStartupScript(typeof(Page), "AlertMessage", stringBuilder.ToString());
                    #endregion

                    return;
                }
            }
            catch (ConstantException ce)
            {
                #region display alert message
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"<script language='javascript'>");
                stringBuilder.AppendFormat(@"alert('{0}')", ce.Message);
                stringBuilder.Append(@"</script>");
                ClientScript.RegisterStartupScript(typeof(Page), "AlertMessage", stringBuilder.ToString());
                #endregion
            }
        }

        #region Private Methods
        /// <summary>
        /// Get Email Campaign information from the form
        /// </summary>
        /// <returns>EmailCampaign</returns>
        private EmailCampaign GetEmailCampaignInformation()
        {
            EmailCampaign campaign;

            if (string.IsNullOrEmpty(this.ECID))
            {
                campaign = new EmailCampaign();
                campaign.ContactLists = new List<ContactList>();
                campaign.Date = DateTime.Now;
                campaign.ID = null;
            }
            else
            {
                campaign = Utility.GetEmailCampaignById(this.AuthenticationData, this.ECID);
                campaign.ID = this.ECID;
            }

            #region contact lists
            // loop throught all the checkbox controls
            campaign.ContactLists = new List<ContactList>();
            foreach (ListItem item in this.chkListContactLists.Items)
            {
                if (!item.Selected) continue;
                ContactList contactList = new ContactList();
                contactList.Name = item.Text;
                contactList.Id = item.Value;
                campaign.ContactLists.Add(contactList);
            }
            #endregion

            campaign.Name = Server.HtmlEncode(this.txtCampaignName.Text.Trim());
            campaign.Subject = Server.HtmlEncode(this.txtCampaignSubject.Text.Trim());
            campaign.FromName = Server.HtmlEncode(this.txtFromName.Text.Trim());
            campaign.FromEmail = Server.HtmlEncode(this.ddlFromEmail.SelectedItem.Text.Trim());
            campaign.FromEmailID = Convert.ToInt32(Server.HtmlEncode(this.ddlFromEmail.SelectedItem.Value.Trim()));
            campaign.ReplyToEmail = Server.HtmlEncode(this.ddlReplyToEmail.SelectedItem.Text.Trim());
            campaign.ReplyToEmailID = Convert.ToInt32(Server.HtmlEncode(this.ddlReplyToEmail.SelectedItem.Value.Trim()));
            campaign.ViewAsWebpage = this.rbViewWebpageY.Checked;
            campaign.ViewAsWebpageLinkText = campaign.ViewAsWebpage ? Server.HtmlEncode(this.txtViewAsWebpageLinkText.Text.Trim()) : "";
            campaign.ViewAsWebpageText = campaign.ViewAsWebpage ? Server.HtmlEncode(this.txtViewAsWebageText.Text.Trim()) : "";
            campaign.PermissionReminder = this.rbPermReminderY.Checked ? true : false;
            campaign.PermissionReminderText = campaign.PermissionReminder ? this.txtPermReminder.Text : "";
            campaign.GreetingSalutation = Server.HtmlEncode(this.txtGreetingSalutation.Text.Trim());
            campaign.GreetingName = this.ddlGreetingName.SelectedValue.Replace(" ", "").Replace("&", "And");
            campaign.GreetingString = Server.HtmlEncode(this.txtGreetingString.Text.Trim());
            campaign.OrganizationName = Server.HtmlEncode(this.txtOrganizationName.Text.Trim());
            campaign.OrganizationAddress1 = Server.HtmlEncode(this.txtOrganizationAddress1.Text.Trim());
            campaign.OrganizationAddress2 = Server.HtmlEncode(this.txtOrganizationAddress2.Text.Trim());
            campaign.OrganizationAddress3 = Server.HtmlEncode(this.txtOrganizationAddress3.Text.Trim());
            campaign.OrganizationCity = Server.HtmlEncode(this.txtOrganizationCity.Text.Trim());
            campaign.OrganizationState = this.ddlUSStates.SelectedValue.Equals("0") ? "" : this.ddlUSStates.SelectedValue;
            campaign.OrganizationInternationalState = Server.HtmlEncode(this.txtOrganizationInternationalState.Text.Trim());
            campaign.OrganizationPostalCode = Server.HtmlEncode(this.txtOrganizationPostalCode.Text.Trim());
            campaign.OrganizationCountry = this.ddlCountry.SelectedValue;
            campaign.IncludeForwardEmail = Convert.ToBoolean(this.rbtnIncludeForwardEmail.SelectedItem.Value);
            campaign.ForwardEmailLinkText = campaign.IncludeForwardEmail ? Server.HtmlEncode(this.txtForwardEmailLinkText.Text.Trim()) : "";
            campaign.IncludeSubscribeLink = Convert.ToBoolean(this.rbtnIncludeSubscribeLink.SelectedItem.Value);
            campaign.SubscribeLinkText = campaign.IncludeSubscribeLink ? Server.HtmlEncode(this.txtSubscribeLinkText.Text.Trim()) : "";
            campaign.Content = Server.HtmlEncode(this.txtContent.Text.Trim());
            campaign.XContent = Server.HtmlEncode(this.txtXContent.Text.Trim());
            campaign.TextContent = Server.HtmlEncode(this.txtTextContent.Text.Trim());
            campaign.EmailContentFormat = this.rbEmailContentFormat.SelectedValue;
            
            return campaign;
        }
        #endregion
    }
}