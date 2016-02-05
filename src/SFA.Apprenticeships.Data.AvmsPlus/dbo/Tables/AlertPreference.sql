CREATE TABLE [dbo].[AlertPreference] (
    [AlertPreferenceId] INT IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CandidateId]       INT NOT NULL,
    [AlertTypeId]       INT NOT NULL,
    [SMSAlert]          BIT NOT NULL,
    [EmailAlert]        BIT NOT NULL,
    CONSTRAINT [PK_AlertPreference] PRIMARY KEY CLUSTERED ([AlertPreferenceId] ASC),
    CONSTRAINT [FK_AlertPreference_AlertType] FOREIGN KEY ([AlertTypeId]) REFERENCES [dbo].[AlertType] ([AlertTypeId]),
    CONSTRAINT [FK_AlertPreference_Candidate] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Candidate] ([CandidateId]),
    CONSTRAINT [uq_idx_alertPreference] UNIQUE NONCLUSTERED ([CandidateId] ASC, [AlertTypeId] ASC)
);

