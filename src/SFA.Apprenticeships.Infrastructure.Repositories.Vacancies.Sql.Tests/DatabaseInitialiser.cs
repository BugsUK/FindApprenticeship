namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.Sql.Tests
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
    
    public class DatabaseInitialiser
    {
        private readonly string _dacpacFilePath;
        private readonly string _targetConnectionString;
        private readonly string _databaseTargetName;

        public DatabaseInitialiser(string dacpacFilePath, string targetConnectionString, string databaseTargetName)
        {
            _dacpacFilePath = dacpacFilePath;
            _targetConnectionString = targetConnectionString;
            _databaseTargetName = databaseTargetName;
        }

        public void Publish(bool dropDatabase)
        {
            var dacServices = new DacServices(_targetConnectionString);

            //Wire up events for Deploy messages and for task progress (For less verbose output, don't subscribe to Message Event (handy for debugging perhaps?)
            dacServices.Message += dacServices_Message;
            dacServices.ProgressChanged += dacServices_ProgressChanged;

            var dbPackage = DacPackage.Load(_dacpacFilePath);

            var dbDeployOptions = new DacDeployOptions {CreateNewDatabase = dropDatabase};

            dacServices.Deploy(dbPackage, _databaseTargetName, true, dbDeployOptions);
        }

        public void Seed(string[] seedScripts)
        {
            using (var connection = new SqlConnection(_targetConnectionString))
            {
                connection.Open();
                foreach (var seedScript in seedScripts)
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

        private void ExecuteInsert(string sql)
        {
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

                var propValue = prop.GetValue(vacancy, null);

                var name = prop.Name;

                if (name != typeIdProperty)
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
                    sqlBuilder.Append(i != props.Count - 1 ? "" : ")");
                }
            }
        }

        private static void BuildColumnNames(IList<PropertyInfo> props, string typeIdProperty, StringBuilder sqlBuilder)
        {
            for (int i = 0; i < props.Count; i++)
            {
                var prop = props[i];

                var name = prop.Name;

                if (name != typeIdProperty)
                {
                    sqlBuilder.Append(i != props.Count - 1 ? $"[{name}], " : $"[{name}]) VALUES (");
                }
            }
        }

        private static string GetTableName(Type myType)
        {
            var tableName = myType.Name;
            var tableAttribute =
                myType.CustomAttributes.FirstOrDefault(
                    a => a.AttributeType == typeof (TableAttribute));
            if (tableAttribute != null)
            {
                tableName = tableAttribute.ConstructorArguments.First().Value.ToString();
            }
            return tableName;
        }

        private static bool ShouldInsertTableId(Type myType, string tableIdPropertyName)
        {
            var idPropertyType = myType.GetProperties().Single(p => p.Name == tableIdPropertyName);

            var identityAttribute =
                idPropertyType.CustomAttributes.FirstOrDefault(
                    a => a.AttributeType == typeof(DatabaseGeneratedAttribute));

            if (identityAttribute != null)
            {
                return (DatabaseGeneratedOption)identityAttribute.ConstructorArguments.First().Value == DatabaseGeneratedOption.None;
            }

            return false;
        }

        private static string GetTableIdPropertyName(Type type)
        {
            var tableName = type.Name;
            var possiblePropertyNames = new[] { $"{tableName}Id", $"{tableName}Guid" };

            return possiblePropertyNames.FirstOrDefault(p => type.GetProperties().Any(prop => prop.Name == p));

            //var possiblePostfixes = new[] {"Id", "Guid"};
            //foreach (var possiblePostfix in possiblePostfixes)
            //{
            //    var propertyName = GetTableName(type) + possiblePostfix;
            //    if (type.GetProperties().Any(p => p.Name == propertyName))
            //    {
            //        return propertyName;
            //    }
            //}
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