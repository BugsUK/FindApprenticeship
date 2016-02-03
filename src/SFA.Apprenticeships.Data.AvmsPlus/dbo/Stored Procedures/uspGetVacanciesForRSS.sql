CREATE PROC [dbo].[uspGetVacanciesForRSS] 
@FeedType INT, 
@DateTimeRange INT,
@FrameworkCode varchar(3),
@OccupationCode varchar(3),
@CountyCode varchar(MAX),
@Town varchar(MAX),
@RegionCode varchar(MAX),
@VacancyReferenceNumber int,
@FeedTitle nvarchar(300) output,
@FeedDescription nvarchar(300) output,
@FeedImageUrl nvarchar(300) output,
@FeedCopyrightInfo nvarchar(300) output,
@AlternateLink	nvarchar(300) output

AS
   -- obtain the vacancy live status ID for use in the main query
    declare @liveVacancyStatusID int =	(
                                            select	VacancyStatusTypeId 
                                            from	VacancyStatusType 
                                            where	CodeName = 'Lve' 
                                        )

    -- obtain Apprenticeship Framework Id from parameter supplied			
    declare @frameworkId int = null
    IF @frameworkCode IS NOT NULL and @frameworkCode <> ''
        select	@frameworkId = ApprenticeshipFrameworkId
        from	ApprenticeshipFramework
        where	CodeName = @frameworkCode
	else
		select @frameworkId = -1

    -- obtain Occupation Id from parameter supplied			
    declare @occupationId int =	null
    IF @occupationCode IS NOT NULL and @occupationCode <> ''
        select	@occupationId = ApprenticeshipOccupationId
        from	ApprenticeshipOccupation
        where	Codename = @occupationCode
	else
		select @occupationId = -1

	-- towns
	declare @TownList TABLE (Name VARCHAR(40))
	INSERT INTO @TownList
	SELECT * FROM fnx_SplitListToTable(@Town)
	
	-- counties
	declare @CountyList TABLE (Id int)
	INSERT INTO @CountyList
	SELECT CountyId FROM fnx_SplitListToTable(@CountyCode) as Codes
		INNER JOIN County ON CodeName = Codes.ID
	
	-- regions
	declare @RegionList TABLE (Code VARCHAR(3))
	INSERT INTO @RegionList
	SELECT * FROM fnx_SplitListToTable(@RegionCode)
        
DECLARE @Today Datetime
SET @Today = GETDATE()

--Get values from system parameters table
SET @FeedTitle = (SELECT ParameterValue FROM dbo.SystemParameters WHERE ParameterName = 'VacancyRSSFeedTitle')
SET @FeedDescription = (SELECT ParameterValue FROM dbo.SystemParameters WHERE ParameterName = 'VacancyRSSFeedDescription')
SET @FeedImageUrl = (SELECT ParameterValue FROM dbo.SystemParameters WHERE ParameterName = 'VacancyRSSImageURL')
SET @FeedCopyrightInfo = (SELECT ParameterValue FROM dbo.SystemParameters WHERE ParameterName = 'VacancyRSSCopyrightInformation')
SET @AlternateLink = (SELECT ParameterValue FROM dbo.SystemParameters WHERE ParameterName = 'VacancyRSSAlternateLink')
DECLARE @DaysTilExpiry int = (SELECT ParameterValue FROM dbo.SystemParameters WHERE ParameterName = 'VacancyRSSDaysUntilVacancyExpiry')
DECLARE @LowApplicantAmount int = (SELECT ParameterValue FROM dbo.SystemParameters WHERE ParameterName = 'VacancyRSSLowApplicantAmount')

