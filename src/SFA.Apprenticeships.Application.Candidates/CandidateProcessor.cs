namespace SFA.Apprenticeships.Application.Candidates
{
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Interfaces.Logging;

    public class CandidateProcessor : ICandidateProcessor
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILogService _logService;

        public CandidateProcessor(IConfigurationService configurationService, ILogService logService)
        {
            _configurationService = configurationService;
            _logService = logService;
        }

        public void QueueCandidates()
        {
            var configuration = _configurationService.Get<HousekeepingConfiguration>();

            //TODO: Check should be INCLUSIVE
            throw new System.NotImplementedException();
        }
    }
}