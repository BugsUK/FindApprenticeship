CREATE TABLE [dbo].[RoleType] (
    [RoleTypeId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]   NVARCHAR (3)   NOT NULL,
    [ShortName]  NVARCHAR (100) NOT NULL,
    [FullName]   NVARCHAR (200) NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleTypeId] ASC)
);

