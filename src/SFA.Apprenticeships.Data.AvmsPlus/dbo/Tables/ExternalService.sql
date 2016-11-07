CREATE TABLE [dbo].[ExternalService] (
    [ID]                        INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [ServiceName]               NVARCHAR (50)  NULL,
    [ServiceShortName]          NVARCHAR (3)   NULL,
    [ServiceDescription]        NVARCHAR (300) NULL,
    [IsEmployerAllowed]         BIT            NULL,
    [IsTrainingProviderAllowed] BIT            NULL,
    [IsThirdPartyAllowed]       BIT            NULL,
    CONSTRAINT [PK_ExternalService] PRIMARY KEY CLUSTERED ([ID] ASC)
);

GO
CREATE NONCLUSTERED INDEX [nci_wi_ExternalMessages_F47B829097E9311BF6EBD1E6E93E27CB] 
ON [dbo].[ExternalMessages]([ReceivedDate] ASC);


GO
