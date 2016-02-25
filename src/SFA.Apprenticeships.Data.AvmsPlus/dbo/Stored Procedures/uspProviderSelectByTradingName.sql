CREATE PROCEDURE [dbo].[uspProviderSelectByTradingName]
	@tradingname nvarchar(255)
AS
BEGIN
	SELECT 
	ProviderID As 'ProviderId',
	UPIN As 'UPIN',
	UKPRN As 'UKPRN',
	FullName As 'FullName',
	TradingName As 'TradingName',
	IsContracted As 'IsContracted',
	ContractedFrom As 'ContractedFrom',
	ContractedTo As 'ContractedTo'
	FROM Provider
	WHERE TradingName = @tradingname AND ProviderStatusTypeID <> 2


END