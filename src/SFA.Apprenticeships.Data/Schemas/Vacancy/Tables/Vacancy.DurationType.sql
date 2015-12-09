CREATE TABLE [Vacancy].[DurationType]
(
	[DurationTypeCode] CHAR(1) NOT NULL,
    [FullName] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_DurationType] PRIMARY KEY ([DurationTypeCode])
)
