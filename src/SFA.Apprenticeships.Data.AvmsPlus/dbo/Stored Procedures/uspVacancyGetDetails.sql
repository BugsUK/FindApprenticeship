CREATE PROCEDURE [dbo].[uspVacancyGetDetails]  
 @VacancyId int   
AS  
BEGIN  
	SELECT          
	 [Vacancy].[VacancyId] AS 'VacancyId',  
	 [Vacancy].[VacancyStatusId] AS 'VacancyStatus',       
	 [Vacancy].[ApplicationClosingDate] AS 'ClosingDate',
	 [Vacancy].[EmployerAnonymousName] AS 'EmployerAnonymousDescription',
	 [Vacancy].[InterviewsFromDate] AS 'InterviewFromDate',   
	 [Vacancy].[VacancyManagerAnonymous] AS 'IsRecruitmentAgncyAnonymous',  
	 [Vacancy].[SmallEmployerWageIncentive] AS 'IsSmallEmployerWageIncentive',
	 [Vacancy].[ExpectedStartDate] AS 'PossibleStartDate',
	 [Vacancy].[VacancyManagerId] as 'VacancyManagerId',
	 [Vacancy].[WageType] AS 'WageType',
	 [Vacancy].[WeeklyWage] AS 'WeeklyWage',        
	 [Vacancy].[LockedForSupportUntil] AS 'LockedForSupportUntil', 
	 isnull([Vacancy].[MasterVacancyId], 0) as 'MasterVacancyId',
	 isnull([Vacancy].[ApprenticeshipType],'') AS 'ApprenticeshipType',
	 isnull([Vacancy].[VacancyReferenceNumber],'') AS 'VacancyReference', 
	 isnull([Vacancy].[EmployersApplicationInstructions],'') AS 'ApplicationInstructions',
	 isnull([Vacancy].[ApplyOutsideNAVMS], 0) AS 'ApplyViaEmployerWebsite', 
	 isnull([Vacancy].[EmployersWebsite],'') AS 'EmployerWebsite',
	 isnull([Vacancy].[ExpectedDuration],'') AS 'ExpectedDuration',
	 isnull([Vacancy].[Description],'') AS 'FullDescription', 
	 isnull([Vacancy].[ShortDescription],'') AS 'ShortDescription',
	 isnull([Vacancy].[VacancyLocationTypeId], 1) AS 'VacancyLocationType',  
	 isnull([Vacancy].[Title],'') AS 'VacancyTitle',
	 isnull([Vacancy].[EmployersRecruitmentWebsite],'') AS 'EmployerRecruitmentWebsite', 
	 isnull([Vacancy].[NumberofPositions], 0) AS 'NumberOfPositions',
	 isnull([Vacancy].[NoOfOfflineApplicants], 0) AS 'NoOfOfflineApplicants',
	 isnull([Vacancy].[NoOfOfflineSystemApplicants], 0) AS 'NoOfOfflineSystemApplicants',
	 isnull([Vacancy].[WageText], '') AS 'WageText',
	 isnull([Vacancy].[WorkingWeek], '') AS 'WorkingWeek',
	 isnull([Vacancy].[ContactName], '') as 'ContactName', 
	 
	 isnull([Vacancy].[AddressLine1], '') AS 'AddressLine1',
	 isnull([Vacancy].[AddressLine2], '') AS 'AddressLine2',
	 isnull([Vacancy].[AddressLine3], '') AS 'AddressLine3',
	 isnull([Vacancy].[AddressLine4], '') AS 'AddressLine4',
	 isnull([Vacancy].[Latitude], 0) AS 'Latitude',
	 isnull([Vacancy].[Longitude], 0) AS 'Longitude',
	 isnull([Vacancy].[PostCode], '') AS 'PostCode',
	 isnull([Vacancy].[Town], '') AS 'Town',
	 isnull([Vacancy].[CountyId], '') AS 'County',
	 isnull([Vacancy].[GeocodeEasting], 0) as 'GeocodeEasting',	 
	 isnull([Vacancy].[GeocodeNorthing], 0) as 'GeocodeNorthing',
     isnull([Vacancy].[LocalAuthorityId], 0) as 'LocalAuthorityId',

	 isnull(vacancytextfield1.[Value],'') AS 'FutureProspects',
	 isnull(vacancytextfield2.[Value],'') AS 'TrainingRequired',
	 isnull(vacancytextfield3.[Value],'') AS 'SkillsRequired',
	 isnull(vacancytextfield5.[Value],'') AS 'PersonalQualities',
	 isnull(vacancytextfield4.[Value],'') AS 'QualificationRequired',
	 isnull(vacancytextfield6.[Value],'') AS 'RealityCheck',
	 isnull(vacancytextfield7.[Value],'') AS 'ImportantOtherInfo',

	 isnull(AdditionalQuestion1.[Question],'') AS 'SupplementaryQuestion1',
	 isnull(AdditionalQuestion2.[Question],'') AS 'SupplementaryQuestion2',

	 VH.HistoryDate AS 'VacancyPostedDate',

	 contractOwnerProvider.[TradingName] AS 'ContractedProviderName',
	 
	 [Employer].[EmployerId] AS 'EmployerId',
	 [Employer].[TradingName] AS 'TradingName',
	 [Employer].[DisableAllowed] AS 'IsDisableAllowed',
     [Employer].[TrackingAllowed] AS 'IsTrackingAllowed',  
	 isnull([Employer].[FullName],'') AS 'EmployerName', 	     

	 isnull([VacancyOwnerRelationship].[EmployerDescription],'') AS 'EmployerDescription',
	 IsNull([VacancyOwnerRelationship].EmployerLogoAttachmentId,0) As  'EmployerLogoAttachmentId',

	 vacancyManager.[TradingName] AS 'VacancyManagerTradingName',
	 
	 isnull(ssr.[PassRate], 0) AS 'ApprFrameworkSuccessRate', 
     isnull(ssr.New, 0) As 'ApprFrameworkNewSector',
	               
	 isnull([ApprenticeshipFramework].[FullName],'') AS 'ApprenticeshipFrameworkFullName',
	 isnull([ApprenticeshipFramework].[ApprenticeshipFrameworkId],'') as 'ApprenticeshipFrameworkId',

	 isnull([ApprenticeshipOccupation].[FullName],'') AS 'ApprenticeshipOccupationFullName',
	 isnull([ApprenticeshipOccupation].[ApprenticeshipOccupationId],'') as 'ApprenticeshipOccupationId',
	 
	 isnull(deliveryOrganisationProvider.[IsNasProvider], 0) AS 'IsNasProvider',
	 deliveryOrganisationProvider.[ProviderId] AS 'DeliveryOrganisationOwnerOrgId',
	 deliveryOrganisation.[ProviderSiteID] AS 'LearningDeliverySiteId',
	 deliveryOrganisation.[TradingName] as 'LearningDeliverySiteName',  
	 deliveryOrganisation.[TrainingProviderStatusTypeId] AS 'LearningDeliverySiteStatus',
	 isnull(deliveryOrganisation.[ContactDetailsForCandidate],'') AS 'LearningDeliverySiteContactDetailsForCandidate',
	 isnull(deliveryOrganisation.[CandidateDescription],'') as 'LearningDeliverySiteDescription',
     
     [ProviderSite].[ProviderSiteID] AS 'TrainingProviderId',      
	 isnull([ProviderSite].[TradingName], '')  as 'TrainingProviderName', 
	 isnull([ProviderSite].[WebPage], '') AS 'WebsiteAddress',
	 isnull([ProviderSite].[CandidateDescription],'') AS 'TrainingProviderDesc', 
	 isnull([ProviderSite].[ContactDetailsForCandidate],'') AS 'ContactForCandidate',

	 [ProviderSiteRelationShip].[ProviderId] AS 'VacancyOwnerOwnerOrgId'
	 
	FROM [dbo].[Vacancy] [Vacancy]                
	 JOIN [VacancyOwnerRelationship] ON [Vacancy].[VacancyOwnerRelationshipId] = [VacancyOwnerRelationship].[VacancyOwnerRelationshipId]        
	 JOIN [Employer] ON [Employer].[EmployerId] = [VacancyOwnerRelationship].[EmployerId]        
	 JOIN [ProviderSiteRelationship] ON [ProviderSiteRelationship].[ProviderSiteID] = [VacancyOwnerRelationship].[ProviderSiteID] AND [ProviderSiteRelationship].[ProviderSiteRelationshipTypeId] = 1
	 JOIN [ProviderSite] ON [ProviderSiteRelationship].[ProviderSiteID] = [ProviderSite].[ProviderSiteID]
	   
	 LEFT JOIN [Provider] contractOwnerProvider ON contractOwnerProvider.[ProviderID] = [Vacancy].[ContractOwnerID]  
	 LEFT JOIN [ProviderSite] deliveryOrganisation ON deliveryOrganisation.[ProviderSiteID] = [Vacancy].[DeliveryOrganisationID]  
	 LEFT JOIN [ProviderSite] vacancyManager ON vacancyManager.[ProviderSiteID] = [Vacancy].[VacancyManagerID]  
	 LEFT JOIN [ProviderSiteRelationship] deliveryOrganisationRelationship ON deliveryOrganisationRelationship.[ProviderSiteID] = [Vacancy].[DeliveryOrganisationID] AND deliveryOrganisationRelationship.[ProviderSiteRelationshipTypeId] = 1
	 LEFT JOIN [Provider] deliveryOrganisationProvider ON deliveryOrganisationProvider.[ProviderID] = deliveryOrganisationRelationship.[ProviderID]
	        
	 LEFT JOIN [ApprenticeshipFramework] ON [Vacancy].[ApprenticeshipFrameworkId] = [ApprenticeshipFramework].[ApprenticeshipFrameworkId]
	 LEFT JOIN [ApprenticeshipOccupation] ON [ApprenticeshipFramework].[ApprenticeshipOccupationId] = [ApprenticeshipOccupation].[ApprenticeshipOccupationId]            
	 LEFT JOIN [AdditionalQuestion] as AdditionalQuestion1 ON [Vacancy].VacancyId = AdditionalQuestion1.VacancyId AND AdditionalQuestion1.QuestionId = 1             
	 LEFT JOIN [AdditionalQuestion] as AdditionalQuestion2 ON [Vacancy].VacancyId = AdditionalQuestion2.VacancyId AND AdditionalQuestion2.QuestionId = 2              
	       
	 LEFT JOIN [SectorSuccessRates] ssr ON contractOwnerProvider.[ProviderID] = ssr.[ProviderID] AND [ApprenticeshipFramework].[ApprenticeshipOccupationId] = ssr.SectorId  
	  
	 LEFT JOIN [VacancyTextField] vacancytextfield1  ON [Vacancy].[VacancyId] = vacancytextfield1.[VacancyId] AND vacancytextfield1.[Field] = 6 --(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Future prospects')       
	 LEFT JOIN [VacancyTextField] vacancytextfield2  ON [Vacancy].[VacancyId] = vacancytextfield2.[VacancyId] AND vacancytextfield2.[Field] = 1 --(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Training to be provided')
	 LEFT JOIN [VacancyTextField] vacancytextfield3  ON [Vacancy].[VacancyId] = vacancytextfield3.[VacancyId] AND vacancytextfield3.[Field] = 3 --(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Skills required')       
	 LEFT JOIN [VacancyTextField] vacancytextfield4  ON [Vacancy].[VacancyId] = vacancytextfield4.[VacancyId] AND vacancytextfield4.[Field] = 2 --(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Qualifications Required')       
	 LEFT JOIN [VacancyTextField] vacancytextfield5  ON [Vacancy].[VacancyId] = vacancytextfield5.[VacancyId] AND vacancytextfield5.[Field] = 4 --(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Personal qualities')       
	 LEFT JOIN [VacancyTextField] vacancytextfield6  ON [Vacancy].[VacancyId] = vacancytextfield6.[VacancyId] AND vacancytextfield6.[Field] = 7 --(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Reality check')       
	 LEFT JOIN [VacancyTextField] vacancytextfield7  ON [Vacancy].[VacancyId] = vacancytextfield7.[VacancyId] AND vacancytextfield7.[Field] = 5 --(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Other important information')       
	 
	 LEFT JOIN (SELECT vh1.VacancyID, MAX(vh1.HistoryDate) AS HistoryDate
				FROM [VacancyHistory] vh1
				WHERE vh1.VacancyHistoryEventSubTypeID = 2
				AND VacancyHistoryEventTypeID = 1
				GROUP BY vh1.VacancyID) VH
	ON Vacancy.VacancyID = VH.VacancyID

	WHERE [Vacancy].[VacancyId]  = @VacancyId
END