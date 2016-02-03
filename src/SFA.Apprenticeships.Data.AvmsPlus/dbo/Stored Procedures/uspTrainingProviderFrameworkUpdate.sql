CREATE PROCEDURE [dbo].[uspTrainingProviderFrameworkUpdate]
	@TrainingProviderFrameworkId INT,
	@TrainingProviderId INT,
	@FrameworkId INT--,
	--@TrainingProviderSectorId INT
	--@PassRate SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE dbo.[ProviderSiteFramework]
		SET dbo.[ProviderSiteFramework].ProviderSiteRelationshipID = @TrainingProviderId,
			dbo.[ProviderSiteFramework].FrameworkId = @FrameworkId--,
			--dbo.[ProviderSiteFramework].TrainingProviderSectorId = @TrainingProviderSectorId
		WHERE dbo.[ProviderSiteFramework].ProviderSiteFrameworkID = @TrainingProviderFrameworkId
END