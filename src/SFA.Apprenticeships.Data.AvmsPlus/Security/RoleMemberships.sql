GO
ALTER ROLE [db_owner] ADD MEMBER [apisvcadmin];

GO
ALTER ROLE [db_datawriter] ADD MEMBER [apisvc];

GO
ALTER ROLE [db_datawriter] ADD MEMBER [faa_user];

GO
ALTER ROLE [db_datareader] ADD MEMBER [apisvc];

GO
ALTER ROLE [db_datareader] ADD MEMBER [datascience_user];

GO
ALTER ROLE [db_datareader] ADD MEMBER [das_user];

GO
ALTER ROLE [db_datareader] ADD MEMBER [faa_user];
