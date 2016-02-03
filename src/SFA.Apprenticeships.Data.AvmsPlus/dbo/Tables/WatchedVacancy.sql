CREATE TABLE [dbo].[WatchedVacancy] (
    [WatchedVacancyId] INT IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CandidateId]      INT NOT NULL,
    [VacancyId]        INT NOT NULL,
    CONSTRAINT [PK_WatchedVacancy] PRIMARY KEY CLUSTERED ([WatchedVacancyId] ASC),
    CONSTRAINT [FK_WatchedVacancy_Candidate] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Candidate] ([CandidateId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_WatchedVacancy_Vacancy] FOREIGN KEY ([VacancyId]) REFERENCES [dbo].[Vacancy] ([VacancyId]) NOT FOR REPLICATION,
    CONSTRAINT [uq_idx_watchedVacancy] UNIQUE NONCLUSTERED ([CandidateId] ASC, [VacancyId] ASC)
);



