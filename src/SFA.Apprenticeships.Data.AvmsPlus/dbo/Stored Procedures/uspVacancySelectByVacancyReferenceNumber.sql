CREATE PROCEDURE [dbo].[uspVacancySelectByVacancyReferenceNumber]       
 @VacancyReferenceNumber int      
AS
BEGIN      
      
 SET NOCOUNT ON      
declare @VacancyId int
       
select @VacancyId = vacancyid from vacancy
where VacancyReferenceNumber = @VacancyReferenceNumber

-- get latest live vacancy status change 
declare @vacancyHistoryId int = null;
select @vacancyHistoryId = max(vh.VacancyHistoryId) 
from VacancyHistory vh
join VacancyHistoryEventType et on et.VacancyHistoryEventTypeId = vh.VacancyHistoryEventTypeId
join VacancyStatusType st on st.VacancyStatusTypeId = vh.VacancyHistoryEventSubTypeId
where   vh.VacancyId = @VacancyId
	and et.CodeName = 'STC'
	and st.CodeName = 'LVE'

select   
  vacancy.VacancyId,        
  Vacancy.[VacancyOwnerRelationshipId] as 'RelationshipId',  
  isnull(vacancy.Title,'') as 'Title',        
  isnull(vacancy.VacancyReferenceNumber,'') as 'VacancyReferenceNumber',        
  isnull(vacancy.ShortDescription,'') as 'ShortDescription',        
  isnull(vacancy.Description,'') as 'Description',        
  vacancy.NumberofPositions as 'NumberofPositions',        
  isnull(vacancy.WorkingWeek,'') as 'WorkingWeek',        
  Vacancy.WeeklyWage as 'WeeklyWage',        
  isnull(vacancy.ContactName,'') as 'ContactName',        
  isnull([Employer].[FullName],'') As 'Employer',        
  --'' as 'EmployerAnonymousName',        
  case when EmployerAnonymousName = null then 0 else 1  
  End as 'EmployerisAnonymous',  
--  EmployerisAnonymous,        
  isnull(EmployerAnonymousName,'') as 'EmployerAnonymousName',        
  isnull([VacancyOwnerRelationship].EmployerDescription,'') As 'EmployerDescription', 
  IsNull([VacancyOwnerRelationship].EmployerLogoAttachmentId,0) As  'EmployerLogoAttachmentId',     
  isnull(Vacancy.EmployersWebsite,'') as 'EmployersWebsite',        
  isnull(vacancy.AddressLine1,'') as 'AddressLine1',         
  isnull(vacancy.AddressLine2,'') as 'AddressLine2',        
  isnull(vacancy.AddressLine3,'') as 'AddressLine3',        
  isnull(vacancy.AddressLine4,'') as 'AddressLine4',        
  isnull(vacancy.Town,'') as 'Town',        
  isnull(vacancy.CountyId,'') as 'CountyId',        
  isnull(County.FullName,'') as 'County',        
  isnull(vacancy.PostCode,'') as 'PostCode',       
  IsNull(vacancy.GeocodeEasting,0) as 'GeocodeEasting',      
  IsNull(vacancy.GeocodeNorthing,0) as 'GeocodeNorthing',      
 IsNull( vacancy.Longitude,0) as 'Longitude',      
  IsNull(vacancy.Latitude,0) as 'Latitude',      
  isnull([ProviderSite].[FullName],'')  as 'TrainingProvider',        
  -- need to check    
isnull(ApprenticeshipOccupation.ApprenticeshipOccupationId,'') as 'ApprenticeshipOccupationId',    
isnull(ApprenticeshipOccupation.Codename,'') as 'ApprenticeshipOccupationCodename',      
isnull(ApprenticeshipOccupation.FullName,'') as 'ApprenticeshipOccupationFullname',        
isnull([ApprenticeshipFramework].[ApprenticeshipFrameworkId],'') as 'ApprenticeshipFrameworkId',     
isnull([ApprenticeshipFramework].[FullName],'') as 'FrameworkName',        
isnull(Vacancy.ApprenticeshipType,'') as 'VacancyType',        
  
isnull(vacancytextfield1.[Value],'') as 'FutureProspects',        
isnull(vacancytextfield2.[Value],'')  as 'Training Available',  
isnull(vacancytextfield3.[Value],'') as 'SkillsRequired',        
isnull(vacancytextfield4.[Value],'') as 'QualificationCriteria',  
isnull(vacancytextfield5.[Value],'') as 'PersonalQualities',    
isnull(vacancytextfield6.[Value],'') as 'RealityCheck',  
isnull(vacancytextfield7.[Value],'') as 'OtherImportantInfo',    
  
