CREATE TABLE [Address].[ValidationSource]
(
	[ValidationSourceCode] CHAR(3) NOT NULL , 
    [FullName] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [PK_ValidationSource] PRIMARY KEY ([ValidationSourceCode])
)
