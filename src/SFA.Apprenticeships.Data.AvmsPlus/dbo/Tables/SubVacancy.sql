CREATE TABLE [dbo].[SubVacancy] (
    [SubVacancyId]           INT        IDENTITY (0, 1) NOT FOR REPLICATION NOT NULL,
    [VacancyId]              INT        NOT NULL,
    [AllocatedApplicationId] INT        NULL,
    [StartDate]              DATETIME   NULL,
    [ILRNumber]              NCHAR (12) NULL,
    CONSTRAINT [PK_SubVacancy] PRIMARY KEY CLUSTERED ([SubVacancyId] ASC),
    CONSTRAINT [FK_SubVacancy_Application] FOREIGN KEY ([AllocatedApplicationId]) REFERENCES [dbo].[Application] ([ApplicationId]),
    CONSTRAINT [FK_SubVacancy_Vacancy] FOREIGN KEY ([VacancyId]) REFERENCES [dbo].[Vacancy] ([VacancyId]),
    CONSTRAINT [uq_idx_subVacancy] UNIQUE NONCLUSTERED ([VacancyId] ASC, [AllocatedApplicationId] ASC)
);

