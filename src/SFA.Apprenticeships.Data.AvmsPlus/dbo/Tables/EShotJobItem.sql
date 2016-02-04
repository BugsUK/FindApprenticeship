CREATE TABLE [dbo].[EShotJobItem] (
    [EShotJobItemId]       INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [EShotJobId]           INT            NOT NULL,
    [EShotJobItemStatusId] INT            NOT NULL,
    [Email]                NVARCHAR (200) NOT NULL,
    [Error]                NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_EShotJobItem] PRIMARY KEY CLUSTERED ([EShotJobItemId] ASC),
    CONSTRAINT [FK_EShotJobItem_EShotJob] FOREIGN KEY ([EShotJobId]) REFERENCES [dbo].[EShotJob] ([EShotJobId]),
    CONSTRAINT [FK_EShotJobItem_EShotJobItemStatusType] FOREIGN KEY ([EShotJobItemStatusId]) REFERENCES [dbo].[EShotJobItemStatusType] ([EShotJobItemStatusTypeId])
);

