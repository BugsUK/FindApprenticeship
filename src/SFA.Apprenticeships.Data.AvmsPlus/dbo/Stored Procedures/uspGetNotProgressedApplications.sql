CREATE PROCEDURE [dbo].[uspGetNotProgressedApplications]      
      
@ManagingAreaId int,      
@DaysNotProgressed int,      
@PageIndex int =  1,            
@PageSize int = 20,            
@IsSortAsc bit= 1,            
@SortByField nvarchar(100) = 'VacancyManager'        
      
AS      
BEGIN      
      
SET NOCOUNT ON      
      
 /*********Set Page Number**********************************************/            
 declare @StartRowNo int            
 declare @EndRowNo int            
 set @StartRowNo =((@PageIndex-1)* @PageSize)+1             
 set @EndRowNo =(@PageIndex * @PageSize)                
 /***********************************************************************/            
             
 /**************Total Number of Rows*************************************/            
 declare @TotalRows int        
 /* call existing sp to retrieve the total number of rows */      
 exec uspGetNotProgressedVacanciesCount @ManagingAreaId, @DaysNotProgressed, @TotalRows output      
 /***********************************************************************/            
             
 /*********set the order by**********************************************/            
 declare @OrderBywithSort varchar(500)            
             
  if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END            
  if @IsSortAsc = 0 BEGIN  set  @SortByField = @SortByField + ' Desc' END       
 /***********************************************************************/         
      
select *       
from (      
  select *,      
    ROW_NUMBER() OVER( ORDER BY             
    Case when @SortByField='VacancyManager Asc'  then VacancyManager  End ASC,            
    Case when @SortByField='VacancyManager Desc'  then VacancyManager  End DESC,            
    Case when @SortByField='VacancyTitle Asc'  then VacancyTitle  End ASC,            
    Case when @SortByField='VacancyTitle Desc'  then VacancyTitle  End DESC,      
    Case when @SortByField='VacancyClosingDate Asc'  then VacancyClosingDate  End ASC,            
    Case when @SortByField='VacancyClosingDate Desc'  then VacancyClosingDate  End DESC,            
    Case when @SortByField='NumberOfApplications Asc'  then NumberOfApplications  End ASC,            
    Case when @SortByField='NumberOfApplications Desc'  then NumberOfApplications  End DESC,      
    Case when @SortByField='NumberOfNotProgressedApplications Asc'  then NumberOfNotProgressedApplications  End ASC,            
    Case when @SortByField='NumberOfNotProgressedApplications Desc'  then NumberOfNotProgressedApplications  End DESC,      
    Case when @SortByField='VacancyID Asc'  then VacancyID  End ASC,            
    Case when @SortByField='VacancyID Desc'  then VacancyID  End DESC,            
    Case when @SortByField='OpenedBy Asc'  then OpenedBy  End ASC,            
    Case when @SortByField='OpenedBy Desc'  then OpenedBy  End DESC            
    ) as RowNum,      
    @TotalRows  as 'TotalRows'       
  from        
   (      
      
----------      
      
SELECT DISTINCT      
 tp.TradingName       
 AS VacancyManager,      
vac.Title AS VacancyTitle,      
vac.ApplicationClosingDate AS VacancyClosingDate,      
      
 (SELECT      
  count(app.ApplicationId)       
      
  FROM      
   
        
  [VacancyOwnerRelationship] vpr      
      
  INNER JOIN      
  [ProviderSite] tp      
  ON vpr.[ProviderSiteID] = tp.ProviderSiteID       
      
  INNER JOIN      
  Vacancy vac      
  ON vpr.[VacancyOwnerRelationshipId] = vac.[VacancyOwnerRelationshipId]       
      
  INNER JOIN      
  [Application] app      
  ON vac.VacancyId = app.VacancyId       
    
  INNER JOIN      
  ApplicationStatusType ast      
  ON app.ApplicationStatusTypeId = ast.ApplicationStatusTypeId       
      
  WHERE      
   app.VacancyId =app1.VacancyId      
            AND UPPER(ast.CodeName) != 'DRF'       
   AND       
     (tp.ManagingAreaID = @ManagingAreaId )      
    
          
  ) AS NumberOfApplications,      
      
 (SELECT      
  count(app.ApplicationId)       
      
  FROM      
   
  [VacancyOwnerRelationship] vpr      
        
  INNER JOIN      
  [ProviderSite] tp      
  ON vpr.[ProviderSiteID] = tp.ProviderSiteID       
      
  INNER JOIN      
  Vacancy vac      
  ON vpr.[VacancyOwnerRelationshipId] = vac.[VacancyOwnerRelationshipId]       
      
  INNER JOIN      
  [Application] app      
  ON vac.VacancyId = app.VacancyId       
      
  INNER JOIN      
  ApplicationStatusType ast      
  ON app.ApplicationStatusTypeId = ast.ApplicationStatusTypeId       
      
  INNER JOIN      
  ApplicationHistory ah      
  ON app.ApplicationId = ah.ApplicationId       
      
 WHERE       
  app.VacancyId =app1.VacancyId      
  AND UPPER(ast.CodeName) in ('NEW', 'APP')       
  AND (tp.ManagingAreaID = @ManagingAreaId )      
  
         
  AND ah.ApplicationHistoryEventSubTypeId = app.ApplicationStatusTypeId      
  
  AND ah.ApplicationHistoryEventDate < DATEADD( dd, -@DaysNotProgressed,GETDATE())   
 ) AS NumberOfNotProgressedApplications,      
      
vac.VacancyID as VacancyID,      
CASE WHEN vac.LockedForSupportUntil > getdate()       
     THEN vac.BeingSupportedBy       
     ELSE ''       
END AS OpenedBy      
      
FROM      
      
 [VacancyOwnerRelationship] vpr      
    
      
 INNER JOIN      
 [ProviderSite] tp      
 ON vpr.[ProviderSiteID] = tp.ProviderSiteID       
      
 INNER JOIN      
 Vacancy vac      
 ON vpr.[VacancyOwnerRelationshipId] = vac.[VacancyOwnerRelationshipId]       
      
 INNER JOIN      
 [Application] app1      
 ON vac.VacancyId = app1.VacancyId       
      
 INNER JOIN      
 ApplicationStatusType ast      
 ON app1.ApplicationStatusTypeId = ast.ApplicationStatusTypeId       
      
 INNER JOIN      
 ApplicationHistory ah      
 ON app1.ApplicationId = ah.ApplicationId       
      
WHERE       
 UPPER(ast.CodeName) in ('NEW', 'APP')       
 AND     
         tp.ManagingAreaID = @ManagingAreaId   
          
 AND ah.ApplicationHistoryEventSubTypeId = app1.ApplicationStatusTypeId      
   AND ah.ApplicationHistoryEventDate < DATEADD( dd, -@DaysNotProgressed,GETDATE())       
      
----------      
) x      
) y      
WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo;       
      
      
SET NOCOUNT OFF      
END