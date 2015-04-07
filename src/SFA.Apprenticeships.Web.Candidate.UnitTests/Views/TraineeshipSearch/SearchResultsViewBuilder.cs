namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.TraineeshipSearch
{
    using System.Web.Routing;
    using Candidate.ViewModels.VacancySearch;
    using Candidate.Views.TraineeshipSearch;
    using HtmlAgilityPack;
    using RazorGenerator.Testing;

    public class SearchResultsViewBuilder
    {
        private TraineeshipSearchResponseViewModel _viewModel = new TraineeshipSearchResponseViewModel();

        public SearchResultsViewBuilder()
        {
            RouteTable.Routes.Clear();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        public SearchResultsViewBuilder With(TraineeshipSearchResponseViewModel viewModel)
        {
            _viewModel = viewModel;
            return this;
        }

        public searchResults Build()
        {
            return new searchResults
            {
                ViewData =
                {
                    Model = _viewModel

                }
            };
        }

        public HtmlDocument Render()
        {
            var view = Build();
            var result = view.RenderAsHtml(_viewModel);

            return result;
        }
    }
}