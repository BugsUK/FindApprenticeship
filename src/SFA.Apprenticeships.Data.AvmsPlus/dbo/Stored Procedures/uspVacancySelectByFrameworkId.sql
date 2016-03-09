Create Procedure [dbo].[uspVacancySelectByFrameworkId]
	@apprFrameworkId int
As
Begin
	SET NOCOUNT ON  

	Begin Try
		Select VacancyId, VacancyStatusId from Vacancy 	
		Where ApprenticeshipFrameworkId = @apprFrameworkId
	End Try

	Begin Catch  
		EXEC RethrowError;  
	END Catch
  
    SET NOCOUNT OFF  
END