CREATE PROCEDURE [dbo].[ReportGetReportDefinitionsByDisplayName]
	@DisplayName nvarchar(100) 

AS
	SELECT DISTINCT
		[DisplayName]  
		,[HTMLVersion]  
		,[CSVVersion]  
		,[SummaryVersion]  
		,[Description]  
		,[GeographicSectionName]  
		, [DateSectionName]  
		, [ApplicationSectionName]  
		,[Flags]  
   FROM [dbo].[ReportDefinitions]  
   WHERE [DisplayName] = @DisplayName