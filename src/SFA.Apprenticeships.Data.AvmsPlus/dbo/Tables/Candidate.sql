CREATE TABLE [dbo].[Candidate] (
    [CandidateId]                    INT              IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [PersonId]                       INT              NOT NULL,
    [CandidateStatusTypeId]          INT              NOT NULL,
    [DateofBirth]                    DATETIME         NOT NULL,
    [AddressLine1]                   NVARCHAR (50)    NOT NULL,
    [AddressLine2]                   NVARCHAR (50)    NULL,
    [AddressLine3]                   NVARCHAR (50)    NULL,
    [AddressLine4]                   NVARCHAR (50)    NULL,
    [AddressLine5]                   NVARCHAR (50)    NULL,
    [Town]                           NVARCHAR (50)    NOT NULL,
    [CountyId]                       INT              NOT NULL,
    [Postcode]                       NVARCHAR (8)     NOT NULL,
    [LocalAuthorityId]               INT              NOT NULL,
    [Longitude]                      DECIMAL (13, 10) NULL,
    [Latitude]                       DECIMAL (13, 10) NULL,
    [GeocodeEasting]                 INT              NULL,
    [GeocodeNorthing]                INT              NULL,
    [NiReference]                    NVARCHAR (10)    NULL,
    [VoucherReferenceNumber]         INT              NULL,
    [UniqueLearnerNumber]            BIGINT           NULL,
    [UlnStatusId]                    INT              CONSTRAINT [DF_Candidate_UlnStatusId] DEFAULT ((0)) NOT NULL,
    [Gender]                         INT              CONSTRAINT [DF_Candidate_Gender] DEFAULT ((0)) NOT NULL,
    [EthnicOrigin]                   INT              CONSTRAINT [DF_Candidate_EthnicOrigin] DEFAULT ((0)) NOT NULL,
    [EthnicOriginOther]              NVARCHAR (50)    NULL,
    [ApplicationLimitEnforced]       BIT              CONSTRAINT [DF_Candidate_ApplicationLimitEnforced] DEFAULT ((0)) NOT NULL,
    [LastAccessedDate]               DATETIME         NULL,
    [AdditionalEmail]                NVARCHAR (50)    NULL,
    [Disability]                     INT              CONSTRAINT [DF_Candidate_Disability] DEFAULT ((0)) NOT NULL,
    [DisabilityOther]                NVARCHAR (256)   NULL,
    [HealthProblems]                 NVARCHAR (256)   NULL,
    [ReceivePushedContent]           BIT              CONSTRAINT [DF_Candidate_ReceivePushedContent] DEFAULT ((0)) NOT NULL,
    [ReferralAgent]                  BIT              CONSTRAINT [DF_Candidate_ReferralAgent] DEFAULT ((0)) NOT NULL,
    [DisableAlerts]                  BIT              CONSTRAINT [DF_Candidate_DisableAlerts] DEFAULT ((0)) NOT NULL,
    [UnconfirmedEmailAddress]        NVARCHAR (100)   NULL,
    [MobileNumberUnconfirmed]        BIT              CONSTRAINT [DF_Candidate_MobileNumberUnconfirmed] DEFAULT ((0)) NOT NULL,
    [DoBFailureCount]                SMALLINT         NULL,
    [ForgottenUsernameRequested]     BIT              CONSTRAINT [DF_Candidate_ForgottenUsernameRequested] DEFAULT ((0)) NOT NULL,
    [ForgottenPasswordRequested]     BIT              CONSTRAINT [DF_Candidate_ForgottenPasswordRequested] DEFAULT ((0)) NOT NULL,
    [TextFailureCount]               SMALLINT         CONSTRAINT [DF_Candidate_TextFailureCount] DEFAULT ((0)) NOT NULL,
    [EmailFailureCount]              SMALLINT         CONSTRAINT [DF_Candidate_EmailFailureCount] DEFAULT ((0)) NOT NULL,
    [LastAccessedManageApplications] DATETIME         NULL,
    [ReferralPoints]                 SMALLINT         CONSTRAINT [DF_Candidate_ReferralPoints] DEFAULT ((0)) NOT NULL,
    [BeingSupportedBy]               NVARCHAR (50)    NULL,
    [LockedForSupportUntil]          DATETIME         NULL,
    [NewVacancyAlertEmail]           BIT              NULL,
    [NewVacancyAlertSMS]             BIT              NULL,
    [AllowMarketingMessages]         BIT              NULL,
    [ReminderMessageSent]            BIT              DEFAULT ((0)) NOT NULL,
	-- NEW FIELDS
	[CandidateGuid]             UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Candidate] PRIMARY KEY CLUSTERED ([CandidateId] ASC),
    CONSTRAINT [FK_Candidate_CandidateDisability] FOREIGN KEY ([Disability]) REFERENCES [dbo].[CandidateDisability] ([CandidateDisabilityId]),
    CONSTRAINT [FK_Candidate_CandidateEthnicOrigin] FOREIGN KEY ([EthnicOrigin]) REFERENCES [dbo].[CandidateEthnicOrigin] ([CandidateEthnicOriginId]),
    CONSTRAINT [FK_Candidate_CandidateGender] FOREIGN KEY ([Gender]) REFERENCES [dbo].[CandidateGender] ([CandidateGenderId]),
    CONSTRAINT [FK_Candidate_CandidateStatus] FOREIGN KEY ([CandidateStatusTypeId]) REFERENCES [dbo].[CandidateStatus] ([CandidateStatusId]),
    CONSTRAINT [FK_Candidate_CandidateULNStatus] FOREIGN KEY ([UlnStatusId]) REFERENCES [dbo].[CandidateULNStatus] ([CandidateULNStatusId]),
    CONSTRAINT [FK_Candidate_County] FOREIGN KEY ([CountyId]) REFERENCES [dbo].[County] ([CountyId]),
    CONSTRAINT [FK_Candidate_LocalAuthority] FOREIGN KEY ([LocalAuthorityId]) REFERENCES [dbo].[LocalAuthority] ([LocalAuthorityId]),
    CONSTRAINT [FK_Candidate_Person] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]),
    CONSTRAINT [uq_idx_candidate_person] UNIQUE NONCLUSTERED ([PersonId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idx_Candidate_CandidateStatusTypeID]
    ON [dbo].[Candidate]([CandidateStatusTypeId] ASC)
    INCLUDE([CandidateId], [Postcode])


GO
CREATE NONCLUSTERED INDEX [idx_Candidate_CandidateGuid] 
	ON [dbo].[Candidate] ([CandidateGuid]) 
	INCLUDE ([CandidateId], [PersonId]) ;