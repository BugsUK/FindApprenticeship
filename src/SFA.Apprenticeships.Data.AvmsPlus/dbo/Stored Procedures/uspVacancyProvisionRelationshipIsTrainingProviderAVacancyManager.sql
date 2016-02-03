CREATE PROCEDURE [dbo].[uspVacancyProvisionRelationshipIsTrainingProviderAVacancyManager]         
 @ProviderId int        
AS        
BEGIN        
        
 SET NOCOUNT ON        
         
  declare @cnt as int    
    
     select @cnt = count(*)     
		from Vacancy v
		join VacancyOwnerRelationship vpr on vpr.VacancyOwnerRelationshipId = v.VacancyOwnerRelationshipId
		inner join VacancyProvisionRelationshipStatusType vprst on vpr.StatusTypeId = vprst.VacancyProvisionRelationshipStatusTypeId
			 and vprst.CodeName IN ('ACT','LIV')
			 and ManagerIsEmployer = 0   
		where v.VacancyManagerId = @ProviderId
     
     if (@cnt > 0)    
  select 1 as Result    
     else    
  select 0 as Result        
        
SET NOCOUNT OFF        
        
END