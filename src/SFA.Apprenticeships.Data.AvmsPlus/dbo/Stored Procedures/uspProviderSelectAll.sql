CREATE PROCEDURE [dbo].[uspProviderSelectAll]
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
	WHERE ProviderStatusTypeID <> 2
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[uspProviderSelectAll] TO [db_executor]
    AS [dbo];

