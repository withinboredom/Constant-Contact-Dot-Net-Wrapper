using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConstantContactBO;
using ConstantContactUtility;
using UploadContactForm.App_Code;
using Utils=ConstantContactUtility.Utils;
using EmailCampaign = ConstantContactBO.Entities.EmailCampaign;

namespace UploadContactForm.EmailCampaignPages
{
    public partial class ListEmailCampaigns : Page
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Session.Remove("_ecid_");
            if (!this.Page.IsPostBack)
            {
                this.ddlStatus.DataSource = Utils.GetEmailCampaignsStatusList();
                this.ddlStatus.DataBind();
                this.bindData();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.bindData();
        }

        private void binGridHeaderNames()
        {
            this.gCampaigns.HeaderRow.Cells[0].Text = "Name";
            this.gCampaigns.HeaderRow.Cells[1].Text = "State";
            this.gCampaigns.HeaderRow.Cells[2].Text = "Date";
            this.gCampaigns.HeaderRow.Cells[3].Text = "Edit";
            this.gCampaigns.HeaderRow.Cells[4].Text = "Remove";

            this.gCampaigns.HeaderRow.Cells[1].Width = 120;
            this.gCampaigns.HeaderRow.Cells[2].Width = 180;
            this.gCampaigns.HeaderRow.Cells[3].Width = 60;
            this.gCampaigns.HeaderRow.Cells[4].Width = 60;
        }

        private void bindData()
        {
            List<EmailCampaign> source = this.ddlStatus.SelectedItem.Text.Equals("ALL") ? Utility.GetEmailCampaignCollection(this.AuthenticationData) : Utility.GetEmailCampaignCollection(this.AuthenticationData, this.ddlStatus.SelectedItem.Text);
            
            this.lblNoEntries.Text = string.Empty;
            
            if (source == null || source.Count == 0)
            {
                this.lblNoEntries.Text = "No Email Campaigns found.";
                source = null;
            }

            this.gCampaigns.DataSource = source;
            this.gCampaigns.DataBind();

            if (source != null)
                binGridHeaderNames();
        }

        protected void gCampaigns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch(e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    {
                        Label lblName = e.Row.FindControl("lN") as Label;
                        Label lblCStatus = e.Row.FindControl("lS") as Label;
                        Label lblDate = e.Row.FindControl("lD") as Label;
                        ImageButton iEdit = e.Row.FindControl("iE") as ImageButton;
                        ImageButton iRemove = e.Row.FindControl("iR") as ImageButton;
                        EmailCampaign source = e.Row.DataItem as EmailCampaign;

                        if (source != null)
                        {
                            if (!source.State.ToString().Equals(CampaignState.Draft.ToString(), StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (iEdit != null)
                                {
                                    iEdit.ImageUrl = this.ResolveUrl("~/App_Themes/DefaultTheme/noedit.gif");
                                    iEdit.Enabled = false;
                                    iEdit.ToolTip = "This campaign cannot be edited anymore.";
                                    iEdit.CommandName = null;
                                }

                                if (iRemove != null)
                                {
                                    iRemove.Enabled = false;
                                    iRemove.ToolTip = "This campaign cannot be removed.";
                                    iRemove.CommandName = null;
                                }
                            }

                            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;

                            if (lblName != null)
                            {
                                lblName.Text = source.Name;
                                lblName.Attributes.Add("ecid", source.ID);
                            }

                            if (lblCStatus != null)
                            {
                                lblCStatus.Text = source.State.ToString();
                            }

                            if (lblDate != null)
                            {
                                lblDate.Text = source.Date.ToShortTimeString();
                            }
                        }
                        break;
                    }
            }
        }

        protected void gCampaigns_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gCampaigns.PageIndex = e.NewPageIndex;
            this.bindData();
        }

        protected void gCampaigns_RowEditing(object sender, GridViewEditEventArgs e)
        {
// ReSharper disable PossibleNullReferenceException
            this.Session["_ecid_"] = this.gCampaigns.DataKeys[e.NewEditIndex].Value.ToString();
// ReSharper restore PossibleNullReferenceException
            this.Response.Redirect("~/EmailCampaignPages/AddEmailCampaign.aspx", true);
        }

        protected void gCampaigns_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
// ReSharper disable PossibleNullReferenceException
            Utility.DeleteEmailCampaign(this.AuthenticationData, this.gCampaigns.DataKeys[e.RowIndex].Value.ToString());
// ReSharper restore PossibleNullReferenceException
            this.bindData();
        }

    }
}