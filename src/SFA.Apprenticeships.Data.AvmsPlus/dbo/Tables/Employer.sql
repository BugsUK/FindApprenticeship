CREATE TABLE [dbo].[Employer] (
    [EmployerId]                INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [EdsUrn]                    INT              NOT NULL,
    [FullName]                  NVARCHAR (255)   COLLATE Latin1_General_CI_AS NOT NULL,
    [TradingName]               NVARCHAR (255)   COLLATE Latin1_General_CI_AS NOT NULL,
    [AddressLine1]              NVARCHAR (50)    COLLATE Latin1_General_CI_AS NOT NULL,
    [AddressLine2]              NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [AddressLine3]              NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [AddressLine4]              NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [AddressLine5]              NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [Town]                      NVARCHAR (50)    COLLATE Latin1_General_CI_AS NOT NULL,
    [CountyId]                  INT              NOT NULL,
    [PostCode]                  NVARCHAR (8)     COLLATE Latin1_General_CI_AS NOT NULL,
    [LocalAuthorityId]          INT              NULL,
    [Longitude]                 DECIMAL (13, 10) NULL,
    [Latitude]                  DECIMAL (13, 10) NULL,
    [GeocodeEasting]            INT              NULL,
    [GeocodeNorthing]           INT              NULL,
    [PrimaryContact]            INT              NOT NULL,
    [NumberofEmployeesAtSite]   INT              NULL,
    [NumberOfEmployeesInGroup]  INT              NULL,
    [OwnerOrgnistaion]          VARCHAR (255)    COLLATE Latin1_General_CI_AS NULL,
    [CompanyRegistrationNumber] VARCHAR (8)      COLLATE Latin1_General_CI_AS NULL,
    [TotalVacanciesPosted]      INT              NULL,
    [BeingSupportedBy]          NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [LockedForSupportUntil]     DATETIME         NULL,
    [EmployerStatusTypeId]      INT              NULL,
    [DisableAllowed]            BIT              CONSTRAINT [DFT_Employer_DisableAllowed] DEFAULT ((0)) NOT NULL,
    [TrackingAllowed]           BIT              CONSTRAINT [DFT_Employer_TrackingAllowed] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Employer] PRIMARY KEY CLUSTERED ([EmployerId] ASC),
    CONSTRAINT [FK_Employer_County] FOREIGN KEY ([CountyId]) REFERENCES [dbo].[County] ([CountyId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_Employer_EmployerContact] FOREIGN KEY ([PrimaryContact]) REFERENCES [dbo].[EmployerContact] ([EmployerContactId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_Employer_EmployerTrainingProviderStatus] FOREIGN KEY ([EmployerStatusTypeId]) REFERENCES [dbo].[EmployerTrainingProviderStatus] ([EmployerTrainingProviderStatusId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_Employer_LocalAuthority] FOREIGN KEY ([LocalAuthorityId]) REFERENCES [dbo].[LocalAuthority] ([LocalAuthorityId]) NOT FOR REPLICATION,
    CONSTRAINT [uq_idx_employer] UNIQUE NONCLUSTERED ([EdsUrn] ASC)
);




GO
CREATE NONCLUSTERED INDEX [idx_Employer_EmployerStatusTypeId]
    ON [dbo].[Employer]([EmployerStatusTypeId] ASC)
    INCLUDE([EmployerId], [LocalAuthorityId])
    ON [Index];

