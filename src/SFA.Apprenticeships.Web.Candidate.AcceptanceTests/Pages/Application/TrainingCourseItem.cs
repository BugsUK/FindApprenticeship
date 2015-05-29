namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Application
{
    using OpenQA.Selenium;
    using SpecBind.Pages;
    using SpecBind.Selenium;

    [ElementLocator(Class = "training-history-item")]
    public class TrainingCourseItem : WebElement
    {
        public TrainingCourseItem(ISearchContext parent)
            : base(parent)
        {
        }

        [ElementLocator(Class = "training-history-provider-span")]
        public IWebElement Provider { get; set; }

        [ElementLocator(Class = "training-history-course-title-span")]
        public IWebElement CourseTitle { get; set; }

        [ElementLocator(Class = "remove-training-history-item-link")]
        public IWebElement RemoveTrainingCourseLink { get; set; }
    }
}