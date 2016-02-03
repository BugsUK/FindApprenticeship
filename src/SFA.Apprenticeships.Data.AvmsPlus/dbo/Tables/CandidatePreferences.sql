CREATE TABLE [dbo].[CandidatePreferences] (
    [CandidatePreferenceId] INT IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CandidateId]           INT NOT NULL,
    [FirstFrameworkId]      INT NULL,
    [FirstOccupationId]     INT NULL,
    [SecondFrameworkId]     INT NULL,
    [SecondOccupationId]    INT NULL,
    CONSTRAINT [PK_CandidatePreference] PRIMARY KEY CLUSTERED ([CandidatePreferenceId] ASC) ON [PRIMARY],
    CONSTRAINT [FK_CandidatePreferences_ApprenticeshipFrameworkId] FOREIGN KEY ([FirstFrameworkId]) REFERENCES [dbo].[ApprenticeshipFramework] ([ApprenticeshipFrameworkId]),
    CONSTRAINT [FK_CandidatePreferences_ApprenticeshipOccupationId] FOREIGN KEY ([FirstOccupationId]) REFERENCES [dbo].[ApprenticeshipOccupation] ([ApprenticeshipOccupationId]),
    CONSTRAINT [FK_CandidatePreferences_CandidateId] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Candidate] ([CandidateId]),
    CONSTRAINT [FK_CandidatePreferences_SecondApprenticeshipFrameworkId] FOREIGN KEY ([SecondFrameworkId]) REFERENCES [dbo].[ApprenticeshipFramework] ([ApprenticeshipFrameworkId]),
    CONSTRAINT [FK_CandidatePreferences_SecondApprenticeshipOccupationId] FOREIGN KEY ([SecondOccupationId]) REFERENCES [dbo].[ApprenticeshipOccupation] ([ApprenticeshipOccupationId])
);

