CREATE PROCEDURE [dbo].[uspTrainingProviderSelectTaskSummary]     
 @ProviderId int    
     
AS    
BEGIN    
	SET NOCOUNT ON    

	DECLARE @FilledVacToComplete int
	DECLARE @MissingILRDetails int
	DECLARE @UnfilledVacsClosingDatePassed int
	DECLARE @VacDescRework int
	DECLARE @VacWithNewApp int
	DECLARE @VacWithWithrawnApp int
	DECLARE @CandidateWithdrawalSearchCount int

	-- SELECT count for each task

	-- Filled Vacancies To Complete
	SELECT @FilledVacToComplete = Count(*)
		FROM 
			(SELECT dbo.Vacancy.VacancyId, dbo.Vacancy.NumberofPositions As NumberOfPositions,
					(SELECT COUNT(*) AS TotalCount
						FROM dbo.Application INNER JOIN
							dbo.ApplicationStatusType ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId
						WHERE (dbo.ApplicationStatusType.CodeName = N'SUC') AND (dbo.Application.VacancyId = dbo.Vacancy.VacancyId)) AS PositionsFilled
				FROM dbo.Vacancy INNER JOIN
					dbo.VacancyStatusType ON dbo.Vacancy.VacancyStatusId = dbo.VacancyStatusType.VacancyStatusTypeId INNER JOIN
					dbo.[VacancyOwnerRelationship] ON 
					dbo.Vacancy.[VacancyOwnerRelationshipId] = dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] INNER JOIN
					dbo.[ProviderSite] ON dbo.[VacancyOwnerRelationship].[ProviderSiteID] = dbo.[ProviderSite].ProviderSiteID
				WHERE (dbo.[ProviderSite].ProviderSiteID = @providerId) AND (dbo.Vacancy.VacancyStatusId <>
					(SELECT VacancyStatusTypeId
						FROM dbo.VacancyStatusType AS VacancyStatusType_1
						WHERE (CodeName = N'Com')))) X
		WHERE (X.NumberOfPositions - X.PositionsFilled) = 0

	-- Vacancies with Missing ILR Details
	SELECT @MissingILRDetails = 0

	-- Unfilled Vacancies past their closing date
	SELECT @UnfilledVacsClosingDatePassed = Count(*)
			FROM dbo.Vacancy INNER JOIN
				dbo.VacancyStatusType ON dbo.Vacancy.VacancyStatusId = dbo.VacancyStatusType.VacancyStatusTypeId INNER JOIN
				dbo.[VacancyOwnerRelationship] ON 
				dbo.Vacancy.[VacancyOwnerRelationshipId] = dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] INNER JOIN
				dbo.[ProviderSite] ON dbo.[VacancyOwnerRelationship].[ProviderSiteID] = dbo.[ProviderSite].ProviderSiteID
			WHERE (dbo.[ProviderSite].ProviderSiteID = @ProviderId)
				AND dbo.[VacancyOwnerRelationship].ManagerIsEmployer = 0
				AND VacancyStatusId IN (SELECT dbo.VacancyStatusType.VacancyStatusTypeId 
										FROM dbo.VacancyStatusType
										WHERE dbo.VacancyStatusType.CodeName = N'Lve' OR dbo.VacancyStatusType.CodeName = N'Cld')
				AND (CAST(FLOOR(CAST(Getdate() AS FLOAT))AS DATETIME) > dbo.Vacancy.ApplicationClosingDate)
				AND Vacancy.ApplyOutsideNAVMS = 0
				AND ((dbo.Vacancy.NumberofPositions - 
						(SELECT COUNT(*) AS TotalCount
							FROM dbo.Application INNER JOIN
								dbo.ApplicationStatusType ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId
							WHERE (dbo.ApplicationStatusType.CodeName = N'SUC') AND (dbo.Application.VacancyId = dbo.Vacancy.VacancyId))) > 0)

	-- Vacancies requiring rework of their description
	SELECT @VacDescRework = COUNT(1)--select *
		FROM vacancy     
			INNER JOIN [VacancyOwnerRelationship] ON [vacancy].[VacancyOwnerRelationshipId] = [VacancyOwnerRelationship].[VacancyOwnerRelationshipId] AND 
				[VacancyOwnerRelationship].[ManagerIsEmployer] = 0    
			INNER JOIN vacancyStatustype ON [vacancy].vacancyStatusId = vacancyStatustype.vacancyStatusTypeId   
		WHERE [VacancyOwnerRelationship].[ProviderSiteID] = @providerId  AND  VacancyStatusType.CodeName = 'Ref'

	-- Vacancies with new Applications    
	SELECT @VacWithNewApp = COUNT(T.VacancyId) 
		FROM (Select Distinct vacancy.VACANCYID AS 'VacancyId'  
				From vacancy     
			INNER JOIN [VacancyOwnerRelationship] ON [vacancy].[VacancyOwnerRelationshipId] = [VacancyOwnerRelationship].[VacancyOwnerRelationshipId] AND 
				[VacancyOwnerRelationship].[ManagerIsEmployer] = 0    
			INNER JOIN [Application] ON [Vacancy].[VacancyId] = [Application].[VacancyId]     
			INNER JOIN [ApplicationStatusType] ON [Application].[ApplicationStatusTypeId] = [ApplicationStatusType].[ApplicationStatusTypeId] AND 
				[ApplicationStatusType].[CodeName] = 'NEW'    
			Inner join VacancyProvisionRelationshipStatustype on [VacancyOwnerRelationship].StatusTypeId = VacancyProvisionRelationshipStatustype.VacancyProvisionRelationshipStatusTypeId   
		Where [VacancyOwnerRelationship].[ProviderSiteID] = @providerId  AND  VacancyProvisionRelationshipStatusType.CodeName != 'DEL'   
		GROUP BY VACANCY.VACANCYID) AS T  

	SELECT @VacWithWithrawnApp = 0

	--	CandidateWithdrawalSearchCount
	select @CandidateWithdrawalSearchCount = count(app.ApplicationId)
		from [application] app  
		inner join vacancy v on app.VacancyId=v.VacancyId
		inner join [VacancyOwnerRelationship] vpr on vpr.[VacancyOwnerRelationshipId]=v.[VacancyOwnerRelationshipId]
		inner join [ProviderSite] prov on prov.ProviderSiteID=vpr.[ProviderSiteID]
		inner join Candidate c on c.CandidateId=app.CandidateId
		inner join Person p on c.PersonId=p.PersonId
		inner join Employer e on e.EmployerId=vpr.EmployerId
		inner join ApprenticeshipFramework af on af.ApprenticeshipFrameworkId=v.ApprenticeshipFrameworkId
		where 
		app.ApplicationStatusTypeId=4 
		and prov.ProviderSiteID=@ProviderId
		-- NOTE: CHECK IF ACK = 0 OR 1
		and app.WithdrawalAcknowledged=0
	
	-- Return all the Task count results
	SELECT @FilledVacToComplete         'FilledVacToComplete',
		 @MissingILRDetails             'MissingILRDetails',
		 @UnfilledVacsClosingDatePassed 'UnfilledVacsClosingDatePassed',
		 @VacDescRework                 'VacDescRework',
		 @VacWithNewApp                 'VacWithNewApp',
		 @VacWithWithrawnApp            'VacWithWithrawnApp',
		 @CandidateWithdrawalSearchCount 'CandidateWithdrawalCount'

	SET NOCOUNT OFF   
END