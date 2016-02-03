CREATE TABLE [dbo].[PostcodeOutcode] (
    [PostcodeOutcodeId] INT       IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Outcode]           NCHAR (4) NOT NULL,
    CONSTRAINT [PK_PostcodeOutcode] PRIMARY KEY CLUSTERED ([PostcodeOutcodeId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [uq_idx_PostcodeOutcode] UNIQUE NONCLUSTERED ([Outcode] ASC) WITH (FILLFACTOR = 90) ON [Index]
);

