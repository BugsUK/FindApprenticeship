CREATE PROCEDURE [dbo].[uspVacancyRequireFurtherWork]              
(       
 @providerId int,  
 @employerId int,
 @vacancyManagerId int = null,
 @PageIndex int =  1,    
 @PageSize int = 20,    
 @IsSortAsc bit= 1,    
 @SortByField nvarchar(100) = 'EmployerName'       
)        
AS          
BEGIN          
  
      
SET NOCOUNT ON      

declare @ManagerIsEmployer int

if @providerId is null
    set @ManagerIsEmployer = 1  
else
   set @ManagerIsEmployer  = 0    
 
declare @StartRowNo int    
declare @EndRowNo int    
set @StartRowNo =((@PageIndex-1)* @PageSize)+1     
set @EndRowNo =(@PageIndex * @PageSize) 
       
/***********************************************************************/    
/*********set the order by**********************************************/
    
declare @OrderBywithSort varchar(500)    
if @SortByField = 'EmployerName'    
Begin     
 if @IsSortAsc = 1 BEGIN set  @SortByField = 'EmployerName Asc' END    
 if @IsSortAsc = 0 BEGIN  set  @SortByField = 'EmployerName desc' END    
End   
if @SortByField = 'ProviderName'    
Begin     
 if @IsSortAsc = 1 BEGIN set  @SortByField = 'ProviderName Asc' END    
 if @IsSortAsc = 0 BEGIN  set  @SortByField = 'ProviderName desc' END    
End   

  
/**************Total Number of Rows*************************************/    
declare @TotalRows int    
select @TotalRows = count(1)  
from   
(
SELECT 
 v.VacancyId 
 FROM [vacancy]  v   
  INNER JOIN [VacancyOwnerRelationship] vr ON v.[VacancyOwnerRelationshipId] = vr.[VacancyOwnerRelationshipId] AND 
    vr.[ManagerIsEmployer] = @ManagerIsEmployer    
  INNER JOIN [Employer] ep ON ep.EmployerId=vr.EmployerId
  INNER JOIN [ProviderSite] tp ON tp.ProviderSiteID=vr.[ProviderSiteID]   
  INNER JOIN [vacancyStatustype] vst ON v.vacancyStatusId = vst.vacancyStatusTypeId
  INNER JOIN [ApprenticeshipType] at ON v.ApprenticeshipType = at.ApprenticeshipTypeId
  LEFT OUTER JOIN [ApprenticeshipFramework] af ON v.ApprenticeshipFrameworkId = af.ApprenticeshipFrameworkId 
 WHERE
   ( vr.[ProviderSiteID] =@providerId or @providerId is null)  AND
   ( vr.[EmployerId]=@employerId or @employerId is null)  AND
    vst.FullName = 'Referred'
	AND (
		(@vacancyManagerId IS NULL) OR
		(v.VacancyManagerId = @vacancyManagerId)
	)
) as derivedtotal  
  
     
/***********************************************************************/    
  
SELECT *,@TotalRows  as 'TotalRows' FROM    
(     
Select ROW_NUMBER() OVER( ORDER BY  
 v.ApplicationClosingDate   ASC,  --trial00053759
 Case when @SortByField='EmployerName Asc'  then ep.FullName End ASC,    
 Case when @SortByField='EmployerName desc'  then ep.FullName End DESC,    
 Case when @SortByField='ProviderName Asc'  then tp.TradingName  End ASC,    
 Case when @SortByField='ProviderName desc'  then tp.TradingName End DESC   
 ) as RowNum,  
 v.VacancyId as 'VacancyId',
 v.numberOfPositions as 'NumberOfPositions', 
 v.Title as 'VacancyTitle',
 v.ApplicationClosingDate as 'ClosingDate',
 v.VacancyLocationTypeId as 'VacancyLocationTypeId',
 v.VacancyStatusId as 'StatusId',
 v.ApprenticeshipType as 'VacancyType',
 v.ApprenticeshipFrameworkId as 'ApprenticeshipFrameworkId',
 af.ApprenticeshipOccupationId as 'ApprenticeshipOccupationId',
 vr.EmployerId as 'EmployerId', 
 vr.[ProviderSiteID] as 'ProviderId',
 ep.FullName as 'EmployerName',
 tp.TradingName as 'TradingName',
 vst.FullName as 'VacancyStatus',
 af.FullName as 'FrameworkName', 
 at.FullName as 'VacancyTypeName'  
 FROM [vacancy]  v   
  INNER JOIN [VacancyOwnerRelationship] vr ON v.[VacancyOwnerRelationshipId] = vr.[VacancyOwnerRelationshipId] AND 
    vr.[ManagerIsEmployer] = @ManagerIsEmployer    
  INNER JOIN [Employer] ep ON ep.EmployerId=vr.EmployerId
  INNER JOIN [ProviderSite] tp ON tp.ProviderSiteID=vr.[ProviderSiteID]   
  INNER JOIN [vacancyStatustype] vst ON v.vacancyStatusId = vst.vacancyStatusTypeId
  INNER JOIN [ApprenticeshipType] at ON v.ApprenticeshipType = at.ApprenticeshipTypeId
  LEFT OUTER JOIN [ApprenticeshipFramework] af ON v.ApprenticeshipFrameworkId = af.ApprenticeshipFrameworkId 
 WHERE
  ( vr.[ProviderSiteID] =@providerId or @providerId is null)  AND
  ( vr.[EmployerId]=@employerId or @employerId is null)  AND
   vst.FullName = 'Referred'  
   AND (
		(@vacancyManagerId IS NULL) OR
		(v.VacancyManagerId = @vacancyManagerId)
	)
) as DerivedTable    
WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo     
           
SET NOCOUNT OFF          
END