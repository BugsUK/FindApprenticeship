create procedure [sp_MSins_dboDisplayText]
    @c1 int,
    @c2 nvarchar(250),
    @c3 int,
    @c4 nvarchar(250)
as
begin  
	insert into [dbo].[DisplayText](
		[DisplayTextId],
		[Type],
		[Id],
		[StandardText]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end