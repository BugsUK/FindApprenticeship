/*
Added to support the existing Data Science ETL process used to produce MI reports
*/
CREATE TABLE [dbo].[Uom](
	[baseuom] [uniqueidentifier] NULL,
	[baseuomname] [nvarchar](100) NULL,
	[baseuomnamedsc] [int] NULL,
	[createdby] [uniqueidentifier] NULL,
	[createdbydsc] [int] NULL,
	[createdbyname] [nvarchar](160) NULL,
	[createdbyyominame] [nvarchar](160) NULL,
	[createdon] [datetime] NULL,
	[createdonutc] [datetime] NULL,
	[importsequencenumber] [int] NULL,
	[isschedulebaseuom] [bit] NULL,
	[isschedulebaseuomname] [nvarchar](255) NULL,
	[modifiedby] [uniqueidentifier] NULL,
	[modifiedbydsc] [int] NULL,
	[modifiedbyname] [nvarchar](160) NULL,
	[modifiedbyyominame] [nvarchar](160) NULL,
	[modifiedon] [datetime] NULL,
	[modifiedonutc] [datetime] NULL,
	[name] [nvarchar](100) NULL,
	[organizationid] [uniqueidentifier] NULL,
	[overriddencreatedon] [datetime] NULL,
	[overriddencreatedonutc] [datetime] NULL,
	[quantity] [numeric](23, 10) NULL,
	[uomid] [uniqueidentifier] NULL,
	[uomscheduleid] [uniqueidentifier] NULL
) ON [PRIMARY]