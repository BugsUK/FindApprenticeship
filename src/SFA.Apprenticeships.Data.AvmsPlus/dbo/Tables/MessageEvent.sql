CREATE TABLE [dbo].[MessageEvent] (
    [MessageEventId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]       NVARCHAR (3)   NOT NULL,
    [ShortName]      NVARCHAR (100) NOT NULL,
    [FullName]       NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_MessageEvent] PRIMARY KEY CLUSTERED ([MessageEventId] ASC),
    CONSTRAINT [uq_idx_MessageEvent] UNIQUE NONCLUSTERED ([FullName] ASC)
);

