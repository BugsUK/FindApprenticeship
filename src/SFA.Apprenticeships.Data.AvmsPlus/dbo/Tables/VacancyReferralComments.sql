CREATE TABLE [dbo].[VacancyReferralComments] (
    [VacancyReferralCommentsID] INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VacancyId]                 INT             NOT NULL,
    [FieldTypeId]               INT             NOT NULL,
    [Comments]                  NVARCHAR (4000) NULL,
    CONSTRAINT [PK_VacancyReferralComments] PRIMARY KEY CLUSTERED ([VacancyReferralCommentsID] ASC),
    CONSTRAINT [FK_VacancyReferralComments_Vacancy] FOREIGN KEY ([VacancyId]) REFERENCES [dbo].[Vacancy] ([VacancyId]),
    CONSTRAINT [FK_VacancyReferralComments_VacancyReferralCommentsFieldType] FOREIGN KEY ([FieldTypeId]) REFERENCES [dbo].[VacancyReferralCommentsFieldType] ([VacancyReferralCommentsFieldTypeId]),
    CONSTRAINT [uq_idx_vacancyReferralComments] UNIQUE NONCLUSTERED ([VacancyId] ASC, [FieldTypeId] ASC)
);

