Create Procedure [dbo].[uspSavedSearchCriteriaSelectByFrameworkId]
	@frameworkId int
As
Begin
	SET NOCOUNT ON;

	Select * from SearchFrameworks sf, SavedSearchCriteria sc
	Where sf.SavedSearchCriteriaId = sc.SavedSearchCriteriaId
	And sf.FrameworkId = @frameworkId
	
	SET NOCOUNT OFF;
End