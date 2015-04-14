namespace SFA.Apprenticeships.Metrics.Candidate.Extensions
{
    using System;
    using MongoDB.Bson;

    public static class BsonDocumentExtensions
    {
        public static DateTime? ToNullableUniversalTime(this BsonDocument document, string propertyName)
        {
            if (document.Contains(propertyName))
            {
                return document[propertyName].ToNullableUniversalTime();
            }

            return null;
        }
    }
}