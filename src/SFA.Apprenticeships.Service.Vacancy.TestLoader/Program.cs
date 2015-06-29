namespace SFA.Apprenticeships.Service.Vacancy.TestLoader
{
    using System;
    using System.IO;
    using System.Linq;
    using Application.Vacancies.Entities;
    using CsvHelper;
    using CsvLoader;
    using Infrastructure.Common.IoC;
    using Infrastructure.Elastic.Common.Entities;
    using Infrastructure.Elastic.Common.IoC;
    using Infrastructure.Logging.IoC;
    using Infrastructure.VacancyIndexer;
    using Infrastructure.VacancyIndexer.IoC;
    using NLog;
    using StructureMap;

    public class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            if (!CheckArgs(args))
            {
                return;
            }

            Logger.Debug("Loading IoC configuration...");

            var container = new Container(c =>
            {
                c.AddRegistry<CommonRegistry>();
                c.AddRegistry<LoggingRegistry>();
                c.AddRegistry<ElasticsearchCommonRegistry>();
                c.AddRegistry<VacancyIndexerRegistry>();
            });

            Logger.Debug("Loaded IoC configuration...");

            Logger.Debug("Loading Csv file: {0}", args[0]);

            var indexer = container.GetInstance<IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>>();
            var indexDate = DateTime.Today;

            using (var reader = File.OpenText(args[0]))
            {
                Logger.Debug("Loaded Csv file: {0}", args[0]);

                Logger.Debug("Loading Csv Mapping");
                var csv = new CsvReader(reader);
                csv.Configuration.RegisterClassMap<VacancyMapper>();
                Logger.Debug("Loaded Csv Mapping");

                Logger.Debug("Loading Csv Rows");
                var allCsvRows = csv.GetRecords<ApprenticeshipSummaryUpdate>().ToList();
                Logger.Debug("Loaded '{0}' Csv Rows", allCsvRows.Count);

                Logger.Debug("Creating index for date: {0}", indexDate);
                indexer.CreateScheduledIndex(indexDate);
                Logger.Debug("Created index for date: {0}", indexDate);

                foreach (var vacancySummaryUpdate in allCsvRows)
                {
                    Logger.Debug("Indexing item: {0}", vacancySummaryUpdate.Title);
                    vacancySummaryUpdate.ScheduledRefreshDateTime = indexDate;
                    indexer.Index(vacancySummaryUpdate);
                    Logger.Debug("Indexed item: {0}", vacancySummaryUpdate.Title);
                }

                Logger.Debug("Swapping index");
                indexer.SwapIndex(indexDate);
                Logger.Debug("Swapped index");
            }
            
            Logger.Debug("Complete");
        }

        private static bool CheckArgs(string[] args)
        {
            if (args == null || args.Length != 1)
            {
                Console.WriteLine("The usage requires the following paramters:");
                Console.WriteLine("SFA.Apprenticeships.Service.Vacancy.TestLoader.exe <PathToCsvData>");
                return false;
            }

            return true;
        }
    }
}
