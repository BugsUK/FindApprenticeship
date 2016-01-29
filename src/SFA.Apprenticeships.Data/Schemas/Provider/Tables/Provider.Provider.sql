CREATE TABLE [Provider].[Provider]
(
	[ProviderId] INT IDENTITY(1,1), 
    [DateCreated] DATETIME2 NOT NULL, 
    [DateUpdated] DATETIME2 NULL, 
    [UKPrn] INT NOT NULL, 
    [FullName] TEXT NOT NULL,
    CONSTRAINT [PK_Provider_Provider] PRIMARY KEY ([ProviderId])
)
