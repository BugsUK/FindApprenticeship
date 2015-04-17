namespace SFA.Apprenticeships.Web.Common.SiteMap
{
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;
    using System.Xml;

    public class SiteMapResult : ActionResult
    {
        private readonly IEnumerable<ISiteMapItem> _siteMapItems;

        public SiteMapResult(IEnumerable<ISiteMapItem> siteMapItems)
        {
            _siteMapItems = siteMapItems;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            response.ContentType = "text/xml";
            response.ContentEncoding = Encoding.UTF8;

            var generator = new SiteMapGenerator();
            var xml = generator.GenerateXml(_siteMapItems);

            using (var writer = new XmlTextWriter(response.Output))
            {
                writer.Formatting = Formatting.Indented;
                xml.WriteTo(writer);
            }
        }
    }
}
