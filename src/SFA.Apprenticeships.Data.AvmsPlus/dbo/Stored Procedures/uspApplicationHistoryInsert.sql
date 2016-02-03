CREATE PROCEDURE [dbo].[uspApplicationHistoryInsert]  
    @applicationHistoryEventDate datetime,  
 @applicationHistoryEventTypeId int,  
 @applicationHistoryId int ,  
 @applicationId int,  
 @comment nvarchar(200) = NULL,  
 @userName nvarchar(50) = NULL  
AS  
BEGIN  
 SET NOCOUNT ON  
   
 BEGIN TRY  
    INSERT INTO [dbo].[ApplicationHistory] ([ApplicationHistoryEventDate], [ApplicationHistoryEventTypeId], [ApplicationId], [Comment], [UserName])  
 VALUES (@applicationHistoryEventDate, @applicationHistoryEventTypeId, @applicationId, @comment, @userName)  
    SET @applicationHistoryId = SCOPE_IDENTITY()  
    END TRY  
  
    BEGIN CATCH  
  EXEC RethrowError;  
 END CATCH  
      
    SET NOCOUNT OFF  
END