CREATE TABLE [Reference].[County]
(
	[CountyId] INT NOT NULL PRIMARY KEY, 
    [CountyCode] NVARCHAR(3) NULL, 
    [ShortName] NVARCHAR(MAX) NULL, 
    [FullName] NVARCHAR(MAX) NULL
)
