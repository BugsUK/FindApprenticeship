namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using System.Web.Routing;
    using Builders;
    using Candidate.ViewModels.VacancySearch;
    using Candidate.Views.ApprenticeshipSearch;
    using HtmlAgilityPack;
    using RazorGenerator.Testing;

    public class ResultsViewBuilder
    {
        private ApprenticeshipSearchResponseViewModel _viewModel;

        public ResultsViewBuilder()
        {
            _viewModel = new ApprenticeshipSearchResponseViewModelBuilder().Build();

            RouteTable.Routes.Clear();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        public ResultsViewBuilder With(ApprenticeshipSearchResponseViewModel viewModel)
        {
            _viewModel = viewModel;
            return this;
        }

        public Results Build()
        {
            var view = new Results {ViewData = {Model = _viewModel}};
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