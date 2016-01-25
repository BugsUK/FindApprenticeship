CREATE TABLE [Vacancy].[VacancyStatus]
(
	[VacancyStatusCode] CHAR(3) NOT NULL , 
    [FullName] VARCHAR(MAX) NOT NULL, 
    CONSTRAINT [PK_VacancyStatus] PRIMARY KEY ([VacancyStatusCode]) 
)
