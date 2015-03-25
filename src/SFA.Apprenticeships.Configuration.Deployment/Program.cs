namespace SFA.Apprenticeships.Configuration.Deployment
{
    using System;
    using System.IO;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Common.Configuration;
    using Infrastructure.Common.IoC;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using StructureMap;

    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var container = new Container(x => x.AddRegistry<CommonRegistry>());
                var configurationManager = container.GetInstance<IConfigurationManager>();
                var mongoConnectionString = configurationManager.GetAppSetting<string>("ConfigurationDb");

                var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;
                var database = new MongoClient(mongoConnectionString).GetServer().GetDatabase(mongoDbName);
                var collection = database.GetCollection("configuration");

                var json = File.ReadAllText(@"Configs\settings.json");
                var document = BsonSerializer.Deserialize<BsonDocument>(json);
                var dateTimeUpdated = DateTime.Now;
                document.InsertAt(0, new BsonElement("DateTimeUpdated", dateTimeUpdated));

                //Ensure there can only be one entry at the end
                IMongoQuery query = Query.NE("DateTimeUpdated", dateTimeUpdated);
                collection.Insert(document);
                collection.Remove(query);
                //Success
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                //Error
                Console.Write("Failed to deploy config with exception: {0}", ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
