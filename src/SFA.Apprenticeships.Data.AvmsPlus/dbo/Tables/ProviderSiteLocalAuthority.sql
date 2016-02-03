CREATE TABLE [dbo].[ProviderSiteLocalAuthority] (
    [ProviderSiteLocalAuthorityID] INT IDENTITY (0, 1) NOT FOR REPLICATION NOT NULL,
    [ProviderSiteRelationshipID]   INT NOT NULL,
    [LocalAuthorityId]             INT NULL,
    CONSTRAINT [PK_TrainingProviderLocation] PRIMARY KEY CLUSTERED ([ProviderSiteLocalAuthorityID] ASC),
    CONSTRAINT [FK_ProviderSiteLocalAuthorities_LocalAuthority] FOREIGN KEY ([LocalAuthorityId]) REFERENCES [dbo].[LocalAuthority] ([LocalAuthorityId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_ProviderSiteLocalAuthorities_ProviderSiteRelationShip] FOREIGN KEY ([ProviderSiteRelationshipID]) REFERENCES [dbo].[ProviderSiteRelationship] ([ProviderSiteRelationshipID]) NOT FOR REPLICATION,
    CONSTRAINT [uq_idx_trainingProviderLocation] UNIQUE NONCLUSTERED ([ProviderSiteRelationshipID] ASC, [LocalAuthorityId] ASC)
);



