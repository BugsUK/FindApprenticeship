CREATE TABLE [dbo].[ApplicationHistoryEvent] (
    [ApplicationHistoryEventId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                  NVARCHAR (3)   NOT NULL,
    [ShortName]                 NVARCHAR (100) NOT NULL,
    [FullName]                  NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_ApplicationHistoryEvent] PRIMARY KEY CLUSTERED ([ApplicationHistoryEventId] ASC),
    CONSTRAINT [uq_idx_ApplicationHistoryEvent] UNIQUE NONCLUSTERED ([FullName] ASC)
);

