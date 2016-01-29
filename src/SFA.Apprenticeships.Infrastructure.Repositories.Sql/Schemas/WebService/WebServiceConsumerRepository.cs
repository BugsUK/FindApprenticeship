namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.WebService
{
    using System;
    using System.Linq;
    using Common;
    using Domain.Entities.WebServices;
    using Domain.Interfaces.Repositories.SFA.Apprenticeship.Api.AvService.Repositories;
    using SFA.Infrastructure.Interfaces;

    public class WebServiceConsumerRepository : IWebServiceConsumerReadRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public WebServiceConsumerRepository(IGetOpenConnection getOpenConnection, IMapper mapper, ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _logger = logger;
        }

        public WebServiceConsumer Get(Guid externalSystemId)
        {
            _logger.Debug($"Calling database to get web service consumer: {externalSystemId}");

            const string sql = "SELECT * FROM WebService.WebServiceConsumer WHERE ExternalSystemId = @externalSystemId";

            var parameters = new
            {
                externalSystemId
            };

            var webServiceConsumer = _getOpenConnection
                .Query<Entities.WebServiceConsumer>(sql, parameters)
                .FirstOrDefault();

            _logger.Debug(webServiceConsumer == null
                ? $"Failed to find web service consumer: {0}"
                : $"Found web service consumer: {0}", externalSystemId);

            return _mapper.Map<Entities.WebServiceConsumer, WebServiceConsumer>(webServiceConsumer);
        }
    }
}
