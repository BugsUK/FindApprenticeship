CREATE TABLE [dbo].[CandidateHistoryEvent] (
    [CandidateHistoryEventId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                NVARCHAR (3)   NOT NULL,
    [ShortName]               NVARCHAR (100) NOT NULL,
    [FullName]                NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_CandidateHistoryEvent] PRIMARY KEY CLUSTERED ([CandidateHistoryEventId] ASC),
    CONSTRAINT [uq_idx_CandidateHistoryEvent] UNIQUE NONCLUSTERED ([FullName] ASC)
);

