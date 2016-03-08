CREATE TABLE [dbo].[AttachedtoItemType] (
    [AttachedtoItemTypeId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]             NVARCHAR (3)   NOT NULL,
    [ShortName]            NVARCHAR (100) NOT NULL,
    [FullName]             NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_AttachedtoItemType] PRIMARY KEY CLUSTERED ([AttachedtoItemTypeId] ASC),
    CONSTRAINT [uq_idx_AttachedToItemType] UNIQUE NONCLUSTERED ([FullName] ASC)
);

