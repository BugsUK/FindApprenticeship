CREATE PROCEDURE [dbo].[uspRegisteredRecruitmentAgencyDetailsSelectByUrnList]
	@urnList VARCHAR (MAX), 
	@UKPRN int,
	@VacancyOwnerId int
AS
BEGIN              
	-- SET NOCOUNT ON added to prevent extra result sets from              
	-- interfering with SELECT statements.              
	SET NOCOUNT ON;              
   
   -- get the provider id for the vacancy owner
	DECLARE @providerId int
	SELECT @providerId = ProviderID
	FROM ProviderSiteRelationship 
	WHERE ProviderSiteID=@VacancyOwnerId AND ProviderSiteRelationShipTypeID = 1

	SELECT	ProviderSite.ProviderSiteID, 
			ProviderSite.EdsUrn  AS 'EdsUrn',
			COUNT(distinct VacancyID) AS ActiveVacancies,
			isnull(ProviderSite.FullName,'') AS 'FullName', 
			isnull(ProviderSite.TradingName,'') AS 'TradingName', 
			isnull(ProviderSite.OwnerOrganisation,'') as 'OwnerOrgnistaion', 
            isnull(ProviderSite.AddressLine1,'') as 'AddressLine1', 
            isnull(ProviderSite.AddressLine2,'') as 'AddressLine2',  
            isnull(ProviderSite.AddressLine3,'') as 'AddressLine3', 
            isnull(ProviderSite.AddressLine4,'') as 'AddressLine4', 
            isnull(ProviderSite.Town,'') as 'Town',  
            County.FullName AS County, 
            ProviderSite.PostCode,
            CASE WHEN EXISTS (Select 1 from ProviderSiteRelationship PSR JOIN Provider P ON
				 PSR.ProviderId = P.ProviderID
            AND PSR.ProvidersiteRelationshipTypeID = 3 AND P.UKPRN = @ukprn AND P.ProviderStatusTypeID <> 2
            AND PSR.ProviderSiteID = ProviderSite.ProviderSiteID )
                      THEN 1
                      ELSE 0
                      END AS Linked,
			ISNULL(TempEmployer.NumLinkedEmployers, 0) as 'NumLinkedEmployers'
	FROM  ProviderSite 
		INNER JOIN County ON County.CountyId = ProviderSite.CountyId 
		LEFT JOIN ProviderSiteRelationship ON ProviderSiteRelationship.ProviderSiteID = ProviderSite.ProviderSiteID AND ProviderSiteRelationship.ProviderSiteRelationShipTypeID = 3
		INNER JOIN (SELECT * from fnx_SplitListToTable(@urnList)) as TEMP on TEMP.ID = [ProviderSite].[EdsUrn]
        LEFT OUTER JOIN 
				(
					SELECT Vacancy.VacancyID, Vacancy.VacancyManagerID, VacancyOwnerRelationship.ProviderSiteID, ProviderSiteRelationship.ProviderID 
					FROM Vacancy 
						INNER JOIN VacancyOwnerRelationship 
							ON VacancyOwnerRelationship.VacancyOwnerRelationshipId = Vacancy.VacancyOwnerRelationshipId 
						INNER JOIN ProviderSiteRelationship 
							ON ProviderSiteRelationship.ProviderSiteID = VacancyOwnerRelationship.ProviderSiteID AND ProviderSiteRelationship.ProviderSiteRelationShipTypeID = 1
						WHERE Vacancy.VacancyStatusId in (1,2,3,5)
				) TempVacancy ON TempVacancy.VacancyManagerID = ProviderSite.ProviderSiteID AND TempVacancy.ProviderID = @providerId
		LEFT OUTER JOIN 
				(
					SELECT ProviderSiteRelationshipID, COUNT(DISTINCT vor.EmployerId) as 'NumLinkedEmployers'
					FROM RecruitmentAgentLinkedRelationships ralr
						INNER JOIN VacancyOwnerRelationship vor ON vor.VacancyOwnerRelationshipId = ralr.VacancyOwnerRelationshipID
					GROUP BY ProviderSiteRelationshipID
						
				) TempEmployer ON TempEmployer.ProviderSiteRelationshipID = ProviderSiteRelationship.ProviderSiteRelationshipID
				               
	WHERE     (ProviderSite.IsRecruitmentAgency = 1) 
		AND ProviderSite.TrainingProviderStatusTypeId = 1
	GROUP BY ProviderSite.FullName, ProviderSite.TradingName, ProviderSite.OwnerOrganisation, ProviderSite.AddressLine1, ProviderSite.AddressLine2, 
                      ProviderSite.AddressLine3, ProviderSite.AddressLine4, ProviderSite.AddressLine5, ProviderSite.Town, County.FullName, 
                      ProviderSite.PostCode, ProviderSite.ProviderSiteID,ProviderSite.EdsUrn, TempEmployer.NumLinkedEmployers

END