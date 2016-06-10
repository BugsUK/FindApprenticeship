CREATE PROCEDURE dbo.uspTransferVacancySearchFilter  
@trainingProviderName   AS NVARCHAR(255) = '',  
@trainingProviderUKPRN  AS INT =  -1,  
@employerName   AS NVARCHAR(255) = '',    
@trainingProviderId     AS NVARCHAR(400) = '',  --@ownerId
@contractOwnerId		AS INT = -1,
@vacancyManagerId		AS INT = -1,
@delivererId			AS INT = -1,
@originalContractOwnerId AS INT = -1,
@apprenticeOccupationId AS INT = -1,  
@apprenticeFrameworkId  AS INT = -1,  
@regionId		        AS INT = -1,  
@requestedPageNo        AS INT = 1,   
@pageSize               AS INT = 10,   
@sortByField            AS INT = -1,  
@sortAscending          AS BIT = 1  
AS  
BEGIN  
    SET NOCOUNT ON  

BEGIN TRY  


 DECLARE @trainingProviderName2  AS NVARCHAR(255)  
 DECLARE @employerName2          AS NVARCHAR(255)  

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
    DECLARE @psIds TABLE  
    (  
        psId   NVARCHAR(20)  
    )  
    DECLARE @position   INT  
    DECLARE @piece      NVARCHAR(20)  
    DECLARE @workingIds NVARCHAR(400)  
  
    -- Need to tack a delimiter onto the end of the input string if one doesn?t exist  
    SET @workingIds = @trainingProviderId  
    IF RIGHT(RTRIM(@workingIds),1) <> ','  
        SET @workingIds = @workingIds  + ','  
   
    SET @position =  PATINDEX('%,%' , @workingIds)  
    WHILE @position <> 0  
    BEGIN  
        SET @piece = LEFT(@workingIds, @position - 1)  
   
    -- You have a piece of data, so insert it, print it, do whatever you want to with it.  
        INSERT INTO @psIds VALUES ( @piece )  
  
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
  
  DECLARE @TempResultVacancyOwner TABLE   
  (   
   VACANCYID INT,
   CodeName VARCHAR(10),
   ProviderId INT  ,
   ProviderSiteId INT   
     )   
     
     DECLARE @TempResultDeliverer TABLE   
  (   
   VACANCYID INT,
   CodeName VARCHAR(10),
   ProviderId INT  ,
   ProviderSiteId INT   
     )   
     
      DECLARE @TempResultContractOwner TABLE   
  (   
   VACANCYID INT,
   CodeName VARCHAR(10),
   ProviderId INT  ,
   ProviderSiteId INT
     )  
 
 DECLARE @TempResultNatVacancyOwner TABLE   
  (   
   VACANCYID INT,
   CodeName VARCHAR(10),
   ProviderId INT  ,
   ProviderSiteId INT   
     )   
     
     DECLARE @TempResultNatDeliverer TABLE   
  (   
   VACANCYID INT,
   CodeName VARCHAR(10),
   ProviderId INT  ,
   ProviderSiteId INT   
  )    
  
       DECLARE @TempResultNatContractOwner TABLE   
  (   
   VACANCYID INT,
   CodeName VARCHAR(10),
   ProviderId INT  ,
   ProviderSiteId INT
     ) 
     
      ---------------------------------------
  
   DECLARE @AllLocalAuthority TABLE  
    (  
        LocalAuthorityId   int,  
        LocalAuthorityGroupID int,  
        CodeName NVARCHAR(6),  
        FullName NVARCHAR(200),
        ShortName NVARCHAR(100)
    )  
    
    INSERT INTO  @AllLocalAuthority
    SELECT DISTINCT LA.LocalAuthorityId , LAG.LocalAuthorityGroupID ,LAG.CodeName ,LAG.FullName, LAG.ShortName
    FROM	dbo.LocalAuthority LA 
			INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID  
			INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID  
			INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID 
			AND LAGT.LocalAuthorityGroupTypeName = N'Region' 
    
  -----------------------------------------   
 
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

 
 INSERT INTO 
		@TempResultVacancyOwner
 --Vacancy owner of single vacancy
SELECT
		DISTINCT  v.VacancyId, vlt.CodeName, p.ProviderId, ps.ProviderSiteId
FROM		
		dbo.[VacancyOwnerRelationship] rel 
		INNER JOIN	dbo.ProviderSite ps ON rel.ProviderSiteID = ps.ProviderSiteID  
		INNER JOIN	dbo.ProviderSiteRelationship psr ON psr.ProviderSiteId = ps.ProviderSiteId  
        INNER JOIN	dbo.Provider p ON p.ProviderId = psr.ProviderId
        INNER JOIN	dbo.Vacancy v ON v.[VacancyOwnerRelationshipId] = rel.[VacancyOwnerRelationshipId]  
        INNER JOIN	dbo.VacancyStatusType vst ON vst.VacancyStatusTypeId = v.VacancyStatusId  
        LEFT JOIN  VacancyLocationType vlt ON vlt.VacancyLocationTypeId = v.VacancyLocationTypeId                   
WHERE  
        vst.CodeName IN ( 'Lve', 'Dft', 'Sub', 'Ref','Cld', 'Com', 'Wdr', 'Pie', 'Del' )   
		AND ( ps.ProviderSiteID IN ( SELECT psId FROM @psIds ) OR @trainingProviderId = '' )  
        AND ( (vlt.CodeName <> 'MUL' AND vlt.CodeName <> 'NAT') OR vlt.CodeName IS NULL )
		AND ( v.MasterVacancyId IS NULL OR v.MasterVacancyId = v.VacancyId )  
		AND psr.ProviderSiteRelationShipTypeID = 1    
		 
		
 --Contract Owner  for single vacancy
