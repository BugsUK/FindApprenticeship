CREATE VIEW [dbo].[vwSubContractors]
	AS SELECT p.ProviderID, p.UPIN, p.TradingName, pOrg.ProviderID as SC_ProviderID, pOrg.UPIN as SC_UPIN, psSC.ProviderSiteID as SC_ProviderSiteID, psSC.TrainingProviderStatusTypeId as SC_ProviderSiteStatusTypeId
	FROM Provider p
		INNER JOIN ProviderSiteRelationship psrSC ON psrSC.ProviderID = p.ProviderID AND psrSC.ProviderSiteRelationShipTypeID = 2
		INNER JOIN ProviderSite psSC ON psSC.ProviderSiteID = psrSc.ProviderSiteID
		INNER JOIN ProviderSiteRelationship psrOrg ON psrSC.ProviderSiteID = psrOrg.ProviderSiteID AND psrOrg.ProviderSiteRelationShipTypeID = 1
		INNER JOIN Provider pOrg ON pOrg.ProviderID = psrOrg.ProviderID 
	WHERE psSC.TrainingProviderStatusTypeId <> 3