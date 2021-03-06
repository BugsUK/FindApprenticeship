﻿CREATE TABLE [dbo].[CandidateEthnicOrigin] (
    [CandidateEthnicOriginId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                NVARCHAR (3)   NOT NULL,
    [ShortName]               NVARCHAR (100) NOT NULL,
    [FullName]                NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_CandidateEthnicOrigin] PRIMARY KEY CLUSTERED ([CandidateEthnicOriginId] ASC)
);