INSERT INTO
		@TempResultContractOwner
SELECT
		DISTINCT  v.VacancyId, vlt.CodeName, p.ProviderId,  psr.ProviderSiteId
FROM		
		dbo.Provider p
		INNER JOIN	dbo.ProviderSiteRelationship psr ON psr.ProviderId = p.ProviderId  
		INNER JOIN	dbo.[VacancyOwnerRelationship] rel ON psr.ProviderSiteId = rel.ProviderSiteId
		INNER JOIN  dbo.Vacancy v ON v.VacancyOwnerRelationshipId = rel.VacancyOwnerRelationshipId
		INNER JOIN	dbo.VacancyStatusType vst ON vst.VacancyStatusTypeId = v.VacancyStatusId  
        LEFT JOIN VacancyLocationType vlt ON vlt.VacancyLocationTypeId = v.VacancyLocationTypeId  
WHERE  
        vst.CodeName IN ( 'Lve', 'Dft', 'Sub', 'Ref','Cld', 'Com', 'Wdr', 'Pie', 'Del' )   
        AND ( (vlt.CodeName <> 'MUL' AND vlt.CodeName <> 'NAT') OR vlt.CodeName IS NULL )
        AND ( v.MasterVacancyId IS NULL OR v.MasterVacancyId = v.VacancyId )  
        --AND ( ps.ProviderSiteID IN (  SELECT psId FROM @psIds  ) OR @trainingProviderId = '' )  
        AND ( p.UKPRN = @trainingProviderUKPRN
			  AND p.ProviderStatusTypeID <> 2 )  
        AND psr.ProviderSiteRelationShipTypeID = 2                                              
        AND ( 0 IN (  SELECT psId FROM @psIds WHERE psId = 0 ) ) 		   
		AND v.ContractOwnerID IN (SELECT ProviderId FROM Provider WHERE UKPRN = @trainingProviderUKPRN
									  AND ProviderStatusTypeID <> 2)
		
 --Deliverer  for single vacancy
INSERT INTO
		@TempResultDeliverer
SELECT
		DISTINCT  v.VacancyId, vlt.CodeName, p.ProviderId, ps.ProviderSiteId
FROM		
		dbo.Vacancy v
		INNER JOIN	dbo.ProviderSite ps ON v.DeliveryOrganisationID = ps.ProviderSiteID
		INNER JOIN	dbo.ProviderSiteRelationship psr ON psr.ProviderSiteId = ps.ProviderSiteId  
        INNER JOIN	dbo.Provider p ON p.ProviderId = psr.ProviderId
        INNER JOIN	dbo.VacancyStatusType vst ON vst.VacancyStatusTypeId = v.VacancyStatusId  
        LEFT JOIN VacancyLocationType vlt ON vlt.VacancyLocationTypeId = v.VacancyLocationTypeId   
WHERE  
        vst.CodeName IN ( 'Lve', 'Dft', 'Sub', 'Ref','Cld', 'Com', 'Wdr', 'Pie', 'Del' )   
		AND ( ps.ProviderSiteID IN ( SELECT psId FROM @psIds ) OR @trainingProviderId = '' )  
        AND ( (vlt.CodeName <> 'MUL' AND vlt.CodeName <> 'NAT') OR vlt.CodeName IS NULL )
        AND ( v.MasterVacancyId IS NULL OR v.MasterVacancyId = v.VacancyId )  
		AND psr.ProviderSiteRelationShipTypeID = 1
        
/********************************************************************************/                                                                                                                                                                                                                                              

-- --Vacancy owner of multi/nat vacancy
INSERT INTO
		@TempResultNatVacancyOwner
SELECT
		DISTINCT  v.VacancyId, vlt.CodeName, p.ProviderId, ps.ProviderSiteId
FROM		
		dbo.[VacancyOwnerRelationship] rel 
		INNER JOIN	dbo.ProviderSite ps ON rel.ProviderSiteID = ps.ProviderSiteID  
		INNER JOIN	dbo.ProviderSiteRelationship psr ON psr.ProviderSiteId = ps.ProviderSiteId  
        INNER JOIN	dbo.Provider p ON p.ProviderId = psr.ProviderId
        INNER JOIN	dbo.Vacancy v ON v.[VacancyOwnerRelationshipId] = rel.[VacancyOwnerRelationshipId]  
        INNER JOIN	dbo.VacancyStatusType vst ON vst.VacancyStatusTypeId = v.VacancyStatusId  
        INNER JOIN  VacancyLocationType vlt ON vlt.VacancyLocationTypeId = v.VacancyLocationTypeId                   
WHERE  
       -- vst.CodeName IN ( 'Lve', 'Dft', 'Sub', 'Ref','Cld', 'Com', 'Wdr', 'Pie', 'Del' )   AND
		 ( ps.ProviderSiteID IN ( SELECT psId FROM @psIds ) OR @trainingProviderId = '' )  
        AND ( vlt.CodeName = 'MUL' OR vlt.CodeName = 'NAT' )  
        AND ( v.MasterVacancyId IS NULL OR v.MasterVacancyId = v.VacancyId )  
        AND ( v.VacancyId IN ( SELECT VacancyId FROM @TempMasterVacancy )   
			OR v.VacancyId IN ( SELECT MasterVacancyId FROM @TempMasterVacancyWithLiveSibling ) ) 
        AND psr.ProviderSiteRelationShipTypeID = 1  
                                 
-- --Deliverer  for multi/nat vacancy
INSERT INTO
		@TempResultNatDeliverer
SELECT
		DISTINCT  v.VacancyId, vlt.CodeName, p.ProviderId, ps.ProviderSiteId
