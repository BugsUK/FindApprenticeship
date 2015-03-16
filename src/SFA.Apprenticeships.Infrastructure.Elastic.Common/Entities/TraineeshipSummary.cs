﻿namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities
{
    using System;
    using Nest;

    [ElasticType(Name = "traineeship")]
    public class TraineeshipSummary : IVacancySummary
    {
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public int Id { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string Title { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public DateTime StartDate { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public DateTime ClosingDate { get; set; }

        [ElasticProperty(Index = FieldIndexOption.Analyzed, Analyzer = "snowballStopwordsBase")]
        public string EmployerName { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string Description { get; set; }

        [ElasticProperty(Type = FieldType.GeoPoint, Index = FieldIndexOption.Analyzed)]
        public GeoPoint Location { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string VacancyReference { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string Sector { get; set; }

        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public string Framework { get; set; }
    }
}