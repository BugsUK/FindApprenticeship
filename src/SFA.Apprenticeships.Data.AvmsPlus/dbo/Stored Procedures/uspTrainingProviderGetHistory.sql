create PROCEDURE [dbo].[uspTrainingProviderGetHistory]
	@ProviderId int           
AS      
BEGIN
	SET NOCOUNT ON 

SELECT MyTable.*  FROM            
(	
	select 
	TotalRows=count(1) Over(),
	ROW_NUMBER() OVER( ORDER BY TPH.HistoryDate   desc) as RowNum,

	TPH.TrainingProviderHistoryId as 'HistoryId',
	TPH.TrainingProviderId,
	TPH.EventTypeId as 'EventTypeId',
	ISNULL(LEFT(TPH.Comment,20),'') as 'Comment',
	TPH.HistoryDate as 'EventDate',
	ISNULL(TPH.UserName,'') as 'UserName'
	from [ProviderSiteHistory] TPH
	inner join [ProviderSiteHistoryEventType] TPHT on TPH.[EventTypeId] = TPHT.[ProviderSiteHistoryEventTypeId]
	where TPHT.CodeName = 'NTE' and TrainingProviderId = @ProviderId
	
) as MyTable
	     
	SET NOCOUNT OFF
END