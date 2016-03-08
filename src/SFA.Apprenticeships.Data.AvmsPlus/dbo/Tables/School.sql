CREATE TABLE [dbo].[School] (
    [SchoolId]            INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [URN]                 NVARCHAR (100)  NOT NULL,
    [SchoolName]          NVARCHAR (120)  NOT NULL,
    [Address]             NVARCHAR (2000) NOT NULL,
    [Address1]            NVARCHAR (100)  NULL,
    [Address2]            NVARCHAR (100)  NULL,
    [Area]                NVARCHAR (100)  NULL,
    [Town]                NVARCHAR (100)  NULL,
    [County]              NVARCHAR (100)  NULL,
    [Postcode]            NVARCHAR (10)   NULL,
    [SchoolNameForSearch] NVARCHAR (120)  NULL,
    CONSTRAINT [PK_School] PRIMARY KEY CLUSTERED ([SchoolId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idx_School_URN]
    ON [dbo].[School]([URN] ASC);

