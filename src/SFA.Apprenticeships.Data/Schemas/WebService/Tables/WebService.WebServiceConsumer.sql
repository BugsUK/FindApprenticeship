CREATE TABLE [WebService].[WebServiceConsumer]
(
	[WebServiceConsumerId] INT NOT NULL IDENTITY, 
    [WebServiceConsumerTypeCode] CHAR NOT NULL, 
    [ExternalSystemId] UNIQUEIDENTIFIER NOT NULL, 
    [ExternalSystemName] NVARCHAR(MAX) NOT NULL, 
    [ExternalSystemPassword] NVARCHAR(16) NOT NULL, 
    [AllowVacancySummaryService] BIT NOT NULL, 
    [AllowVacancyDetailService] BIT NOT NULL, 
    [AllowReferenceDataService] BIT NOT NULL, 
    [AllowVacancyUploadService] BIT NOT NULL, 
    CONSTRAINT [CK_WebServiceConsumer_WebServiceConsumerTypeCode] CHECK ([WebServiceConsumerTypeCode] IN ('P', 'T', 'E')), 
    CONSTRAINT [PK_WebServiceConsumer] PRIMARY KEY ([WebServiceConsumerId]),
)
