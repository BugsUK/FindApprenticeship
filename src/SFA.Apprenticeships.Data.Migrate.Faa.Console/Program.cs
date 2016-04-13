namespace SFA.Apprenticeships.Data.Migrate.Faa.Console
{
    using SFA.Infrastructure.Azure.Configuration;
    using SFA.Infrastructure.Console;
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities.Mongo;
    using Infrastructure.Repositories.Sql.Common;
    using Mappers;
    using Repository.Mongo;
    using Repository.Sql;

    public class Program
    {
        public static void Main(string[] args)
        {
            var log = new ConsoleLogService();

            log.Info("Initialisation");

            var configService = new AzureBlobConfigurationService(new MyConfigurationManager(), log);
            var persistentConfig = configService.Get<MigrateFromAvmsConfiguration>();

            var sourceDatabase = new GetOpenConnectionFromConnectionString(persistentConfig.SourceConnectionString);
            var targetDatabase = new GetOpenConnectionFromConnectionString(persistentConfig.TargetConnectionString);
            var genericSyncRepository = new GenericSyncRespository(log, sourceDatabase, targetDatabase);

            var applicationTable = new ApplicationTable();
            genericSyncRepository.DeleteAll(applicationTable);

            var candidateRepository = new CandidateRepository(configService, log);
            var apprenticeshipApplicationsRepository = new ApprenticeshipApplicationsRepository(configService);

            log.Info("Loading candidates");
            var candidates = candidateRepository.GetAllCandidatesAsync().Result;
            log.Info($"Completed loading {candidates.Count} candidates");

            var expectedCount = apprenticeshipApplicationsRepository.GetApprenticeshipApplicationsCount().Result;
            var count = 0;

            var cursor = apprenticeshipApplicationsRepository.GetAllApprenticeshipApplications().Result;
            while (cursor.MoveNextAsync().Result)
            {
                var batch = cursor.Current.ToList();
                log.Info($"Processing {batch.Count} Applications");

                var applications = batch.Where(a => candidates.ContainsKey(a.CandidateId)).Select(a => a.ToApplicationDictionary(candidates[a.CandidateId])).ToList();
                count += applications.Count;
                log.Info($"Inserting {applications.Count} Applications");
                try
                {
                    genericSyncRepository.BulkInsert(applicationTable, applications);
                }
                catch (Exception ex)
                {
                    log.Error("Error while inserting applications", ex);
                }
                var percentage = ((double)count / expectedCount) * 100;
                log.Info($"Inserted {applications.Count} Applications and {count} Applications out of {expectedCount} in total. {Math.Round(percentage, 2)}% complete");
            }

            Console.WriteLine(count);
            Console.ReadKey();
        }
    }

    internal class MyConfigurationManager : IConfigurationManager
    {
        public string ConfigurationFilePath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string GetAppSetting(string key)
        {
            throw new NotImplementedException();
        }

        public T GetAppSetting<T>(string key)
        {
            if (key == "ConfigurationStorageConnectionString")
                return (T)Convert.ChangeType("UseDevelopmentStorage=true", typeof(T));

            throw new NotImplementedException();
        }

        public string TryGetAppSetting(string key)
        {
            throw new NotImplementedException();
        }
    }
}
