CREATE TABLE [dbo].[VacancyProvisionRelationshipStatusType] (
    [VacancyProvisionRelationshipStatusTypeId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                                 NVARCHAR (3)   NOT NULL,
    [ShortName]                                NVARCHAR (100) NOT NULL,
    [FullName]                                 NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_VacancyProvisionRelationshipStatusType] PRIMARY KEY CLUSTERED ([VacancyProvisionRelationshipStatusTypeId] ASC),
    CONSTRAINT [uq_idx_VacancyProvisionRelationshipStatusType] UNIQUE NONCLUSTERED ([FullName] ASC)
);

