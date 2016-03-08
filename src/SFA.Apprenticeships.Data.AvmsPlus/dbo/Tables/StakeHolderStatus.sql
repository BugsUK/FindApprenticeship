CREATE TABLE [dbo].[StakeHolderStatus] (
    [StakeHolderStatusId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]            NVARCHAR (3)   NOT NULL,
    [ShortName]           NVARCHAR (100) NOT NULL,
    [FullName]            NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_StakeHolderStatus] PRIMARY KEY CLUSTERED ([StakeHolderStatusId] ASC),
    CONSTRAINT [uq_idx_StakeHolderStatus] UNIQUE NONCLUSTERED ([FullName] ASC)
);

