CREATE PROCEDURE [dbo].[uspVacancyGetFilledWithOpenApplications]                                        
(                                 
 @ManagingArea int,                            
 @daysFromClosingDateForVacancyNotFilled int,                            
 @daysFromClosingDateFor0ApplicationVacancies int,                            
 @numberOfDaysForFilledVacanciesWithOpenApplications int,                            
 @noApplicationsOnly BIT=1,                            
 @PageIndex int =  1,                              
 @PageSize int = 20,                              
 @IsSortAsc bit= 1,                              
 @SortByField nvarchar(100) = 'ApplicationHistoryDate'                                 
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
              
/**************Total Number of Rows*************************************/                        
                    
 DECLARE @totalRows int                    
                           
 IF @noApplicationsOnly = 0                           
  set @noApplicationsOnly = null                          
                           
 IF(@ManagingArea = 0)                       
    SET @ManagingArea = NULL                    
                     
                  
                
              
SELECT MyTable.*  FROM                
 (     
 select     
 TotalRows = count(1) over(),    
  ROW_NUMBER() OVER( ORDER BY               
                         
  Case when @SortByField='ClosingDate Asc'  then ApplicationClosingDate  End ASC,                        
  Case when @SortByField='ClosingDate desc'  then ApplicationClosingDate  End DESC,            
              
  Case when @SortByField='VacancyManager Asc'  then (CASE WHEN vpr.ManagerIsEmployer = 1 THEN emp.FullName ELSE tp.FullName END)  End ASC,                        
  Case when @SortByField='VacancyManager desc'  then (CASE WHEN vpr.ManagerIsEmployer = 1 THEN emp.FullName ELSE tp.FullName END)  End DESC,                        
              
  Case when @SortByField='VacancyPartner Asc'  then (CASE WHEN vpr.ManagerIsEmployer = 0 THEN emp.FullName ELSE tp.FullName END)  End ASC,                        
  Case when @SortByField='VacancyPartner desc'  then (CASE WHEN vpr.ManagerIsEmployer = 0 THEN emp.FullName ELSE tp.FullName END)  End DESC,                        
              
  Case when @SortByField='VacancyTitle Asc'  then Title  End ASC,                        
  Case when @SortByField='VacancyTitle desc'  then Title  End DESC,             
              
  Case when @SortByField='OpenedBy Asc'  then (Case WHEN vac.LockedForSupportUntil > getdate() THEN vac.BeingSupportedBy ELSE '' END)  End ASC,                        
  Case when @SortByField='OpenedBy desc'  then (Case WHEN vac.LockedForSupportUntil > getdate() THEN vac.BeingSupportedBy ELSE '' END)  End DESC ,              
            
               
  Case when @SortByField='ApplicationHistoryDate Asc'  then  ( SELECT Max(APPHIST.ApplicationHistoryEventDate) FROM [Application] APPL INNER JOIN dbo.ApplicationHistory APPHIST ON APPL.ApplicationId = APPHIST.ApplicationId                       
 INNER JOIN dbo.ApplicationHistoryEvent APPHISTEVT ON APPHIST.ApplicationHistoryEventTypeId = APPHISTEVT.ApplicationHistoryEventId                        
 INNER JOIN dbo.ApplicationStatusType APPSTAT ON APPHIST.ApplicationHistoryEventSubTypeId = APPSTAT.ApplicationStatusTypeId                        
 WHERE APPHISTEVT.FullName = 'Status Change'                        
 AND APPSTAT.FullName = 'Successful'                        
 AND APPHIST.ApplicationHistoryEventDate <= GETDATE() - @numberOfDaysForFilledVacanciesWithOpenApplications          
 AND Appl.VacancyId = vac.VacancyId )END ASC,                      
             
  Case when @SortByField='ApplicationHistoryDate desc'  then   (SELECT Max(APPHIST.ApplicationHistoryEventDate) FROM [Application] APPL INNER JOIN dbo.ApplicationHistory APPHIST ON APPL.ApplicationId = APPHIST.ApplicationId                        
 INNER JOIN dbo.ApplicationHistoryEvent APPHISTEVT ON APPHIST.ApplicationHistoryEventTypeId = APPHISTEVT.ApplicationHistoryEventId                        
 INNER JOIN dbo.ApplicationStatusType APPSTAT ON APPHIST.ApplicationHistoryEventSubTypeId = APPSTAT.ApplicationStatusTypeId                        
 WHERE APPHISTEVT.FullName = 'Status Change'                        
 AND APPSTAT.FullName = 'Successful'                        
 AND APPHIST.ApplicationHistoryEventDate <= GETDATE() - @numberOfDaysForFilledVacanciesWithOpenApplications          
 AND Appl.VacancyId = vac.VacancyId )end DESC               
           
                       
  ) as RowNum,              
  emp.EmployerId, tp.ProviderSiteID,                          
  (CASE WHEN vpr.ManagerIsEmployer = 1 THEN emp.FullName ELSE tp.FullName END) AS VacancyManager,                          
  (CASE WHEN vpr.ManagerIsEmployer = 0 THEN emp.FullName ELSE tp.FullName END) AS VacancyPartner,                          
  vac.Title, vac.ApplicationClosingDate, vac.VacancyId,                          
  (Case WHEN vac.LockedForSupportUntil > getdate() THEN vac.BeingSupportedBy ELSE '' END) as OpenedBy,            
  (SELECT max(APPHIST.ApplicationHistoryEventDate) FROM [Application] APPL INNER JOIN dbo.ApplicationHistory APPHIST ON APPL.ApplicationId = APPHIST.ApplicationId                        
 INNER JOIN dbo.ApplicationHistoryEvent APPHISTEVT ON APPHIST.ApplicationHistoryEventTypeId = APPHISTEVT.ApplicationHistoryEventId                        
 INNER JOIN dbo.ApplicationStatusType APPSTAT ON APPHIST.ApplicationHistoryEventSubTypeId = APPSTAT.ApplicationStatusTypeId                        
 WHERE APPHISTEVT.FullName = 'Status Change'                        
AND APPSTAT.FullName = 'Successful'                        
 AND APPHIST.ApplicationHistoryEventDate <= GETDATE() - @numberOfDaysForFilledVacanciesWithOpenApplications          
 AND Appl.VacancyId = vac.VacancyId) AS ApplicationHistoryDate                        
                            
  FROM Vacancy vac                          
   inner join [VacancyOwnerRelationship] vpr                          
    on vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]                          
   inner join Employer emp                          
    on vpr.EmployerId = emp.EmployerId      
   INNER JOIN dbo.LocalAuthority LA ON emp.LocalAuthorityId = LA.LocalAuthorityId    
   INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID    
   INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID    
   INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID    
  AND LocalAuthorityGroupTypeName = N'Managing Area'                        
   left outer join [ProviderSite] tp                          
    on vpr.[ProviderSiteID] = tp.ProviderSiteID                           
   INNER JOIN dbo.VacancyStatusType VST ON vac.VacancyStatusId=VST.VacancyStatusTypeId                              
                                 
  WHERE (VST.FullName='Live' OR VST.FullName='Closed')                        
  AND vac.NumberofPositions =(SELECT COUNT(*)                               
   FROM dbo.[Application]                              
   INNER JOIN dbo.ApplicationStatusType ON dbo.ApplicationStatusType.ApplicationStatusTypeId=dbo.[Application].ApplicationStatusTypeId                              
   WHERE dbo.ApplicationStatusType.FullName ='Successful'                              
   AND [Application].VacancyId=vac.VacancyId)                         
  AND vac.VacancyId IN                         
  (SELECT APPL.VacancyID FROM [Application] APPL INNER JOIN dbo.ApplicationHistory APPHIST ON APPL.ApplicationId = APPHIST.ApplicationId                        
  INNER JOIN dbo.ApplicationHistoryEvent APPHISTEVT ON APPHIST.ApplicationHistoryEventTypeId = APPHISTEVT.ApplicationHistoryEventId                        
  INNER JOIN dbo.ApplicationStatusType APPSTAT ON APPHIST.ApplicationHistoryEventSubTypeId = APPSTAT.ApplicationStatusTypeId                        
  WHERE APPHISTEVT.FullName = 'Status Change'                        
  --AND APPSTAT.FullName = 'Successful'       
--  Addded  By Hitesh because TIR trial00051666 raised for the same and it's    
--  says that "Count the number Vacancies at the status of Filled but have any outstanding (open) applications."                  
-- then only need to show them if it's decline don't need to show in this list    
  and APPL.ApplicationStatusTypeId in(select ApplicationStatusTypeId     
          from ApplicationStatusType    
          where CodeName in ('New','App'))    
    
  AND APPHIST.ApplicationHistoryEventDate <= GETDATE() - @numberOfDaysForFilledVacanciesWithOpenApplications)                        
  AND (isnull(Case WHEN vpr.ManagerIsEmployer = 1 THEN LAG.LocalAuthorityGroupID ELSE tp.ManagingAreaID End,0) = @ManagingArea OR @ManagingArea IS NULL)                         
) as MyTable                
          
WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo                       
                          
SET NOCOUNT OFF                              
                                 
END