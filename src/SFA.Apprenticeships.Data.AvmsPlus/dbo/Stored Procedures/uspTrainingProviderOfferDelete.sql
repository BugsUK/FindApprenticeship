CREATE PROCEDURE [dbo].[uspTrainingProviderOfferDelete]
	 @offerId INT
AS
BEGIN
	SET NOCOUNT ON
	
    DELETE FROM [dbo].[ProviderSiteOffer]
	WHERE ProviderSiteOfferID = @offerId
    
    SET NOCOUNT OFF
END