FROM		
		dbo.Vacancy v
		INNER JOIN	dbo.ProviderSite ps ON v.DeliveryOrganisationID = ps.ProviderSiteID  
        INNER JOIN	dbo.ProviderSiteRelationship psr ON psr.ProviderSiteId = ps.ProviderSiteId  
        INNER JOIN	dbo.Provider p ON p.ProviderId = psr.ProviderId
        INNER JOIN	dbo.VacancyStatusType vst ON vst.VacancyStatusTypeId = v.VacancyStatusId  
        INNER JOIN VacancyLocationType vlt ON vlt.VacancyLocationTypeId = v.VacancyLocationTypeId   
WHERE  
       -- vst.CodeName IN ( 'Lve', 'Dft', 'Sub', 'Ref','Cld', 'Com', 'Wdr', 'Pie', 'Del' )   AND
		 ( ps.ProviderSiteID IN ( SELECT psId FROM @psIds ) OR @trainingProviderId = '' )  
        AND ( vlt.CodeName = 'MUL' OR vlt.CodeName = 'NAT' )  
        AND ( v.MasterVacancyId IS NULL OR v.MasterVacancyId = v.VacancyId )  
		AND ( v.VacancyId IN ( SELECT VacancyId FROM @TempMasterVacancy )   
			OR v.VacancyId IN ( SELECT MasterVacancyId FROM @TempMasterVacancyWithLiveSibling ) ) 
       AND psr.ProviderSiteRelationShipTypeID = 1
       

-- --Contract Owner  for multi/nat vacancy
INSERT INTO
		@TempResultNatContractOwner
SELECT
		DISTINCT  v.VacancyId, vlt.CodeName, p.ProviderId,  psr.ProviderSiteId
FROM		
		dbo.Provider p
		INNER JOIN	dbo.ProviderSiteRelationship psr ON psr.ProviderId = p.ProviderId  
		INNER JOIN	dbo.[VacancyOwnerRelationship] rel ON psr.ProviderSiteId = rel.ProviderSiteId
		INNER JOIN  dbo.Vacancy v ON v.VacancyOwnerRelationshipId = rel.VacancyOwnerRelationshipId
		INNER JOIN	dbo.VacancyStatusType vst ON vst.VacancyStatusTypeId = v.VacancyStatusId  
        INNER JOIN VacancyLocationType vlt ON vlt.VacancyLocationTypeId = v.VacancyLocationTypeId  
WHERE  
        ( vlt.CodeName = 'MUL' OR vlt.CodeName = 'NAT' )  
        AND ( p.UKPRN = @trainingProviderUKPRN   AND p.ProviderStatusTypeID <> 2)
        AND ( v.MasterVacancyId IS NULL OR v.MasterVacancyId = v.VacancyId )  
        AND ( v.VacancyId IN ( SELECT VacancyId FROM @TempMasterVacancy )   
			OR v.VacancyId IN ( SELECT MasterVacancyId FROM @TempMasterVacancyWithLiveSibling ) ) 
		AND psr.ProviderSiteRelationShipTypeID = 2  
        AND ( 0 IN (  SELECT psId FROM @psIds WHERE psId = 0 ) )
        AND v.ContractOwnerID IN (SELECT ProviderId FROM Provider WHERE UKPRN = @trainingProviderUKPRN AND ProviderStatusTypeID <> 2) 
        
--select * from @TempResultVacancyOwner
--select * from @TempResultDeliverer
--select * from @TempResultContractOwner
--select * from @TempResultNatVacancyOwner
--select * from @TempResultNatDeliverer
--select * from @TempResultNatContractOwner


--/*******************STD VACANCIES ***********************************************************/ 
DECLARE @ResultsStandardVacancy TABLE  
    (  
       
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
        
        ContractOwnerId INT,
        ContractOwnerTradingName NVARCHAR (200),  
        VacancyManagerId INT,
        VacancyManager NVARCHAR (200),
        VacancyManagerTown NVARCHAR (200),
        DelivererId INT,  
        Deliverer NVARCHAR (200), 
        DelivererTown NVARCHAR (200),    
	    OriginalContractOwnerId INT,
		OriginalContractOwner NVARCHAR (200),  
				
		SingleOrMultipleLocationSortOrder NVARCHAR (20),  
        SubSingleOrMultipleLocationSortOrder NVARCHAR (20)           
		 )
		
