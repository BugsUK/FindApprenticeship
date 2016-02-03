CREATE PROCEDURE [dbo].[uspProviderSelectByUKPRN]
	@ukprn int
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
	ProviderStatusTypeID AS 'ProviderStatusId',
	IsNASProvider As 'IsNASProvider'
	FROM Provider
	WHERE UKPRN=@ukprn AND ProviderStatusTypeID <> 2


END