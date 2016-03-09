CREATE PROCEDURE [dbo].[uspGetThirdPartyUrnById]
	@ThirdpartyID int
AS
	SELECT [EDSURN] 'URN'
	FROM ThirdParty
	WHERE [ID] = @ThirdpartyID
RETURN 0