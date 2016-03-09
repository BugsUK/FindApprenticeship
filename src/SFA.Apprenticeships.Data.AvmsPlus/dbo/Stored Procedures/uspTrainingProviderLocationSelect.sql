CREATE PROCEDURE [dbo].[uspTrainingProviderLocationSelect]
	@TrainingProviderLocationId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [dbo].[ProviderSiteLocalAuthority].ProviderSiteLocalAuthorityID, 
		[dbo].[ProviderSiteLocalAuthority].[ProviderSiteRelationshipID], 
				[dbo].[ProviderSiteLocalAuthority].[LocalAuthorityId] 
		FROM [ProviderSiteLocalAuthority] 
		WHERE ProviderSiteLocalAuthorityID = @TrainingProviderLocationId
END