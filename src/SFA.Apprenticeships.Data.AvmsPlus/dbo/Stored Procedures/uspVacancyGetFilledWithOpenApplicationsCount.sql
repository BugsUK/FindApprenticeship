create PROCEDURE [dbo].[uspVacancyGetFilledWithOpenApplicationsCount]                                
(                         
 @ManagingAreaId int,                    
 @numberOfDaysForFilledVacanciesWithOpenApplications int                    
                     
)                          
AS                            
BEGIN                            
                        
SET NOCOUNT ON                            
      
DECLARE @totalRows int  

 IF(@ManagingAreaId = 0)               
    SET @ManagingAreaId = NULL            
             
 SELECT  @totalRows =  COUNT(1)               
                   
 FROM Vacancy vac                  
  inner join [VacancyOwnerRelationship] vpr                  
   on vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]      
    inner join Employer emp                        
     on vpr.EmployerId = emp.EmployerId   
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
 and APPL.ApplicationStatusTypeId in(select ApplicationStatusTypeId 
 										from ApplicationStatusType
										where CodeName in ('New','App'))

 AND APPHIST.ApplicationHistoryEventDate <= GETDATE() - @numberOfDaysForFilledVacanciesWithOpenApplications)                
 AND tp.ManagingAreaID = @ManagingAreaId
 
            
/***********************************************************************/                
      
RETURN @totalRows  
                  
SET NOCOUNT OFF                      
                         
END