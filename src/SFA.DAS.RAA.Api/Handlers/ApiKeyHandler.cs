namespace SFA.DAS.RAA.Api.Handlers
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using Extensions;
    using Services;

    public class ApiKeyHandler : DelegatingHandler
    {
        private readonly IAuthenticationService _authenticationService;

        public ApiKeyHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Set the current principal based on authentication response
            var principal = _authenticationService.Authenticate(request.GetQueryStrings());
            Thread.CurrentPrincipal = principal;
            HttpContext.Current.User = principal;

            //Allow the request to process further down the pipeline
            var response = await base.SendAsync(request, cancellationToken);

            //Return the response back up the chain
            return response;
        }
    }
}