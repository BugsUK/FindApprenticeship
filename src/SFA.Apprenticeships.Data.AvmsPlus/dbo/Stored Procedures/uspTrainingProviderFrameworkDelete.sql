CREATE PROCEDURE [dbo].[uspTrainingProviderFrameworkDelete]
	@TrainingProviderFrameworkId Int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE dbo.[ProviderSiteFramework]
		WHERE ProviderSiteFrameworkID = @TrainingProviderFrameworkId
END