--Now perform search on database
IF (@FeedType = 1)
BEGIN
	DECLARE @MinDate Datetime
	SET @MinDate = GETDATE() - @DateTimeRange

	SELECT
		V.VacancyId AS VacancyId,
		V.Title as VacancyTitle,
		E.TradingName AS EmployerTradingName,
		V.ShortDescription,
		V.ApprenticeshipType AS VacancyType,
		V.Town AS VacancyLocation,
		F.FullName AS JobRole,
		ApplicationClosingDate AS ClosingDate,
		H.HistoryDate AS PublishDate,
		V.VacancyLocationTypeId,		
		V.VacancyReferenceNumber, 
		V.WeeklyWage

	  FROM dbo.Vacancy V
		  INNER JOIN dbo.[VacancyOwnerRelationship] VPR
		  on V.[VacancyOwnerRelationshipId] = VPR.[VacancyOwnerRelationshipId]
		  INNER JOIN dbo.Employer E ON VPR.EmployerId = e.EmployerId
		  INNER JOIN dbo.[ProviderSite] TP ON VPR.[ProviderSiteID] = TP.ProviderSiteID
		  LEFT OUTER JOIN dbo.LocalAuthority LA ON V.LocalAuthorityId = LA.LocalAuthorityId
		  --INNER JOIN dbo.LSCRegion R ON LA.LSCRegionId = R.LSCRegionId
		  left outer join dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
		  left outer join dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
		  left outer join dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID

		  INNER JOIN dbo.ApprenticeshipFramework F on V.ApprenticeshipFrameworkId = F.ApprenticeshipFrameworkId
		  INNER JOIN dbo.VacancyHistory H ON (V.VacancyId = H.VacancyId 
												AND H.VacancyHistoryEventSubTypeId = @liveVacancyStatusID
												-- ITSM3896337. Only fetch most recent matching history. RLD.
												AND H.HistoryDate =	(
													SELECT	MAX(vh1.HistoryDate)  
													FROM	VacancyHistory vh1
													WHERE	vh1.VacancyId = V.VacancyId 
															and	vh1.VacancyHistoryEventSubTypeId = @liveVacancyStatusID)
											)
		  INNER JOIN dbo.ApprenticeshipOccupation O on F.ApprenticeshipOccupationId = O.ApprenticeshipOccupationId
	  WHERE V.VacancyStatusId = @liveVacancyStatusID
		  AND LAGT.LocalAuthorityGroupTypeName = N'Region'
		  AND H.HistoryDate BETWEEN @MinDate AND @Today
		  AND (V.ApprenticeshipFrameworkId = @frameworkId OR @frameworkId IS NULL or @frameworkId = -1)
		  AND (V.VacancyReferenceNumber = @VacancyReferenceNumber OR @VacancyReferenceNumber IS NULL OR @VacancyReferenceNumber = -1)		  
		  AND (O.ApprenticeshipOccupationId = @occupationId OR @occupationId IS NULL OR @occupationId = -1)
		  AND ((V.VacancyLocationTypeId = 3) OR V.CountyId IN (SELECT Id FROM @CountyList) OR @countyCode IS NULL OR @countyCode = '')
		  AND ((V.VacancyLocationTypeId = 3) OR V.Town IN (SELECT Name FROM @TownList) or @Town IS NULL OR @Town = '')
		  AND ((V.VacancyLocationTypeId = 3) OR LAG.CodeName IN (SELECT Code FROM @RegionList) OR @RegionCode IS NULL OR @RegionCode = '')
		  AND (V.ApplicationClosingDate >= convert(date, GETDATE())) -- remove time part from the current date
		  
	   ORDER BY PublishDate DESC
  
