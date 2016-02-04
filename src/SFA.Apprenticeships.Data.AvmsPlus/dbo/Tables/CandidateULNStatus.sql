CREATE TABLE [dbo].[CandidateULNStatus] (
    [CandidateULNStatusId] INT            IDENTITY (0, 1) NOT FOR REPLICATION NOT NULL,
    [Codename]             NVARCHAR (3)   NOT NULL,
    [Shortname]            NVARCHAR (10)  NOT NULL,
    [Fullname]             NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_CandidateULNStatus] PRIMARY KEY CLUSTERED ([CandidateULNStatusId] ASC),
    CONSTRAINT [uq_idx_CandidateULNStatus] UNIQUE NONCLUSTERED ([Fullname] ASC)
);

