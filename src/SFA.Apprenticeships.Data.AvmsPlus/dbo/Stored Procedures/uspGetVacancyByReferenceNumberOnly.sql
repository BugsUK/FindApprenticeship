CREATE PROCEDURE [dbo].[uspGetVacancyByReferenceNumberOnly]   
 @VacancyReferenceNumber int  
AS  
BEGIN  
 /* Optimize */  
 SET NOCOUNT ON  
   
 /* Execute */  
 SELECT   
   v.[VacancyId] AS VacancyId,        
   v.[Title] AS Title,        
   coalesce( v.EmployerAnonymousName, e.FullName) as Employer,        
   v.[Town] AS 'Town',   
   v.[ShortDescription] AS 'ShortDescription',        
   v.[ApprenticeshipType] As 'ApprenticeShipType',  
   af.ApprenticeshipFrameworkName As 'Framework',   
   v.ApplicationClosingDate AS 'ClosingDate',        
   vh.[PostedDate] AS 'VacancyPostedDate',   
   v.VacancyStatusId AS 'VacancyStatus',  
   v.NumberofPositions AS 'NoofPositions',  
   v.VacancyLocationTypeId AS 'VacancyLocationTypeId',  
   [COP].[ProviderID] as 'ContractOwnerID',  
 [COP].[TradingName] as 'ContractOwnerName',  
 [DO].[ProviderSiteID] as 'DeliveryOrganisationID',  
 [DO].[TradingName] as 'DeliveryOrganisationName',  
 isnull([DO].CandidateDescription,'') as 'DeliveryOrganisationDesc',  -- trial00050657         
 isnull([DO].ContactDetailsForCandidate,'') as 'DeliveryOrganisationContactNumberforCandidate',   -- trial00050657     
 isnull(tp.[TradingName],'')  as 'TrainingProvider',      
 tp.ProviderSiteID AS 'TrainingProviderId',
 DO.TrainingProviderStatusTypeId AS 'DeliveryOrganisationStatusType',
 VM.ProviderSiteID AS VacancyManagerID,
 VM.TradingName AS VacancyManagerTradingName,
 v.VacancyManagerAnonymous,
 DOP.IsNasProvider,
 DOP.ProviderId as 'DeliveryOrganisationOwnerOrgID',
 VOPSR.ProviderId AS 'VacancyOwnerOwnerOrgID'
 FROM   
  Vacancy v   
 INNER JOIN [VacancyOwnerRelationship] vpr   
  ON v.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]  
 INNER JOIN Employer e  
  ON vpr.EmployerId = e.EmployerId  
 INNER JOIN [ProviderSite] tp  
  ON vpr.[ProviderSiteID] = tp.ProviderSiteID AND tp.TrainingProviderStatusTypeId != 3 
  INNER JOIN dbo.ProviderSiteRelationship VOPSR ON VOPSR.ProviderSiteID = tp.ProviderSiteID AND VOPSR.ProviderSiteRelationshipTypeId = 1
  
 LEFT JOIN Provider COP on COP.ProviderID = v.ContractOwnerID  
 LEFT JOIN ProviderSite DO on DO.ProviderSiteID = v.DeliveryOrganisationID  
 LEFT JOIN ProviderSiteRelationship DOR ON DOR.ProviderSiteId = V.DeliveryOrganisationId AND DOR.ProviderSiteRelationshipTypeId = 1
 LEFT JOIN Provider DOP ON DOP.ProviderId = DOR.ProviderID
 LEFT JOIN ProviderSite VM ON V.VacancyManagerID = VM.ProviderSiteID
 INNER JOIN (SELECT   
     a.ApprenticeshipFrameworkId,   
     a.FullName as ApprenticeshipFrameworkName,  
     ao.Fullname as ApprenticeshipOccupationName  
    FROM   
     apprenticeshipframework a   
     INNER JOIN apprenticeshipoccupation ao ON   
      a.ApprenticeshipOccupationId = ao.ApprenticeshipOccupationId) as af  
  ON v.ApprenticeshipFrameworkId = af.ApprenticeshipFrameworkId  
 INNER JOIN (select   
     VacancyId   
     ,Max(HistoryDate) as PostedDate   
    from vacancyhistory where VacancyHistoryEventSubTypeId = 2   
  GROUP BY vacancyid) AS vh  
 ON vh.vacancyid = v.vacancyid  
WHERE   
 VacancyReferenceNumber = @VacancyReferenceNumber  
  
 /* Reset */  
 SET NOCOUNT OFF  
END