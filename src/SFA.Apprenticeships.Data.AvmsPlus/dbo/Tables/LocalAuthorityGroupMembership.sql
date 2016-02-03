CREATE TABLE [dbo].[LocalAuthorityGroupMembership] (
    [LocalAuthorityID]      INT NOT NULL,
    [LocalAuthorityGroupID] INT NOT NULL,
    CONSTRAINT [PK_LocalAuthorityGroupMembership] PRIMARY KEY CLUSTERED ([LocalAuthorityID] ASC, [LocalAuthorityGroupID] ASC),
    CONSTRAINT [FK_LAGroupMembership_LAGroup] FOREIGN KEY ([LocalAuthorityGroupID]) REFERENCES [dbo].[LocalAuthorityGroup] ([LocalAuthorityGroupID]),
    CONSTRAINT [FK_LAGroupMembership_LocalAuthority] FOREIGN KEY ([LocalAuthorityID]) REFERENCES [dbo].[LocalAuthority] ([LocalAuthorityId])
);

