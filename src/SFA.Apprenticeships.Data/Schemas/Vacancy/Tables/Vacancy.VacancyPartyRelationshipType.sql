CREATE TABLE [Vacancy].[VacancyPartyRelationshipType]
(
	[VacancyPartyRelationshipTypeCode] CHAR(3) NOT NULL , 
    [FullName] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [PK_VacancyPartyRelationshipType] PRIMARY KEY ([VacancyPartyRelationshipTypeCode])
)
