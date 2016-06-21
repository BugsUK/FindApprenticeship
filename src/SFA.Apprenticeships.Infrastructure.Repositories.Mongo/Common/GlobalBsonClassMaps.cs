namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Common
{
    using System.Linq;
    using Domain.Entities.Locations;
    using MongoDB.Bson.Serialization;

    internal static class GlobalBsonClassMaps
    {
        private static bool _mapped = false;
        private static readonly object _lock = new object();

        public static void Register()
        {
            if (!_mapped)
            {
                lock (_lock)
                {
                    if (!_mapped)
                    {
                        BsonClassMap.RegisterClassMap<Address>(classMap =>
                        {

                            classMap.AutoMap();
                            classMap.UnmapProperty(each => each.County);
                            classMap.UnmapProperty(each => each.Town);
                            
                            _mapped = true;

                        });
                    }
                }
            }
        }
    }
}