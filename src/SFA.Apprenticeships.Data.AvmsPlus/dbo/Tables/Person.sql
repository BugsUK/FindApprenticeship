CREATE TABLE [dbo].[Person] (
    [PersonId]       INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [Title]          INT            CONSTRAINT [DF_Person_Title] DEFAULT ((0)) NOT NULL,
    [OtherTitle]     NVARCHAR (10)  NULL,
    [FirstName]      NVARCHAR (35)  NULL,
    [MiddleNames]    NVARCHAR (35)  NULL,
    [Surname]        NVARCHAR (35)  NOT NULL,
    [LandlineNumber] NVARCHAR (16)  NULL,
    [MobileNumber]   NVARCHAR (16)  NULL,
    [Email]          NVARCHAR (100) NULL,
    [PersonTypeId]   INT            NOT NULL,
    CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([PersonId] ASC),
    CONSTRAINT [FK_Person_PersonTitleType] FOREIGN KEY ([Title]) REFERENCES [dbo].[PersonTitleType] ([PersonTitleTypeId]),
    CONSTRAINT [FK_Person_PersonType] FOREIGN KEY ([PersonTypeId]) REFERENCES [dbo].[PersonType] ([PersonTypeId])
);

GO
CREATE NONCLUSTERED INDEX [nci_wi_Person_D958DE5AC11A2A5D45EC53330A964D4D] 
ON [dbo].[Person]([FirstName] ASC)
INCLUDE ([MiddleNames], [PersonId], [Surname])

GO
CREATE NONCLUSTERED INDEX [nci_wi_Person_C290A71EFC9AFDB4FF06BD8554719222] 
ON [dbo].[Person]([Surname] ASC)
INCLUDE ([FirstName],[MiddleNames]) 

GO
