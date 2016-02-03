CREATE PROCEDURE [dbo].[uspEmployerSelectAlertSummary]     
	@employerId int,
	@numberOfDays int
AS    
BEGIN    
	SET NOCOUNT ON    

	DECLARE @candidateWithStalledApps int
	DECLARE @vacApproachingClosingDate int

	SELECT @candidateWithStalledApps = 0

	/* Vacancies with no applications approaching their closing date */
	SELECT @vacApproachingClosingDate = COUNT(*)
		FROM dbo.Employer
			INNER JOIN dbo.[VacancyOwnerRelationship] ON dbo.Employer.EmployerId = dbo.[VacancyOwnerRelationship].EmployerId 
			INNER JOIN dbo.Vacancy ON dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] = dbo.Vacancy.[VacancyOwnerRelationshipId]
		WHERE (dbo.[VacancyOwnerRelationship].ManagerIsEmployer = 1)
			AND dbo.Employer.EmployerId = @EmployerId 
			AND ApplyOutsideNAVMS = 0
			AND VacancyStatusId IN (SELECT dbo.VacancyStatusType.VacancyStatusTypeId 
										FROM dbo.VacancyStatusType
										WHERE dbo.VacancyStatusType.CodeName = N'Lve')
			AND DATEDIFF(dd,dbo.fnx_RemoveTime(GETDATE()),Vacancy.ApplicationClosingDate) <= @numberOfDays
			AND DATEDIFF(dd,dbo.fnx_RemoveTime(GETDATE()),Vacancy.ApplicationClosingDate) >= 0
			AND (SELECT COUNT(*) FROM dbo.[Application] WHERE dbo.[Application].VacancyId = dbo.Vacancy.VacancyId) = 0
	/*---------> End */
	

	-- Return all the Task count results
	SELECT @candidateWithStalledApps         'CandidateWithStalledApps',
		 @vacApproachingClosingDate          'VacApproachingClosingDate'

	SET NOCOUNT OFF    
END