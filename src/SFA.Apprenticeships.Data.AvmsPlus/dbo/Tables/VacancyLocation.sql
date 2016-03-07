CREATE TABLE [dbo].[VacancyLocation] (
    [VacancyLocationId] INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VacancyId]         INT              NOT NULL,
    [NumberofPositions] SMALLINT         NULL,
    [AddressLine1]      NVARCHAR (MAX)   NULL,
    [AddressLine2]      NVARCHAR (MAX)   NULL,
    [AddressLine3]      NVARCHAR (MAX)   NULL,
    [AddressLine4]      NVARCHAR (MAX)   NULL,
    [AddressLine5]      NVARCHAR (MAX)   NULL,
    [Town]              NVARCHAR (MAX)   NULL,
    [CountyId]          INT              NULL,
    [PostCode]          NVARCHAR (MAX)   NULL,
    [LocalAuthorityId]  INT              NULL,
    [GeocodeEasting]    INT              NULL,
    [GeocodeNorthing]   INT              NULL,
    [Longitude]         DECIMAL (13, 10) NULL,
    [Latitude]          DECIMAL (13, 10) NULL,
    [EmployersWebsite]  NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_VacancyLocation_VacancyLocationId] PRIMARY KEY CLUSTERED ([VacancyLocationId] ASC),
    CONSTRAINT [FK_VacancyLocation_County] FOREIGN KEY ([CountyId]) REFERENCES [dbo].[County] ([CountyId]),
    CONSTRAINT [FK_VacancyLocation_VacancyId] FOREIGN KEY ([VacancyId]) REFERENCES [dbo].[Vacancy] ([VacancyId])
);

