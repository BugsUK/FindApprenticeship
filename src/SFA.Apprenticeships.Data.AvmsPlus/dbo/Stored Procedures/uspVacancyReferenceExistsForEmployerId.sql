CREATE PROCEDURE [dbo].[uspVacancyReferenceExistsForEmployerId]
    @vacancyReference INT,
    @employerId INT
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Find the Vacanies that match the given Reference and Employer Id
    SELECT
        * 
    FROM
        Vacancy INNER JOIN [VacancyOwnerRelationship] 
            ON Vacancy.[VacancyOwnerRelationshipId] = [VacancyOwnerRelationship].[VacancyOwnerRelationshipId]
            AND [VacancyOwnerRelationship].EmployerId = @employerId
            AND Vacancy.VacancyReferenceNumber = @vacancyReference

END