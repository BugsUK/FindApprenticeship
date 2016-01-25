CREATE TABLE [Vacancy].[VacancyLocation]
(
	[VacancyLocationId] INT NOT NULL IDENTITY, 
	[VacancyId] UNIQUEIDENTIFIER NOT NULL, 
    [PostalAddressId] INT NOT NULL, 
    [NumberOfPositions] INT NOT NULL, 
    [DirectApplicationUrl] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_VacancyLocation] PRIMARY KEY ([VacancyLocationId]), 
    CONSTRAINT [FK_VacancyLocation_VacancyId] FOREIGN KEY ([VacancyId]) REFERENCES [Vacancy].[Vacancy]([VacancyId]), 
    CONSTRAINT [FK_VacancyLocation_PostalAddressId] FOREIGN KEY ([PostalAddressId]) REFERENCES [Address].[PostalAddress]([PostalAddressId]), 
    CONSTRAINT [CK_VacancyLocation_NumberOfPositions] CHECK (NumberOfPositions > 0) 
)
