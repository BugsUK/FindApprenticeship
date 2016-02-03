CREATE TABLE [dbo].[SchoolAttended] (
    [SchoolAttendedId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CandidateId]      INT            NOT NULL,
    [SchoolId]         INT            NULL,
    [OtherSchoolName]  NVARCHAR (120) COLLATE Latin1_General_CI_AS NULL,
    [OtherSchoolTown]  NVARCHAR (120) COLLATE Latin1_General_CI_AS NULL,
    [StartDate]        DATETIME       NOT NULL,
    [EndDate]          DATETIME       NULL,
    [ApplicationId]    INT            NULL,
    CONSTRAINT [PK_SchoolAttended] PRIMARY KEY CLUSTERED ([SchoolAttendedId] ASC),
    CONSTRAINT [FK_SchoolAttended_Candidate] FOREIGN KEY ([CandidateId]) REFERENCES [dbo].[Candidate] ([CandidateId]) NOT FOR REPLICATION,
    CONSTRAINT [FK_SchoolAttended_School] FOREIGN KEY ([SchoolId]) REFERENCES [dbo].[School] ([SchoolId]) NOT FOR REPLICATION,
    CONSTRAINT [uq_idx_schoolAttended] UNIQUE NONCLUSTERED ([CandidateId] ASC, [ApplicationId] ASC)
);



