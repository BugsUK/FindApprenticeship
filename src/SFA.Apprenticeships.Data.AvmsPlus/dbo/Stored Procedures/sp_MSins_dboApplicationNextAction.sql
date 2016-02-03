create procedure [sp_MSins_dboApplicationNextAction]
    @c1 int,
    @c2 nvarchar(3),
    @c3 nvarchar(10),
    @c4 nvarchar(100)
as
begin  
	insert into [dbo].[ApplicationNextAction](
		[ApplicationNextActionId],
		[CodeName],
		[ShortName],
		[FullName]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end