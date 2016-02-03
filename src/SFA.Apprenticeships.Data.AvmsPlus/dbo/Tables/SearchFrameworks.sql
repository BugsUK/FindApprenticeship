﻿CREATE TABLE [dbo].[SearchFrameworks] (
    [SearchFrameworksId]    INT IDENTITY (0, 1) NOT FOR REPLICATION NOT NULL,
    [FrameworkId]           INT NOT NULL,
    [SavedSearchCriteriaId] INT NOT NULL,
    CONSTRAINT [PK_SearchFrameworks] PRIMARY KEY CLUSTERED ([SearchFrameworksId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [FK_SearchFrameworks_ApprenticeshipFramework] FOREIGN KEY ([FrameworkId]) REFERENCES [dbo].[ApprenticeshipFramework] ([ApprenticeshipFrameworkId]),
    CONSTRAINT [FK_SearchFrameworks_SavedSearchCriteria] FOREIGN KEY ([SavedSearchCriteriaId]) REFERENCES [dbo].[SavedSearchCriteria] ([SavedSearchCriteriaId])
);


GO
CREATE NONCLUSTERED INDEX [idx_SearchFrameworks_SavedSearchCriteriaId]
    ON [dbo].[SearchFrameworks]([SavedSearchCriteriaId] ASC)
    INCLUDE([FrameworkId]) WITH (FILLFACTOR = 90)
    ON [Index];

