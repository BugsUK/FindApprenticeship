CREATE TABLE [Vacancy].[VacancyParty]
(
    [VacancyPartyId] INT NOT NULL IDENTITY, 
    [VacancyPartyTypeCode] CHAR(3) NOT NULL, 
    [FullName] NVARCHAR(MAX) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [PostalAddressId] INT NULL, 
    [WebsiteUrl] NVARCHAR(MAX) NULL, 
    [EDSURN] INT NULL, 
    [UKPRN] INT NULL, 
    CONSTRAINT [PK_VacancyParty] PRIMARY KEY ([VacancyPartyId]), 
    CONSTRAINT [FK_VacancyParty_VacancyTypeCode] FOREIGN KEY ([VacancyPartyTypeCode]) REFERENCES [Vacancy].[VacancyPartyType]([VacancyPartyTypeCode]), 
    CONSTRAINT [FK_VacancyParty_PostalAddress] FOREIGN KEY ([PostalAddressId]) REFERENCES [Address].[PostalAddress]([PostalAddressId]), 
    CONSTRAINT [CK_VacancyParty_EDSURN_UKPRN] CHECK ((EDSURN IS NOT NULL AND UKPRN IS NULL) OR (EDSURN IS NULL AND UKPRN IS NOT NULL))
)
