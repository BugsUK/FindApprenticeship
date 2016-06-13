CREATE PROCEDURE [dbo].[uspStakeHolderIsUserCredentialRequestValid]
	@emailAddress NVARCHAR (100), 
	@Postcode NVARCHAR (8), 
	@StakeHolderId INT OUTPUT  
AS  
BEGIN        
	SET NOCOUNT ON    

	BEGIN TRY        
		Set		@StakeHolderId = 0  -- Default Zero if there's an error   
		Select	@StakeHolderId = S.StakeHolderID    
		From	StakeHolder S, Person P   
		Where	S.PersonId  = P.PersonId    
		And		Postcode = @Postcode 
		And		P.Email = @emailAddress    
    
		return @StakeHolderId  -- Returning StakeHolderId    
	END TRY    
        
	BEGIN CATCH    
		EXEC RethrowError;    
	END CATCH    
	SET NOCOUNT OFF    
END