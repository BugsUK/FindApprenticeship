Create PROCEDURE [dbo].[uspGetNoOfOfflineApplicants]
@VacancyId int

AS
BEGIN

	SET NOCOUNT ON
	
	select	NoOfOfflineApplicants
	from	Vacancy
	where VacancyId=@VacancyId

	
	SET NOCOUNT OFF
END