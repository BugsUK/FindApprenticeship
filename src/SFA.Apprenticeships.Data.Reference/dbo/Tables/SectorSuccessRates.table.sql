CREATE TABLE [dbo].[SectorSuccessRates]
(
	ProviderID int NOT NULL, 
	SectorID int NOT NULL,
	PassRate smallint NOT NULL,
	New bit NOT NULL
)
