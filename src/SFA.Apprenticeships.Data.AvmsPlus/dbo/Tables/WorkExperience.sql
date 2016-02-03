CREATE TABLE [dbo].[WorkExperience] (
    [WorkExperienceId]    INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CandidateId]         INT            NOT NULL,
    [Employer]            NVARCHAR (50)  COLLATE Latin1_General_CI_AS NOT NULL,
    [FromDate]            DATETIME       NULL,
    [ToDate]              DATETIME       NULL,
    [TypeOfWork]          NVARCHAR (200) COLLATE Latin1_General_CI_AS NULL,
    [PartialCompletion]   BIT            CONSTRAINT [DF_WorkExperience_PartialCompletion] DEFAULT ((0)) NOT NULL,
    [VoluntaryExperience] BIT            CONSTRAINT [DF_WorkExperience_VoluntaryExperience] DEFAULT ((0)) NOT NULL,
    [ApplicationId]       INT            NULL,
    CONSTRAINT [PK_WorkExperience] PRIMARY KEY CLUSTERED ([WorkExperienceId] ASC),
    CONSTRAINT [FK_WorkExperience_Candidate] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Candidate] ([CandidateId]) NOT FOR REPLICATION
);




GO
CREATE NONCLUSTERED INDEX [idx_WorkExperience_CandidateId_ApplicationId]
    ON [dbo].[WorkExperience]([CandidateId] ASC, [ApplicationId] ASC) WITH (FILLFACTOR = 90)
    ON [Index];

