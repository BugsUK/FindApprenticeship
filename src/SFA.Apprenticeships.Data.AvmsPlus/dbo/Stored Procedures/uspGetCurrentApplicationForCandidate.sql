CREATE PROCEDURE [dbo].[uspGetCurrentApplicationForCandidate]  
   @CandidateId int,  
   @ProviderId int,
   @RecruitmentAgencyId int 
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
DECLARE @NewStatusId int  
DECLARE @AppliedStatusId int  
DECLARE @SuccessStatusId int  
  
SELECT @NewStatusId = applicationStatustypeiD   
FROM applicationStatustype 
-- John Shaw:  Modified the Where predicate to use the Codename
WHERE CodeName = 'NEW'
  
  
SELECT @AppliedStatusId = applicationStatustypeiD   
FROM applicationStatustype  
-- John Shaw:  Modified the Where predicate to use the Codename
WHERE CodeName = 'APP'
  
  
SELECT @SuccessStatusId = applicationStatustypeiD   
FROM applicationStatustype  
-- John Shaw:  Modified the Where predicate to use the Codename
WHERE CodeName = 'SUC'   

IF @RecruitmentAgencyId = 0 SET @RecruitmentAgencyId = null    
           
 SELECT           
    [Application].vacancyid As 'Vacancy Id' ,  
    --ApplicationHistoryId,  
    [Application].[ApplicationId] AS 'ApplicationId'     
   ,[Application].CandidateId As 'Candidate Id'    
--  ISNULL([ApplicationHistory].[ApplicationHistoryEventDate],GETDATE()) as 'Status Change Date',          
  ,Max([ApplicationHistory].[ApplicationHistoryEventDate]) as 'Date Applied'    
  ,[ApplicationHistory].ApplicationHistoryEventSubTypeId  
  --,APPLICATIONSTATUSTYPE.FullName as 'Status FullName'  
  ,Vacancy.Title as 'Vacancy Title'  
  ,[Application].UnsuccessfulReasonId  
  ,[Application].OutcomeReasonOther  
          
 FROM [dbo].[Application]          
   LEFT OUTER JOIN ApplicationHistory ON [Application].[ApplicationId] =   
    [ApplicationHistory].[ApplicationId] 
    AND [ApplicationHistory].[ApplicationHistoryEventSubTypeId] = [Application].ApplicationStatusTypeId --in     (@NewStatusId,@AppliedStatusId,@SuccessStatusId)             
   INNER JOIN Candidate ON [Application].[CandidateId] = [Candidate].[CandidateId]          
   INNER JOIN Person ON [Candidate].[PersonId] = [Person].[PersonId]          
   LEFT OUTER JOIN  SubVacancy ON [SubVacancy].[VacancyId] =[Application].[VacancyId] AND [SubVacancy].[AllocatedApplicationId] = [Application].[ApplicationId]      
   INNER JOIN Vacancy ON Vacancy.VacancyId = Application.VacancyId  
      
INNER JOIN [VacancyOwnerRelationship] VPR ON VPR.[VacancyOwnerRelationshipId] = Vacancy.[VacancyOwnerRelationshipId]  
 WHERE          
    
 [Application].ApplicationStatusTypeId in (@NewStatusId,@AppliedStatusId,@SuccessStatusId)    
   
    AND [Application].[CandidateId] = @CandidateId  
    AND (VPR.[ProviderSiteID] = @ProviderId)  
	AND (@RecruitmentAgencyId IS NULL OR Vacancy.VacancyManagerID = @RecruitmentAgencyId)  
group by   
    [Application].vacancyid  
    --,ApplicationHistoryId,  
    ,[Application].[ApplicationId]   
   ,[Application].CandidateId   
--  ISNULL([ApplicationHistory].[ApplicationHistoryEventDate],GETDATE()) as 'Status Change Date',          
  --,[ApplicationHistory].[ApplicationHistoryEventDate]   
  ,[ApplicationHistory].ApplicationHistoryEventSubTypeId  
  --,APPLICATIONSTATUSTYPE.FullName as 'Status FullName'  
  ,Vacancy.Title   
  ,[Application].UnsuccessfulReasonId  
  ,[Application].OutcomeReasonOther      
  
  
END