INSERT INTO		@ResultsStandardVacancy 		 
SELECT   DISTINCT
                        ps.ProviderSiteID,  
                        ps.FullName AS TrainingProviderFullName,  
                        ps.TradingName AS TrainingProviderTradingName,  
                        ps.WebPage AS TrainingProviderWebPage,  
                        ps.Town AS TrainingProviderTown,  
                        ps.PostCode AS TrainingProviderPostCode,  
                        ps.TrainingProviderStatusTypeId,  
  
                        LA.LocalAuthorityGroupID AS ManagingAreaId,  
                        LA.CodeName AS ManagingAreaCodeName,  
                        LA.FullName AS ManagingAreaFullName,  
                        LA.ShortName AS ManagingAreaShortName,  
  
                        emp.EmployerId,  
                        emp.FullName AS EmployerFullName,  
                        emp.TradingName AS EmployerTradingName,  
                        emp.OwnerOrgnistaion AS EmployerOwnerOrganisation,  
                        emp.Town AS EmployerTown,  
                        emp.PostCode AS EmployerPostCode,  
  
                        v.VacancyId,  
                        v.MasterVacancyId AS VacancyMasterVacancyId,  
                        v.PostCode AS VacancyPostCode,  
                        v.ShortDescription AS VacancyShortDescription,  
                        v.Title AS VacancyTitle,  
                        v.VacancyReferenceNumber,  
  
                        vst.VacancyStatusTypeId,  
                        vst.CodeName AS VacancyStatusCodeName,  
                        vst.FullName AS VacancyStatusFullName,  
                        vst.ShortName AS VacancyStatusShortName,  
                        v.Town AS VacancyLocation,  
                        af.ApprenticeshipFrameworkId,  
                        af.CodeName AS ApprenticeFrameworkCodeName,  
                        af.ShortName AS ApprenticeFrameworkShortName,  
                        af.FullName AS ApprenticeFrameworkFullName,  
                          
                        vlt.CodeName AS VacancyLocationCode,  
                        vlt.ShortName AS VacancyLocationShortName,  
                        -- This is used to display the town on the UI as it uses the   
                        -- VacancyLocationFullName to display town, multi-location or nationwide  
                        v.Town AS VacancyLocationFullName,  
                        
                        pc.ProviderId AS ContractOwnwerId,
                        pc.TradingName AS ContractOwnerTradingName, 
                        psv.ProviderSiteID AS VacancyManagerId,  
                        psv.TradingName AS VacancyManagerFullName,  
                        psv.Town,
                        psd.ProviderSiteID AS DelivererID,  
                        psd.TradingName AS DelivererFullName,  
                        psd.Town,
                        poc.ProviderId,
						poc.TradingName,
  
                        -- Used to make sure multiple and national locations are at the end / beginning  
                        -- and sub sorted by multiple locations then national  
                        1 AS SingleOrMultipleLocationSortOrder,  
                        1 AS SubSingleOrMultipleLocationSortOrder                           
                    FROM
						dbo.Vacancy  v 
						INNER JOIN @TempResultDeliverer cmanager ON v.VacancyId = cmanager.VacancyId
						INNER JOIN dbo.Provider P ON cmanager.ProviderID = P.ProviderID  
						INNER JOIN dbo.ProviderSite PSD ON cmanager.ProviderSiteID = PSD.ProviderSiteID  
						INNER JOIN dbo.[VacancyOwnerRelationship] rel ON v.[VacancyOwnerRelationshipId] = rel.[VacancyOwnerRelationshipId]  
						INNER JOIN dbo.Employer emp ON rel.EmployerID = emp.EmployerID  
						LEFT JOIN @AllLocalAuthority LA ON v.LocalAuthorityId = LA.LocalAuthorityId 
						INNER JOIN dbo.VacancyStatusType vst ON vst.VacancyStatusTypeId = v.VacancyStatusId  
						LEFT JOIN ApprenticeshipFramework af ON af.ApprenticeshipFrameworkId = v.ApprenticeshipFrameworkId  
						LEFT JOIN VacancyLocationType vlt ON vlt.VacancyLocationTypeId = v.VacancyLocationTypeId   
						LEFT JOIN ProviderSite PSV ON PSV.ProviderSiteId = v.VacancyManagerID
						INNER JOIN ProviderSite PS ON PS.ProviderSiteId = rel.ProviderSiteId
						LEFT JOIN Provider POC ON POC.ProviderId = v.OriginalContractOwnerId						
						LEFT JOIN Provider PC ON PC.ProviderId = v.ContractOwnerId	
UNION

SELECT   DISTINCT
                        ps.ProviderSiteID,  
                        ps.FullName AS TrainingProviderFullName,  
                        ps.TradingName AS TrainingProviderTradingName,  
                        ps.WebPage AS TrainingProviderWebPage,  
                        ps.Town AS TrainingProviderTown,  
                        ps.PostCode AS TrainingProviderPostCode,  
                        ps.TrainingProviderStatusTypeId,  
  
                        LA.LocalAuthorityGroupID AS ManagingAreaId,  
                        LA.CodeName AS ManagingAreaCodeName,  
                        LA.FullName AS ManagingAreaFullName,  
                        LA.ShortName AS ManagingAreaShortName,  
  
                        emp.EmployerId,  
                        emp.FullName AS EmployerFullName,  
                        emp.TradingName AS EmployerTradingName,  
                        emp.OwnerOrgnistaion AS EmployerOwnerOrganisation,  
                        emp.Town AS EmployerTown,  
                        emp.PostCode AS EmployerPostCode,  
  
                        v.VacancyId,  
                        v.MasterVacancyId AS VacancyMasterVacancyId,  
                        v.PostCode AS VacancyPostCode,  
                        v.ShortDescription AS VacancyShortDescription,  
                        v.Title AS VacancyTitle,  
                        v.VacancyReferenceNumber,  
  
                        vst.VacancyStatusTypeId,  
                        vst.CodeName AS VacancyStatusCodeName,  
                        vst.FullName AS VacancyStatusFullName,  
                        vst.ShortName AS VacancyStatusShortName,  
                        v.Town AS VacancyLocation,  
                        af.ApprenticeshipFrameworkId,  
                        af.CodeName AS ApprenticeFrameworkCodeName,  
                        af.ShortName AS ApprenticeFrameworkShortName,  
                        af.FullName AS ApprenticeFrameworkFullName,  
                          
                        vlt.CodeName AS VacancyLocationCode,  
                        vlt.ShortName AS VacancyLocationShortName,  
                        -- This is used to display the town on the UI as it uses the   
                        -- VacancyLocationFullName to display town, multi-location or nationwide  
                        v.Town AS VacancyLocationFullName,  
                        
                        pc.ProviderId AS ContractOwnwerId,
                        pc.TradingName AS ContractOwnerTradingName, 
                        psv.ProviderSiteID AS VacancyManagerId,  
                        psv.TradingName AS VacancyManagerFullName,  
                        psv.Town,
                        psd.ProviderSiteID AS DelivererID,  
                        psd.TradingName AS DelivererFullName,  
                        psd.Town,
                        poc.ProviderId,
						poc.TradingName,
  
                        -- Used to make sure multiple and national locations are at the end / beginning  
                        -- and sub sorted by multiple locations then national  
                        1 AS SingleOrMultipleLocationSortOrder,  
                        1 AS SubSingleOrMultipleLocationSortOrder                           
                    FROM
						dbo.Vacancy  v 
						INNER JOIN @TempResultVacancyOwner cmanager ON v.VacancyId = cmanager.VacancyId
						INNER JOIN dbo.Provider P ON cmanager.ProviderID = P.ProviderID  
						INNER JOIN dbo.ProviderSite PS ON cmanager.ProviderSiteID = PS.ProviderSiteID  
						INNER JOIN dbo.[VacancyOwnerRelationship] rel ON v.[VacancyOwnerRelationshipId] = rel.[VacancyOwnerRelationshipId]  
						INNER JOIN dbo.Employer emp ON rel.EmployerID = emp.EmployerID  
						LEFT JOIN @AllLocalAuthority LA ON v.LocalAuthorityId = LA.LocalAuthorityId  
						INNER JOIN dbo.VacancyStatusType vst ON vst.VacancyStatusTypeId = v.VacancyStatusId  
						LEFT JOIN ApprenticeshipFramework af ON af.ApprenticeshipFrameworkId = v.ApprenticeshipFrameworkId  
						LEFT JOIN VacancyLocationType vlt ON vlt.VacancyLocationTypeId = v.VacancyLocationTypeId   
						LEFT JOIN ProviderSite PSD ON PSD.ProviderSiteId = v.DeliveryOrganisationID
						LEFT JOIN ProviderSite PSV ON PSV.ProviderSiteId = v.VacancyManagerID
						LEFT JOIN Provider POC ON POC.ProviderId = v.OriginalContractOwnerId
						LEFT JOIN Provider PC ON PC.ProviderId = v.ContractOwnerId	
