CREATE TABLE [dbo].[RoleType] (
    [RoleTypeId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]   NVARCHAR (3)   COLLATE Latin1_General_CI_AS NOT NULL,
    [ShortName]  NVARCHAR (100) COLLATE Latin1_General_CI_AS NOT NULL,
    [FullName]   NVARCHAR (200) COLLATE Latin1_General_CI_AS NOT NULL,
    CONSTRAINT [PK__RoleType__1CFC3D38] PRIMARY KEY CLUSTERED ([RoleTypeId] ASC)
);



