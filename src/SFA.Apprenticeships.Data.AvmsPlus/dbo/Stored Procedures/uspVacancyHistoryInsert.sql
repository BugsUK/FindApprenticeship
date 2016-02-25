CREATE PROCEDURE [dbo].[uspVacancyHistoryInsert]  
    @comment nvarchar(4000) = NULL,  
 @historyDate datetime,  
 @userName nvarchar(50),  
 @vacancyHistoryEventId int,  
 @vacancyHistoryId int ,  
 @vacancyId int  
AS  
BEGIN  
 SET NOCOUNT ON  
   
 BEGIN TRY  
    INSERT INTO [dbo].[VacancyHistory] (
		[Comment], 
		[HistoryDate], 
		[UserName], 
		[VacancyHistoryEventTypeId], 
		[VacancyHistoryEventSubTypeId], 
		[VacancyId])  
 VALUES (
	@comment, 
	@historyDate, 
	@userName, 
	1, --TODO: Pass in [VacancyHistoryEventTypeId]
	@vacancyHistoryEventId, 
	@vacancyId)  
    SET @vacancyHistoryId = SCOPE_IDENTITY()  
    END TRY  
  
    BEGIN CATCH  
  EXEC RethrowError;  
 END CATCH  
      
    SET NOCOUNT OFF  
END