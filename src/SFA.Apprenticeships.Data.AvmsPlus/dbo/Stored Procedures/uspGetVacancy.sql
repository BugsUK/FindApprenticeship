CREATE PROCEDURE [dbo].[uspGetVacancy] 
	@SearchType VARCHAR (30), 
	@SearchTerm VARCHAR (200)=null, 
	@ApprenticeFrameworks VARCHAR (4000)=null, 
	@LocationId INT=null, 
	@VacancyPostedSince DATETIME=null, 
	@Distancefrom INT=null, 
	@Easting INT=null, 
	@Northing INT=null, 
	@WeeklyWagesFrom INT=null, 
	@WeeklyWagesTo INT=null,
	@ApprenticeshipTypeValue int=null, 
	@PageNo INT=1, 
	@PageSize INT=10, 
	@SortByField NVARCHAR (100)='Rank', 
	@SortOrder BIT=1
AS
BEGIN
/* Optimize */
	SET NOCOUNT ON
	--Converting the date to an int allows the query to run on multiple processors
	DECLARE @CurrentDate INT 
	SET @CurrentDate = FLOOR(CAST(GETDATE() AS FLOAT))

		-- KJB-FIX-08/05/2012 This is to fix the fact the locationID could be either a 
	-- CountyID or an alias LocalAuthorityGroupID.
	-- I have added @LocationType needs to be passed into the sp in later releases
	DECLARE @LocationType INT=null 	
	if @locationId in (SELECT CountyID FROM County WHERE CountyID = @locationId)
		SET @LocationType = 1 -- LocationId is a county ID (if LocationId is both County ID and 
							  -- Local Authority ID we use County ID
	ELSE
		BEGIN
		IF @locationId IN (SELECT LocalAuthorityGroupID	   
		FROM dbo.LocalAuthorityGroup LAG
		INNER JOIN dbo.LocalAuthorityGroupType AS LAGT 
		ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
		AND   LAGT.LocalAuthorityGroupTypeName = N'Alias'
		AND LAG.LocalAuthorityGroupID = @LocationID)
				SET @LocationType = 2 -- LocationId is a Local Authority Group ID
		END

DECLARE @results TABLE
(
		RowNumber int,
		VacancyId			INT,      
		Title				NVARCHAR(100),      
		Employer			NVARCHAR(256),      
		Town				NVARCHAR(40), 
		ShortDescription	NVARCHAR(256),
		ApprenticeShipType	INT,
		Framework			NVARCHAR(200),	
		ClosingDate			DATETIME,      
		VacancyPostedDate	DATETIME, 
		NumberofPositions	INT,
		VacancyLocationTypeId INT,
		ContractOwnerID  INT,
		ContractOwnerTradingName NVARCHAR(255),
		VacancyOwnerID INT,
		VacancyOwnerTradingName NVARCHAR(255),
		VacancyManagerID INT,
		VacancyManagerTradingName NVARCHAR(255),
		DeliveryOrganisationID INT,
		DeliveryOrganisationTradingName NVARCHAR(255),
		DeliveryOrganisationOwnerOrgID INT,
		VacancyOwnerOwnerOrgID INT,
		TotalNumberofPositions INT,
		NationalTypePositions INT,
		NationalPositions	INT,
		NationalVacancies	INT,
		TotalRows			INT,
		DeliveryOrganisationStatusType INT,
		VacancyManagerAnonymous BIT,
		IsNasProvider BIT)	

	;WITH VacanciesBySearchType AS
	(
		SELECT 
			RANK()OVER(ORDER BY Rank DESC) as Rank, --Invert the rank
			vs.VacancySearchId 
		FROM [dbo].[fnx_ConditionalFreeText](@SearchType, @SearchTerm) ft 
			INNER JOIN VacancySearch vs
				ON vs.VacancySearchId = ft.[Key]
		WHERE 
			@SearchType IN ('Employer', 'TrainingProvider', 'Keyword')
			AND
				[ApplicationClosingDateAsInt] >= @currentdate
			AND   
			(
				@VacancyPostedSince IS NULL 
				OR
				[VacancyPostedDate] >= @VacancyPostedSince
			)
			AND    
			(
				(
					(@WeeklyWagesFrom IS NOT NULL AND @WeeklyWagesTo IS NOT NULL)
					AND
					(([WeeklyWage] between @WeeklyWagesFrom AND @WeeklyWagesTo) OR [WageType] = 0)
				)
				OR
				(@WeeklyWagesFrom IS NULL OR @WeeklyWagesTo IS NULL)
			)
			AND   -- apprenticeship type NB. 999 means any apprenticeship
			(
				@ApprenticeshipTypeValue IS NULL 
				OR @ApprenticeshipTypeValue = 0
				OR (@ApprenticeshipTypeValue = 999 AND ApprenticeshipType in (1,2,3))
				OR ([ApprenticeshipType] = @ApprenticeshipTypeValue)
			)	
		UNION ALL
		SELECT 
			0 AS Rank,
			vs.VacancySearchId
		FROM VacancySearch vs 	
		INNER JOIN dbo.fnx_ConvertToArrayTable(@apprenticeFrameworks, '|') af
			ON vs.ApprenticeshipFrameworkId = af.[Value]
		WHERE
				[ApplicationClosingDateAsInt] >= @currentdate 
			AND    
			(
				(
					(@WeeklyWagesFrom IS NOT NULL AND @WeeklyWagesTo IS NOT NULL)
					AND
					(([WeeklyWage] between @WeeklyWagesFrom AND @WeeklyWagesTo) OR [WageType] = 0)
				)
				OR
				(@WeeklyWagesFrom IS NULL OR @WeeklyWagesTo IS NULL)
			)
			AND   
			(
				@VacancyPostedSince IS NULL 
				OR
				[VacancyPostedDate] >= @VacancyPostedSince
			)
			AND   -- apprenticeship type NB. 999 means any apprenticeship
			(
				@ApprenticeshipTypeValue IS NULL 
				OR @ApprenticeshipTypeValue = 0
				OR (@ApprenticeshipTypeValue = 999 AND ApprenticeshipType in (1,2,3))
				OR ([ApprenticeshipType] = @ApprenticeshipTypeValue)
			)	
		UNION ALL
		SELECT 
			0 AS Rank,
			vs.VacancySearchId
		FROM 
			VacancySearch vs 
		WHERE
				@ApprenticeFrameworks is null and @SearchType = 'Occupation'	
			AND
				[ApplicationClosingDateAsInt] >= @currentdate 
			AND    
			(
				(
					(@WeeklyWagesFrom IS NOT NULL AND @WeeklyWagesTo IS NOT NULL)
					AND
					(([WeeklyWage] between @WeeklyWagesFrom AND @WeeklyWagesTo) OR [WageType] = 0)
				)
				OR
				(@WeeklyWagesFrom IS NULL OR @WeeklyWagesTo IS NULL)
				OR
				([WageType] = 0)
			)
			AND   
			(
				@VacancyPostedSince IS NULL 
				OR
				[VacancyPostedDate] >= @VacancyPostedSince
			)
			AND   -- apprenticeship type NB. 999 means any apprenticeship
			(
				@ApprenticeshipTypeValue IS NULL 
				OR @ApprenticeshipTypeValue = 0
				OR (@ApprenticeshipTypeValue = 999 AND ApprenticeshipType in (1,2,3))
				OR ([ApprenticeshipType] = @ApprenticeshipTypeValue)
			)	
		UNION ALL
		SELECT 
			0 AS Rank,
			vs.VacancySearchId
		FROM 
			VacancySearch vs 
		WHERE
			@SearchType = 'ApprenticeshipType'	
			AND
				[ApplicationClosingDateAsInt] >= @currentdate 
			AND    
			(
				(
					(@WeeklyWagesFrom IS NOT NULL AND @WeeklyWagesTo IS NOT NULL)
					AND
					(([WeeklyWage] between @WeeklyWagesFrom AND @WeeklyWagesTo) OR [WageType] = 0)
				)
				OR
				(@WeeklyWagesFrom IS NULL OR @WeeklyWagesTo IS NULL)
				OR
				([WageType] = 0)
			)
			AND   
			(
				@VacancyPostedSince IS NULL 
				OR
				[VacancyPostedDate] >= @VacancyPostedSince
			)
			AND   -- apprenticeship type NB. 999 means any apprenticeship
			(
				@ApprenticeshipTypeValue IS NULL 
				OR @ApprenticeshipTypeValue = 0
				OR (@ApprenticeshipTypeValue = 999 AND ApprenticeshipType in (1,2,3))
				OR ([ApprenticeshipType] = @ApprenticeshipTypeValue)
			)	
		UNION ALL
		SELECT
			0 AS Rank,
			vs.VacancySearchId
		FROM 
			VacancySearch vs 
		WHERE 
			EmployerName = @SearchTerm
			AND
			@SearchType = 'EmployerId'
			AND
			[ApplicationClosingDateAsInt] >= @currentdate	
		UNION ALL
		SELECT
			0 AS Rank,
			vs.VacancySearchId
		FROM 
			VacancySearch vs 
		WHERE 
			([VacancyOwnerName] = @SearchTerm OR [DeliveryOrganisationName] = @SearchTerm)
			AND
			@SearchType = 'TrainingProviderId'
			AND
			[ApplicationClosingDateAsInt] >= @currentdate	

	), MatchedVacancies AS
	(
		SELECT 
			CASE @SortByField
				WHEN 'Location' THEN rank() over(order by vs.[Town])
				WHEN 'JobRole' THEN rank() over(order by [ApprenticeshipFrameworkName])
				ELSE rank() over(order by vs.[ApplicationClosingDate])
			END AS SORTFIELD,
			vst.Rank, 
			vs.[VacancySearchId],
			v.[NumberofPositions],
			CASE vs.[NationalVacancy]
				WHEN 1 THEN v.[NumberofPositions]
				ELSE 0
			END AS NumberofNationalTypePositions,
			v.[VacancyLocationTypeId]
			FROM VacancySearch vs 
				INNER JOIN VacanciesBySearchType vst 
					ON vs.VacancySearchId = vst.VacancySearchId	
				INNER JOIN vacancy v
					ON vs.VacancyId = v.VacancyId	
				inner join [VacancyOwnerRelationship] VPR 
					on VPR.[VacancyOwnerRelationshipId] = v.[VacancyOwnerRelationshipId] 
				inner join dbo.[ProviderSite] tp on tp.ProviderSiteID = VPR.[ProviderSiteID] 
					AND tp.TrainingProviderStatusTypeId != 3
				INNER JOIN PROVIDER CO ON v.contractownerid = CO.ProviderID
					AND co.ProviderStatusTypeID != 3 
	)
	, SortableVacancies AS
	(
		SELECT 
			--Sorting is done by Rank DESC and then by the default sortfield
			--unless a sort order is requested in which case it is done 
			--by the requested field in the required direction and then by rank desc
			CASE --Descending is only ever sorted by sortfield
				WHEN @SortOrder = 0 THEN SORTFIELD
				ELSE NULL 
			END AS SORT_DESC,
			CASE	-- When sorting by rank use the rank if sorting 
					-- by rank otherwise use the sortfield
				WHEN @SortOrder = 1 AND @SortByField <> 'Rank' THEN SORTFIELD
				WHEN @SortOrder = 1 AND @SortByField = 'Rank' THEN Rank
				ELSE NULL 
			END AS SORT_ASC,
			CASE --If Sorting by Rank tiebreak on the sortfield, 
				 --otherwise tiebreak on the rank
				WHEN @SortOrder = 1 AND @SortByField <> 'Rank' THEN RANK
				ELSE SORTFIELD 
			END AS TIEBREAKER,
			VacancySearchId,
			NumberofPositions,
			NumberofNationalTypePositions,
			VacancyLocationTypeId,
			SUM(NumberofPositions) over() AS NationalPositions ,
			count(VacancySearchId) over() AS   NationalVacancies 
		FROM 
			MatchedVacancies

	)
	, PagedVacancies AS
	(
		SELECT Row_Number() OVER(
				ORDER BY 
				SORT_ASC ASC,
				SORT_DESC DESC,
				TIEBREAKER ASC
			) AS RowNumber, 
			sv.VacancySearchId, 
			NumberofPositions,
			VacancyLocationTypeId,
			NationalPositions ,
			NationalVacancies ,
			SUM(NumberofPositions) over() AS TotalNumberofPositions ,
			SUM(NumberofNationalTypePositions) over() AS NationalTypePositions,
			COUNT(*) OVER() AS TotalRows 
		FROM 
			SortableVacancies sv inner join vacancysearch vs on sv.VacancySearchId = vs.VacancySearchId
		WHERE
			(
				@LocationId IS NULL 
				OR
				(@LocationType = 1) AND vs.[CountyId] = @LocationId
				-- THIS ONLY WORKS BECAUSE THE COUNTY IDS DON'T OVERLAP WITH THE ALIASES 
				-- see Product Backlog Item 5579		
				OR (@LocationType = 2) AND VS.LocalAuthorityID IN (SELECT LocalAuthorityID FROM LocalAuthorityGroupMembership WHERE LocalAuthorityGroupID = @locationID)		
				OR
				vs.NationalVacancy = 1
			)
			AND
			(
				@DistanceFrom IS NULL
				OR
				(
					vs.GeocodeEasting BETWEEN (@Easting - @DistanceFrom) AND (@Easting + @DistanceFrom)
					AND
					vs.GeocodeNorthing BETWEEN (@Northing - @DistanceFrom) AND (@Northing + @DistanceFrom)
				)
				OR
				vs.NationalVacancy = 1
			)
		union all
		select top 1 null, null, 0, 0, NationalPositions,
			NationalVacancies, 0, 0, 0
		FROM 
			SortableVacancies sv 
	)

	
	/* Execute */
	INSERT INTO @results
	(
	RowNumber,
	VacancyId			,
	Title				,
	Employer			,
	Town				,
	ShortDescription	,
	ApprenticeShipType	,
	Framework			,
	ClosingDate			,
	VacancyPostedDate	,
	NumberofPositions	,
	VacancyLocationTypeId,
	ContractOwnerID ,
	ContractOwnerTradingName,
	VacancyOwnerID,
	VacancyOwnerTradingName,
	VacancyManagerID,
	VacancyManagerTradingName,
	DeliveryOrganisationID,
	DeliveryOrganisationTradingName,
	DeliveryOrganisationOwnerOrgID,
	VacancyOwnerOwnerOrgID,
	TotalNumberofPositions,
	NationalTypePositions,
	NationalPositions	,
	NationalVacancies	,
	TotalRows,
    DeliveryOrganisationStatusType,
	VacancyManagerAnonymous,
	IsNasProvider	
	)
	SELECT 
		RowNumber,
		vs.[VacancyId]					AS 'VacancyId',      
		vs.[Title]						AS 'Title',      
		vs.[EmployerName]				AS 'Employer',      
		vs.[Town]						AS 'Town', 
		vs.[ShortDescription]			AS 'ShortDescription',      
		vs.[ApprenticeshipType]			AS 'ApprenticeShipType',
		ApprenticeshipFrameworkName		AS 'Framework',	
		vs.[ApplicationClosingDate]		AS 'ClosingDate',      
		vs.[VacancyPostedDate]			AS 'VacancyPostedDate', 
		pv.[NumberofPositions]			AS 'NumberofPositions',
		pv.[VacancyLocationTypeId]      AS 'VacancyLocationTypeId',
		Co.ProviderID AS ContractOwnerID,
		CO.TradingName AS ContractOwnerTradingName,
		VO.ProviderSiteID AS VacancyOwnerID,
		VO.TradingName AS VacancyOwnerTradingName,
		VM.ProviderSiteID AS VacancyManagerID,
		VM.TradingName AS VacancyManagerTradingName,
		DO.ProviderSiteID AS DeliveryOrganisationID,
		DO.TradingName AS DeliveryOrganisationTradingName,	
		DOP.ProviderId AS DeliveryOrganisationOwnerOrgID,	
		VOPSR.ProviderId AS VacancyOwnerOwnerOrgID,
		TotalNumberofPositions			AS 'TotalNumberofPositions',
		NationalTypePositions           AS 'NationalTypePositions',
		NationalPositions ,
		NationalVacancies ,
		TotalRows,
		DO.TrainingProviderStatusTypeId AS 'DeliveryOrganisationStatusType',
		v.VacancyManagerAnonymous,
		DOP.IsNasProvider
	FROM 
		PagedVacancies pv 
		INNER JOIN vacancysearch vs ON pv.VacancySearchId = vs.vacancySearchId
		INNER JOIN Vacancy V ON vs.VacancyId = V.VacancyId
		INNER JOIN dbo.Provider CO ON V.ContractOwnerID = CO.ProviderID
		INNER JOIN dbo.VacancyOwnerRelationship VOR ON V.VacancyOwnerRelationshipId = VOR.VacancyOwnerRelationshipId
		INNER JOIN dbo.ProviderSite VO ON VOR.ProviderSiteID = VO.ProviderSiteID
		INNER JOIN dbo.ProviderSiteRelationship VOPSR ON VOPSR.ProviderSiteID = VO.ProviderSiteID AND VOPSR.ProviderSiteRelationshipTypeId = 1
		LEFT JOIN ProviderSite VM ON V.VacancyManagerID = VM.ProviderSiteID
		LEFT JOIN ProviderSIte DO ON V.DeliveryOrganisationID = DO.ProviderSiteID
		LEFT JOIN ProviderSiteRelationship DOR ON DOR.ProviderSiteId = V.DeliveryOrganisationId AND DOR.ProviderSiteRelationshipTypeId = 1
		LEFT JOIN Provider DOP ON DOP.ProviderId = DOR.ProviderID
	WHERE 
		RowNumber BETWEEN 
			((@PageNo -1) * @PageSize) + 1 
			AND 
			((@PageNo -1) * @PageSize) + @PageSize 
	
--Resultset 1	
	SELECT TOP 1 
		TotalNumberofPositions,
		NationalTypePositions,
		NationalPositions	,
		NationalVacancies	,
		TotalRows
	FROM @results
--Resultset 2
	SELECT 
		VacancyId						,
		Title							,
		Employer						,
		Town							,
		ShortDescription				,
		RowNumber						,
		ApprenticeShipType				,
		Framework						,
		ClosingDate						,
		VacancyPostedDate				,
		NumberofPositions				,
		VacancyLocationTypeId			,
		ContractOwnerID					,
		ContractOwnerTradingName		,
		VacancyOwnerID					,
		VacancyOwnerTradingName			,
		VacancyManagerID				,
		VacancyManagerTradingName		,
		DeliveryOrganisationID			,
		DeliveryOrganisationTradingName ,
		DeliveryOrganisationStatusType	,
		VacancyManagerAnonymous			,
		IsNasProvider,
		DeliveryOrganisationOwnerOrgID,
		VacancyOwnerOwnerOrgID
	FROM @results where vacancyid is not null 
	ORDER BY RowNumber    -- ITSM9577648 - GJG - To correct ordering on a page
	/* Reset */
	SET NOCOUNT OFF
END