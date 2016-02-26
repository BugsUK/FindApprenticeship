namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Common.ViewModels.Candidate;

    public class TrainingCourseViewModelBuilder
    {
        private string _provider;
        private string _title;
        private int _fromMonth;
        private string _fromYear;
        private int _toMonth;
        private string _toYear;

        public TrainingCourseViewModelBuilder WithProvider(string provider)
        {
            _provider = provider;
            return this;
        }

        public TrainingCourseViewModelBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public TrainingCourseViewModelBuilder WithFromMonth(int fromMonth)
        {
            _fromMonth = fromMonth;
            return this;
        }

        public TrainingCourseViewModelBuilder WithFromYear(string fromYear)
        {
            _fromYear = fromYear;
            return this;
        }

        public TrainingCourseViewModelBuilder WithToMonth(int toMonth)
        {
            _toMonth = toMonth;
            return this;
        }

        public TrainingCourseViewModelBuilder WithToYear(string toYear)
        {
            _toYear = toYear;
            return this;
        }

        public TrainingCourseViewModel Build()
        {
            return new TrainingCourseViewModel
            {
                Provider = _provider,
                Title = _title,
                FromMonth = _fromMonth,
                FromYear = _fromYear,
                ToMonth = _toMonth,
                ToYear = _toYear
            };
        }
    }
}