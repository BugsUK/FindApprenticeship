CREATE TABLE [dbo].[ProviderSiteOffer] (
    [ProviderSiteOfferID]          INT IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ProviderSiteLocalAuthorityID] INT NOT NULL,
    [ProviderSiteFrameworkID]      INT NOT NULL,
    [Apprenticeship]               BIT NOT NULL,
    [AdvancedApprenticeship]       BIT NOT NULL,
    [HigherApprenticeship]         BIT CONSTRAINT [DFT_TrainingProviderOffer_HigherApprenticeShip] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TrainingProviderOffer] PRIMARY KEY CLUSTERED ([ProviderSiteOfferID] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [FK_TrainingProviderOffer_TrainingProviderFramework] FOREIGN KEY ([ProviderSiteFrameworkID]) REFERENCES [dbo].[ProviderSiteFramework] ([ProviderSiteFrameworkID]) ON DELETE CASCADE,
    CONSTRAINT [FK_TrainingProviderOffer_TrainingProviderLocation] FOREIGN KEY ([ProviderSiteLocalAuthorityID]) REFERENCES [dbo].[ProviderSiteLocalAuthority] ([ProviderSiteLocalAuthorityID]) ON DELETE CASCADE,
    CONSTRAINT [uq_idx_trainingProviderOffer] UNIQUE NONCLUSTERED ([ProviderSiteLocalAuthorityID] ASC, [ProviderSiteFrameworkID] ASC) WITH (FILLFACTOR = 90) ON [Index]
);

