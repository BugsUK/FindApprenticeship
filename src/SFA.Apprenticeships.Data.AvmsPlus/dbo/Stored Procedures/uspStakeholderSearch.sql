CREATE PROCEDURE [dbo].[uspStakeholderSearch]
@FirstName VARCHAR (70), @SurName VARCHAR (70), @OrganizationId INT, @GeographicRegionCodeName VARCHAR(3), @PostCode VARCHAR (50), @PageIndex INT=1, @PageSize INT=20, @IsSortAsc BIT=1, @SortByField NVARCHAR (100)='surname'
AS
BEGIN         
IF @OrganizationId = 0 SET @OrganizationId = null    
IF @GeographicRegionCodeName = '' SET @GeographicRegionCodeName = null    
if @FirstName = '' SET @FirstName = null
if @SurName = '' set @SurName = null
if @PostCode = '' set @PostCode = null

      
                
SET NOCOUNT ON                  
/*********Set Page Number**********************************************/            
declare @StartRowNo int            
declare @EndRowNo int            
set @StartRowNo =((@PageIndex-1)* @PageSize)+1             
set @EndRowNo =(@PageIndex * @PageSize)                
/***********************************************************************/            
             
		   
/*********set the order by**********************************************/            
			            
declare @OrderBywithSort varchar(500)            
			            
if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END            
if @IsSortAsc = 0 BEGIN  set  @SortByField = @SortByField + ' desc' END       
/***********************************************************************/            


SELECT MyTable.*  FROM            
	( 
	select 
	TotalRows = count(1) over(),
	ROW_NUMBER() OVER( ORDER BY             
			  Case when @SortByField='surname Asc'  then p.SurName   End ASC,            
			  Case when @SortByField='surname desc'  then p.SurName End DESC,
			  Case when @SortByField='surname Asc'  then p.FirstName End ASC,            
			  Case when @SortByField='surname desc'  then p.FirstName End DESC,
			  Case when @SortByField='FirstName Asc'  then p.FirstName   End ASC,            
			  Case when @SortByField='FirstName desc'  then p.FirstName End DESC,
			  Case when @SortByField='OrganizationName Asc'  then o.FullName   End ASC,            
			  Case when @SortByField='OrganizationName desc'  then o.FullName End DESC,
			  Case when @SortByField='PostCode Asc'  then s.Postcode   End ASC,            
			  Case when @SortByField='PostCode desc'  then s.Postcode End DESC,
			  Case when @SortByField='Region Asc'  then vwRLA.GeographicFullName   End ASC,            
			  Case when @SortByField='Region desc'  then vwRLA.GeographicFullName End DESC
	 
			  ) as RowNum,                 
			s.StakeholderId,
			ISNULL(p.FirstName,'') AS 'FirstName',
			ISNULL(p.SurName,'') AS 'SurName',
			CASE WHEN o.FullName = 'Other' THEN
			'Other - ' + s.OrganisationOther
			ELSE
			o.FullName 
			END AS 'OrganizationName',
			ISNULL(s.Postcode,'') AS 'PostCode',
			ISNULL(vwRLA.GeographicFullName,'') AS 'RegionName',
			ISNULL(s.OrganisationOther,'') AS 'OrganisationOther'
			FROM StakeHolder s 
			 INNER join Person p ON p.PersonId= s.PersonId 
			 INNER JOIN dbo.LocalAuthority LA ON s.LocalAuthorityId = LA.LocalAuthorityId
			 JOIN vwRegionsAndLocalAuthority vwRLA on vwRLA.LocalAuthorityId = LA.LocalAuthorityId
			 --INNER JOIN dbo.LocalAuthority LA ON s.LocalAuthorityId = LA.LocalAuthorityId
			 --INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
			 --INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
			 --INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
			 --AND LocalAuthorityGroupTypeName = N'Region'  
			--INNER JOIN LSCRegion R ON s.LSCRegionId = r.LSCRegionId
			INNER JOIN Organisation o ON s.OrganisationId = o.OrganisationId
			WHERE 
				(p.FirstName LIKE  @FirstName + '%' OR @FirstName IS NULL)
			AND (p.SurName LIKE  @SurName + '%' OR @SurName IS NULL)
			AND (s.OrganisationId = @OrganizationId OR @OrganizationId IS NULL)
			AND (vwRLA.GeographicCodeName = @GeographicRegionCodeName OR @GeographicRegionCodeName IS NULL)
			AND (s.PostCode LIKE  @PostCode + '%' OR @PostCode IS NULL)
			AND (s.StakeholderStatusId = 2 or s.StakeholderStatusId = 1)
		
			GROUP BY s.StakeholderId,p.FirstName,p.SurName,o.FullName,s.Postcode,vwRLA.GeographicFullName, s.OrganisationOther					  
	) as MyTable            
			   
			WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo            
            
             
                  
SET NOCOUNT OFF                  
END