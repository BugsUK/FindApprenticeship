CREATE TABLE [dbo].[PostcodeOutcode] (
    [PostcodeOutcodeId] INT       IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Outcode]           NCHAR (4) NOT NULL,
    CONSTRAINT [PK_PostcodeOutcode] PRIMARY KEY CLUSTERED ([PostcodeOutcodeId] ASC),
    CONSTRAINT [uq_idx_PostcodeOutcode] UNIQUE NONCLUSTERED ([Outcode] ASC)
);

