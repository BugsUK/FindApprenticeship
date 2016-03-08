CREATE PROCEDURE [dbo].[uspGetApplicationHistoryCandidateUpdateCount]    
 @candidateId int,
 @candidateCount int OUT  


AS


BEGIN
	SET NOCOUNT ON
    
--//TODO CandidateHistoryEventTypeId Status or Significance to be confirmed in CandidateHistory Table.     
             
DECLARE @WithdrawnApplicationHistoryEventID int
DECLARE @RejectedApplicationHistoryEventID int
DECLARE @ApplicationCount int
DECLARE @LastLoginDate Datetime

SELECT @WithdrawnApplicationHistoryEventID = ApplicationHistoryEventId  from ApplicationHistoryEvent Where 
                                             FullName = 'Withdrawn'

SELECT @RejectedApplicationHistoryEventID = ApplicationHistoryEventId  from ApplicationHistoryEvent Where 
                                             FullName = 'Rejected'

SELECT TOP 1 @LastLoginDate = LastAccessedManageApplications from Candidate Where CandidateId = @candidateId 
                              

SELECT @candidateCount = Count(*) from ApplicationHistory AH
                           Inner Join Application A on A.ApplicationId = AH.ApplicationID
                           Where A.CandidateId = @candidateId 
-- Commented as per design change from ATul as on 21-Aug-08
-- Uncomment for issue in UAT and AM
                          AND 
                           AH.ApplicationHistoryEventSubTypeId Not in 
						(select Applicationstatustypeid from applicationstatustype where codename in ('DRF','NEW'))	
--                          (@WithdrawnApplicationHistoryEventID,@RejectedApplicationHistoryEventID)
-- Commented as per design change from ATul as on 21-Aug-08
                          AND
                           Convert(datetime,AH.ApplicationHistoryEventDate,113) >= Convert(datetime,@LastLoginDate,113)

        --Print @candidateCount

        --SET @candidateCount = @candidateCount
--Candidate        

	SET NOCOUNT OFF
END