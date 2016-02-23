create PROCEDURE [dbo].[uspApplicationExistAppliedForVacancyProvisionRelationship]                      
(                  
 @VacancyProvisionRelationshipId int     
)                  
AS                  
BEGIN                  
 SET NOCOUNT ON                  
              
       
declare @OpenVacancy int                  
             
select @OpenVacancy = count([application].applicationId)                   
from vacancy 
inner join [application] on  [application].VacancyId = vacancy.VacancyId            
inner join applicationStatusType on [application].ApplicationStatusTypeId = applicationStatusType.ApplicationStatusTypeId                  
where                   
[VacancyOwnerRelationshipId] = @VacancyProvisionRelationshipId                 
and applicationStatusType.FullName in ('Applied')                 
                   
 if @OpenVacancy = 0                  
  select 0 as result                
 else                  
  select 1 as result                 
                  
 SET NOCOUNT OFF                  
END