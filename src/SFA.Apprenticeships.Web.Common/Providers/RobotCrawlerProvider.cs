namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Web;
    using SFA.Infrastructure.Interfaces;
    using Newtonsoft.Json;

    public class RobotCrawlerProvider : IRobotCrawlerProvider
    {
        private readonly ILogService _logService;
        public const string CrawlerAgentList = "https://raw.githubusercontent.com/monperrus/crawler-user-agents/master/crawler-user-agents.json";

        private readonly CrawlerAgent[] _robotAgents;

        public RobotCrawlerProvider(ILogService logService)
        {
            _logService = logService;
            try
            {
                var webClient = new WebClient();
                var agentsJson = webClient.DownloadString(CrawlerAgentList);
                _robotAgents = JsonConvert.DeserializeObject<CrawlerAgent[]>(agentsJson);
            }
            catch (Exception ex)
            {
                _logService.Error(ex);
            }
        }

        public bool IsRobot(HttpContextBase httpContext)
        {
            if (_robotAgents == null || httpContext == null || httpContext.Request == null || httpContext.Request.UserAgent == null)
            {
                return false;
            }

            foreach (CrawlerAgent crawlerAgent in _robotAgents)
            {
                if (Regex.IsMatch(httpContext.Request.UserAgent, crawlerAgent.Pattern, RegexOptions.IgnoreCase))
                {
                    _logService.Info("Bot Detected using User Agent: {0}, tested against pattern: {1}", httpContext.Request.UserAgent, crawlerAgent.Pattern);
                    return true;
                }
            }

            return false;
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class CrawlerAgent
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Pattern { get; set; }
        }
    }
}