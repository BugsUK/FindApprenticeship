CREATE TABLE [dbo].[VacancyReferralCommentsFieldType] (
    [VacancyReferralCommentsFieldTypeId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                           NVARCHAR (3)   NOT NULL,
    [ShortName]                          NVARCHAR (100) NOT NULL,
    [FullName]                           NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_VacancyReferralCommentsFieldType] PRIMARY KEY CLUSTERED ([VacancyReferralCommentsFieldTypeId] ASC),
    CONSTRAINT [uq_idx_VacancyReferralCommentsFieldType] UNIQUE NONCLUSTERED ([FullName] ASC)
);

