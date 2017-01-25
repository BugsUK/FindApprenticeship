﻿namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages
{
    using System.Linq;
    using global::SpecBind.Pages;
    using global::SpecBind.Selenium;
    using OpenQA.Selenium;

    public class BaseValidationPage : BasePage
    {
        public BaseValidationPage(ISearchContext context) : base(context)
        {
        }

        [ElementLocator(Class = "error-summary")]
        public IWebElement ValidationSummary { get; set; }

        [ElementLocator(Class = "error-summary-list")]
        public IElementList<IWebElement, ValidationSummaryItem> ValidationSummaryItems { get; set; }

        [ElementLocator(Class = "error-summary-list")]
        public IElementList<IWebElement, ValidationSummaryNoLinkItem> ValidationSummaryNoLinkItems { get; set; }

        [ElementLocator(TagName = "form")]
        public IElementList<IWebElement, ValidationErrorFieldItem> ValidationFieldErrorItems { get; set; }

        public string ValidationSummaryCount
        {
            get { return ValidationSummaryItems.Count().ToString(); }
        }

        public string ValidationSummaryNoLinkCount
        {
            get { return ValidationSummaryNoLinkItems.Count().ToString(); }
        }

        public string ValidationFieldErrorCount
        {
            get { return ValidationFieldErrorItems.Count().ToString(); }
        }

        [ElementLocator(TagName = "a")]
        public class ValidationSummaryItem : WebElement
        {
            public ValidationSummaryItem(ISearchContext parent)
                : base(parent)
            {
            }

            public string Href
            {
                get { return GetAttribute("href").Substring(GetAttribute("href").IndexOf("#")); }
            }
        }

        [ElementLocator(TagName = "li")]
        public class ValidationSummaryNoLinkItem : WebElement
        {
            public ValidationSummaryNoLinkItem(ISearchContext parent)
                : base(parent)
            {
            }
        }

        [ElementLocator(Class = "field-validation-error")]
        public class ValidationErrorFieldItem : WebElement
        {
            public ValidationErrorFieldItem(ISearchContext parent)
                :base(parent)
            {
            }
        }
    }
}