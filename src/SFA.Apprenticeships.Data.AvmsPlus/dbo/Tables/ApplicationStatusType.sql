CREATE TABLE [dbo].[ApplicationStatusType] (
    [ApplicationStatusTypeId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                NVARCHAR (3)   NOT NULL,
    [ShortName]               NVARCHAR (100) NOT NULL,
    [FullName]                NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_ApplicationStatusType] PRIMARY KEY CLUSTERED ([ApplicationStatusTypeId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [uq_idx_ApplicationStatusType] UNIQUE NONCLUSTERED ([FullName] ASC) WITH (FILLFACTOR = 90) ON [Index]
);

