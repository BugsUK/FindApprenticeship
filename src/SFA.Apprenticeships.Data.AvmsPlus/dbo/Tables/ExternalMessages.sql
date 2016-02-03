CREATE TABLE [dbo].[ExternalMessages] (
    [MessageId]       UNIQUEIDENTIFIER NOT NULL,
    [ReceivedDate]    DATETIME         NOT NULL,
    [ProcessedDate]   DATETIME         NULL,
    [Request]         XML              NOT NULL,
    [Response]        XML              NULL,
    [MessageStatusId] INT              NOT NULL,
    CONSTRAINT [PK_ExternalMessages] PRIMARY KEY CLUSTERED ([MessageId] ASC),
    CONSTRAINT [FK_ExternalMessages_MessageStatus] FOREIGN KEY ([MessageStatusId]) REFERENCES [dbo].[MessageStatus] ([MessageStatusId])
);

