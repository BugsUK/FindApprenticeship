﻿CREATE TABLE [dbo].[VacancyLocationType] (
    [VacancyLocationTypeId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]              NVARCHAR (3)   NOT NULL,
    [ShortName]             NVARCHAR (100) NOT NULL,
    [FullName]              NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_VacancyLocationType_VacancyLocationTypeId] PRIMARY KEY CLUSTERED ([VacancyLocationTypeId] ASC)
);

