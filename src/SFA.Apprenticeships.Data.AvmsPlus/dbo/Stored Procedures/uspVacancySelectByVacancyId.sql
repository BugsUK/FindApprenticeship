CREATE PROCEDURE [dbo].[uspVacancySelectByVacancyId]  
@vacancyId INT  
AS  
BEGIN              
              
SET NOCOUNT ON              
             
Select         
 vacancy.VacancyId,  
 isnull(vacancy.MasterVacancyId,0) as 'MasterVacancyId',              
 Vacancy.[VacancyOwnerRelationshipId] as 'RelationshipId',        
 isnull(vacancy.Title,'') as 'Title',              
 isnull(vacancy.VacancyReferenceNumber,'') as 'VacancyReferenceNumber',              
 isnull(vacancy.ShortDescription,'') as 'ShortDescription',              
 isnull(vacancy.Description,'') as 'Description',              
 vacancy.NumberofPositions as 'NumberofPositions',              
 isnull(vacancy.WorkingWeek,'') as 'WorkingWeek',              
 Vacancy.WeeklyWage as 'WeeklyWage',
 Vacancy.WageType as 'WageType',
 isnull(Vacancy.WageText,'') as 'WageText',              
 isnull(vacancy.ContactName,'') as 'ContactName',              
 isnull([Employer].[FullName],'') As 'Employer',              
 --'' as 'EmployerAnonymousName',              
 case when EmployerAnonymousName IS NULL then 0 else 1        
 End as 'EmployerisAnonymous',        
 --  EmployerisAnonymous,              
 EmployerAnonymousName as 'EmployerAnonymousName',              
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
 IsNull(vacancy.LocalAuthorityId,0) as 'LocalAuthorityId',   
 isnull([ProviderSite].[TradingName],'')  as 'TrainingProvider',      
 [ProviderSite].ProviderSiteID AS 'TrainingProviderId',      
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
 isnull([ProviderSite].CandidateDescription,'') as 'TrainingProviderDesc',  -- trial00050657         
 isnull([ProviderSite].ContactDetailsForCandidate,'') as 'ContactNumberforCandidate',   -- trial00050657         
 isnull([ProviderSite].WebPage,'') as 'WebsiteAddress',   -- trial00050657         
  
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
 VacancyHistory.HistoryDate AS 'VacancyPostedDate' ,             
 VacancyStatusType.FullName as Status,      
 VacancyStatusId as StatusId,       
 [Employer].TradingName,      
 employer.employerid AS 'EmployerId',  
 employer.TrackingAllowed,  
 --tpf.PassRate As ApprFrameworkSuccessRate   
 ssr.PassRate As ApprFrameworkSuccessRate,  
 ssr.New As NewSector   
   
  , [ProviderSite].ContactDetailsForEmployer as 'ContactDetailsForEmployer',  
    vacancy.NoOfOfflineApplicants,  
    isnull(Vacancy.VacancyLocationTypeId,1) as VacancyLocationTypeId,  
 [Employer].DisableAllowed,  
    Vacancy.NoOfOfflineSystemApplicants,  
 [COP].[ProviderID] as 'ContractOwnerID',  
 [COP].[TradingName] as 'ContractOwnerName',  
 [DO].[ProviderSiteID] as 'DeliveryOrganisationID',  
 [DO].[TradingName] as 'DeliveryOrganisationName',  
 isnull([DO].CandidateDescription,'') as 'DeliveryOrganisationDesc',  -- trial00050657 ,
 [DO].[TrainingProviderStatusTypeId] as 'DeliveryOrganisationStatusId',
 isnull([DO].ContactDetailsForCandidate,'') as 'DeliveryOrganisationContactNumberforCandidate',   -- trial00050657         
 Vacancy.SmallEmployerWageIncentive,     -- CCR11983  
 Vacancy.VacancyManagerID as 'VacancyManagerID',  
 MO.TradingName as 'VacancyManagerTradingName',
 vacancy.VacancyManagerAnonymous,
 DOP.IsNasProvider,
 DOP.ProviderId as 'DeliveryOrganisationOwnerOrgID',
 [ProviderSiteRelationship].ProviderID as 'VacancyOwnerOwnerOrgID'
