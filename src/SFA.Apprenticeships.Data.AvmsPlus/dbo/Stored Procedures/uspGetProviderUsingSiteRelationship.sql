CREATE PROCEDURE [dbo].[uspGetProviderUsingSiteRelationship]
	@id int,
	@relationshipTypeId int,
	@PageIndex INT=1, 
	@PageSize INT=20, 
	@IsSortAsc BIT=1, 
	@SortByField NVARCHAR (100)='TradingName'
AS
BEGIN
	
	SET NOCOUNT ON

	/* PAGING */
	declare @StartRowNo int                  
	declare @EndRowNo int                  
	set @StartRowNo =((@PageIndex-1)* @PageSize)+1                   
	set @EndRowNo =(@PageIndex * @PageSize)   

	/* GET TOTAL ROW COUNT */
	declare @TotalRows int     
	SELECT @TotalRows= count(1)  FROM Provider P
	JOIN ProviderSiteRelationship PSR ON PSR.ProviderId = P.ProviderID
	WHERE PSR.ProviderSiteID = @id AND PSR.ProviderSiteRelationShipTypeID = @relationshipTypeId

	/* SORTING */
	if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END            
	if @IsSortAsc = 0 BEGIN  set  @SortByField = @SortByField + ' desc' END  

	SELECT *, @TotalRows  as 'TotalRows' from     
	(SELECT ROW_NUMBER() OVER 
		(ORDER BY 
			Case when @SortByField='TradingName Asc'  then P.TradingName  End ASC,          
			Case when @SortByField='TradingName desc'  then P.TradingName  End DESC,
			Case when @SortByField='UKPRN Asc'  then P.UKPRN  End ASC,          
			Case when @SortByField='UKPRN desc'  then P.UKPRN  End DESC
		) AS RowNum, 
	P.ProviderID As 'ProviderId',
	P.UPIN As 'UPIN',
	P.UKPRN As 'UKPRN',
	P.FullName As 'FullName',
	P.TradingName As 'TradingName',
	P.IsContracted As 'IsContracted',
	P.ContractedFrom As 'ContractedFrom',
	P.ContractedTo As 'ContractedTo'
	FROM Provider P
	JOIN ProviderSiteRelationship PSR ON PSR.ProviderId = P.ProviderID
	WHERE PSR.ProviderSiteId = @id AND PSR.ProviderSiteRelationShipTypeID = @relationshipTypeId
	) As DerivedTable                        
	WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo  

	SET NOCOUNT OFF
END