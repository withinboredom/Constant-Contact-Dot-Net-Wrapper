<%@ Page Title="" Language="C#" MasterPageFile="~/EmailCampaignMasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="AddCampaignConfirmation.aspx.cs" Inherits="Pages_AddCampaignConfirmation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MasterContentPlaceHolder" runat="Server">
    <div>
        <asp:Label ID="lblInfo" runat="server" Font-Bold="True" Font-Names="Calibri" Font-Size="16pt"></asp:Label>
    </div>
    <br />
    <asp:Label ID="lbl3" runat="server" Text="Please" Font-Names="Calibri"></asp:Label>
    <asp:HyperLink ID="hLink1" runat="server" NavigateUrl="~/Default.aspx" Font-Names="Calibri">CLICK HERE</asp:HyperLink>
    <asp:Label ID="lbl4" runat="server" Text="to return to our main page." Font-Names="Calibri"></asp:Label>
</asp:Content>