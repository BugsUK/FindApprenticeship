/*
Added to support the existing Data Science ETL process used to produce MI reports
*/
CREATE TABLE [dbo].[ResourceGroup](
	[businessunitid] [uniqueidentifier] NULL,
	[businessunitiddsc] [int] NULL,
	[businessunitidname] [nvarchar](160) NULL,
	[grouptypecode] [int] NULL,
	[grouptypecodename] [nvarchar](255) NULL,
	[name] [nvarchar](160) NULL,
	[objecttypecode] [int] NULL,
	[objecttypecodename] [nvarchar](255) NULL,
	[organizationid] [uniqueidentifier] NULL,
	[organizationiddsc] [int] NULL,
	[organizationidname] [nvarchar](160) NULL,
	[resourcegroupid] [uniqueidentifier] NULL
) ON [PRIMARY]