UNION					
							 
SELECT   DISTINCT
                        ps.ProviderSiteID,  
                        ps.FullName AS TrainingProviderFullName,  
                        ps.TradingName AS TrainingProviderTradingName,  
                        ps.WebPage AS TrainingProviderWebPage,  
                        ps.Town AS TrainingProviderTown,  
                        ps.PostCode AS TrainingProviderPostCode,  
                        ps.TrainingProviderStatusTypeId,  
  
                        LA.LocalAuthorityGroupID AS ManagingAreaId,  
                        LA.CodeName AS ManagingAreaCodeName,  
                        LA.FullName AS ManagingAreaFullName,  
                        LA.ShortName AS ManagingAreaShortName,  
  
                        emp.EmployerId,  
                        emp.FullName AS EmployerFullName,  
                        emp.TradingName AS EmployerTradingName,  
                        emp.OwnerOrgnistaion AS EmployerOwnerOrganisation,  
                        emp.Town AS EmployerTown,  
                        emp.PostCode AS EmployerPostCode,  
  
                        v.VacancyId,  
                        v.MasterVacancyId AS VacancyMasterVacancyId,  
                        v.PostCode AS VacancyPostCode,  
                        v.ShortDescription AS VacancyShortDescription,  
                        v.Title AS VacancyTitle,  
                        v.VacancyReferenceNumber,  
  
                        vst.VacancyStatusTypeId,  
                        vst.CodeName AS VacancyStatusCodeName,  
                        vst.FullName AS VacancyStatusFullName,  
                        vst.ShortName AS VacancyStatusShortName,  
                        v.Town AS VacancyLocation,  
                        af.ApprenticeshipFrameworkId,  
                        af.CodeName AS ApprenticeFrameworkCodeName,  
                        af.ShortName AS ApprenticeFrameworkShortName,  
                        af.FullName AS ApprenticeFrameworkFullName,  
                          
                        vlt.CodeName AS VacancyLocationCode,  
                        vlt.ShortName AS VacancyLocationShortName,  
                        -- This is used to display the town on the UI as it uses the   
                        -- VacancyLocationFullName to display town, multi-location or nationwide  
                        v.Town AS VacancyLocationFullName,  
                        
                        pc.ProviderId AS ContractOwnwerId,
                        pc.TradingName AS ContractOwnerTradingName, 
                        psv.ProviderSiteID AS VacancyManagerId,  
                        psv.TradingName AS VacancyManagerFullName,  
                        psv.Town,
                        psd.ProviderSiteID AS DelivererID,  
                        psd.TradingName AS DelivererFullName,  
                        psd.Town,  
                        poc.ProviderId,
                        poc.TradingName,
                          
                        -- Used to make sure multiple and national locations are at the end / beginning  
                        -- and sub sorted by multiple locations then national  
                        1 AS SingleOrMultipleLocationSortOrder,  
                        1 AS SubSingleOrMultipleLocationSortOrder                           
                    FROM
						dbo.Vacancy  v 
						INNER JOIN @TempResultContractOwner cmanager ON v.VacancyId = cmanager.VacancyId
						INNER JOIN dbo.Provider P ON cmanager.ProviderID = P.ProviderID  
						INNER JOIN dbo.[VacancyOwnerRelationship] rel ON v.[VacancyOwnerRelationshipId] = rel.[VacancyOwnerRelationshipId]  
						INNER JOIN dbo.ProviderSite PS ON rel.ProviderSiteID = PS.ProviderSiteID  
						INNER JOIN dbo.Employer emp ON rel.EmployerID = emp.EmployerID  
						LEFT JOIN @AllLocalAuthority LA ON v.LocalAuthorityId = LA.LocalAuthorityId  
						INNER JOIN dbo.VacancyStatusType vst ON vst.VacancyStatusTypeId = v.VacancyStatusId  
						LEFT JOIN ApprenticeshipFramework af ON af.ApprenticeshipFrameworkId = v.ApprenticeshipFrameworkId  
						LEFT JOIN VacancyLocationType vlt ON vlt.VacancyLocationTypeId = v.VacancyLocationTypeId   
						LEFT JOIN ProviderSite PSD ON PSD.ProviderSiteId = v.DeliveryOrganisationID
						LEFT JOIN ProviderSite PSV ON PSV.ProviderSiteId = v.VacancyManagerID
						LEFT JOIN Provider POC ON POC.ProviderId = v.OriginalContractOwnerId
						LEFT JOIN Provider PC ON PC.ProviderId = v.ContractOwnerId	
					
