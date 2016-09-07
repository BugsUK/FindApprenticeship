namespace SFA.Apprenticeships.Domain.Entities.Feature
{
    using System.Linq;

    public static class FeatureConfigurationExtensions
    {
        public static bool IsSubcontractorsFeatureEnabled(this FeatureConfiguration configuration)
        {
            if (configuration == null) return false;
            var feature = configuration.Features.SingleOrDefault(f => f.Name == "Subcontractors");
            return feature != null && feature.Enabled;
        }
    }
}