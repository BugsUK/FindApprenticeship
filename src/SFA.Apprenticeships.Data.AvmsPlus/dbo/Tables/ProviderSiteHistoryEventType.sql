﻿CREATE TABLE [dbo].[ProviderSiteHistoryEventType] (
    [ProviderSiteHistoryEventTypeId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                       NVARCHAR (3)   NOT NULL,
    [ShortName]                      NVARCHAR (100) NOT NULL,
    [FullName]                       NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_TrainingProviderHistoryEventType] PRIMARY KEY CLUSTERED ([ProviderSiteHistoryEventTypeId] ASC),
    CONSTRAINT [uq_idx_TrainingProviderHistoryEventType] UNIQUE NONCLUSTERED ([FullName] ASC)
);

