CREATE PROCEDURE [dbo].[uspTrainingProviderSelectBySearchCriteria]
@RegisteredName NVARCHAR (100), @tradingName NVARCHAR (100), @City NVARCHAR (120), @UPIN INT, @UKPRN INT, @GeographicRegionId INT=0, @ManagingAreaId INT=0,  @PageIndex INT=1, @PageSize INT=20, @IsSortAsc BIT=1, @SortByField NVARCHAR (100)='TrainingProviderName'
AS
BEGIN                        
                  
                      
SET NOCOUNT ON                        
if @RegisteredName='' set @RegisteredName=NULL                  
if @tradingName='' set @tradingName=NULL                  
if @City='' set @City=NULL                  
if @UPIN='' set @UPIN=NULL
if @ManagingAreaId = 0  SET @ManagingAreaId=NULL
If @GeographicRegionId = 0 SET @GeographicRegionId = null

/***************CCR11193***********************************************/

declare @RegisteredName2 nvarchar(100)
declare @tradingName2 nvarchar(100)

if @RegisteredName = 'and'
	set @RegisteredName2 = '&'
else if @RegisteredName = '&'
	set @RegisteredName2 = ' and '
else if charindex('&', @RegisteredName) > 0
	set @RegisteredName2 = replace(@RegisteredName, '&', 'and')
else if charindex('and', @RegisteredName) > 0
	set @RegisteredName2 = replace(@RegisteredName, 'and', '&')

if @tradingName = 'and'
	set @tradingName2 = '&'
else if @tradingName = '&'
	set @tradingName2 = ' and '
else if charindex('&', @tradingName) > 0
	set @tradingName2 = replace(@tradingName, '&', 'and')
else if charindex('and', @tradingName) > 0
	set @tradingName2 = replace(@tradingName, 'and', '&')

/**********************************************************************/
             
/*********Set Page Number**********************************************/                  
declare @StartRowNo int                  
declare @EndRowNo int                  
set @StartRowNo =((@PageIndex-1)* @PageSize)+1                   
set @EndRowNo =(@PageIndex * @PageSize)                      
/***********************************************************************/                  
                  
/**************Total Number of Rows*************************************/                  
declare @TotalRows int                  
select @TotalRows= count(1) FROM [dbo].[ProviderSite] 
 JOIN ProviderSiteRelationship
 ON ProviderSite.ProviderSiteID = ProviderSiteRelationship.ProviderSiteID
 JOIN ProviderSiteRelationshipType ON ProviderSiteRelationship.ProviderSiteRelationshipTYpeID
 = ProviderSiteRelationshipTYpe.ProviderSiteRelationshipTYpeID and ProviderSiteRelationshipTYpeName = N'Owner'
 JOIN Provider ON ProviderSiteRelationship.ProviderID = Provider.ProviderID
 JOIN dbo.vwRegionsAndLocalAuthority Geographic ON Geographic.LocalAuthorityId = ProviderSite.LocalAuthorityId                    
where     
((([ProviderSite].fullName like '%'+ @RegisteredName + '%' or @RegisteredName is null) or ([ProviderSite].fullName like '%'+ @RegisteredName2 + '%' or @RegisteredName is null))   
OR((ProviderSite.tradingName like '%' + @tradingName + '%' or @tradingName is null) or (ProviderSite.tradingName like '%' + @tradingName2 + '%' or @tradingName is null)))                      
and (town = @City or @City is null)          
AND (ManagingAreaID = @ManagingAreaId OR  @ManagingAreaId IS NULL)   
AND (GeographicRegionId = @GeographicRegionId OR @GeographicRegionId IS NULL)                     
AND (TrainingProviderStatusTypeId = 1 OR  TrainingProviderStatusTypeId = 3)                      
and (UPIN = @UPIN or @UPIN is null) 
and (
		ISNULL(@UKPRN,Provider.UKPRN) = Provider.UKPRN
		AND Provider.ProviderStatusTypeID <> 2 
     )                     
/***********************************************************************/                  
                  
/*********set the order by**********************************************/                  
                  
