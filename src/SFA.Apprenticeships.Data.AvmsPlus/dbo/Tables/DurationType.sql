CREATE TABLE [dbo].[DurationType]
(
	[DurationTypeId] INT IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [FullName] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_DurationType] PRIMARY KEY ([DurationTypeId])
)
