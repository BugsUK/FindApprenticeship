CREATE TABLE [dbo].[RoleTypePrivileges] (
    [RoleTypeId]      INT NOT NULL,
    [PrivilegeTypeId] INT NOT NULL,
    FOREIGN KEY ([PrivilegeTypeId]) REFERENCES [dbo].[PrivilegeType] ([PrivilegeTypeId]),
    FOREIGN KEY ([RoleTypeId]) REFERENCES [dbo].[RoleType] ([RoleTypeId]),
    CONSTRAINT [FK_RoleTypePrivilege_RoleType] FOREIGN KEY ([RoleTypeId]) REFERENCES [dbo].[RoleType] ([RoleTypeId]),
    CONSTRAINT [FK_RoleTypePrivileges_PrivilegeType] FOREIGN KEY ([PrivilegeTypeId]) REFERENCES [dbo].[PrivilegeType] ([PrivilegeTypeId])
);

