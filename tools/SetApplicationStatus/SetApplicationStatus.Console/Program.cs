namespace SetApplicationStatus.Console
{
    using System;
    using System.Linq;
    using Entities.Sql;
    using NLog;

    public class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            // TODO: traineeships

            var options = new CommandLineOptions();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine(options.GetUsage());
                Environment.ExitCode = 1;
                return;
            }

            var ukprn = options.Ukprn;
            var sqlConnectionString = options.SqlConnectionString;
            var mongoConnectionString = options.MongoConnectionString;
            var update = options.Update;

            Logger.Info("Started");

            var sqlApplicationRepository = new Repositories.Sql.ApplicationRepository(sqlConnectionString);

            var sourceApplications = sqlApplicationRepository
                .GetApplicationIdsByUkprn(ukprn)
                .Where(application => application.Status == ApplicationStatus.InProgress);

            var mongoApplicationRepository = new Repositories.Mongo.ApplicationRepository(mongoConnectionString);

            foreach (var sourceApplication in sourceApplications)
            {
                Logger.Info($"SOURCE: Application ID: {sourceApplication.ApplicationId}, Status: {sourceApplication.Status}");

                var ids = new[]
                {
                    sourceApplication.ApplicationId
                };

                var targetApplications = mongoApplicationRepository.GetApplicationByLegacyIds(ids);

                foreach (var targetApplication in targetApplications)
                {
                    Logger.Info($"TARGET: Id: {targetApplication.Id} Legacy Application ID: {targetApplication.LegacyApplicationId}, Status: {targetApplication.Status}");

                    if (targetApplication.Status != Entities.Mongo.ApplicationStatus.InProgress)
                    {
                        Logger.Warn($"Expected: {Entities.Mongo.ApplicationStatus.InProgress}, Actual: {targetApplication.Status}");

                        if (targetApplication.Status == Entities.Mongo.ApplicationStatus.Submitted)
                        {
                            if (update)
                            {
                                var updated = mongoApplicationRepository.SetApplicationStatus(
                                    targetApplication.Id, Entities.Mongo.ApplicationStatus.InProgress);

                                if (updated)
                                {
                                    Logger.Info("^Updated");
                                }
                                else
                                {
                                    Logger.Error("^Update failed");
                                }
                            }
                        }
                    }
                }
            }

            Logger.Info("Finished");
        }
    }
}
