﻿CREATE TABLE [dbo].[County] (
    [CountyId]  INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]  NVARCHAR (3)   NOT NULL,
    [ShortName] NVARCHAR (50)  NOT NULL,
    [FullName]  NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_County] PRIMARY KEY CLUSTERED ([CountyId] ASC),
    CONSTRAINT [uq_idx_County] UNIQUE NONCLUSTERED ([FullName] ASC)
);

