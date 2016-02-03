﻿CREATE TABLE [dbo].[SICCode] (
    [SICCodeId]   INT            IDENTITY (0, 1) NOT FOR REPLICATION NOT NULL,
    [Year]        SMALLINT       NOT NULL,
    [SICCode]     INT            NOT NULL,
    [Description] NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_SICCode] PRIMARY KEY CLUSTERED ([SICCodeId] ASC) WITH (FILLFACTOR = 90) ON [PRIMARY]
);

