/*
Added to support the existing Data Science ETL process used to produce MI reports
*/
CREATE TABLE [dbo].[ContactLeads](
	[contactid] [uniqueidentifier] NULL,
	[contactleadid] [uniqueidentifier] NULL,
	[leadid] [uniqueidentifier] NULL
) ON [PRIMARY]
