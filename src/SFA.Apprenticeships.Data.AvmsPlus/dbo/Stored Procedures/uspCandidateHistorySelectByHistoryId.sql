create  PROCEDURE [dbo].[uspCandidateHistorySelectByHistoryId]         
 @historyId INT
 
AS        
BEGIN        
        
SET NOCOUNT ON        
         
select 
CH.CandidateHistoryId as 'HistoryId',
CH.CandidateId,
CH.CandidateHistoryEventTypeId as 'EventTypeId',
isnull(CH.Comment,'') as 'Comment',
CH.EventDate as 'EventDate',
ISNULL(CH.UserName,'') as 'UserName'
from CandidateHistory CH
WHERE CandidateHistoryId = @historyId 
	
SET NOCOUNT OFF        
END