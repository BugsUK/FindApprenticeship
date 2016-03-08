CREATE PROCEDURE [dbo].[uspExternalSystemUpdate]
	@SystemId Int,
	@SystemName	nvarchar(400),
	@ContactName	nvarchar(300),
	@ContactEmail	nvarchar(400),
	@ContactNumber	nvarchar(64),
	@IsNasDisabled	bit,
	@IsUserEnabled	bit	
AS

	UPDATE ExternalSystem 
	SET 
		SystemName = @SystemName,
		ContactName = @ContactName,
		ContactEmail = @ContactEmail,
		ContactNumber = @ContactNumber,
		IsNasDisabled = @IsNasDisabled,
		IsUserEnabled = @IsUserEnabled
	WHERE ID = @SystemId