/*******************NAT/MULT VACANCIES ***********************************************************/
						
DECLARE @ResultsNatMultVacancy TABLE  
    (  
       
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
        
        ContractOwnerId INT,
        ContractOwnerTradingName NVARCHAR (200),  
        VacancyManagerId INT,
        VacancyManager NVARCHAR (200),
        VacancyManagerTown NVARCHAR (200),
        DelivererId INT,  
        Deliverer NVARCHAR (200),
        DelivererTown NVARCHAR (200),    
		OriginalContractOwnerId INT,
		OriginalContractOwner NVARCHAR (200),

		SingleOrMultipleLocationSortOrder NVARCHAR (20),  
        SubSingleOrMultipleLocationSortOrder NVARCHAR (20)           
		 )
		
INSERT INTO		@ResultsNatMultVacancy 		 
SELECT   DISTINCT
                        ps.ProviderSiteID,  
                        ps.FullName AS TrainingProviderFullName,  
                        ps.TradingName AS TrainingProviderTradingName,  
                        ps.WebPage AS TrainingProviderWebPage,  
                        ps.Town AS TrainingProviderTown,  
                        ps.PostCode AS TrainingProviderPostCode,  
                        ps.TrainingProviderStatusTypeId,  
  
                        LA.LocalAuthorityGroupID AS ManagingAreaId,  
                        LA.CodeName AS ManagingAreaCodeName,  
                        LA.FullName AS ManagingAreaFullName,  
                        LA.ShortName AS ManagingAreaShortName,  
  
                        emp.EmployerId,  
                        emp.FullName AS EmployerFullName,  
                        emp.TradingName AS EmployerTradingName,  
                        emp.OwnerOrgnistaion AS EmployerOwnerOrganisation,  
                        emp.Town AS EmployerTown,  
                        emp.PostCode AS EmployerPostCode,  
  
                        v.VacancyId,  
                        v.MasterVacancyId AS VacancyMasterVacancyId,  
                        v.PostCode AS VacancyPostCode,  
                        v.ShortDescription AS VacancyShortDescription,  
                        v.Title AS VacancyTitle,  
                        v.VacancyReferenceNumber,  
  
                        vst.VacancyStatusTypeId,  
                        vst.CodeName AS VacancyStatusCodeName,  
                        vst.FullName AS VacancyStatusFullName,  
                        vst.ShortName AS VacancyStatusShortName,  
                        v.Town AS VacancyLocation,  
                        af.ApprenticeshipFrameworkId,  
                        af.CodeName AS ApprenticeFrameworkCodeName,  
                        af.ShortName AS ApprenticeFrameworkShortName,  
                        af.FullName AS ApprenticeFrameworkFullName,  
                          
                        vlt.CodeName AS VacancyLocationCode,  
                        vlt.ShortName AS VacancyLocationShortName,  
                         --This is used to display the town on the UI as it uses the   
                         --VacancyLocationFullName to display town, multi-location or nationwide  
                        v.Town AS VacancyLocationFullName,  
                        
                        pc.ProviderId AS ContractOwnwerId,
                        pc.TradingName AS ContractOwnerTradingName, 
                        psv.ProviderSiteID AS VacancyManagerId,  
                        psv.TradingName AS VacancyManagerFullName,  
                        psv.Town,
                        psd.ProviderSiteID AS DelivererID,  
                        psd.TradingName AS DelivererFullName, 
                        psd.Town,
                        poc.ProviderId,
						poc.TradingName,
 
                          
                         --Used to make sure multiple and national locations are at the end / beginning  
                         --and sub sorted by multiple locations then national  
                        1 AS SingleOrMultipleLocationSortOrder,  
                        1 AS SubSingleOrMultipleLocationSortOrder                           
                    FROM
						dbo.Vacancy  v 
						INNER JOIN @TempResultNatDeliverer cmanager ON v.VacancyId = cmanager.VacancyId
						INNER JOIN dbo.Provider P ON cmanager.ProviderID = P.ProviderID  
						INNER JOIN dbo.ProviderSite PSD ON cmanager.ProviderSiteID = PSD.ProviderSiteID  
						INNER JOIN dbo.[VacancyOwnerRelationship] rel ON v.[VacancyOwnerRelationshipId] = rel.[VacancyOwnerRelationshipId]  
						INNER JOIN dbo.Employer emp ON rel.EmployerID = emp.EmployerID  
						LEFT JOIN @AllLocalAuthority LA ON v.LocalAuthorityId = LA.LocalAuthorityId   
						INNER JOIN dbo.VacancyStatusType vst ON vst.VacancyStatusTypeId = v.VacancyStatusId  
						LEFT JOIN ApprenticeshipFramework af ON af.ApprenticeshipFrameworkId = v.ApprenticeshipFrameworkId  
						INNER JOIN VacancyLocationType vlt ON vlt.VacancyLocationTypeId = v.VacancyLocationTypeId   
						LEFT JOIN ProviderSite PSV ON PSV.ProviderSiteId = v.VacancyManagerID
						INNER JOIN ProviderSite PS ON PS.ProviderSiteId = rel.ProviderSiteId
						LEFT JOIN Provider POC ON POC.ProviderId = v.OriginalContractOwnerId
						LEFT JOIN Provider PC ON PC.ProviderId = v.ContractOwnerId	
