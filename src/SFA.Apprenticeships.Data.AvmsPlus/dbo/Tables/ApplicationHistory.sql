CREATE TABLE [dbo].[ApplicationHistory] (
    [ApplicationHistoryId]             INT             IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [ApplicationId]                    INT             NOT NULL,
    [UserName]                         NVARCHAR (50)   NULL,
    [ApplicationHistoryEventDate]      DATETIME        NOT NULL,
    [ApplicationHistoryEventTypeId]    INT             NOT NULL,
    [ApplicationHistoryEventSubTypeId] INT             NULL,
    [Comment]                          NVARCHAR (4000) NULL,
    CONSTRAINT [PK_ApplicationHistory] PRIMARY KEY CLUSTERED ([ApplicationHistoryId] ASC),
    CONSTRAINT [FK_ApplicationHistory_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Application] ([ApplicationId]),
    CONSTRAINT [FK_ApplicationHistory_ApplicationHistoryEvent] FOREIGN KEY ([ApplicationHistoryEventTypeId]) REFERENCES [dbo].[ApplicationHistoryEvent] ([ApplicationHistoryEventId])
);


GO
CREATE NONCLUSTERED INDEX [idx_ApplicationHistory_ApplicationHistoryEventDate]
    ON [dbo].[ApplicationHistory]([ApplicationHistoryEventDate] ASC)
    INCLUDE([ApplicationId], [ApplicationHistoryEventSubTypeId]);


GO
CREATE NONCLUSTERED INDEX [idx_ApplicationHistory_ApplicationId_ApplicationHistoryEventSubTypeId]
    ON [dbo].[ApplicationHistory]([ApplicationId] ASC, [ApplicationHistoryEventSubTypeId] ASC)
    INCLUDE([ApplicationHistoryEventTypeId], [ApplicationHistoryId], [ApplicationHistoryEventDate]);

