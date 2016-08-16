CREATE TABLE [dbo].[VacancySource] (
    [VacancySourceId]		  INT           NOT NULL IDENTITY (-1, -1),
    [CodeName]                NVARCHAR (3)  NOT NULL,
    [ShortName]               NVARCHAR (10) NOT NULL,
    [FullName]                NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_VacancySource] PRIMARY KEY CLUSTERED ([VacancySourceId] ASC),
	CONSTRAINT [uq_idx_VacancySource] UNIQUE NONCLUSTERED ([FullName] ASC)
);

