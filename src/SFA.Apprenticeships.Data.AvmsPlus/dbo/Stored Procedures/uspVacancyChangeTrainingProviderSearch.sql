/****** Object:  StoredProcedure [dbo].[uspVacancyChangeTrainingProviderSearch]    Script Date: 02/18/2011 09:08:30 ******/
CREATE PROCEDURE [dbo].[uspVacancyChangeTrainingProviderSearch]
@trainingProviderName   AS NVARCHAR(255) = '',
@trainingProviderUKPRN  AS INT = -1,
@trainingProviderId     AS NVARCHAR(400) = '',
@employerName           AS NVARCHAR(255) = '',
@vacancyRef             AS INT = -1,
@apprenticeOccupationId AS INT = -1,
@apprenticeFrameworkId  AS INT = -1,
@regionId               AS INT = -1,
@requestedPageNo        AS INT = 1, 
@pageSize               AS INT = 10, 
@sortByField            AS INT = -1,
@sortAscending          AS BIT = 1

AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    DECLARE @trainingProviderName2  AS NVARCHAR(255)
    DECLARE @employerName2          AS NVARCHAR(255)

    -- Want to be able to search for 'and' as well as '&' as being synonymous with each other

    IF @trainingProviderName = 'and'
        SET @trainingProviderName2 = '&'
    ELSE IF @trainingProviderName = '&'
        SET @trainingProviderName2 = ' and '
    ELSE IF CHARINDEX('&', @trainingProviderName) > 0
        SET @trainingProviderName2 = REPLACE(@trainingProviderName, '&', 'and')
    ELSE IF CHARINDEX('and', @trainingProviderName) > 0
        SET @trainingProviderName2 = REPLACE(@trainingProviderName, 'and', '&')
    
    IF @employerName = 'and'
        SET @employerName2 = '&'
    ELSE IF @employerName = '&'
        SET @employerName2 = ' and '
    ELSE IF CHARINDEX('&', @employerName) > 0
        SET @employerName2 = REPLACE(@employerName, '&', 'and')
    ELSE IF CHARINDEX('and', @employerName) > 0
        SET @employerName2 = REPLACE(@employerName, 'and', '&')

    -- Temporary table to hold Training Provider Ids
    DECLARE @tpIds TABLE
    (
        TPId   NVARCHAR(20)
    )
    DECLARE @position   INT
    DECLARE @piece      NVARCHAR(20)
    DECLARE @workingIds NVARCHAR(400)

    -- Need to tack a delimiter onto the end of the input string if one doesn�t exist
    SET @workingIds = @trainingProviderId
    IF RIGHT(RTRIM(@workingIds),1) <> ','
        SET @workingIds = @workingIds  + ','
 
    SET @position =  PATINDEX('%,%' , @workingIds)
    WHILE @position <> 0
    BEGIN
        SET @piece = LEFT(@workingIds, @position - 1)
 
    -- You have a piece of data, so insert it, print it, do whatever you want to with it.
        INSERT INTO @tpIds VALUES ( @piece )

        SET @workingIds = STUFF(@workingIds, 1, @position, '')
        SET @position =  PATINDEX('%,%' , @workingIds)
    END
    
		DECLARE @TempMasterVacancyWithLiveSibling TABLE 
		( 
			MASTERVACANCYID INT
	    ) 
	    
	    DECLARE @TempMasterVacancy TABLE 
		( 
			VACANCYID INT 
	    ) 


     --Temp table to get all master VACANCY ids with siblngs in live, draft, submitted, referred, closed
    INSERT INTO @TempMasterVacancyWithLiveSibling SELECT DISTINCT MASTERVACANCYID FROM VACANCY 
								INNER JOIN VACANCYLOCATIONTYPE VLT ON VLT.VACANCYLOCATIONTYPEID = VACANCY.VACANCYLOCATIONTYPEID 
								INNER JOIN DBO.VACANCYSTATUSTYPE VST ON VST.VACANCYSTATUSTYPEID = VACANCY.VACANCYSTATUSID
								WHERE VLT.CODENAME IN ('MUL')
								AND VST.CODENAME IN ( 'LVE', 'DFT', 'SUB', 'REF', 'CLD', 'COM', 'WDR', 'PIE', 'DEL' )
								AND VACANCY.MASTERVACANCYID IS NOT NULL
								AND VACANCY.MASTERVACANCYID != VACANCY.VACANCYID
	
	--Temp table to get all VACANCY ids status in live, draft, submitted, referred, closed							
	INSERT INTO @TempMasterVacancy SELECT DISTINCT VACANCYID FROM VACANCY 
								INNER JOIN VACANCYLOCATIONTYPE VLT ON VLT.VACANCYLOCATIONTYPEID = VACANCY.VACANCYLOCATIONTYPEID 
								INNER JOIN DBO.VACANCYSTATUSTYPE VST ON VST.VACANCYSTATUSTYPEID = VACANCY.VACANCYSTATUSID
								WHERE VLT.CODENAME IN ('MUL', 'NAT')
								AND VST.CODENAME IN ( 'LVE', 'DFT', 'SUB', 'REF', 'CLD', 'COM', 'WDR', 'PIE', 'DEL' )
								AND (VACANCY.MASTERVACANCYID IS NULL OR VACANCY.MASTERVACANCYID = VACANCY.VACANCYID)

    -- Temporary table to hold results that we intend to pass back
    DECLARE @results TABLE
    (
        RowNumber INT,
        SortField NVARCHAR (255),
        TrainingProviderId INT,
        TrainingProviderFullName NVARCHAR (255),
        TrainingProviderTradingName NVARCHAR (255),
        TrainingProviderWebPage NVARCHAR (255),
        TrainingProviderTown NVARCHAR (50),
        TrainingProviderPostCode NVARCHAR (8),
        TrainingProviderStatusTypeId INT,
        
        ManagingAreaId INT,
        ManagingAreaCodeName NVARCHAR (3),
        ManagingAreaFullName NVARCHAR (100),
        ManagingAreaShortName NVARCHAR (50),

        EmployerId INT,
        EmployerFullName NVARCHAR (255),
        EmployerTradingName NVARCHAR (255),
        EmployerOwnerOrganisation NVARCHAR (255),
        EmployerTown NVARCHAR (50),
        EmployerPostCode NVARCHAR (8),

        VacancyId INT,
        VacancyMasterVacancyId INT,
        VacancyPostCode NVARCHAR (8),
        VacancyShortDescription NVARCHAR (256),
        VacancyTitle NVARCHAR (100),
        VacancyReferenceNumber INT,

        VacancyStatusTypeId INT,
        VacancyStatusCodeName NVARCHAR (3),
        VacancyStatusFullName NVARCHAR (200),
        VacancyStatusShortName NVARCHAR (100),
        VacancyLocation NVARCHAR (80),

        ApprenticeshipFrameworkId INT,
        ApprenticeFrameworkCodeName NVARCHAR (3),
        ApprenticeFrameworkShortName NVARCHAR (100),
        ApprenticeFrameworkFullName NVARCHAR (200),

        VacancyLocationCode NVARCHAR (3),
        VacancyLocationShortName NVARCHAR (100),
        VacancyLocationFullName NVARCHAR (200),
        
        TotalRows INT
    );WITH PagedVacancies AS
    (
        -- Actions the Sort Direction either Ascending or Descending
        -- Also forces multiple/national type of location to be at the end / beginning and sub sort 
        -- by national / multiple type of locations
        SELECT 
            CASE
                WHEN @sortAscending = 1 THEN RANK()OVER(ORDER BY SingleOrMultipleLocationSortOrder ASC, SubSingleOrMultipleLocationSortOrder ASC, SortField ASC)
                ELSE RANK()OVER(ORDER BY SingleOrMultipleLocationSortOrder ASC, SubSingleOrMultipleLocationSortOrder ASC, SortField DESC)
            END AS RowNumber,
            *
        FROM
            (
                -- This statement adds a ranking index to the data
                -- this allows us to sort the data by an unspecied field and to
                -- also allow us to retrieve a sub-section of the data, ie a pages worth of data
                SELECT 
                    ROW_NUMBER() OVER(ORDER BY SortByFieldText ASC) AS SortField,
                    MatchedVacancies.*,
                    COUNT(MatchedVacancies.VacancyId) OVER() AS TotalRows
                FROM
                    (
                    -- Most important SELECT statement that finds, links and retrieves the required data
                    -- Vacancies that match the requirements and are appropriately
                    -- linked to their related tables.
                    -- WHERE clause criteria as specied on the procedures parameter list
                    -- also, only interested in vacancies that have the status:
                    -- 'Lve' - Live
                    -- 'Dft' - Draft
                    -- 'Sub' - Submitted
                    -- 'Ref' - Referred
                    -- 'Cld' - Closed
                    
                    -- Single Location Types

                    SELECT 
                        CASE @sortByField
                            WHEN  '1' THEN tp.TradingName
                            WHEN  '2' THEN tp.FullName
                            WHEN  '3' THEN tp.Town
                            WHEN  '4' THEN LAG.FullName
                            WHEN  '5' THEN LAG.ShortName
                            WHEN  '6' THEN emp.TradingName
                            WHEN  '7' THEN emp.FullName
                            WHEN  '8' THEN emp.OwnerOrgnistaion
                            WHEN  '9' THEN emp.Town
                            WHEN '10' THEN Vacancy.Title
                            WHEN '11' THEN CONVERT(NVARCHAR(10), Vacancy.VacancyReferenceNumber)
                            WHEN '12' THEN vst.ShortName
                            WHEN '13' THEN vst.FullName
                            WHEN '14' THEN af.ShortName
                            WHEN '15' THEN af.FullName
                            WHEN '16' THEN Vacancy.Town
                            ELSE tp.FullName
                        END AS SortByFieldText,

                        tp.ProviderSiteID,
                        tp.FullName AS TrainingProviderFullName,
                        tp.TradingName AS TrainingProviderTradingName,
                        tp.WebPage AS TrainingProviderWebPage,
                        tp.Town AS TrainingProviderTown,
                        tp.PostCode AS TrainingProviderPostCode,
                        tp.TrainingProviderStatusTypeId,

                        LAG.LocalAuthorityGroupID AS ManagingAreaId,
                        LAG.CodeName AS ManagingAreaCodeName,
                        LAG.FullName AS ManagingAreaFullName,
                        LAG.ShortName AS ManagingAreaShortName,

                        emp.EmployerId,
                        emp.FullName AS EmployerFullName,
                        emp.TradingName AS EmployerTradingName,
                        emp.OwnerOrgnistaion AS EmployerOwnerOrganisation,
                        emp.Town AS EmployerTown,
                        emp.PostCode AS EmployerPostCode,

                        Vacancy.VacancyId,
                        Vacancy.MasterVacancyId AS VacancyMasterVacancyId,
                        Vacancy.PostCode AS VacancyPostCode,
                        Vacancy.ShortDescription AS VacancyShortDescription,
                        Vacancy.Title AS VacancyTitle,
                        Vacancy.VacancyReferenceNumber,

                        vst.VacancyStatusTypeId,
                        vst.CodeName AS VacancyStatusCodeName,
                        vst.FullName AS VacancyStatusFullName,
                        vst.ShortName AS VacancyStatusShortName,
                        Vacancy.Town AS VacancyLocation,
                        af.ApprenticeshipFrameworkId,
                        af.CodeName AS ApprenticeFrameworkCodeName,
                        af.ShortName AS ApprenticeFrameworkShortName,
                        af.FullName AS ApprenticeFrameworkFullName,
                        
                        vlt.CodeName AS VacancyLocationCode,
                        vlt.ShortName AS VacancyLocationShortName,
                        -- This is used to display the town on the UI as it uses the 
                        -- VacancyLocationFullName to display town, multi-location or nationwide
                        Vacancy.Town AS VacancyLocationFullName,
                        
                        -- Used to make sure multiple and national locations are at the end / beginning
                        -- and sub sorted by multiple locations then national
                        1 AS SingleOrMultipleLocationSortOrder,
                        1 AS SubSingleOrMultipleLocationSortOrder
                        
                        
                    FROM
                        dbo.[VacancyOwnerRelationship] rel INNER JOIN 
      
   
                   			dbo.[ProviderSiteRelationShip] PSR ON rel.[ProviderSiteID] = PSR.[ProviderSiteRelationshipID]
							
							INNER JOIN dbo.Provider P ON PSR.ProviderID = P.ProviderID
							INNER JOIN dbo.ProviderSite tp ON PSR.ProviderSiteID = tp.ProviderSiteID
							         			
                            INNER JOIN dbo.Employer emp ON rel.EmployerID = emp.EmployerID
							INNER JOIN dbo.LocalAuthority LA ON emp.LocalAuthorityId = LA.LocalAuthorityId
							INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
							INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
							INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
							AND LAGT.LocalAuthorityGroupTypeName = N'Managing Area'
                            INNER JOIN dbo.Vacancy ON Vacancy.[VacancyOwnerRelationshipId] = rel.[VacancyOwnerRelationshipId]
                            INNER JOIN dbo.VacancyStatusType vst ON vst.VacancyStatusTypeId = Vacancy.VacancyStatusId
                            INNER JOIN ApprenticeshipFramework af ON af.ApprenticeshipFrameworkId = Vacancy.ApprenticeshipFrameworkId
                            --INNER JOIN LocalAuthority la ON la.LocalAuthorityId = Vacancy.LocalAuthorityId
                            --INNER JOIN LSCRegion ON la.LSCRegionId = LSCRegion.LSCRegionId
                            INNER JOIN VacancyLocationType vlt ON vlt.VacancyLocationTypeId = Vacancy.VacancyLocationTypeId 
                    WHERE
                        vst.CodeName IN ( 'Lve', 'Dft', 'Sub', 'Ref', 'Cld', 'Com', 'Wdr', 'Pie', 'Del' )
                        AND ( tp.TradingName LIKE '%' + @trainingProviderName + '%' OR tp.TradingName LIKE '%' + @trainingProviderName2 + '%'  OR @trainingProviderName = '' )
                        AND ( (p.UKPRN = @trainingProviderUKPRN AND ProviderStatusTypeID <> 2)  OR @trainingProviderUKPRN = -1 )
                        AND ( emp.TradingName LIKE '%' + @employerName + '%' OR emp.TradingName LIKE '%' + @employerName2 + '%' OR @employerName = '' )
                        AND ( Vacancy.VacancyReferenceNumber = @vacancyRef OR @vacancyRef = -1)
                        AND ( af.ApprenticeshipOccupationId = @apprenticeOccupationId OR @apprenticeOccupationId = -1 )
                        AND ( af.ApprenticeshipFrameworkId = @apprenticeFrameworkId OR @apprenticeFrameworkId = -1 )
                        AND ( LAG.LocalAuthorityGroupID = @regionId or @regionId = -1 )
                        AND ( vlt.CodeName <> 'MUL' AND vlt.CodeName <> 'NAT' )
                        AND ( tp.ProviderSiteID IN ( SELECT TPId FROM @tpIds ) OR @trainingProviderId = '' )
                        AND ( Vacancy.MasterVacancyId IS NULL OR Vacancy.MasterVacancyId = Vacancy.VacancyId )
                    
                    UNION ALL
                    
                    -- Multi and National Location Types
                    SELECT
                        CASE @sortByField
                            WHEN  '1' THEN tp.TradingName
                            WHEN  '2' THEN tp.FullName
                            WHEN  '3' THEN tp.Town
                            WHEN  '4' THEN LAG.FullName
                            WHEN  '5' THEN LAG.ShortName
                            WHEN  '6' THEN emp.TradingName
                            WHEN  '7' THEN emp.FullName
                            WHEN  '8' THEN emp.OwnerOrgnistaion
                            WHEN  '9' THEN emp.Town
                            WHEN '10' THEN Vacancy.Title
                            WHEN '11' THEN CONVERT(NVARCHAR(10), Vacancy.VacancyReferenceNumber)
                            WHEN '12' THEN vst.ShortName
                            WHEN '13' THEN vst.FullName
                            WHEN '14' THEN af.ShortName
                            WHEN '15' THEN af.FullName
                            WHEN '16' THEN Vacancy.Town
                            ELSE tp.FullName
                        END AS SortByFieldText,

                        tp.ProviderSiteID,
                        tp.FullName AS TrainingProviderFullName,
                        tp.TradingName AS TrainingProviderTradingName,
                        tp.WebPage AS TrainingProviderWebPage,
                        tp.Town AS TrainingProviderTown,
                        tp.PostCode AS TrainingProviderPostCode,
                        tp.TrainingProviderStatusTypeId,

						LAG.LocalAuthorityGroupID AS ManagingAreaId,
                        LAG.CodeName AS ManagingAreaCodeName,
                        LAG.FullName AS ManagingAreaFullName,
                        LAG.ShortName AS ManagingAreaShortName,

                        emp.EmployerId,
                        emp.FullName AS EmployerFullName,
                        emp.TradingName AS EmployerTradingName,
                        emp.OwnerOrgnistaion AS EmployerOwnerOrganisation,
                        emp.Town AS EmployerTown,
                        emp.PostCode AS EmployerPostCode,

                        Vacancy.VacancyId,
                        Vacancy.MasterVacancyId AS VacancyMasterVacancyId,
                        Vacancy.PostCode AS VacancyPostCode,
                        Vacancy.ShortDescription AS VacancyShortDescription,
                        Vacancy.Title AS VacancyTitle,
                        Vacancy.VacancyReferenceNumber,

                        vst.VacancyStatusTypeId,
                        vst.CodeName AS VacancyStatusCodeName,
                        vst.FullName AS VacancyStatusFullName,
                        vst.ShortName AS VacancyStatusShortName,
                        Vacancy.Town AS VacancyLocation,
                        af.ApprenticeshipFrameworkId,
                        af.CodeName AS ApprenticeFrameworkCodeName,
                        af.ShortName AS ApprenticeFrameworkShortName,
                        af.FullName AS ApprenticeFrameworkFullName,
                        
                        vlt.CodeName AS VacancyLocationCode,
                        vlt.ShortName AS VacancyLocationShortName,
                        CASE vlt.CodeName
                            WHEN  'MUL' THEN 'Multiple Location'
                            WHEN  'NAT' THEN 'Nationwide'
                        END AS VacancyLocationFullName,
                        
                        -- Used to make sure multiple and national locations are at the end / beginning
                        -- and sub sorted by multiple locations then national
                        2 AS SingleOrMultipleLocationSortOrder,
                        CASE vlt.CodeName
                            WHEN  'MUL' THEN 2
                            WHEN  'NAT' THEN 3
                        END AS SubSingleOrMultipleLocationSortOrder
                        
                    FROM
                        dbo.[VacancyOwnerRelationship] rel INNER JOIN 
      
   
                   			dbo.[ProviderSiteRelationShip] PSR ON rel.[ProviderSiteID] = PSR.[ProviderSiteRelationshipID]
							
							INNER JOIN dbo.Provider P ON PSR.ProviderID = P.ProviderID
							INNER JOIN dbo.ProviderSite tp ON PSR.ProviderSiteID = tp.ProviderSiteID
							 INNER JOIN dbo.Employer emp ON rel.EmployerID = emp.EmployerID
							 INNER JOIN dbo.LocalAuthority LA ON emp.LocalAuthorityId = LA.LocalAuthorityId
							 INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
							 INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
							 INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
							 AND LAGT.LocalAuthorityGroupTypeName = N'Managing Area'
                             INNER JOIN dbo.Vacancy ON Vacancy.[VacancyOwnerRelationshipId] = rel.[VacancyOwnerRelationshipId]
                             INNER JOIN dbo.VacancyStatusType vst ON vst.VacancyStatusTypeId = Vacancy.VacancyStatusId
                             INNER JOIN ApprenticeshipFramework af ON af.ApprenticeshipFrameworkId = Vacancy.ApprenticeshipFrameworkId
                             --INNER JOIN LocalAuthority la ON la.LocalAuthorityId = Vacancy.LocalAuthorityId
                             --INNER JOIN LSCRegion ON la.LSCRegionId = LSCRegion.LSCRegionId
                             INNER JOIN VacancyLocationType vlt ON vlt.VacancyLocationTypeId = Vacancy.VacancyLocationTypeId 
                    WHERE
                        --vst.CodeName IN ( 'Lve', 'Dft', 'Sub', 'Ref', 'Cld', 'Com', 'Wdr', 'Pie', 'Del' ) AND
							( tp.TradingName LIKE '%' + @trainingProviderName + '%' OR tp.TradingName LIKE '%' + @trainingProviderName2 + '%'  OR @trainingProviderName = '' )
                        AND (( p.UKPRN = @trainingProviderUKPRN AND ProviderStatusTypeID <> 2 ) OR @trainingProviderUKPRN = -1 )
                        AND ( emp.TradingName LIKE '%' + @employerName + '%' OR emp.TradingName LIKE '%' + @employerName2 + '%' OR @employerName = '')
                        AND ( Vacancy.VacancyReferenceNumber = @vacancyRef OR @vacancyRef = -1)
                        AND ( af.ApprenticeshipOccupationId = @apprenticeOccupationId OR @apprenticeOccupationId = -1 )
                        AND ( af.ApprenticeshipFrameworkId = @apprenticeFrameworkId OR @apprenticeFrameworkId = -1)
                        AND ( LAG.LocalAuthorityGroupID = @regionId or @regionId = -1 )
                        AND ( vlt.CodeName = 'MUL' OR vlt.CodeName = 'NAT' )
                        AND ( tp.ProviderSiteID IN ( SELECT TPId FROM @tpIds ) OR @trainingProviderId = '' )
                        AND ( Vacancy.MasterVacancyId IS NULL OR Vacancy.MasterVacancyId = Vacancy.VacancyId )
                        AND ( Vacancy.VacancyId IN ( SELECT VacancyId FROM @TempMasterVacancy ) 
							   OR Vacancy.VacancyId IN ( SELECT MasterVacancyId FROM @TempMasterVacancyWithLiveSibling )
							 )
                        
                    ) AS MatchedVacancies 
                    
            ) AS SortedVacancies 
   )

    -- INSERT INTO the results temporary table to allow us to only pull back a portion of the data
    -- that the client is interested in.
    -- normally this will be a page worth of records. 1..10, 11..20, 21..30 etc
    

        INSERT INTO @results
        (
            RowNumber,
            SortField,
            TrainingProviderId,
            TrainingProviderFullName,
            TrainingProviderTradingName,
            TrainingProviderWebPage,
            TrainingProviderTown,
            TrainingProviderPostCode,
            TrainingProviderStatusTypeId,
              
			ManagingAreaId ,
			ManagingAreaCodeName,
			ManagingAreaFullName,
			ManagingAreaShortName,

            EmployerId,
            EmployerFullName,
            EmployerTradingName,
            EmployerOwnerOrganisation,
            EmployerTown,
            EmployerPostCode,

            VacancyId,
            VacancyMasterVacancyId,
            VacancyPostCode,
            VacancyShortDescription,
            VacancyTitle,
            VacancyReferenceNumber,

            VacancyStatusTypeId,
            VacancyStatusCodeName,
            VacancyStatusFullName,
            VacancyStatusShortName,
            VacancyLocation,

            ApprenticeshipFrameworkId,
            ApprenticeFrameworkCodeName,
            ApprenticeFrameworkShortName,
            ApprenticeFrameworkFullName,

            VacancyLocationCode,
            VacancyLocationShortName,
            VacancyLocationFullName,
            
            TotalRows
        )
            SELECT
                RowNumber,
                SortField,
                ProviderSiteID,
                TrainingProviderFullName,
                TrainingProviderTradingName,
                TrainingProviderWebPage,
                TrainingProviderTown,
                TrainingProviderPostCode,
                TrainingProviderStatusTypeId,
                
                ManagingAreaId,
				ManagingAreaCodeName,
				ManagingAreaFullName,
				ManagingAreaShortName,

                EmployerId,
                EmployerFullName,
                EmployerTradingName,
                EmployerOwnerOrganisation,
                EmployerTown,
                EmployerPostCode,

                VacancyId,
                VacancyMasterVacancyId,
                VacancyPostCode,
                VacancyShortDescription,
                VacancyTitle,
                VacancyReferenceNumber,

                VacancyStatusTypeId,
                VacancyStatusCodeName,
                VacancyStatusFullName,
                VacancyStatusShortName,
                VacancyLocation,

                ApprenticeshipFrameworkId,
                ApprenticeFrameworkCodeName,
                ApprenticeFrameworkShortName,
                ApprenticeFrameworkFullName,

                VacancyLocationCode,
                VacancyLocationShortName,
                VacancyLocationFullName,
                
                TotalRows
            FROM
                PagedVacancies
            WHERE
                RowNumber BETWEEN 
                    ((@requestedPageNo - 1) * @pageSize) + 1 
                    AND 
                    ((@requestedPageNo - 1) * @pageSize) + @pageSize 
            ORDER BY RowNumber

-- All done now, pull the data from the temporary table so the client can retrieve it
SELECT * FROM @results


END