CREATE TABLE [dbo].[VacancyStatusType] (
    [VacancyStatusTypeId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]            NVARCHAR (3)   NOT NULL,
    [ShortName]           NVARCHAR (100) NOT NULL,
    [FullName]            NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_VacancyStatusType] PRIMARY KEY CLUSTERED ([VacancyStatusTypeId] ASC),
    CONSTRAINT [uq_idx_VacancyStatusType] UNIQUE NONCLUSTERED ([FullName] ASC)
);

