CREATE PROCEDURE [dbo].[uspEmployerDelete]
	 @employerId int
AS
BEGIN
	SET NOCOUNT ON

	declare @del_employer int
	declare @del_employerhistory int
	
	begin transaction

		DELETE FROM [dbo].[EmployerHistory]
		WHERE [EmployerId]=@employerId

		select @del_employerhistory = @@error

		DELETE FROM [dbo].[Employer]
		WHERE [EmployerId]=@employerId

		select @del_employer = @@error

	if @del_employerhistory = 0 and @del_employer = 0
		begin 
			commit transaction
		end
	else
		begin 
			rollback transaction
		end
  
    SET NOCOUNT OFF
END