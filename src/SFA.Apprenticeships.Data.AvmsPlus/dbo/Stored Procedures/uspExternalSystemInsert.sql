CREATE PROCEDURE [dbo].[uspExternalSystemInsert]
	@SystemCode	UNIQUEIDENTIFIER,
	@SystemName	nvarchar(400),
	@OrganisationId	int,
	@OrganisationType	int,
	@ContactName	nvarchar(300),
	@ContactEmail	nvarchar(400),
	@ContactNumber	nvarchar(64),
	@IsNasDisabled	bit,
	@IsUserEnabled	bit,
	@SystemId Int out
AS
	
	INSERT INTO ExternalSystem
	(
		SystemCode,
		SystemName,
		OrganisationId,
		OrganisationType,
		ContactName,
		ContactEmail,
		ContactNumber,
		IsNasDisabled,
		IsUserEnabled
	)
	VALUES
	(
		@SystemCode,
		@SystemName,
		@OrganisationId,
		@OrganisationType,
		@ContactName,
		@ContactEmail,
		@ContactNumber,
		@IsNasDisabled,
		@IsUserEnabled
	)

	SET @SystemId = SCOPE_IDENTITY()