CREATE TABLE [dbo].[VacancyOwnerRelationship] (
    [VacancyOwnerRelationshipId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [EmployerId]                 INT            NOT NULL,
    [ProviderSiteID]             INT            NOT NULL,
    [ContractHolderIsEmployer]   BIT            NOT NULL,
    [ManagerIsEmployer]          BIT            NOT NULL,
    [StatusTypeId]               INT            NOT NULL,
    [Notes]                      VARCHAR (4000) COLLATE Latin1_General_CI_AS NULL,
    [EmployerDescription]        NVARCHAR (MAX) COLLATE Latin1_General_CI_AS NULL,
    [EmployerWebsite]            NVARCHAR (256) COLLATE Latin1_General_CI_AS NULL,
    [EmployerLogoAttachmentId]   INT            NULL,
    [NationWideAllowed]          BIT            CONSTRAINT [DF__VacancyPr__Natio__037C6257] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Contract] PRIMARY KEY CLUSTERED ([VacancyOwnerRelationshipId] ASC),
    CONSTRAINT [FK_Contract_Employer] FOREIGN KEY ([EmployerId]) REFERENCES [dbo].[Employer] ([EmployerId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_VacancyOwnerRelationship_ProviderSite] FOREIGN KEY ([ProviderSiteID]) REFERENCES [dbo].[ProviderSite] ([ProviderSiteID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_VacancyProvisionRelationship_AttachedDocument] FOREIGN KEY ([EmployerLogoAttachmentId]) REFERENCES [dbo].[AttachedDocument] ([AttachedDocumentId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_VacancyProvisionRelationship_VacancyProvisionRelationshipStatusType] FOREIGN KEY ([StatusTypeId]) REFERENCES [dbo].[VacancyProvisionRelationshipStatusType] ([VacancyProvisionRelationshipStatusTypeId]) NOT FOR REPLICATION,
    CONSTRAINT [uq_idx_vacancyProvisionRelationship] UNIQUE NONCLUSTERED ([EmployerId] ASC, [ProviderSiteID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [idx_VacancyOwnerRelationship_ProviderSiteID]
    ON [dbo].[VacancyOwnerRelationship]([ProviderSiteID] ASC, [ManagerIsEmployer] ASC)
    INCLUDE([VacancyOwnerRelationshipId], [EmployerId])
    ON [Index];


GO
CREATE NONCLUSTERED INDEX [idx_VacancyOwnerRelationship_StatusTypeId]
    ON [dbo].[VacancyOwnerRelationship]([StatusTypeId] ASC)
    INCLUDE([EmployerId])
    ON [Index];

