CREATE TABLE [dbo].[NASSupportContact] (
    [NASSupportContactId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [ManagingAreaID]      INT            NOT NULL,
    [EmailAddress]        NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_NASSupportContact] PRIMARY KEY CLUSTERED ([NASSupportContactId] ASC),
    CONSTRAINT [uq_idx_NASSupportContact] UNIQUE NONCLUSTERED ([ManagingAreaID] ASC)
);

