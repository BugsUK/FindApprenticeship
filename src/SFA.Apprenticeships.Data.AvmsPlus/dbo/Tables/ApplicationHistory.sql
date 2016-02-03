CREATE TABLE [dbo].[ApplicationHistory] (
    [ApplicationHistoryId]             INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ApplicationId]                    INT             NOT NULL,
    [UserName]                         NVARCHAR (50)   COLLATE Latin1_General_CI_AS NULL,
    [ApplicationHistoryEventDate]      DATETIME        NOT NULL,
    [ApplicationHistoryEventTypeId]    INT             NOT NULL,
    [ApplicationHistoryEventSubTypeId] INT             NULL,
    [Comment]                          NVARCHAR (4000) COLLATE Latin1_General_CI_AS NULL,
    CONSTRAINT [PK_ApplicationHistory] PRIMARY KEY CLUSTERED ([ApplicationHistoryId] ASC),
    CONSTRAINT [FK_ApplicationHistory_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Application] ([ApplicationId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_ApplicationHistory_ApplicationHistoryEvent] FOREIGN KEY ([ApplicationHistoryEventTypeId]) REFERENCES [dbo].[ApplicationHistoryEvent] ([ApplicationHistoryEventId]) NOT FOR REPLICATION
);




GO
CREATE NONCLUSTERED INDEX [idx_ApplicationHistory_ApplicationHistoryEventDate]
    ON [dbo].[ApplicationHistory]([ApplicationHistoryEventDate] ASC)
    INCLUDE([ApplicationId], [ApplicationHistoryEventSubTypeId])
    ON [Index];


GO
CREATE NONCLUSTERED INDEX [idx_ApplicationHistory_ApplicationId_ApplicationHistoryEventSubTypeId]
    ON [dbo].[ApplicationHistory]([ApplicationId] ASC, [ApplicationHistoryEventSubTypeId] ASC)
    INCLUDE([ApplicationHistoryEventTypeId], [ApplicationHistoryId], [ApplicationHistoryEventDate]) WITH (FILLFACTOR = 90)
    ON [Index];

