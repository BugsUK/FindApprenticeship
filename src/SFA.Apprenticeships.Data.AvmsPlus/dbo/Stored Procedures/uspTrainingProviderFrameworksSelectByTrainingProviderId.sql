CREATE PROCEDURE [dbo].[uspTrainingProviderFrameworksSelectByTrainingProviderId]
	@TrainingProviderId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT dbo.ApprenticeshipFramework.ApprenticeshipFrameworkId, 
			dbo.ApprenticeshipFramework.ApprenticeshipOccupationId, 
			dbo.ApprenticeshipFramework.CodeName, 
			dbo.ApprenticeshipFramework.ShortName, 
			dbo.ApprenticeshipFramework.FullName
		FROM dbo.[ProviderSiteFramework] 
			INNER JOIN dbo.ApprenticeshipFramework 
				ON dbo.[ProviderSiteFramework].FrameworkId = dbo.ApprenticeshipFramework.ApprenticeshipFrameworkId
		WHERE (dbo.[ProviderSiteFramework].ProviderSiteRelationshipID = @TrainingProviderId)
END