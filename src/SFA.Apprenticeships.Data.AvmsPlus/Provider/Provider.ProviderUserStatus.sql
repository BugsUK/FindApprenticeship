CREATE TABLE [Provider].[ProviderUserStatus]
(
	[ProviderUserStatusId] INT NOT NULL PRIMARY KEY, 
    [CodeName] NVARCHAR(3) NOT NULL, 
    [ShortName] NVARCHAR(MAX) NOT NULL, 
    [FullName] NVARCHAR(MAX) NOT NULL
)
