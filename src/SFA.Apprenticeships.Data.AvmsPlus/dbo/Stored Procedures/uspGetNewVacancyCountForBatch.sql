CREATE PROCEDURE [dbo].[uspGetNewVacancyCountForBatch] 
	--@DateFrom DateTime,
	@SearchType varchar(30),
	@SearchTerm varchar(200) = null,
	@ApprenticeFramework int = null,	
	@LocationId int = null,
	@VacancyPostedSince datetime =  null,
	@VacancyReferenceNumber int = null,
	@Distancefrom int = null,
	@Easting int = null,
	@Northing int = null,
	@WeeklyWagesFrom int = null,
	@WeeklyWagesTo int = null,
	@ApprenticeshipTypeValue int = null 
--	@PageNo int = 1,
--	@PageSize int = 10,
--	@SortByField nvarchar(100)='ClosingDate',
--	@SortOrder bit = 1,
	--@NewVacancyCount Int = 0 Output 
AS
/*BEGIN
	SET NOCOUNT ON
--	SELECT @NewVacancyCount = count([vacancy].[vacancyid]) 
--		FROM [dbo].[vacancy] with (nolock) 


		
	SET NOCOUNT OFF
END

*/

----

BEGIN
	/* Optimize */
	SET NOCOUNT ON


	-- KJB-FIX-08/05/2012 This is to fix the fact the locationID could be either a 
	-- CountyID or an alias LocalAuthorityGroupID.
	-- I have added @LocationType needs to be passed into the sp in later releases
	DECLARE @LocationType INT=null 	
	if @locationId in (SELECT CountyId FROM County WHERE CountyId = @locationId)
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
	

	/* Initialise */
	IF @SearchType = N'Keyword' AND @SearchTerm = null
		SET @SearchTerm = ''


	/* Build Queries */
	;WITH ValidVacancies AS
	(
		SELECT *
			FROM VacancySearch vs
			WHERE 
			(
				(
					@SearchType = 'VacancyReferenceNumber' 
					AND
					VacancyReferenceNumber = @VacancyReferenceNumber
				)
				OR
				(
					@SearchType = 'Occupation' 
					AND
					(
						vs.ApprenticeshipFrameworkId = @apprenticeFramework
						OR 
						@apprenticeFramework IS NULL
					)
				)
				OR 
				(
					NOT @SearchType = 'Occupation'
					AND 
					NOT @SearchType = 'VacancyReferenceNumber'
				)
			)
			AND
			(				
				@LocationId IS NULL OR  @LocationId=0
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
				[WeeklyWage] >= ISNULL(@WeeklyWagesFrom,0) 
				AND 
				[WeeklyWage] <= ISNULL(@WeeklyWagesTo,100000)
			)
			AND   
			(
				@VacancyPostedSince IS NULL 
				OR
				[VacancyPostedDate] >= @VacancyPostedSince
			)
			AND
			(
				@DistanceFrom IS NULL
				OR
				(
					GeocodeEasting BETWEEN (@Easting - @DistanceFrom) AND (@Easting + @DistanceFrom)
					AND
					GeocodeNorthing BETWEEN (@Northing - @DistanceFrom) AND (@Northing + @DistanceFrom)
				)
			)
			AND   --Added new parameter    
			(    
				(@ApprenticeshipTypeValue IS NULL or @ApprenticeshipTypeValue = 0)    
					OR    
				[ApprenticeshipType] = @ApprenticeshipTypeValue    
			)   		
	)
	, OrderedVacancies AS 
	(
		SELECT Rank, vs.* 
			FROM ValidVacancies vs
				INNER JOIN [dbo].[fnx_ConditionalFreeText](@SearchType, @SearchTerm) ft 
					ON vs.VacancySearchId = ft.[Key]
			WHERE 
				@SearchType IN ('Employer', 'TrainingProvider', 'Keyword')

		UNION ALL

		SELECT 0 AS Rank, vs.* 
			FROM ValidVacancies vs
			WHERE 
				@SearchType in ('VacancyReferenceNumber', 'Occupation')
	)
	, PagedVacancies AS
	(
		SELECT Row_Number() OVER(
				ORDER BY Rank DESC, 

				CASE WHEN 'ClosingDate'='ClosingDate' THEN [ApplicationClosingDate]  END ASC

			) AS RowNumber, 
			[VacancyId] AS 'VacancyId',      
			[Title] AS 'Title',      
			[EmployerName] As 'Employer',      
			[Town] AS 'Town', 
			[ShortDescription] AS 'ShortDescription',      
			[ApprenticeshipType] As 'ApprenticeShipType',
			ApprenticeshipFrameworkName As 'Framework',	
			[ApplicationClosingDate] AS 'ClosingDate',      
			[VacancyPostedDate] AS 'VacancyPostedDate', 
			COUNT(*) OVER() AS TotalRows  
		FROM 
			OrderedVacancies
	) 

	/* Execute */

    select count(TotalRows) as VacancyCount  FROM  PagedVacancies 

	/* Reset */
	SET NOCOUNT OFF
END