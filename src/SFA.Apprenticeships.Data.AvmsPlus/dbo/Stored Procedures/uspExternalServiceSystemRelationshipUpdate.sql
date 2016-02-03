CREATE PROCEDURE [dbo].[uspExternalServiceSystemRelationshipUpdate]
	@ExternalServiceId INT,
	@ExternalSystemId INT,
	@IsNasDisabled BIT,
	@IsUserEnabled BIT
AS
	
	IF EXISTS(select 1 FROM ExternalServiceSystemRelationship WHERE ExternalServiceId = @ExternalServiceId AND ExternalSystemId = @ExternalSystemId)
		UPDATE ExternalServiceSystemRelationship
		SET IsNasDisabled = @IsNasDisabled,
			IsUserEnabled = @IsUserEnabled
		WHERE ExternalServiceId = @ExternalServiceId 
		AND	  ExternalSystemId = @ExternalSystemId
	ELSE
		INSERT  INTO ExternalServiceSystemRelationship (ExternalServiceId, ExternalSystemId, IsNasDisabled, IsUserEnabled)
		VALUES (@ExternalServiceId, @ExternalSystemId, @IsNasDisabled, @IsUserEnabled)