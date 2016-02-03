CREATE PROCEDURE [dbo].[uspTrainingProviderHistoryInsert]    
    @comment nvarchar(4000) = NULL,    
 @event int,    
 @historyDate datetime,    
 --@historyId int,    
 @trainingProviderHistoryId int ,    
 @trainingProviderId int,    
 @userName nvarchar(50)    
AS    
BEGIN    
 SET NOCOUNT ON    
     
 BEGIN TRY    
    INSERT INTO [dbo].[ProviderSiteHistory]     
 (    
 [Comment],     
 [EventTypeId],     
 [HistoryDate],    
 -- [HistoryId],     
 [TrainingProviderId],     
 [UserName])    
 VALUES (    
 @comment,     
 @event,     
 @historyDate,     
    --@historyId,     
 @trainingProviderId,     
 @userName)    
    SET @trainingProviderHistoryId = SCOPE_IDENTITY()    
    END TRY    
    
    BEGIN CATCH    
  EXEC RethrowError;    
 END CATCH    
        
    SET NOCOUNT OFF    
END