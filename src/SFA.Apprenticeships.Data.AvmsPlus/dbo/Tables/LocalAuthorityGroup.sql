CREATE TABLE [dbo].[LocalAuthorityGroup] (
    [LocalAuthorityGroupID]        INT            NOT NULL,
    [CodeName]                     NVARCHAR (3)   NOT NULL,
    [ShortName]                    NVARCHAR (50)  NOT NULL,
    [FullName]                     NVARCHAR (100) NOT NULL,
    [LocalAuthorityGroupTypeID]    INT            NOT NULL,
    [LocalAuthorityGroupPurposeID] INT            NOT NULL,
    [ParentLocalAuthorityGroupID]  INT            NULL,
    CONSTRAINT [PK_LocalAuthorityGroup] PRIMARY KEY CLUSTERED ([LocalAuthorityGroupID] ASC),
    CONSTRAINT [FK_LocalAuthorityGroup_LocalAuthorityGroupPurpose] FOREIGN KEY ([LocalAuthorityGroupPurposeID]) REFERENCES [dbo].[LocalAuthorityGroupPurpose] ([LocalAuthorityGroupPurposeID]),
    CONSTRAINT [FK_LocalAuthorityGroup_LocalAuthorityGroupType] FOREIGN KEY ([LocalAuthorityGroupTypeID]) REFERENCES [dbo].[LocalAuthorityGroupType] ([LocalAuthorityGroupTypeID]),
    CONSTRAINT [FK_LocalAuthorityGroup_ParentLocalAuthorityGroup] FOREIGN KEY ([ParentLocalAuthorityGroupID]) REFERENCES [dbo].[LocalAuthorityGroup] ([LocalAuthorityGroupID]),
    CONSTRAINT [UC_LocalAuthorityGroup_CodeName] UNIQUE NONCLUSTERED ([CodeName] ASC, [LocalAuthorityGroupTypeID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idx_LocalAuthorityGroup_LocalAuthorityGroupTypeID]
    ON [dbo].[LocalAuthorityGroup]([LocalAuthorityGroupTypeID] ASC)
    INCLUDE([ParentLocalAuthorityGroupID]) WITH (FILLFACTOR = 90)
    ON [Index];

