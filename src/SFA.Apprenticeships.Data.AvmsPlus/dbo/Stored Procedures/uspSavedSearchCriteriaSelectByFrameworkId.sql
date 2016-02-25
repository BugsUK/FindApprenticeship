Create Procedure [dbo].[uspSavedSearchCriteriaSelectByFrameworkId]
	@frameworkId int
As
Begin
	SET NOCOUNT ON;

	Select * from SearchFrameworks sf, SavedSearchCriteria sc
	Where sf.SavedSearchCriteriaID = sc.SavedSearchCriteriaID
	And sf.FrameworkId = @frameworkId
	
	SET NOCOUNT OFF;
End