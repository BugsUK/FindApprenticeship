create  PROCEDURE [dbo].[uspTrainingProviderHistorySelectByHistoryId]         
 @historyId INT
 
AS        
BEGIN        
        
SET NOCOUNT ON        
         
	select 
	TPH.TrainingProviderHistoryId as 'HistoryId',
	TPH.TrainingProviderId,
	TPH.EventTypeId as 'EventTypeId',
	ISNULL(TPH.Comment,'') as 'Comment',
	TPH.HistoryDate as 'EventDate',
	ISNULL(TPH.UserName,'') as 'UserName'
	from [ProviderSiteHistory] TPH
	WHERE TrainingProviderHistoryId = @historyId

SET NOCOUNT OFF        
END