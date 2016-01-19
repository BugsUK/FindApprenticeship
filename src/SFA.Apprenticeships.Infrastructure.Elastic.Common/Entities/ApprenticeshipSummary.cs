namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities
{
    using System;
    using Nest;

    [ElasticType(Name = "apprenticeship")]
    public class ApprenticeshipSummary : IApprenticeshipVacancySummary
    {
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public int Id { get; set; }

        [ElasticProperty(Index = FieldIndexOption.Analyzed, Analyzer = "snowballStopwordsBase")]
        public string Title { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public DateTime StartDate { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public DateTime ClosingDate { get; set; }

        [ElasticProperty(Index = FieldIndexOption.Analyzed)]
        public DateTime PostedDate { get; set; }

        [ElasticProperty(Index = FieldIndexOption.Analyzed, Analyzer = "stopwordsBase")]
        public string EmployerName { get; set; }

        [ElasticProperty(Index = FieldIndexOption.Analyzed, Analyzer = "stopwordsBase")]
        public string ProviderName { get; set; }

        [ElasticProperty(Index = FieldIndexOption.Analyzed, Analyzer = "snowballStopwordsExtended")]
        public string Description { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public int NumberOfPositions { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public bool IsPositiveAboutDisability { get; set; }

        [ElasticProperty(Type = FieldType.String, Index = FieldIndexOption.Analyzed)]
        public VacancyLocationType VacancyLocationType { get; set; }

        [ElasticProperty(Type = FieldType.GeoPoint, Index = FieldIndexOption.Analyzed)]
        public GeoPoint Location { get; set; }
        
        [ElasticProperty(Type = FieldType.String, Index = FieldIndexOption.Analyzed)]
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string VacancyReference { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string Category { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string CategoryCode { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string SubCategory { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string SubCategoryCode { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string Wage { get; set; }

        [ElasticProperty(Type = FieldType.String, Index = FieldIndexOption.NotAnalyzed)]
        public WageUnit WageUnit { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string WorkingWeek { get; set; }
    }
}