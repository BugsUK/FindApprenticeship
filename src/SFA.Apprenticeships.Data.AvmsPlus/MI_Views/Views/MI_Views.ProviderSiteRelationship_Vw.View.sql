CREATE VIEW [MI_Views].[ProviderSiteRelationship_Vw]
AS
     SELECT ProviderSiteRelationshipID,
            ProviderID,
            ProviderSiteID,
            ProviderSiteRelationShipTypeID
     FROM dbo.ProviderSiteRelationship;