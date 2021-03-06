﻿CREATE TABLE [dbo].[ApplicationNextAction] (
    [ApplicationNextActionId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                NVARCHAR (3)   NOT NULL,
    [ShortName]               NVARCHAR (10)  NOT NULL,
    [FullName]                NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ApplicationNextAction] PRIMARY KEY CLUSTERED ([ApplicationNextActionId] ASC),
    CONSTRAINT [uq_idx_ApplicationNextAction] UNIQUE NONCLUSTERED ([FullName] ASC)
);

