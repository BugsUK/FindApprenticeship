CREATE TABLE [dbo].[CandidateBroadcastMessage] (
    [CandidateId] INT NOT NULL,
    [MessageId]   INT NOT NULL,
    CONSTRAINT [PK_CandidateBroadcastMessage] PRIMARY KEY CLUSTERED ([CandidateId] ASC, [MessageId] ASC),
    CONSTRAINT [FK_CandidateBroadcastMessage_CandidateId] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Candidate] ([CandidateId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_CandidateBroadcastMessage_MessageId] FOREIGN KEY ([MessageId]) REFERENCES [dbo].[Message] ([MessageId]) NOT FOR REPLICATION
);




GO
CREATE NONCLUSTERED INDEX [idx_CandidateBroadcastMessage_MessageID]
    ON [dbo].[CandidateBroadcastMessage]([MessageId] ASC) WITH (FILLFACTOR = 90)
    ON [Index];

