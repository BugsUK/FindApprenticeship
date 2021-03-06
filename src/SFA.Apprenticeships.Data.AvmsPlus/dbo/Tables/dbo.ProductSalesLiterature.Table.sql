/*
Added to support the existing Data Science ETL process used to produce MI reports
*/
CREATE TABLE [dbo].[ProductSalesLiterature](
	[productid] [uniqueidentifier] NULL,
	[productsalesliteratureid] [uniqueidentifier] NULL,
	[salesliteratureid] [uniqueidentifier] NULL
) ON [PRIMARY]