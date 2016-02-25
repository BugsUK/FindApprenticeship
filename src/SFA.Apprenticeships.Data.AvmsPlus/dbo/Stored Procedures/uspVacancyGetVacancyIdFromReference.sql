CREATE PROCEDURE [dbo].[uspVacancyGetVacancyIdFromReference]
    @vacancyReference   INT,
    @vacancyId          INT OUTPUT
AS
BEGIN            
            
SET NOCOUNT ON            
           
    SELECT
        @vacancyId = Vacancy.VacancyId
    FROM
        Vacancy
    WHERE
        Vacancy.VacancyReferenceNumber = @vacancyReference

SET NOCOUNT OFF            
END