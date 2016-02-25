CREATE PROCEDURE [dbo].[uspVacancyDelete]      
	@vacancyId int      
AS      
BEGIN      
	SET NOCOUNT ON      
	
	DELETE FROM [dbo].[VacancyTextField]
		WHERE [VacancyId] = @VacancyId
		
	DELETE FROM [dbo].[AdditionalQuestion]
		WHERE [VacancyId]=@vacancyId
	 
	DELETE FROM [dbo].[Application]
		WHERE [VacancyId]=@vacancyId

	DELETE FROM [dbo].[subVacancy]
		WHERE [VacancyId]=@vacancyId

	DELETE FROM [dbo].[VacancyHistory]
		WHERE [VacancyId]=@vacancyId
	 
	DELETE FROM [dbo].[WatchedVacancy]
		WHERE [VacancyId]=@vacancyId

	DELETE FROM [dbo].[VacancyReferralComments]
		WHERE [VacancyId]=@vacancyId

	DELETE FROM [dbo].[Vacancy]      
		WHERE [VacancyId]=@vacancyId      
	      
	SET NOCOUNT OFF      
END