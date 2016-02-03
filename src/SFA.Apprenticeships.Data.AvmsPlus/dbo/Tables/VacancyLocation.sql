CREATE TABLE [dbo].[VacancyLocation] (
    [VacancyLocationId] INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VacancyId]         INT              NOT NULL,
    [NumberofPositions] SMALLINT         NULL,
    [AddressLine1]      NVARCHAR (50)    NULL,
    [AddressLine2]      NVARCHAR (50)    NULL,
    [AddressLine3]      NVARCHAR (50)    NULL,
    [AddressLine4]      NVARCHAR (50)    NULL,
    [AddressLine5]      NVARCHAR (50)    NULL,
    [Town]              NVARCHAR (40)    NULL,
    [CountyId]          INT              NULL,
    [PostCode]          NVARCHAR (8)     NULL,
    [LocalAuthorityId]  INT              NULL,
    [GeocodeEasting]    INT              NULL,
    [GeocodeNorthing]   INT              NULL,
    [Longitude]         DECIMAL (13, 10) NULL,
    [Latitude]          DECIMAL (13, 10) NULL,
    [EmployersWebsite]  NVARCHAR (256)   NULL,
    CONSTRAINT [PK_VacancyLocation_VacancyLocationId] PRIMARY KEY CLUSTERED ([VacancyLocationId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [FK_VacancyLocation_County] FOREIGN KEY ([CountyId]) REFERENCES [dbo].[County] ([CountyId]),
    CONSTRAINT [FK_VacancyLocation_VacancyId] FOREIGN KEY ([VacancyId]) REFERENCES [dbo].[Vacancy] ([VacancyId])
);