FROM [dbo].[Vacancy] [vacancy]                
 join [VacancyOwnerRelationship] on Vacancy.[VacancyOwnerRelationshipId] = [VacancyOwnerRelationship].[VacancyOwnerRelationshipId]        
 join Employer on Employer.EmployerId = [VacancyOwnerRelationship].EmployerId        
 join [ProviderSiteRelationship] on [ProviderSiteRelationship].[ProviderSiteID] = [VacancyOwnerRelationship].[ProviderSiteID]  AND [ProviderSiteRelationship].ProviderSiteRelationshipTypeId = 1
 JOIN ProviderSIte on ProviderSIteRElationship.ProviderSiteID = ProviderSite.ProviderSiteID  
 JOIN Provider on ProviderSiteRelationShip.ProviderID = Provider.ProviderID  
 
  
 LEFT JOIN Provider COP on COP.ProviderID = ContractOwnerID  
 LEFT JOIN ProviderSite DO on DO.ProviderSiteID = DeliveryOrganisationID  
 LEFT JOIN ProviderSite MO on MO.ProviderSiteID = VacancyManagerID  
 LEFT JOIN ProviderSiteRelationship DOR on DOR.ProviderSiteId = DeliveryOrganisationID AND DOR.ProviderSiteRelationshipTypeId = 1
 LEFT JOIN Provider DOP on DOP.ProviderId = DOR.ProviderId
        
 left JOIN ApprenticeshipFramework on [vacancy].[ApprenticeshipFrameworkId] = [ApprenticeshipFramework].[ApprenticeshipFrameworkId]              
 left JOIN ApprenticeshipOccupation on ApprenticeshipFramework.ApprenticeshipOccupationId = ApprenticeshipOccupation.ApprenticeshipOccupationId            
 left JOIN AdditionalQuestion as AdditionalQuestion1 on [Vacancy].VacancyId = AdditionalQuestion1.VacancyId and AdditionalQuestion1.QuestionId = 1             
 left JOIN AdditionalQuestion as AdditionalQuestion2 on [Vacancy].VacancyId = AdditionalQuestion2.VacancyId and AdditionalQuestion2.QuestionId = 2              
 left JOIN VacancyStatusType as VacancyStatusType on [Vacancy].VacancyStatusId = VacancyStatusType.VacancyStatusTypeId             
       
 left JOIN SectorSUccessRates ssr on COP .ProviderID = ssr.ProviderID AND ApprenticeshipOccupation.ApprenticeshipOccupationId = ssr.SectorId  
  
 left outer JOIN vacancytextfield vacancytextfield1  on [vacancy].[VacancyId] = vacancytextfield1.[VacancyId] and vacancytextfield1.Field =       
 (Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Future prospects')       
 left outer JOIN vacancytextfield vacancytextfield2  on [vacancy].[VacancyId] = vacancytextfield2.[VacancyId] and vacancytextfield2.Field =       
 (Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Training to be provided')       
 left outer JOIN vacancytextfield vacancytextfield3  on [vacancy].[VacancyId] = vacancytextfield3.[VacancyId] and vacancytextfield3.Field =       
 (Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Skills required')       
 left outer JOIN vacancytextfield vacancytextfield4  on [vacancy].[VacancyId] = vacancytextfield4.[VacancyId] and vacancytextfield4.Field =       
 (Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Qualifications Required')       
 left outer JOIN vacancytextfield vacancytextfield5  on [vacancy].[VacancyId] = vacancytextfield5.[VacancyId] and vacancytextfield5.Field =       
 (Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Personal qualities')       
 left outer JOIN vacancytextfield vacancytextfield6  on [vacancy].[VacancyId] = vacancytextfield6.[VacancyId] and vacancytextfield6.Field =       
 (Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Reality check')       
 left outer JOIN vacancytextfield vacancytextfield7  on [vacancy].[VacancyId] = vacancytextfield7.[VacancyId] and vacancytextfield7.Field =       
 (Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Other important information')       
       
 Left Outer Join [ProviderSiteFramework] tpf On tpf.ProviderSiteRelationshipID = [VacancyOwnerRelationship].[ProviderSiteID]  
  AND vacancy.apprenticeshipframeworkid = tpf.frameworkid  
  
 --Left Outer Join TrainingProviderSector tps On tps.TrainingProviderId = [VacancyOwnerRelationship].[ProviderSiteRelationshipID]  
 -- AND tps.TrainingProviderSectorId = tpf.TrainingProviderSectorId  
  
 Left Outer join County on County.CountyId = Vacancy.CountyId        
 -- and AdditionalQuestion2.QuestionId = 2              
 left outer JOIN       
  VacancyHistory on       
  [vacancy].[VacancyId] = [VacancyHistory].VacancyId       
   and       
    VacancyHistoryEventTypeId = 1   
    and VacancyHistoryEventSubTypeId = (      
  select        
   VacancyStatusTypeId       
  from       
   VacancyStatusType       
  where VacancyStatusType.[FullName] = 'Live')          
 WHERE [vacancy].[VacancyId]  = @VacancyId     
        
 SET NOCOUNT OFF              
END