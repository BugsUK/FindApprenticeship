create PROCEDURE [dbo].[uspCandidateGetHistory]
	@CandidateId int           
AS      
BEGIN
	SET NOCOUNT ON 

SELECT MyTable.*  FROM            
(	
	select 
	TotalRows=count(1) Over(),
	ROW_NUMBER() OVER( ORDER BY CH.EventDate   desc) as RowNum,
	
	CH.CandidateHistoryId as 'HistoryId',
	CH.CandidateId,
	CH.CandidateHistoryEventTypeId as 'EventTypeId',
	isnull(LEFT(CH.Comment,20),'') as 'Comment',
	CH.EventDate as 'EventDate',
	isnull(CH.UserName,'') as 'UserName'
	from CandidateHistory CH
	inner join CandidateHistoryEvent CHE on CH.CandidateHistoryEventTypeId = CHE.CandidateHistoryEventId
	where CHE.CodeName = 'NTE' and CandidateId = @CandidateId
	
) as MyTable

	     
	SET NOCOUNT OFF
END