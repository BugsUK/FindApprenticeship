CREATE PROCEDURE [dbo].[uspApplicationHistorySelectByApplicationId]  
	@applicationId int  
AS        
BEGIN        
      
SET NOCOUNT ON        
BEGIN TRY        
	Select ApplicationId, ApplicationHistoryId, UserName,   
		ApplicationHistoryEventDate, 
		ApplicationHistoryEvent.FullName As ApplicationHistoryEventType, 
		ApplicationStatusType.FullName As ApplicationHistoryEventSubType,
		ApplicationHistory.ApplicationHistoryEventTypeId, 
		Comment
	From ApplicationHistory, ApplicationHistoryEvent, ApplicationStatusType
	Where ApplicationHistory.ApplicationId = @applicationId  
		And ApplicationHistory.ApplicationHistoryEventTypeId = ApplicationHistoryEvent.ApplicationHistoryEventId
		And ApplicationHistory.ApplicationHistoryEventSubTypeId = ApplicationStatusType.ApplicationStatusTypeId
		Order By ApplicationHistoryEventDate Desc  
    END TRY        
	BEGIN CATCH        
		RAISERROR('Associated application details does not exists.  Query aborted.', 16, 2)  
	EXEC RethrowError;        
	END CATCH         
	SET NOCOUNT OFF        
END