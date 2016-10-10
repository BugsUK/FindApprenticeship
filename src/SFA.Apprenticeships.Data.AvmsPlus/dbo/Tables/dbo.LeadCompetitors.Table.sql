/*
Added to support the existing Data Science ETL process used to produce MI reports
*/
CREATE TABLE [dbo].[LeadCompetitors](
	[competitorid] [uniqueidentifier] NULL,
	[leadcompetitorid] [uniqueidentifier] NULL,
	[leadid] [uniqueidentifier] NULL
) ON [PRIMARY]