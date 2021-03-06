/*
Added to support the existing Data Science ETL process used to produce MI reports
*/
CREATE TABLE [dbo].[ContactOrders](
	[contactid] [uniqueidentifier] NULL,
	[contactorderid] [uniqueidentifier] NULL,
	[salesorderid] [uniqueidentifier] NULL
) ON [PRIMARY]