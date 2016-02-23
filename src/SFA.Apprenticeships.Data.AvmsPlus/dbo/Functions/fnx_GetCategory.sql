-- =============================================
-- Author:		Hitesh
-- Create date: 05 sep 2008
-- Description:	
-- =============================================
create FUNCTION [dbo].[fnx_GetCategory]
(
	@CandidateId int
)
RETURNS int
AS
BEGIN

			DECLARE @category int
			declare @cntRejectedApplication int
			Declare @cntReferralPoints int
		
			select @cntRejectedApplication=count(t1.candidateId),@cntReferralPoints=t1.ReferralPoints
			from
				(SELECT Candidate.CandidateId,Candidate.ReferralPoints,application.applicationId,ApplicationHistory.ApplicationHistoryEventDate
				 FROM Candidate           
							  INNER JOIN [APPLICATION] ON dbo.[Application].CandidateId = candidate.CandidateId  
							  inner join ApplicationUnsuccessfulReasonType
								on ApplicationUnsuccessfulReasonType.ApplicationUnsuccessfulReasonTypeId = [APPLICATION].UnsuccessfulReasonId
								and ApplicationUnsuccessfulReasonType.ReferralPoints != 0
				 LEFT outer JOIN dbo.ApplicationHistory ON   
							  dbo.[Application].ApplicationId = dbo.ApplicationHistory.ApplicationId   
							  AND ApplicationHistoryEventTypeId = 1      -- Status Change  
							  AND ApplicationHistoryEventSubTypeId =  5  -- ApplicationstatusType.Rejected
				 where candidate.candidateId = @CandidateId
				) t1
			left outer join 
				(select Candidate.CandidateId,max(candidateHistory.EventDate) as historydate
				from candidate
				left outer join candidateHistory on candidate.candidateId = CandidateHistory.candidateId
				and candidateHistory.CandidateHistoryEventTypeId =2
				where Candidate.candidateid = @CandidateId
				group by Candidate.candidateid 
				) t2
		on t1.candidateid = t2.candidateId
		where 
		(t1.ApplicationHistoryEventDate > t2.historydate  or t2.historydate is null)
		group by t1.candidateid ,t1.ReferralPoints
		order by t1.candidateId
	
		--select @cntRejectedApplication
		--select @cntReferralPoints
		if (@cntRejectedApplication / @cntReferralPoints) <= 1
			set @category =1
		else
			set @category =2
		
	


	RETURN @category 

END