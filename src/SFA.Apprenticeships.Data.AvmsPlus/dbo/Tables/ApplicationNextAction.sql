CREATE TABLE [dbo].[ApplicationNextAction] (
    [ApplicationNextActionId] INT            IDENTITY (0, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                NVARCHAR (3)   NOT NULL,
    [ShortName]               NVARCHAR (10)  NOT NULL,
    [FullName]                NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ApplicationNextAction] PRIMARY KEY CLUSTERED ([ApplicationNextActionId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [uq_idx_ApplicationNextAction] UNIQUE NONCLUSTERED ([FullName] ASC) WITH (FILLFACTOR = 90) ON [Index]
);

