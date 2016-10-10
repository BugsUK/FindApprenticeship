/*
Added to support the existing Data Science ETL process used to produce MI reports
*/
CREATE TABLE [dbo].[Organization](
	[createdby] [uniqueidentifier] NULL,
	[createdbydsc] [int] NULL,
	[createdbyname] [nvarchar](160) NULL,
	[createdbyyominame] [nvarchar](160) NULL,
	[createdon] [datetime] NULL,
	[createdonutc] [datetime] NULL,
	[fiscalperiodtype] [int] NULL,
	[modifiedby] [uniqueidentifier] NULL,
	[modifiedbydsc] [int] NULL,
	[modifiedbyname] [nvarchar](160) NULL,
	[modifiedbyyominame] [nvarchar](160) NULL,
	[modifiedon] [datetime] NULL,
	[modifiedonutc] [datetime] NULL,
	[timezoneruleversionnumber] [int] NULL,
	[utcconversiontimezonecode] [int] NULL,
	[acknowledgementtemplateid] [uniqueidentifier] NULL,
	[acknowledgementtemplateiddsc] [int] NULL,
	[acknowledgementtemplateidname] [nvarchar](200) NULL,
	[allowaddressbooksyncs] [bit] NULL,
	[allowautoresponsecreation] [bit] NULL,
	[allowautounsubscribe] [bit] NULL,
	[allowautounsubscribeacknowledgement] [bit] NULL,
	[allowmarketingemailexecution] [bit] NULL,
	[allowofflinescheduledsyncs] [bit] NULL,
	[allowoutlookscheduledsyncs] [bit] NULL,
	[allowunresolvedpartiesonemailsend] [bit] NULL,
	[allowwebexcelexport] [bit] NULL,
	[amdesignator] [nvarchar](25) NULL,
	[basecurrencyid] [uniqueidentifier] NULL,
	[basecurrencyiddsc] [int] NULL,
	[basecurrencyidname] [nvarchar](100) NULL,
	[blockedattachments] [ntext] NULL,
	[bulkoperationprefix] [nvarchar](20) NULL,
	[businessclosurecalendarid] [uniqueidentifier] NULL,
	[calendartype] [int] NULL,
	[campaignprefix] [nvarchar](20) NULL,
	[caseprefix] [nvarchar](20) NULL,
	[contractprefix] [nvarchar](20) NULL,
	[currencydecimalprecision] [int] NULL,
	[currencydisplayoption] [int] NULL,
	[currencyformatcode] [int] NULL,
	[currencyformatcodename] [nvarchar](255) NULL,
	[currencysymbol] [nvarchar](5) NULL,
	[currentbulkoperationnumber] [int] NULL,
	[currentcampaignnumber] [int] NULL,
	[currentcasenumber] [int] NULL,
	[currentcontractnumber] [int] NULL,
	[currentimportsequencenumber] [int] NULL,
	[currentinvoicenumber] [int] NULL,
	[currentkbnumber] [int] NULL,
	[currentordernumber] [int] NULL,
	[currentparsedtablenumber] [int] NULL,
	[currentquotenumber] [int] NULL,
	[dateformatcode] [int] NULL,
	[dateformatcodename] [nvarchar](255) NULL,
	[dateformatstring] [nvarchar](255) NULL,
	[dateseparator] [nvarchar](5) NULL,
	[decimalsymbol] [nvarchar](5) NULL,
	[disabledreason] [nvarchar](500) NULL,
	[emailsendpollingperiod] [int] NULL,
	[enablepricingoncreate] [bit] NULL,
	[featureset] [ntext] NULL,
	[fiscalcalendarstart] [datetime] NULL,
	[fiscalcalendarstartutc] [datetime] NULL,
	[fiscalperiodformat] [nvarchar](25) NULL,
	[fiscalsettingsupdated] [bit] NULL,
	[fiscalsettingsupdatedname] [nvarchar](255) NULL,
	[fiscalyeardisplaycode] [tinyint] NULL,
	[fiscalyearformat] [nvarchar](25) NULL,
	[fiscalyearperiodconnect] [nvarchar](5) NULL,
	[fullnameconventioncode] [int] NULL,
	[fullnameconventioncodename] [nvarchar](255) NULL,
	[grantaccesstonetworkservice] [bit] NULL,
	[ignoreinternalemail] [bit] NULL,
	[integrationuserid] [uniqueidentifier] NULL,
	[invoiceprefix] [nvarchar](20) NULL,
	[isappmode] [bit] NULL,
	[isdisabled] [bit] NULL,
	[isdisabledname] [nvarchar](255) NULL,
	[isduplicatedetectionenabled] [bit] NULL,
	[isduplicatedetectionenabledforimport] [bit] NULL,
	[isduplicatedetectionenabledforofflinesync] [bit] NULL,
	[isduplicatedetectionenabledforonlinecreateupdate] [bit] NULL,
	[isfiscalperiodmonthbased] [bit] NULL,
	[ispresenceenabled] [bit] NULL,
	[ispresenceenabledname] [nvarchar](255) NULL,
	[isregistered] [bit] NULL,
	[isregisteredname] [nvarchar](255) NULL,
	[issopintegrationenabled] [bit] NULL,
	[isvintegrationcode] [int] NULL,
	[kbprefix] [nvarchar](20) NULL,
	[languagecode] [int] NULL,
	[languagecodename] [nvarchar](255) NULL,
	[localeid] [int] NULL,
	[longdateformatcode] [int] NULL,
	[maxappointmentdurationdays] [int] NULL,
	[maximumtrackingnumber] [int] NULL,
	[maxrecordsforexporttoexcel] [int] NULL,
	[maxuploadfilesize] [int] NULL,
	[minaddressbooksyncinterval] [int] NULL,
	[minofflinesyncinterval] [int] NULL,
	[minoutlooksyncinterval] [int] NULL,
	[name] [nvarchar](160) NULL,
	[negativecurrencyformatcode] [int] NULL,
	[negativeformatcode] [int] NULL,
	[negativeformatcodename] [nvarchar](255) NULL,
	[nexttrackingnumber] [int] NULL,
	[numberformat] [nvarchar](2) NULL,
	[numbergroupformat] [nvarchar](50) NULL,
	[numberseparator] [nvarchar](5) NULL,
	[orderprefix] [nvarchar](20) NULL,
	[organizationid] [uniqueidentifier] NULL,
	[parsedtablecolumnprefix] [nvarchar](20) NULL,
	[parsedtableprefix] [nvarchar](20) NULL,
	[picture] [ntext] NULL,
	[pmdesignator] [nvarchar](25) NULL,
	[pricingdecimalprecision] [int] NULL,
	[privilegeusergroupid] [uniqueidentifier] NULL,
	[privreportinggroupid] [uniqueidentifier] NULL,
	[privreportinggroupname] [nvarchar](256) NULL,
	[quoteprefix] [nvarchar](20) NULL,
	[referencesitemapxml] [ntext] NULL,
	[rendersecureiframeforemail] [bit] NULL,
	[reportinggroupid] [uniqueidentifier] NULL,
	[reportinggroupname] [nvarchar](256) NULL,
	[schemanameprefix] [nvarchar](8) NULL,
	[sharetopreviousowneronassign] [bit] NULL,
	[showweeknumber] [bit] NULL,
	[showweeknumbername] [nvarchar](255) NULL,
	[sitemapxml] [ntext] NULL,
	[sortid] [int] NULL,
	[sqlaccessgroupid] [uniqueidentifier] NULL,
	[sqlaccessgroupname] [nvarchar](256) NULL,
	[sqmenabled] [bit] NULL,
	[supportuserid] [uniqueidentifier] NULL,
	[systemuserid] [uniqueidentifier] NULL,
	[tagmaxaggressivecycles] [tinyint] NULL,
	[tagpollingperiod] [int] NULL,
	[timeformatcode] [int] NULL,
	[timeformatcodename] [nvarchar](255) NULL,
	[timeformatstring] [nvarchar](255) NULL,
	[timeseparator] [nvarchar](5) NULL,
	[tokenexpiry] [smallint] NULL,
	[trackingprefix] [nvarchar](256) NULL,
	[trackingtokenidbase] [int] NULL,
	[trackingtokeniddigits] [tinyint] NULL,
	[uniquespecifierlength] [int] NULL,
	[usergroupid] [uniqueidentifier] NULL,
	[v3calloutconfighash] [nvarchar](256) NULL,
	[weekstartdaycode] [int] NULL,
	[weekstartdaycodename] [nvarchar](255) NULL,
	[yearstartweekcode] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]