CREATE PROCEDURE [dbo].[uspVacancyManagerUpdate]
@VacancyManagerId INT, @EmployerId INT, @TrainingProviderId INT, @EmployerContractHolder BIT, @EmployerVacancyManager BIT, @RelationshipStatus VARCHAR (100), @Notes NVARCHAR (4000), @EmployerDescription NVARCHAR (MAX), @EmployerWebsite NVARCHAR (256), @EmployerLogoAttachmentId INT, @NationwideAllowed BIT
AS
BEGIN                
 SET NOCOUNT ON                
            
declare @vacancyStatusTypeId int           
select distinct  @vacancyStatusTypeId = VacancyProvisionRelationshipStatusTypeId from VacancyProvisionRelationshipStatusType          
where FullName =@RelationshipStatus  

set @vacancyStatusTypeId=Isnull(@vacancyStatusTypeId,@RelationshipStatus)
        
          
--if @vacancyStatusTypeId is not null          
--begin          
 update [VacancyOwnerRelationship]                
  set              
  StatusTypeId = @vacancyStatusTypeId,               
  ContractHolderisEmployer=@EmployerContractHolder,              
  ManagerIsEmployer=@EmployerVacancyManager,    
  Notes=@Notes,
  EmployerDescription=@EmployerDescription,
  EmployerWebsite=@EmployerWebsite,
  EmployerLogoAttachmentId=@EmployerLogoAttachmentId  ,
  NationWideAllowed =  @NationwideAllowed                     
 where [VacancyOwnerRelationshipId] =@VacancyManagerId              
--End          
                
                  
              
 SET NOCOUNT OFF                
END