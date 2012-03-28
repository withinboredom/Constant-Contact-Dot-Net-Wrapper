using System;

public partial class Master : System.Web.UI.MasterPage
{
	protected void Page_Load(object sender, EventArgs e)
	{
	}

    protected void cclogo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }
}
