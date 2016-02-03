CREATE PROCEDURE [dbo].[uspTrainingProviderLocationDelete]
	@TrainingProviderLocationId Int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE dbo.[ProviderSiteLocalAuthority]
		WHERE ProviderSiteLocalAuthorityID = @TrainingProviderLocationId
END