CREATE PROCEDURE [dbo].[uspStakeHolderUpdateActivationValues]
	@StakeHolderId INT
AS
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
	SET NOCOUNT ON;  

	BEGIN TRY      
		UPDATE StakeHolder 
		SET StakeHolderStatusId = (Select StakeHolderStatusId from StakeHolderStatus
									Where CodeName = 'AVT')
		Where StakeHolderID = @StakeHolderId

		IF @@ROWCOUNT = 0      
			BEGIN      
			RAISERROR('Concurrent update error. Updated aborted.', 16, 2)      
		END      
	END TRY      

	BEGIN CATCH      
		EXEC RethrowError;      
	END CATCH       
END