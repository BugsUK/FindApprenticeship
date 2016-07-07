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
            // Get MongoDB connection string from args
            // Get SQL connection string from args
            // Get ukprns from args

            // For each ukprn:
            //  For each applications in AvmsPlus where status is In Progress:
            //      Get application from MongoDB
            //      If application status is Submitted:
            //          Set application status to In Progress
            //          Set DateUpdated to UTC now

            // TODO: flag to update / not update
            // TODO: traineeships

            var ukprn = args[0];
            var sqlConnectionString = args[1];
            var mongoConnectionString = args[2];
            var readOnly = args.Length == 4 && string.Equals(args[3], "Y", StringComparison.InvariantCultureIgnoreCase);
            
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
                            if (!readOnly)
                            {
                                var updated = mongoApplicationRepository.SetApplicationStatus(
                                    Guid.NewGuid(),
                                    Entities.Mongo.ApplicationStatus.InProgress);

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
