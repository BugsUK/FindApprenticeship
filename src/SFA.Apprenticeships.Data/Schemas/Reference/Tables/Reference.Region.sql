CREATE TABLE [Reference].[Region]
(
	[RegionId] INT NOT NULL , 
    [CodeName] CHAR(3) NOT NULL, 
    [ShortName] NVARCHAR(MAX) NOT NULL, 
    [FullName] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_Reference_Region] PRIMARY KEY ([RegionId])
)
