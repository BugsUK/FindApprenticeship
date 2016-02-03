CREATE PROCEDURE [dbo].[uspVacancyNASDetailsUpdate]      
 @VacancyId int,      
 @BeingSupportedBy nvarchar(50),      
 @LockedForSupportUntil datetime       
       
AS      
BEGIN      
 SET NOCOUNT ON      
 BEGIN TRY      
--      
       
 UPDATE [dbo].[vacancy] 
 SET      
  BeingSupportedBy = @BeingSupportedBy,      
  LockedForSupportUntil =@LockedForSupportUntil      
 WHERE      
  VacancyId = @VacancyId      
       
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