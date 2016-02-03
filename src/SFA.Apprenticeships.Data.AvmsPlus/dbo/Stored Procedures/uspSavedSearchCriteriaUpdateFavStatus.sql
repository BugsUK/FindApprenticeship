CREATE PROCEDURE [dbo].[uspSavedSearchCriteriaUpdateFavStatus]  
 @savedSearchCriteriaId int,  
 @backgroundSearch bit = NULL  
   
AS  
BEGIN  
  
 --The [dbo].[SavedSearchCriteria] table doesn't have a timestamp column. Optimistic concurrency logic cannot be generated  
 SET NOCOUNT ON  
  
 BEGIN TRY  
 UPDATE [dbo].[SavedSearchCriteria]   
 SET   
  [BackgroundSearch] = ISNULL(@backgroundSearch,[BackgroundSearch])    
 WHERE [SavedSearchCriteriaId] = @savedSearchCriteriaId  
  
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