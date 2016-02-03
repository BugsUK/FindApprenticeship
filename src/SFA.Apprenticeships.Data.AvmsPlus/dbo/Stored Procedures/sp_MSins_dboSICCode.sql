create procedure [sp_MSins_dboSICCode]
    @c1 int,
    @c2 smallint,
    @c3 int,
    @c4 nvarchar(256)
as
begin  
	insert into [dbo].[SICCode](
		[SICCodeId],
		[Year],
		[SICCode],
		[Description]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end