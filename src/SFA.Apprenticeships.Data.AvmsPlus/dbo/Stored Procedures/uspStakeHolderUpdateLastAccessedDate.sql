CREATE PROCEDURE [dbo].[uspStakeHolderUpdateLastAccessedDate]
	@StakeHolderId INT
AS
BEGIN      
	SET NOCOUNT ON      
       
    UPDATE StakeHolder
        SET LastAccessedDate = getdate()
    WHERE
        StakeHolderId = @StakeHolderId
          
	SET NOCOUNT OFF      
END