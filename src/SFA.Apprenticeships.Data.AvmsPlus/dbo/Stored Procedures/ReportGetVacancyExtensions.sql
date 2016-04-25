CREATE PROCEDURE [dbo].[ReportGetVacancyExtensions]
	@startReportDateTime as datetime,
	@endReportDateTime as datetime,
	@providerToStudy as int,
	@vacancyStatus as int
AS
	BEGIN

	declare @VacanciesWithExtensions table (VacancyId int,VacancyOwnerRelationshipId int, ApplicationClosingDate datetime null, ContractOwnerId int, VacancyStatus int)
	declare @TempResults table (VacancyId int, VacancyStatus int, VacancyOwnerRelationshipId int, EmployerId int, EmployerName nvarchar(255), ProviderSiteId int,
		ContractOwnerId int, OriginalPostingDate datetime, CurrentClosingDate datetime, NumberOfExtensions int)
	declare @ProvidersToStudy table (ProviderId int)

	IF (@providerToStudy is null ) BEGIN
		INSERT INTO @ProvidersToStudy(ProviderId)
		SELECT ProviderId FROM Provider
	END
	ELSE BEGIN
		INSERT INTO @ProvidersToStudy(ProviderId) SELECT @providerToStudy
	END

	INSERT INTO @VacanciesWithExtensions(VacancyId,VacancyOwnerRelationshipId, ApplicationClosingDate, ContractOwnerId, VacancyStatus)
		SELECT VacancyId,VacancyOwnerRelationshipId, ApplicationClosingDate, ContractOwnerID, VacancyStatusId
		FROM Vacancy
		WHERE VacancyId IN(
			SELECT
			[VacancyId]
			FROM[dbo].[VacancyHistory]
			WHERE VacancyHistoryEventSubTypeId=2
			GROUP BY VacancyId
			HAVING COUNT(*) > 1)
		AND VacancyStatusId = @vacancyStatus
	;
	WITH cteOriginalPostingDate (VacancyId, HistoryDate) AS (
		SELECT tmp.VacancyId, tmp.HistoryDate
		FROM (
			SELECT VacancyId, HistoryDate, ROW_NUMBER() OVER (PARTITION BY VacancyId ORDER BY HistoryDate) as RowNumber
			from VacancyHistory as vh
			where vh.VacancyHistoryEventTypeId = 1 and vh.VacancyHistoryEventSubTypeId = 2
			GROUP BY VacancyId, HistoryDate
		) tmp
		Where tmp.RowNumber = 1
	), cteNumberOfVacancyExtensions(VacancyId, NumberOfExtensions) AS (
		SELECT VacancyId, count(VacancyId) - 1
		from VacancyHistory as vh
		where vh.VacancyHistoryEventTypeId = 1 and vh.VacancyHistoryEventSubTypeId = 2
		GROUP BY VacancyId
	)

	INSERT INTO @TempResults (VacancyId, VacancyStatus, VacancyOwnerRelationshipId, EmployerId, EmployerName, ProviderSiteId, ContractOwnerId, OriginalPostingDate, 
		CurrentClosingDate, NumberOfExtensions)
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
			cteNumberOfVacancyExtensions.NumberOfExtensions as NumberOfExtensions
		FROM @VacanciesWithExtensions as vwe
		INNER JOIN VacancyOwnerRelationship as vor ON vwe.VacancyOwnerRelationshipId=vor.VacancyOwnerRelationshipId
		INNER JOIN Employer as e ON vor.EmployerId=e.EmployerId
		INNER JOIN ProviderSite as ps ON ps.ProviderSiteID=vor.ProviderSiteID
		INNER JOIN cteOriginalPostingDate ON cteOriginalPostingDate.VacancyId = vwe.VacancyId
		INNER JOIN cteNumberOfVacancyExtensions ON cteNumberOfVacancyExtensions.VacancyId = vwe.VacancyId
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

	SELECT tmp.*, p.FullName as ProviderName
		FROM @TempResults as tmp
		INNER JOIN cteProviderId ON cteProviderId.ProviderSiteID = tmp.ProviderSiteID
		INNER JOIN dbo.Provider as p ON cteProviderId.ProviderID = p.ProviderID
		WHERE tmp.ContractOwnerId is null AND 
			  p.ProviderID IN ( SELECT ProviderId FROM @ProvidersToStudy)
	UNION 
		SELECT tmp.*, p.FullName as ProviderName FROM @TempResults as tmp
		INNER JOIN dbo.Provider as p ON tmp.ContractOwnerId = p.ProviderID
		WHERE tmp.ContractOwnerId is not null AND 
			  p.ProviderID IN ( SELECT ProviderId FROM @ProvidersToStudy)

	END
