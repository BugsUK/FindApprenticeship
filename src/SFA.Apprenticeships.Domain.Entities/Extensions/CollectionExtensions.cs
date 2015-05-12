namespace SFA.Apprenticeships.Domain.Entities.Extensions
{
    using System;

    //todo: move to web common
    //Can't move it to web common because it is used (indirectly) in both Web and Infrastructure.Communication. Needs a target project a little lower down the stack
    public static class CollectionExtensions
    {
        public static string ToQueryString(this string[] collection, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
            {
                throw new ArgumentNullException("parameterName");
            }

            if (collection == null || collection.Length == 0) return string.Empty;

            var queryStringSeparator = string.Format("&{0}=", parameterName);

            var queryString = string.Join(queryStringSeparator, collection);

            return queryStringSeparator + queryString;
        }
    }
}