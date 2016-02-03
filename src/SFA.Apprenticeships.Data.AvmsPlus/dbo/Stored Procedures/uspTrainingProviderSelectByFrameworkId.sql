CREATE PROCEDURE [dbo].[uspTrainingProviderSelectByFrameworkId]
	@frameworkId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
		FROM [ProviderSiteFramework] 
		WHERE FrameworkId = @frameworkId
END