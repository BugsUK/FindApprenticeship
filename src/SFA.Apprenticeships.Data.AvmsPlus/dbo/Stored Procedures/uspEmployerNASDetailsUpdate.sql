CREATE PROCEDURE [dbo].[uspEmployerNASDetailsUpdate]      
 @EmployerID int,      
 @BeingSupportedBy nvarchar(50),      
 @LockedForSupportUntil datetime       
       
AS      
BEGIN      
 SET NOCOUNT ON      
 BEGIN TRY      
--      
       
 UPDATE [dbo].[Employer] 
 SET      
  BeingSupportedBy = @BeingSupportedBy,      
  LockedForSupportUntil =@LockedForSupportUntil      
 WHERE      
  EmployerId = @EmployerID      
       
--      
-- IF @@ROWCOUNT = 0      
-- BEGIN      
--  RAISERROR('Concurrent update error. Updated aborted.', 16, 2)      
-- END      
    END TRY      
      
    BEGIN CATCH      
  EXEC RethrowError;      
 END CATCH       
      
    SET NOCOUNT OFF      
END