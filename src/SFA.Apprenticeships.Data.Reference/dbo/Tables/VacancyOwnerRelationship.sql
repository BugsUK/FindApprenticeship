CREATE TABLE [dbo].[VacancyOwnerRelationship] (
    [VacancyOwnerRelationshipId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [EmployerId]                 INT            NOT NULL,
    [ProviderSiteID]             INT            NOT NULL,
    [ContractHolderIsEmployer]   BIT            NOT NULL,
    [ManagerIsEmployer]          BIT            NOT NULL,
    [StatusTypeId]               INT            NOT NULL,
    [Notes]                      VARCHAR (4000) NULL,
    [EmployerDescription]        NVARCHAR (MAX) NULL,
    [EmployerWebsite]            NVARCHAR (256) NULL,
    [EmployerLogoAttachmentId]   INT            NULL,
    [NationWideAllowed]          BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Contract] PRIMARY KEY CLUSTERED ([VacancyOwnerRelationshipId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [FK_Contract_Employer] FOREIGN KEY ([EmployerId]) REFERENCES [dbo].[Employer] ([EmployerId]),
    CONSTRAINT [FK_VacancyOwnerRelationship_ProviderSite] FOREIGN KEY ([ProviderSiteID]) REFERENCES [dbo].[ProviderSite] ([ProviderSiteID]),
    CONSTRAINT [FK_VacancyProvisionRelationship_AttachedDocument] FOREIGN KEY ([EmployerLogoAttachmentId]) REFERENCES [dbo].[AttachedDocument] ([AttachedDocumentId]),
    CONSTRAINT [FK_VacancyProvisionRelationship_VacancyProvisionRelationshipStatusType] FOREIGN KEY ([StatusTypeId]) REFERENCES [dbo].[VacancyProvisionRelationshipStatusType] ([VacancyProvisionRelationshipStatusTypeId]),
) TEXTIMAGE_ON [PRIMARY];

