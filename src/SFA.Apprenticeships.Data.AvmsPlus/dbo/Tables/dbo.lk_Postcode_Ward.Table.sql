/*
Added to support the existing Data Science ETL process used to produce MI reports
*/
CREATE TABLE [dbo].[lk_Postcode_Ward](
	[Postcode] [varchar](8) NULL,
	[Ward_03_Code] [varchar](6) NULL
) ON [PRIMARY]