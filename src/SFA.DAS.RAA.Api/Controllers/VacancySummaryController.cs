namespace SFA.DAS.RAA.Api.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Repositories;
    using Services;
    using Swashbuckle.Swagger.Annotations;

    public class VacancySummaryController : ApiController
    {
        private readonly IRaaApiUserRepository _raaApiUserRepository;
        private readonly IAuthenticationService _authenticationService;

        public VacancySummaryController(IRaaApiUserRepository raaApiUserRepository, IAuthenticationService authenticationService)
        {
            _raaApiUserRepository = raaApiUserRepository;
            _authenticationService = authenticationService;
        }

        [SwaggerOperation("GetAll")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}