namespace SFA.Apprenticeships.Data.Migrate.Faa.Console
{
    using SFA.Infrastructure.Azure.Configuration;
    using SFA.Infrastructure.Console;
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities.Mongo;
    using Mappers;
    using Repository.Mongo;

    public class Program
    {
        public static void Main(string[] args)
        {
            var log = new ConsoleLogService();

            log.Info("Initialisation");

            var configService = new AzureBlobConfigurationService(new MyConfigurationManager(), log);
            var persistentConfig = configService.Get<MigrateFromAvmsConfiguration>();

            var candidateRepository = new CandidateRepository(configService);
            var apprenticeshipApplicationsRepository = new ApprenticeshipApplicationsRepository(configService);

            var lastId = default(Guid?);
            var running = true;
            var count = 0;

            while (running)
            {
                var cursor = apprenticeshipApplicationsRepository.GetApprenticeshipApplicationsPageAsync(lastId).Result;
                while (cursor.MoveNextAsync().Result)
                {
                    var batch = cursor.Current.ToList();

                    var candidates = new Dictionary<Guid, Candidate>(batch.Count);
                    var candidatesCursor = candidateRepository.GetCandidatesByIds(batch.Select(b => b.CandidateId)).Result;
                    while (candidatesCursor.MoveNextAsync().Result)
                    {
                        var candidatesBatch = candidatesCursor.Current.ToList();
                        foreach (var candidate in candidatesBatch)
                        {
                            candidates[candidate.Id] = candidate;
                        }
                    }

                    var applications = batch.Where(a => candidates.ContainsKey(a.CandidateId)).Select(a => (IDictionary<string, object>)a.ToApplication(candidates[a.CandidateId])).ToList();
                    running = applications.Any();
                    count += applications.Count;
                    if (running)
                    {
                        //syncrepo.BulkInsert(table, applications);
                        lastId = (Guid)applications.Last()["ApplicationGuid"];
                    }
                }
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
