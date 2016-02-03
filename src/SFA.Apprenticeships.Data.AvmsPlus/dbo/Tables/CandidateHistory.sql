CREATE TABLE [dbo].[CandidateHistory] (
    [CandidateHistoryId]             INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CandidateId]                    INT             NOT NULL,
    [CandidateHistoryEventTypeId]    INT             NOT NULL,
    [CandidateHistorySubEventTypeId] INT             NULL,
    [EventDate]                      DATETIME        NOT NULL,
    [Comment]                        NVARCHAR (4000) NULL,
    [UserName]                       NVARCHAR (50)   NULL,
    CONSTRAINT [PK_CandidateHistory] PRIMARY KEY CLUSTERED ([CandidateHistoryId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [FK_CandidateHistory_Candidate] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Candidate] ([CandidateId]),
    CONSTRAINT [FK_CandidateHistory_CandidateHistoryEvent] FOREIGN KEY ([CandidateHistoryEventTypeId]) REFERENCES [dbo].[CandidateHistoryEvent] ([CandidateHistoryEventId])
);


GO
CREATE NONCLUSTERED INDEX [idx_CandidateHistory_CandidateId_CandidateHistorySubEventTypeId]
    ON [dbo].[CandidateHistory]([CandidateId] ASC, [CandidateHistorySubEventTypeId] ASC)
    INCLUDE([CandidateHistoryEventTypeId], [EventDate]) WITH (FILLFACTOR = 90)
    ON [Index];

