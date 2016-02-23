CREATE TABLE [dbo].[SavedSearchCriteriaVacancyPostedSince] (
    [SavedSearchCriteriaVacancyPostedSince] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                              NVARCHAR (3)   NOT NULL,
    [ShortName]                             NVARCHAR (100) NOT NULL,
    [FullName]                              NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_SavedSearchCriteriaVacancyPostedSince] PRIMARY KEY CLUSTERED ([SavedSearchCriteriaVacancyPostedSince] ASC),
    CONSTRAINT [uq_idx_SavedSearchCriteriaVacancyPostedSince] UNIQUE NONCLUSTERED ([FullName] ASC)
);

