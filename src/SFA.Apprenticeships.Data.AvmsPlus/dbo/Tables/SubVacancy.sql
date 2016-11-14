CREATE TABLE [dbo].[SubVacancy] (
    [SubVacancyId]           INT        IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [VacancyId]              INT        NOT NULL,
    [AllocatedApplicationId] INT        NULL,
    [StartDate]              DATETIME   NULL,
    [ILRNumber]              NCHAR (12) NULL,
    CONSTRAINT [PK_SubVacancy] PRIMARY KEY CLUSTERED ([SubVacancyId] ASC),
    CONSTRAINT [FK_SubVacancy_Application] FOREIGN KEY ([AllocatedApplicationId]) REFERENCES [dbo].[Application] ([ApplicationId]),
    CONSTRAINT [FK_SubVacancy_Vacancy] FOREIGN KEY ([VacancyId]) REFERENCES [dbo].[Vacancy] ([VacancyId]),
    CONSTRAINT [uq_idx_subVacancy] UNIQUE NONCLUSTERED ([VacancyId] ASC, [AllocatedApplicationId] ASC)
);

GO
CREATE NONCLUSTERED INDEX [nci_wi_SubVacancy_B595B0A6C60C23A2AA8ACE078660BCDC] ON [dbo].[SubVacancy]
([AllocatedApplicationId] ASC)
INCLUDE ([ILRNumber],[StartDate],[VacancyId])


GO