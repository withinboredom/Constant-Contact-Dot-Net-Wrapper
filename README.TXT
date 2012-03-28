# CONSTANT CONTACT - UPLOAD Email Campaign FORM - ASP Application #


1. ABOUT THIS PACKAGE

This is an ASP sample application that allows you to:
	- add a new Email Campaign
	- edit an existing Email Campaign
	- list all existing Email Campaigns
	- remove an existing Email Campaigns from all lists


2. WHAT THIS PACKAGE CONTAINS 

Below is a list of files released as part of this build.

	[BaseFolder]\Development - Contains the source code of the ConstantContactBO.DLL and ConstantContactUtility.DLL
	[BaseFolder]\Development\ConstantContactBO - Provides access to the following .NET wrapper objects: Contact Lists, Contacts, Email and Email Campaign.
	[BaseFolder]\Development\ConstantContactUtility - Provides access to utility methods to create a new Contact, Email Campaign, update an existing one, search Contact by email, etc.
	[BaseFolder]\Development\SolutionInfo.cs - Stores information about the ConstantContactBO.DLL and ConstantContactUtility.DLL assemblies (Assembly Company, Assemby Product, Assembly Copyright and Assemby Trademark)
	[BaseFolder]\Development\SolutionVersionInfo.cs - Stores information about the ConstantContactBO.DLL and ConstantContactUtility.DLL assemblies (Assembly Version [v1.0.0.0] and Assembly File Version [v1.0.0.0])
	[BaseFolder]\output\Debug\ConstantContactBO.dll - Output for the ConstantContactBO class library project
	[BaseFolder]\output\Debug\ConstantContactUtility.dll - Output for the ConstantContactUtility class library project
	[BaseFolder]\Release\Setup - Contains the setup to install the sample
	[BaseFolder]\Web - Contains the source code for the ASP web application
	[BaseFolder]\Web\UploadContactForm\App_Code\ConstantContact.cs - This class contains API Key, Username and Password used to acces Constant resources. Also, it contains definition for State/Province and Country
	[BaseFolder]\Web\UploadContactForm\App_Themes - This Folder contains images used in UI desigh
	[BaseFolder]\Web\UploadContactForm\EmailCampaignMasterPages\MasterPage.master - Master Page of the Email Campaign Form
	[BaseFolder]\Web\UploadContactForm\EmailCampaignMasterPages\MasterPage.master.cs - Code behind of the Master Page 
	[BaseFolder]\Web\UploadContactForm\EmailCampaignPages\AddEmailCampaign.aspx - Page for creating/editing a campaign
	[BaseFolder]\Web\UploadContactForm\EmailCampaignPages\AddEmailCampaign.aspx.cs - Code behind of the AddEmailCampaign.aspx
	[BaseFolder]\Web\UploadContactForm\EmailCampaignPages\ListEmailCampaigns.aspx - Page used to list or search campaigns
	[BaseFolder]\Web\UploadContactForm\EmailCampaignPages\ListEmailCampaigns.aspx.cs - Code behind of the ListEmailCampaigns.aspx
	[BaseFolder]\Web\UploadContactForm\EmailCampaignPages\Default.aspx - main page of the sample
	[BaseFolder]\Web\UploadContactForm\EmailCampaignPages\Default.aspx.cs - code behind of the main page
	[BaseFolder]\Web\UploadContactForm\InputData\countries.txt - list of countries and their codes
	[BaseFolder]\Web\UploadContactForm\InputData\states.txt - list of US states and their codes
	[BaseFolder]\Web\UploadContactForm\Web.Config - Configuration file that exposes the APIKey, Username and Password used to access Constant Contact REST API			
	[BaseFolder]\ASPSample.sln - Visual Studio 2008 solution project
			

3. VERSION AND CHANGES

Version number: 1.0
Release date: 01.05.2010 17:05
Changes: none
Bug Fix: none