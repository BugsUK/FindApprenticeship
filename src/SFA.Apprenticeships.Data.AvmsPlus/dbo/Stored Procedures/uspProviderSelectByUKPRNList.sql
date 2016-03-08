CREATE PROCEDURE [dbo].[uspProviderSelectByUKPRNList]
	@ukprnlist VARCHAR (1000)
AS
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
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
	INNER Join (SELECT * from dbo.fnx_SplitListToTable(@ukprnlist)) as TEMP on TEMP.ID = UKPRN
	WHERE 
		ProviderStatusTypeID <> 2
	


END