﻿CREATE TABLE [dbo].[StakeHolder] (
    [StakeHolderID]              INT              IDENTITY (60000000, 1) NOT FOR REPLICATION NOT NULL,
    [PersonId]                   INT              NOT NULL,
    [StakeHolderStatusId]        INT              NOT NULL,
    [AddressLine1]               NVARCHAR (50)    NOT NULL,
    [AddressLine2]               NVARCHAR (50)    NULL,
    [AddressLine3]               NVARCHAR (50)    NULL,
    [AddressLine4]               NVARCHAR (50)    NULL,
    [AddressLine5]               NVARCHAR (50)    NULL,
    [Town]                       NVARCHAR (50)    NOT NULL,
    [CountyId]                   INT              NOT NULL,
    [UnconfirmedEmailAddress]    NVARCHAR (100)   NULL,
    [Postcode]                   NVARCHAR (8)     NOT NULL,
    [OrganisationId]             INT              NOT NULL,
    [OrganisationOther]          NVARCHAR (50)    NULL,
    [Longitude]                  DECIMAL (13, 10) NULL,
    [Latitude]                   DECIMAL (13, 10) NULL,
    [GeocodeEasting]             INT              NULL,
    [GeocodeNorthing]            INT              NULL,
    [LastAccessedDate]           DATETIME         NULL,
    [ForgottenUsernameRequested] BIT              CONSTRAINT [DF_StakeHolder_ForgottenUsernameRequested] DEFAULT ((0)) NOT NULL,
    [ForgottenPasswordRequested] BIT              CONSTRAINT [DF_StakeHolder_ForgottenPasswordRequested] DEFAULT ((0)) NOT NULL,
    [EmailAlertSent]             BIT              CONSTRAINT [DF_StakeHolder_EmailAlertSent] DEFAULT ((0)) NOT NULL,
    [BeingSupportedBy]           NVARCHAR (50)    NULL,
    [LockedForSupportUntil]      DATETIME         NULL,
    [LocalAuthorityId]           INT              NULL,
    PRIMARY KEY CLUSTERED ([StakeHolderID] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [FK_StakeHolder_County] FOREIGN KEY ([CountyId]) REFERENCES [dbo].[County] ([CountyId]),
    CONSTRAINT [FK_StakeHolder_LocalAuthority] FOREIGN KEY ([LocalAuthorityId]) REFERENCES [dbo].[LocalAuthority] ([LocalAuthorityId]),
    CONSTRAINT [FK_StakeHolder_OrganisationId] FOREIGN KEY ([OrganisationId]) REFERENCES [dbo].[Organisation] ([OrganisationId]),
    CONSTRAINT [FK_StakeHolder_Person] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]),
    CONSTRAINT [FK_StakeHolder_StakeHolderStatus] FOREIGN KEY ([StakeHolderStatusId]) REFERENCES [dbo].[StakeHolderStatus] ([StakeHolderStatusId])
);

