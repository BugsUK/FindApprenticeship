/*
Added to support the existing Data Science ETL process used to produce MI reports
*/
CREATE TABLE [dbo].[ContactQuotes](
	[contactid] [uniqueidentifier] NULL,
	[contactquoteid] [uniqueidentifier] NULL,
	[quoteid] [uniqueidentifier] NULL
) ON [PRIMARY]