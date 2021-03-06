/*
Added to support the existing Data Science ETL process used to produce MI reports
*/
CREATE TABLE [dbo].[SalesProcessInstance](
	[businessunitid] [uniqueidentifier] NULL,
	[businessunitiddsc] [int] NULL,
	[businessunitidname] [nvarchar](160) NULL,
	[opportunityid] [uniqueidentifier] NULL,
	[opportunityiddsc] [int] NULL,
	[opportunityidname] [nvarchar](300) NULL,
	[salesprocessinstanceid] [uniqueidentifier] NULL,
	[salesprocessname] [nvarchar](256) NULL,
	[salesstagename] [nvarchar](256) NULL
) ON [PRIMARY]