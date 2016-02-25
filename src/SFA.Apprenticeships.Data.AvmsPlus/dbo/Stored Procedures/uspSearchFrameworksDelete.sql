Create Procedure [dbo].[uspSearchFrameworksDelete]
	@savedSearchCriteriaId int,
	@frameworkId int
As
Begin

	Delete from SearchFrameworks
	Where	SavedSearchCriteriaId = @savedSearchCriteriaId 
	And		FrameworkId = @frameworkId
End