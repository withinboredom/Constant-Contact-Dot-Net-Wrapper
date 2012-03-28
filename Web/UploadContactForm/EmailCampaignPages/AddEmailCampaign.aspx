<%@ Page Title="" Language="C#" ValidateRequest="false" MasterPageFile="~/EmailCampaignMasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="AddEmailCampaign.aspx.cs" Inherits="UploadContactForm.EmailCampaignPages.AddEmailCampaign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MasterContentPlaceHolder" Runat="Server">

    <div style="margin-left: 30px; margin-top: 10px">
        <asp:Label runat="server" ID="lblAddInfo" Font-Names="Calibri" Font-Size="20pt"></asp:Label>
    </div>

    <table style="margin-top: 10px; vertical-align: top">
        <tr>
            <td style="padding-left: 25px;" valign="top">
                <table style="border-right: green thin solid; border-top: green thin solid; border-left: green thin solid; border-bottom: green thin solid;">
                    <tr>
                        <td style="width: 200px">
                            <asp:Label ID="lblName" Text="Campaign Name:" ToolTip="Name for the Campaign." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCampaignName" runat="server" ToolTip="Name for the Campaign." Font-Names="Calibri" Font-Size="11pt" Width="254px" MaxLength="50" TabIndex="1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqName" Display="Dynamic" runat="server" ControlToValidate="txtCampaignName">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="lnInfo" ForeColor="GrayText" runat="server" Font-Bold="False" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="8pt" Text="This name is NOT displayed in your emails. It is for your personal use, to help you identify each unique email."></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblSubject" Text="Subject:" ToolTip="Subject line for the email." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCampaignSubject" runat="server" ToolTip="Subject line for the email." Font-Names="Calibri" Font-Size="11pt" Width="254px" MaxLength="50" TabIndex="2"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqsubj" Display="Dynamic" runat="server" ControlToValidate="txtCampaignSubject">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblFromName" Text="From Name:" ToolTip="The name that is displayed in the from box in the recipients email client." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtFromName" runat="server" ToolTip="The name that is displayed in the from box in the recipients email client." Font-Names="Calibri" Font-Size="11pt" Width="254px" MaxLength="50" TabIndex="3"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqFromName" Display="Dynamic" runat="server" ControlToValidate="txtFromName">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblFromEmail" Text="From Email Address:" ToolTip="Email address entry represents From address shows the address that the email originated from." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlFromEmail" runat="server" ToolTip="Email address entry represents From address shows the address that the email originated from." TabIndex="4">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblReplyToEmail" Text="Reply Email Address:" ToolTip="Email address entry represents the field used by an email client's reply function." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlReplyToEmail" runat="server" ToolTip="Email address entry represents the field used by an email client's reply function." TabIndex="5">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="vertical-align:top;">
                            <asp:Label ID="lblPermissionReminder" Text="Permission Reminder:" ToolTip="Whether to show a permission reminder at the top of the email allowing recipients to confirm their opt in." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
							<asp:RadioButton ID="rbPermReminderY" GroupName="rbpr" onClick="permRemVisibility('visible');" Checked="true" runat="server" Text="YES" ToolTip="Whether to show a permission reminder at the top of the email allowing recipients to confirm their opt in."  Font-Names="Calibri" Font-Size="11pt" TabIndex="6" />
							<asp:RadioButton ID="rbPermReminderN" GroupName="rbpr" onClick="permRemVisibility('hidden');" runat="server" Text="NO" ToolTip="Whether to show a permission reminder at the top of the email allowing recipients to confirm their opt in."  Font-Names="Calibri" Font-Size="11pt" TabIndex="6" />
                        </td>
                    </tr>
                    
                    <tr id="trPRT" runat="server" visible="true">
                        <td style="vertical-align:top;">
                            <asp:Label ID="lblPermissionReminderText" Text="Permission Reminder Text:" ToolTip="Whether to show a permission reminder at the top of the email allowing recipients to confirm their opt in." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
							<asp:TextBox id="txtPermReminder" runat="server" TextMode="MultiLine" Width="254px" Rows="7"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblViewAsWebpage" Text="Webpage Version:" ToolTip="This allows contacts who cannot view images in their email program to open your email in a browser window." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:RadioButton ID="rbViewWebpageY" GroupName="rbvwp" onClick="viewRemVisibility('visible');" Checked="true" runat="server" Text="YES" ToolTip="This allows contacts who cannot view images in their email program to open your email in a browser window."  Font-Names="Calibri" Font-Size="11pt" TabIndex="8" />
							<asp:RadioButton ID="rbViewWebpageN" GroupName="rbvwp" onClick="viewRemVisibility('hidden');" runat="server" Text="NO" ToolTip="This allows contacts who cannot view images in their email program to open your email in a browser window."  Font-Names="Calibri" Font-Size="11pt" TabIndex="8" />                           
                        </td>
                    </tr>
                    
                    <tr id="trVWT" runat="server" visible="true">
                        <td>
                            <asp:Label ID="lblViewAsWebageText" Text="Text:" ToolTip="The text displayed together with the LinkText at the top of your email." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtViewAsWebageText" runat="server" ToolTip="The text displayed together with the LinkText at the top of your email." Font-Names="Calibri" Font-Size="11pt" Width="254px" MaxLength="50" TabIndex="9"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr id="trVWLT" runat="server" visible="true">
                        <td>
                            <asp:Label ID="lblViewAsWebpageLinkText" Text="Link Text:" ToolTip="The text for the actual link in the View As Webpage link in the email." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtViewAsWebpageLinkText" runat="server" ToolTip="The text for the actual link in the View As Webpage link in the email." Font-Names="Calibri" Font-Size="11pt" Width="254px" MaxLength="50" TabIndex="10"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblGreetingSalutation" Text="Greeting Salutation:" ToolTip="Describes the chosen salutation to be used at the opening of the email." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtGreetingSalutation" runat="server" ToolTip="Describes the chosen salutation to be used at the opening of the email." Font-Names="Calibri" Font-Size="11pt" Width="254px" MaxLength="50" TabIndex="11"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblGreetingName" Text="Greeting Name:" ToolTip="Indicates if the email greeting should include just the recipients FirstName, just the LastName, Both, or neither (None)." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlGreetingName" ToolTip="Indicates if the email greeting should include just the recipients FirstName, just the LastName, Both, or neither (None)." runat="server" Width="150px" Font-Names="Calibri" Font-Size="11pt" TabIndex="12">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblGreetingString" Text="Greeting String:" ToolTip="Allows you to specify the full greeting string instead of the components of the previous two properties." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtGreetingString" runat="server" ToolTip="Allows you to specify the full greeting string instead of the components of the previous two properties." Font-Names="Calibri" Font-Size="11pt" Width="254px" MaxLength="50" TabIndex="13"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblOrganizationName" Text="Organization Name:" ToolTip="Organization Name." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOrganizationName" runat="server" ToolTip="Organization Name." Font-Names="Calibri" Font-Size="11pt" Width="254px" MaxLength="50" TabIndex="14"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqOrg" Display="Dynamic" runat="server" ControlToValidate="txtOrganizationName">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblOrganizationAddress1" Text="Address 1:" ToolTip="Line 1 of the organization address for use in the email footer." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOrganizationAddress1" runat="server" ToolTip="Line 1 of the organization address for use in the email footer." Font-Names="Calibri" Font-Size="11pt" Width="254px" MaxLength="50" TabIndex="15"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqAdr" Display="Dynamic" runat="server" ControlToValidate="txtOrganizationAddress1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblOrganizationAddress2" Text="Address 2:" ToolTip="Line 2 of the organization address for use in the email footer." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOrganizationAddress2" runat="server" ToolTip="Line 2 of the organization address for use in the email footer." Font-Names="Calibri" Font-Size="11pt" Width="254px" MaxLength="50" TabIndex="16"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblOrganizationAddress3" Text="Address 3:" ToolTip="Line 3 of the organization address for use in the email footer." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOrganizationAddress3" runat="server" ToolTip="Line 3 of the organization address for use in the email footer." Font-Names="Calibri" Font-Size="11pt" Width="254px" MaxLength="50" TabIndex="17"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblOrganizationCity" Text="City:" ToolTip="City of the organization for use in the email footer." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOrganizationCity" runat="server" ToolTip="City of the organization for use in the email footer." Font-Names="Calibri" Font-Size="11pt" Width="254px" MaxLength="50" TabIndex="18"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqCity" Display="Dynamic" runat="server" ControlToValidate="txtOrganizationCity">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblOrganizationState" Text="State:" ToolTip="State of the organization for use in the email footer." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
							<asp:DropDownList id="ddlUSStates" Width="254" runat="server" TabIndex="19"></asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblOrganizationInternationalState" Text="International State:" ToolTip="International &quot;State&quot; if outside the US for use in the email footer." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOrganizationInternationalState" runat="server" ToolTip="International &quot;State&quot; if outside the US for use in the email footer." Font-Names="Calibri" Font-Size="11pt" Width="254px" MaxLength="50" TabIndex="20"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblOrganizationPostalCode" Text="Zip/Postal Code:" ToolTip="PostalCode/Zipcode of the organization for use in the email footer." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOrganizationPostalCode" runat="server" ToolTip="PostalCode/Zipcode of the organization for use in the email footer." Font-Names="Calibri" Font-Size="11pt" Width="254px" MaxLength="50" TabIndex="21"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqPostalCode" Display="Dynamic" runat="server" ControlToValidate="txtOrganizationPostalCode">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblOrganizationCountry" Text="Country:" ToolTip="Country of the organization for use in the email footer." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
							<asp:DropDownList id="ddlCountry" Width="245" runat="server" TabIndex="22"></asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblIncludeForwardEmail" Text="Forward Email to a Friend:" ToolTip="Indicates if the email should include a Forward This Email link." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rbtnIncludeForwardEmail" ToolTip="Indicates if the email should include a Forward This Email link." runat="server" RepeatDirection="Horizontal" Font-Names="Calibri" Font-Size="11pt" TabIndex="23">
                                <asp:ListItem Selected="True" Value="true">YES</asp:ListItem>
                                <asp:ListItem Value="false">NO</asp:ListItem>
                            </asp:RadioButtonList>                            
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblForwardEmailLinkText" Text="Forward Email Link Text:" ToolTip="If IncludeForwardEmail property is set to YES, then this property should be specified and will appear in the email as the text of the link." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtForwardEmailLinkText" runat="server" ToolTip="If IncludeForwardEmail property is set to YES, then this property should be specified and will appear in the email as the text of the link." Font-Names="Calibri" Font-Size="11pt" Width="254px" MaxLength="50" TabIndex="24"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblIncludeSubscribeLink" Text="Include Subscribe Link:" ToolTip="Indicates if the email should include a Subscribe link." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rbtnIncludeSubscribeLink" ToolTip="Indicates if the email should include a Subscribe link." runat="server" RepeatDirection="Horizontal" Font-Names="Calibri" Font-Size="11pt" TabIndex="25">
                                <asp:ListItem Selected="True" Value="true">YES</asp:ListItem>
                                <asp:ListItem Value="false">NO</asp:ListItem>
                            </asp:RadioButtonList>                            
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblEmailContentFormat" Text="Email Content Format:" ToolTip="Describes whether email content is based on HTML or XHTML." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rbEmailContentFormat" ToolTip="Describes whether email content is based on HTML or XHTML." runat="server" RepeatDirection="Horizontal" Font-Names="Calibri" Font-Size="11pt" TabIndex="26">
                                <asp:ListItem Selected="True" Value="HTML">HTML</asp:ListItem>
                                <asp:ListItem Value="XHTML">XHTML</asp:ListItem>
                            </asp:RadioButtonList>                            
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblSubscribeLinkText" Text="Subscribe Link Text:" ToolTip="If IncludeSubscribeLink property is set to YES, then this property should be specified and will appear in the email as the text of the link." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSubscribeLinkText" runat="server" ToolTip="If IncludeSubscribeLink property is set to YES, then this property should be specified and will appear in the email as the text of the link." Font-Names="Calibri" Font-Size="11pt" Width="254px" MaxLength="50" TabIndex="27"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblContactLists" Text="Contact Lists:" ToolTip="Collection of ContactList entries that are associated with this email Campaign." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
							<%--<asp:CheckBox ID="ckUnsubscr" AutoPostBack="true" Text="UNSUBSCRIBE FROM ALL LISTS" runat="server" Font-Names="Calibri" Font-Size="11pt" OnCheckedChanged="ckUnsubscr_CheckedChanged" TabIndex="28" />--%>
							<asp:Label ID="lblCLI" runat="server" Text="Send campaign to following lists:" Font-Names="Calibri" Font-Size="11pt"></asp:Label>
							<br />
                            <asp:Panel ID="contactListsPanel" runat="server" BackColor="White" BorderColor="Green" BorderWidth="1px" Height="348px" Width="245px" ScrollBars="Both">
                                <asp:CheckBoxList ID="chkListContactLists" runat="server" Width="300px" DataTextField="Name" DataValueField="Id" CellSpacing="5" Font-Names="Calibri" Font-Size="11pt" TabIndex="29">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
            
            <td style="padding-left: 25px;" valign="top">
                <table style="border-right: green thin solid; border-top: green thin solid; border-left: green thin solid; border-bottom: green thin solid;">
                    <tr>
                        <td>
                            <asp:Label ID="lblContent" Text="Content:" ToolTip="The full HTML content of the email." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtContent" runat="server" ToolTip="The full HTML content of the email." Font-Names="Calibri" Font-Size="11pt" Width="600px" Height="333px" TextMode="MultiLine" TabIndex="30"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblXContent" Text="X Content:" ToolTip="The full XHTML content of the email." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtXContent" runat="server" ToolTip="The full XHTML content of the email." Font-Names="Calibri" Font-Size="11pt" Width="600px" Height="333px" TextMode="MultiLine" TabIndex="31"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="lblTextContent" Text="Text Content:" ToolTip="The text version of the email content. Will be used for email clients that do not have HTML email capability or have it disabled." runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Calibri" Font-Overline="False" Font-Size="11pt" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTextContent" runat="server" ToolTip="The text version of the email content. Will be used for email clients that do not have HTML email capability or have it disabled." Font-Names="Calibri" Font-Size="11pt" Width="600px" Height="333px" TextMode="MultiLine" TabIndex="32"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        
        <tr>
            <td colspan="2">
                <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Font-Names="Calibri" Font-Size="11pt" TabIndex="33" />
                <%--<asp:CustomValidator ID="customValidator" runat="server" Display="None" OnServerValidate="customValidator_ServerValidate">
                </asp:CustomValidator>--%>
            </td>
        </tr>
    </table>
    
    <script type="text/javascript" language="javascript">
    	function permRemVisibility(state) {
    		var objPermReminder = document.getElementById('<%=trPRT.ClientID%>');
    		objPermReminder.style.visibility = state;
    	}

    	function viewRemVisibility(state) {
    		var objVWT = document.getElementById('<%=trVWT.ClientID%>');
    		objVWT.style.visibility = state;
    		var objVWLT = document.getElementById('<%=trVWLT.ClientID%>');
    		objVWLT.style.visibility = state;
    	}

    	function makeVis() {
    		var objPermReminderY = document.getElementById('<%=rbPermReminderY.ClientID%>');
    		var objViewWebpageY = document.getElementById('<%=rbViewWebpageY.ClientID%>');

    		var objPermReminder = document.getElementById('<%=trPRT.ClientID%>');
    		if (objPermReminderY.checked) {
    			objPermReminder.style.visibility = 'visible';
    		}
    		else {
    			objPermReminder.style.visibility = 'hidden';
    		}

    		var objVWT = document.getElementById('<%=trVWT.ClientID%>');
    		var objVWLT = document.getElementById('<%=trVWLT.ClientID%>');

    		if (objViewWebpageY.checked) {
    			objVWLT.style.visibility = 'visible';
    			objVWT.style.visibility = 'visible';
    		}
    		else {
    			objVWLT.style.visibility = 'hidden';
    			objVWT.style.visibility = 'hidden';
    		}
    	}

    	makeVis();
    </script>

</asp:Content>