declare @OrderBywithSort varchar(500)                  
                  
 if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END                  
 if @IsSortAsc = 0 BEGIN  set  @SortByField = @SortByField + ' desc' END             
/***********************************************************************/                  
                  
SELECT *,@TotalRows  as 'TotalRows' FROM                  
(                   
SELECT  ROW_NUMBER() OVER( ORDER BY                   
 Case when @SortByField='TrainingProviderName Asc'  then ProviderSite.TradingName  End ASC,                  
    Case when @SortByField='TrainingProviderName desc'  then ProviderSite.TradingName  End DESC ,         
        
Case when @SortByField='Town Asc'  then [Town]  End ASC,                  
    Case when @SortByField='Town desc'  then [Town]  End DESC  ,        
        
Case when @SortByField='UKPRN Asc'  then [ukprn]  End ASC,                  
    Case when @SortByField='UKPRN desc'  then [ukprn]  End DESC        ,        
        
Case when @SortByField='Contracted Asc'  then IsContracted   End ASC,                   
    Case when @SortByField='Contracted desc'  then IsContracted  End DESC,
    
Case when (@SortByField='Contracted Asc' OR @SortByField='Contracted desc') then ProviderSite.TradingName  End ASC           
               
 ) as RowNum,    
[ProviderSite].ProviderSiteID,                      
isnull([ProviderSite].TradingName,'') as 'TradingName',                      
isnull([AddressLine1],'') AS 'AddressLine1',                        
isnull([AddressLine2],'') AS 'AddressLine2',                        
isnull([AddressLine3],'') AS 'AddressLine3',                        
isnull([AddressLine4],'') AS 'AddressLine4',                     
[County].[FullName] AS 'County',            
[ProviderSite].[CountyId],                
isnull([Postcode],'') AS 'Postcode',                        
isnull([Town],'') AS 'Town',          
ISNULL([ukprn],'') AS 'UKPRN',
[ProviderSite].TrainingProviderStatusTypeId AS 'ProviderSiteStatus',
[ProviderSite].ManagingAreaID,
Provider.IsContracted                      
FROM [dbo].[ProviderSite]   
 JOIN ProviderSiteRelationship
 ON ProviderSite.ProviderSiteID = ProviderSiteRelationship.ProviderSiteID
 JOIN ProviderSiteRelationshipType ON ProviderSiteRelationship.ProviderSiteRelationshipTYpeID
 = ProviderSiteRelationshipTYpe.ProviderSiteRelationshipTYpeID and ProviderSiteRelationshipTYpeName = N'Owner'
 JOIN Provider ON ProviderSiteRelationship.ProviderID = Provider.ProviderID                      
 LEFT JOIN County ON [County].[CountyId] = [ProviderSite].[CountyId]
 JOIN dbo.vwRegionsAndLocalAuthority Geographic ON  Geographic.LocalAuthorityId =  ProviderSite.LocalAuthorityId              
where                       
((([ProviderSite].fullName like '%'+ @RegisteredName + '%' or @RegisteredName is null) or ([ProviderSite].fullName like '%'+ @RegisteredName2 + '%' or @RegisteredName is null))                      
OR ((ProviderSite.tradingName like '%' + @tradingName + '%' or @tradingName is null) or (ProviderSite.tradingName like '%' + @tradingName2 + '%' or @tradingName is null)))                      
and (town = @City or @City is null)
AND (GeographicRegionId = @GeographicRegionId OR @GeographicRegionId IS NULL)           
AND (ManagingAreaID = @ManagingAreaId OR  @ManagingAreaId IS NULL)      
AND (TrainingProviderStatusTypeId = 1 OR  TrainingProviderStatusTypeId = 3)                     
and (Provider.UPIN = @UPIN or @UPIN is null)
and (ISNULL(@UKPRN,Provider.UKPRN) = Provider.UKPRN
	AND Provider.ProviderStatusTypeID <> 2 )     

  ) as DerivedTable           
WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo                  
                  
                    
                    
                        
SET NOCOUNT OFF                        
END