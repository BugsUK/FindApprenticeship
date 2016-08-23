namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class ExcludeLiveClosingDateResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);

            properties = properties.Where(p => p.PropertyName != "LiveClosingDate").ToList();

            return properties;
        }
    }
}