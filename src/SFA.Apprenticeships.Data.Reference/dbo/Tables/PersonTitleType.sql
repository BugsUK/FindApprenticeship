﻿CREATE TABLE [dbo].[PersonTitleType] (
    [PersonTitleTypeId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]          NVARCHAR (3)   NOT NULL,
    [ShortName]         NVARCHAR (100) NOT NULL,
    [FullName]          NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_PersonTitleType] PRIMARY KEY CLUSTERED ([PersonTitleTypeId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY],
);
