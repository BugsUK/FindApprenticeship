CREATE PROCEDURE [dbo].[uspVacancyGetClosing]                                            
(                                     
 @ManagingArea int,                                
 @daysFromClosingDateForVacancyNotFilled int,                                
 @daysFromClosingDateFor0ApplicationVacancies int,                                
 @numberOfDaysForFilledVacanciesWithOpenApplications int,                                
 @noApplicationsOnly BIT=1,                                
 @PageIndex int =  1,                                  
 @PageSize int = 20,                                  
 @IsSortAsc bit= 1,                                  
 @SortByField nvarchar(100) = 'VacancyManager'                                     
)                                      
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
                          
 if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END                          
 if @IsSortAsc = 0 BEGIN  set  @SortByField = @SortByField + ' desc' END                     
/***********************************************************************/                          
                
                
DECLARE @totalRows int                    
                              
IF @noApplicationsOnly = 0                               
 set @noApplicationsOnly = null                              
                              
IF(@ManagingArea=0)                      
SET @ManagingArea = NULL                            
                                   
SELECT *      
FROM                        
(                          
 SELECT  ROW_NUMBER() OVER( ORDER BY                           
  Case when @SortByField='ClosingDate Asc'  then ApplicationClosingDate  End ASC,                          
  Case when @SortByField='ClosingDate desc'  then ApplicationClosingDate  End DESC,              
                
  Case when @SortByField='VacancyManager Asc'  then (CASE WHEN vpr.ManagerIsEmployer = 1 THEN emp.FullName ELSE tp.FullName END)  End ASC,                          
  Case when @SortByField='VacancyManager desc'  then (CASE WHEN vpr.ManagerIsEmployer = 1 THEN emp.FullName ELSE tp.FullName END)  End DESC,                          
                
  Case when @SortByField='VacancyPartner Asc'  then (CASE WHEN vpr.ManagerIsEmployer = 0 THEN emp.FullName ELSE tp.FullName END)  End ASC,                          
  Case when @SortByField='VacancyPartner desc'  then (CASE WHEN vpr.ManagerIsEmployer = 0 THEN emp.FullName ELSE tp.FullName END)  End DESC,                          
                
  Case when @SortByField='VacancyTitle Asc'  then Title  End ASC,                          
  Case when @SortByField='VacancyTitle desc'  then Title  End DESC,               
                
  Case when @SortByField='OpenedBy Asc'  then (Case WHEN vac.LockedForSupportUntil > getdate() THEN vac.BeingSupportedBy ELSE '' END)  End ASC,                          
  Case when @SortByField='OpenedBy desc'  then (Case WHEN vac.LockedForSupportUntil > getdate() THEN vac.BeingSupportedBy ELSE '' END)  End DESC                 
                             
                
  ) as RowNum,                                
 emp.EmployerId, tp.ProviderSiteID,                              
 tp.FullName AS VacancyManager,                              
 emp.FullName AS VacancyPartner,                              
 vac.Title, vac.ApplicationClosingDate, vac.VacancyId,                              
 (Case WHEN vac.LockedForSupportUntil > getdate() THEN vac.BeingSupportedBy ELSE '' END) as OpenedBy,      
 TotalRows = count(1) over()        
                               
 FROM Vacancy vac                              
  INNER JOIN [VacancyOwnerRelationship] vpr on vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]                              
  inner join Employer emp on vpr.EmployerId = emp.EmployerId      
  LEFT JOIN [ProviderSite] tp on vpr.[ProviderSiteID] = tp.ProviderSiteID                               
  INNER JOIN dbo.VacancyStatusType VST ON vac.VacancyStatusId=VST.VacancyStatusTypeId                                  
                                   
  WHERE VST.FullName='Live'                              
  AND vac.ApplyOutsideNAVMS = 0 -- Exclude applications outside NAVMS.                          
  AND DATEDIFF(dd,GETDATE(),ApplicationClosingDate) <= @daysFromClosingDateFor0ApplicationVacancies                              
  AND DATEDIFF(dd,GETDATE(),ApplicationClosingDate) >=0                              
  AND (tp.ManagingAreaID = @ManagingArea OR @ManagingArea IS NULL)                             
  AND (        
(SELECT COUNT(1) FROM dbo.[Application] appl WHERE appl.Vacancyid = vac.vacancyid) = (SELECT COUNT(1) FROM dbo.[Application] appl INNER JOIN dbo.ApplicationStatusType applStatus ON applStatus.ApplicationStatusTypeId=appl.ApplicationStatusTypeId WHERE vac.
  
    
      
VacancyId=appl.vacancyid AND ApplStatus.FullName='Withdrawn'))         
) as DerivedTable                          
                
WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo                     
                          
                              
SET NOCOUNT OFF                                  
                                     
END