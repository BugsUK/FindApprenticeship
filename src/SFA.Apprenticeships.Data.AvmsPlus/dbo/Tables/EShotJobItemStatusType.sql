CREATE TABLE [dbo].[EShotJobItemStatusType] (
    [EShotJobItemStatusTypeId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                 NVARCHAR (3)   NOT NULL,
    [FullName]                 NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_EShotJobItemStatusType] PRIMARY KEY CLUSTERED ([EShotJobItemStatusTypeId] ASC)
);

