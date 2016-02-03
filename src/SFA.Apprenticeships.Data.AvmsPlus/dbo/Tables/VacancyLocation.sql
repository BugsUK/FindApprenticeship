CREATE TABLE [dbo].[VacancyLocation] (
    [VacancyLocationId] INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VacancyId]         INT              NOT NULL,
    [NumberofPositions] SMALLINT         NULL,
    [AddressLine1]      NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [AddressLine2]      NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [AddressLine3]      NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [AddressLine4]      NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [AddressLine5]      NVARCHAR (50)    COLLATE Latin1_General_CI_AS NULL,
    [Town]              NVARCHAR (40)    COLLATE Latin1_General_CI_AS NULL,
    [CountyId]          INT              NULL,
    [PostCode]          NVARCHAR (8)     COLLATE Latin1_General_CI_AS NULL,
    [LocalAuthorityId]  INT              NULL,
    [GeocodeEasting]    INT              NULL,
    [GeocodeNorthing]   INT              NULL,
    [Longitude]         DECIMAL (13, 10) NULL,
    [Latitude]          DECIMAL (13, 10) NULL,
    [EmployersWebsite]  NVARCHAR (256)   COLLATE Latin1_General_CI_AS NULL,
    CONSTRAINT [PK_VacancyLocation_VacancyLocationId] PRIMARY KEY CLUSTERED ([VacancyLocationId] ASC),
    CONSTRAINT [FK_VacancyLocation_County] FOREIGN KEY ([CountyId]) REFERENCES [dbo].[County] ([CountyId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_VacancyLocation_VacancyId] FOREIGN KEY ([VacancyId]) REFERENCES [dbo].[Vacancy] ([VacancyId]) NOT FOR REPLICATION
);



