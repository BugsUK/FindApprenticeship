CREATE TABLE [dbo].[InternalMessages] (
    [MessageId]       UNIQUEIDENTIFIER NOT NULL,
    [ReceivedDate]    DATETIME         NOT NULL,
    [ProcessedDate]   DATETIME         NULL,
    [Request]         XML              NOT NULL,
    [Response]        XML              NULL,
    [MessageStatusId] INT              NOT NULL,
    CONSTRAINT [PK_InternalMessages] PRIMARY KEY CLUSTERED ([MessageId] ASC),
    CONSTRAINT [FK_InternalMessages_MessageStatus] FOREIGN KEY ([MessageStatusId]) REFERENCES [dbo].[MessageStatus] ([MessageStatusId])
);

