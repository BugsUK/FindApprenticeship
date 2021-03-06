/*
Added to support the existing Data Science ETL process used to produce MI reports
*/
CREATE TABLE [dbo].[LSC_companyflag](
	[CreatedByDsc] [int] NULL,
	[CreatedByName] [nvarchar](160) NULL,
	[ModifiedByDsc] [int] NULL,
	[ModifiedByName] [nvarchar](160) NULL,
	[OrganizationIdDsc] [int] NULL,
	[OrganizationIdName] [nvarchar](160) NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[DeletionStateCode] [int] NULL,
	[ImportSequenceNumber] [int] NULL,
	[LSC_companyflagId] [uniqueidentifier] NOT NULL,
	[LSC_name] [nvarchar](100) NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[ModifiedOn] [datetime] NULL,
	[OrganizationId] [uniqueidentifier] NULL,
	[OverriddenCreatedOn] [datetime] NULL,
	[statecode] [int] NOT NULL,
	[statuscode] [int] NULL,
	[TimeZoneRuleVersionNumber] [int] NULL,
	[UTCConversionTimeZoneCode] [int] NULL,
	[VersionNumber] [timestamp] NULL
) ON [PRIMARY]
