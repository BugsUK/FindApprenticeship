CREATE TABLE [Reference].[LocalAuthority]
(
	[LocalAuthorityId] INT NOT NULL , 
    [CodeName] CHAR(4) NOT NULL, 
    [ShortName] NVARCHAR(MAX) NOT NULL, 
    [FullName] NVARCHAR(MAX) NULL, 
	[CountyId] INT NOT NULL, 
    CONSTRAINT [PK_Reference_LocalAuthority] PRIMARY KEY ([LocalAuthorityId]),
    CONSTRAINT [FK_Reference_County_CountyId] FOREIGN KEY ([CountyId]) REFERENCES [Reference].[County]([CountyId]) 
)
