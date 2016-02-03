EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'sfasql2008\svc-AVMSLogReaderAge';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'sfasql2008\svc-AVMSReportsSnaps';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'sfasql2008\svc-AVMSSandboxSnaps';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'sfasql2008\svc-appvacapppool';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'sfasql2008\svc-appvacapppool';

