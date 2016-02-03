CREATE TABLE [dbo].[SavedSearchCriteria] (
    [SavedSearchCriteriaId]  INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CandidateId]            INT              NOT NULL,
    [Name]                   NVARCHAR (50)    COLLATE Latin1_General_CI_AS NOT NULL,
    [SearchType]             INT              NOT NULL,
    [CountyId]               INT              NULL,
    [Postcode]               NVARCHAR (8)     COLLATE Latin1_General_CI_AS NULL,
    [Longitude]              DECIMAL (13, 10) NULL,
    [Latitude]               DECIMAL (13, 10) NULL,
    [GeocodeEasting]         INT              NULL,
    [GeocodeNorthing]        INT              NULL,
    [DistanceFromPostcode]   SMALLINT         NULL,
    [MinWages]               SMALLINT         NULL,
    [MaxWages]               SMALLINT         NULL,
    [VacancyReferenceNumber] INT              NULL,
    [Employer]               NVARCHAR (255)   COLLATE Latin1_General_CI_AS NULL,
    [TrainingProvider]       NVARCHAR (255)   COLLATE Latin1_General_CI_AS NULL,
    [Keywords]               NVARCHAR (100)   COLLATE Latin1_General_CI_AS NULL,
    [DateSearched]           DATETIME         NULL,
    [BackgroundSearch]       BIT              CONSTRAINT [DF_SavedSearchCriteria_BackgroundSearch] DEFAULT ((0)) NOT NULL,
    [AlertSent]              BIT              CONSTRAINT [DF_SavedSearchCriteria_AlertSent] DEFAULT ((0)) NOT NULL,
    [CountBackgroundMatches] INT              NULL,
    [VacancyPostedSince]     INT              CONSTRAINT [DF_SavedSearchCriteria_VacancyPostedSince] DEFAULT ((0)) NOT NULL,
    [ApprenticeshipTypeId]   INT              NULL,
    CONSTRAINT [PK_SavedSearchCriteria] PRIMARY KEY CLUSTERED ([SavedSearchCriteriaId] ASC),
    CONSTRAINT [FK_SavedSearchCriteria_Candidate] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Candidate] ([CandidateId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_SavedSearchCriteria_SavedSearchCriteriaSearchType] FOREIGN KEY ([SearchType]) REFERENCES [dbo].[SavedSearchCriteriaSearchType] ([SavedSearchCriteriaSearchTypeId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_SavedSearchCriteria_SavedSearchCriteriaVacancyPostedSince] FOREIGN KEY ([VacancyPostedSince]) REFERENCES [dbo].[SavedSearchCriteriaVacancyPostedSince] ([SavedSearchCriteriaVacancyPostedSince]) NOT FOR REPLICATION,
    CONSTRAINT [uq_idx_savedSearchCriteria] UNIQUE NONCLUSTERED ([CandidateId] ASC, [Name] ASC)
);



