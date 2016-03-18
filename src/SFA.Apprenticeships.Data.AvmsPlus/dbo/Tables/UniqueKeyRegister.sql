CREATE TABLE [dbo].[UniqueKeyRegister] (
    [UniqueKeyRegisterId] INT           IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [KeyType]             NCHAR (2)     NOT NULL,
    [KeyValue]            NVARCHAR (30) NOT NULL,
    [DateTimeStamp]       DATETIME      NULL,
    CONSTRAINT [PK_UniqueKeyRegister] PRIMARY KEY CLUSTERED ([UniqueKeyRegisterId] ASC)
);

