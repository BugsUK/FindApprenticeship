CREATE TABLE [dbo].[VacancyHistory] (
    [VacancyHistoryId]             INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VacancyId]                    INT             NOT NULL,
    [UserName]                     NVARCHAR (50)   NOT NULL,
    [VacancyHistoryEventTypeId]    INT             NOT NULL,
    [VacancyHistoryEventSubTypeId] INT             NULL,
    [HistoryDate]                  DATETIME        NOT NULL,
    [Comment]                      NVARCHAR (4000) NULL,
    CONSTRAINT [PK_Vacancy_History] PRIMARY KEY CLUSTERED ([VacancyHistoryId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [FK_VacancyHistory_Vacancy] FOREIGN KEY ([VacancyId]) REFERENCES [dbo].[Vacancy] ([VacancyId]),
    CONSTRAINT [FK_VacancyHistory_VacancyHistoryEvent] FOREIGN KEY ([VacancyHistoryEventTypeId]) REFERENCES [dbo].[VacancyHistoryEventType] ([VacancyHistoryEventTypeId])
);


GO
CREATE NONCLUSTERED INDEX [idx_VacancyHistory_VacancyHistoryEventSubTypeId]
    ON [dbo].[VacancyHistory]([VacancyHistoryEventSubTypeId] ASC)
    INCLUDE([VacancyId], [HistoryDate])
    ON [Index];


GO
CREATE NONCLUSTERED INDEX [idx_VacancyHistory_VacancyHistoryEventTypeId_VacancyHistoryEventSubTypeId]
    ON [dbo].[VacancyHistory]([VacancyHistoryEventTypeId] ASC, [VacancyHistoryEventSubTypeId] ASC)
    INCLUDE([VacancyId], [HistoryDate])
    ON [Index];


GO
CREATE NONCLUSTERED INDEX [idx_VacancyHistory_VacancyId_VacancyHistoryEventSubTypeId]
    ON [dbo].[VacancyHistory]([VacancyId] ASC, [VacancyHistoryEventSubTypeId] ASC)
    INCLUDE([HistoryDate])
    ON [Index];

