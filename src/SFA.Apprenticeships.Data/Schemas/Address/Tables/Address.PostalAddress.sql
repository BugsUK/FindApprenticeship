CREATE TABLE [Address].[PostalAddress]
(
	[PostalAddressId] INT NOT NULL PRIMARY KEY, 
    [AddressLine1] NVARCHAR(MAX) NOT NULL, 
    [AddressLine2] NVARCHAR(MAX) NULL, 
    [AddressLine3] NVARCHAR(MAX) NULL, 
    [AddressLine4] NVARCHAR(MAX) NULL, 
    [AddressLine5] NVARCHAR(MAX) NULL, 
    [PostTown] NVARCHAR(MAX) NULL, 
    [Postcode] NVARCHAR(MAX) NOT NULL, 
    [PostalAddressSourceTypeId] INT NULL, 
    [PostalAddressSourceKey] NVARCHAR(MAX) NULL, 
    [Easting] INT NULL, 
    [Northing] INT NULL, 
    [Longitude] DECIMAL(13, 10) NULL, 
    [Latitude] DECIMAL(13, 10) NULL, 
    [CountyId] INT NULL, 
    CONSTRAINT [FK_PostalAddress_PostalAddressSourceType] FOREIGN KEY ([PostalAddressSourceTypeId]) REFERENCES [Address].[PostalAddressSourceType]([PostalAddressSourceTypeId])
)
