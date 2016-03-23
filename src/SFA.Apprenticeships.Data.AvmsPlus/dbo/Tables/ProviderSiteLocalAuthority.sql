CREATE TABLE [dbo].[ProviderSiteLocalAuthority] (
    [ProviderSiteLocalAuthorityID] INT IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [ProviderSiteRelationshipID]   INT NOT NULL,
    [LocalAuthorityId]             INT NULL,
    CONSTRAINT [PK_TrainingProviderLocation] PRIMARY KEY CLUSTERED ([ProviderSiteLocalAuthorityID] ASC),
    CONSTRAINT [FK_ProviderSiteLocalAuthorities_LocalAuthority] FOREIGN KEY ([LocalAuthorityId]) REFERENCES [dbo].[LocalAuthority] ([LocalAuthorityId]),
    CONSTRAINT [FK_ProviderSiteLocalAuthorities_ProviderSiteRelationShip] FOREIGN KEY ([ProviderSiteRelationshipID]) REFERENCES [dbo].[ProviderSiteRelationship] ([ProviderSiteRelationshipID]),
    CONSTRAINT [uq_idx_trainingProviderLocation] UNIQUE NONCLUSTERED ([ProviderSiteRelationshipID] ASC, [LocalAuthorityId] ASC)
);

