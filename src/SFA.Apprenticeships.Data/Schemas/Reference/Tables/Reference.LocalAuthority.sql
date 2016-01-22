CREATE TABLE [Reference].[LocalAuthority]
(
	[LocalAuthorityId] INT NOT NULL , 
    [CodeName] CHAR(4) NOT NULL, 
    [ShortName] NVARCHAR(MAX) NOT NULL, 
    [FullName] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_Reference_LocalAuthority] PRIMARY KEY ([LocalAuthorityId])
)