isnull(Vacancy.ExpectedDuration,'') as 'ExpectedDuration',        
isnull([ProviderSite].EmployerDescription,'') as 'TrainingProviderDesc',        
isnull(AdditionalQuestion1.Question,'') as 'Question1',        
isnull(AdditionalQuestion2.Question,'') as 'Question2',        
vacancy.ApplicationClosingDate,        
vacancy.InterviewsFromDate,        
vacancy.ExpectedStartDate,          
Vacancy.ApplyOutsideNAVMS as 'ApplyOutsideNAVMS',--True => Candidate should apply using the instruction and link in EmployersApplicationInstructions and EmployersRecruitmentWebsite.  
             --False => Apply via NAVMS, EmployersApplicationInstructions and EmployersRecruitmentWebsite should be null.  
isnull(Vacancy.EmployersRecruitmentWebsite,'') as 'EmployersRecruitmentWebsite',         
vacancy.MaxNumberofApplications,        
isnull(vacancy.EmployersApplicationInstructions,'') as 'EmployersApplicationInstructions',          
vacancy.BeingSupportedBy,        
vacancy.LockedForSupportUntil,        
vh.HistoryDate AS 'VacancyPostedDate' ,       
VacancyStatusType.FullName as Status,
VacancyStatusId as StatusId,
vacancy.NoOfOfflineSystemApplicants
FROM [dbo].[Vacancy] [vacancy]          
left outer join [VacancyOwnerRelationship] on Vacancy.[VacancyOwnerRelationshipId] = [VacancyOwnerRelationship].[VacancyOwnerRelationshipId]  
left outer join Employer on Employer.EmployerId = [VacancyOwnerRelationship].EmployerId  
left outer join [ProviderSite] on [ProviderSite].ProviderSiteID = [VacancyOwnerRelationship].[ProviderSiteID]  
left JOIN ApprenticeshipFramework on [vacancy].[ApprenticeshipFrameworkId] = [ApprenticeshipFramework].[ApprenticeshipFrameworkId]        
left JOIN ApprenticeshipOccupation on ApprenticeshipFramework.ApprenticeshipOccupationId = ApprenticeshipOccupation.ApprenticeshipOccupationId      
left JOIN AdditionalQuestion as AdditionalQuestion1 on [Vacancy].VacancyId = AdditionalQuestion1.VacancyId and AdditionalQuestion1.QuestionId = 1       
left JOIN AdditionalQuestion as AdditionalQuestion2 on [Vacancy].VacancyId = AdditionalQuestion2.VacancyId and AdditionalQuestion2.QuestionId = 2        
left JOIN VacancyStatusType as VacancyStatusType on [Vacancy].VacancyStatusId = VacancyStatusType.VacancyStatusTypeId       

left outer JOIN vacancytextfield vacancytextfield1 	on [vacancy].[VacancyId] = vacancytextfield1.[VacancyId] and vacancytextfield1.Field = 
(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Future prospects') 
left outer JOIN vacancytextfield vacancytextfield2 	on [vacancy].[VacancyId] = vacancytextfield2.[VacancyId] and vacancytextfield1.Field = 
(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Training to be provided') 
left outer JOIN vacancytextfield vacancytextfield3 	on [vacancy].[VacancyId] = vacancytextfield3.[VacancyId] and vacancytextfield1.Field = 
(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Skills required') 
left outer JOIN vacancytextfield vacancytextfield4 	on [vacancy].[VacancyId] = vacancytextfield4.[VacancyId] and vacancytextfield1.Field = 
(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Qualification Required') 
left outer JOIN vacancytextfield vacancytextfield5 	on [vacancy].[VacancyId] = vacancytextfield5.[VacancyId] and vacancytextfield1.Field = 
(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Personal qualities') 
left outer JOIN vacancytextfield vacancytextfield6 	on [vacancy].[VacancyId] = vacancytextfield6.[VacancyId] and vacancytextfield1.Field = 
(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Reality check') 
left outer JOIN vacancytextfield vacancytextfield7 	on [vacancy].[VacancyId] = vacancytextfield7.[VacancyId] and vacancytextfield1.Field = 
(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Other important information') 

Left Outer join County on County.CountyId = Vacancy.CountyId  
-- and AdditionalQuestion2.QuestionId = 2        
left outer JOIN VacancyHistory vh on [vacancy].[VacancyId] = vh.VacancyId 
WHERE [vacancy].VacancyId   = @VacancyId
	and VacancyStatusType.FullName = 'Live'   
	and vh.VacancyHistoryId = @vacancyHistoryId
--AND [vacancyhistory].[Event] = 'Status Live'      
      
      
SET NOCOUNT OFF      
      
END