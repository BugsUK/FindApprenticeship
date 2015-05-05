namespace SFA.Apprenticeships.Web.Common.SiteMap
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using CuttingEdge.Conditions;

    public class SiteMapGenerator : ISiteMapGenerator
    {
        private static readonly XNamespace Xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private static readonly XNamespace Xsi = "http://www.w3.org/2001/XMLSchema-instance";

        public XDocument GenerateXml(IEnumerable<ISiteMapItem> siteMapItems)
        {
            // ReSharper disable PossibleMultipleEnumeration
            Condition.Requires(siteMapItems, "items").IsNotNull();

            var siteMap = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(Xmlns + "urlset",
                    new XAttribute("xmlns", Xmlns),
                    new XAttribute(XNamespace.Xmlns + "xsi", Xsi),
                    new XAttribute(Xsi + "schemaLocation",
                        "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd"),
                    from item in siteMapItems
                    select CreateItemElement(item))
                );

            // ReSharper restore PossibleMultipleEnumeration
            return siteMap;
        }

        #region Helper

        private static XElement CreateItemElement(ISiteMapItem siteMapItem)
        {
            var element = new XElement(Xmlns + "url", new XElement(Xmlns + "loc", siteMapItem.Url.ToLowerInvariant()));

            if (siteMapItem.LastModified.HasValue)
            {
                element.Add(new XElement(Xmlns + "lastmod", siteMapItem.LastModified.Value.ToString("o")));
            }

            if (siteMapItem.ChangeFrequency.HasValue)
            {
                element.Add(new XElement(Xmlns + "changefreq", siteMapItem.ChangeFrequency.Value.ToString().ToLower()));
            }

            if (siteMapItem.Priority.HasValue)
            {
                element.Add(new XElement(Xmlns + "priority",
                    siteMapItem.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));
            }

            return element;
        }

        #endregion
    }
}
