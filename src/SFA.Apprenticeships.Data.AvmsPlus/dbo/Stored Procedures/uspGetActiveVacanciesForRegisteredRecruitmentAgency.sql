CREATE PROCEDURE [dbo].[uspGetActiveVacanciesForRegisteredRecruitmentAgency]
	@RecAgentId INT
AS
BEGIN              
	-- SET NOCOUNT ON added to prevent extra result sets from              
	-- interfering with SELECT statements.              
	SET NOCOUNT ON;              
   
	SELECT COUNT(Vacancy.VacancyID) as 'ActiveVacancies'
	FROM Vacancy 
	WHERE Vacancy.VacancyStatusId in (1,2,3,5)
		AND VacancyManagerID = @RecAgentId

END