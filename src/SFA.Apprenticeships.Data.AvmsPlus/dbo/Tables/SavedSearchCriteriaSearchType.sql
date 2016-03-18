CREATE TABLE [dbo].[SavedSearchCriteriaSearchType] (
    [SavedSearchCriteriaSearchTypeId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                        NVARCHAR (3)   NOT NULL,
    [ShortName]                       NVARCHAR (100) NOT NULL,
    [FullName]                        NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_SavedSearchCriteriaSearchType] PRIMARY KEY CLUSTERED ([SavedSearchCriteriaSearchTypeId] ASC),
    CONSTRAINT [uq_idx_SavedSearchCriteriaSearchType] UNIQUE NONCLUSTERED ([FullName] ASC)
);

