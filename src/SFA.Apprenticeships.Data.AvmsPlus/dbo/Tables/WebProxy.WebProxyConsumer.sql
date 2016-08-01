 CREATE TABLE WebProxy.WebProxyConsumer
                (
                    ExternalSystemId                    UNIQUEIDENTIFIER PRIMARY KEY,
                    WebProxyConsumerId                  INT IDENTITY(1,1),
                    ShortDescription                    VARCHAR(MAX),
                    FullDescription                     VARCHAR(MAX),
                    RouteToCompatabilityWebServiceRegex VARCHAR(MAX)
                );
