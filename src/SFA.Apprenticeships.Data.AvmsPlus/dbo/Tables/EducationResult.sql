CREATE TABLE [dbo].[EducationResult] (
    [EducationResultId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CandidateId]       INT            NOT NULL,
    [Subject]           NVARCHAR (50)  COLLATE Latin1_General_CI_AS NOT NULL,
    [Level]             INT            NOT NULL,
    [LevelOther]        NVARCHAR (100) COLLATE Latin1_General_CI_AS NULL,
    [Grade]             NVARCHAR (20)  COLLATE Latin1_General_CI_AS NULL,
    [DateAchieved]      DATETIME       NULL,
    [ApplicationId]     INT            NULL,
    CONSTRAINT [PK_EducationResult] PRIMARY KEY CLUSTERED ([EducationResultId] ASC),
    CONSTRAINT [FK_EducationResult_Candidate] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Candidate] ([CandidateId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_EducationResult_EducationResultLevel1] FOREIGN KEY ([Level]) REFERENCES [dbo].[EducationResultLevel] ([EducationResultLevelId]) NOT FOR REPLICATION
);




GO
CREATE NONCLUSTERED INDEX [idx_EducationResult_CandidateId_ApplicationId]
    ON [dbo].[EducationResult]([CandidateId] ASC, [ApplicationId] ASC) WITH (FILLFACTOR = 90)
    ON [Index];

