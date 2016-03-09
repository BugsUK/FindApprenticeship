CREATE PROCEDURE [dbo].[uspRoleTypePrivilegesSelectByRoleTypeId]
@RoleTypeId INT
AS
BEGIN        

	SET NOCOUNT ON          
	BEGIN TRY          
		-- Obtain Privileges for Selected Role 
		Select 
			RoleTypeId,
			PrivilegeTypeId
		FROM
			RoleTypePrivileges
		WHERE
			RoleTypeId = @RoleTypeId

	END TRY          
          
    BEGIN CATCH          
		EXEC RethrowError;          
	END CATCH

	SET NOCOUNT OFF          
END