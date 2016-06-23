CREATE PROCEDURE [dbo].[uspApplicationSelectByUnSuccessfulCandidateId]                  
(            
@CandidateId int,            
@RejectedReasons bit
)            
AS              
BEGIN              
        
            
SET NOCOUNT ON              

SELECT [Application].CandidateId, 
Reason = CASE  AURT.FullName when 'Other' THEN [Application].OutcomeReasonOther
		 ELSE  AURT.FullName
		 END,
NextAction= CASE  ANA.FullName when 'Other' THEN [Application].NextActionOther
		 ELSE  ANA.FullName
		 END,
[Application].UnsuccessfulReasonId,
[Application].NextActionId,ANA.FullName

FROM [Application]
INNER JOIN ApplicationUnsuccessfulReasonType AURT ON AURT.ApplicationUnsuccessfulReasonTypeId=[dbo].[Application].UnsuccessfulReasonId
INNER JOIN ApplicationNextAction ANA ON ANA.ApplicationNextActionId = [Application].NextActionId
WHERE CandidateId = @CandidateId
ORDER BY AURT.ReferralPoints   desc
SET NOCOUNT OFF              
END