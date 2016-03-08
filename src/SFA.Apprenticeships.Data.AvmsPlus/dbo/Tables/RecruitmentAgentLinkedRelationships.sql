CREATE TABLE [dbo].[RecruitmentAgentLinkedRelationships] (
    [ProviderSiteRelationshipID] INT NOT NULL,
    [VacancyOwnerRelationshipID] INT NOT NULL,
    CONSTRAINT [PK_RecruitmentAgentLinkedRelationships] PRIMARY KEY CLUSTERED ([ProviderSiteRelationshipID] ASC, [VacancyOwnerRelationshipID] ASC),
    CONSTRAINT [FK_RALR_PSR] FOREIGN KEY ([ProviderSiteRelationshipID]) REFERENCES [dbo].[ProviderSiteRelationship] ([ProviderSiteRelationshipID]),
    CONSTRAINT [FK_RALR_VOR] FOREIGN KEY ([VacancyOwnerRelationshipID]) REFERENCES [dbo].[VacancyOwnerRelationship] ([VacancyOwnerRelationshipId])
);

