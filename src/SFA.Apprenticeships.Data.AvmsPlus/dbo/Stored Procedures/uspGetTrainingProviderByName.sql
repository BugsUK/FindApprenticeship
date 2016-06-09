CREATE PROCEDURE [dbo].[uspGetTrainingProviderByName]
@ProviderName NVARCHAR (500), 
@PageIndex INT=1, 
@PageSize INT=20, 
@IsSortAsc BIT=1, 
@SortByField NVARCHAR (100)='TradingName'
AS
BEGIN
	SET NOCOUNT ON

	SET @SortByField = 'TrainingProviderName'

	/*********Set Page Number**********************************************/    
	declare @StartRowNo int    
	declare @EndRowNo int    
	set @StartRowNo =((@PageIndex-1)* @PageSize)+1     
	set @EndRowNo =(@PageIndex * @PageSize)        
	/***********************************************************************/    
	/*********set the order by**********************************************/    
	declare @OrderBywithSort varchar(500)    
	if @SortByField = 'TrainingProviderName'    
	Begin     
	 if @IsSortAsc = 1 BEGIN set  @SortByField = 'TrainingProviderName Asc' END    
	 if @IsSortAsc = 0 BEGIN  set  @SortByField = 'TrainingProviderName desc' END    
	End 

	/**************CCR11193*************************************************/

	declare @ProviderName2 nvarchar(500)

	if @ProviderName = 'and'
		set @ProviderName2 = '&'
	else if @ProviderName = '&'
		set @ProviderName2 = ' and '
	else if charindex('&', @ProviderName) > 0
		set @ProviderName2 = replace(@ProviderName, '&', 'and')
	else if charindex('and', @ProviderName) > 0
		set @ProviderName2 = replace(@ProviderName, 'and', '&')

	/***********************************************************************/  

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
		IsContracted BIT NOT NULL
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
		IsContracted)
	SELECT
		tp.ProviderSiteID
		,tp.FullName
		,tp.TradingName
		,tp.AddressLine1
		,tp.AddressLine2
		,tp.AddressLine3
		,tp.AddressLine4
		,tp.AddressLine5
		,tp.Town
		,tp.CountyId
		,tp.PostCode
		,tp.WebPage
		,'Owner' as 'Relationship'
		,tps.FullName as 'Status'
		,p.ProviderID
		,p.TradingName as 'ProviderTradingName'
		,p.IsContracted
	FROM [ProviderSite] tp
		INNER JOIN ProviderSiteRelationship psr ON tp.ProviderSiteID = PSR.ProviderSiteID AND psr.ProviderSiteRelationShipTypeID = 1
		INNER JOIN Provider P ON psr.ProviderID = p.ProviderID
		INNER JOIN EmployerTrainingProviderStatus tps ON tps.EmployerTrainingProviderStatusId = tp.TrainingProviderStatusTypeId
	WHERE
		(
			((tp.TradingName LIKE  '%' + @ProviderName +  '%') OR (tp.TradingName LIKE  '%' + @ProviderName2 +  '%')) 
			Or
			((tp.FullName LIKE  '%' + @ProviderName +  '%') OR (tp.FullName LIKE  '%' + @ProviderName2 +  '%'))
		)
		AND tp.HideFromSearch =0
		AND tp.TrainingProviderStatusTypeId = 1 
		AND  p.IsContracted = 1
		AND p.IsNASProvider = 0
	UNION
	SELECT
		tp.ProviderSiteID
		,tp.FullName
		,tp.TradingName
		,tp.AddressLine1
		,tp.AddressLine2
		,tp.AddressLine3
		,tp.AddressLine4
		,tp.AddressLine5
		,tp.Town
		,tp.CountyId
		,tp.PostCode
		,tp.WebPage
		,'Subcontractor' as 'Relationship'
		,tps.FullName as 'Status'
		,p.ProviderID
		,p.TradingName as 'ProviderTradingName'
		,p.IsContracted
	FROM [ProviderSite] tp
		INNER JOIN ProviderSiteRelationship psr ON tp.ProviderSiteID = PSR.ProviderSiteID AND psr.ProviderSiteRelationShipTypeID = 2
		INNER JOIN Provider P ON psr.ProviderID = p.ProviderID
		INNER JOIN EmployerTrainingProviderStatus tps ON tps.EmployerTrainingProviderStatusId = tp.TrainingProviderStatusTypeId
	WHERE
		(
			((tp.TradingName LIKE  '%' + @ProviderName +  '%') OR (tp.TradingName LIKE  '%' + @ProviderName2 +  '%')) 
			Or
			((tp.FullName LIKE  '%' + @ProviderName +  '%') OR (tp.FullName LIKE  '%' + @ProviderName2 +  '%'))
			Or
			((p.TradingName LIKE  '%' + @ProviderName +  '%') OR (p.TradingName LIKE  '%' + @ProviderName2 +  '%')) 
			Or
			((p.FullName LIKE  '%' + @ProviderName +  '%') OR (p.FullName LIKE  '%' + @ProviderName2 +  '%'))
		)
		AND tp.HideFromSearch =0
		AND tp.TrainingProviderStatusTypeId = 1 
		AND p.ProviderStatusTypeID = 1 
		AND  p.IsContracted = 1
		AND p.IsNASProvider = 0
	
	/**************Total Number of Rows*************************************/    
	SELECT @TotalRows = count(*)  
	FROM @SearchResults

	SELECT * FROM 
	(	SELECT ProviderSiteID,
		ROW_NUMBER() OVER( ORDER BY     
				Case when @SortByField='TrainingProviderName Asc'  then TradingName End ASC,    
				Case when @SortByField='TrainingProviderName Desc'  then TradingName End DESC,     
			 TradingName ASC   ) as RowNum      ,
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
		@TotalRows  as 'TotalRows'
	FROM @SearchResults) as results
	WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo   
	
	SET NOCOUNT OFF
END