CREATE TABLE [dbo].[AlertType] (
    [AlertTypeId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]    NVARCHAR (3)   NOT NULL,
    [ShortName]   NVARCHAR (50)  NOT NULL,
    [FullName]    NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Alert_Types] PRIMARY KEY CLUSTERED ([AlertTypeId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [uq_idx_alertType] UNIQUE NONCLUSTERED ([FullName] ASC) WITH (FILLFACTOR = 90) ON [Index]
);

