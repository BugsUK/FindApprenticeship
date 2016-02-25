CREATE PROCEDURE [dbo].[uspEmployerSelectTaskSummary]       
	@EmployerId int           
AS      
BEGIN
	SET NOCOUNT ON      

	Declare @FilledVacToComplete int
	Declare @MissingILRDetails int
	Declare @UnfilledVacsClosingDatePassed int
	Declare @VacDescRework int
	Declare @VacWithNewApp int
	Declare @VacWithWithrawnApp int

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
					dbo.Employer ON dbo.[VacancyOwnerRelationship].EmployerId = dbo.Employer.EmployerId
				WHERE (dbo.Employer.EmployerId = @EmployerId) AND (dbo.Vacancy.VacancyStatusId <>
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
				dbo.Employer ON dbo.[VacancyOwnerRelationship].EmployerId = dbo.Employer.EmployerId
			WHERE (dbo.Employer.EmployerId = @EmployerId) 
				AND dbo.[VacancyOwnerRelationship].ManagerIsEmployer = 1
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
	SELECT @VacDescRework = COUNT(1)
		FROM vacancy 
			INNER JOIN [VacancyOwnerRelationship] ON 
				[vacancy].[VacancyOwnerRelationshipId] = [VacancyOwnerRelationship].[VacancyOwnerRelationshipId] AND 
				[VacancyOwnerRelationship].[ManagerIsEmployer] = 1
			INNER JOIN vacancyStatustype ON 
				[vacancy].vacancyStatusId = vacancyStatustype.vacancyStatusTypeId   
		WHERE [VacancyOwnerRelationship].[EmployerId] = @EmployerId  AND  
				[VacancyStatusType].[CodeName] = 'Ref'     

	-- Vacancies with new Applications
	SELECT @VacWithNewApp = COUNT(T.VacancyId) 
		FROM (Select Distinct vacancy.VACANCYID AS 'VacancyId'
				From vacancy
					INNER JOIN [VacancyOwnerRelationship] ON [VacancyOwnerRelationship].[VacancyOwnerRelationshipId] = vacancy.[VacancyOwnerRelationshipId] 
						AND [VacancyOwnerRelationship].[ManagerIsEmployer] = 1  
					INNER JOIN [Application] ON [Vacancy].[VacancyId] = [Application].[VacancyId]   
					INNER JOIN [ApplicationStatusType] ON [Application].[ApplicationStatusTypeId] = [ApplicationStatusType].[ApplicationStatusTypeId] AND [ApplicationStatusType].[CodeName] = 'NEW'  
					--Inner join vacancyStatustype on VacancyProvisionRelationship.StatusTypeId = vacancyStatustype.vacancyStatusTypeId 
					Inner join vacancyStatustype on [Vacancy].VacancyStatusId = vacancyStatustype.vacancyStatusTypeId 

				where [VacancyOwnerRelationship].[EmployerId] = @EmployerId
					AND  VacancyStatusType.CodeName != 'Del' 
					GROUP BY VACANCY.VACANCYID) AS T

	SELECT @VacWithWithrawnApp = 0

	-- Return all the Task count results
	SELECT @FilledVacToComplete           'FilledVacToComplete',
		 @MissingILRDetails             'MissingILRDetails',
		 @UnfilledVacsClosingDatePassed 'UnfilledVacsClosingDatePassed',
		 @VacDescRework                 'VacDescRework',
		 @VacWithNewApp                 'VacWithNewApp',
		 @VacWithWithrawnApp            'VacWithWithrawnApp'

	SET NOCOUNT OFF
END