CREATE TABLE [dbo].[Vacancy] (
    [VacancyId]                        INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VacancyOwnerRelationshipId]       INT              NOT NULL,
    [VacancyReferenceNumber]           INT              NULL,
    [ContactName]                      NVARCHAR (100)   COLLATE Latin1_General_CI_AS NULL,
    [VacancyStatusId]                  INT              NOT NULL,
    [AddressLine1]                     NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [AddressLine2]                     NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [AddressLine3]                     NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [AddressLine4]                     NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [AddressLine5]                     NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [Town]                             NVARCHAR (40)    COLLATE Latin1_General_CI_AS NULL,
    [CountyId]                         INT              NULL,
    [PostCode]                         NVARCHAR (8)     COLLATE Latin1_General_CI_AS NULL,
    [LocalAuthorityId]                 INT              NULL,
    [GeocodeEasting]                   INT              NULL,
    [GeocodeNorthing]                  INT              NULL,
    [Longitude]                        DECIMAL (13, 10) NULL,
    [Latitude]                         DECIMAL (13, 10) NULL,
    [ApprenticeshipFrameworkId]        INT              CONSTRAINT [DF_Vacancy_ApprenticeshipFrameworkId] DEFAULT ((0)) NULL,
    [Title]                            NVARCHAR (100)   COLLATE Latin1_General_CI_AS NULL,
    [ApprenticeshipType]               INT              NULL,
    [ShortDescription]                 NVARCHAR (256)   COLLATE Latin1_General_CI_AS NULL,
    [Description]                      NVARCHAR (MAX)   COLLATE Latin1_General_CI_AS NULL,
    [WeeklyWage]                       MONEY            NULL,
    [WageType]                         INT              CONSTRAINT [DFT_WageType] DEFAULT ((1)) NOT NULL,
    [WageText]                         NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [NumberofPositions]                SMALLINT         NULL,
    [ApplicationClosingDate]           DATETIME         NULL,
    [InterviewsFromDate]               DATETIME         NULL,
    [ExpectedStartDate]                DATETIME         NULL,
    [ExpectedDuration]                 NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [WorkingWeek]                      NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [NumberOfViews]                    INT              NULL,
    [EmployerAnonymousName]            NVARCHAR (255)   COLLATE Latin1_General_CI_AS NULL,
    [EmployerDescription]              NVARCHAR (MAX)   COLLATE Latin1_General_CI_AS NULL,
    [EmployersWebsite]                 NVARCHAR (256)   COLLATE Latin1_General_CI_AS NULL,
    [MaxNumberofApplications]          INT              NULL,
    [ApplyOutsideNAVMS]                BIT              NULL,
    [EmployersApplicationInstructions] NVARCHAR (MAX)   COLLATE Latin1_General_CI_AS NULL,
    [EmployersRecruitmentWebsite]      NVARCHAR (256)   COLLATE Latin1_General_CI_AS NULL,
    [BeingSupportedBy]                 NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [LockedForSupportUntil]            DATETIME         NULL,
    [NoOfOfflineApplicants]            INT              NULL,
    [MasterVacancyId]                  INT              NULL,
    [VacancyLocationTypeId]            INT              NULL,
    [NoOfOfflineSystemApplicants]      INT              NULL,
    [VacancyManagerID]                 INT              NULL,
    [DeliveryOrganisationID]           INT              NULL,
    [ContractOwnerID]                  INT              NULL,
    [SmallEmployerWageIncentive]       BIT              CONSTRAINT [DF_Vacancy_SmallEmployerWageIncentive] DEFAULT ((0)) NOT NULL,
    [OriginalContractOwnerId]          INT              NULL,
    [VacancyManagerAnonymous]          BIT              CONSTRAINT [DFT_VacancyManagerAnonymous] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Vacancy_1] PRIMARY KEY CLUSTERED ([VacancyId] ASC),
    CONSTRAINT [FK_Vacancy_ApprenticeshipFramework] FOREIGN KEY ([ApprenticeshipFrameworkId]) REFERENCES [dbo].[ApprenticeshipFramework] ([ApprenticeshipFrameworkId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_Vacancy_ApprenticeshipType] FOREIGN KEY ([ApprenticeshipType]) REFERENCES [dbo].[ApprenticeshipType] ([ApprenticeshipTypeId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_Vacancy_County] FOREIGN KEY ([CountyId]) REFERENCES [dbo].[County] ([CountyId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_Vacancy_LocalAuthority] FOREIGN KEY ([LocalAuthorityId]) REFERENCES [dbo].[LocalAuthority] ([LocalAuthorityId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_Vacancy_MasterVacancyId] FOREIGN KEY ([MasterVacancyId]) REFERENCES [dbo].[Vacancy] ([VacancyId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_Vacancy_Provider_As_ContractOwner] FOREIGN KEY ([ContractOwnerID]) REFERENCES [dbo].[Provider] ([ProviderID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_Vacancy_ProviderSite_As_DeliveryOrg] FOREIGN KEY ([DeliveryOrganisationID]) REFERENCES [dbo].[ProviderSite] ([ProviderSiteID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_Vacancy_ProviderSite_As_VacancyManager] FOREIGN KEY ([VacancyManagerID]) REFERENCES [dbo].[ProviderSite] ([ProviderSiteID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_Vacancy_VacancyOwnerRelationship] FOREIGN KEY ([VacancyOwnerRelationshipId]) REFERENCES [dbo].[VacancyOwnerRelationship] ([VacancyOwnerRelationshipId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_Vacancy_VacancyStatusType] FOREIGN KEY ([VacancyStatusId]) REFERENCES [dbo].[VacancyStatusType] ([VacancyStatusTypeId]) NOT FOR REPLICATION,
    CONSTRAINT [uq_idx_vacancy] UNIQUE NONCLUSTERED ([VacancyReferenceNumber] ASC)
);




GO
CREATE NONCLUSTERED INDEX [idx_Vacancy_ApprenticeshipFrameworkId]
    ON [dbo].[Vacancy]([ApprenticeshipFrameworkId] ASC)
    INCLUDE([VacancyId], [VacancyStatusId])
    ON [Index];


GO
CREATE NONCLUSTERED INDEX [idx_Vacancy_ContractOwnerID]
    ON [dbo].[Vacancy]([ContractOwnerID] ASC)
    ON [Index];


GO
CREATE NONCLUSTERED INDEX [idx_Vacancy_DeliveryOrganisationID]
    ON [dbo].[Vacancy]([DeliveryOrganisationID] ASC)
    ON [Index];


GO
CREATE NONCLUSTERED INDEX [idx_Vacancy_VacancyManagerID]
    ON [dbo].[Vacancy]([VacancyManagerID] ASC)
    ON [Index];


GO
CREATE NONCLUSTERED INDEX [idx_Vacancy_VacancyOwnerRelationshipId]
    ON [dbo].[Vacancy]([VacancyOwnerRelationshipId] ASC)
    INCLUDE([ApprenticeshipFrameworkId], [Title], [VacancyManagerID], [ApplicationClosingDate], [VacancyStatusId], [NumberofPositions], [ApplyOutsideNAVMS]) WITH (FILLFACTOR = 90)
    ON [Index];


GO

GO

GO
