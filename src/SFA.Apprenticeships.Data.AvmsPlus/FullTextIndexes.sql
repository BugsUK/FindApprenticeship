CREATE FULLTEXT INDEX ON [dbo].[Vacancy]
    ([Title] LANGUAGE 2057, [ShortDescription] LANGUAGE 2057, [Description] LANGUAGE 1033)
    KEY INDEX [PK_Vacancy_1]
    ON ([NAVMS_FTI_CATALOG], FILEGROUP [PRIMARY])
    WITH STOPLIST OFF;


GO
CREATE FULLTEXT INDEX ON [dbo].[VacancySearch]
    ([EmployerName] LANGUAGE 1033, [VacancyOwnerName] LANGUAGE 1033, [Title] LANGUAGE 1033, [ShortDescription] LANGUAGE 1033, [Description] LANGUAGE 1033, [ApprenticeshipOccupationName] LANGUAGE 1033, [ApprenticeshipFrameworkName] LANGUAGE 1033, [EmployerSearch] LANGUAGE 1033, [TrainingProviderSearch] LANGUAGE 1033, [DeliveryOrganisationSearch] LANGUAGE 1033, [RealityCheck] LANGUAGE 1033, [OtherImportantInformation] LANGUAGE 1033)
    KEY INDEX [PK_VacancySearch_1]
    ON ([NAVMS_FTI_CATALOG], FILEGROUP [PRIMARY])
    WITH STOPLIST OFF;


GO
CREATE FULLTEXT INDEX ON [dbo].[School]
    ([SchoolNameForSearch] LANGUAGE 2057)
    KEY INDEX [PK_School]
    ON ([NAVMS_FTI_SchoolSearchCATALOG], FILEGROUP [PRIMARY])
    WITH STOPLIST OFF;

