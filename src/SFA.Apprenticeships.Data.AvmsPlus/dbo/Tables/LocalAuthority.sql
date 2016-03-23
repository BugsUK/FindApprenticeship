CREATE TABLE [dbo].[LocalAuthority] (
    [LocalAuthorityId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]         NCHAR (4)      NOT NULL,
    [ShortName]        NVARCHAR (50)  NOT NULL,
    [FullName]         NVARCHAR (100) NOT NULL,
    [CountyId]         INT            NOT NULL,
    CONSTRAINT [PK_LocalAuthority] PRIMARY KEY CLUSTERED ([LocalAuthorityId] ASC),
    CONSTRAINT [FK_LocalAuthority_County] FOREIGN KEY ([CountyId]) REFERENCES [dbo].[County] ([CountyId]),
    CONSTRAINT [uq_idx_LocalAuthority_code] UNIQUE NONCLUSTERED ([CodeName] ASC),
    CONSTRAINT [uq_idx_LocalAuthority_name] UNIQUE NONCLUSTERED ([FullName] ASC)
);

