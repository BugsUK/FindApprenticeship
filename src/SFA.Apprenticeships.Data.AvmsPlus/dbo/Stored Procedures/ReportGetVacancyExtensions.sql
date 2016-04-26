CREATE PROCEDURE [dbo].[ReportGetVacancyExtensions]
	@startReportDateTime as datetime,
	@endReportDateTime as datetime,
	@providerToStudyUkprn as int,
	@vacancyStatusToStudy as int
AS
	BEGIN

	declare @VacanciesWithExtensions table (VacancyId int,VacancyOwnerRelationshipId int, ApplicationClosingDate datetime null, ContractOwnerId int, VacancyStatus int, VacancyReferenceNumber int, VacancyTitle nvarchar(255))
	declare @TempResults table (VacancyId int, VacancyStatus int, VacancyOwnerRelationshipId int, EmployerId int, EmployerName nvarchar(255), ProviderSiteId int,
		ContractOwnerId int, OriginalPostingDate datetime, CurrentClosingDate datetime, NumberOfExtensions int, NumberOfApplications int, VacancyReferenceNumber int, VacancyTitle nvarchar(255))
	declare @ProvidersToStudy table (ProviderId int)
	declare @VacancyStatusesToStudy table (VacancyStatusId int)

	IF (@providerToStudyUkprn is null ) BEGIN
		INSERT INTO @ProvidersToStudy(ProviderId)
		SELECT ProviderId FROM Provider
	END
	ELSE BEGIN
		INSERT INTO @ProvidersToStudy(ProviderId) SELECT ProviderId FROM Provider WHERE UKPRN = @providerToStudyUkprn
	END

	IF (@vacancyStatusToStudy is null ) BEGIN
		INSERT INTO @VacancyStatusesToStudy(VacancyStatusId)
		SELECT VacancyStatusTypeId FROM VacancyStatusType
	END
	ELSE BEGIN
		INSERT INTO @VacancyStatusesToStudy(VacancyStatusId) SELECT @vacancyStatusToStudy
	END

	INSERT INTO @VacanciesWithExtensions(VacancyId,VacancyOwnerRelationshipId, ApplicationClosingDate, ContractOwnerId, VacancyStatus, VacancyReferenceNumber, VacancyTitle)
		SELECT VacancyId,VacancyOwnerRelationshipId, ApplicationClosingDate, ContractOwnerID, VacancyStatusId, VacancyReferenceNumber, Title
		FROM Vacancy
		WHERE VacancyId IN(
			SELECT
			[VacancyId]
			FROM[dbo].[VacancyHistory]
			WHERE VacancyHistoryEventSubTypeId=2
			GROUP BY VacancyId
			HAVING COUNT(*) > 1)
		AND VacancyStatusId IN (SELECT VacancyStatusId FROM @VacancyStatusesToStudy)
	;
	WITH cteOriginalPostingDate (VacancyId, HistoryDate) AS (
		SELECT tmp.VacancyId, tmp.HistoryDate
		FROM (
			SELECT VacancyId, HistoryDate, ROW_NUMBER() OVER (PARTITION BY VacancyId ORDER BY HistoryDate) as RowNumber
			FROM VacancyHistory as vh
			WHERE vh.VacancyHistoryEventTypeId = 1 and vh.VacancyHistoryEventSubTypeId = 2
			GROUP BY VacancyId, HistoryDate
		) tmp
		Where tmp.RowNumber = 1
	), cteNumberOfVacancyExtensions(VacancyId, NumberOfExtensions) AS (
		SELECT VacancyId, count(VacancyId) - 1
		FROM VacancyHistory as vh
		WHERE vh.VacancyHistoryEventTypeId = 1 and vh.VacancyHistoryEventSubTypeId = 2
		GROUP BY VacancyId
	), cteNumberOfApplications(VacancyId, NumberOfApplications) AS (
		SELECT VacancyId, count(VacancyId)
		FROM Application as app
		WHERE app.ApplicationStatusTypeId IN (2,3,5,6) -- Sent, In Progress, Successful, UnSuccessful
		GROUP BY VacancyId
	)

	INSERT INTO @TempResults (VacancyId, VacancyStatus, VacancyOwnerRelationshipId, EmployerId, EmployerName, ProviderSiteId, ContractOwnerId, OriginalPostingDate, 
		CurrentClosingDate, NumberOfExtensions, NumberOfApplications, VacancyReferenceNumber, VacancyTitle)
		SELECT 
			vwe.VacancyId,
			vwe.VacancyStatus,
			vor.VacancyOwnerRelationshipId,
			e.EmployerId,
			e.FullName as EmployerName,
			ps.ProviderSiteID, 
			vwe.ContractOwnerId,
			cteOriginalPostingDate.HistoryDate as OriginalPostingDate,
			vwe.ApplicationClosingDate as CurrentClosingDate,
			cteNumberOfVacancyExtensions.NumberOfExtensions as NumberOfExtensions,
			cteNumberOfApplications.NumberOfApplications as NumberOfApplications,
			vwe.VacancyReferenceNumber,
			vwe.VacancyTitle
		FROM @VacanciesWithExtensions as vwe
		INNER JOIN VacancyOwnerRelationship as vor ON vwe.VacancyOwnerRelationshipId=vor.VacancyOwnerRelationshipId
		INNER JOIN Employer as e ON vor.EmployerId=e.EmployerId
		INNER JOIN ProviderSite as ps ON ps.ProviderSiteID=vor.ProviderSiteID
		INNER JOIN cteOriginalPostingDate ON cteOriginalPostingDate.VacancyId = vwe.VacancyId
		INNER JOIN cteNumberOfVacancyExtensions ON cteNumberOfVacancyExtensions.VacancyId = vwe.VacancyId
		LEFT JOIN cteNumberOfApplications ON cteNumberOfApplications.VacancyId = vwe.VacancyId
		WHERE cteOriginalPostingDate.HistoryDate >= @startReportDateTime AND 
			  cteOriginalPostingDate.HistoryDate <= @endReportDateTime 
		ORDER BY VacancyId

	;WITH cteProviderId(ProviderSiteID, ProviderId) AS (
		SELECT tmp.ProviderSiteID, tmp.ProviderID 
		FROM ( 
			SELECT psr.ProviderSiteID, p.ProviderID, ROW_NUMBER() OVER(PARTITION BY psr.ProviderSiteID ORDER BY p.ProviderId DESC) AS RowNumber
			FROM dbo.ProviderSiteRelationship AS psr 
			INNER JOIN Provider AS p ON p.ProviderID = psr.ProviderID
			GROUP BY p.ProviderID, psr.ProviderSiteID
		) tmp
		WHERE RowNumber = 1
	)

	SELECT VacancyId, CONCAT('VAC', FORMAT(VacancyReferenceNumber, '000000000')), VacancyTitle, VacancyStatus, VacancyOwnerRelationshipId, EmployerId, EmployerName, tmp.ProviderSiteId, ContractOwnerId, OriginalPostingDate, 
		CurrentClosingDate, NumberOfExtensions, ISNULL(NumberOfApplications, 0) as NumberOfApplications, p.FullName as ProviderName
		FROM @TempResults as tmp
		INNER JOIN cteProviderId ON cteProviderId.ProviderSiteID = tmp.ProviderSiteID
		INNER JOIN dbo.Provider as p ON cteProviderId.ProviderID = p.ProviderID
		WHERE tmp.ContractOwnerId is null AND 
			  p.ProviderID IN ( SELECT ProviderId FROM @ProvidersToStudy)
	UNION 
		SELECT VacancyId, CONCAT('VAC', FORMAT(VacancyReferenceNumber, '000000000')), VacancyTitle, VacancyStatus, VacancyOwnerRelationshipId, EmployerId, EmployerName, tmp.ProviderSiteId, ContractOwnerId, OriginalPostingDate, 
		CurrentClosingDate, NumberOfExtensions, ISNULL(NumberOfApplications, 0) as NumberOfApplications, p.FullName as ProviderName FROM @TempResults as tmp
		INNER JOIN dbo.Provider as p ON tmp.ContractOwnerId = p.ProviderID
		WHERE tmp.ContractOwnerId is not null AND 
			  p.ProviderID IN ( SELECT ProviderId FROM @ProvidersToStudy)

	END

