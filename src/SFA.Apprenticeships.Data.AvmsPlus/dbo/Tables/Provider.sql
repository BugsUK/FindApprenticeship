CREATE TABLE [dbo].[Provider] (
    [ProviderID]           INT            IDENTITY (1, 1) NOT NULL,
    [UPIN]                 INT            NOT NULL,
    [UKPRN]                INT            NOT NULL,
    [FullName]             NVARCHAR (255) NULL,
    [TradingName]          NVARCHAR (255) NULL,
    [IsContracted]         BIT            CONSTRAINT [DFT_Provider_IsContracted] DEFAULT ((0)) NOT NULL,
    [ContractedFrom]       DATETIME       NULL,
    [ContractedTo]         DATETIME       NULL,
    [ProviderStatusTypeID] INT            CONSTRAINT [DFT_ProviderStatus] DEFAULT ((1)) NOT NULL,
    [IsNASProvider]        BIT            CONSTRAINT [DF_Provider_IsNASProvider] DEFAULT ((0)) NOT NULL,
    [OriginalUPIN]         INT            NULL,
    CONSTRAINT [PK_Provider] PRIMARY KEY CLUSTERED ([ProviderID] ASC),
    CONSTRAINT [FK_TrainingProvider_ProviderStatusTypeID] FOREIGN KEY ([ProviderStatusTypeID]) REFERENCES [dbo].[EmployerTrainingProviderStatus] ([EmployerTrainingProviderStatusId]),
    CONSTRAINT [UQ_Provider_UKPRN] UNIQUE NONCLUSTERED ([UKPRN] ASC, [ProviderStatusTypeID] ASC)
);

