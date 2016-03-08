CREATE PROCEDURE [dbo].[uspGetVacancyByVacancyId]   
	@vacancyId int  
AS  
BEGIN  
  
 SET NOCOUNT ON  
 SELECT  
  [vacancy].[Title] AS 'Title',  
  [vacancy].[Description] AS 'Description',  
  [vacancy].[ShortDescription] AS 'ShortDescription',  
  [Vacancy].[ApprenticeshipType] As 'Vacancy Type',  
  isnull([vacancy].[AddressLine1],'') + ' ' + isnull([vacancy].[AddressLine2],'') + ' ' + isnull([vacancy].[AddressLine3],'') + ' ' + isnull([vacancy].[AddressLine4],'') + ' ' +isnull([vacancy].[Town],'') + ' ' +  isnull(County.FullName,'') + ' '  + isnull([vacancy].[PostCode],'') collate Latin1_General_CI_AS As  'Vacancy Location',  
   [vacancy].[WorkingWeek] AS 'HoursofWork',  
  [vacancy].[WeeklyWage] AS 'WagesDuringApprenticeship',  
  [vacancy].[VacancyReferenceNumber] AS 'Vacancy Reference Number',  
  [vacancy].[ExpectedDuration] AS 'ExpectedDuration',  
  --[Vacancy].[VacancyPostedDate] AS 'Vacancy Posted Date',  
  VacancyHistory.HistoryDate AS 'Vacancy Posted Date' ,       
  [vacancy].[ApplicationClosingDate] AS 'ApplicationClosingDate',  
  [vacancy].[InterviewsFromDate] AS 'InterviewsFromDate',  
  [vacancy].[ExpectedStartDate] AS 'ExpectedStartDate',  
  [ApprenticeshipFramework].[Fullname] AS 'Occupation and Framework',  
  
  isnull(vt.FutureProspects,'') as 'FutureProspects',        
isnull(vt.TrainingToBeProvided,'')  as 'Training Available',  
isnull(vt.SkillsRequired,'') as 'SkillsRequired',        
isnull(vt.QualificationRequired,'') as 'QualificationCriteria',  
isnull(vt.PersonalQualities,'') as 'PersonalQualities',    
isnull(vt.RealityCheck,'') as 'RealityCheck',  
isnull(vt.OtherImportantInformation,'') as 'OtherImportantInfo', 
Vacancy.ApplyOutsideNAVMS as 'ApplyOutsideNAVMS',--True => Candidate should apply using the instruction and link in EmployersApplicationInstructions and EmployersRecruitmentWebsite.  
             --False => Apply via NAVMS, EmployersApplicationInstructions and EmployersRecruitmentWebsite should be null.  
   
--  [vacancy].[ApplicationRoute] AS 'Application Route',
  [vacancy].[EmployersApplicationInstructions] AS 'EmployersApplicationInstructions',
  [vacancy].[EmployersRecruitmentWebsite] AS 'EmployersRecruitmentWebsite',
  [Employer].[FullName] As 'Employer',  
  [ProviderSite].[TradingName] as 'Training Provider',  
  --[Trainingprovider].[SuccessRate] as 'SuccessRate',  
  --[Trainingprovider].[OFSTEDDate] as 'Up To Date',  
  --[vacancy].[EmployerisAnonymous] As 'IsEmployerAnonymous'  
  case when EmployerAnonymousName = null then 0 else 1  
  End as 'IsEmployerAnonymous'  
 FROM [dbo].[Vacancy] [vacancy]   
  inner join [VacancyOwnerRelationship] on Vacancy.[VacancyOwnerRelationshipId] = [VacancyOwnerRelationship].[VacancyOwnerRelationshipId]  
  inner join Employer on Employer.EmployerId = [VacancyOwnerRelationship].EmployerId  
  inner join [ProviderSite] on [ProviderSite].ProviderSiteID = [VacancyOwnerRelationship].[ProviderSiteID]   
  INNER JOIN ApprenticeshipFramework on [vacancy].[ApprenticeshipFrameworkId] = [ApprenticeshipFramework].[ApprenticeshipFrameworkId]  
  INNER JOIN VacancyStatusType ON [vacancy].[VacancyStatusId] = [VacancyStatusType].[VacancyStatusTypeId]  
  LEFT OUTER JOIN (select vacancyid , 
	Max(case when FullName = 'Future prospects' then vtf.[Value] end) as FutureProspects,
	Max(case when FullName = 'Training to be provided' then vtf.[Value] end) as TrainingToBeProvided,
	Max(case when FullName = 'Skills required' then vtf.[Value] end) as SkillsRequired,
	Max(case when FullName = 'Qualifications Required' then vtf.[Value] end) as QualificationRequired,
	Max(case when FullName = 'Personal qualities' then vtf.[Value] end) as PersonalQualities,
	Max(case when FullName = 'Reality check' then vtf.[Value] end) as RealityCheck,
	Max(case when FullName = 'Other important information' then vtf.[Value] end) as OtherImportantInformation
		from vacancytextfieldValue vtfv inner join vacancytextfield vtf 
		on vtf.Field = vtfv.vacancytextfieldValueId 
		group by vacancyid) as vt on vt.vacancyId = vacancy.vacancyid
  Left Outer join County on County.CountyId = Vacancy.CountyId  
  left outer JOIN VacancyHistory on [vacancy].[VacancyId] = [VacancyHistory].VacancyId and VacancyHistoryEventSubTypeId = (select  VacancyStatusTypeId from VacancyStatusType where [FullName] = 'Live')    

 WHERE [vacancy].[VacancyId]  = @VacancyId AND  
   [VacancyStatusType].[FullName] = 'Live'  
  
 SET NOCOUNT OFF  
END