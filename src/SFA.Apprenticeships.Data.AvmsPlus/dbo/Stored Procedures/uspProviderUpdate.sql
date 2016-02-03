CREATE PROCEDURE [dbo].[uspProviderUpdate]
	@ProviderId int, 
	@UKPRN int, 
	@FullName nvarchar(255),
	@TradingName nvarchar(255),
	@IsContracted bit,
	@ContractedFrom date,
	@ContractedTo date,
	@ProviderStatusTypeID int,
	@IsNASProvider bit,
	@UPIN int
AS
Begin
	UPDATE [Provider]
	SET ProviderStatusTypeID = @ProviderStatusTypeID, 
	FullName = @FullName,
	TradingName = @TradingName,
	IsNASProvider = @IsNASProvider,
	IsContracted = @IsContracted, 
	ContractedFrom = @ContractedFrom,
	ContractedTo = @ContractedTo,
	Upin = @UPIN
	WHERE ProviderId = @ProviderId
End