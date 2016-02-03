CREATE PROCEDURE [dbo].[uspChangeProviderUKPRN]
	@oldUKPRN int,
	@oldUPIN int,
	@newUKPRN int	
AS
	UPDATE dbo.PROVIDER SET UKPRN = @newUKPRN WHERE UPIN = @oldUPIN AND UKPRN = @oldUKPRN