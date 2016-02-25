CREATE PROCEDURE [dbo].[uspVacancyProvisionRelationshipIsEmployerAVacancyOwner]       
 @EmployerId int      
AS      
BEGIN      
      
 SET NOCOUNT ON      
       
  declare @cnt as int  
  
     select @cnt = count(*)   
     from [VacancyOwnerRelationship] vpr
     inner join VacancyProvisionRelationshipStatusType vprst on vpr.StatusTypeId = vprst.VacancyProvisionRelationshipStatusTypeId
     where EmployerId = @EmployerId
     and vprst.CodeName IN ('ACT','LIV')
     and ManagerIsEmployer = 1  
   
     if (@cnt > 0)  
  select 1 as Result  
     else  
  select 0 as Result      
      
SET NOCOUNT OFF      
      
END