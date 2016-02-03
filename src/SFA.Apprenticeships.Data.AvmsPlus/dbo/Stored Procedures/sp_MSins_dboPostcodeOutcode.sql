create procedure [sp_MSins_dboPostcodeOutcode]
    @c1 int,
    @c2 nchar(4)
as
begin  
	insert into [dbo].[PostcodeOutcode](
		[PostcodeOutcodeId],
		[Outcode]
	) values (
    @c1,
    @c2	) 
end