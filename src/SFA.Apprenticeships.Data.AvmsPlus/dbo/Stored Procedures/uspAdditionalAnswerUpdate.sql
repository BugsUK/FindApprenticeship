CREATE PROCEDURE [dbo].[uspAdditionalAnswerUpdate]  
 @AdditionalAnswerId int,  
 @Answer nvarchar(4000)  
AS  
BEGIN  
  
 SET NOCOUNT ON  
 BEGIN TRY  
 UPDATE [dbo].[AdditionalAnswer]   
 SET   [Answer] = @Answer  
 WHERE [AdditionalAnswerId]=@AdditionalAnswerId  
  
 IF @@ROWCOUNT = 0  
 BEGIN  
  RAISERROR('Concurrent update error. Updated aborted.', 16, 2)  
 END  
    END TRY  
  
    BEGIN CATCH  
  EXEC RethrowError;  
 END CATCH   
 SET NOCOUNT OFF  
END