UNION

SELECT   DISTINCT
                        ps.ProviderSiteID,  
                        ps.FullName AS TrainingProviderFullName,  
                        ps.TradingName AS TrainingProviderTradingName,  
                        ps.WebPage AS TrainingProviderWebPage,  
                        ps.Town AS TrainingProviderTown,  
                        ps.PostCode AS TrainingProviderPostCode,  
                        ps.TrainingProviderStatusTypeId,  
  
                        LA.LocalAuthorityGroupID AS ManagingAreaId,  
                        LA.CodeName AS ManagingAreaCodeName,  
                        LA.FullName AS ManagingAreaFullName,  
                        LA.ShortName AS ManagingAreaShortName,  
  
                        emp.EmployerId,  
                        emp.FullName AS EmployerFullName,  
                        emp.TradingName AS EmployerTradingName,  
                        emp.OwnerOrgnistaion AS EmployerOwnerOrganisation,  
                        emp.Town AS EmployerTown,  
                        emp.PostCode AS EmployerPostCode,  
  
                        v.VacancyId,  
                        v.MasterVacancyId AS VacancyMasterVacancyId,  
                        v.PostCode AS VacancyPostCode,  
                        v.ShortDescription AS VacancyShortDescription,  
                        v.Title AS VacancyTitle,  
                        v.VacancyReferenceNumber,  
  
                        vst.VacancyStatusTypeId,  
                        vst.CodeName AS VacancyStatusCodeName,  
                        vst.FullName AS VacancyStatusFullName,  
                        vst.ShortName AS VacancyStatusShortName,  
                        v.Town AS VacancyLocation,  
                        af.ApprenticeshipFrameworkId,  
                        af.CodeName AS ApprenticeFrameworkCodeName,  
                        af.ShortName AS ApprenticeFrameworkShortName,  
                        af.FullName AS ApprenticeFrameworkFullName,  
                          
                        vlt.CodeName AS VacancyLocationCode,  
                        vlt.ShortName AS VacancyLocationShortName,  
                        -- This is used to display the town on the UI as it uses the   
                        -- VacancyLocationFullName to display town, multi-location or nationwide  
                        v.Town AS VacancyLocationFullName,  
                        
                        pc.ProviderId AS ContractOwnerId,
                        pc.TradingName AS ContractOwnerTradingName, 
                        psv.ProviderSiteID AS VacancyManagerId,  
                        psv.TradingName AS VacancyManagerFullName,  
                        psv.Town,
                        psd.ProviderSiteID AS DelivererID,  
                        psd.TradingName AS DelivererFullName,  
                        psd.Town,
                        poc.ProviderId,
						poc.TradingName,
  
                        -- Used to make sure multiple and national locations are at the end / beginning  
                        -- and sub sorted by multiple locations then national  
                        1 AS SingleOrMultipleLocationSortOrder,  
                        1 AS SubSingleOrMultipleLocationSortOrder                           
                    FROM
						dbo.Vacancy  v 
						INNER JOIN @TempResultNatVacancyOwner cmanager ON v.VacancyId = cmanager.VacancyId
						INNER JOIN dbo.Provider P ON cmanager.ProviderID = P.ProviderID  
						INNER JOIN dbo.ProviderSite PS ON cmanager.ProviderSiteID = PS.ProviderSiteID  
						INNER JOIN dbo.[VacancyOwnerRelationship] rel ON v.[VacancyOwnerRelationshipId] = rel.[VacancyOwnerRelationshipId]  
						INNER JOIN dbo.Employer emp ON rel.EmployerID = emp.EmployerID  
						LEFT JOIN @AllLocalAuthority LA ON v.LocalAuthorityId = LA.LocalAuthorityId  
						INNER JOIN dbo.VacancyStatusType vst ON vst.VacancyStatusTypeId = v.VacancyStatusId  
						LEFT  JOIN ApprenticeshipFramework af ON af.ApprenticeshipFrameworkId = v.ApprenticeshipFrameworkId  
						INNER JOIN VacancyLocationType vlt ON vlt.VacancyLocationTypeId = v.VacancyLocationTypeId   
						LEFT JOIN ProviderSite PSD ON PSD.ProviderSiteId = v.DeliveryOrganisationID
						LEFT JOIN ProviderSite PSV ON PSV.ProviderSiteId = v.VacancyManagerID
						LEFT JOIN Provider POC ON POC.ProviderId = v.OriginalContractOwnerId
						LEFT JOIN Provider PC ON PC.ProviderId = v.ContractOwnerId	
						
UNION

