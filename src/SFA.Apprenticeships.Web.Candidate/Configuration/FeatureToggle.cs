namespace SFA.Apprenticeships.Web.Candidate.Configuration
{
    using System.Collections.Generic;
    using Domain.Interfaces.Configuration;

    public class FeatureToggle : IFeatureToggle
    {
        private readonly IConfigurationService _configurationService;

        public FeatureToggle(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        private readonly Dictionary<Feature, string> _featureToggleKeys = new Dictionary<Feature, string>
        {
            {Feature.SavedSearches, "SavedSearchesEnabled"},
            {Feature.Sms, "SmsEnabled"}
        };

        public bool IsActive(Feature feature)
        {
            if (!_featureToggleKeys.ContainsKey(feature))
            {
                throw new KeyNotFoundException(string.Format("{0} was not found in feature toggle dictionary.",
                    feature));
            }

            return _configurationService.GetCloudAppSetting<bool>(_featureToggleKeys[feature]);
        }
    }
}