CREATE TABLE [Vacancy].[VacancyPartyRelationship]
(
	[VacancyPartyRelationshipId] INT NOT NULL IDENTITY, 
    [FromVacancyPartyId] INT NOT NULL, 
    [ToVacancyPartyId] INT NOT NULL, 
    [VacancyPartyRelationshipTypeCode] CHAR(3) NULL, 
    CONSTRAINT [PK_VacancyPartyRelationship] PRIMARY KEY ([VacancyPartyRelationshipId]), 
    CONSTRAINT [FK_VacancyPartyRelationship_FromVacancyPartyId] FOREIGN KEY ([FromVacancyPartyId]) REFERENCES [Vacancy].[VacancyParty]([VacancyPartyId]), 
    CONSTRAINT [FK_VacancyPartyRelationship_ToVacancyPartyId] FOREIGN KEY ([ToVacancyPartyId]) REFERENCES [Vacancy].[VacancyParty]([VacancyPartyId]), 
    CONSTRAINT [FK_VacancyPartyRelationship_VacancyPartyRelationshipType] FOREIGN KEY ([VacancyPartyRelationshipTypeCode]) REFERENCES [Vacancy].[VacancyPartyRelationshipType]([VacancyPartyRelationshipTypeCode]) 
)
