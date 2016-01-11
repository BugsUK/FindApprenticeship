﻿CREATE TABLE [Vacancy].[Vacancy] (
    [VacancyId]                            INT             IDENTITY (1, 1) NOT NULL,
    [VacancyReferenceNumber]               INT             NOT NULL,
    [VacancyTypeCode]                      CHAR (1)        NOT NULL,
    [VacancyStatusCode]                    CHAR (3)        NOT NULL,
    [VacancyLocationTypeCode]              CHAR (1)        NOT NULL,
    [ParentVacancyId]                      INT             NULL,
    [EmployerVacancyPartyId]               INT             NOT NULL,
    [OwnerVacancyPartyId]                  INT             NOT NULL,
    [ManagerVacancyPartyId]                INT             NOT NULL,
    [DeliveryProviderVacancyPartyId]       INT             NOT NULL,
    [ContractOwnerVacancyPartyId]          INT             NOT NULL,
    [OriginalContractOwnerVacancyPartyId]  INT             NULL,
    [Title]                                NVARCHAR (MAX)  NOT NULL,
    [TitleComment]                         NVARCHAR (MAX)  NULL,
    [ShortDescription]                     NVARCHAR (MAX)  NULL,
    [ShortDescriptionComment]              NVARCHAR (MAX)  NULL,
    [LongDescription]                      NVARCHAR (MAX)  NULL,
    [LongDescriptionComment]               NVARCHAR (MAX)  NULL,
    [TrainingTypeCode]                     CHAR (1)        NOT NULL,
    [FrameworkId]                          INT             NULL,
    [FrameworkIdComment]                   INT             NOT NULL,
    [StandardId]                           INT             NULL,
    [StandardIdComment]                    NVARCHAR (MAX)  NULL,
    [LevelCode]                            CHAR (1)        NOT NULL,
    [LevelCodeComment]                     NVARCHAR (MAX)  NULL,
    [WageValue]                            MONEY           NULL,
    [WageTypeCode]                         CHAR (3)        NULL,
    [AV_WageText]                          NVARCHAR (MAX)  NULL,
    [WageComment]                          NVARCHAR (MAX)  NULL,
    [PossibleStartDateDate]                DATETIME2 (7)   NULL,
    [PossibleStartDateComment]             NVARCHAR (MAX)  NULL,
    [ClosingDate]                          DATETIME2 (7)   NULL,
    [ClosingDateComment]                   NVARCHAR (MAX)  NULL,
    [StartDate]                            DATETIME2 (7)   NULL,
    [AV_InterviewStartDate]                DATETIME2 (7)   NULL,
    [DurationValue]                        SMALLINT        NULL,
    [DurationTypeCode]                     CHAR (1)        NULL,
    [DurationComment]                      NVARCHAR (MAX)  NULL,
    [WorkingWeekText]                      NVARCHAR (MAX)  NULL,
    [HoursPerWeek]                         DECIMAL (10, 2) NULL,
    [AV_ContactName]                       NVARCHAR (MAX)  NULL,
    [EmployerDescription]                  NVARCHAR (MAX)  NULL,
    [IsDirectApplication]                  BIT             NULL,
    [DirectApplicationUrl]                 NVARCHAR (MAX)  NULL,
    [DirectApplicationUrlComment]          NVARCHAR (MAX)  NULL,
    [DirectApplicationInstructions]        NVARCHAR (MAX)  NULL,
    [DirectApplicationInstructionsComment] NVARCHAR (MAX)  NULL,
    [DesiredSkills]                        NVARCHAR (MAX)  NULL,
    [DesiredSkillsComment]                 NVARCHAR (MAX)  NULL,
    [FutureProspects]                      NVARCHAR (MAX)  NULL,
    [FutureProspectsComment]               NVARCHAR (MAX)  NULL,
    [PersonalQualities]                    NVARCHAR (MAX)  NULL,
    [PersonalQualitiesComment]             NVARCHAR (MAX)  NULL,
    [ThingsToConsider]                     NVARCHAR (MAX)  NULL,
    [ThingsToConsiderComment]              NVARCHAR (MAX)  NULL,
    [DesiredQualifications]                NVARCHAR (MAX)  NULL,
    [DesiredQualificationsComment]         NVARCHAR (MAX)  NULL,
    [FirstQuestion]                        NVARCHAR (MAX)  NULL,
    [FirstQuestionComment]                 NVARCHAR (MAX)  NULL,
    [SecondQuestion]                       NVARCHAR (MAX)  NULL,
    [SecondQuestionComment]                NVARCHAR (MAX)  NULL,
    [QAUserName]                           NVARCHAR (MAX)  NULL,
    [DateQAApproved]                       DATETIME2 (7)   NULL,
    PRIMARY KEY CLUSTERED ([VacancyId] ASC),
    CONSTRAINT [CK_FrameworkId_StandardId] CHECK ([TrainingTypeCode]='F' AND [FrameworkId] IS NOT NULL AND [StandardId] IS NULL OR [TrainingTypeCode]='S' AND [FrameworkId] IS NULL AND [StandardId] IS NOT NULL),
    CONSTRAINT [CK_TrainingTypeCode] CHECK ([TrainingTypeCode]='S' OR [TrainingTypeCode]='F'),
    CONSTRAINT [FK_Vacancy_ContractedProviderVacancyPartyId] FOREIGN KEY ([ContractOwnerVacancyPartyId]) REFERENCES [Vacancy].[VacancyParty] ([VacancyPartyId]),
    CONSTRAINT [FK_Vacancy_DeliveryProviderVacancyPartyId] FOREIGN KEY ([DeliveryProviderVacancyPartyId]) REFERENCES [Vacancy].[VacancyParty] ([VacancyPartyId]),
    CONSTRAINT [FK_Vacancy_DurationTypeCode] FOREIGN KEY ([DurationTypeCode]) REFERENCES [Vacancy].[DurationType] ([DurationTypeCode]),
    CONSTRAINT [FK_Vacancy_EmployerPartyId] FOREIGN KEY ([EmployerVacancyPartyId]) REFERENCES [Vacancy].[VacancyParty] ([VacancyPartyId]),
    CONSTRAINT [FK_Vacancy_FrameworkId] FOREIGN KEY ([FrameworkId]) REFERENCES [Reference].[Framework] ([FrameworkId]),
    CONSTRAINT [FK_Vacancy_LevelCode] FOREIGN KEY ([LevelCode]) REFERENCES [Reference].[Level] ([LevelCode]),
    CONSTRAINT [FK_Vacancy_ManagerVacancyPartyId] FOREIGN KEY ([ManagerVacancyPartyId]) REFERENCES [Vacancy].[VacancyParty] ([VacancyPartyId]),
    CONSTRAINT [FK_Vacancy_OriginalContractedProviderVacancyPartyId] FOREIGN KEY ([OriginalContractOwnerVacancyPartyId]) REFERENCES [Vacancy].[VacancyParty] ([VacancyPartyId]),
    CONSTRAINT [FK_Vacancy_OwnerVacancyPartyId] FOREIGN KEY ([OwnerVacancyPartyId]) REFERENCES [Vacancy].[VacancyParty] ([VacancyPartyId]),
    CONSTRAINT [FK_Vacancy_ParentVacancyId] FOREIGN KEY ([ParentVacancyId]) REFERENCES [Vacancy].[Vacancy] ([VacancyId]),
    CONSTRAINT [FK_Vacancy_StandardId] FOREIGN KEY ([StandardId]) REFERENCES [Reference].[Standard] ([StandardId]),
    CONSTRAINT [FK_Vacancy_TrainingTypeCode] FOREIGN KEY ([TrainingTypeCode]) REFERENCES [Vacancy].[TrainingType] ([TrainingTypeCode]),
    CONSTRAINT [FK_Vacancy_VacancyLocationTypeCode] FOREIGN KEY ([VacancyLocationTypeCode]) REFERENCES [Vacancy].[VacancyLocationType] ([VacancyLocationTypeCode]),
    CONSTRAINT [FK_Vacancy_VacancyStatusCode] FOREIGN KEY ([VacancyStatusCode]) REFERENCES [Vacancy].[VacancyStatus] ([VacancyStatusCode]),
    CONSTRAINT [FK_Vacancy_VacancyTypeCode] FOREIGN KEY ([VacancyTypeCode]) REFERENCES [Vacancy].[VacancyType] ([VacancyTypeCode]),
    CONSTRAINT [FK_Vacancy_WageTypeCode] FOREIGN KEY ([WageTypeCode]) REFERENCES [Vacancy].[WageType] ([WageTypeCode])
);



GO

CREATE UNIQUE INDEX [IX_Vacancy_VacancyReferenceNumber] ON [Vacancy].[Vacancy] ([VacancyReferenceNumber])

GO
GRANT SELECT
    ON OBJECT::[Vacancy].[Vacancy] TO [migrate]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[Vacancy].[Vacancy] TO [migrate]
    AS [dbo];

