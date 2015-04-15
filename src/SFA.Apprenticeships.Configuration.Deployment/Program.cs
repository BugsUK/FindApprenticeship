namespace SFA.Apprenticeships.Configuration.Deployment
{
    using System;
    using System.IO;
    using Infrastructure.Common.Configuration;
    using Infrastructure.Common.IoC;
    using Infrastructure.Logging;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using StructureMap;

    public class Program
    {
        static void Main()
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
                var fileVersion = VersionLogging.GetVersion();

                if (fileVersion == "1.0.0.0")
                {
                    // Developers use the memory cache, not Azure cache.
                    document["CacheConfiguration"]["DefaultCache"] = "MemoryCacheService";
                }

                document.InsertAt(0, new BsonElement("DateTimeUpdated", dateTimeUpdated));
                document.InsertAt(0, new BsonElement("DeploymentVersion", fileVersion));

                //Ensure there can only be one entry with this version (mostly for local)
                var query = Query.And(Query.EQ("DeploymentVersion", fileVersion), Query.NE("DateTimeUpdated", dateTimeUpdated));

                collection.Insert(document);
                collection.Remove(query);

                Console.WriteLine("Successfully deployed configuration");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to deploy config with exception: {0}", ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
