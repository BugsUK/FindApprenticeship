CREATE PROCEDURE [dbo].[uspGetPastApplicationForCandidate]
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
DECLARE @UnspecStatusId int
DECLARE @DraftStatusId int

-- NEW
SELECT @NewStatusId = applicationStatustypeiD   
FROM applicationStatustype  
-- John Shaw:  Modified the Where predicate to use the Codename
WHERE CodeName = 'NEW' 
  

--IN PROGRESS (APPLIED)
SELECT @AppliedStatusId = applicationStatustypeiD   
FROM applicationStatustype  
-- John Shaw:  Modified the Where predicate to use the Codename
WHERE CodeName = 'APP' 
  

--SUCCESSFUL
SELECT @SuccessStatusId = applicationStatustypeiD   
FROM applicationStatustype  
-- John Shaw:  Modified the Where predicate to use the Codename
WHERE CodeName = 'SUC'

--DRAFT (UNSENT)
SELECT @DraftStatusId = applicationStatustypeiD 
FROM applicationStatustype 
-- John Shaw:  Modified the Where predicate to use the Codename
WHERE CodeName = 'DRF'

IF @RecruitmentAgencyId = 0 SET @RecruitmentAgencyId = null   
         
 SELECT         
    --ApplicationHistoryId,
    [Application].[ApplicationId] AS 'ApplicationId'
   ,[Application].vacancyid As 'Vacancy Id' 
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
    AND [ApplicationHistory].[ApplicationHistoryEventSubTypeId] =[Application].ApplicationStatusTypeId --Not in     (@NewStatusId,@AppliedStatusId,@SuccessStatusId,@DraftStatusId)           
   INNER JOIN Candidate ON [Application].[CandidateId] = [Candidate].[CandidateId]        
   INNER JOIN Person ON [Candidate].[PersonId] = [Person].[PersonId]        
   LEFT OUTER JOIN  SubVacancy ON [SubVacancy].[VacancyId] =[Application].[VacancyId] AND [SubVacancy].[AllocatedApplicationId] = [Application].[ApplicationId]    
   INNER JOIN Vacancy ON Vacancy.VacancyId = Application.VacancyId
   --INNER JOIN APPLICATIONSTATUSTYPE ON 
   -- APPLICATIONSTATUSTYPE.ApplicationStatusTypeId = Application.ApplicationStatusTypeId
INNER JOIN [VacancyOwnerRelationship] VPR ON VPR.[VacancyOwnerRelationshipId] = Vacancy.[VacancyOwnerRelationshipId]



 WHERE        
    ApplicationHistoryEventSubTypeId Not in (@NewStatusId,@AppliedStatusId,@SuccessStatusId,@DraftStatusId)          
   AND [Application].ApplicationStatusTypeId Not in (@NewStatusId,@AppliedStatusId,@SuccessStatusId,@DraftStatusId) 
    AND
    [Application].[CandidateId] = @CandidateId
      AND (VPR.[ProviderSiteID] = @ProviderId)
	  AND (@RecruitmentAgencyId IS NULL OR Vacancy.VacancyManagerID = @RecruitmentAgencyId)  
group by 
    [Application].vacancyid
    --,ApplicationHistoryId,
    ,[Application].[ApplicationId] 
   ,[Application].CandidateId 
--  ISNULL([ApplicationHistory].[ApplicationHistoryEventDate],GETDATE()) as 'Status Change Date',        
  --[ApplicationHistory].[ApplicationHistoryEventDate] 
  ,[ApplicationHistory].ApplicationHistoryEventSubTypeId
  --,APPLICATIONSTATUSTYPE.FullName as 'Status FullName'
  ,Vacancy.Title 
  ,[Application].UnsuccessfulReasonId
  ,[Application].OutcomeReasonOther  

END