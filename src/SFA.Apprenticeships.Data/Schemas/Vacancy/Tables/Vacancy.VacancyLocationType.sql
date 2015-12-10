CREATE TABLE [Vacancy].[VacancyLocationType]
(
	[VacancyLocationTypeCode] CHAR NOT NULL, 
    [FullName] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [PK_VacancyLocationType] PRIMARY KEY ([VacancyLocationTypeCode]) 
)
