CREATE TABLE [dbo].[Application] (
    [ApplicationId]               INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CandidateId]                 INT            NOT NULL,
    [VacancyId]                   INT            NOT NULL,
    [ApplicationStatusTypeId]     INT            NOT NULL,
    [WithdrawnOrDeclinedReasonId] INT            CONSTRAINT [DF_Application_WithdrawnOrDeclinedReasonId] DEFAULT ((0)) NOT NULL,
    [UnsuccessfulReasonId]        INT            CONSTRAINT [DF_Application_UnsuccessfulReasonId] DEFAULT ((0)) NOT NULL,
    [OutcomeReasonOther]          NVARCHAR (100) NULL,
    [NextActionId]                INT            CONSTRAINT [DF_Application_NextActionId] DEFAULT ((0)) NOT NULL,
    [NextActionOther]             NVARCHAR (100) NULL,
    [AllocatedTo]                 NVARCHAR (200) NULL,
    [CVAttachmentId]              INT            NULL,
    [BeingSupportedBy]            NVARCHAR (50)  NULL,
    [LockedForSupportUntil]       DATETIME       NULL,
    [WithdrawalAcknowledged]      BIT            CONSTRAINT [DF_Application_WithdrawalAcknowledged] DEFAULT ((1)) NULL,
	-- NEW FIELDS
	[ApplicationGuid]             UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Application_1] PRIMARY KEY CLUSTERED ([ApplicationId] ASC),
    CONSTRAINT [FK_Application_ApplicationNextAction] FOREIGN KEY ([NextActionId]) REFERENCES [dbo].[ApplicationNextAction] ([ApplicationNextActionId]),
    CONSTRAINT [FK_Application_ApplicationStatusType] FOREIGN KEY ([ApplicationStatusTypeId]) REFERENCES [dbo].[ApplicationStatusType] ([ApplicationStatusTypeId]),
    CONSTRAINT [FK_Application_ApplicationUnsuccessfulReasonType] FOREIGN KEY ([UnsuccessfulReasonId]) REFERENCES [dbo].[ApplicationUnsuccessfulReasonType] ([ApplicationUnsuccessfulReasonTypeId]),
    CONSTRAINT [FK_Application_ApplicationWithdrawnOrDeclinedReasonType] FOREIGN KEY ([WithdrawnOrDeclinedReasonId]) REFERENCES [dbo].[ApplicationWithdrawnOrDeclinedReasonType] ([ApplicationWithdrawnOrDeclinedReasonTypeId]),
    CONSTRAINT [FK_Application_AttachedDocument] FOREIGN KEY ([CVAttachmentId]) REFERENCES [dbo].[AttachedDocument] ([AttachedDocumentId]),
    --TODO: Reinstate
	--CONSTRAINT [FK_Application_Candidate] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Candidate] ([CandidateId]),
    CONSTRAINT [FK_Application_Vacancy1] FOREIGN KEY ([VacancyId]) REFERENCES [dbo].[Vacancy] ([VacancyId]),
    CONSTRAINT [uq_idx_application] UNIQUE NONCLUSTERED ([CandidateId] ASC, [VacancyId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idx_Application_ApplicationStatusTypeId]
    ON [dbo].[Application]([ApplicationStatusTypeId] ASC)
    INCLUDE([VacancyId]);


GO
CREATE NONCLUSTERED INDEX [idx_Application_VacancyID]
    ON [dbo].[Application]([VacancyId] ASC)
    INCLUDE([ApplicationStatusTypeId], [CandidateId], [WithdrawalAcknowledged], [ApplicationId]);

GO
CREATE UNIQUE NONCLUSTERED INDEX [idx_Application_ApplicationGuid]
	ON [dbo].[Application]([ApplicationGuid])