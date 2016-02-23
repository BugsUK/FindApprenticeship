CREATE PROCEDURE [dbo].[uspProviderStatusChangeByUKPRN]
	 @UKPRN int,
	 @statusId int
AS
Begin
	UPDATE [Provider]
	SET ProviderStatusTypeID = @statusId
	WHERE UKPRN =  @UKPRN AND ProviderStatusTypeID <> 2
End