/*
Added to support the existing Data Science ETL process used to produce MI reports
*/
CREATE TABLE [dbo].[ProductAssociation](
	[associatedproduct] [uniqueidentifier] NULL,
	[productassociationid] [uniqueidentifier] NULL,
	[productid] [uniqueidentifier] NULL
) ON [PRIMARY]