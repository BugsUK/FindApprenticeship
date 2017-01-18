namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests.Common
{
    using System;
    using System.IO;
    using System.Linq;
    using Configuration;
    using SFA.Infrastructure.Azure.Configuration;
    using SFA.Infrastructure.Configuration;
    using Web.Common.Configuration;

    internal class DatabaseConfigurationProvider
    {
        private const string DatabaseProjectName = "SFA.Apprenticeships.Data.AvmsPlus";
        private static DatabaseConfigurationProvider _instance;

        public static DatabaseConfigurationProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseConfigurationProvider();
                }

                return _instance;
            }
        }

        public string DacPacFilePath { get; }

        public string TargetConnectionString { get; }

        public string DatabaseTargetName { get; }

        private DatabaseConfigurationProvider()
        {
            var configurationManager = new ConfigurationManager();

            var configurationService = new AzureBlobConfigurationService(configurationManager);

            var environment = configurationService.Get<CommonWebConfiguration>().Environment;
            var connectionString = configurationService.Get<SqlConfiguration>().ConnectionString;

            var originInitialCatalog = connectionString
                .Split(';')
                .Single(p => p.StartsWith("Initial Catalog"))
                .Split('=')
                .Last();

            DatabaseTargetName = $"AvmsPlus-{environment}-Test";
            
            TargetConnectionString = connectionString.Replace($"Initial Catalog={originInitialCatalog}",
                $"Initial Catalog={DatabaseTargetName}");

            var databaseProjectPath = AppDomain.CurrentDomain.BaseDirectory + $"\\..\\..\\..\\{DatabaseProjectName}";
            var dacPacRelativePath = $"\\bin\\{environment}\\{DatabaseProjectName}.dacpac";
            DacPacFilePath = Path.Combine(databaseProjectPath + dacPacRelativePath);
            if (!File.Exists(DacPacFilePath))
            {
                //For NCrunch on Dave's machine
                databaseProjectPath = $"C:\\_Git\\Beta\\src\\{DatabaseProjectName}";
                DacPacFilePath = Path.Combine(databaseProjectPath + dacPacRelativePath);
            }
        }
    }
}