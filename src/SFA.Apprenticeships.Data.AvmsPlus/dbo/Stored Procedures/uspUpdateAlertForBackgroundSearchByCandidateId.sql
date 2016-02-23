Create PROCEDURE [dbo].[uspUpdateAlertForBackgroundSearchByCandidateId]        
  @CandidateId int
AS    
BEGIN    
    
 SET NOCOUNT ON     
 Update [SavedSearchCriteria] 
 Set [savedSearchCriteria].[AlertSent]=0 
 WHERE
  [savedSearchCriteria].[CandidateId]=@CandidateId
 
     
 SET NOCOUNT OFF    
END