CREATE TABLE [dbo].[VacancyProvisionRelationshipHistoryEventType] (
    [VacancyProvisionRelationshipHistoryEventTypeId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                                       NVARCHAR (50)  NOT NULL,
    [ShortName]                                      NVARCHAR (100) NOT NULL,
    [FullName]                                       NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_VacancyProvisionRelationshipHistoryEventType] PRIMARY KEY CLUSTERED ([VacancyProvisionRelationshipHistoryEventTypeId] ASC),
    CONSTRAINT [uq_idx_VacancyProvisionRelationshipHistoryEventType] UNIQUE NONCLUSTERED ([FullName] ASC)
);

