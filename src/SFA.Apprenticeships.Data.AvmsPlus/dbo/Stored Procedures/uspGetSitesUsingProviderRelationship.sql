CREATE PROCEDURE [dbo].[uspGetSitesUsingProviderRelationship]
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
	SELECT @TotalRows= count(1)  FROM [dbo].[ProviderSite]  
	JOIN ProviderSiteRelationship
	ON ProviderSite.ProviderSiteID = ProviderSiteRelationship.ProviderSiteID
	JOIN Provider ON ProviderSiteRelationship.ProviderID = Provider.ProviderID         
	LEFT JOIN County ON [County].[CountyId] = [ProviderSite].[CountyId]      
	INNER JOIN dbo.LocalAuthorityGroup LAG ON  LAG.LocalAuthorityGroupID = [ProviderSite].ManagingAreaID 
	INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
	AND LocalAuthorityGroupTypeName = N'Managing Area'  
	WHERE [Provider].ProviderID = @id  AND ProviderSiteRelationship.ProviderSiteRelationShipTypeID = @relationshipTypeId
	AND [ProviderSite].TrainingProviderStatusTypeId <> 2

	/* SORTING */
	if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END            
	if @IsSortAsc = 0 BEGIN  set  @SortByField = @SortByField + ' desc' END  

	SELECT *, @TotalRows  as 'TotalRows' from     
	(SELECT ROW_NUMBER() OVER 
		(ORDER BY 
			Case when @SortByField='TrainingProviderName Asc'  then [ProviderSite].[TradingName]  End ASC,          
			Case when @SortByField='TrainingProviderName desc'  then [ProviderSite].[TradingName]  End DESC,
			Case when @SortByField='Town Asc'  then [ProviderSite].[Town]  End ASC,          
			Case when @SortByField='Town desc'  then [ProviderSite].[Town]  End DESC
		) AS RowNum,    

	isnull([ProviderSite].[FullName],'') AS 'FullName',   
   [ProviderSite].[OutofDate] AS 'OutofDate',            
   isnull([ProviderSite].[TradingName],'') AS 'TradingName',            
   [ProviderSite].[ProviderSiteID] AS 'TrainingProviderId',            
   [Provider].[UPIN] AS 'UPIN',
   [ProviderSite].[EDSURN] AS 'EDSURN',
   isnull([AddressLine1],'') AS 'AddressLine1',              
   isnull([AddressLine2],'') AS 'AddressLine2',              
   isnull([AddressLine3],'') AS 'AddressLine3',             
   isnull([AddressLine4],'') AS 'AddressLine4',          
   County.FullName AS 'County',              
   isnull([Postcode],'') AS 'Postcode',              
   isnull([Town],'') AS 'Town',      
   LAG.FullName AS 'Region',  
   [ProviderSite].ManagingAreaID,  
   [ProviderSite].[CountyId]     
   ,ISNULL(EmployerDescription,'') AS EmployerDescription
   ,ISNULL(ContactDetailsForEmployer,'') AS  ContactDetailsForEmployer
    ,ISNULL(WebPage,'') AS  WebPage    
    ,ISNULL(HideFromSearch,'') AS HideFromSearch
    ,ISNULL(ContactDetailsForCandidate,'') AS ContactDetailsForCandidate
    ,ISNULL(CandidateDescription,'') AS CandidateDescription
	,[Provider].UKPRN
	,[ProviderSite].OwnerOrganisation
	,[ProviderSite].TrainingProviderStatusTypeId
	,[Provider].ProviderID
	,isnull([Provider].TradingName,'') AS 'ProviderTradingName'          
	,isnull([Provider].FullName,'') AS 'ProviderFullName'
	,[Provider].IsContracted
	,[Provider].ContractedFrom
	,[Provider].ContractedTo

  FROM [dbo].[ProviderSite]  
   JOIN ProviderSiteRelationship
 ON ProviderSite.ProviderSiteID = ProviderSiteRelationship.ProviderSiteID
 JOIN Provider ON ProviderSiteRelationship.ProviderID = Provider.ProviderID         
   LEFT JOIN County ON [County].[CountyId] = [ProviderSite].[CountyId]      
   INNER JOIN dbo.LocalAuthorityGroup LAG ON  LAG.LocalAuthorityGroupID = [ProviderSite].ManagingAreaID 
   INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
   AND LocalAuthorityGroupTypeName = N'Managing Area'  
  WHERE [Provider].ProviderID = @id  AND ProviderSiteRelationship.ProviderSiteRelationShipTypeID = @relationshipTypeId
  AND [ProviderSite].TrainingProviderStatusTypeId <> 2
	  ) As DerivedTable                        
	WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo  
	SET NOCOUNT OFF          
END