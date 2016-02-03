CREATE TABLE [dbo].[ProviderSiteRelationshipType] (
    [ProviderSiteRelationshipTypeID]   INT            NOT NULL,
    [ProviderSiteRelationshipTypeName] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ProviderSiteRelationshipType] PRIMARY KEY CLUSTERED ([ProviderSiteRelationshipTypeID] ASC)
);

