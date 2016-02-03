CREATE TABLE [dbo].[CandidateDeRegistrationControl] (
    [CandidateDeRegistrationControlId] INT IDENTITY (1, 1) NOT NULL,
    [CandidateId]                      INT NOT NULL,
    [isDeletedFromAdam]                BIT NULL,
    [isDeleteFromAOL]                  BIT NULL,
    [roleId]                           INT CONSTRAINT [DFT_Candidate_RoleID] DEFAULT ((0)) NOT NULL,
    [isHardDelete]                     BIT NULL,
    CONSTRAINT [PK_CandidateDeRegistrationControl] PRIMARY KEY CLUSTERED ([CandidateDeRegistrationControlId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY]
);

