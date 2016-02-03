CREATE TABLE [dbo].[ProviderSiteRelationship] (
    [ProviderSiteRelationshipID]     INT IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ProviderID]                     INT NOT NULL,
    [ProviderSiteID]                 INT NOT NULL,
    [ProviderSiteRelationShipTypeID] INT NOT NULL,
    CONSTRAINT [PK_ProviderSiteRelationship] PRIMARY KEY CLUSTERED ([ProviderSiteRelationshipID] ASC),
    CONSTRAINT [FK_ProviderSiteRelationship_Provider] FOREIGN KEY ([ProviderID]) REFERENCES [dbo].[Provider] ([ProviderID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_ProviderSiteRelationship_ProviderSite] FOREIGN KEY ([ProviderSiteID]) REFERENCES [dbo].[ProviderSite] ([ProviderSiteID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_ProviderSiteRelationship_ProviderSiteRelationshipType] FOREIGN KEY ([ProviderSiteRelationShipTypeID]) REFERENCES [dbo].[ProviderSiteRelationshipType] ([ProviderSiteRelationshipTypeID]) NOT FOR REPLICATION,
    CONSTRAINT [UC_PSR] UNIQUE NONCLUSTERED ([ProviderID] ASC, [ProviderSiteID] ASC, [ProviderSiteRelationShipTypeID] ASC)
);



