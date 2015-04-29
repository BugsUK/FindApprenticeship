namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipApplication
{
    using Candidate.ViewModels.Applications;
    using Candidate.Views.ApprenticeshipApplication;
    using HtmlAgilityPack;
    using RazorGenerator.Testing;

    public class WhatHappensNextViewBuilder : ViewBuilderBase
    {
        private WhatHappensNextApprenticeshipViewModel _viewModel;
        private WhatHappensNext _view;

        public WhatHappensNextViewBuilder With(WhatHappensNextApprenticeshipViewModel viewModel)
        {
            _viewModel = viewModel;
            return this;
        }

        public WhatHappensNext Build()
        {
            var view = new WhatHappensNext { ViewData = { Model = _viewModel } };
            return view;
        }

        public HtmlDocument RenderAsHtml()
        {
            if (_view == null)
            {
                _view = Build();
            }
            var result = _view.RenderAsHtml(_viewModel);
            return result;
        } 
    }
}