END
ELSE
BEGIN
		SELECT 
			vacanies.* 
		FROM(
				SELECT 
					V.VacancyId AS VacancyId,
					V.Title as VacancyTitle,
					E.TradingName AS EmployerTradingName,
					V.ShortDescription,
					V.ApprenticeshipType AS VacancyType,
					V.Town AS VacancyLocation,
					F.FullName AS JobRole,
					ApplicationClosingDate AS ClosingDate,
					(SELECT COUNT(A.CandidateId)) AS ApplicantCount,
					CASE WHEN V.NoOfOfflineApplicants IS NULL THEN 0 
						ELSE V.NoOfOfflineApplicants END AS NoOfOfflineApplicants,
					H.HistoryDate AS PublishDate,
					V.VacancyLocationTypeId	AS VacancyLocationTypeId,
					V.VacancyReferenceNumber,
					V.WeeklyWage		
				FROM dbo.Vacancy V
					  INNER JOIN dbo.[VacancyOwnerRelationship] VPR
					  on V.[VacancyOwnerRelationshipId] = VPR.[VacancyOwnerRelationshipId]
					  INNER JOIN dbo.Employer E ON VPR.EmployerId = e.EmployerId
					  INNER JOIN dbo.[ProviderSite] TP ON VPR.[ProviderSiteID] = TP.ProviderSiteID
					  LEFT OUTER JOIN dbo.LocalAuthority LA ON V.LocalAuthorityId = LA.LocalAuthorityId

					  left outer join dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
					  left outer join dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
					  left outer join dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID

					  --INNER JOIN dbo.LSCRegion R ON LA.LSCRegionId = R.LSCRegionId
					  INNER JOIN dbo.ApprenticeshipFramework F on V.ApprenticeshipFrameworkId = F.ApprenticeshipFrameworkId			 
					  INNER JOIN dbo.ApprenticeshipOccupation O on F.ApprenticeshipOccupationId = O.ApprenticeshipOccupationId
					  LEFT JOIN dbo.Application A ON V.VacancyId = A.VacancyId
					  INNER JOIN dbo.VacancyHistory H ON (V.VacancyId = H.VacancyId 
												AND H.VacancyHistoryEventSubTypeId = @liveVacancyStatusID
												-- ITSM3896337. Only fetch most recent matching history. RLD.
													AND H.HistoryDate =	(
													SELECT	MAX(vh1.HistoryDate)  
													FROM	VacancyHistory vh1
													WHERE	vh1.VacancyId = V.VacancyId 
															and	vh1.VacancyHistoryEventSubTypeId = @liveVacancyStatusID)
												)
				WHERE V.VacancyStatusId = @liveVacancyStatusID
					AND LAGT.LocalAuthorityGroupTypeName = N'Region'
					AND (V.ApprenticeshipFrameworkId = @frameworkId OR @frameworkId IS NULL or @frameworkId = -1)
					AND (V.VacancyReferenceNumber = @VacancyReferenceNumber OR @VacancyReferenceNumber IS NULL OR @VacancyReferenceNumber = -1)		  
					AND (O.ApprenticeshipOccupationId = @occupationId OR @occupationId IS NULL OR @occupationId = -1)
					AND ((V.VacancyLocationTypeId = 3) OR V.CountyId IN (SELECT Id FROM @CountyList) OR @countyCode IS NULL OR @countyCode = '')
					AND ((V.VacancyLocationTypeId = 3) OR V.Town IN (SELECT Name FROM @TownList) or @Town IS NULL OR @Town = '')
					AND ((V.VacancyLocationTypeId = 3) OR LAG.CodeName IN (SELECT Code FROM @RegionList) OR @RegionCode IS NULL OR @RegionCode = '')
					AND DATEDIFF(dd,@Today, V.ApplicationClosingDate) BETWEEN 0 AND @DaysTilExpiry
					
				GROUP BY 
						V.Title,
						V.VacancyReferenceNumber,
						E.TradingName,
						TP.TradingName,
						V.ShortDescription,
						V.ApprenticeshipType,
						V.NumberofPositions,
						V.Town,
						F.FullName,
						ApplicationClosingDate,
						V.NoOfOfflineApplicants,
						V.VacancyId,
						H.HistoryDate,
						VacancyLocationTypeId,
						V.VacancyReferenceNumber,
						V.WeeklyWage
			) vacanies
			WHERE 
				NoOfOfflineApplicants + ApplicantCount <= @LowApplicantAmount
			ORDER BY PublishDate desc

END