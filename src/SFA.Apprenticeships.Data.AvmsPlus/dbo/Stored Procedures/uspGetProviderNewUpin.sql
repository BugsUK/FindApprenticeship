CREATE PROCEDURE [dbo].[uspGetProviderNewUpin]
	@Upin int

AS
	
	SELECT UPIN
	FROM Provider
	WHERE (OriginalUPIN = @Upin
	OR UPIN = @Upin)
	AND ProviderStatusTypeID !=2