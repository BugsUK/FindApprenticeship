CREATE TABLE [dbo].[TermsAndConditions] (
    [TermsAndConditionsId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [UserTypeId]           INT            NOT NULL,
    [Fullname]             NVARCHAR (200) NULL,
    [Content]              NTEXT          NULL,
    CONSTRAINT [PK_TermsAndConditions] PRIMARY KEY CLUSTERED ([TermsAndConditionsId] ASC),
    CONSTRAINT [FK_TermsAndConditions_UserType] FOREIGN KEY ([UserTypeId]) REFERENCES [dbo].[UserType] ([UserTypeId])
);

