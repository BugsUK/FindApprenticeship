namespace SFA.Apprenticeships.Web.Common.SiteMap
{
    using System;

    public interface ISiteMapItem
    {
        string Url { get; }
        DateTime? LastModified { get; }
        SiteMapChangeFrequency? ChangeFrequency { get; }
        double? Priority { get; }
    }
}
