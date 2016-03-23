CREATE TABLE [dbo].[VacancyTextFieldValue] (
    [VacancyTextFieldValueId] INT           IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                NVARCHAR (3)  NOT NULL,
    [ShortName]               NVARCHAR (10) NOT NULL,
    [FullName]                NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_VacancyTextFieldValue] PRIMARY KEY CLUSTERED ([VacancyTextFieldValueId] ASC),
    CONSTRAINT [uq_idx_VacancyTextFieldValue] UNIQUE NONCLUSTERED ([FullName] ASC)
);