SELECT   DISTINCT
                        ps.ProviderSiteID,  
                        ps.FullName AS TrainingProviderFullName,  
                        ps.TradingName AS TrainingProviderTradingName,  
                        ps.WebPage AS TrainingProviderWebPage,  
                        ps.Town AS TrainingProviderTown,  
                        ps.PostCode AS TrainingProviderPostCode,  
                        ps.TrainingProviderStatusTypeId,  
  
                        LA.LocalAuthorityGroupID AS ManagingAreaId,  
                        LA.CodeName AS ManagingAreaCodeName,  
                        LA.FullName AS ManagingAreaFullName,  
                        LA.ShortName AS ManagingAreaShortName,  
  
                        emp.EmployerId,  
                        emp.FullName AS EmployerFullName,  
                        emp.TradingName AS EmployerTradingName,  
                        emp.OwnerOrgnistaion AS EmployerOwnerOrganisation,  
                        emp.Town AS EmployerTown,  
                        emp.PostCode AS EmployerPostCode,  
  
                        v.VacancyId,  
                        v.MasterVacancyId AS VacancyMasterVacancyId,  
                        v.PostCode AS VacancyPostCode,  
                        v.ShortDescription AS VacancyShortDescription,  
                        v.Title AS VacancyTitle,  
                        v.VacancyReferenceNumber,  
  
                        vst.VacancyStatusTypeId,  
                        vst.CodeName AS VacancyStatusCodeName,  
                        vst.FullName AS VacancyStatusFullName,  
                        vst.ShortName AS VacancyStatusShortName,  
                        v.Town AS VacancyLocation,  
                        af.ApprenticeshipFrameworkId,  
                        af.CodeName AS ApprenticeFrameworkCodeName,  
                        af.ShortName AS ApprenticeFrameworkShortName,  
                        af.FullName AS ApprenticeFrameworkFullName,  
                          
                        vlt.CodeName AS VacancyLocationCode,  
                        vlt.ShortName AS VacancyLocationShortName,  
                        -- This is used to display the town on the UI as it uses the   
                        -- VacancyLocationFullName to display town, multi-location or nationwide  
                        v.Town AS VacancyLocationFullName,  
                        
                        pc.ProviderId AS ContractOwnwerId,
                        pc.TradingName AS ContractOwnerTradingName, 
                        psv.ProviderSiteID AS VacancyManagerId,  
                        psv.TradingName AS VacancyManagerFullName,  
                        psv.Town,
                        psd.ProviderSiteID AS DelivererID,  
                        psd.TradingName AS DelivererFullName,  
                        psd.Town,  
						poc.ProviderId,
						poc.TradingName,
                          
                        -- Used to make sure multiple and national locations are at the end / beginning  
                        -- and sub sorted by multiple locations then national  
                        1 AS SingleOrMultipleLocationSortOrder,  
                        1 AS SubSingleOrMultipleLocationSortOrder                           
                    FROM
						dbo.Vacancy  v 
						INNER JOIN @TempResultNatContractOwner cmanager ON v.VacancyId = cmanager.VacancyId
						INNER JOIN dbo.Provider P ON cmanager.ProviderID = P.ProviderID  
						INNER JOIN dbo.[VacancyOwnerRelationship] rel ON v.[VacancyOwnerRelationshipId] = rel.[VacancyOwnerRelationshipId]  
						INNER JOIN dbo.ProviderSite PS ON rel.ProviderSiteID = PS.ProviderSiteID  
						INNER JOIN dbo.Employer emp ON rel.EmployerID = emp.EmployerID  
						LEFT JOIN @AllLocalAuthority LA ON v.LocalAuthorityId = LA.LocalAuthorityId   
						INNER JOIN dbo.VacancyStatusType vst ON vst.VacancyStatusTypeId = v.VacancyStatusId  
						LEFT JOIN ApprenticeshipFramework af ON af.ApprenticeshipFrameworkId = v.ApprenticeshipFrameworkId  
						INNER JOIN VacancyLocationType vlt ON vlt.VacancyLocationTypeId = v.VacancyLocationTypeId   
						LEFT JOIN ProviderSite PSD ON PSD.ProviderSiteId = v.DeliveryOrganisationID
						LEFT JOIN ProviderSite PSV ON PSV.ProviderSiteId = v.VacancyManagerID
						LEFT JOIN Provider POC ON POC.ProviderId = v.OriginalContractOwnerId
						LEFT JOIN Provider PC ON PC.ProviderId = v.ContractOwnerId	
						
						SELECT CASE @sortByField  
                            WHEN  '1' THEN TrainingProviderTradingName  
                            WHEN  '2' THEN TrainingProviderFullName  
                            WHEN  '3' THEN TrainingProviderTown    
                            WHEN  '4' THEN EmployerTradingName 
                            WHEN  '5' THEN EmployerFullName  
                            WHEN  '6' THEN EmployerOwnerOrganisation  
                            WHEN  '7' THEN EmployerTown  
                            WHEN  '8' THEN VacancyTitle  
                            WHEN  '9' THEN CONVERT(NVARCHAR(10), VacancyReferenceNumber)  
                            WHEN '10' THEN VacancyStatusFullName 
                            WHEN '11' THEN VacancyStatusShortName   
                            WHEN '12' THEN VacancyLocation  
							WHEN '13' THEN ContractOwnerTradingName
							WHEN '14' THEN VacancyManager
							WHEN '15' THEN Deliverer
                            ELSE EmployerTradingName  
                        END AS SortByFieldText,  
						* FROM @ResultsStandardVacancy rsv               
						UNION
						
							SELECT CASE @sortByField  
                            WHEN  '1' THEN TrainingProviderTradingName  
                            WHEN  '2' THEN TrainingProviderFullName  
                            WHEN  '3' THEN TrainingProviderTown    
                            WHEN  '4' THEN EmployerTradingName 
                            WHEN  '5' THEN EmployerFullName  
                            WHEN  '6' THEN EmployerOwnerOrganisation  
                            WHEN  '7' THEN EmployerTown  
                            WHEN  '8' THEN VacancyTitle  
                            WHEN  '9' THEN CONVERT(NVARCHAR(10), VacancyReferenceNumber)  
                            WHEN '10' THEN VacancyStatusCodeName  
                            WHEN '11' THEN VacancyStatusFullName 
                            WHEN '12' THEN VacancyStatusShortName  
                            WHEN '13' THEN VacancyStatusFullName  
                            WHEN '14' THEN VacancyLocation  
							WHEN '15' THEN  ContractOwnerTradingName
							WHEN '16' THEN VacancyManager
							WHEN '17' THEN Deliverer
                            ELSE EmployerTradingName  
                        END AS SortByFieldText,  
						* from @ResultsNatMultVacancy rnmv
           
  END TRY  
    BEGIN CATCH  
        EXEC RethrowError;  
    END CATCH  
  
    SET NOCOUNT OFF  
END