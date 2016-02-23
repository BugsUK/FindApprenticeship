CREATE TABLE [dbo].[SectorSuccessRates] (
    [ProviderID] INT      NOT NULL,
    [SectorID]   INT      NOT NULL,
    [PassRate]   SMALLINT NOT NULL,
    [New]        BIT      NOT NULL,
    CONSTRAINT [PK_SectorSuccessRates] PRIMARY KEY CLUSTERED ([ProviderID] ASC, [SectorID] ASC),
    CONSTRAINT [FK_SectorSuccessRates_ApprenticeshipOccupation] FOREIGN KEY ([SectorID]) REFERENCES [dbo].[ApprenticeshipOccupation] ([ApprenticeshipOccupationId]),
    CONSTRAINT [FK_SectorSuccessRates_Provider] FOREIGN KEY ([ProviderID]) REFERENCES [dbo].[Provider] ([ProviderID])
);

