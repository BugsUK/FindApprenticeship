CREATE TABLE [dbo].[School] (
    [SchoolId]            INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [URN]                 NVARCHAR (100)  COLLATE Latin1_General_CI_AS NOT NULL,
    [SchoolName]          NVARCHAR (120)  COLLATE Latin1_General_CI_AS NOT NULL,
    [Address]             NVARCHAR (2000) COLLATE Latin1_General_CI_AS NOT NULL,
    [Address1]            NVARCHAR (100)  COLLATE Latin1_General_CI_AS NULL,
    [Address2]            NVARCHAR (100)  COLLATE Latin1_General_CI_AS NULL,
    [Area]                NVARCHAR (100)  COLLATE Latin1_General_CI_AS NULL,
    [Town]                NVARCHAR (100)  COLLATE Latin1_General_CI_AS NULL,
    [County]              NVARCHAR (100)  COLLATE Latin1_General_CI_AS NULL,
    [Postcode]            NVARCHAR (10)   COLLATE Latin1_General_CI_AS NULL,
    [SchoolNameForSearch] NVARCHAR (120)  COLLATE Latin1_General_CI_AS NULL,
    CONSTRAINT [PK_School] PRIMARY KEY CLUSTERED ([SchoolId] ASC)
);




GO
CREATE NONCLUSTERED INDEX [idx_School_URN]
    ON [dbo].[School]([URN] ASC)
    ON [Index];

