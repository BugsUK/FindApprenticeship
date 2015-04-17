namespace SFA.Apprenticeships.Web.Common.SiteMap
{
    using System;
    using CuttingEdge.Conditions;

    public class SiteMapItem : ISiteMapItem
    {
        public SiteMapItem(string url, DateTime? lastModified = null, SiteMapChangeFrequency? changeFrequency = null, double? priority = null)
        {
            Condition.Requires(url, "url").IsNotNullOrWhiteSpace();

            Url = url;
            LastModified = lastModified;
            ChangeFrequency = changeFrequency;
            Priority = priority;
        }

        public string Url { get; private set; }

        public DateTime? LastModified { get; private set; }

        public SiteMapChangeFrequency? ChangeFrequency { get; private set; }

        // TODO: US438: consider deriving priority based on how recently vacancy was added.
        public double? Priority { get; private set; }
    }
}
