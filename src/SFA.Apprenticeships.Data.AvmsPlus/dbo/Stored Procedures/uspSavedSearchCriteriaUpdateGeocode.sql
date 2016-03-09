CREATE PROCEDURE [dbo].[uspSavedSearchCriteriaUpdateGeocode]
	@savedSearchCriteriaId int = null,
	@geocodeEasting int = NULL,      
	@geocodeNorthing int = NULL
AS
BEGIN      
      
	SET NOCOUNT ON  
	UPDATE [dbo].[SavedSearchCriteria]         
	SET         
	[GeocodeEasting] = @geocodeEasting,       
	[GeocodeNorthing] = @geocodeNorthing  
	WHERE         
	[SavedSearchCriteriaId]=@savedSearchCriteriaId      

	IF @@ROWCOUNT = 0        
	BEGIN        
		RAISERROR('Concurrent update error. Updated aborted.', 16, 2)        
	END          
              
	SET NOCOUNT OFF      
END