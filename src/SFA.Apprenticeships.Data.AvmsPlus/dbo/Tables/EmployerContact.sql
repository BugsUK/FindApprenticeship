CREATE TABLE [dbo].[EmployerContact] (
    [EmployerContactId]       INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [PersonId]                INT           NOT NULL,
    [AddressLine1]            NVARCHAR (50) COLLATE Latin1_General_CI_AS NULL,
    [AddressLine2]            NVARCHAR (50) COLLATE Latin1_General_CI_AS NULL,
    [AddressLine3]            NVARCHAR (50) COLLATE Latin1_General_CI_AS NULL,
    [AddressLine4]            NVARCHAR (50) COLLATE Latin1_General_CI_AS NULL,
    [AddressLine5]            NVARCHAR (50) COLLATE Latin1_General_CI_AS NULL,
    [Town]                    NVARCHAR (50) COLLATE Latin1_General_CI_AS NULL,
    [CountyId]                INT           NULL,
    [PostCode]                NVARCHAR (8)  COLLATE Latin1_General_CI_AS NULL,
    [LocalAuthorityId]        INT           NULL,
    [JobTitle]                NVARCHAR (50) COLLATE Latin1_General_CI_AS NULL,
    [Department]              NVARCHAR (50) COLLATE Latin1_General_CI_AS NULL,
    [FaxNumber]               NVARCHAR (16) COLLATE Latin1_General_CI_AS NULL,
    [AlternatePhoneNumber]    NVARCHAR (16) COLLATE Latin1_General_CI_AS NULL,
    [ContactPreferenceTypeId] INT           NOT NULL,
    [Availability]            NVARCHAR (50) COLLATE Latin1_General_CI_AS NULL,
    CONSTRAINT [PK_EmployerContact] PRIMARY KEY CLUSTERED ([EmployerContactId] ASC),
    CONSTRAINT [FK_Employer_Contact_Person] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_EmployerContact_ContactPreferenceType] FOREIGN KEY ([ContactPreferenceTypeId]) REFERENCES [dbo].[ContactPreferenceType] ([ContactPreferenceTypeId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_EmployerContact_County] FOREIGN KEY ([CountyId]) REFERENCES [dbo].[County] ([CountyId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_EmployerContact_LocalAuthority] FOREIGN KEY ([LocalAuthorityId]) REFERENCES [dbo].[LocalAuthority] ([LocalAuthorityId]) NOT FOR REPLICATION,
    CONSTRAINT [uq_idx_employerContact] UNIQUE NONCLUSTERED ([PersonId] ASC)
);



