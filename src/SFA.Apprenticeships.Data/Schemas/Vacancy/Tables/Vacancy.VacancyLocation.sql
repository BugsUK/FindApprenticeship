CREATE TABLE [Vacancy].[VacancyLocation]
(
	[VacancyLocationId] INT NOT NULL IDENTITY, 
	[VacancyId] UNIQUEIDENTIFIER NOT NULL, 
    [AddressId] INT NOT NULL, 
    [NumberOfPositions] INT NOT NULL, 
    [DirectApplicationUrl] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_VacancyLocation] PRIMARY KEY ([VacancyLocationId]), 
    CONSTRAINT [FK_VacancyLocation_VacancyId] FOREIGN KEY ([VacancyId]) REFERENCES [Vacancy].[Vacancy]([VacancyId]), 
    CONSTRAINT [CK_VacancyLocation_NumberOfPositions] CHECK (NumberOfPositions > 0) 
)
