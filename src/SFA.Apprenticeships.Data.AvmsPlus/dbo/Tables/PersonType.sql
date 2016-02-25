CREATE TABLE [dbo].[PersonType] (
    [PersonTypeId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]     NVARCHAR (3)   NOT NULL,
    [ShortName]    NVARCHAR (100) NOT NULL,
    [FullName]     NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_PersonType] PRIMARY KEY CLUSTERED ([PersonTypeId] ASC),
    CONSTRAINT [uq_idx_PersonType] UNIQUE NONCLUSTERED ([FullName] ASC)
);

