namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.ViewModels.Candidate;

    public class TrainingHistoryViewModelBuilder
    {
        private string _provider;
        private string _courseTitle;
        private string _description;
        private int _fromMonth;
        private string _fromYear;
        private int _toMonth;
        private string _toYear;

        public TrainingHistoryViewModelBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public TrainingHistoryViewModelBuilder WithProvider(string provider)
        {
            _provider = provider;
            return this;
        }

        public TrainingHistoryViewModelBuilder WithCourseTitle(string courseTitle)
        {
            _courseTitle = courseTitle;
            return this;
        }

        public TrainingHistoryViewModelBuilder WithFromMonth(int fromMonth)
        {
            _fromMonth = fromMonth;
            return this;
        }

        public TrainingHistoryViewModelBuilder WithFromYear(string fromYear)
        {
            _fromYear = fromYear;
            return this;
        }

        public TrainingHistoryViewModelBuilder WithToMonth(int toMonth)
        {
            _toMonth = toMonth;
            return this;
        }

        public TrainingHistoryViewModelBuilder WithToYear(string toYear)
        {
            _toYear = toYear;
            return this;
        }

        public TrainingHistoryViewModel Build()
        {
            return new TrainingHistoryViewModel
            {
                Provider = _provider,
                CourseTitle = _courseTitle,
                FromMonth = _fromMonth,
                FromYear = _fromYear,
                ToMonth = _toMonth,
                ToYear = _toYear
            };
        }
    }
}