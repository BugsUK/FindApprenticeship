create procedure [sp_MSins_dboTermsAndConditions]
    @c1 int,
    @c2 int,
    @c3 nvarchar(200),
    @c4 ntext
as
begin  
	insert into [dbo].[TermsAndConditions](
		[TermsAndConditionsId],
		[UserTypeId],
		[Fullname],
		[Content]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end