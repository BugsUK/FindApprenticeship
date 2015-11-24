CREATE TABLE [Organisation].[OrganisationSite]
(
	[OrganisationSiteId] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(MAX) NOT NULL, 
    [PostalAddressId] INT NOT NULL, 
    [WebSiteUrl] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_OrganisationSite_PostalAddress] FOREIGN KEY ([PostalAddressId]) REFERENCES [Address].[PostalAddress]([PostalAddressId])
)
