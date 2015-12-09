CREATE TABLE [Reference].[FrameworkStatus]
(
	[FrameworkStatusId] INT NOT NULL, 
    [CodeName] NCHAR(3) NOT NULL, 
    [ShortName] NVARCHAR(MAX) NOT NULL, 
    [FullName] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [PK_Reference_FrameworkStatus] PRIMARY KEY ([FrameworkStatusId]) 
)
