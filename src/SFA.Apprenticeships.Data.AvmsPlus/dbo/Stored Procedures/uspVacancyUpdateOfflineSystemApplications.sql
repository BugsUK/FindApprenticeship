CREATE PROCEDURE [dbo].[uspVacancyUpdateOfflineSystemApplications]
    @vacancyReference INT,
    @newApplicantCount INT,
    @oldApplicationCount INT OUTPUT
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Retrieve and store the currently set No of Offline System Applicants
    SELECT
        @oldApplicationCount = Vacancy.NoOfOfflineSystemApplicants
    FROM
        Vacancy
    WHERE
        VacancyReferenceNumber = @vacancyReference



    -- Overwrite any existing value with whatever has been supplied
    -- we are NOT doing any addition to existing counts
    UPDATE 
        Vacancy 
    SET 
        NoOfOfflineSystemApplicants = @newApplicantCount 
    WHERE 
        VacancyReferenceNumber = @vacancyReference

END