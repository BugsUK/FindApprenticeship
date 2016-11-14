/*
Added to support the existing Data Science ETL process used to produce MI reports
*/
CREATE TABLE [dbo].[leadproduct](
	[productid] [uniqueidentifier] NULL,
	[leadid] [uniqueidentifier] NULL,
	[leadproductid] [uniqueidentifier] NULL
) ON [PRIMARY]