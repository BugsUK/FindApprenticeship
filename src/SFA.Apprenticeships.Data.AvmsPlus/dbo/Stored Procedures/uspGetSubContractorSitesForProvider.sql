CREATE PROCEDURE [dbo].[uspGetSubContractorSitesForProvider]
	@ProviderId int, 
	@ContractorProviderId int = null
AS
BEGIN	
	-- SET NOCOUNT ON added to prevent extra result sets from              
	-- interfering with SELECT statements.              
	SET NOCOUNT ON;  

	SELECT  
		ps.ProviderSiteID,  
		isnull(ps.TradingName,'') AS 'TradingName',            
		isnull(AddressLine1,'') AS 'AddressLine1',              
		isnull(AddressLine2,'') AS 'AddressLine2',              
		isnull(AddressLine3,'') AS 'AddressLine3',             
		isnull(AddressLine4,'') AS 'AddressLine4',          
		isnull(Town,'') AS 'Town',
		c.FullName AS 'County',              
		isnull(Postcode,'') AS 'Postcode',
		ISNULL(COUNT(unionedvacanies.VacancyId), 0) as NumActiveVacancies,
		CASE
			WHEN psrSC.ProviderID IS NULL THEN 0
			ELSE 1
		END AS 'SubContracted',
		profilesExists.ProfileExists
     
	FROM [ProviderSite] ps
		LEFT JOIN County c ON c.CountyId = ps.CountyId
		JOIN ProviderSiteRelationship psr ON ps.ProviderSiteID = psr.ProviderSiteID AND psr.ProviderSiteRelationShipTypeID = 1
		LEFT JOIN ProviderSiteRelationship psrSC ON ps.ProviderSiteID = psrSC.ProviderSiteID AND psrSC.ProviderSiteRelationShipTypeID = 2 AND psrSC.ProviderID = @ContractorProviderId 
		LEFT JOIN 
		(
			SELECT  
				ps.ProviderSiteID,  
				CASE
					WHEN Count(frwk.ProviderSiteFrameworkID) > 0 THEN 1
					WHEN Count(loc.ProviderSiteLocalAuthorityID) > 0 THEN 1
					ELSE 0
				END AS 'ProfileExists'
     
				FROM [ProviderSite] ps
				LEFT JOIN ProviderSiteRelationship psrSC ON ps.ProviderSiteID = psrSC.ProviderSiteID AND psrSC.ProviderSiteRelationShipTypeID = 2 AND psrSC.ProviderID = @ContractorProviderId 
				LEFT JOIN ProviderSiteFramework frwk ON frwk.ProviderSiteRelationshipID = psrSC.ProviderSiteRelationshipID 
				LEFT JOIN ProviderSiteLocalAuthority loc ON loc.ProviderSiteRelationshipID = psrSC.ProviderSiteRelationshipID 
	
				GROUP BY ps.ProviderSiteID
		) profilesExists ON profilesExists.ProviderSiteID = ps.ProviderSiteID
		
		LEFT JOIN
			( 
				SELECT v.VacancyId, v.DeliveryOrganisationID as ProviderSiteID 
				FROM Vacancy v
					INNER JOIN VacancyStatusType vs ON vs.VacancyStatusTypeId = v.VacancyStatusId
				WHERE 
					vs.CodeName <> 'Cld' AND vs.CodeName <> 'Wdr' AND vs.CodeName <> 'Com' AND vs.CodeName <> 'Del' AND vs.CodeName <> 'Pie'  
					AND v.ContractOwnerID = @ContractorProviderId
				UNION
				SELECT v.VacancyId, vor.ProviderSiteID as ProviderSiteID 
				FROM Vacancy v
					INNER JOIN VacancyOwnerRelationship vor ON v.VacancyOwnerRelationshipId = vor.VacancyOwnerRelationshipId
					INNER JOIN VacancyStatusType vs ON vs.VacancyStatusTypeId = v.VacancyStatusId
				WHERE 
					vs.CodeName <> 'Cld' AND vs.CodeName <> 'Wdr' AND vs.CodeName <> 'Com' AND vs.CodeName <> 'Del' AND vs.CodeName <> 'Pie'  
					AND v.ContractOwnerID = @ContractorProviderId
			) unionedvacanies ON unionedvacanies.ProviderSiteID = ps.ProviderSiteID 
	
	WHERE psr.ProviderID = @ProviderId
	AND PS.TrainingProviderStatusTypeID != 2      -- GJG -- Provider Issue (Issue 83 and 84 from R5a post-live sheet)
	GROUP BY ps.ProviderSiteID,ps.TradingName,AddressLine1,AddressLine2, AddressLine3,AddressLine4,Town,c.FullName, Postcode,psrSC.ProviderID, profilesExists.ProfileExists


END