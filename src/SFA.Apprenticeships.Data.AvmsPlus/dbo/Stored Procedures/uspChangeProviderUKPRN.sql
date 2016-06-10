CREATE PROCEDURE [dbo].[uspChangeProviderUKPRN]
	@oldUKPRN int,
	@oldUPIN int,
	@newUKPRN int	
AS
	UPDATE dbo.Provider SET UKPRN = @newUKPRN WHERE UPIN = @oldUPIN AND UKPRN = @oldUKPRN