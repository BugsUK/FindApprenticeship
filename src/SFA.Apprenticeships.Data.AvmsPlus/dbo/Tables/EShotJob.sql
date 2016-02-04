CREATE TABLE [dbo].[EShotJob] (
    [EShotJobId]       INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [EShotJobStatusId] INT            NOT NULL,
    [Subject]          NVARCHAR (400) NOT NULL,
    [Message]          NVARCHAR (MAX) NOT NULL,
    [DateCreated]      DATETIME       NOT NULL,
    [DateUpdated]      DATETIME       NOT NULL,
    [User]             NVARCHAR (400) NOT NULL,
    CONSTRAINT [PK_EShotJob] PRIMARY KEY CLUSTERED ([EShotJobId] ASC),
    CONSTRAINT [FK_EShotJob_EShotJobStatusType] FOREIGN KEY ([EShotJobStatusId]) REFERENCES [dbo].[EShotJobStatusType] ([EShotJobStatusTypeId])
);

