CREATE TABLE [dbo].[ExternalApplication] (
    [ExternalApplicationId] INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CandidateId]           INT              NOT NULL,
    [VacancyId]             INT              NOT NULL,
    [ClickthroughDate]      DATETIME         NOT NULL,
    [ExternalTrackingId]    UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_ExternalApplication] PRIMARY KEY CLUSTERED ([ExternalApplicationId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [FK_ExternalApplication_Candidate] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Candidate] ([CandidateId]),
    CONSTRAINT [FK_ExternalApplication_Vacancy] FOREIGN KEY ([VacancyId]) REFERENCES [dbo].[Vacancy] ([VacancyId]),
    CONSTRAINT [uq_idx_externalApplication] UNIQUE NONCLUSTERED ([CandidateId] ASC, [VacancyId] ASC) WITH (FILLFACTOR = 90) ON [Index]
);

