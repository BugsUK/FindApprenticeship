CREATE TABLE [Vacancy].[VacancyType]
(
	[VacancyTypeCode] CHAR NOT NULL, 
    [FullName] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [PK_VacancyType] PRIMARY KEY ([VacancyTypeCode]) 
)
