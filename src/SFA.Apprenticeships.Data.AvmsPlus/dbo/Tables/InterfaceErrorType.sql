CREATE TABLE [dbo].[InterfaceErrorType] (
    [InterfaceErrorTypeId]      INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ErrorCode]                 INT            NOT NULL,
    [ErrorDescription]          NVARCHAR (500) NULL,
    [InterfaceErrorGroupTypeId] INT            NULL,
    CONSTRAINT [PK_InterfaceErrorId_1] PRIMARY KEY CLUSTERED ([InterfaceErrorTypeId] ASC),
    CONSTRAINT [FK_InterfaceErrorType_InterfaceErrorTypeGroup_1] FOREIGN KEY ([InterfaceErrorGroupTypeId]) REFERENCES [dbo].[InterfaceErrorGroupType] ([InterfaceErrorGroupTypeId])
);

