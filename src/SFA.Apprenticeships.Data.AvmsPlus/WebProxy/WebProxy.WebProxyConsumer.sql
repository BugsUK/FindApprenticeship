CREATE TABLE [WebProxy].[WebProxyConsumer]
(
	[WebProxyConsumerId] [int] IDENTITY(1,1) NOT NULL,
	[ExternalSystemId] [uniqueidentifier] NOT NULL,
	[ShortDescription] [nvarchar](50),
	[FullDescription] [nvarchar](max),
	[RouteToCompatabilityWebServiceRegex] [nvarchar](max)
    CONSTRAINT [PK_WebProxyConsumer] PRIMARY KEY ([WebProxyConsumerId])
)
