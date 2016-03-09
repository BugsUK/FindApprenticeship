CREATE PROCEDURE [dbo].[uspVacancyProvisionRelationshipIsTrainingProviderAVacancyOwner]  
 @ProviderId int        
AS        
BEGIN        
        
 SET NOCOUNT ON        
         
  declare @cnt as int    
    
     select @cnt = count(*)     
     from [VacancyOwnerRelationship] vpr
     inner join VacancyProvisionRelationshipStatusType vprst on vpr.StatusTypeId = vprst.VacancyProvisionRelationshipStatusTypeId
     where [ProviderSiteID] = @ProviderId
     and vprst.CodeName IN ('ACT','LIV')
     and ManagerIsEmployer = 0    
     
     if (@cnt > 0)    
  select 1 as Result    
     else    
  select 0 as Result        
        
SET NOCOUNT OFF        
        
END