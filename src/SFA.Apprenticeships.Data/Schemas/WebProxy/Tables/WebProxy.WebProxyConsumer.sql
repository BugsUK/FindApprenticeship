CREATE TABLE [WebProxy].[WebProxyConsumer]
(
	[WebProxyConsumerId] [int] IDENTITY(1,1) NOT NULL,
	[ExternalSystemId] [uniqueidentifier] NOT NULL,
	[ShortDescription] [nvarchar](50) NOT NULL,
	[FullDescription] [nvarchar](max) NOT NULL,
	[RouteToCompatabilityWebServiceRegex] [nvarchar](max) NOT NULL
    CONSTRAINT [PK_WebProxyConsumer] PRIMARY KEY ([WebProxyConsumerId])
)
