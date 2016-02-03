CREATE TABLE [dbo].[ContactPreferenceType] (
    [ContactPreferenceTypeId] INT            IDENTITY (0, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                NVARCHAR (3)   NOT NULL,
    [ShortName]               NVARCHAR (100) NOT NULL,
    [FullName]                NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_ContactPreferenceType] PRIMARY KEY CLUSTERED ([ContactPreferenceTypeId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
    CONSTRAINT [uq_idx_ContactPreferenceType] UNIQUE NONCLUSTERED ([FullName] ASC) WITH (FILLFACTOR = 90) ON [Index]
);

