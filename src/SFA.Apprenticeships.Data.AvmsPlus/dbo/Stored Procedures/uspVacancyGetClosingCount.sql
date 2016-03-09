Create PROCEDURE [dbo].[uspVacancyGetClosingCount]                                      
(                               
 @ManagingAreaId int,                          
 @daysFromClosingDateFor0ApplicationVacancies int
                
                       
)                                
AS                                  
BEGIN                                  
      
SET NOCOUNT ON      
      
DECLARE @totalRows int      

IF(@ManagingAreaId=0)                
SET @ManagingAreaId = NULL                      
       
          
/**************Total Number of Rows*************************************/                    
                   
SELECT  @totalRows = COUNT(1)                    
FROM Vacancy vac                        
    inner join [VacancyOwnerRelationship] vpr                        
     on vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]                        
    inner join Employer emp                        
     on vpr.EmployerId = emp.EmployerId
    left outer join [ProviderSite] tp                        
     on vpr.[ProviderSiteID] = tp.ProviderSiteID                         
    INNER JOIN dbo.VacancyStatusType VST ON vac.VacancyStatusId=VST.VacancyStatusTypeId                            
WHERE VST.FullName='Live'                        
AND vac.ApplyOutsideNAVMS = 0 -- Exclude applications outside NAVMS.                    
AND DATEDIFF(dd,GETDATE(),ApplicationClosingDate) <= @daysFromClosingDateFor0ApplicationVacancies                        
AND DATEDIFF(dd,GETDATE(),ApplicationClosingDate) >=0                        
AND tp.ManagingAreaID  = @ManagingAreaId 
AND (  
(SELECT COUNT(1) FROM dbo.[Application] appl WHERE appl.Vacancyid = vac.vacancyid) = (SELECT COUNT(1) FROM dbo.[Application] appl INNER JOIN dbo.ApplicationStatusType applStatus ON applStatus.ApplicationStatusTypeId=appl.ApplicationStatusTypeId WHERE vac.
VacancyId=appl.vacancyid AND ApplStatus.FullName='Withdrawn'))   
  
                        
/***********************************************************************/                    
      
RETURN @totalRows                
                        
SET NOCOUNT OFF                            
                               
END