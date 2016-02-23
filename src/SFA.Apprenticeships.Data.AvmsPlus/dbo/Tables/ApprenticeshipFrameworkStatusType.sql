CREATE TABLE [dbo].[ApprenticeshipFrameworkStatusType] (
    [ApprenticeshipFrameworkStatusTypeId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                            NVARCHAR (3)   NOT NULL,
    [ShortName]                           NVARCHAR (100) NOT NULL,
    [FullName]                            NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_ApprenticeshipFrameworkStatusType] PRIMARY KEY CLUSTERED ([ApprenticeshipFrameworkStatusTypeId] ASC)
);

