CREATE TABLE [dbo].[LocalAuthority] (
    [LocalAuthorityId] INT            IDENTITY (0, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]         NCHAR (4)      COLLATE Latin1_General_CI_AS NOT NULL,
    [ShortName]        NVARCHAR (50)  COLLATE Latin1_General_CI_AS NOT NULL,
    [FullName]         NVARCHAR (100) COLLATE Latin1_General_CI_AS NOT NULL,
    [CountyId]         INT            NOT NULL,
    CONSTRAINT [PK_LocalAuthority] PRIMARY KEY CLUSTERED ([LocalAuthorityId] ASC),
    CONSTRAINT [FK_LocalAuthority_County] FOREIGN KEY ([CountyId]) REFERENCES [dbo].[County] ([CountyId]) NOT FOR REPLICATION,
    CONSTRAINT [uq_idx_LocalAuthority_code] UNIQUE NONCLUSTERED ([CodeName] ASC),
    CONSTRAINT [uq_idx_LocalAuthority_name] UNIQUE NONCLUSTERED ([FullName] ASC)
);



