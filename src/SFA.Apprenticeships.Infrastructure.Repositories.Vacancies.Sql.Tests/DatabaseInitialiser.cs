namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.Sql.Tests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.SqlServer.Dac;

    public class DatabaseInitialiser
    {
        private readonly string _databaseProjectPath;
        private readonly string _connectionString;
        private readonly string _databaseTargetName;

        public DatabaseInitialiser(string databaseProjectPath, string connectionString, string databaseTargetName)
        {
            _databaseProjectPath = databaseProjectPath;
            _connectionString = connectionString;
            _databaseTargetName = databaseTargetName;
        }

        public void Publish(bool dropDatabase)
        {
            var dacServices = new DacServices(_connectionString);

            //Wire up events for Deploy messages and for task progress (For less verbose output, don't subscribe to Message Event (handy for debugging perhaps?)
            dacServices.Message += new EventHandler<DacMessageEventArgs>(dacServices_Message);
            dacServices.ProgressChanged += new EventHandler<DacProgressEventArgs>(dacServices_ProgressChanged);

            var databaseProjectName = Path.GetFileName(_databaseProjectPath);
            var snapshotPath = Path.Combine(_databaseProjectPath + $"\\bin\\Local\\{databaseProjectName}.dacpac"); //configure Local
            var dbPackage = DacPackage.Load(snapshotPath);

            var dbDeployOptions = new DacDeployOptions();
            //Cut out a lot of options here for configuring deployment, but are all part of DacDeployOptions
            dbDeployOptions.SqlCommandVariableValues.Add("debug", "false");
            dbDeployOptions.CreateNewDatabase = dropDatabase;
            
            dacServices.Deploy(dbPackage, _databaseTargetName, true, dbDeployOptions);
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