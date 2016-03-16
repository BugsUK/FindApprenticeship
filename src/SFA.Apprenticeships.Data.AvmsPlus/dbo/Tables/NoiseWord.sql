CREATE TABLE [dbo].[NoiseWord] (
    [NoiseWordId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [NoiseWord]   NVARCHAR (100) NULL,
    CONSTRAINT [PK_NoiseWord] PRIMARY KEY CLUSTERED ([NoiseWordId] ASC)
);

