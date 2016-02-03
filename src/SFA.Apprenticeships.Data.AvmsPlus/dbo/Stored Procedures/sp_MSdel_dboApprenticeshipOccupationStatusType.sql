create procedure [sp_MSdel_dboApprenticeshipOccupationStatusType]
		@pkc1 int
as
begin  
	delete [dbo].[ApprenticeshipOccupationStatusType]
where [ApprenticeshipOccupationStatusTypeId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end