CREATE VIEW [MI_Views].[ProviderSiteLocalAuthority_Vw]
AS
     SELECT ProviderSiteLocalAuthorityID,
            ProviderSiteRelationshipID,
            LocalAuthorityId
     FROM dbo.ProviderSiteLocalAuthority;