namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Dashboard
{
    using Candidate.Views.Account;
    using Common.ViewModels.MyApplications;
    using HtmlAgilityPack;
    using RazorGenerator.Testing;

    public class IndexViewBuilder : ViewBuilderBase
    {
        private MyApplicationsViewModel _viewModel;

        public IndexViewBuilder With(MyApplicationsViewModel viewModel)
        {
            _viewModel = viewModel;
            return this;
        }

        public Index Build()
        {
            var view = new Index { ViewData = { Model = _viewModel } };
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