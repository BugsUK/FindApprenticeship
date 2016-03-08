CREATE TABLE [dbo].[LocalAuthorityGroupPurpose] (
    [LocalAuthorityGroupPurposeID]   INT           NOT NULL,
    [LocalAuthorityGroupPurposeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_LocalAuthorityGroupPurpose] PRIMARY KEY CLUSTERED ([LocalAuthorityGroupPurposeID] ASC)
);

