CREATE PROCEDURE [dbo].[uspVacancyGetChildVacancyIdsFromMasterVacancyId]
    @masterVacancyId    INT
AS
BEGIN

SET NOCOUNT ON

    SELECT
        VacancyId
    FROM
        Vacancy
    WHERE
        Vacancy.MasterVacancyId = @masterVacancyId
        AND Vacancy.VacancyId <> @masterVacancyId

SET NOCOUNT OFF
END