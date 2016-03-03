CREATE TABLE [dbo].[VacancyType] (
    [VacancyTypeId]			  INT           NOT NULL IDENTITY,
    [CodeName]                NVARCHAR (3)  NOT NULL,
    [ShortName]               NVARCHAR (10) NOT NULL,
    [FullName]                NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_VacancyType] PRIMARY KEY CLUSTERED ([VacancyTypeId] ASC),
	CONSTRAINT [uq_idx_VacancyType] UNIQUE NONCLUSTERED ([FullName] ASC)
);

