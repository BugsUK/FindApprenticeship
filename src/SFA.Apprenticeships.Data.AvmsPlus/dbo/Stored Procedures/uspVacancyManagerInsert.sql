CREATE PROCEDURE [dbo].[uspVacancyManagerInsert]
@contractHolderisEmployer BIT=NULL, @employerId INT, @managerisEmployer BIT=NULL, @trainingProviderId INT, @VacancyStatus NVARCHAR (100), @Notes NVARCHAR (4000), @NationwideAllowed BIT=NULL, @employerDescription NVARCHAR(MAX)=NULL, @VacancyManagerId INT OUTPUT
AS
BEGIN                    
 SET NOCOUNT ON                    
                     
 BEGIN TRY                    
declare @VacancyStatusTypeId int     
declare @relationshipid int               
               
select       
 @VacancyStatusTypeId  = MAX([VacancyProvisionRelationshipStatusTypeId] )      
from       
 [VacancyProvisionRelationshipStatusType]       
where       
 FullName =  @VacancyStatus                
    
set @VacancyManagerId = 0    
                
--CHECK WHETHER THE RECORD EXISTS     
    
SELECT @VacancyManagerId = [VacancyOwnerRelationshipId]   
FROM [VacancyOwnerRelationship] WHERE     
[EmployerId]=@employerId   
and [ProviderSiteID]=@trainingProviderId  

    
IF(@VacancyManagerId != 0)    
 BEGIN    
    
  UPDATE [dbo].[VacancyOwnerRelationship]    
  SET [ContractHolderisEmployer]=@contractHolderisEmployer,                 
  [EmployerId]=@employerId,                 
  [ManagerIsEmployer]=@managerisEmployer,                 
  [ProviderSiteID]=@trainingProviderId,             
  [EmployerDescription]=@employerDescription,
  [StatusTypeId]=@VacancyStatusTypeId,          
  [Notes]=@Notes  ,    
  [NationWideAllowed] =  @NationwideAllowed                
  WHERE [VacancyOwnerRelationshipId]=@VacancyManagerId    
    
 END    
ELSE    
   BEGIN            
  INSERT INTO [dbo].[VacancyOwnerRelationship]                 
  (                
          
  [ContractHolderisEmployer],                 
  [EmployerId],                 
  [ManagerisEmployer],                 
  [ProviderSiteID],             
  [EmployerDescription],
  [StatusTypeId],          
  [Notes],    
  [NationWideAllowed]                
  )                    
  VALUES                 
  (            
                
  @contractHolderisEmployer,                 
  @employerId,                 
  @managerisEmployer,                 
  @trainingProviderId,                
  @employerDescription,            
  @VacancyStatusTypeId,          
  @Notes     ,    
  @NationwideAllowed           
  )                    
        
  SET @VacancyManagerId = SCOPE_IDENTITY()     
    
 END    
     
    
                        
END TRY                    
        
        
                    
BEGIN CATCH                    
  EXEC RethrowError;                    
END CATCH                    
                        
   SET NOCOUNT OFF                    
END