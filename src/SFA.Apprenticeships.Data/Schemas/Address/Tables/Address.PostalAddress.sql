CREATE TABLE [Address].[PostalAddress]
(
	[PostalAddressId] INT NOT NULL IDENTITY , 
    [AddressLine1] NVARCHAR(MAX) NOT NULL, 
    [AddressLine2] NVARCHAR(MAX) NULL, 
    [AddressLine3] NVARCHAR(MAX) NULL, 
    [AddressLine4] NVARCHAR(MAX) NULL, 
    [AddressLine5] NVARCHAR(MAX) NULL, 
    [PostTown] NVARCHAR(MAX) NULL, 
    [Postcode] NVARCHAR(MAX) NOT NULL, 
    [ValidationSourceCode] CHAR(3) NULL, 
    [ValidationSourceKeyValue] NVARCHAR(MAX) NULL, 
	[DateValidated] DATETIME2 NULL, 
    [Easting] INT NULL, 
    [Northing] INT NULL, 
    [Longitude] DECIMAL(13, 10) NULL, 
    [Latitude] DECIMAL(13, 10) NULL, 
    [CountyId] INT NULL, 
    CONSTRAINT [PK_PostalAddress] PRIMARY KEY ([PostalAddressId]),
    CONSTRAINT [FK_PostalAddress_PostalAddressSourceType] FOREIGN KEY ([ValidationSourceCode]) REFERENCES [Address].[ValidationSource]([ValidationSourceCode]),
    CONSTRAINT [FK_PostalAddress_CountyId] FOREIGN KEY ([CountyId]) REFERENCES [Reference].[County]([CountyId])
)
