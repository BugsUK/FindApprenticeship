CREATE TABLE [dbo].[CandidateDisability] (
    [CandidateDisabilityId] INT            IDENTITY (0, 1) NOT FOR REPLICATION NOT NULL,
    [Codename]              NVARCHAR (3)   NOT NULL,
    [ShortName]             NVARCHAR (50)  NOT NULL,
    [FullName]              NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_CandidateDisability] PRIMARY KEY CLUSTERED ([CandidateDisabilityId] ASC),
    CONSTRAINT [uq_idx_CandidateDisability] UNIQUE NONCLUSTERED ([FullName] ASC)
);

