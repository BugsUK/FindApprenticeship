namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.Shared
{
    using System.Collections.Generic;
    using System.Linq;
    using HtmlAgilityPack;

    public class ValidationSummaryDocument
    {
        private readonly HtmlDocument _document;

        public ValidationSummaryDocument(HtmlDocument document)
        {
            _document = document;
        }

        public HtmlNode ErrorsNode => _document.GetElementbyId("validation-summary-errors");

        public HtmlNode WarningsNode => _document.GetElementbyId("validation-summary-warnings");

        public string ErrorsClass => ErrorsNode.Attributes["class"].Value;

        public string WarningsClass => WarningsNode.Attributes["class"].Value;

        public List<HtmlNode> Errors => ErrorsNode.ChildNodes.FindFirst("div").ChildNodes.FindFirst("ul").ChildNodes.Where(n => n.Name == "li").ToList();

        public List<HtmlNode> Warnings => WarningsNode.ChildNodes.FindFirst("div").ChildNodes.FindFirst("ul").ChildNodes.Where(n => n.Name == "li").ToList();
    }
}