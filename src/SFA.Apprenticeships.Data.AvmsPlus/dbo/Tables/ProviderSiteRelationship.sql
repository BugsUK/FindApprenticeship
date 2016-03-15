CREATE TABLE [dbo].[ProviderSiteRelationship] (
    [ProviderSiteRelationshipID]     INT IDENTITY (-1, -1) NOT NULL,
    [ProviderID]                     INT NOT NULL,
    [ProviderSiteID]                 INT NOT NULL,
    [ProviderSiteRelationShipTypeID] INT NOT NULL,
    CONSTRAINT [PK_ProviderSiteRelationship] PRIMARY KEY CLUSTERED ([ProviderSiteRelationshipID] ASC),
    CONSTRAINT [FK_ProviderSiteRelationship_Provider] FOREIGN KEY ([ProviderID]) REFERENCES [dbo].[Provider] ([ProviderID]),
    CONSTRAINT [FK_ProviderSiteRelationship_ProviderSite] FOREIGN KEY ([ProviderSiteID]) REFERENCES [dbo].[ProviderSite] ([ProviderSiteID]),
    CONSTRAINT [FK_ProviderSiteRelationship_ProviderSiteRelationshipType] FOREIGN KEY ([ProviderSiteRelationShipTypeID]) REFERENCES [dbo].[ProviderSiteRelationshipType] ([ProviderSiteRelationshipTypeID]),
    CONSTRAINT [UC_PSR] UNIQUE NONCLUSTERED ([ProviderID] ASC, [ProviderSiteID] ASC, [ProviderSiteRelationShipTypeID] ASC)
);

