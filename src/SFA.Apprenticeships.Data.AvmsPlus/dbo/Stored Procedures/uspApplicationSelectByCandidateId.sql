Create PROCEDURE [dbo].[uspApplicationSelectByCandidateId]                    
(              
@CandidateId int,              
@OnlyRejection bit  
)              
AS                
BEGIN                
          
              
SET NOCOUNT ON                
  
SELECT [Application].ApplicationId,[APPLICATION].CandidateId,   
Reason = CASE  AURT.FullName when 'Other' THEN [Application].OutcomeReasonOther  
   ELSE  AURT.FullName  
   END,  
NextAction= CASE  ANA.FullName when 'Other' THEN [Application].NextActionOther  
   ELSE  ANA.FullName  
   END,  
--[APPLICATION].UnsuccessfulReasonId,  
--[Application].NextActionId,
AURT.ReferralPoints,
MAX(ApplicationHistoryEventDate) AS 'LastDateOfRejection'  
  
FROM [Application]  
INNER JOIN ApplicationUnsuccessfulReasonType AURT ON AURT.ApplicationUnsuccessfulReasonTypeId=[dbo].[Application].UnsuccessfulReasonId  
INNER JOIN ApplicationNextAction ANA ON ANA.ApplicationNextActionId = [Application].NextActionId
LEFT outer JOIN dbo.ApplicationHistory ON   
dbo.[Application].ApplicationId = dbo.ApplicationHistory.ApplicationId   
AND ApplicationHistoryEventTypeId = 1      -- Status Change  
AND ApplicationHistoryEventSubTypeId =  5  -- ApplicationstatusType.Rejected 
			  
WHERE CandidateId = @CandidateId 
AND AURT.ReferralPoints != 0
GROUP BY dbo.[Application].ApplicationId,[APPLICATION].CandidateId,
AURT.FullName,Application.OutcomeReasonOther,ANA.FullName,Application.NextActionOther
,AURT.ReferralPoints
ORDER BY AURT.ReferralPoints   desc  
SET NOCOUNT OFF                
END