CREATE TABLE [dbo].[CandidateGender] (
    [CandidateGenderId] INT            IDENTITY (0, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]          NVARCHAR (3)   NOT NULL,
    [ShortName]         NVARCHAR (100) NOT NULL,
    [FullName]          NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_CandidateGender] PRIMARY KEY CLUSTERED ([CandidateGenderId] ASC),
    CONSTRAINT [uq_idx_CandidateGender] UNIQUE NONCLUSTERED ([FullName] ASC)
);

