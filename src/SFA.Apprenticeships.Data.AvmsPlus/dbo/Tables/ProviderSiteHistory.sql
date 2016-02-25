CREATE TABLE [dbo].[ProviderSiteHistory] (
    [TrainingProviderHistoryId] INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [TrainingProviderId]        INT             NOT NULL,
    [UserName]                  NVARCHAR (50)   NOT NULL,
    [HistoryDate]               DATETIME        NOT NULL,
    [EventTypeId]               INT             NOT NULL,
    [Comment]                   NVARCHAR (4000) NULL,
    CONSTRAINT [PK_Training_Provider_History] PRIMARY KEY CLUSTERED ([TrainingProviderHistoryId] ASC),
    CONSTRAINT [FK_Training_Provider_History_Training_Provider] FOREIGN KEY ([TrainingProviderId]) REFERENCES [dbo].[ProviderSite] ([ProviderSiteID]),
    CONSTRAINT [FK_TrainingProviderHistory_TrainingProviderHistoryEventType] FOREIGN KEY ([EventTypeId]) REFERENCES [dbo].[ProviderSiteHistoryEventType] ([ProviderSiteHistoryEventTypeId])
);

