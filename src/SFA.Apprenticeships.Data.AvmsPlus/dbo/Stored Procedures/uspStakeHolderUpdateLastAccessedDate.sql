CREATE PROCEDURE [dbo].[uspStakeHolderUpdateLastAccessedDate]
	@StakeHolderId INT
AS
BEGIN      
	SET NOCOUNT ON      
       
    UPDATE StakeHolder
        SET LastAccessedDate = getdate()
    WHERE
        StakeHolderID = @StakeHolderId
          
	SET NOCOUNT OFF      
END