namespace SFA.Apprenticeships.Web.Common.SiteMap
{
    using System.Xml.Linq;
    using System.Collections.Generic;

    public interface ISiteMapGenerator
    {
        XDocument GenerateXml(IEnumerable<ISiteMapItem> siteMapItems);
    }
}
