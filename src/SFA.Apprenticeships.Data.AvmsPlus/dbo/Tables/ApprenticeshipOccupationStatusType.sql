CREATE TABLE [dbo].[ApprenticeshipOccupationStatusType] (
    [ApprenticeshipOccupationStatusTypeId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                             NVARCHAR (100) NOT NULL,
    [ShortName]                            NVARCHAR (100) NOT NULL,
    [FullName]                             NVARCHAR (200) NOT NULL,
    CONSTRAINT [pk_ApprenticeshipOccupationStatusType] PRIMARY KEY CLUSTERED ([ApprenticeshipOccupationStatusTypeId] ASC)
);

