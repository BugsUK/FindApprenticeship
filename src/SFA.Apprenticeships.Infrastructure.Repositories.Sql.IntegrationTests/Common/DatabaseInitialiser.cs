namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Microsoft.SqlServer.Dac;
    using Sql.Common;

    /// <summary>
    /// Class to initialise the database independant (as far as possible) of the database access method used to query data.
    /// </summary>
    public class DatabaseInitialiser
    {
        private readonly string _dacpacFilePath;
        private readonly string _targetConnectionString;
        private readonly string _databaseTargetName;

        public DatabaseInitialiser()
        {
            _databaseTargetName = DatabaseConfigurationProvider.Instance.DatabaseTargetName;
            _targetConnectionString = DatabaseConfigurationProvider.Instance.TargetConnectionString;
            _dacpacFilePath = DatabaseConfigurationProvider.Instance.DacPacFilePath;
        }

        public void Publish(bool dropDatabase)
        {
            var dacServices = new DacServices(_targetConnectionString);

            //Wire up events for Deploy messages and for task progress (For less verbose output, don't subscribe to Message Event (handy for debugging perhaps?)
            dacServices.Message += DacServicesHandlers.Message;
            dacServices.ProgressChanged += DacServicesHandlers.ProgressChanged;

            var dbPackage = DacPackage.Load(_dacpacFilePath);
            var dbDeployOptions = new DacDeployOptions { CreateNewDatabase = dropDatabase };

            dacServices.Deploy(dbPackage, _databaseTargetName, true, dbDeployOptions);
        }

        public void Seed(string[] scriptFilePaths)
        {
            using (var connection = new SqlConnection(_targetConnectionString))
            {
                connection.Open();

                foreach (var seedScript in scriptFilePaths)
                {
                    ExecuteScript(seedScript, connection);
                }

                connection.Close();
            }
        }

        public void Seed(IEnumerable<object> seedObjects)
        {
            foreach (var objectToSeed in seedObjects)
            {
                var objectType = objectToSeed.GetType();
                var propertiesToInsert = objectType.GetProperties().Where(p => !IsVirtual(p)).ToList();
                var typeIdPropertyName = GetTableIdPropertyName(objectType);

                if (ShouldInsertTableId(objectType, typeIdPropertyName))
                {
                    typeIdPropertyName = "InsertIdProperty";
                }

                var sqlBuilder = new StringBuilder($"INSERT INTO {GetTableName(objectType)} (");

                BuildColumnNames(propertiesToInsert, typeIdPropertyName, sqlBuilder);
                BuildValues(propertiesToInsert, objectToSeed, typeIdPropertyName, sqlBuilder);
                ExecuteInsert(sqlBuilder.ToString());
            }
        }

        public IGetOpenConnection GetOpenConnection()
        {
            return new GetOpenConnectionFromConnectionString(_targetConnectionString);
        }

        private void ExecuteInsert(string sql)
        {
            Debug.WriteLine(sql);

            using (var connection = new SqlConnection(_targetConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private static bool IsVirtual(PropertyInfo property)
        {
            return property.GetAccessors()[0].IsVirtual;
        }

        private static void BuildValues(IList<PropertyInfo> props, object vacancy, string typeIdProperty, StringBuilder sqlBuilder)
        {
            for (var i = 0; i < props.Count; i++)
            {
                var prop = props[i];
                var name = prop.Name;
                var propValue = prop.GetValue(vacancy, null);

                var isAttributeMapped = prop.CustomAttributes.FirstOrDefault(
                    attributeData => attributeData.AttributeType == typeof(NotMappedAttribute)) == null;

                if (name != typeIdProperty && isAttributeMapped)
                {
                    if (propValue == null)
                    {
                        sqlBuilder.Append("NULL");
                    }
                    else
                    {
                        var formattedValue = InstanceFormatter.FormatTypeInstance(propValue, prop.PropertyType);

                        sqlBuilder.Append(formattedValue);
                    }

                    sqlBuilder.Append(i != props.Count - 1 ? ", " : ")");
                }
                else
                {
                    sqlBuilder.Replace(",", "", sqlBuilder.Length - 2, 2);
                    sqlBuilder.Append(i != props.Count - 1 ? "" : ")");
                }
            }
        }

        private static void BuildColumnNames(IList<PropertyInfo> props, string typeIdProperty, StringBuilder sqlBuilder)
        {
            for (var i = 0; i < props.Count; i++)
            {
                var prop = props[i];
                var name = prop.Name;

                var isAttributeMapped = prop.CustomAttributes.FirstOrDefault(
                    attributeData => attributeData.AttributeType == typeof(NotMappedAttribute)) == null;

                if (name == typeIdProperty)
                {
                    continue;
                }

                if (isAttributeMapped)
                {
                    sqlBuilder.Append(i != props.Count - 1 ? $"[{name}], " : $"[{name}]) VALUES (");
                }
                else if (i == props.Count - 1)
                {
                    sqlBuilder.Replace(",", "", sqlBuilder.Length - 2, 2);
                    sqlBuilder.Append(") VALUES (");
                }
            }
        }

        private static string GetTableName(Type myType)
        {
            var tableName = myType.Name;
            var tableAttribute = myType.CustomAttributes.FirstOrDefault(
                attributeData => attributeData.AttributeType == typeof(TableAttribute));

            if (tableAttribute != null)
            {
                tableName = tableAttribute.ConstructorArguments.First().Value.ToString();
            }

            return tableName;
        }

        private static bool ShouldInsertTableId(Type myType, string tableIdPropertyName)
        {
            // No default in the database for this - must be inserted by caller
            // TODO: Rename to Guid to everything knows this??
            if (tableIdPropertyName == "VacancyId")
                return true;

            var idPropertyInfo = myType.GetProperties().Single(p => p.Name == tableIdPropertyName);

            var identityAttribute =
                idPropertyInfo.CustomAttributes.FirstOrDefault(
                    a => a.AttributeType == typeof(DatabaseGeneratedAttribute));

            if (identityAttribute == null)
            {
                return false;
            }

            return (DatabaseGeneratedOption)identityAttribute.ConstructorArguments.First().Value == DatabaseGeneratedOption.None;
        }

        private static string GetTableIdPropertyName(Type type)
        {
            var tableName = type.Name;
            var possiblePropertyNames = new[] { $"{tableName}Id", $"{tableName}Guid" };

            return possiblePropertyNames.FirstOrDefault(p => type.GetProperties().Any(prop => prop.Name == p));
        }

        private static void ExecuteScript(string seedScript, SqlConnection connection)
        {
            var commandText = File.ReadAllText(seedScript);

            using (var command = new SqlCommand(commandText, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static class DacServicesHandlers
        {
            public static void Message(object sender, DacMessageEventArgs e)
            {
                Debug.WriteLine("DAC Message: {0}", e.Message);
            }

            public static void ProgressChanged(object sender, DacProgressEventArgs e)
            {
                Debug.WriteLine(e.Status + ": " + e.Message);
            }
        }
    }
}
