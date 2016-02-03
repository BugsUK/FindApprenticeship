Create Procedure [dbo].[uspApprenticeshipFrameworkSelectByFrameworkId]
	@frameworkId int
As
Begin

	Select * from ApprenticeshipFramework
	Where ApprenticeshipFrameworkId = @frameworkId
End