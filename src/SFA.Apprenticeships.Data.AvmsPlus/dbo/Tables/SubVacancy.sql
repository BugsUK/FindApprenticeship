CREATE TABLE [dbo].[SubVacancy] (
    [SubVacancyId]           INT        IDENTITY (0, 1) NOT FOR REPLICATION NOT NULL,
    [VacancyId]              INT        NOT NULL,
    [AllocatedApplicationId] INT        NULL,
    [StartDate]              DATETIME   NULL,
    [ILRNumber]              NCHAR (12) COLLATE Latin1_General_CI_AS NULL,
    CONSTRAINT [PK_SubVacancy] PRIMARY KEY CLUSTERED ([SubVacancyId] ASC),
    CONSTRAINT [FK_SubVacancy_Application] FOREIGN KEY ([AllocatedApplicationId]) REFERENCES [dbo].[Application] ([ApplicationId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_SubVacancy_Vacancy] FOREIGN KEY ([VacancyId]) REFERENCES [dbo].[Vacancy] ([VacancyId]) NOT FOR REPLICATION,
    CONSTRAINT [uq_idx_subVacancy] UNIQUE NONCLUSTERED ([VacancyId] ASC, [AllocatedApplicationId] ASC)
);



