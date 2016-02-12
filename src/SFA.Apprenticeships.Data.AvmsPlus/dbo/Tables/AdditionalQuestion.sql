CREATE TABLE [dbo].[AdditionalQuestion] (
    [AdditionalQuestionId] INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VacancyId]            INT             NOT NULL,
    [QuestionId]           SMALLINT        NOT NULL,
    [Question]             NVARCHAR (4000) NOT NULL,
    CONSTRAINT [PK_AdditionalQuestion] PRIMARY KEY CLUSTERED ([AdditionalQuestionId] ASC),
    CONSTRAINT [FK_AdditionalQuestion_Vacancy] FOREIGN KEY ([VacancyId]) REFERENCES [dbo].[Vacancy] ([VacancyId]),
    CONSTRAINT [uq_idx_additionalQuestion] UNIQUE NONCLUSTERED ([VacancyId] ASC, [QuestionId] ASC)
);

GO
CREATE NONCLUSTERED INDEX [idx_additionalQuestion_vacancyId_questionId] 
ON [dbo].[AdditionalQuestion]
(
	[VacancyId] ASC,
	[QuestionId] ASC
)
