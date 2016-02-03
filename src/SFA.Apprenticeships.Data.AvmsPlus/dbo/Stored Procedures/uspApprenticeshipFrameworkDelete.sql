Create Procedure [dbo].[uspApprenticeshipFrameworkDelete]
	@frameworkId int
As
Begin

	Delete From ApprenticeshipFramework
	Where	ApprenticeshipFrameworkId = @frameworkId

End