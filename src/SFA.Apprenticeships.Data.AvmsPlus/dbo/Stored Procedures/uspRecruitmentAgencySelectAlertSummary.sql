CREATE PROCEDURE [dbo].[uspRecruitmentAgencySelectAlertSummary]    
	@trainingProviderId int,
	@recruitmentAgencyId int,
	@numberOfDays int
AS    
BEGIN    
	SET NOCOUNT ON    

	DECLARE @candidateWithStalledApps int
	DECLARE @vacApproachingClosingDate int

	-- Candidates with stalled applications
	SELECT @candidateWithStalledApps = 0

	-- Vacancies with no applications approaching their closing date
	SELECT @vacApproachingClosingDate = COUNT(*)
		FROM dbo.[ProviderSite] 
			INNER JOIN dbo.[VacancyOwnerRelationship] ON dbo.[ProviderSite].ProviderSiteID = dbo.[VacancyOwnerRelationship].[ProviderSiteID] 
			INNER JOIN dbo.Vacancy ON dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] = dbo.Vacancy.[VacancyOwnerRelationshipId]
		WHERE (dbo.[VacancyOwnerRelationship].ManagerIsEmployer = 0)
			AND dbo.[ProviderSite].ProviderSiteID = @TrainingProviderId 
			AND ApplyOutsideNAVMS = 0
			AND dbo.Vacancy.VacancyManagerId = @recruitmentAgencyId
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
END