﻿CREATE TABLE [dbo].[UserType] (
    [UserTypeId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]   NVARCHAR (3)   NOT NULL,
    [ShortName]  NVARCHAR (100) NOT NULL,
    [FullName]   NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_OwnerType] PRIMARY KEY CLUSTERED ([UserTypeId] ASC),
    CONSTRAINT [uq_idx_UserType] UNIQUE NONCLUSTERED ([FullName] ASC)
);

