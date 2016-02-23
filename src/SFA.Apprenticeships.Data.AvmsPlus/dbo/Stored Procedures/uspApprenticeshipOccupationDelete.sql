Create Procedure [dbo].[uspApprenticeshipOccupationDelete]
	@apprOccpId int
As
Begin

	-- Remove second candidate preference to deleted occupation / all frameworks
	Update CandidatePreferences
	Set    SecondOccupationId = Null
	Where  SecondOccupationId = @apprOccpId
	And    SecondFrameworkId Is Null

	-- Remove first candidate preference to deleted occupation / all frameworks
	-- and cascade second to the first
	Update CandidatePreferences
	Set    FirstOccupationId = SecondOccupationId,
	       FirstFrameworkId = SecondFrameworkId,
	       SecondOccupationId = Null,
	       SecondFrameworkId = Null
	Where  FirstOccupationId = @apprOccpId
	And    FirstFrameworkId Is Null

	-- Delete the occupation
	Delete From ApprenticeshipOccupation
	Where	ApprenticeshipOccupationId = @apprOccpId

End