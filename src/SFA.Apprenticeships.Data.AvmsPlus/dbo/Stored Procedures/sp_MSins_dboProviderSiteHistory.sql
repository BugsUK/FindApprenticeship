create procedure [sp_MSins_dboProviderSiteHistory]
    @c1 int,
    @c2 int,
    @c3 nvarchar(50),
    @c4 datetime,
    @c5 int,
    @c6 nvarchar(4000)
as
begin  
	insert into [dbo].[ProviderSiteHistory](
		[TrainingProviderHistoryId],
		[TrainingProviderId],
		[UserName],
		[HistoryDate],
		[EventTypeId],
		[Comment]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5,
    @c6	) 
end