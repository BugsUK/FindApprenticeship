CREATE TABLE [dbo].[InterfaceErrorGroupType] (
    [InterfaceErrorGroupTypeId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [GroupDescription]          NVARCHAR (200) NULL,
    CONSTRAINT [PK_InterfaceErrorGroupType_1] PRIMARY KEY CLUSTERED ([InterfaceErrorGroupTypeId] ASC)
);

