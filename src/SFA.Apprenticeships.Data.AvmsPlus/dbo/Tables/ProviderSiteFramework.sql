CREATE TABLE [dbo].[ProviderSiteFramework] (
    [ProviderSiteFrameworkID]    INT IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ProviderSiteRelationshipID] INT NOT NULL,
    [FrameworkId]                INT NOT NULL,
    CONSTRAINT [PK_ProviderSiteFramework] PRIMARY KEY CLUSTERED ([ProviderSiteFrameworkID] ASC),
    CONSTRAINT [FK_ProviderSiteFramework_ProviderSiteRelationship] FOREIGN KEY ([ProviderSiteRelationshipID]) REFERENCES [dbo].[ProviderSiteRelationship] ([ProviderSiteRelationshipID]),
    CONSTRAINT [FK_TrainingProviderFramework_ApprenticeshipFramework] FOREIGN KEY ([FrameworkId]) REFERENCES [dbo].[ApprenticeshipFramework] ([ApprenticeshipFrameworkId]),
    CONSTRAINT [uq_idx_trainingProviderFramework] UNIQUE NONCLUSTERED ([ProviderSiteRelationshipID] ASC, [FrameworkId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idx_ProviderSiteFramework_ProviderSiteFrameworkID]
    ON [dbo].[ProviderSiteFramework]([ProviderSiteFrameworkID] ASC);

