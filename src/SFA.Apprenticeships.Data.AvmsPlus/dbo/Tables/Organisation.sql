CREATE TABLE [dbo].[Organisation] (
    [OrganisationId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]       NCHAR (3)      NOT NULL,
    [ShortName]      NVARCHAR (100) NOT NULL,
    [FullName]       NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_Organisation] PRIMARY KEY CLUSTERED ([OrganisationId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [uq_idx_Organisation] UNIQUE NONCLUSTERED ([FullName] ASC) WITH (FILLFACTOR = 90) ON [Index]
);

