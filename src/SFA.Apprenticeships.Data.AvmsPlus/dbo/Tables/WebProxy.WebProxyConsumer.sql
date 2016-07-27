 CREATE TABLE WebProxy.WebProxyConsumer
                (
                    ExternalSystemId                    UNIQUEIDENTIFIER PRIMARY KEY,
                    WebProxyConsumerId                  INT,
                    ShortDescription                    VARCHAR(MAX),
                    FullDescription                     VARCHAR(MAX),
                    RouteToCompatabilityWebServiceRegex VARCHAR(MAX)
                );
