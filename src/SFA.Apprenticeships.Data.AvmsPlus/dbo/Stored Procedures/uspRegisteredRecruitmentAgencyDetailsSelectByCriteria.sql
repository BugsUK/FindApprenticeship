CREATE PROCEDURE [dbo].[uspRegisteredRecruitmentAgencyDetailsSelectByCriteria]
	@Name NVARCHAR(255) = null,
	@URN int = null,
	@Town NVARCHAR(50) = null,
	@Postcode NVARCHAR(8) = null,
	@VacancyOwnerId int,
	@PageNo INT=1, 
	@PageSize INT=10, 
	@IsSortAsc BIT=1, 
	@SortByField NVARCHAR (100)='TradingName'
AS
BEGIN              
	-- SET NOCOUNT ON added to prevent extra result sets from              
	-- interfering with SELECT statements.              
	SET NOCOUNT ON;              
   
   -- get the provider id for the vacancy owner
	DECLARE @providerId int
	SELECT @providerId = ProviderID
	FROM ProviderSiteRelationship 
	WHERE ProviderSiteID=@VacancyOwnerId AND ProviderSiteRelationShipTypeID = 1


	/*********Set Page Number**********************************************/                  
	declare @StartRowNo int                  
	declare @EndRowNo int                  
	set @StartRowNo =((@PageNo-1)* @PageSize)+1                   
	set @EndRowNo =(@PageNo * @PageSize)                      
	/***********************************************************************/   

	/*********set the order by**********************************************/                  
	 if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END                  
	 if @IsSortAsc = 0 BEGIN  set  @SortByField = @SortByField + ' desc' END             
	/***********************************************************************/   
	
	-- nullify empty search parameters
	IF(@Name = '') SET @Name = NULL
	IF(@URN = 0) SET @URN = NULL
	IF(@Town = '') SET @Town = NULL
	IF(@Postcode = '') SET @Postcode = NULL


	DECLARE @results TABLE
	(
		RowNum INT,
		ProviderSiteID INT,
		EdsUrn	INT,
		ActiveVacancies	INT,
		FullName NVARCHAR(255),
		TradingName NVARCHAR(255),
		OwnerOrgnistaion NVARCHAR(255),
		AddressLine1 NVARCHAR(50),
		AddressLine2 NVARCHAR(50),
		AddressLine3 NVARCHAR(50),
		AddressLine4 NVARCHAR(50),
		Town NVARCHAR(50),
		County NVARCHAR(150),
		PostCode NVARCHAR(8),
		Linked BIT,
		NumLinkedEmployers INT
	);


	INSERT INTO @results
	SELECT	ROW_NUMBER() OVER( ORDER BY                   
				CASE WHEN @SortByField='TradingName Asc'  then TradingName End ASC,                  
				CASE WHEN @SortByField='TradingName desc'  then TradingName End DESC               
			) as RowNum,
			ProviderSite.ProviderSiteID, 
			ProviderSite.EdsUrn  AS 'EdsUrn',
			COUNT(distinct VacancyID) AS ActiveVacancies,
			isnull(ProviderSite.FullName,'') AS 'FullName', 
			isnull(ProviderSite.TradingName,'') AS 'TradingName', 
			isnull(ProviderSite.OwnerOrganisation,'') as 'OwnerOrgnistaion', 
            isnull(ProviderSite.AddressLine1,'') as 'AddressLine1', 
            isnull(ProviderSite.AddressLine2,'') as 'AddressLine2',  
            isnull(ProviderSite.AddressLine3,'') as 'AddressLine3', 
            isnull(ProviderSite.AddressLine4,'') as 'AddressLine4', 
            isnull(ProviderSite.Town,'') as 'Town',  
            County.FullName AS County, 
            ProviderSite.PostCode,
            CASE WHEN EXISTS (Select 1 from ProviderSiteRelationship PSR WHERE PSR.ProvidersiteRelationshipTypeID = 3 AND PSR.ProviderID = @providerId AND PSR.ProviderSiteID = ProviderSite.ProviderSiteID )
                      THEN 1
                      ELSE 0
                      END AS Linked,
			ISNULL(TempEmployer.NumLinkedEmployers, 0) as 'NumLinkedEmployers'
	FROM  ProviderSite 
		INNER JOIN County ON County.CountyId = ProviderSite.CountyId 
		LEFT JOIN ProviderSiteRelationship ON ProviderSiteRelationship.ProviderSiteID = ProviderSite.ProviderSiteID AND ProviderSiteRelationship.ProviderSiteRelationShipTypeID = 3 AND ProviderSiteRelationship.ProviderID = @providerId 
		LEFT OUTER JOIN 
				(
					SELECT Vacancy.VacancyID, Vacancy.VacancyManagerID, VacancyOwnerRelationship.ProviderSiteID, ProviderSiteRelationship.ProviderID 
					FROM Vacancy 
						INNER JOIN VacancyOwnerRelationship 
							ON VacancyOwnerRelationship.VacancyOwnerRelationshipId = Vacancy.VacancyOwnerRelationshipId 
						INNER JOIN ProviderSiteRelationship 
							ON ProviderSiteRelationship.ProviderSiteID = VacancyOwnerRelationship.ProviderSiteID AND ProviderSiteRelationship.ProviderSiteRelationShipTypeID = 1
						WHERE Vacancy.VacancyStatusId in (1,2,3,5)
				) TempVacancy ON TempVacancy.VacancyManagerID = ProviderSite.ProviderSiteID AND TempVacancy.ProviderID = @providerId
		LEFT OUTER JOIN 
				(
					SELECT ProviderSiteRelationshipID, COUNT(DISTINCT vor.EmployerId) as 'NumLinkedEmployers'
					FROM RecruitmentAgentLinkedRelationships ralr
						INNER JOIN VacancyOwnerRelationship vor ON vor.VacancyOwnerRelationshipId = ralr.VacancyOwnerRelationshipID
					GROUP BY ProviderSiteRelationshipID
						
				) TempEmployer ON TempEmployer.ProviderSiteRelationshipID = ProviderSiteRelationship.ProviderSiteRelationshipID
				               
	WHERE     
		(ProviderSite.IsRecruitmentAgency = 1) 
		AND ProviderSite.TrainingProviderStatusTypeId = 1
		AND (@Name IS NULL OR ProviderSite.TradingName LIKE  '%' + @Name + '%' OR ProviderSite.FullName LIKE  '%' + @Name + '%')
		AND (@Town IS NULL OR ProviderSite.Town LIKE  '%' + @Town + '%')
		AND (@Postcode IS NULL OR ProviderSite.PostCode LIKE  '%' + @Postcode + '%')
		AND (@URN IS NULL OR ProviderSite.EDSURN = @URN)
	GROUP BY ProviderSite.FullName, ProviderSite.TradingName, ProviderSite.OwnerOrganisation, ProviderSite.AddressLine1, ProviderSite.AddressLine2, 
                      ProviderSite.AddressLine3, ProviderSite.AddressLine4, ProviderSite.AddressLine5, ProviderSite.Town, County.FullName, 
                      ProviderSite.PostCode, ProviderSite.ProviderSiteID,ProviderSite.EdsUrn, TempEmployer.NumLinkedEmployers
                      
	-- total rows                      
	DECLARE @TotalRows int            
	SELECT @TotalRows= COUNT(*) FROM @results   
	
	
	SELECT @TotalRows AS 'TotalRow', *
	FROM @results
	WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo  
END