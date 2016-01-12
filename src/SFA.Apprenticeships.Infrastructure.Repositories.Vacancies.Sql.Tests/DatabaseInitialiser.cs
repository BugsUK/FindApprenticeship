namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.Sql.Tests
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.SqlServer.Dac;

    public class DatabaseInitialiser
    {
        private readonly string _databaseProjectPath;
        private readonly string _targetConnectionString;
        private readonly string _databaseTargetName;

        public DatabaseInitialiser(string databaseProjectPath, string targetConnectionString, string databaseTargetName)
        {
            _databaseProjectPath = databaseProjectPath;
            _targetConnectionString = targetConnectionString;
            _databaseTargetName = databaseTargetName;
        }

        public void Publish(bool dropDatabase, IEnumerable<string> seedScripts)
        {
            var dacServices = new DacServices(_targetConnectionString);

            //Wire up events for Deploy messages and for task progress (For less verbose output, don't subscribe to Message Event (handy for debugging perhaps?)
            dacServices.Message += dacServices_Message;
            dacServices.ProgressChanged += dacServices_ProgressChanged;

            var databaseProjectName = Path.GetFileName(_databaseProjectPath);
            var snapshotPath = Path.Combine(_databaseProjectPath + $"\\bin\\Local\\{databaseProjectName}.dacpac"); //TODO: get configuration from settings
            var dbPackage = DacPackage.Load(snapshotPath);

            var dbDeployOptions = new DacDeployOptions {CreateNewDatabase = dropDatabase};

            dacServices.Deploy(dbPackage, _databaseTargetName, true, dbDeployOptions);

            SeedData(seedScripts, _targetConnectionString);
        }

        private void SeedData(IEnumerable<string> seedScripts, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var seedScript in seedScripts)
                {
                    ExecuteScript(seedScript, connection);
                }
                connection.Close();
            }
        }

        private void ExecuteScript(string seedScript, SqlConnection connection)
        {

            var commandText = File.ReadAllText(seedScript);

            using (var command = new SqlCommand(commandText, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        static void dacServices_Message(object sender, DacMessageEventArgs e)
        {
            Debug.WriteLine("DAC Message: {0}", e.Message);
        }

        static void dacServices_ProgressChanged(object sender, DacProgressEventArgs e)
        {
            Debug.WriteLine(e.Status + ": " + e.Message);
        }
    }
}