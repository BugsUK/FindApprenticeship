CREATE TABLE [dbo].[VacancySearch] (
    [VacancySearchId]              INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VacancyId]                    INT              NOT NULL,
    [VacancyReferenceNumber]       INT              NOT NULL,
    [EmployerName]                 NVARCHAR (256)   NOT NULL,
    [VacancyOwnerName]             NVARCHAR (256)   NOT NULL,
    [DeliveryOrganisationName]     NVARCHAR (256)   NULL,
    [GeocodeEasting]               INT              NULL,
    [GeocodeNorthing]              INT              NULL,
    [Latitude]                     DECIMAL (13, 10) NOT NULL,
    [Longitude]                    DECIMAL (13, 10) NOT NULL,
    [LocalAuthorityID]             INT              NULL,
    [VacancyPostedDate]            DATETIME         NOT NULL,
    [Title]                        NVARCHAR (100)   NOT NULL,
    [ShortDescription]             NVARCHAR (256)   NOT NULL,
    [Description]                  NVARCHAR (MAX)   NULL,
    [Status]                       INT              NOT NULL,
    [ApplicationClosingDate]       DATETIME         NOT NULL,
    [ApprenticeshipFrameworkId]    INT              NOT NULL,
    [ApprenticeshipOccupationName] NVARCHAR (200)   NOT NULL,
    [ApprenticeshipFrameworkName]  NVARCHAR (200)   NOT NULL,
    [CountyId]                     INT              NOT NULL,
    [WeeklyWage]                   MONEY            NULL,
    [WageType]                     INT              DEFAULT ((1)) NOT NULL,
    [Town]                         NVARCHAR (40)    NOT NULL,
    [ApprenticeshipType]           INT              NOT NULL,
    [ApplicationClosingDateAsInt]  INT              NOT NULL,
    [EmployerSearch]               AS               (replace([EmployerName],'&','xxvvzz')) PERSISTED,
    [TrainingProviderSearch]       AS               (replace([VacancyOwnerName],'&','xxvvzz')) PERSISTED,
    [DeliveryOrganisationSearch]   AS               (replace([DeliveryOrganisationName],'&','xxvvzz')) PERSISTED,
    [RealityCheck]                 NVARCHAR (MAX)   NULL,
    [OtherImportantInformation]    NVARCHAR (MAX)   NULL,
    [NationalVacancy]              BIT              CONSTRAINT [DFT_VacancySearchNational] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_VacancySearch_1] PRIMARY KEY CLUSTERED ([VacancySearchId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [FK_VacancySearch_ApprenticeshipFramework] FOREIGN KEY ([ApprenticeshipFrameworkId]) REFERENCES [dbo].[ApprenticeshipFramework] ([ApprenticeshipFrameworkId]),
    CONSTRAINT [FK_VacancySearch_ApprenticeshipType] FOREIGN KEY ([ApprenticeshipType]) REFERENCES [dbo].[ApprenticeshipType] ([ApprenticeshipTypeId]),
    CONSTRAINT [FK_VacancySearch_County] FOREIGN KEY ([CountyId]) REFERENCES [dbo].[County] ([CountyId]),
    CONSTRAINT [FK_VacancySearch_Vacancy] FOREIGN KEY ([VacancyId]) REFERENCES [dbo].[Vacancy] ([VacancyId]),
    CONSTRAINT [FK_VacancySearch_VacancyStatusType] FOREIGN KEY ([Status]) REFERENCES [dbo].[VacancyStatusType] ([VacancyStatusTypeId]),
    CONSTRAINT [uq_idx_vacancySearch_referenceNumber] UNIQUE NONCLUSTERED ([VacancyReferenceNumber] ASC) WITH (FILLFACTOR = 90) ON [Index],
    CONSTRAINT [uq_idx_vacancySearch_vacancyId] UNIQUE NONCLUSTERED ([VacancyId] ASC) WITH (FILLFACTOR = 90) ON [Index]
) TEXTIMAGE_ON [PRIMARY];


GO
CREATE NONCLUSTERED INDEX [idx_VacancySearch_ApplicationClosingDateAsInt]
    ON [dbo].[VacancySearch]([ApplicationClosingDateAsInt] ASC)
    INCLUDE([VacancySearchId], [VacancyPostedDate], [WeeklyWage], [WageType], [ApprenticeshipType], [GeocodeEasting], [GeocodeNorthing], [CountyId])
    ON [Index];


GO
CREATE NONCLUSTERED INDEX [idx_VacancySearch_ApprenticeshipFrameworkId_CountyId_ApplicationClosingDateAsInt_VacancySearchId]
    ON [dbo].[VacancySearch]([ApprenticeshipFrameworkId] ASC, [CountyId] ASC, [ApplicationClosingDateAsInt] ASC, [VacancySearchId] ASC) WITH (FILLFACTOR = 90)
    ON [Index];


GO
CREATE NONCLUSTERED INDEX [idx_VacancySearch_EmployerName]
    ON [dbo].[VacancySearch]([EmployerName] ASC, [ApplicationClosingDateAsInt] ASC)
    INCLUDE([VacancySearchId], [VacancyOwnerName], [DeliveryOrganisationName])
    ON [Index];


GO
CREATE NONCLUSTERED INDEX [idx_VacancySearch_WeeklyWage]
    ON [dbo].[VacancySearch]([WeeklyWage] ASC)
    INCLUDE([VacancyReferenceNumber], [GeocodeEasting], [GeocodeNorthing], [VacancyPostedDate], [ApprenticeshipFrameworkId], [CountyId])
    ON [Index];

