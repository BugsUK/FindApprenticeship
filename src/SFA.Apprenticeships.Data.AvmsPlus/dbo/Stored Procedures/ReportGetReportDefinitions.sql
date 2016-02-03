CREATE PROCEDURE [dbo].[ReportGetReportDefinitions]
	@RoleTypeID int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [RoleTypeID]
		  ,[DisplayName]
		  ,[HTMLVersion]
		  ,[CSVVersion]
		  ,[SummaryVersion]
		  ,[Description]
		  ,[GeographicSectionName]
		  ,[DateSectionName]
		  ,[ApplicationSectionName]
	      ,[Flags]
	  FROM [dbo].[ReportDefinitions]
	  WHERE [RoleTypeID] = @RoleTypeID
END