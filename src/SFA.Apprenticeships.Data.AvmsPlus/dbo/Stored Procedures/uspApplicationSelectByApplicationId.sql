CREATE PROCEDURE [dbo].[uspApplicationSelectByApplicationId]         
 @ApplicationId int           
AS        
BEGIN        
        
 SET NOCOUNT ON        
         
select       
	ApplicationId,      
	CandidateId,      
	[Application].VacancyId,      
	[Application].ApplicationStatusTypeId as 'ApplicationStatusTypeId',      
	null as UniqueApplicationReference,    --TODO: Update dbml  
	[Application].UnsuccessfulReasonId as 'WithdrawnOrDeclinedReasonId',  
	ApplicationUnsuccessfulReasonType.FullName as 'ApplicationWithdrawnOrDeclinedReason', -- 'OutcomeReason'     
	isnull(OutcomeReasonOther,'') as 'OutcomeReasonOther',      
	ApplicationStatusType.CodeName as 'ApplicationStatus',    
	isnull(ShortDescription,'') as 'VacancyShortDescription',    
	isnull(Description,'') as 'VacancyDescription',    
	isnull(Title,'') as 'VacancyTitle',    
	isnull(AllocatedTo,'') as 'AllocatedTo',    
	CVAttachmentId,  
	SubVacancy.startdate  as 'ILR StartDate',  
	--SubVacancy.ILRNumber  as 'ILR Number',  
	  
	Isnull(NextActionId,0) as 'NextActionId',  
	isnull(NextActionOther,'') as 'NextActionOther',  
	emp.DisableAllowed as 'AllowsDisabled'
  
from [Application]      
	inner join ApplicationStatusType on [Application].ApplicationStatusTypeId = ApplicationStatusType.ApplicationStatusTypeId      
	INNER JOIN vacancy on vacancy.vacancyId = [Application].vacancyId 
	INNER JOIN [VacancyOwnerRelationship] vpr on vpr.[VacancyOwnerRelationshipId] = vacancy.[VacancyOwnerRelationshipId]
	inner join Employer emp on emp.EmployerId = vpr.EmployerId
	LEFT OUTER JOIN SubVacancy on [Application].ApplicationId = SubVacancy.AllocatedApplicationId AND [Application].[VacancyId] = [SubVacancy].[vacancyId]
    Left Outer join ApplicationUnsuccessfulReasonType on ApplicationUnsuccessfulReasonType.ApplicationUnsuccessfulReasonTypeId = [Application].UnsuccessfulReasonId
where ApplicationId = @ApplicationId      
        
SET NOCOUNT OFF        
END