CREATE PROCEDURE [dbo].[uspTrainingProviderStatusChange]
	 @trainingProviderId int,
	 @statusId int
AS
Begin
	UPDATE [ProviderSite]
	SET TrainingProviderStatusTypeId = @statusId
	WHERE ProviderSiteID =  @trainingProviderId
End