CREATE PROCEDURE [dbo].[uspGetProviderNewUpin]
	@Upin int

AS
	
	SELECT UPIN
	FROM provider
	WHERE (OriginalUPIN = @Upin
	OR UPIN = @Upin)
	AND ProviderStatusTypeID !=2