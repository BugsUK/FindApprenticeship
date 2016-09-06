namespace SFA.Apprenticeships.Domain.Entities.Feature
{
    using System.Collections.Generic;

    public class FeatureConfiguration
    {
        public IReadOnlyList<Feature> Features { get; set; }
    }
}