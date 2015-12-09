CREATE TABLE [Reference].[County]
(
	[CountyId] INT NOT NULL , 
    [CodeName] CHAR(3) NOT NULL, 
    [ShortName] NVARCHAR(MAX) NOT NULL, 
    [FullName] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_Reference_County] PRIMARY KEY ([CountyId])
)
