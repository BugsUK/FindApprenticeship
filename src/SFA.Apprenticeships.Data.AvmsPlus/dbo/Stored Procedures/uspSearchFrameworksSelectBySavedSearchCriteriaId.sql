Create Procedure [dbo].[uspSearchFrameworksSelectBySavedSearchCriteriaId]
	@savedSearchCriteriaId int
As
Begin

	Select SearchFrameworksId, ssc.SavedSearchCriteriaId, FrameworkId, af.ApprenticeshipOccupationId
	From SearchFrameworks sf, ApprenticeshipFramework af, SavedSearchCriteria ssc
	Where	sf.SavedSearchCriteriaId = ssc.SavedSearchCriteriaId
	And		sf.FrameworkId = af.ApprenticeshipFrameworkId
	And		ssc.SavedSearchCriteriaId = @savedSearchCriteriaId
			
End