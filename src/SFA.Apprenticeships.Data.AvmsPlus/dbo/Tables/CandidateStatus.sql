CREATE TABLE [dbo].[CandidateStatus] (
    [CandidateStatusId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]          NVARCHAR (3)   NOT NULL,
    [ShortName]         NVARCHAR (100) NOT NULL,
    [FullName]          NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_CandidateStatus] PRIMARY KEY CLUSTERED ([CandidateStatusId] ASC),
    CONSTRAINT [uq_idx_CandidateStatus] UNIQUE NONCLUSTERED ([FullName] ASC)
);

