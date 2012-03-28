using System;

public partial class Pages_AddCampaignConfirmation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblInfo.Text = "Email Campaign has been added to your list.";
        if (!this.Request["ecid"].Equals("0"))
        {
            lblInfo.Text = "Email Campaign has been updated.";
        }

        this.Session.Remove("_ecid_");
    }
}
