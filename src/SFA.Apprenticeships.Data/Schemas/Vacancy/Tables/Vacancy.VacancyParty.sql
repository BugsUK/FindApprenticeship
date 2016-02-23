CREATE TABLE [Vacancy].[VacancyParty]
(
    [VacancyPartyId] INT NOT NULL IDENTITY, 
    [VacancyPartyTypeCode] CHAR(3) NOT NULL, 
    [FullName] NVARCHAR(MAX) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [PostalAddressId] INT NULL, 
    [WebsiteUrl] NVARCHAR(MAX) NULL, 
    [EdsUrn] INT NULL, 
    [UKPrn] INT NULL, 
    CONSTRAINT [PK_VacancyParty] PRIMARY KEY ([VacancyPartyId]), 
    CONSTRAINT [FK_VacancyParty_VacancyTypeCode] FOREIGN KEY ([VacancyPartyTypeCode]) REFERENCES [Vacancy].[VacancyPartyType]([VacancyPartyTypeCode]), 
    CONSTRAINT [FK_VacancyParty_PostalAddress] FOREIGN KEY ([PostalAddressId]) REFERENCES [Address].[PostalAddress]([PostalAddressId])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Identifies the site',
    @level0type = N'SCHEMA',
    @level0name = N'Vacancy',
    @level1type = N'TABLE',
    @level1name = N'VacancyParty',
    @level2type = N'COLUMN',
    @level2name = 'EdsUrn'
