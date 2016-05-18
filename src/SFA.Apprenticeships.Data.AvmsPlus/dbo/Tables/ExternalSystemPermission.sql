CREATE TABLE [dbo].[ExternalSystemPermission] (
    [Username]         UNIQUEIDENTIFIER NOT NULL,
    [Password]         BINARY (64)      NOT NULL,
    [UserParameters]   VARCHAR (30)     NULL,
    [Businesscategory] VARCHAR (30)     NULL,
    [Company]          INT              NULL,
    [Employeetype]     VARCHAR (50)     NULL,
    [Salt]             UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_User_name] PRIMARY KEY CLUSTERED ([Username] ASC)
);

