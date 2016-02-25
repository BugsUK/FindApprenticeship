CREATE PROCEDURE [dbo].[uspSearchVacUnfilledPassedClosingDate]
	@EmployerId Int,
	@TrainingProviderId Int,
	@vacancyManagerId int = null,
	@SortByField NVarchar(20),
	@IsSortAsc Bit,
	@PageNumber Int,
	@PageSize Int,
	@TotalRows Int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	/*---------> Initialisation */
	Declare @FirstRecord Int
	Declare @LastRecord Int

	/* Ensure we have a valid page number */
	If @PageNumber < 1
		Set @PageNumber = 1

	/* Ensure we have a valid page size */
	If @PageSize < 1
		Set @PageSize = 10

	/* Get the start and end row number for the 
		requested page */
	Set @FirstRecord = (@PageNumber - 1) * @PageSize
	Set @LastRecord = @FirstRecord + @PageSize


	If @SortByField = 'VacancyTitle'
	BEGIN  
		SET @SortByField = 'Title'
	END
	
	If @SortByField = 'EmployerName'
	BEGIN  
		SET @SortByField = 'FullName'
	END

	If @IsSortAsc = 1 BEGIN Set @SortByField = @SortByField + ' Asc' END            
	If @IsSortAsc = 0 BEGIN Set @SortByField = @SortByField + ' desc' END  
	/*---------> End */



	IF ISNULL(@TrainingProviderId, 0) <> 0
	BEGIN

		/*---------> Get the total number of vacancies for this query */
		SELECT @TotalRows = COUNT(*)
			FROM dbo.Vacancy INNER JOIN
				dbo.VacancyStatusType ON dbo.Vacancy.VacancyStatusId = dbo.VacancyStatusType.VacancyStatusTypeId INNER JOIN
				dbo.[VacancyOwnerRelationship] ON 
				dbo.Vacancy.[VacancyOwnerRelationshipId] = dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] INNER JOIN
				dbo.[ProviderSite] ON dbo.[VacancyOwnerRelationship].[ProviderSiteID] = dbo.[ProviderSite].ProviderSiteID
			WHERE (dbo.[ProviderSite].ProviderSiteID = @TrainingProviderId)
				AND dbo.[VacancyOwnerRelationship].ManagerIsEmployer = 0
				AND (
					(@vacancyManagerId IS NULL) OR
					(Vacancy.VacancyManagerId = @vacancyManagerId)
				)
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
		/*---------> End */



		/*---------> Get the desired page of data for this query */
		SELECT *
			FROM (
				SELECT ROW_NUMBER() OVER
						( 
							ORDER BY
							Case When @SortByField = 'Title Asc' Then dbo.Vacancy.Title End Asc, 
							Case When @SortByField = 'Title Desc' Then dbo.Vacancy.Title End Desc,
							Case When @SortByField = 'Town Asc' Then dbo.Vacancy.Town End Asc,
							Case When @SortByField = 'Town Desc' Then dbo.Vacancy.Town End Desc,
							-- Added ITSM803848 - DS
							Case When @SortByField = 'FullName Asc' Then dbo.Employer.FullName End Asc,
							Case When @SortByField = 'FullName Desc' Then dbo.Employer.FullName End Desc,
							Case When @SortByField = 'VacancyType Asc' Then dbo.Vacancy.ApprenticeshipType End Asc,
							Case When @SortByField = 'VacancyType Desc' Then dbo.Vacancy.ApprenticeshipType End Desc,
							Case When @SortByField = 'FrameworkName Asc' Then dbo.ApprenticeshipFramework.FullName End Asc,
							Case When @SortByField = 'FrameworkName Desc' Then dbo.ApprenticeshipFramework.FullName End Desc,
							Case When @SortByField = 'NoOfVacs Asc' Then dbo.Vacancy.NumberofPositions End Asc,
							Case When @SortByField = 'NoOfVacs Desc' Then dbo.Vacancy.NumberofPositions End Desc,
							Case When @SortByField = 'NoOfApps Asc' Then (SELECT COUNT(*) FROM dbo.Application 
								INNER JOIN dbo.ApplicationStatusType 
									ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId
							WHERE (dbo.Application.VacancyId = dbo.Vacancy.VacancyId) AND
								  (dbo.ApplicationStatusType.CodeName <> N'DRF')) End Asc,
							Case When @SortByField = 'NoOfApps Desc' Then (SELECT COUNT(*) FROM dbo.Application 
								INNER JOIN dbo.ApplicationStatusType 
									ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId
							WHERE (dbo.Application.VacancyId = dbo.Vacancy.VacancyId) AND
								  (dbo.ApplicationStatusType.CodeName <> N'DRF')) End Desc,
							Case When @SortByField = 'ClosingDate Asc' Then dbo.Vacancy.ApplicationClosingDate End Asc,
							Case When @SortByField = 'ClosingDate Desc' Then dbo.Vacancy.ApplicationClosingDate End Desc,
							-- ITSM803848
						dbo.Vacancy.ApplicationClosingDate Asc
						) AS 'RowNumber',
						dbo.Vacancy.VacancyId,
						dbo.Vacancy.ApprenticeshipType AS VacancyType, 
						dbo.Vacancy.Title, 
						dbo.Vacancy.VacancyStatusId, 
						--dbo.TrainingProvider.FullName,
						dbo.Employer.FullName,
						dbo.Vacancy.NumberofPositions, 
						dbo.Vacancy.ApplicationClosingDate, 
						dbo.ApprenticeshipFramework.ApprenticeshipFrameworkId, 
						dbo.ApprenticeshipFramework.ApprenticeshipOccupationId,
						dbo.ApprenticeshipFramework.FullName AS FrameworkName,
						(SELECT COUNT(*) FROM dbo.Application 
								INNER JOIN dbo.ApplicationStatusType 
									ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId
							WHERE (dbo.Application.VacancyId = dbo.Vacancy.VacancyId) AND
								  (dbo.ApplicationStatusType.CodeName <> N'DRF')) AS NumberOfApplications,
						(SELECT COUNT(*) FROM dbo.Application 
								INNER JOIN dbo.ApplicationStatusType 
									ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId
							WHERE (dbo.ApplicationStatusType.CodeName = N'SUC') AND (dbo.Application.VacancyId = dbo.Vacancy.VacancyId)) AS SuccessfulApplications
					FROM dbo.Vacancy 
						INNER JOIN dbo.ApprenticeshipFramework ON dbo.Vacancy.ApprenticeshipFrameworkId = dbo.ApprenticeshipFramework.ApprenticeshipFrameworkId 
						INNER JOIN dbo.[VacancyOwnerRelationship] ON dbo.Vacancy.[VacancyOwnerRelationshipId] = dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] 
						INNER JOIN dbo.[ProviderSite] ON dbo.[VacancyOwnerRelationship].[ProviderSiteID] = dbo.[ProviderSite].ProviderSiteID
						INNER JOIN dbo.Employer ON dbo.Employer.EmployerId = dbo.[VacancyOwnerRelationship].EmployerId
					WHERE 
					(
						(@vacancyManagerId IS NULL) OR
						(Vacancy.VacancyManagerId = @vacancyManagerId)
					) AND
					dbo.Vacancy.VacancyId IN
						(SELECT Vacancy.VacancyId
							FROM dbo.Vacancy INNER JOIN
								dbo.VacancyStatusType ON dbo.Vacancy.VacancyStatusId = dbo.VacancyStatusType.VacancyStatusTypeId INNER JOIN
								dbo.[VacancyOwnerRelationship] ON 
								dbo.Vacancy.[VacancyOwnerRelationshipId] = dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] INNER JOIN
								dbo.[ProviderSite] ON dbo.[VacancyOwnerRelationship].[ProviderSiteID] = dbo.[ProviderSite].ProviderSiteID
							WHERE (dbo.[ProviderSite].ProviderSiteID = @TrainingProviderId) 
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
											WHERE (dbo.ApplicationStatusType.CodeName = N'SUC') AND (dbo.Application.VacancyId = dbo.Vacancy.VacancyId))) > 0))
								) X1
				WHERE (RowNumber > @FirstRecord) AND (RowNumber <= @LastRecord)
		/*---------> End */
	
	END
	ELSE
	BEGIN
	
		/*---------> Get the total number of vacancies for this query */
		SELECT @TotalRows = COUNT(*)
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
		/*---------> End */



		/*---------> Get the desired page of data for this query */
		SELECT *
			FROM (
				SELECT ROW_NUMBER() OVER
						( 
							ORDER BY
							Case When @SortByField = 'Title Asc' Then dbo.Vacancy.Title End Asc, 
							Case When @SortByField = 'Title Desc' Then dbo.Vacancy.Title End Desc,
							Case When @SortByField = 'Town Asc' Then dbo.Vacancy.Town End Asc,
							Case When @SortByField = 'Town Desc' Then dbo.Vacancy.Town End Desc,
							-- Added ITSM803848 - DS
							Case When @SortByField = 'FullName Asc' Then dbo.Employer.FullName End Asc,
							Case When @SortByField = 'FullName Desc' Then dbo.Employer.FullName End Desc,
							Case When @SortByField = 'VacancyType Asc' Then dbo.Vacancy.ApprenticeshipType End Asc,
							Case When @SortByField = 'VacancyType Desc' Then dbo.Vacancy.ApprenticeshipType End Desc,
							Case When @SortByField = 'FrameworkName Asc' Then dbo.ApprenticeshipFramework.FullName End Asc,
							Case When @SortByField = 'FrameworkName Desc' Then dbo.ApprenticeshipFramework.FullName End Desc,
							Case When @SortByField = 'NoOfVacs Asc' Then dbo.Vacancy.NumberofPositions End Asc,
							Case When @SortByField = 'NoOfVacs Desc' Then dbo.Vacancy.NumberofPositions End Desc,
							Case When @SortByField = 'NoOfApps Asc' Then (SELECT COUNT(*) FROM dbo.Application 
								INNER JOIN dbo.ApplicationStatusType 
									ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId
							WHERE (dbo.Application.VacancyId = dbo.Vacancy.VacancyId) AND
								  (dbo.ApplicationStatusType.CodeName <> N'DRF')) End Asc,
							Case When @SortByField = 'NoOfApps Desc' Then (SELECT COUNT(*) FROM dbo.Application 
								INNER JOIN dbo.ApplicationStatusType 
									ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId
							WHERE (dbo.Application.VacancyId = dbo.Vacancy.VacancyId) AND
								  (dbo.ApplicationStatusType.CodeName <> N'DRF')) End Desc,
							Case When @SortByField = 'ClosingDate Asc' Then dbo.Vacancy.ApplicationClosingDate End Asc,
							Case When @SortByField = 'ClosingDate Desc' Then dbo.Vacancy.ApplicationClosingDate End Desc,
							-- ITSM803848
							dbo.Vacancy.ApplicationClosingDate Asc
						) AS 'RowNumber',
						dbo.Vacancy.VacancyId,
						dbo.Vacancy.ApprenticeshipType AS VacancyType, 
						dbo.Vacancy.Title, 
						dbo.Vacancy.VacancyStatusId,
						dbo.[ProviderSite].FullName,
						--dbo.Employer.FullName,
						dbo.Vacancy.NumberofPositions, 
						dbo.Vacancy.ApplicationClosingDate, 
						dbo.ApprenticeshipFramework.ApprenticeshipFrameworkId, 
						dbo.ApprenticeshipFramework.ApprenticeshipOccupationId,
						dbo.ApprenticeshipFramework.FullName AS FrameworkName,
						(SELECT COUNT(*) FROM dbo.Application 
								INNER JOIN dbo.ApplicationStatusType 
									ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId
							WHERE (dbo.Application.VacancyId = dbo.Vacancy.VacancyId) AND
								  (dbo.ApplicationStatusType.CodeName <> N'DRF')) AS NumberOfApplications,
						(SELECT COUNT(*) FROM dbo.Application 
								INNER JOIN dbo.ApplicationStatusType 
									ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId
							WHERE (dbo.ApplicationStatusType.CodeName = N'SUC') AND (dbo.Application.VacancyId = dbo.Vacancy.VacancyId)) AS SuccessfulApplications
					FROM dbo.Vacancy 
						INNER JOIN dbo.ApprenticeshipFramework ON dbo.Vacancy.ApprenticeshipFrameworkId = dbo.ApprenticeshipFramework.ApprenticeshipFrameworkId 
						INNER JOIN dbo.[VacancyOwnerRelationship] ON dbo.Vacancy.[VacancyOwnerRelationshipId] = dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] 
						INNER JOIN dbo.Employer ON dbo.[VacancyOwnerRelationship].EmployerId = dbo.Employer.EmployerId
						INNER JOIN dbo.[ProviderSite] ON dbo.[VacancyOwnerRelationship].[ProviderSiteID] = dbo.[ProviderSite].ProviderSiteID
					WHERE dbo.Vacancy.VacancyId IN
						(SELECT Vacancy.VacancyId
							FROM dbo.Vacancy INNER JOIN
								dbo.VacancyStatusType ON dbo.Vacancy.VacancyStatusId = dbo.VacancyStatusType.VacancyStatusTypeId INNER JOIN
								dbo.[VacancyOwnerRelationship] ON 
								dbo.Vacancy.[VacancyOwnerRelationshipId] = dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] INNER JOIN
								dbo.[ProviderSite] ON dbo.[VacancyOwnerRelationship].[ProviderSiteID] = dbo.[ProviderSite].ProviderSiteID
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
											WHERE (dbo.ApplicationStatusType.CodeName = N'SUC') AND (dbo.Application.VacancyId = dbo.Vacancy.VacancyId))) > 0))
								) X2
				WHERE (RowNumber > @FirstRecord) AND (RowNumber <= @LastRecord)
		/*---------> End */
		
	END
END