CREATE TABLE [dbo].[Person] (
    [PersonId]       INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Title]          INT            CONSTRAINT [DF_Person_Title] DEFAULT ((0)) NOT NULL,
    [OtherTitle]     NVARCHAR (10)  COLLATE Latin1_General_CI_AS NULL,
    [FirstName]      NVARCHAR (35)  COLLATE Latin1_General_CI_AS NULL,
    [MiddleNames]    NVARCHAR (35)  COLLATE Latin1_General_CI_AS NULL,
    [Surname]        NVARCHAR (35)  COLLATE Latin1_General_CI_AS NOT NULL,
    [LandlineNumber] NVARCHAR (16)  COLLATE Latin1_General_CI_AS NULL,
    [MobileNumber]   NVARCHAR (16)  COLLATE Latin1_General_CI_AS NULL,
    [Email]          NVARCHAR (100) COLLATE Latin1_General_CI_AS NULL,
    [PersonTypeId]   INT            NOT NULL,
    CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([PersonId] ASC),
    CONSTRAINT [FK_Person_PersonTitleType] FOREIGN KEY ([Title]) REFERENCES [dbo].[PersonTitleType] ([PersonTitleTypeId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_Person_PersonType] FOREIGN KEY ([PersonTypeId]) REFERENCES [dbo].[PersonType] ([PersonTypeId]) NOT FOR REPLICATION
);



