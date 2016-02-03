CREATE PROCEDURE [dbo].[uspVacancySelectFilledNotCompleted]
	@EmployerId Int = 0,
	@TrainingProviderId Int = 0,
	@VacancyManagerId int = null,
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


	
	IF @TrainingProviderId <> 0
	BEGIN

	/*---------> Get the total number of vacancies for this query */
	SELECT @TotalRows = COUNT(*)
		FROM dbo.Vacancy INNER JOIN
			dbo.VacancyStatusType ON dbo.Vacancy.VacancyStatusId = dbo.VacancyStatusType.VacancyStatusTypeId INNER JOIN
			dbo.[VacancyOwnerRelationship] ON 
			dbo.Vacancy.[VacancyOwnerRelationshipId] = dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] INNER JOIN
			dbo.[ProviderSite] ON dbo.[VacancyOwnerRelationship].[ProviderSiteID] = dbo.[ProviderSite].ProviderSiteID
		WHERE (dbo.[ProviderSite].ProviderSiteID = @TrainingProviderId) 
			AND
			(
				@VacancyManagerId IS NULL OR dbo.Vacancy.VacancyManagerId = @VacancyManagerId
			)
			AND 
			(dbo.Vacancy.VacancyStatusId <>
				(SELECT VacancyStatusTypeId
					FROM dbo.VacancyStatusType AS VacancyStatusType_1
					WHERE (CodeName = N'Com'))
			AND
			(Vacancy.NumberofPositions - 
				(SELECT COUNT(*)
					FROM dbo.Application INNER JOIN
						dbo.ApplicationStatusType ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId
					WHERE (dbo.ApplicationStatusType.CodeName = N'SUC') AND (dbo.Application.VacancyId = dbo.Vacancy.VacancyId))) = 0) 
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
					dbo.ApprenticeshipFramework.Fullname as 'FrameworkName',
					(SELECT COUNT(*) FROM dbo.Application WHERE dbo.Application.VacancyId = dbo.Vacancy.VacancyId) AS NumberOfApplications,
					(SELECT COUNT(*) FROM dbo.Application 
							INNER JOIN dbo.ApplicationStatusType ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId
						WHERE (dbo.ApplicationStatusType.CodeName = N'SUC') AND (dbo.Application.VacancyId = dbo.Vacancy.VacancyId)) AS SuccessfulApplications
				FROM dbo.Vacancy 
					INNER JOIN dbo.ApprenticeshipFramework ON dbo.Vacancy.ApprenticeshipFrameworkId = dbo.ApprenticeshipFramework.ApprenticeshipFrameworkId 
					INNER JOIN dbo.[VacancyOwnerRelationship] ON dbo.Vacancy.[VacancyOwnerRelationshipId] = dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] 
					--INNER JOIN dbo.TrainingProvider ON dbo.VacancyProvisionRelationship.TrainingProviderId = dbo.TrainingProvider.TrainingProviderId
					INNER JOIN dbo.Employer ON dbo.Employer.EmployerID = dbo.[VacancyOwnerRelationship].EmployerID
				WHERE 
				(
					@VacancyManagerId IS NULL OR dbo.Vacancy.VacancyManagerId = @VacancyManagerId
				)
				AND
				dbo.Vacancy.VacancyId IN
					(SELECT dbo.Vacancy.VacancyId
						FROM dbo.Vacancy INNER JOIN
							dbo.VacancyStatusType ON dbo.Vacancy.VacancyStatusId = dbo.VacancyStatusType.VacancyStatusTypeId INNER JOIN
							dbo.[VacancyOwnerRelationship] ON 
							dbo.Vacancy.[VacancyOwnerRelationshipId] = dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] INNER JOIN
							dbo.[ProviderSite] ON dbo.[VacancyOwnerRelationship].[ProviderSiteID] = dbo.[ProviderSite].ProviderSiteID
						WHERE 
							(dbo.[ProviderSite].ProviderSiteID = @TrainingProviderId) 
							AND 
							(dbo.Vacancy.VacancyStatusId <>
								(SELECT VacancyStatusTypeId
									FROM dbo.VacancyStatusType AS VacancyStatusType_1
									WHERE (CodeName = N'Com')))
							AND
							(Vacancy.NumberofPositions - 
								(SELECT COUNT(*) AS TotalCount
									FROM dbo.Application INNER JOIN
										dbo.ApplicationStatusType ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId
											WHERE (dbo.ApplicationStatusType.CodeName = N'SUC') AND (dbo.Application.VacancyId = dbo.Vacancy.VacancyId))) = 0)
			) X
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
				AND 
				(dbo.Vacancy.VacancyStatusId <>
					(SELECT VacancyStatusTypeId
						FROM dbo.VacancyStatusType AS VacancyStatusType_1
						WHERE (CodeName = N'Com'))
				AND
				(Vacancy.NumberofPositions - 
					(SELECT COUNT(*)
						FROM dbo.Application INNER JOIN
							dbo.ApplicationStatusType ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId
						WHERE (dbo.ApplicationStatusType.CodeName = N'SUC') AND (dbo.Application.VacancyId = dbo.Vacancy.VacancyId))) = 0) 
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
							dbo.Vacancy.ApplicationClosingDate Asc
						) AS 'RowNumber',
						dbo.Vacancy.VacancyId,
						dbo.Vacancy.ApprenticeshipType AS VacancyType, 
						dbo.Vacancy.Title, 
						dbo.Vacancy.VacancyStatusId, 
						--dbo.Employer.FullName,
						dbo.[ProviderSite].FullName,
						dbo.Vacancy.NumberofPositions, 
						dbo.Vacancy.ApplicationClosingDate, 
						dbo.ApprenticeshipFramework.ApprenticeshipFrameworkId, 
						dbo.ApprenticeshipFramework.ApprenticeshipOccupationId,
						dbo.ApprenticeshipFramework.Fullname as 'FrameworkName',
						(SELECT COUNT(*) FROM dbo.Application WHERE dbo.Application.VacancyId = dbo.Vacancy.VacancyId) AS NumberOfApplications,
						(SELECT COUNT(*) FROM dbo.Application 
								INNER JOIN dbo.ApplicationStatusType ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId
							WHERE (dbo.ApplicationStatusType.CodeName = N'SUC') AND (dbo.Application.VacancyId = dbo.Vacancy.VacancyId)) AS SuccessfulApplications
					FROM dbo.Vacancy 
						INNER JOIN dbo.ApprenticeshipFramework ON dbo.Vacancy.ApprenticeshipFrameworkId = dbo.ApprenticeshipFramework.ApprenticeshipFrameworkId 
						INNER JOIN dbo.[VacancyOwnerRelationship] ON dbo.Vacancy.[VacancyOwnerRelationshipId] = dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] 
						INNER JOIN dbo.Employer ON dbo.[VacancyOwnerRelationship].EmployerId = dbo.Employer.EmployerId
						INNER JOIN dbo.[ProviderSite] ON dbo.[VacancyOwnerRelationship].[ProviderSiteID] = dbo.[ProviderSite].ProviderSiteID
					WHERE dbo.Vacancy.VacancyId IN
						(SELECT dbo.Vacancy.VacancyId
							FROM dbo.Vacancy INNER JOIN
								dbo.VacancyStatusType ON dbo.Vacancy.VacancyStatusId = dbo.VacancyStatusType.VacancyStatusTypeId INNER JOIN
								dbo.[VacancyOwnerRelationship] ON 
								dbo.Vacancy.[VacancyOwnerRelationshipId] = dbo.[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] INNER JOIN
								dbo.[ProviderSite] ON dbo.[VacancyOwnerRelationship].[ProviderSiteID] = dbo.[ProviderSite].ProviderSiteID
							WHERE 
								(dbo.Employer.EmployerId = @EmployerId) 
								AND 
								(dbo.Vacancy.VacancyStatusId <>
									(SELECT VacancyStatusTypeId
										FROM dbo.VacancyStatusType AS VacancyStatusType_1
										WHERE (CodeName = N'Com')))
								AND
								(Vacancy.NumberofPositions - 
									(SELECT COUNT(*) AS TotalCount
										FROM dbo.Application INNER JOIN
											dbo.ApplicationStatusType ON dbo.Application.ApplicationStatusTypeId = dbo.ApplicationStatusType.ApplicationStatusTypeId
												WHERE (dbo.ApplicationStatusType.CodeName = N'SUC') AND (dbo.Application.VacancyId = dbo.Vacancy.VacancyId))) = 0)
				) X
				WHERE (RowNumber > @FirstRecord) AND (RowNumber <= @LastRecord)
		/*---------> End */

	END

END