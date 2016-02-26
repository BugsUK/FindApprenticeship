namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Dashboard
{
    using Common.ViewModels.MyApplications;
    using Common.Views.Shared.DisplayTemplates;
    using HtmlAgilityPack;
    using RazorGenerator.Testing;

    public class MyApplicationsViewBuilder : ViewBuilderBase
    {
        private MyApplicationsViewModel _viewModel;

        public MyApplicationsViewBuilder With(MyApplicationsViewModel viewModel)
        {
            _viewModel = viewModel;
            return this;
        }

        public MyApplications_ Build()
        {
            var view = new MyApplications_ { ViewData = { Model = _viewModel } };
            return view;
        }

        public HtmlDocument Render()
        {
            var view = Build();
            var result = view.RenderAsHtml(_viewModel);
            return result;
        }
    }
}