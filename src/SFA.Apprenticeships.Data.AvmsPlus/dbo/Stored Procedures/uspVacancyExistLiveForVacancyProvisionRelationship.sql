create PROCEDURE [dbo].[uspVacancyExistLiveForVacancyProvisionRelationship]                    
(                
 @VacancyProvisionRelationshipId int     
)                
AS                
BEGIN                
 SET NOCOUNT ON                
      
         
 declare @OpenVacancy int                
                
 select @OpenVacancy = count(VacancyId)                 
 from vacancy     
 inner join vacancyStatusType on vacancy.VacancyStatusId = vacancyStatusType.VacancyStatusTypeId                
 where                 
  [VacancyOwnerRelationshipId] = @VacancyProvisionRelationshipId
        
 and vacancyStatusType.FullName in  ('Live')                 
      
      
 if @OpenVacancy = 0                
  select 0 as result              
 else                
  select 1 as result               
                
 SET NOCOUNT OFF                
END