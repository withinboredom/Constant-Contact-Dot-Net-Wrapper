<%@ Page Title="List Email Campaigns" StyleSheetTheme="DefaultTheme" Language="C#" MasterPageFile="~/EmailCampaignMasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="ListEmailCampaigns.aspx.cs" Inherits="UploadContactForm.EmailCampaignPages.ListEmailCampaigns" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MasterContentPlaceHolder" Runat="Server">

    <div style="margin-left: 20px; margin-top: 10px; margin-right:20px;">
        <asp:Label runat="server" ID="lblTitle" Text="List all Email Campaigns" Font-Names="Calibri" Font-Size="20pt"></asp:Label>
        <br />
        <br />
        <asp:Label Font-Names="Calibri" ID="lblStatus" Text="Email Campaign Status:" runat="server"></asp:Label>
        <asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList>
        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" />
        <br />
        <br />
        <asp:Panel ID="Panel1" runat="server">
            <ASP:GRIDVIEW Font-Names="Calibri" id="gCampaigns" runat="server" DataKeyNames="ID" OnRowDataBound="gCampaigns_RowDataBound" OnRowEditing="gCampaigns_RowEditing" OnPageIndexChanging="gCampaigns_PageIndexChanging" OnRowDeleting="gCampaigns_RowDeleting" skinid="custgrid" allowpaging="True" pagesize="7">
				<COLUMNS>
				    <asp:TemplateField>
						<ItemTemplate>
							<asp:Label ID="lN" runat="server"></asp:Label>
						</ItemTemplate>
				    </asp:TemplateField>
				    <asp:TemplateField>
						<ItemTemplate>
							<asp:Label ID="lS" runat="server"></asp:Label>
						</ItemTemplate>
				    </asp:TemplateField>
				    <asp:TemplateField>
						<ItemTemplate>
							<asp:Label ID="lD" runat="server"></asp:Label>
						</ItemTemplate>
				    </asp:TemplateField>
				    <asp:TemplateField>
						<ItemTemplate>
							<asp:ImageButton ImageUrl="~/App_Themes/DefaultTheme/edit.gif" id="iE" runat="server" CommandName="edit"/>
						</ItemTemplate>
				    </asp:TemplateField>
				    <asp:TemplateField>
						<ItemTemplate>
							<asp:ImageButton ImageUrl="~/App_Themes/DefaultTheme/rem.gif" id="iR" runat="server" CommandName="delete"/>
						</ItemTemplate>
				    </asp:TemplateField>
				</COLUMNS>
			</ASP:GRIDVIEW>
        </asp:Panel>
        <center>
			<asp:Label ID="lblNoEntries" Font-Names="Calibri" runat="server"></asp:Label>
		</center>
    </div>

</asp:Content>

