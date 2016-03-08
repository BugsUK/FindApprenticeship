Create Procedure [dbo].[uspCandidatePreferencesUpdate]
	@candidatePreferenceId int,
	@firstFrameworkId int,
	@firstOccupationId int,
	@secondFrameworkId int,
	@secondOccupationId int
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    If @secondFrameworkId = 0
		Set @secondFrameworkId = null
		
    If @secondOccupationId = 0
		Set @secondOccupationId = null

	If @firstFrameworkId = 0
		Set @firstFrameworkId = null  

	Update CandidatePreferences  
	Set	FirstFrameworkId = @firstFrameworkId,
		FirstOccupationId = @firstOccupationId,
		SecondFrameworkId = @secondFrameworkId,
		SecondOccupationId = @secondOccupationId
	Where	CandidatePreferenceId = @candidatePreferenceId

END