CREATE TABLE [dbo].[TrainingType] (
    [TrainingTypeId]		  INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                NVARCHAR (3)  NOT NULL,
    [ShortName]               NVARCHAR (10) NOT NULL,
    [FullName]                NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_TrainingType] PRIMARY KEY CLUSTERED ([TrainingTypeId] ASC),
    CONSTRAINT [uq_idx_TrainingType] UNIQUE NONCLUSTERED ([FullName] ASC)
);

