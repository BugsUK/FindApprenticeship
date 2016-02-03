create procedure [sp_MSins_dboEmployerSICCodes]
    @c1 int,
    @c2 int,
    @c3 int
as
begin  
	insert into [dbo].[EmployerSICCodes](
		[EmployerSICCodes],
		[EmployerId],
		[SICId]
	) values (
    @c1,
    @c2,
    @c3	) 
end