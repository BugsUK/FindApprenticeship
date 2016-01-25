CREATE TABLE [Vacancy].[VacancyPartyType]
(
	[VacancyPartyTypeCode] CHAR(3) NOT NULL, 
    [FullName] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [PK_VacancyPartyType] PRIMARY KEY ([VacancyPartyTypeCode]) 
)
