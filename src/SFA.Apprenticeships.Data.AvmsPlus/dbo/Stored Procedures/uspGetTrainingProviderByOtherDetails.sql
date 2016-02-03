CREATE PROCEDURE [dbo].[uspGetTrainingProviderByOtherDetails]
@FrameworkId INT=0, 
@Occupation INT=0, 
@GeographicRegionId INT=0, 
@LocalAuthorityId INT=0, 
@NationalOnly BIT=0,
@PageIndex INT=1, 
@PageSize INT=20, 
@IsSortAsc BIT=1, 
@SortByField NVARCHAR (100)='TradingName',
@ApprenticeType INT=0
AS
BEGIN

	SET NOCOUNT ON

	/*********Set Page Number**********************************************/    
	declare @StartRowNo int    
	declare @EndRowNo int    
	set @StartRowNo =((@PageIndex-1)* @PageSize)+1     
	set @EndRowNo =(@PageIndex * @PageSize)        
	/***********************************************************************/    
	/*********set the order by**********************************************/    
	declare @OrderBywithSort varchar(500)    
	if @SortByField = 'TradingName'    
	Begin     
	 if @IsSortAsc = 1 BEGIN set  @SortByField = 'TrainingProviderName Asc' END    
	 if @IsSortAsc = 0 BEGIN  set  @SortByField = 'TrainingProviderName desc' END    
	End

	if @SortByField = 'PassRate'    
	Begin     
	 if @IsSortAsc = 1 BEGIN set  @SortByField = 'PassRate Asc' END    
	 if @IsSortAsc = 0 BEGIN  set  @SortByField = 'PassRate desc' END    
	End

	DECLARE @TotalRows int
	DECLARE @SearchResults TABLE
	(
		ProviderSiteID INT NOT NULL,
		FullName NVARCHAR(255) NOT NULL,
		TradingName NVARCHAR(255) NOT NULL,
		AddressLine1 NVARCHAR(50) NOT NULL,
		AddressLine2 NVARCHAR(50) NULL,
		AddressLine3 NVARCHAR(50) NULL,
		AddressLine4 NVARCHAR(50) NULL,
		AddressLine5 NVARCHAR(50) NULL,
		Town NVARCHAR(50) NOT NULL,
		CountyId INT NOT NULL,
		PostCode NVARCHAR(8) NOT NULL,
		WebPage  NVARCHAR(100) NULL,
		Relationship  NVARCHAR(100) NOT NULL,
		[Status]  NVARCHAR(100) NOT NULL,
		ProviderID INT NOT NULL,
		ProviderTradingName NVARCHAR(255) NULL,
		IsContracted BIT NOT NULL,
		PassRate SMALLINT NOT NULL,
		NewRecord BIT NOT NULL
	);
	
	-- perfrom the search
	INSERT INTO @SearchResults (
		ProviderSiteID,
		FullName,
		TradingName,
		AddressLine1,
		AddressLine2,
		AddressLine3,
		AddressLine4,
		AddressLine5,
		Town,
		CountyId,
		PostCode,
		WebPage,
		Relationship, 
		Status,
		ProviderID,
		ProviderTradingName,
		IsContracted,
		PassRate,
		NewRecord)
	SELECT 
		tp.ProviderSiteID,
		tp.FullName,
		tp.TradingName,
		tp.AddressLine1,
		tp.AddressLine2,
		tp.AddressLine3,
		tp.AddressLine4,
		tp.AddressLine5,
		tp.Town,
		tp.CountyId,
		tp.PostCode,
		tp.WebPage,
		'Owner' as 'Relationship',
		tps.FullName as 'Status',
		p.ProviderID,
		p.TradingName as 'ProviderTradingName',
		p.IsContracted as 'IsContracted',
		ISNULL(ssr.PassRate, 0) as Passrate,
		ISNULL(ssr.new, 1)  as 'NewRecord'
   FROM [ProviderSite] tp
		INNER JOIN ProviderSiteRelationship psr ON tp.ProviderSIteID = PSR.ProviderSiteID AND psr.ProviderSiteRelationShipTypeID = 1
		INNER JOIN Provider P ON psr.ProviderID = p.ProviderID
		INNER JOIN [ProviderSiteFramework] tpf ON psr.ProviderSiteRelationshipID = tpf.ProviderSiteRelationshipID
		INNER JOIN ApprenticeshipFramework af ON tpf.FrameworkId = af.ApprenticeshipFrameworkId
		INNER JOIN ApprenticeshipOccupation ao ON ao.ApprenticeshipOccupationId = af.ApprenticeshipOccupationId
		INNER JOIN [ProviderSiteLocalAuthority] tpl ON  psr.ProviderSiteRelationshipID = tpl.[ProviderSiteRelationshipID]
		LEFT JOIN vwRegionsAndLocalAuthority vwRLA ON tpl.LocalAuthorityId = vwRLA.LocalAuthorityId
		INNER JOIN [ProviderSiteOffer] tpo ON tpf.ProviderSiteFrameworkID = tpo.[ProviderSiteFrameworkID] AND tpl.ProviderSiteLocalAuthorityID = tpo.ProviderSiteLocalAuthorityID 
      	LEFT JOIN SectorSuccessRates ssr ON ssr.ProviderID = p.ProviderID AND ssr.SectorID = ao.ApprenticeshipOccupationId
		INNER JOIN EmployerTrainingProviderStatus tps ON tps.EmployerTrainingProviderStatusId = tp.TrainingProviderStatusTypeId
   WHERE
		tp.TrainingProviderStatusTypeId = 1 
		AND  tp.HideFromSearch = 0
		AND  tp.ProviderSiteID <> 0
		AND  (p.IsContracted = 1)
		AND p.IsNASProvider = 0
		AND  (
				(@GeographicRegionId = 0 AND @NationalOnly = 0)
			OR  (vwRLA.GeographicRegionId IS NULL AND @NationalOnly = 1)
			OR  (vwRLA.GeographicRegionId = @GeographicRegionId AND  (tpl.LocalAuthorityId IS NULL OR @LocalAuthorityId = 0))
			OR  (tpl.LocalAuthorityId = @LocalAuthorityId)
		)
		AND (ao.ApprenticeshipOccupationId = @Occupation OR @Occupation = 0)
		AND (tpf.FrameworkId = @FrameworkId OR @FrameworkId = 0)
		
		
				
		
		AND (
				(@ApprenticeType = 0)
			OR	(@ApprenticeType = 1 AND tpo.Apprenticeship = 1)
			OR 	(@ApprenticeType = 2 AND tpo.AdvancedApprenticeship = 1)
			OR 	(@ApprenticeType = 3 AND tpo.HigherApprenticeship = 1)
			)
		
		
		
	GROUP BY
		tp.ProviderSiteID, tp.FullName, tp.TradingName, 
		tp.AddressLine1, tp.AddressLine2, tp.AddressLine3, tp.AddressLine4, tp.AddressLine5, tp.Town, tp.CountyId, tp.PostCode,
		tp.WebPage, ssr.PassRate, ssr.new, tps.FullName, p.ProviderID, p.TradingName, p.IsContracted
	UNION
	SELECT 
		tp.ProviderSiteID,
		tp.FullName,
		tp.TradingName,
		tp.AddressLine1,
		tp.AddressLine2,
		tp.AddressLine3,
		tp.AddressLine4,
		tp.AddressLine5,
		tp.Town,
		tp.CountyId,
		tp.PostCode,
		tp.WebPage,
		'Subcontractor' as 'Relationship',
		tps.FullName as 'Status',
		p.ProviderID,
		p.TradingName as 'ProviderTradingName',
		p.IsContracted as 'IsContracted',
		ISNULL(ssr.PassRate, 0) as Passrate,
		ISNULL(ssr.new, 1)  as 'NewRecord'
   FROM [ProviderSite] tp
		INNER JOIN ProviderSiteRelationship psrSc ON tp.ProviderSIteID = psrSc.ProviderSiteID AND psrSc.ProviderSiteRelationShipTypeID = 2
		INNER JOIN Provider P ON psrSc.ProviderID = p.ProviderID
		INNER JOIN [ProviderSiteFramework] tpf ON psrSc.ProviderSiteRelationshipID = tpf.ProviderSiteRelationshipID
		INNER JOIN ApprenticeshipFramework af ON tpf.FrameworkId = af.ApprenticeshipFrameworkId
		INNER JOIN ApprenticeshipOccupation ao ON ao.ApprenticeshipOccupationId = af.ApprenticeshipOccupationId
		INNER JOIN [ProviderSiteLocalAuthority] tpl ON  psrSc.ProviderSiteRelationshipID = tpl.[ProviderSiteRelationshipID]
		LEFT JOIN vwRegionsAndLocalAuthority vwRLA ON tpl.LocalAuthorityId = vwRLA.LocalAuthorityId
		INNER JOIN [ProviderSiteOffer] tpo ON tpf.ProviderSiteFrameworkID = tpo.[ProviderSiteFrameworkID] AND tpl.ProviderSiteLocalAuthorityID = tpo.ProviderSiteLocalAuthorityID 
      	LEFT JOIN SectorSuccessRates ssr ON ssr.ProviderID = p.ProviderID AND ssr.SectorID = ao.ApprenticeshipOccupationId
		INNER JOIN EmployerTrainingProviderStatus tps ON tps.EmployerTrainingProviderStatusId = tp.TrainingProviderStatusTypeId
   
   WHERE
		tp.TrainingProviderStatusTypeId = 1 
		AND p.ProviderStatusTypeID = 1 
		AND  tp.HideFromSearch = 0
		AND  tp.ProviderSiteID <> 0
		AND  (p.IsContracted = 1)
		AND p.IsNASProvider = 0
		AND  (
				(@GeographicRegionId = 0 AND @NationalOnly = 0)
			OR  (vwRLA.GeographicRegionId IS NULL AND @NationalOnly = 1)
			OR  (vwRLA.GeographicRegionId = @GeographicRegionId AND  (tpl.LocalAuthorityId IS NULL OR @LocalAuthorityId = 0))
			OR  (tpl.LocalAuthorityId = @LocalAuthorityId)

		)
		AND (ao.ApprenticeshipOccupationId = @Occupation OR @Occupation = 0)
		AND (tpf.FrameworkId = @FrameworkId OR @FrameworkId = 0)
		
		
		AND (
				(@ApprenticeType = 0)
			OR	(@ApprenticeType = 1 AND tpo.Apprenticeship = 1)
			OR 	(@ApprenticeType = 2 AND tpo.AdvancedApprenticeship = 1)
			OR 	(@ApprenticeType = 3 AND tpo.HigherApprenticeship = 1)
			)
		
		
	GROUP BY
		tp.ProviderSiteID, tp.FullName, tp.TradingName, 
		tp.AddressLine1, tp.AddressLine2, tp.AddressLine3, tp.AddressLine4, tp.AddressLine5, tp.Town, tp.CountyId, tp.PostCode,
		tp.WebPage, ssr.PassRate, ssr.new, tps.FullName, p.ProviderID, p.TradingName, p.IsContracted
	
	
	/**************Total Number of Rows*************************************/    
	SELECT @TotalRows = count(*)  
	FROM @SearchResults

	SELECT * FROM 
	(	SELECT ProviderSiteID,
		ROW_NUMBER() OVER( ORDER BY     
            Case when @SortByField='TrainingProviderName Asc'  then TradingName End ASC,    
            Case when @SortByField='TrainingProviderName Desc'  then TradingName End DESC, 
            Case when @SortByField='PassRate Asc'  then PassRate End ASC,    
            Case when @SortByField='PassRate Desc'  then PassRate End DESC,     
         TradingName ASC   ) as RowNum,
		FullName,
		TradingName,
		AddressLine1,
		AddressLine2,
		AddressLine3,
		AddressLine4,
		AddressLine5,
		Town,
		CountyId,
		PostCode,
		WebPage,
		Relationship,
		Status,
		ProviderID,
		ProviderTradingName,
		IsContracted,
		PassRate,
		NewRecord,
		@TotalRows  as 'TotalRows'
	FROM @SearchResults) as results
	WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo   
	
	SET NOCOUNT OFF
END