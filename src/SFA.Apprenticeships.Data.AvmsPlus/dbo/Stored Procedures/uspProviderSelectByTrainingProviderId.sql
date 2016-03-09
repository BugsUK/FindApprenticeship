CREATE PROCEDURE [dbo].[uspProviderSelectByTrainingProviderId]
	@providerSiteId int
AS
BEGIN
	SELECT 
	p.ProviderID As 'ProviderId',
	p.UPIN As 'UPIN',
	p.UKPRN As 'UKPRN',
	p.FullName As 'FullName',
	p.TradingName As 'TradingName',
	p.IsContracted As 'IsContracted',
	p.ContractedFrom As 'ContractedFrom',
	p.ContractedTo As 'ContractedTo',
	p.ProviderStatusTypeID AS 'ProviderStatusId'
	FROM ProviderSiteRelationship psr
		INNER JOIN Provider p on psr.ProviderId = p.ProviderId AND psr.ProviderSiteRelationShipTypeID = 1
	WHERE psr.ProviderSiteID = @providerSiteId
END