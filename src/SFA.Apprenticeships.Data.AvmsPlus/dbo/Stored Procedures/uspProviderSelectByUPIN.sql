CREATE PROCEDURE [dbo].[uspProviderSelectByUPIN]
	@upin int
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
	ContractedTo As 'ContractedTo',
	ProviderStatusTypeID AS 'ProviderStatusId'
	FROM Provider
	WHERE UPIN=@upin AND ProviderStatusTypeID <> 2

END