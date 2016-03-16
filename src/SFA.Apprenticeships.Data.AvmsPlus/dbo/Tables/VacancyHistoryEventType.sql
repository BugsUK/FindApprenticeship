CREATE TABLE [dbo].[VacancyHistoryEventType] (
    [VacancyHistoryEventTypeId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                  NVARCHAR (3)   NOT NULL,
    [ShortName]                 NVARCHAR (100) NOT NULL,
    [FullName]                  NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_VacancyHistoryEventType] PRIMARY KEY CLUSTERED ([VacancyHistoryEventTypeId] ASC),
    CONSTRAINT [uq_idx_VacancyHistoryEventType] UNIQUE NONCLUSTERED ([FullName] ASC)
);

